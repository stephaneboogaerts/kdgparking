using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kdgparking.DAL;
using kdgparking.BL.Domain;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;
using OfficeOpenXml;
using System.IO;
using Syroot.Windows.IO;


namespace kdgparking.BL
{
    public class CSVManager : ICSVManager
    {
        private readonly IHolderRepository repo;

        public CSVManager()
        {
            repo = new kdgparking.DAL.HolderRepository();
        }

        // Lijst van InputHolders omzetten naar respectievelijke klasse en wegschrijven naar DB
        //  Methode geeft een Lijst terug van InputHolders voor overzicht gecommitte records
        public List<InputHolder> ProcessInputholderList(List<InputHolder> inputHolderList)
        {
            HolderManager HoldMng = new HolderManager();
            ContractManager ContMng = new ContractManager();
            List<InputHolder> iHolderList = new List<InputHolder>();
            Vehicle vehicle;
            Contract contract;
            Company company;
            List<Vehicle> vehicles;
            InputHolder iHolder;
            foreach (InputHolder inputHolder in inputHolderList)
            {
                iHolder = new InputHolder();

                // TODO : Badge extractie


                // Holder extractie
                // Kijken of een Holder alreeds bestaat in de DB
                // Wanneer de Holder niet gevonden word, wordt er een nieuwe Holder aangemaakt
                Holder holder = HoldMng.GetHolder(inputHolder.PNumber);
                if (holder == null)
                {
                    holder = HoldMng.AddHolder(inputHolder.Name, inputHolder.FirstName, inputHolder.PNumber, inputHolder.Email, inputHolder.Telefoon,
                        inputHolder.GSM, inputHolder.Stad, inputHolder.Straat, inputHolder.Post, inputHolder.Company);

                    //Contract extractie voor nieuwe Holder
                    contract = new Contract()
                    {
                        StartDate = inputHolder.StartDate,
                        EndDate = inputHolder.EndDate,
                        Holder = holder,
                        Vehicles = new List<Vehicle>()
                    };
                    contract = ContMng.AddContract(contract);
                }
                else
                {
                    // Holder bestaat : Update termijn
                    contract = holder.Contract;
                    contract.StartDate = inputHolder.StartDate;
                    contract.EndDate = inputHolder.EndDate;
                    contract = ContMng.ChangeContract(contract);
                }

                // Vehicle extractie
                // Kijken of Vehicle (adhv nummerplaat) al bestaat in DB
                vehicle = ContMng.GetVehicle(inputHolder.NumberPlate);
                if (vehicle == null)
                {
                    vehicle = ContMng.AddVehicle(inputHolder.VoertuigNaam, inputHolder.NumberPlate);
                }

                vehicles = contract.Vehicles;
                // Kijken of het contract al eenzelfde Vehicle bevat
                //  : indien wel geen verdere actie nodig
                if (!vehicles.Contains(vehicle))
                {
                    vehicles.Add(vehicle);
                    contract.Vehicles = vehicles;
                    ContMng.ChangeContract(contract);
                }

                // Deze values worden teruggegeven aan de Controller voor laatste overzicht van commit
                iHolder.Name = holder.Name;
                iHolder.FirstName = holder.FirstName;
                iHolder.NumberPlate = vehicle.Numberplate;
                iHolder.StartDate = contract.StartDate;
                iHolder.EndDate = contract.EndDate;
                iHolder.Company = holder.Company.CompanyName;

                iHolderList.Add(iHolder);
            }
            return iHolderList;
        }

        // Er komt een excel file binnen vanuit een view
        //  Note : Headers worden niet dynamisch uitgelezen
        public List<InputHolder> ProcessFile(HttpPostedFileBase file)
        {
            try
            {
                // HttpPostedFileBase file uit view als excel lezen 
                using (ExcelPackage package = new ExcelPackage(file.InputStream))
                {
                    // Excel file wordt omgezet naar een array van strings
                    // Elke row wordt een string, values worden opgeslitst adhv een tab (\t)
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    for (int row = 5; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            // Kijkt of column een waarde bevat alvorens het toe te voegen
                            // Wanneer value null is wordt er een default value toegewezen
                            if ((worksheet.Cells[row, col].Value) != null)
                            {
                                var val = worksheet.Cells[row, col].Value.ToString() + "\t";
                                sb.Append(val);
                            }
                            else if (col == 11) // Wanneer nummerplaat (col 11) null zou zijn
                            {
                                sb.Append("null\t");
                            }
                            // Note : Exact 1 Tarif decimal moet meegegeven worden 
                            //  >>> Opvangen of vermelden in documentatie
                            //else if (col == 8 || col == 9 || col == 10) { } // Tarieven (col 8, 9 &10)
                            else if (col == 16) // Einddatum (col 16) = 2099 wanneer null
                            {
                                sb.Append("73000\t");
                            }
                            else if (col == 22 || col == 23) // Tel/ GSM (col 22 &23) = "null" wanneer null
                            {
                                sb.Append("null\t");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    // Data in text formaat doorsturen naar ProcessFileData() om text om te zetten naar list van objecten
                    List<InputHolder> inputHolderList = new List<InputHolder>();
                    return inputHolderList = ProcessFileData(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some error occured while importing. " + ex.Message);
            }
        }

        // We schrijven data naar een tijdelijk model klasse (InputHolder) om eerst in de controller de input te valideren
        // Daarna zal de data naar hun respectievelijke klasse worden omgezet en naar de DB worden weggeschreven
        private List<InputHolder> ProcessFileData(string fileData)
        {
            try
            {
                using (StringReader reader = new StringReader(fileData))
                {
                    InputHolder inputHolder;
                    List<InputHolder> ihList = new List<InputHolder>();
                    string line;
                    // Elke row uitlezen
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Elke column uit een row opsplitsen (\t = tab)
                        string[] para = line.Split('\t');
                        // Lege rows negeren >= 18 (voorbeeld excel bevat lege rows)
                        if (para.Length >= 18)
                        {
                            try
                            {
                                // Voor- en achternaam uit fullname halen
                                string[] fullname = para[5].Split(' ');
                                string fName = "";
                                string lName = "";
                                for (int i = 0; i < fullname.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        fName = fullname[i];
                                    }
                                    else
                                    {
                                        lName += fullname[i];
                                    }
                                }
                                // Hier komt logica : data naar object
                                inputHolder = new InputHolder()
                                {
                                    Badge = Int32.Parse(para[2]),
                                    PNumber = para[3],
                                    ContractId = para[4],
                                    FirstName = fName,
                                    Name = lName,
                                    VoertuigNaam = para[6],
                                    NumberPlate = para[7],
                                    Tarief = decimal.Parse(para[8]),
                                    StartDate = DateTime.FromOADate(Int32.Parse(para[9])), // <-- Serialdate(Excel) wordt omgezet naar DateTime.
                                    EndDate = DateTime.FromOADate(Int32.Parse(para[10])),
                                    Waarborg = decimal.Parse(para[11]),
                                    WaarborgBadge = decimal.Parse(para[12]),
                                    Straat = para[13],
                                    Post = para[14],
                                    Stad = para[15],
                                    Telefoon = para[16],
                                    GSM = para[17],
                                    Email = para[18],
                                    Company = "BuurtParking" // <-- Company juist zetten
                                };
                                // Validatie excel input voor toe te voegen aan list
                                this.Validate(inputHolder);
                                ihList.Add(inputHolder);
                            }
                            catch (Exception ex)
                            {
                                throw new ArgumentException("Some error occured while converting excel data to object. "
                                    + ex.Message + " Line with invalid data = " + line);

                            }
                        }
                    }
                    return ihList;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Some error occured while converting excel data to object. " + ex.Message);
            }
        }

        public string CsvExport(IEnumerable<Vehicle> vehicles)
        {
            // Syntax : 1GXXXX2,Naam,Voornaam
            string downloadsPath = new KnownFolder(KnownFolderType.Downloads).Path;
            string fileIdentifier = "CsvExport_" + DateTime.Now.ToString("yyyyddMM-HHmmss") + ".csv";
            downloadsPath = Path.Combine(downloadsPath, fileIdentifier);
            using (var w = new StreamWriter(downloadsPath))
            {
                foreach (Vehicle v in vehicles)
                {
                    if (!String.IsNullOrEmpty(v.Contract.Holder.Name) && !String.IsNullOrEmpty(v.Contract.Holder.FirstName))
                    {
                        var nrPlate = v.Numberplate.ToUpper();
                        var name = v.Contract.Holder.Name;
                        var firstname = v.Contract.Holder.FirstName;
                        var line = string.Format("{0},{1},{2}", nrPlate, name, firstname);
                        w.WriteLine(line);
                        w.Flush();
                    }
                }
            }
            return downloadsPath.ToString();
        }

        public void Validate(InputHolder inputHolder)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(inputHolder, new ValidationContext(inputHolder), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException("InputHolder " + inputHolder.Name + " not valid!");
        }

    }
}

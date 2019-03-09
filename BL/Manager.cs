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
    public class Manager : IManager
    {
        private readonly IRepository repo;

        public Manager()
        {
            repo = new kdgparking.DAL.Repository();
            //this.SeedTestData(); // <-- telkens wanneer Manager wordt geïnitialisseerd
        }

        private void SeedTestData()
        {
            Holder testDummy = new Holder()
            {
                Name = "McTesterson",
                FirstName = "Test",
                Phone = "0123456789",
                GSM = "9874563210",
                Email = "mctesterson@testers.tst",
                HolderNumber = "P0110"
            };
            this.AddHolder(testDummy);
        }

        //REFACTORING & DOCUMENTATIE NODIG
        public Holder ChangeHolder(int id, InputHolder updatedHolder)
        {
            Holder ChangedHolder = this.GetHolder(id);
            ChangedHolder.Name = updatedHolder.Name;
            ChangedHolder.FirstName = updatedHolder.FirstName;
            ChangedHolder.Email = updatedHolder.Email;
            ChangedHolder.Contract.StartDate = updatedHolder.StartDate;
            ChangedHolder.Contract.EndDate = updatedHolder.EndDate;
            //Add new Company indien nodig
            if (!updatedHolder.Company.Equals(ChangedHolder.Company.CompanyName))
            {
                Company HolderCompany = this.GetCompany(updatedHolder.Company);
                if (HolderCompany == null)
                {
                    HolderCompany = this.AddCompany(new Company() { CompanyName = updatedHolder.Company });
                }
                ChangedHolder.Company = HolderCompany;
            }
            repo.UpdateHolder(ChangedHolder);
            repo.UpdateContract(ChangedHolder.Contract);
            return ChangedHolder;
        }

        public Holder AddHolder(string name)
        {
            Holder h = new Holder()
            {
                Name = name
            };
            return this.AddHolder(h);
        }

        public Holder AddHolder(string name, string firstName, string phone, string email)
        {
            Holder h = new Holder()
            {
                Name = name,
                FirstName = firstName, // <-- te verplaatsen naar overload functie (als organisatie geen aparte klasse wordt)
                Phone = phone,
                Email = email
            };
            return this.AddHolder(h);
        }

        public Holder AddNewHolder(InputHolder inputHolder)
        {
            Company HolderCompany = this.GetCompany(inputHolder.Company);
            if (HolderCompany == null)
            {
                HolderCompany = this.AddCompany(new Company() { CompanyName = inputHolder.Company });
            }
            Holder CreatedHolder = new Holder()
            {
                Name = inputHolder.Name,
                FirstName = inputHolder.FirstName,
                Email = inputHolder.Email,
                Company = HolderCompany,
                //CompanyId = HolderCompany.CompanyId
            };

            Contract NewContract = new Contract()
            {
                StartDate = inputHolder.StartDate,
                EndDate = inputHolder.EndDate,
                Holder = CreatedHolder
            };

            CreatedHolder = this.AddHolder(CreatedHolder);
            CreatedHolder.Contract = NewContract;
            //NewContract.HolderId = CreatedHolder.Id;
            this.AddContract(NewContract);
            repo.UpdateHolder(CreatedHolder);
            return CreatedHolder;
        }

        public Holder GetHolder(int id)
        {
            return repo.ReadHolder(id);
        }

        public Holder GetHolder(string pNumber)
        {
            return repo.ReadHolder(pNumber);
        }

        public IEnumerable<Holder> GetHolders()
        {
            return repo.ReadHolders();
        }

        public IEnumerable<Holder> GetHolders(string searchString)
        {
            return repo.ReadHolders(searchString);
        }

        public IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles()
        {
            return repo.ReadHoldersWithContractsAndVehicles();
        }

        public IEnumerable<Holder> GetHoldersWithCompanyContractsAndVehicles(string company)
        {
            return repo.ReadHoldersWithContractsAndVehicles(company);
        }

        private Holder AddHolder(Holder holder)
        {
            // Validatie gebeurt in InputHolder
            return repo.CreateHolder(holder);
        }

        public Holder AddHolder(string name, string firstName, string holderNr, string email,
            string phone, string gsm, string city, string street, string post, string companyName)
        {
            // Kijken of een Company alreeds bestaat in de DB
            // Wanneer de Company niet gevonden word, wordt er een nieuwe Company aangemaakt
            Company company = GetCompany(companyName);
            if (company == null)
            {
                company = AddCompany(companyName);
            }

            Holder holder = new Holder()
            {
                Name = name,
                FirstName = firstName,
                HolderNumber = holderNr,
                Email = email,
                Phone = phone,
                GSM = gsm,
                City = city,
                Street = street,
                PostalCode = post,
                Company = company
            };
            return this.AddHolder(holder);
        }

        public void RemoveHolder(int id)
        {
            int ContractId = this.GetHolderContract(id).Id;
            repo.DeleteHolder(this.GetHolder(id));
            repo.DeleteContract(this.GetContract(ContractId));
            return;
        }

        private void Validate(InputHolder inputHolder)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(inputHolder, new ValidationContext(inputHolder), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException("InputHolder " + inputHolder.Name + " not valid!");
        }

        // Lijst van InputHolders omzetten naar respectievelijke klasse en wegschrijven naar DB
        //  Methode geeft een Lijst terug van InputHolders voor overzicht gecommitte records
        public List<InputHolder> ProcessInputholderList(List<InputHolder> inputHolderList)
        {
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
                Holder holder = GetHolder(inputHolder.PNumber);
                if (holder == null)
                {
                    holder = AddHolder(inputHolder.Name, inputHolder.FirstName, inputHolder.PNumber, inputHolder.Email, inputHolder.Telefoon,
                        inputHolder.GSM, inputHolder.Stad, inputHolder.Straat, inputHolder.Post, inputHolder.Company);

                    //Contract extractie voor nieuwe Holder
                    contract = new Contract()
                    {
                        StartDate = inputHolder.StartDate,
                        EndDate = inputHolder.EndDate,
                        Holder = holder,
                        Vehicles = new List<Vehicle>()
                    };
                    contract = this.AddContract(contract);
                }
                else
                {
                    // Holder bestaat : Update termijn
                    contract = holder.Contract;
                    contract.StartDate = inputHolder.StartDate;
                    contract.EndDate = inputHolder.EndDate;
                    contract = ChangeContract(contract);
                }

                // Vehicle extractie
                // Kijken of Vehicle (adhv nummerplaat) al bestaat in DB
                vehicle = GetVehicle(inputHolder.NumberPlate);
                if(vehicle == null)
                {
                    vehicle = this.AddVehicle(inputHolder.VoertuigNaam, inputHolder.NumberPlate);
                }

                vehicles = contract.Vehicles;
                // Kijken of het contract al eenzelfde Vehicle bevat
                //  : indien wel geen verdere actie nodig
                if (!vehicles.Contains(vehicle))
                {
                    vehicles.Add(vehicle);
                    contract.Vehicles = vehicles;
                    this.ChangeContract(contract);
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
                foreach( Vehicle v in vehicles)
                {
                    if(!String.IsNullOrEmpty(v.Contract.Holder.Name) && !String.IsNullOrEmpty(v.Contract.Holder.FirstName))
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
    }
}
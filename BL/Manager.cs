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

namespace kdgparking.BL
{
    public class Manager : IManager
    {
        private readonly IRepository repo;

        public Manager()
        {
            repo = new kdgparking.DAL.Repository();
            this.SeedTestData();
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
        public Holder UpdateHolder(int id, InputHolder updatedHolder)
        {
            throw new NotImplementedException();
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

        public Holder AddNewHolder(InputHolder newHolder)
        {
            Holder h = new Holder()
            {
                Name = newHolder.Name,
                FirstName = newHolder.FirstName
            };
            return this.AddHolder(h);
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

        private Holder AddHolder(Holder holder)
        {
            this.Validate(holder);
            return repo.CreateHolder(holder);
        }

        public List<Holder> ProcessInputholderList(List<InputHolder> inputHolderList)
        {
            List<Holder> holderList = new List<Holder>();
            foreach (InputHolder inputHolder in inputHolderList)
            {
                // Kijken of een Holder alreeds bestaat in de DB
                Holder holder = GetHolder(inputHolder.PNumber);
                // Wanneer Holder niet gevonden word, wordt er een nieuwe Holder aangemaakt
                if (holder == null)
                {
                    holder = new Holder()
                    {
                        Name = inputHolder.Name,
                        FirstName = inputHolder.FirstName,
                        HolderNumber = inputHolder.PNumber,
                        Email = inputHolder.Email,
                        Phone = inputHolder.Telefoon,
                        GSM = inputHolder.GSM
                    };
                    holder = this.AddHolder(holder);
                }
                holderList.Add(holder);

                // TODO : create contract, address, etc..
            }

            return holderList;
        }

        public Contract AddContract(int holderId, string numberplate, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge)
        {
            Holder holder = this.GetHolder(holderId);
            Vehicle vehicle = this.GetVehicle(numberplate);
            List<Vehicle> vehicles = new List<Vehicle>();
            vehicles.Add(vehicle);

            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end,
                Tarif = tarif,
                Warranty = warranty,
                WarrantyBadge = warranty,
                Vehicles = vehicles
            };

            return contract;
        }

        public Vehicle GetVehicle(string numberplate)
        {
            return repo.ReadVehicle(numberplate);
        }

        private void Validate(Holder holder)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(holder, new ValidationContext(holder), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException("Holder not valid!");
        }

        private void Validate(InputHolder inputHolder)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(inputHolder, new ValidationContext(inputHolder), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException("InputHolder " + inputHolder.PNumber + " not valid!");
        }

        public List<InputHolder> ProcessFile(HttpPostedFileBase file)
        {
            try
            {
                // file.InputStream reads HttpPostedFileBase as excel 
                using (ExcelPackage package = new ExcelPackage(file.InputStream))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns; //21;
                    for (int row = 5; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            // Kijkt of column een waarde bevat alvorens het toe te voegen
                            if ((worksheet.Cells[row, col].Value) != null)
                            {
                                var val = worksheet.Cells[row, col].Value.ToString() + "\t";
                                sb.Append(val);
                                //sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
                            }
                            else if (col == 11) // Wanneer nummerplaat null zou zijn
                            {
                                sb.Append("null\t");
                            }
                            // Moet hier onderscheid in gemaakt worden?
                            //else if (col == 8 || col == 9 || col == 10) // Tarieven = 0 wanneer null
                            //{
                            //    sb.Append("0" + "\t");
                            //}
                            else if (col == 16) // Einddatum = 0 wanneer null
                            {
                                sb.Append("0\t");
                            }
                            else if (col == 22 || col == 23) // Tel/GSM = "" wanneer null
                            {
                                sb.Append("null\t");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    // Data in text formaat doorsturen naar functie om text om te zetten naar list van objecten
                    List<InputHolder> inputHolderList = new List<InputHolder>();
                    return inputHolderList = ProcessFileData(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(("Some error occured while importing. " + ex.Message));
            }
            return new List<InputHolder>();
        }

        // We schrijven data naar een tijdelijk model klasse om eerst in de controller de input te valideren
        // Daarna zal de data naar hun respectievelijke klasse worden omgezet en naar de db worden weggeschreven
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
                        // Lege rows negeren (voorbeeld excel bevat lege rows)
                        if (para.Length >= 18)
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
                                EndDate = DateTime.FromOADate(Int32.Parse(para[10])), // <-- veld kan leeg zijn?
                                Waarborg = decimal.Parse(para[11]),
                                WaarborgBadge = decimal.Parse(para[12]),
                                Straat = para[13],
                                Post = para[14],
                                Stad = para[15],
                                Telefoon = para[16],
                                GSM = para[17],
                                Email = para[18],
                                Company = "BuurtParking"
                            };
                            // Validatie excel input voor toe te voegen aan list
                            //  TODO : Laten weten aan user wanneer een object invalid is
                            this.Validate(inputHolder);
                            ihList.Add(inputHolder);
                        }
                    }
                    return ihList;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(("Some error occured while importing. " + ex.Message));
            }
            return new List<InputHolder>();
        }
    }
}
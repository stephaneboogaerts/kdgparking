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
        }

        public Holder AddHolder(string id, string name, string firstName, int phone, string email)
        {
            Holder h = new Holder
            {
                HolderNumber = id,
                Name = name,
                FirstName = firstName, // <-- te verplaatsen naar overload functie (als organisatie geen aparte klasse wordt)
                Phone = phone,
                Email = email
            };
            return this.AddHolder(h);
        }

        public Holder GetHolder(string id)
        {
            return repo.ReadHolder(id);
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

        public Contract AddContract(string holderId, string numberplate, DateTime begin, DateTime end, decimal tarif, decimal warranty, decimal warrantyBadge)
        {
            Holder holder = this.GetHolder(holderId);
            Vehicle vehicle = this.GetVehicle(numberplate);

            Contract contract = new Contract
            {
                Holder = holder,
                StartDate = begin,
                EndDate = end,
                Tarif = tarif,
                Warranty = warranty,
                WarrantyBadge = warranty,
                Vehicle = vehicle
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
        
        public void ProcessFile(HttpPostedFileBase file)
        {
            try
            {
                // file.InputStream reads HttpPostedFileBase as excel 
                using (ExcelPackage package = new ExcelPackage(file.InputStream))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    for (int row = 5; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            // Kijkt of column een waarde bevat alvorens het toe te voegen
                            if((worksheet.Cells[row, col].Value) != null)
                            {
                                sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    //System.Diagnostics.Debug.WriteLine(sb.ToString());
                    ProcessFileData(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(("Some error occured while importing." + ex.Message));
                //return ("Some error occured while importing." + ex.Message).ToString();
            }
        }

        private void ProcessFileData(string fileData)
        {
            using (StringReader reader = new StringReader(fileData))
            {
                string line;
                // Elke row uitlezen
                while ((line = reader.ReadLine()) != null)
                {
                    // Elke column uit een row opsplitsen (\t = tab)
                    string[] para = line.Split('\t');
                    // Lege rows negeren
                    if (para.Length > 3)
                    {
                        for(int i = 0; i < para.Length; i++)
                        {
                            // Hier komt logica : data naar object
                            System.Diagnostics.Debug.WriteLine(para[i]);
                        }
                        System.Diagnostics.Debug.WriteLine(" ");
                    }
                }
            }
        }
    }
}

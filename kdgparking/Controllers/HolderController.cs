using kdgparking.BL;
using kdgparking.BL.Domain;
using kdgparking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace testParkingWeb.Controllers
{
    public class HolderController : Controller
    {
        private StringCleaner cleaner = new StringCleaner();
        private IHolderManager mng = new HolderManager();

        //User feedback is minimaal

        public ActionResult Index()
        {
            //Lijst pagina is de index
            return RedirectToAction("Lijst");
        }

        public ActionResult Lijst(string searchString)
        {
            IEnumerable<Holder> holders;
            //Wordt er gezocht of is het enkel een oplijsting?
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = cleaner.CleanString(searchString);
                holders = mng.GetHolders(searchString);
            }
            else
            {
                holders = mng.GetHolders();
            }
            List<InputHolder> iHolders = new List<InputHolder>();
            foreach (Holder h in holders)
            {
                //Formateer de holders naar InputHolders
                iHolders.Add(mng.ComposeInputHolder(h));
            }
            ViewData.Model = iHolders;
            return View();
        }

        public ActionResult Toevoegen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Toevoegen(InputHolder nieuweHolder)
        {
            if (ModelState.IsValid)
            {
                nieuweHolder = cleaner.CleanInputHolder(nieuweHolder);
                mng.AddNewHolder(nieuweHolder);
            }
            return View();
        }

        public ActionResult LijstVoertuigen(string searchString)
        {
            IHolderManager holdMgt = new HolderManager();
            // TODO : Testen of Holder &Vehicle Distinct zijn
            List<HolderVehicle> modelList = new List<HolderVehicle>();
            HolderVehicle model = new HolderVehicle();
            if (!String.IsNullOrEmpty(searchString))
            {
                IEnumerable<Vehicle> vehicles = holdMgt.GetVehicles(searchString);
                foreach (Vehicle v in vehicles)
                {
                    model = new HolderVehicle()
                    {
                        FirstName = v.Contract.Holder.FirstName,
                        Name = v.Contract.Holder.Name,
                        GSM = v.Contract.Holder.GSM,
                        Phone = v.Contract.Holder.Phone,
                        Email = v.Contract.Holder.Email,
                        VehicleName = v.VehicleName,
                        Numberplate = v.Numberplate
                    };
                    modelList.Add(model);
                }
            }
            return View(modelList);
        }

        public ActionResult LijstActive(string searchString)
        {
            ICompanyManager CompMng = new CompanyManager();
            // Initialisatie ContractViewModel : Bevat List van ContracModels 
            //  &List van SelectListItems voor bestaande Companies in DB om op te filteren adhv dropdown
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.contractmodels = new List<ContractModel>();
            contractViewModel.Companies = new List<SelectListItem>();
            // ContractModel: Holder, Contract, Company & 'Active' veld
            ContractModel contractModel;

            // Ophalen bestaande companies in DB om op te filteren adhv dropdown
            List<Company> companies = CompMng.GetCompanies();
            foreach (Company c in companies)
            {
                contractViewModel.Companies.Add(new SelectListItem() { Text = c.CompanyName, Value = c.CompanyName });
            }

            IEnumerable<Holder> holderContracts;

            if (!String.IsNullOrEmpty(searchString))
            {
                // filteren op Company
                holderContracts = mng.GetHoldersWithCompanyContractsAndVehicles(searchString);
            }
            else
            {
                holderContracts = mng.GetHoldersWithCompanyContractsAndVehicles();
            }

            foreach (Holder h in holderContracts)
            {
                if (h.Contracts.FirstOrDefault(c => c.Archived == false) != null)
                {
                    // TODO : Check order to see if latest contract
                    //h.Contract.OrderBy(c => c.ContractId);
                    // Check if latest contract is active
                    DateTime start = h.Contracts.FirstOrDefault(c => c.Archived == false).StartDate;
                    DateTime end = h.Contracts.FirstOrDefault(c => c.Archived == false).EndDate;

                    int active = (start < DateTime.Now && DateTime.Now < end) ? 1 : 0;

                    contractModel = new ContractModel()
                    {
                        FirstName = h.FirstName,
                        Name = h.Name,
                        Email = h.Email,
                        Active = active,
                        StartDate = start,
                        EndDate = end,
                        Company = h.Company.CompanyName
                    };
                    contractViewModel.contractmodels.Add(contractModel);
                }
            }
            return View(contractViewModel);
        }

        public ActionResult Edit(int? inputId)
        {
            int id = VerifyId(inputId);
            if (id != -1)
            {
                ViewData.Model = mng.ComposeInputHolder(mng.GetHolder(id));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult EditHolder(int? inputId, InputHolder updateHolder)
        {
            int id = VerifyId(inputId);
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(500);
            } else if (id == -1)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            updateHolder = cleaner.CleanInputHolder(updateHolder);
            mng.ChangeHolder(newId, updateHolder);
            return RedirectToAction("Lijst");


        }

        public ActionResult Details(int? inputId)
        {
            int id = VerifyId(inputId);
            if (id != -1)
            {
                ViewData.Model = mng.ComposeInputHolder(mng.GetHolder(id));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult Delete(int? inputId)
        {
            int id = VerifyId(inputId);
            if (id != -1)
            {
                ViewData.Model = mng.ComposeInputHolder(mng.GetHolder(id));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        //Checkt ID's of ze not null zijn en of ze bestaan in het systeem
        //Returnt int id als de id valid is, anders id = -1
        //Deze methode in Validator plaatsen vraagt toegang tot de mngers
        private int VerifyId(int? id)
        {
            if (id == null)
            {
                return -1;
            }
            else if (mng.GetHolder((int)id) == null)
            {
                return -1;
            }
            return (int)id;
        }

    }
}
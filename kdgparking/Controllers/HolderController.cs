using kdgparking.BL;
using kdgparking.BL.Domain;
using kdgparking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace testParkingWeb.Controllers
{
    [HandleError]
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
            //Als er geen searchString is gewoon een oplijsting
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
                //Formateer de holders naar InputHolders, kans dat dit crasht wanneer er geen contract is
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
            List<HolderVehicle> modelList = new List<HolderVehicle>();
            HolderVehicle model = new HolderVehicle();
            if (!String.IsNullOrEmpty(searchString))
            {
                IEnumerable<Vehicle> vehicles = holdMgt.GetVehicles(searchString);
                foreach (Vehicle v in vehicles)
                {
                    foreach (Holder h in v.Holders)
                    {
                        model = new HolderVehicle()
                        {
                            FirstName = h.FirstName,
                            Name =h.Name,
                            GSM = h.GSM,
                            Phone = h.Phone,
                            Email = h.Email,
                            VehicleName = v.VehicleName,
                            Numberplate = v.Numberplate
                        };
                        modelList.Add(model);
                    }
                }
            }
            return View(modelList);
        }

        public ActionResult LijstActive(string searchString, string status)
        {
            ICompanyManager CompMng = new CompanyManager();
            // Initialisatie ContractViewModel : Bevat List van ContracModels 
            //  &List van SelectListItems voor bestaande Companies in DB om op te filteren adhv dropdown
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.contractmodels = new List<ContractModel>();
            contractViewModel.Companies = new List<SelectListItem>();
            contractViewModel.Status = new List<SelectListItem>();
            // ContractModel: Holder, Contract, Company & 'Active' veld
            ContractModel contractModel;

            // Ophalen bestaande companies & BadgeStatus in DB om op te filteren adhv dropdown
            List<Company> companies = CompMng.GetCompanies();
            foreach (Company c in companies)
            {
                contractViewModel.Companies.Add(new SelectListItem() { Text = c.CompanyName, Value = c.CompanyName });
            }

            foreach (BadgeStatus s in (BadgeStatus[])Enum.GetValues(typeof(BadgeStatus)))
            {
                contractViewModel.Status.Add(new SelectListItem() { Text = s.ToString(), Value = s.ToString() });
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

            if (!String.IsNullOrEmpty(status))
            {
                // filteren op status
                holderContracts = holderContracts.Where(h => h.Contracts.FirstOrDefault(c => c.Archived == false).Badge.BadgeStatus.ToString() == status);
            }

            foreach (Holder h in holderContracts)
            {
                if (h.Contracts.FirstOrDefault(c => c.Archived == false) != null)
                {
                    DateTime start = h.Contracts.FirstOrDefault(c => c.Archived == false).StartDate;
                    DateTime end = h.Contracts.FirstOrDefault(c => c.Archived == false).EndDate;

                    // Check if latest contract is active
                    int active = (start < DateTime.Now && DateTime.Now < end &&
                        h.Contracts.FirstOrDefault(c => c.Archived == false).Badge.BadgeStatus == BadgeStatus.Ok) ? 1 : 0;

                    contractModel = new ContractModel()
                    {
                        HolderId = h.Id,
                        BadgeId = h.Contracts.FirstOrDefault(c => c.Archived == false).Badge.MifareSerial,
                        FirstName = h.FirstName,
                        Name = h.Name,
                        Email = h.Email,
                        Active = active,
                        BadgeStatus = h.Contracts.FirstOrDefault(c => c.Archived == false).Badge.BadgeStatus,
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
            //Check de id en transformeer deze naar non nullable
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
            updateHolder = cleaner.CleanInputHolder(updateHolder);
            mng.ChangeHolder(id, updateHolder);
            return RedirectToAction("Lijst");


        }

        public ActionResult Details(int inputId)
        {
            int id = VerifyId(inputId);
            if (id != -1)
            {
                ViewData.Model = mng.ComposeInputHolder(mng.GetHolder(inputId/*id*/));
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
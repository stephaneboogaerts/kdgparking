using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kdgparking.BL;
using kdgparking.BL.Domain;
using System.Net.Mail;
using System.Threading.Tasks;
using kdgparking.Models;

namespace testParkingWeb.Controllers
{
    public class PersoonController : Controller
    {
        private IManager mng = new Manager();

        //Meer feedback

        public ActionResult Index()
        {
            return RedirectToAction("Lijst");
        }

        public ActionResult Lijst(string searchString)
        {
            IEnumerable<Holder> holders;
            if (!String.IsNullOrEmpty(searchString))
            {
                holders = mng.GetHolders(searchString);
            }
            else
            {
                holders = mng.GetHolders();
            }
            List<InputHolder> iHolders = new List<InputHolder>();
            foreach (Holder h in holders)
            {
                iHolders.Add(this.ComposeInputHolder(h));
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
                mng.AddNewHolder(nieuweHolder);
            }
            return View();
        }

        public ActionResult LijstVoertuigen(string searchString)
        {
            // TODO : Testen of Holder &Vehicle Distinct zijn
            List<HolderVehicle> modelList = new List<HolderVehicle>();
            HolderVehicle model = new HolderVehicle();
            if (!String.IsNullOrEmpty(searchString))
            {
                IEnumerable<Vehicle> vehicles = mng.GetVehicles(searchString);
                foreach(Vehicle v in vehicles)
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
            // Initialisatie ContractViewModel : Bevat List van ContracModels 
            //  &List van SelectListItems voor bestaande Companies in DB om op te filteren adhv dropdown
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.contractmodels = new List<ContractModel>();
            contractViewModel.Companies = new List<SelectListItem>();
            // ContractModel: Holder, Contract, Company & 'Active' veld
            ContractModel contractModel;

            // Ophalen bestaande companies in DB om op te filteren adhv dropdown
            List<Company> companies = mng.GetCompanies();
            foreach(Company c in companies)
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
                // TODO : Check order to see if latest contract
                h.Contracts.OrderBy(c => c.ContractId);
                // Check if latest contract is active
                int active = (h.Contracts[0].StartDate < DateTime.Now && DateTime.Now < h.Contracts[0].EndDate) ? 1 : 0;

                contractModel = new ContractModel()
                {
                    ContractId = h.Contracts[0].ContractId,
                    FirstName = h.FirstName,
                    Name = h.Name,
                    Email = h.Email,
                    Active = active,
                    StartDate = h.Contracts[0].StartDate,
                    EndDate = h.Contracts[0].EndDate,
                    Company = h.Company.CompanyName
                };
                contractViewModel.contractmodels.Add(contractModel);
            }
            return View(contractViewModel);
        }

            public ActionResult Edit(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = this.ComposeInputHolder(mng.GetHolder(newId));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult EditHolder(int? id, InputHolder updateHolder)
        {
            if (!this.VerifyId(id))
            {
                return new HttpStatusCodeResult(404);
            } else if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(500);
            }
            int newId = (int)id;
            mng.ChangeHolder(newId, updateHolder);
            return RedirectToAction("Lijst");
        }

        public ActionResult Details(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = this.ComposeInputHolder(mng.GetHolder(newId));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (this.VerifyId(id))
            {
                int newId = (int)id;
                ViewData.Model = this.ComposeInputHolder(mng.GetHolder(newId));
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(404);
            }
        }

        private InputHolder ComposeInputHolder(Holder holder)
        {
            Contract holderContract = mng.GetHolderContract(holder.Id);
            return new InputHolder()
            {
                HolderId = holder.Id,
                Name = holder.Name,
                FirstName = holder.FirstName,
                Company = holder.Company.CompanyName,
                Email = holder.Email,
                StartDate = holderContract.StartDate,
                EndDate = holderContract.EndDate,
            };
        }

        private bool VerifyId(int? id)
        {
            if (id == null)
            {
                return false;
            } else
            {
                int newId = (int)id;
                if (mng.GetHolder(newId) == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
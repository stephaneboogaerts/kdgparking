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
        [HttpPost]
        public ActionResult Toevoegen(InputHolder nieuweHolder)
        {
            if (ModelState.IsValid)
            {
                mng.AddNewHolder(nieuweHolder);
            }
            return View();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Lijst");
        }

        public ActionResult Toevoegen()
        {
            return View();
        }
        
        public ActionResult Lijst(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewData.Model = mng.GetHolders(searchString);
            }else
            {
                ViewData.Model = mng.GetHolders();
            }
            return View();
        }

        public ActionResult LijstVoertuigen(string searchString)
        {
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
            ContractModel contractModel;
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.contractmodels = new List<ContractModel>();
            contractViewModel.Companies = new List<SelectListItem>();

            List<Company> companies = mng.GetCompanies();
            foreach(Company c in companies)
            {
                contractViewModel.Companies.Add(new SelectListItem() { Text = c.CompanyName, Value = c.CompanyName });
            }

            IEnumerable<Holder> holderContracts;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                holderContracts = mng.GetHoldersWithCompanyContractsAndVehicles(searchString);

                foreach (Holder h in holderContracts)
                {
                    // TODO : Check order to see if latest contract
                    h.Contracts.OrderBy(c => c.ContractId);

                    if (h.Contracts[0].StartDate < DateTime.Now && DateTime.Now < h.Contracts[0].EndDate)
                    {
                        contractModel = new ContractModel()
                        {
                            ContractId = h.Contracts[0].ContractId,
                            FirstName = h.FirstName,
                            Name = h.Name,
                            Email = h.Email,
                            Active = 1,
                            StartDate = h.Contracts[0].StartDate,
                            EndDate = h.Contracts[0].EndDate,
                            Company = h.Company.CompanyName
                        };
                        contractViewModel.contractmodels.Add(contractModel);
                    }
                }
            }
            else
            {
                holderContracts = mng.GetHoldersWithCompanyContractsAndVehicles();

                foreach (Holder h in holderContracts)
                {
                    // TODO : Check order to see if latest contract
                    h.Contracts.OrderBy(c => c.ContractId);
                    // Check if latest contract is active
                    int active = (h.Contracts[0].StartDate < DateTime.Now && DateTime.Now < h.Contracts[0].EndDate) ?  1 : 0;

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
            }
            return View(contractViewModel);
        }

            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int) id;
            ViewData.Model = mng.GetHolder(newId);
            if (ViewData.Model == null)
            {
                return new HttpStatusCodeResult(404);
            } else
            {
                return View();
            }
        }

        [HttpPut]
        public ActionResult Edit(int? id, InputHolder updateHolder)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            if (mng.GetHolder(newId) == null)
            {
                return new HttpStatusCodeResult(404);
            } else if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(500);
            }
            mng.ChangeHolder(newId, updateHolder);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            ViewData.Model = mng.GetHolder(newId);
            if (ViewData.Model == null)
            {
                return new HttpStatusCodeResult(404);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(404);
            }
            int newId = (int)id;
            ViewData.Model = mng.GetHolder(newId);
            if (ViewData.Model == null)
            {
                return new HttpStatusCodeResult(404);
            }
            else
            {
                return View();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kdgparking.Models
{
    public class ContractViewModel
    {
        public List<ContractModel> contractmodels { get; set; }
        public List<SelectListItem> Companies { get; set; }
        public List<SelectListItem> Status { get; set; }
        public string CompanySearch { get; set; }
    }
}
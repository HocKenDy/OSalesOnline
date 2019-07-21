using Erp.Domain.Account.Entities;
using qts.webapp.backend.domain.Models.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Account.Models
{
    public class ImportViewModel
    {
        public ImportViewModel()
        {

        }
        public Customer CustomerList { get; set; }
        public Cars Cars { get; set; }

    }
}
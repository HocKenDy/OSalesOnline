using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.BackOffice.Areas.Administration.Models
{
    public partial class UserTypeViewModel
    {
        public UserTypeViewModel()
        {
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Scope { get; set; }
        public List<User> ListUser { get; set; }
    }
    public class UserTypeCodeClass
    {
        public const string SalesExcutive = "SalesExcutive";
    }
}
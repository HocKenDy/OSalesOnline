using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Entities
{
    public partial class UserType
    {
        public UserType()
        {
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string Note { get; set; }
        public Nullable<bool> Scope { get; set; }
        public string HomePage { get; set; }
        public string Code { get; set; }
    }
}
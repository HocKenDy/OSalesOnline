using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Administration
{
    public class Province  : BaseModel
    {
        public Province()
        {
            
        }
                public string Name { get; set; }
        public string Type { get; set; }
        public string ProvinceId { get; set; }
        //public bool? IsCCCD { get; set; }
    }
}

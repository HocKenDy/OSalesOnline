using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Administration
{
    public class District  : BaseModel
    {
        public District()
        {
            
        }
                public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }

    }
}

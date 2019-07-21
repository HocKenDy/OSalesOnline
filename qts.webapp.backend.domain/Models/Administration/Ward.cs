using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Administration
{
    public class Ward  : BaseModel
    {
        public Ward()
        {
            
        }
                public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }

    }
}

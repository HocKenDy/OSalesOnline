using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Administration
{
    public class vwWard : BaseModel
    {
        public vwWard()
        {

        }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictType { get; set; }
        public string ProvinceType { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceId { get; set; }
    }
}

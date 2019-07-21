using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class CarLine  : BaseModel
    {
        public CarLine()
        {
            
        }

        public string ManufacturerCar { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

    }
}

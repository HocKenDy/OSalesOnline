using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Sale
{
    public class Cars : BaseModel
    {
        public Cars()
        {

        }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Vehicles { get; set; }
        public string Color { get; set; }
        public Nullable<int> Frames { get; set; }
        public Nullable<int> Number { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CarLineId { get; set; }
        public string Note { get; set; }
        public string Plate { get; set; }

    }
}

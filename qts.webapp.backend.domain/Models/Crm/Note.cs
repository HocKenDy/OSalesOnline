using qts.webapp.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qts.webapp.backend.domain.Models.Crm
{
    public class Note  : BaseModel
    {
        public Note()
        {
            
        }
                public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string ContentHtml { get; set; }

    }
}

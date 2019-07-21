using Erp.BackOffice.Filters;
using qts.webapp.domain.Models;
using System;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Erp.BackOffice.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class BaseController : Controller
    {
        protected void SetModifier(BaseModel model, bool isEdit = false)
        {
           if (!isEdit) {
               model.IsDeleted = false;
               model.CreatedUserId = WebSecurity.CurrentUserId;
               model.CreatedDate = DateTime.Now;
           }
           model.ModifiedUserId = WebSecurity.CurrentUserId;
           model.ModifiedDate = DateTime.Now;
       }
    }
}

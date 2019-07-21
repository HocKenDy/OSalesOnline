using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Areas.Administration.Models
{
    public class AccessRightViewModel
    {
        public List<PageGroupViewModel> ListPageGroup { get; set; }
        public List<int> ListAccessRight { get; set; }
        public int? UserTypeId { get; set; }
        public int? UserId { get; set; }
        public List<UserTypeViewModel> ListUserTypes { get; set; }
        public List<UserType> ListUserType { get; set; }
        public List<vwUsers> ListUser { get; set; }
        public int ApplicationId { get; set; }
    }

    public class AccessRightCreateModel
    {
        public List<int> ListPages { get; set; }
        public int UserTypeId { get; set; }
        public int UserId { get; set; }
        //public int ApplicationId { get; set; }
    }

    public class PageGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool Selected { get; set; }
        public bool Visible { get; set; }
        public bool Status { get; set; }
        public List<PageGroupViewModel> ListPageGroup { get; set; }
        public int CountSubAction { get; set; }
        public List<PageGroupViewModel> ListSubAction { get; set; }
    }
}
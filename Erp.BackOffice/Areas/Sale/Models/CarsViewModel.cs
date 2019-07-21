using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale.Models
{
    public class CarsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }
        public int? CustomerId { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "AssignedUserId", ResourceType = typeof(Wording))]
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        //[StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        public string Name { get; set; }
        [Display(Name = "ManufacturerCar", ResourceType = typeof(Wording))]
        public string Manufacturer { get; set; }
        [Display(Name = "Vehicles", ResourceType = typeof(Wording))]
        public string Vehicles { get; set; }
        [Display(Name = "Vehicles", ResourceType = typeof(Wording))]
        public int? CarLineId { get; set; }
        [Display(Name = "Vehicles", ResourceType = typeof(Wording))]
        public string CarLineName { get; set; }
        [Display(Name = "ColorCar", ResourceType = typeof(Wording))]
        public string Color { get; set; }
        [Display(Name = "Frames", ResourceType = typeof(Wording))]
        public Nullable<int> Frames { get; set; }
        [Display(Name = "NumberCar", ResourceType = typeof(Wording))]
        public Nullable<int> Number { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Wording))]
        public string Note { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Plate", ResourceType = typeof(Wording))]
        public string Plate { get; set; }

		
    }
}
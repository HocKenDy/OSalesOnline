using Erp.BackOffice.App_GlobalResources;
using Erp.BackOffice.Areas.Administration.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Erp.BackOffice.Sale.Models
{
    public class ConfigsViewModel
    {
        public List<SettingViewModel> ListTemplatePrint { get; set; }
        public List<SettingViewModel> ListSaleSetting { get; set; }
        public List<SettingViewModel> ListSaleSetting_Prefix { get; set; }
        public List<SettingViewModel> ListSaleSetting_OrderNo { get; set; }
        public List<SettingViewModel> ListSaleSetting_Manual { get; set; }
    }
}
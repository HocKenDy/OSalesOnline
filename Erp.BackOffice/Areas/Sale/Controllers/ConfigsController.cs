using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Repositories;
using Erp.Domain.Account.Interfaces;
using Erp.Domain.Account.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using System.Web.Script.Serialization;
using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using Erp.BackOffice.Account.Controllers;
using Erp.BackOffice.Account.Models;
using System.Web;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class ConfigsController : Controller
    {
        //private readonly IWarehouseRepository WarehouseRepository;
        //private readonly IWarehouseLocationItemRepository WarehouseLocationItemRepository;
        //private readonly IProductInvoiceRepository ProductInvoiceRepository;
        //private readonly IPurchaseOrderRepository PurchaseOrderRepository;
        //private readonly IInventoryRepository InventoryRepository;
        //private readonly IProductRepository ProductRepository;
        //private readonly IProductOutboundRepository ProductOutboundRepository;
        //private readonly IProductInboundRepository ProductInboundRepository;
        private readonly IUserRepository userRepository;
        //private readonly ITemplatePrintRepository templatePrintRepository;
        private readonly IQueryHelper QueryHelper;
        //private readonly ICustomerRepository customerRepository;
        private readonly ISettingRepository settingRepository;
        public ConfigsController(
            // IWarehouseRepository _Warehouse
            //, IWarehouseLocationItemRepository _WarehouseLocationItem
            //, IInventoryRepository _Inventory
            //, IProductInvoiceRepository _ProductInvoice
            //, IPurchaseOrderRepository _PurchaseOrder
            //, IProductRepository _Product
            //, IProductOutboundRepository _ProductOutbound
            //, IProductInboundRepository _ProductInbound
             IUserRepository _user
            , IQueryHelper _QueryHelper
            //, ITemplatePrintRepository _templatePrint
            //, ICustomerRepository customer
            , ISettingRepository _Setting
            )
        {
            //WarehouseRepository = _Warehouse;
            //WarehouseLocationItemRepository = _WarehouseLocationItem;
            //InventoryRepository = _Inventory;
            //ProductInvoiceRepository = _ProductInvoice;
            //PurchaseOrderRepository = _PurchaseOrder;
            //ProductRepository = _Product;
            //ProductOutboundRepository = _ProductOutbound;
            //ProductInboundRepository = _ProductInbound;
            userRepository = _user;
            QueryHelper = _QueryHelper;
            //templatePrintRepository = _templatePrint;
            //customerRepository = customer;
            settingRepository = _Setting;
        }

        #region Index
        public ViewResult Index()
        {
            ConfigsViewModel model = new ConfigsViewModel();
            model.ListTemplatePrint = new List<Areas.Administration.Models.SettingViewModel>();
            model.ListSaleSetting = new List<Areas.Administration.Models.SettingViewModel>();
            model.ListSaleSetting_Prefix = new List<Areas.Administration.Models.SettingViewModel>();
            model.ListSaleSetting_OrderNo = new List<Areas.Administration.Models.SettingViewModel>();
            model.ListSaleSetting_Manual = new List<Areas.Administration.Models.SettingViewModel>();

            //settingprint
            var q1 = settingRepository.GetAlls()
                .Where(item => item.Code == "settingprint")
                .OrderBy(item => item.Note).ToList();

            AutoMapper.Mapper.Map(q1, model.ListTemplatePrint);

            //salesetting
            var q2 = settingRepository.GetAlls()
                .Where(item => item.Code == "salesetting")
                .OrderBy(item => item.Note).ToList();

            AutoMapper.Mapper.Map(q2, model.ListSaleSetting);

            //salesetting_prefix
            var q3 = settingRepository.GetAlls()
                .Where(item => item.Code == "salesetting_prefix")
                .OrderBy(item => item.Note).ToList();

            AutoMapper.Mapper.Map(q3, model.ListSaleSetting_Prefix);

            //salesetting_orderno
            var q4 = settingRepository.GetAlls()
                .Where(item => item.Code == "salesetting_orderno")
                .OrderBy(item => item.Note).ToList();

            AutoMapper.Mapper.Map(q4, model.ListSaleSetting_OrderNo);

            //salesetting_manual
            var q5 = settingRepository.GetAlls()
                .Where(item => item.Code == "salesetting_manual")
                .OrderBy(item => item.Note).ToList();

            AutoMapper.Mapper.Map(q5, model.ListSaleSetting_Manual);

            return View(model);
        }
        #endregion
    }
}

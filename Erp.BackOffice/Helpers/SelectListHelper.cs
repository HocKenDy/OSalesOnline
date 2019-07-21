using Erp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erp.Domain.Sale.Repositories;
using Erp.Domain.Account.Repositories;
using Erp.Domain.Entities;
using System.Globalization;
using qts.webapp.backend.domain.Services.Administration;
using qts.webapp.domain.Repositories;
using qts.webapp.backend.domain.Services.Sale;
using Erp.Domain.Sale.Interfaces;

namespace Erp.BackOffice.Helpers
{
    public class SelectListHelper
    {
        public static SelectList SelectListMonth(string text)
        {
            var selectListItems = new List<SelectListItem>();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            for (int i = 1; i <= 12; i++)
            {
                string value = string.Format("{0}/{1}", i.ToString("00"), year);
                var item = new SelectListItem { Value = value, Text = string.Format(text, value) };
                selectListItems.Add(item);
            }

            if (month <= 3)
            {
                for (int i = 12; i >= 10; i--)
                {
                    string value = string.Format("{0}/{1}", i.ToString("00"), year - 1);
                    var item = new SelectListItem { Value = value, Text = string.Format(text, value) };
                    selectListItems.Insert(0, item);
                }
            }

            if (month >= 10)
            {
                for (int i = 1; i <= 3; i++)
                {
                    string value = string.Format("{0}/{1}", i.ToString("00"), year + 1);
                    var item = new SelectListItem { Value = value, Text = string.Format(text, value) };
                    selectListItems.Insert(selectListItems.Count, item);
                }
            }

            var selectList = new SelectList(selectListItems, "Value", "Text", string.Format("{0}/{1}", month.ToString("00"), year));

            return selectList;
        }

        public static List<SelectListItem> SelectListItemMonth(string text)
        {
            var selectListItems = new List<SelectListItem>();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            for (int i = 1; i <= 12; i++)
            {
                string value = string.Format("{0}/{1}", i.ToString("00"), year);
                var item = new SelectListItem { Value = value, Text = string.Format("{0} {1}", text, value) };
                selectListItems.Add(item);
            }

            if (month <= 3)
            {
                for (int i = 12; i >= 10; i--)
                {
                    string value = string.Format("{0}/{1}", i.ToString("00"), year - 1);
                    var item = new SelectListItem { Value = value, Text = string.Format("{0} {1}", text, value) };
                    selectListItems.Insert(0, item);
                }
            }

            if (month >= 10)
            {
                for (int i = 1; i <= 3; i++)
                {
                    string value = string.Format("{0}/{1}", i.ToString("00"), year + 1);
                    var item = new SelectListItem { Value = value, Text = string.Format("{0} {1}", text, value) };
                    selectListItems.Insert(selectListItems.Count, item);
                }
            }

            return selectListItems;
        }

        public static SelectList GetSelectList_Category(string sCode, object SelectedValue, string NullOrNameEmpty)
        {
            return GetSelectList_Category(sCode, SelectedValue, null, NullOrNameEmpty);
        }
        public static SelectList GetSelectList_CarLine(string manufacturerCar, object SelectedValue, string NullOrNameEmpty)
        {
            ICarLineService carLineService = DependencyResolver.Current.GetService<ICarLineService>();
            var selectListItems = new List<SelectListItem>();
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var db = carLineService.GetByManufacturerCar(manufacturerCar).ToList();
                foreach (var i in db)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }
        public static SelectList GetSelectList_Category(string sCode, object SelectedValue, string sValueField, string NullOrNameEmpty)
        {
            CategoryRepository categoryRepository = new CategoryRepository(new Domain.ErpDbContext());
            var selectListItems = new List<SelectListItem>();

            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var q = categoryRepository.GetCategoryByCode(sCode);

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    if (sValueField != null && sValueField == "Name")
                        item.Value = i.Name;
                    else if (sValueField != null && sValueField == "Value")
                        item.Value = i.Value;
                    else
                        item.Value = i.Value;

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }
        public static SelectList GetSelectList_Category2(string sCode, object SelectedValue, string NullOrNameEmpty, string ValueEmpty)
        {
            return GetSelectList_Category2(sCode, SelectedValue, null, NullOrNameEmpty, ValueEmpty);
        }
        public static SelectList GetSelectList_Category2(string sCode, object SelectedValue, string sValueField, string NullOrNameEmpty, string ValueEmpty)
        {
            CategoryRepository categoryRepository = new CategoryRepository(new Domain.ErpDbContext());
            var selectListItems = new List<SelectListItem>();

            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                if (!string.IsNullOrEmpty(ValueEmpty))
                {
                    itemEmpty.Value = ValueEmpty;
                }
                else
                {
                    itemEmpty.Value = null;
                }
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var q = categoryRepository.GetCategoryByCode(sCode);

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    if (sValueField != null && sValueField == "Name")
                        item.Value = i.Name;
                    else if (sValueField != null && sValueField == "Value")
                        item.Value = i.Value;
                    else
                        item.Value = i.Value;

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }
        public static SelectList GetSelectList_Number(int nStart, int nEnd, object sValue)
        {
            var selectListItems = new List<SelectListItem>();

            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;

            selectListItems.Add(itemEmpty);

            try
            {
                for (int i = nStart; i <= nEnd; i++)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.ToString();
                    item.Value = i.ToString();

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Gender(object sValue)
        {
            var selectListItems = new List<SelectListItem>();

            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;

            selectListItems.Add(itemEmpty);

            try
            {
                SelectListItem item = new SelectListItem();
                item.Text = "Nam";
                item.Value = "false";

                selectListItems.Add(item);

                SelectListItem item2 = new SelectListItem();
                item2.Text = "Nữ";
                item2.Value = "true";

                selectListItems.Add(item2);
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Location(Domain.Interfaces.ILocationRepository locationRepository, string sParentId, object sValue)
        {
            var selectListItems = new List<SelectListItem>();

            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;

            selectListItems.Add(itemEmpty);

            try
            {
                var q = locationRepository.GetList(sParentId);

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Erp.BackOffice.Helpers.Common.Capitalize(i.Name.ToLower());
                    item.Value = i.Id;

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        #region SelectList for PageMenu
        public static SelectList GetSelectList_Page(string AreaName)
        {
            var selectListItems = new List<SelectListItem>();
            PageRepository pageRepository = new PageRepository(new Domain.ErpDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);

            try
            {
                var q = pageRepository.GetPages()
                    .Where(item => item.AreaName == AreaName).ToList();

                var controllerList = q.GroupBy(
                                        p => p.ControllerName,
                                        (key, g) => new
                                        {
                                            ControllerName = key,
                                            ActionList = g.ToList()
                                        }
                                        ).OrderBy(item => item.ControllerName);
                int index1 = 1;
                foreach (var controller in controllerList)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = index1 + ". " + controller.ControllerName;
                    item.Value = controller.ControllerName;
                    selectListItems.Add(item);

                    int index2 = 1;
                    foreach (var action in controller.ActionList)
                    {
                        SelectListItem itemAction = new SelectListItem();
                        itemAction.Text = HttpContext.Current.Server.HtmlDecode(@"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + index1 + "." + index2 + controller.ControllerName + "/" + action.ActionName;
                        itemAction.Value = action.Id.ToString();
                        selectListItems.Add(itemAction);

                        index2++;
                    }

                    index1++;
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", null);

            return selectList;
        }

        public static SelectList GetSelectList_PageMenu(string LanguageId)
        {
            var selectListItems = new List<SelectListItem>();
            PageMenuRepository pageMenuRepository = new PageMenuRepository(new Domain.ErpDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);

            try
            {
                var q = pageMenuRepository.GetPageMenus(LanguageId).ToList();

                List<vwPageMenu> pageMenuResults = new List<vwPageMenu>();
                BuildListMuiltiLevel(q, pageMenuResults, null);

                foreach (var i in pageMenuResults)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", null);

            return selectList;
        }

        public static void BuildListMuiltiLevel(List<vwPageMenu> pageMenus, List<vwPageMenu> pageMenuResults, int? parentId, string level = "")
        {
            int index = 1;
            var parentMenus = pageMenus.Where(x => x.ParentId == parentId).OrderBy(x => x.OrderNo).ToList();
            foreach (vwPageMenu item in parentMenus)
            {
                string prefix = string.IsNullOrEmpty(level) ? index.ToString(CultureInfo.InvariantCulture) : level + "." + index.ToString(CultureInfo.InvariantCulture);
                int n = prefix.Split('.').Count();
                string tab = "";
                for (int i = 1; i < n; i++)
                {
                    tab = tab + @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                item.Name = HttpContext.Current.Server.HtmlDecode(tab) + prefix + " " + item.Name;
                pageMenuResults.Add(item);
                BuildListMuiltiLevel(pageMenus, pageMenuResults, item.Id, prefix);
                index++;
            }
        }
        #endregion

        public static SelectList GetSelectList_Module(object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            ModuleRepository ModuleRepository = new ModuleRepository(new Domain.ErpDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = ModuleRepository.GetAllModule().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Name.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_MetadataFields(string ModuleName, object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            MetadataFieldRepository MetadataFieldRepository = new MetadataFieldRepository(new Domain.ErpDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = MetadataFieldRepository.GetAllMetadataField()
                    .Where(item => item.ModuleName == ModuleName)
                    .OrderBy(item => item.Id).ToList();
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Name.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Branch(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            BranchRepository branchRepository = new BranchRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = branchRepository.GetAllBranch().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_BranchDepartment(object sValue, int BranchId, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            BranchDepartmentRepository branchDepartmentRepository = new BranchDepartmentRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = branchDepartmentRepository.GetAllvwBranchDepartment().Where(x => x.Sale_BranchId == BranchId).OrderBy(item => item.Sale_DepartmentId);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Sale_DepartmentId;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_User(object sValue)
        {
            return GetSelectList_User(sValue, null,null, App_GlobalResources.Wording.Empty);
        }
        public static SelectList GetSelectList_User(object sValue, int? BranchId)
        {
            return GetSelectList_User(sValue, null, BranchId, App_GlobalResources.Wording.Empty);
        }
        public static SelectList GetSelectList_User(object sValue, string UserTypeCode,  string NullOrNameEmpty)
        {
            return GetSelectList_User(sValue, UserTypeCode, null, NullOrNameEmpty);
        }
        public static SelectList GetSelectList_User(object sValue, string UserTypeCode, int? BranchId,string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = userRepository.GetAllvwUsers().OrderBy(item => item.FullName).AsEnumerable();
                if(!string.IsNullOrEmpty(UserTypeCode))
                {
                    q = q.Where(x => x.UserTypeCode == UserTypeCode);
                }
                if(BranchId != null)
                {
                    q = q.Where(x => x.BranchId == BranchId);
                }
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.FullName;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList(string TableName, string DisplayField, string ValueField, object sValue, string emptyLabel = null)
        {
            var selectListItems = new List<SelectListItem>();
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = emptyLabel == null ? App_GlobalResources.Wording.Empty : emptyLabel;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = Domain.Helper.SqlHelper.QuerySQL<SelectListItem>(string.Format("select {0} as [Value], {1} as Text from {2} order by {3}", DisplayField, ValueField, TableName, DisplayField));
                selectListItems.AddRange(q);
                //foreach (var i in q)
                //{
                //    SelectListItem item = new SelectListItem();
                //    item.Text = i.UserName;
                //    item.Value = i.Id.ToString();
                //    selectListItems.Add(item);
                //}
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Contact(object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            ContactRepository ContactRepository = new ContactRepository(new Domain.Account.ErpAccountDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = ContactRepository.GetAllContact().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.LastName + " " + i.FirstName;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Contract(object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            ContractRepository contractRepository = new ContractRepository(new Domain.Account.ErpAccountDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = contractRepository.GetAllContract().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Code;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_CategoryValueTextName(string sCode, object SelectedValue, string sValueField, string NullOrNameEmpty)
        {
            CategoryRepository categoryRepository = new CategoryRepository(new Domain.ErpDbContext());
            var selectListItems = new List<SelectListItem>();

            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var q = categoryRepository.GetCategoryByCode(sCode);

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name + " - " + i.Value;
                    if (sValueField != null && sValueField == "Name")
                        item.Value = i.Name;
                    else if (sValueField != null && sValueField == "Value")
                        item.Value = i.Value;
                    else
                        item.Value = i.Value;

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }

        public static SelectList GetSelectList_UserbyCreateModuel(object sValue, string ActionName, string ModuelName, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
            UserTypePageRepository userTypePageRepository = new UserTypePageRepository(new Domain.ErpDbContext());
            PageRepository pageRepository = new PageRepository(new Domain.ErpDbContext());

            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var page = pageRepository.GetPageByAcctionController(ActionName, ModuelName);
                var utype = userTypePageRepository.GetAllItem().Where(x => x.PageId == page.Id);
                var model = utype.Select(x => new Areas.Administration.Models.UserTypePageViewModel
                {
                    PageId = x.PageId,
                    UserTypeId = x.UserTypeId
                }).ToList();
                foreach (var i in model)
                {
                    var UserList = userRepository.GetUsers().Where(x => x.UserTypeId == i.UserTypeId)
               .Select(x => new SelectListItem
               {
                   Value = x.Id.ToString(),
                   Text = x.FullName

               }).ToList();
                    selectListItems = selectListItems.Union(UserList).ToList();
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_TemplatePrint(object sValue, string ModelName, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            TemplatePrintRepository templatePrintRepository = new TemplatePrintRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains(ModelName)).OrderBy(item => item.CreatedDate);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Title;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_WarehouseLocationItem(object sValue, int? ProductId)
        {
            var selectListItems = new List<SelectListItem>();
            WarehouseLocationItemRepository ContactRepository = new WarehouseLocationItemRepository(new Domain.Sale.ErpSaleDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = ContactRepository.GetAllLocationItem().Where(x => x.ProductId == ProductId && x.IsOut != true).OrderBy(item => item.ExpiryDate).ToList();
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = "Tầng: " + i.Floor + "Kệ: " + i.Shelf + " - Ngày sản xuất:" + i.ExpiryDate.Value.ToString() + " - Lô sản xuất:" + i.LoCode;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_FullUserName(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = userRepository.GetAllUsers().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.FullName;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_Customer(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            CustomerRepository customerRepository = new CustomerRepository(new Domain.Account.ErpAccountDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = customerRepository.GetAllCustomer().Where(x=>x.IsDeleted != true).OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_Setting(object sValue, string Code, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            SettingRepository settingRepository = new SettingRepository(new Domain.ErpDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = settingRepository.GetAll().Where(x => x.Code == Code).OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Note;
                    item.Value = i.Key.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_Warehouse(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            WarehouseRepository warehouseRepository = new WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = warehouseRepository.GetAllWarehouse().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_Warehouse(object sValue, int? BranchId, string NullOrNameEmpty)
        {
            return GetSelectList_Warehouse(sValue, BranchId, null,null, NullOrNameEmpty);
        }
        public static SelectList GetSelectList_Warehouse(object sValue, int? BranchId, string Categories,int? KeeperId, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            WarehouseRepository warehouseRepository = new WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = warehouseRepository.GetAllWarehouse().OrderBy(item => item.Id).AsEnumerable();
                if (BranchId != null)
                {
                    q = q.Where(x => x.BranchId == BranchId);
                }
                if (!string.IsNullOrEmpty(Categories))
                {
                    q = q.Where(x => x.Categories.Contains(Categories));
                }
                if(KeeperId != null)
                {
                    q = q.Where(x => x.KeeperId.Contains(KeeperId.Value.ToString()));
                }
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_Warehouse(object sValue, int? BranchId, string NullOrNameEmpty, bool IsSale = true)
        {
            var selectListItems = new List<SelectListItem>();
            WarehouseRepository warehouseRepository = new WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = warehouseRepository.GetAllWarehouse().Where(x => x.IsSale == IsSale).OrderBy(item => item.Id).AsEnumerable();
                if (BranchId != null)
                {
                    q = q.Where(x => x.BranchId == BranchId);
                }
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_Supplier(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            SupplierRepository supplierRepository = new SupplierRepository(new Domain.Sale.ErpSaleDbContext());
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = supplierRepository.GetAllSupplier().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.CompanyName;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Provinces(object SelectedValue, string NullOrNameEmpty, bool notGetAll = true)
        {
            IProvinceService provinceRepository = DependencyResolver.Current.GetService<IProvinceService>();
            var selectListItems = new List<SelectListItem>();
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = provinceRepository.Get();

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.ProvinceId;
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }
        public static SelectList GetSelectList_Card(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            IProductRepository ProductRepository = DependencyResolver.Current.GetService<IProductRepository>();
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = ProductRepository.GetAllProductByType("card").OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
        public static SelectList GetSelectList_MemberCardType(object sValue, string NullOrNameEmpty)
        {
            var selectListItems = new List<SelectListItem>();
            IMemberCardTypeRepository MemberCardTypeRepository = DependencyResolver.Current.GetService<IMemberCardTypeRepository>();
            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }
            try
            {
                var q = MemberCardTypeRepository.GetAll().OrderBy(item => item.Name);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
    }
}
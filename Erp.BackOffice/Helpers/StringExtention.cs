using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erp.BackOffice.Helpers
{
    public static class StringExtention
    {
        public static string ToLowerOrEmpty(this string input)
        {
            return (input != null ? input.ToLower() : "");
        }

        public const string ACTION_CREATE = "Create";
        public const string ACTION_DELETE = "Delete";
        public const string ACTION_INDEX = "Index";
        public const string ACTION_DETAIL = "Detail";
        public const string ACTION_PRINT = "Print";
        public const string ACTION_PRINT_ALL = "PrintAll";
        public const string ACTION_PRINT_LIST = "PrintList";
        public const string ACTION_EDIT = "Edit";
        public const string ACTION_EXPORT_EXCEL = "ExportExcel";
        public const string ACTION_SUBMIT = "Submit";
        public const string ACTION_ACCEPT = "Accept";
        public const string ACTION_APPROVED = "Approved";
        public const string ACTION_COMMAND = "Command";
        public const string ACTION_SYNCH = "Synch";
        public const string ACTION_ITEM = "Item";

        //AREA
        public const string AREA_SALE = "Sale";
        public const string AREA_STAFF = "Staff";
        public const string AREA_CRM = "Crm";
        public const string AREA_ACCOUNT = "Account";
        public const string AREA_ADMINISTRATION = "Administration";
        public const string AREA_SERVICECHARGE = "ServiceCharge";

        //FORMAT
        public const string FORMAT_DATE = "dd/mm/yyyy";

        //STRING
        public const string STRING_DETAILLIST = "DetailList";
        public const string STRING_VND = "VNĐ";
        public const string STRING_PROVINCE = "AdProvince";
        public const string STRING_DISTRICT = "District";
        public const string STRING_WARD = "Ward";
        public const string STRING_SUCCESS = "success";
        public const string STRING_FAIL = "Fail";
        public const string STRING_CUSTOMER = "Customer";
        public const string STRING_SUPPLIER = "Supplier";
        public const string STRING_PAYMENT_CATEGORY = "payment_Category";
        public const string STRING_SPENDING_EXCEEDS_THE_LIMIT = "spending_exceeds_the_limit";
        public const string STRING_CYCLEUNIT_CATEGORY = "CycleUnit";
        public const string STRING_VAT_CATEGORY = "vatCategory";
        public const string STRING_CYCLEUNIT_DAY = "Ngày";
        public const string STRING_CYCLEUNIT_MONTH = "Tháng";
        public const string STRING_CYCLEUNIT_YEAR = "Năm";

        //LAYOUT
        public const string LAYOUT_ADMIN = "~/Views/Shared/ACE_AdminLayout.cshtml";
        public const string LAYOUT_POPUP = "~/Views/Shared/_PopupLayout.cshtml";

        //CONBTROL - MODULE
        public const string MODULE_MONTH_PERIOD_DETAIL = "MonthPeriodDetail";
        public const string MODULE_TYPECUSTOMER = "TypeCustomer";
        public const string MODULE_CUSTOMER = "Customer";
        public const string MODULE_YES = "Yes";
        public const string MODULE_PARTNER = "Partner";
        public const string MODULE_CHARGINGPARTNER = "ChargingPartner";
        public const string MODULE_MANAGEACCOUNT = "ManageAccount";
        public const string MODULE_PRODUCT = "Product";
        public const string MODULE_SUPPLIERSERVICE = "SupplierService";
        public const string MODULE_SERVICEPACKAGE = "ServicePackage";
        public const string MODULE_USER = "User";
        public const string MODULE_SUPPLIERSERVICECONTRACT = "SupplierServiceContract";
        public const string MODULE_SUPPLIERSERVICECONTRACTDETAIL = "SupplierServiceContractDetail";
        public const string MODULE_SUPPLIERSERVICEINVOICE = "SupplierServiceInvoice";
        public const string MODULE_CARLINE = "CarLine";
        public const string MODULE_HISTORYPOINT = "HistoryPoint";
        public const string MODULE_MEMBERCARDTYPE = "MemberCardType";
        public const string MODULE_REPAYPOINTS = "RepayPoints";


    }
}
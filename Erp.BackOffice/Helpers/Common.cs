namespace Erp.BackOffice.Helpers
{
    using Erp.Domain;
    using Erp.Domain.Repositories;
    using Erp.Domain.Sale;
    using Erp.Domain.Sale.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Erp.Domain.Entities;
    using System.Linq;
    using Erp.BackOffice.Models;
    using WebMatrix.WebData;
    using System.Web;
    using System.Configuration;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using System.Data;
    using System.ComponentModel;
    using Erp.Domain.Account.Repositories;
    using Erp.Domain.Account;
    using System.Globalization;
    using Domain.Interfaces;

    //using StackExchange.Redis;

    public class Common
    {
        //public static ConnectionMultiplexer ConnectionMultiplexer = null;

        #region function datetime
        public static string ConvertToDateRange(ref DateTime StartDate, ref DateTime EndDate, bool single, int year, int month, int quarter)
        {
            string str = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;

            //Nếu là tháng/năm
            if (single)
            {
                StartDate = new DateTime(year, month, 1);
                EndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);

                str = string.Format("Tháng {0} năm {1}", month < 10 ? "0" + month.ToString() : month.ToString(), year);
            }
            else //Ngược lại là quý
            {
                switch (quarter)
                {
                    case 1:
                        StartDate = new DateTime(year, 1, 1);
                        EndDate = new DateTime(year, 3, DateTime.DaysInMonth(year, 3), 23, 59, 59);
                        str = string.Format("Quý I năm {0}", year);
                        break;
                    case 2:
                        StartDate = new DateTime(year, 4, 1);
                        EndDate = new DateTime(year, 6, DateTime.DaysInMonth(year, 6), 23, 59, 59);
                        str = string.Format("Quý II năm {0}", year);
                        break;
                    case 3:
                        StartDate = new DateTime(year, 7, 1);
                        EndDate = new DateTime(year, 9, DateTime.DaysInMonth(year, 9), 23, 59, 59);
                        str = string.Format("Quý III năm {0}", year);
                        break;
                    case 4:
                        StartDate = new DateTime(year, 10, 1);
                        EndDate = new DateTime(year, 12, DateTime.DaysInMonth(year, 12), 23, 59, 59);
                        str = string.Format("Quý IV năm {0}", year);
                        break;
                }
            }

            return str;
        }

        public static string FormatDateTime(object value)
        {
            return Convert.ToDateTime(value).ToString("HH:mm - dd/MM/yyyy");
        }

        public static double CalculateTwoDates(DateTime start, DateTime end, string valueGet = "days")
        {
            TimeSpan subtractValue = end.Subtract(start);
            //DateTime diff1 = e.Subtract(subtractValue);
            double returnValue = 0;

            switch (valueGet)
            {
                case "days":
                    returnValue = subtractValue.Days;
                    break;
                case "totaldays":
                    returnValue = subtractValue.TotalDays;
                    break;
                case "houses":
                    returnValue = subtractValue.Hours;
                    break;
                case "totalhouses":
                    returnValue = subtractValue.TotalHours;
                    break;
                case "minutes":
                    returnValue = subtractValue.Minutes;
                    break;
                case "totalminutes":
                    returnValue = subtractValue.TotalMinutes;
                    break;
                case "seconds":
                    returnValue = subtractValue.Seconds;
                    break;
                case "totalseconds":
                    returnValue = subtractValue.TotalSeconds;
                    break;
                case "ticks":
                    returnValue = subtractValue.Ticks;
                    break;
                default:
                    returnValue = subtractValue.Days;
                    break;
            }

            return returnValue;

        }
        public static string ConvertDayToYearMonthWeekDays(object value)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                int day = Int32.Parse(value.ToString());
                int year, month, week, days;
                const int DAYSINWEEK = 7, MONTHINYEAR = 30;

                year = day / 365;
                month = (day % 365) / MONTHINYEAR;
                week = ((day % 365) % MONTHINYEAR) / DAYSINWEEK;
                days = ((day % 365) % MONTHINYEAR) % DAYSINWEEK;

                if (year > 0) {
                    result.Append(year + " năm ");
                }
                if (month > 0) {
                    result.Append(month + " tháng ");
                }
                if (week > 0) {
                    result.Append(week + " tuần ");
                }
                if (days > 0) {
                    result.Append(days + " ngày");
                }
                else {
                    result.Append(0 + " ngày");
                }
            }
            catch {
                return "0";
            }
            return result.ToString();
        }
        public static void ParseBetweenDate(string StartDate, string EndDate, ref string s_startDate, ref string s_endDate, bool IsWeek = false)
        {
            DateTime d_startDate = new DateTime();
            DateTime d_endDate = new DateTime();
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                if (DateTime.TryParseExact(StartDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
                {
                    if (DateTime.TryParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                    {
                        d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    }
                }
            }
            else
            {
                if (IsWeek)
                {
                    d_startDate = Helpers.Common.GetFirstDayOfWeek(DateTime.Now);
                    d_endDate = d_startDate.AddDays(7);
                }
                else
                {
                    d_startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    d_endDate = d_startDate.AddMonths(1).AddDays(-1);
                }
            }
            s_startDate = d_startDate.ToString("yyyy-MM-dd HH:mm:ss");
            s_endDate = d_endDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            return GetFirstDateOfWeek(dayInWeek, defaultCultureInfo);
        }
        private static DateTime GetFirstDateOfWeek(DateTime dayInWeek, CultureInfo defaultCultureInfo)
        {
            DayOfWeek firstDay = defaultCultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
        #endregion

        public static string getAllAddress(string p, string d, string w, string a)
        {
            var address = string.Format("{0}{1}{2}{3}"
                , !string.IsNullOrEmpty(a) ? a + ", " : ""
                , !string.IsNullOrEmpty(w) ? w + ", " : ""
                , !string.IsNullOrEmpty(d) ? d + ", " : ""
                , !string.IsNullOrEmpty(p) ? p : ""
                );
            return address;
        }
        public static List<RequestInfo> ListRequest
        {
            get
            {
                var ListRequest = HttpContext.Current.Application["ListRequest"] as List<RequestInfo>;
                return ListRequest;
            }
            set
            {
                HttpContext.Current.Application["ListRequest"] = value;
            }
        }
        public static List<Erp.BackOffice.Areas.Administration.Models.AddOtherUserImportViewModel> ListOtherUser { get; set; }
        public static void WriteEventLog(String logData)
        {
            if (!EventLog.SourceExists("ErpPlus"))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("ErpPlus", "ErpPlusLog");
                Console.WriteLine("CreatedEventSource");
                Console.WriteLine("Exiting, execute the application a second time to use the source.");
                // The source is created.  Exit the application to allow it to be registered.
                return;
            }

            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = "ErpPlus";

            // Write an informational entry to the event log.    
            myLog.WriteEntry(logData);
        }

        public static string StripHTML(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string PhanCachHangNgan(object str)
        {
            try
            {
                if (Convert.ToInt32(str) >= 1000)
                    return Convert.ToInt32(str).ToString("0,000").Replace(",", ".");
                else
                    return str.ToString();
            }
            catch { }
            return "";
        }
        public static string PhanCachHangNgan2(object str)
        {
            try
            {
                if (Convert.ToDecimal(str) >= 1000)
                    return Convert.ToDecimal(str).ToString("0,000").Replace(",", ".");
                else
                    return str.ToString();
            }
            catch { }
            return "";
        }

        public static Erp.Domain.Entities.vwUsers CurrentUser
        {

            get
            {
                var currentUser = new vwUsers();
                if (HttpContext.Current.Session == null)
                {
                    IUserRepository userRepository = DependencyResolver.Current.GetService<IUserRepository>();
                    currentUser = userRepository.GetvwUserById(WebSecurity.CurrentUserId);
                    return currentUser;
                }
                if (HttpContext.Current.Session["CurrentUser"] != null)
                {
                    currentUser = HttpContext.Current.Session["CurrentUser"] as vwUsers;
                    return currentUser;
                }
                else
                {
                    IUserRepository userRepository = DependencyResolver.Current.GetService<IUserRepository>();
                    currentUser = userRepository.GetvwUserById(WebSecurity.CurrentUserId);
                    if (currentUser != null)
                    {
                        HttpContext.Current.Session["CurrentUser"] = currentUser;
                    }
                    return currentUser;
                }
            }

            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
            //get
            //{
            //    var ListRequest = HttpContext.Current.Application["ListRequest"] as List<RequestInfo>;
            //    if (ListRequest != null)
            //    {
            //        var user = ListRequest.Where(item => item.User != null && item.User.UserName == WebSecurity.CurrentUserName)
            //            .Select(item => item.User).FirstOrDefault();
            //        return user;
            //    }

            //    return null;
            //}
            //set
            //{
            //    var ListRequest = HttpContext.Current.Application["ListRequest"] as List<RequestInfo>;
            //    if (ListRequest != null)
            //    {
            //        var r = ListRequest.Where(item => item.User != null && item.User.UserName == WebSecurity.CurrentUserName).FirstOrDefault();
            //        if (r != null)
            //            r.User = value;
            //    }
            //}
        }

        public static void TrackRequest()
        {
            if (ListRequest == null)
                ListRequest = new List<RequestInfo>();
            var ip = HttpContext.Current.Request.UserHostAddress;
            var requestInfo = ListRequest.Where(item => item.IP == ip).FirstOrDefault();

            //Nếu đã có request với ip, thì kiểm tra login, và get currentUser
            if (requestInfo != null && WebSecurity.IsAuthenticated && requestInfo.User != null && requestInfo.User.UserName != WebSecurity.CurrentUserName.ToLower())
            {
                requestInfo = null;
            }

            if (requestInfo == null)
            {
                requestInfo = new RequestInfo();
                requestInfo.IP = ip;
                requestInfo.FirstDate = DateTime.Now;
                requestInfo.LastDate = DateTime.Now;
                requestInfo.IsLocked = false;
                requestInfo.AddUrl(HttpContext.Current.Request.RawUrl);

                ListRequest.Add(requestInfo);
            }

            //Get currentUser
            if (WebSecurity.IsAuthenticated && requestInfo.User == null)
            {
                UserRepository userRepository = new UserRepository(new ErpDbContext());
                vwUsers user = userRepository.GetByvwUserName(WebSecurity.CurrentUserName);
                requestInfo.User = user;
            }

            requestInfo.LastDate = DateTime.Now;
            requestInfo.AddUrl(HttpContext.Current.Request.RawUrl);
        }

        public static string Capitalize(string value)
        {
            if (string.IsNullOrEmpty(value))
                value = "";
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLower());
        }

        public static string ChuyenThanhKhongDau(string s)
        {
            if (string.IsNullOrEmpty(s) == true)
                return "";

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }

        public static string ChuyenSoThanhChu(string number)
        {
            string[] strTachPhanSauDauPhay;
            if (number.Contains(".") || number.Contains(","))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSoThanhChu(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSoThanhChu(strTachPhanSauDauPhay[1]));
            }

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "linh ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "năm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return FirstCharToUpper(doc);
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string ChuyenSoThanhChu(int number)
        {
            return ChuyenSoThanhChu(number.ToString());
        }

        public static string GetUserSetting(string key)
        {
            try
            {
                UserSettingRepository userSettingRepository = new UserSettingRepository(new ErpDbContext());

                return userSettingRepository.GetUserSettingByKey(key, WebMatrix.WebData.WebSecurity.CurrentUserId);
            }
            catch { }

            return null;
        }

        //public static string GetCode(string prefix, int value, int lenght = 6)
        //{
        //    var numberStr = value.ToString();
        //    while (numberStr.Length < lenght)
        //    {
        //        numberStr = "0" + numberStr;
        //    }

        //    return prefix + Erp.BackOffice.Helpers.Common.CurrentUser.BranchCode + numberStr;

        //    //var value = GetUserSetting(key);
        //    //[!@#$%^&*(){}|\?><:"",./~`=+_-] //separators
        //    //var result = Regex.Split(value, @"[^a-zA-Z0-9]"); //text:HD_1  => [0]:HD, [1]:1
        //    //var result2 = Regex.Split(value, @"^[a-zA-Z0-9]"); //text:HD_1  => [0]:"", [1]:D_1

        //    //if (string.IsNullOrEmpty(value))
        //    //    return "";

        //    //var onlyNumber = Regex.Replace(value, @"[^0-9]", "");

        //    //int number;
        //    //if (int.TryParse(onlyNumber, out number))
        //    //{
        //    //    number += valueChange;
        //    //    string numberStr = number.ToString();
        //    //    while (numberStr.Length < onlyNumber.Length)
        //    //    {
        //    //        numberStr = "0" + numberStr;
        //    //    }

        //    //    return value.Replace(onlyNumber, numberStr);
        //    //}


        //    //return value;
        //}

        public static string GetOrderNo(string key, string Code = null)
        {
            var sale_auto_outbound = Erp.BackOffice.Helpers.Common.GetSetting("sale_auto_outbound");
            if (sale_auto_outbound == "true")
            {
                string machungtu_prefix = Helpers.Common.GetSetting("prefixOrderNo_" + key);
                string machungtu_stt = Helpers.Common.GetSetting("orderNo_" + key);
                if (string.IsNullOrEmpty(machungtu_stt))
                {
                    machungtu_stt = "1";
                }
                while (machungtu_stt.Length < 4)
                {
                    machungtu_stt = "0" + machungtu_stt;
                }

                return machungtu_prefix + Erp.BackOffice.Helpers.Common.CurrentUser.BranchCode + machungtu_stt;
            }
            else
            {
                var manualOrderNo = Erp.BackOffice.Helpers.Common.GetSetting("manualOrderNo_" + key);
                if (manualOrderNo != null && manualOrderNo == "true" && Code != null)
                {
                    return Code;
                }
                else
                {
                    string machungtu_prefix = Helpers.Common.GetSetting("prefixOrderNo_" + key);
                    string machungtu_stt = Helpers.Common.GetSetting("orderNo_" + key);
                    if (string.IsNullOrEmpty(machungtu_stt))
                    {
                        machungtu_stt = "1";
                    }
                    while (machungtu_stt.Length < 4)
                    {
                        machungtu_stt = "0" + machungtu_stt;
                    }

                    return machungtu_prefix + Erp.BackOffice.Helpers.Common.CurrentUser.BranchCode + machungtu_stt;

                }
            }
        }
        public static void SetOrderNo(string key)
        {
            var sale_auto_outbound = Erp.BackOffice.Helpers.Common.GetSetting("sale_auto_outbound");
            if (sale_auto_outbound == "true")
            {
                string machungtu_stt = Helpers.Common.GetSetting("orderNo_" + key);
                if (!string.IsNullOrEmpty(machungtu_stt))
                {
                    Helpers.Common.SetSetting("orderNo_" + key, (Convert.ToInt32(machungtu_stt) + 1).ToString());
                }
                else
                {
                    Helpers.Common.SetSetting("orderNo_" + key, "2");
                }
            }
            else
            {
                var manualOrderNo = Erp.BackOffice.Helpers.Common.GetSetting("manualOrderNo_" + key);
                if (manualOrderNo == null || manualOrderNo != "true")
                {
                    string machungtu_stt = Helpers.Common.GetSetting("orderNo_" + key);
                    if (!string.IsNullOrEmpty(machungtu_stt))
                    {
                        Helpers.Common.SetSetting("orderNo_" + key, (Convert.ToInt32(machungtu_stt) + 1).ToString());
                    }
                    else
                    {
                        Helpers.Common.SetSetting("orderNo_" + key, "2");
                    }
                }
            }
        }
        public static void SetUserSetting(string key, string value)
        {
            try
            {
                UserSettingRepository userSettingRepository = new UserSettingRepository(new ErpDbContext());

                userSettingRepository.SetUserSettingByKey(key, WebMatrix.WebData.WebSecurity.CurrentUserId, value);
            }
            catch { }
        }

        public static string GetSetting(string key)
        {
            try
            {
                SettingRepository settingRepository = new SettingRepository(new ErpDbContext());

                return settingRepository.GetSettingByKey(key).Value;
            }
            catch { }

            return null;
        }

        public static void SetSetting(string key, string value)
        {
            try
            {
                SettingRepository settingRepository = new SettingRepository(new ErpDbContext());
                var setting = settingRepository.GetSettingByKey(key);
                setting.Value = value;
                settingRepository.Update(setting);
            }
            catch { }
        }
        public static Domain.Entities.Category GetCategoryByValueOrId(string keyField, object value)
        {
            try
            {
                keyField = keyField.ToLower();
                Domain.Entities.Category item = new Category();

                Domain.Repositories.CategoryRepository categoryRepository = new CategoryRepository(new ErpDbContext());
                switch (keyField)
                {
                    case "value":
                        item = categoryRepository.GetAllCategories().Where(x => x.Value == value).FirstOrDefault();
                        break;
                    case "id":
                        int id = -1;
                        if (int.TryParse(value.ToString(), out id))
                        {
                            item = categoryRepository.GetCategoryById(id);
                        }
                        break;
                }
                return item;
            }
            catch { }

            return null;
        }
         
        public static int? GetCountInvoiceOfCustomerById(int id)
        {
            ProductInvoiceRepository productInvoiceRepository = new ProductInvoiceRepository(new ErpSaleDbContext());
            var countInvoice = productInvoiceRepository.GetAllvwInvoiceByCustomer(id).Count();
            return countInvoice;

        }
        public static Domain.Entities.Category GetCategoryByValueCodeOrId(string keyField, string value, string Code)
        {
            try
            {
                keyField = keyField.ToLower();
                Domain.Entities.Category item = new Category();

                Domain.Repositories.CategoryRepository categoryRepository = new CategoryRepository(new ErpDbContext());
                switch (keyField)
                {
                    case "value":
                        item = categoryRepository.GetAllCategories().Where(x => x.Value == value && x.Code == Code).FirstOrDefault();
                        break;
                    case "id":
                        int id = -1;
                        if (int.TryParse(value.ToString(), out id))
                        {
                            item = categoryRepository.GetCategoryById(id);
                        }
                        break;
                }
                return item;
            }
            catch { }

            return null;
        }
        public static string KiemTraTonTaiHinhAnh(string Image, string NameUrlImage, string NoImage)
        {
            //NameUrlImage là cột code trong setting .
            var ImageUrl = "";
            //chọn thay thế ảnh khi không có tên hình trong database.
            switch (NoImage)
            {
                case "product":
                    NoImage = "/assets/css/images/noimage.gif";
                    break;
                case "user":
                    NoImage = "/assets/img/no-avatar.png";
                    break;
                case "service":
                    NoImage = "/assets/css/images/noimage.gif";
                    break;
            }
            //lấy đường dẫn hình ảnh
            var ImagePath = Helpers.Common.GetSetting(NameUrlImage);
            var filepath = System.Web.HttpContext.Current.Server.MapPath("~" + ImagePath);
            //nếu có hình ảnh
            if (!string.IsNullOrEmpty(Image))
            {
                ImageUrl = ImagePath + Image;
                //kiểm tra hình ảnh có tồn tại hay không..
                if (!System.IO.File.Exists(filepath + Image))
                {
                    ImageUrl = NoImage;
                }
                else
                {
                    ImageUrl = ImagePath + Image;
                }
            }
            else
                //không có ảnh
                if (string.IsNullOrEmpty(Image))
            {
                ImageUrl = NoImage;
            }
            return ImageUrl;
        }
        public static string GetSettingbyNote(string key)
        {
            try
            {
                SettingRepository settingRepository = new SettingRepository(new ErpDbContext());

                return settingRepository.GetSettingByKey(key).Note;
            }
            catch { }

            return null;
        }

        #region - get select list
        public static SelectList GetSelectList_Category(string sCode, object SelectedValue, string sValueField, bool hasItemEmpty = true)
        {
            Domain.Repositories.CategoryRepository categoryRepository = new CategoryRepository(new ErpDbContext());

            var selectListItems = new List<SelectListItem>();

            if (hasItemEmpty)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = App_GlobalResources.Wording.Empty;
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
                    if (sValueField != null && sValueField.ToLower() == "name")
                        item.Value = i.Name;
                    else if (sValueField != null && sValueField.ToLower() == "value")
                        item.Value = i.Value;
                    else
                        item.Value = i.Id.ToString();

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }
        public static SelectList GetSelectList_Category(int? parentId, object SelectedValue, string sValueField, bool hasItemEmpty = true)
        {
            Domain.Repositories.CategoryRepository categoryRepository = new CategoryRepository(new ErpDbContext());

            var selectListItems = new List<SelectListItem>();

            if (hasItemEmpty)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = App_GlobalResources.Wording.Empty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var q = categoryRepository.GetCategoryByParentId(parentId.Value);

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    if (sValueField != null && sValueField.ToLower() == "name")
                        item.Value = i.Name;
                    else if (sValueField != null && sValueField.ToLower() == "value")
                        item.Value = i.Value;
                    else
                        item.Value = i.Id.ToString();

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }
        public static SelectList GetSelectList_Location(string sParentId, object sValue)
        {
            Domain.Repositories.LocationRepository locationRepository = new LocationRepository(new ErpDbContext());

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
                    item.Text = Capitalize(i.Name.ToLower());
                    item.Value = i.Id;

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

        #endregion

        public static string GetWebConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static bool SendEmailAttachment(string emailFrom, string emailPasswordFrom, string SentTo, string subject, string body, string cc, string bcc, string displayName, string filePath = null, string fileNameDisplayHasExtention = null)
        {

            //string from = System.Configuration.ConfigurationManager.AppSettings["Email"];
            //string password = System.Configuration.ConfigurationManager.AppSettings["Email_Password"];
            string port = Erp.BackOffice.Helpers.Common.GetSetting("Port");
            string ssl = Erp.BackOffice.Helpers.Common.GetSetting("SSL");
            string smtp = Erp.BackOffice.Helpers.Common.GetSetting("SMTP");

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(emailFrom, displayName);
            msg.To.Add(SentTo);

            if (string.IsNullOrEmpty(cc) == false)
                msg.CC.Add(cc);

            if (string.IsNullOrEmpty(bcc) == false)
                msg.Bcc.Add(bcc);

            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            if (string.IsNullOrEmpty(filePath) == false)
            {
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(filePath, new ContentType(MediaTypeNames.Application.Octet));
                //attachment.TransferEncoding = System.Net.Mime.TransferEncoding.;
                attachment.ContentDisposition.FileName = fileNameDisplayHasExtention;
                //attachment.ContentDisposition.Size = attachment.ContentStream.Length;
                msg.Attachments.Add(attachment);
            }

            SmtpClient client = new SmtpClient();

            client.Host = smtp;
            client.Port = Convert.ToInt32(port);
            client.UseDefaultCredentials = false;

            if (ssl.ToLower() == "true")
                client.EnableSsl = true;
            else
                client.EnableSsl = false;

            client.Credentials = new NetworkCredential(emailFrom, emailPasswordFrom);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;


            try
            {
                client.Send(msg);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static string ChuyenSoThanhChu_2(string number)
        {
            string[] strTachPhanSauDauPhay;
            if (number.Contains(".") || number.Contains(","))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSoThanhChu_2(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSoThanhChu_2(strTachPhanSauDauPhay[1]));
            }

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "năm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            if (!string.IsNullOrEmpty(number))
            {
                var ky_tu_cuoi = number[len - 1].ToString();
                if (ky_tu_cuoi == "0")
                {
                    doc = doc + " đồng chẵn";
                }
                else
                {
                    doc = doc + " đồng";
                }
            }

            return doc;
        }

        public static string ChuyenSoThanhChu_2(int number)
        {
            return ChuyenSoThanhChu_2(number.ToString());
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
        //hàm convert từ byte sang kilobyte, MB, GB
        public static string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
        public static string GetCodebyBranch(string prefix, int value, string BranchCode, int lenght = 6)
        {
            var numberStr = value.ToString();
            while (numberStr.Length < lenght)
            {
                numberStr = "0" + numberStr;
            }

            return prefix + BranchCode + numberStr;
        }

        public static bool KiemTraNgaySuaChungTu(DateTime CreatedDate)
        {
            var limit_daterange_for_update_data = GetSetting("limit_daterange_for_update_data");
            if (string.IsNullOrEmpty(limit_daterange_for_update_data))
            {
                limit_daterange_for_update_data = "0";
            }

            if (CreatedDate.AddDays(Convert.ToInt32(limit_daterange_for_update_data)) > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetContentTextInElement(string text)
        {
            string decode = System.Web.HttpUtility.HtmlDecode(text);
            Regex objRegExp = new Regex("<(.|\n)+?>");
            string replace = objRegExp.Replace(decode, "");
            return replace.Trim("\t\r\n ".ToCharArray()).Replace("\">", "");
        }
        public static string RenderPartialViewToString<T>(Controller controller, string viewName, T model, bool examAnswer = false)
        {
            controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.ToString();
            }
        }
        public static void DienDuLieu<T>(ref string content, T model, bool isThousandsFormat = true, bool showTime = false, string sConlumnName = null) where T : class, new()
        {
            if (model == null)
                model = new T();
            var pi = model.GetType().GetProperties();

            foreach (var p in pi)
            {
                var result = p.GetValue(model);
                var sDisplayText = "";
                if (result != null)
                {
                    switch (result.GetType().FullName)
                    {
                        case "System.DateTime":
                        if ((DateTime)result != DateTime.MinValue)
                        {
                            if (showTime)
                            {
                                sDisplayText = ((DateTime)result).ToString("dd/MM/yyyy HH:mm");
                            }
                            else
                            {
                                sDisplayText = ((DateTime)result).ToString("dd/MM/yyyy");
                            }
                        }
                        break;
                        case "System.Boolean":
                        if (sConlumnName == "Gender")
                        {
                            sDisplayText = result == null ? "" : (((bool)result) ? "Nữ" : "Nam");
                        }
                        else
                        {
                            bool bChecked = ((bool)result);
                            string inputUI = "<label><input type=\"checkbox\" " + (bChecked ? "checked=\"checked\"" : "") + " class=\"ace\" disabled=\"disabled\" /><span class=\"lbl\"></span></label>";
                            sDisplayText = inputUI;
                        }
                        break;
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Float":
                        case "System.Double":
                        case "System.Decimal":
                        if (isThousandsFormat)
                        {
                            sDisplayText = Helpers.Common.PhanCachHangNgan(double.Parse(result.ToString()));
                        }
                        else
                        {
                            sDisplayText = result.ToString();
                        }
                        break;
                        default:
                        sDisplayText = result.ToString();
                        break;
                    }
                }
                content = content.Replace("{" + p.Name + "}", sDisplayText);
            }
        }
    }

    public static class CommonSatic
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }

    public enum InfoDetailCustomer{
        Details, TransitionHistory, Contact, InfoCar, RePayHistory
    }

}



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using WiseX.Data;
using WiseX.Helpers;
using WiseX.Models;
using WiseX.Services;
using WiseX.ViewModels.Account;
using WiseX.ViewModels.Admin;
using WiseX.ViewModels.Home;

namespace WiseX.Controllers
{
    [Authorize]
    [SessionTimeout]
    public class HomeController : Controller
    {
        private readonly AdminService _adminService;
        private readonly HomeService _homeService;
        private readonly MenuUtils _menuUtils;
        private readonly CommonService _commonService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ReportService _reportService;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _adminService = new AdminService(applicationDbContext);
            _homeService = new HomeService(applicationDbContext);
            _commonService = new CommonService(applicationDbContext);
            _menuUtils = new MenuUtils(applicationDbContext);
            _hostingEnvironment = hostingEnvironment;
            _reportService = new ReportService(applicationDbContext);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //To set menu
            await _menuUtils.SetMenu(HttpContext.Session);
            var userId = "";
            var RoleId = "";
            var RoleName = "";
            int ProjectID = 0;
            var ProjectName = "";
            int ResourceID = 0;
            try
            {
                userId = HttpContext.Session.GetString("UserID");
                RoleId = HttpContext.Session.GetString("RoleID");
                RoleName = HttpContext.Session.GetString("RoleName");

                ProjectName = string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ProjectName")) ? "" : HttpContext.Session.GetString("ProjectName");
                if (HttpContext.Session.GetString("ProjectID") != null)
                    ProjectID = Int32.Parse(HttpContext.Session.GetString("ProjectID"));
                if (HttpContext.Session.GetString("ResourceID") != null)
                    ResourceID = Convert.ToInt32(HttpContext.Session.GetString("ResourceID"));
            }
            catch (Exception Ex)
            {
            }
            if (userId == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            IList<NotificationDetails> NotificationDetails = new List<NotificationDetails>();
            NotificationDetails = await _commonService.GetNotificationDetailsList(userId);

            //Save List in Session
            var str = JsonConvert.SerializeObject(NotificationDetails);
            HttpContext.Session.SetString("my-message", str);


            //Dashboard details
            DataSet data = new DataSet();
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.ChartBoxes = await _homeService.GetBoxes();
            string UserID = HttpContext.Session.GetString("UserID");
            //homeViewModel.ChartBoxPropertiesList = await _homeService.GetBoxesList("0", UserID);
            if (homeViewModel.ChartBoxes == null)
                homeViewModel.ChartBoxes = new ChartProperties();
            return View(homeViewModel);
        }
        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult AccessDenied()
        {
            return View("Unauthorized");
        }

        public async Task<ActionResult> GetDashboardList(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxPropertiesList = await _homeService.GetBoxesList(BoxName, UserID);

            return PartialView("_BoxesList", homeViewModel);
        }

        public async Task<ActionResult> GetDashboardListForB5(string BoxName)
        {
            // HomeService HS = new HomeService();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            var list= await _homeService.GetBoxesForB5Data(Start, Length, BoxName, UserID);
            return Json(new { data = list });
        }

        public async Task<JsonResult> GetDashboardListForGlobalReportB7(string BoxName)
        {
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");

            var list = await _homeService.GetDashboardListCommonDetails(Start, Length, BoxName, UserID);

            return Json(new { data = list });
        }
        public async Task<JsonResult> GetDashboardListForAllFacilityReportB8(string BoxName)
        {
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");

            var list = await _homeService.GetDashboardListCommonDetails(Start, Length, BoxName, UserID);

            return Json(new { data = list });
        }

        public async Task<JsonResult> GetDashboardListForClientAgingWithNotesB6(string BoxName)
        {
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");

            var list = await _homeService.GetDashboardListCommonDetails(Start, Length, BoxName, UserID);

            return Json(new { data = list });
        }
        public async Task<JsonResult> GetDashboardListForClientAgingWithNotesB5(string BoxName)
        {
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");

            var list = await _homeService.GetDashboardListCommonDetails(Start, Length, BoxName, UserID);

            return Json(new { data = list });
        }
        public async Task<JsonResult> GetDashboardListForClientAgingWithNotesB13(string BoxName)
        {
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");

            var list = await _homeService.GetDashboardListCommonDetails(Start, Length, BoxName, UserID);

            return Json(new { data = list });
        }
        public async Task<ActionResult> GetDashboardList1(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.NonEmergencyFacilityBalanceLoad = await _homeService.GetBoxesNEFacilityList(Start, Length, BoxName, UserID);
            int totalrows = 0;// (homeViewModel.NonEmergencyFacilityBalanceLoad.Count > 0 ? homeViewModel.NonEmergencyFacilityBalanceLoad.First().TotalCount : 0);
            int totalRowsAfterFiltering = 0;// (homeViewModel.NonEmergencyFacilityBalanceLoad.Count > 0 ? homeViewModel.NonEmergencyFacilityBalanceLoad.First().TotalCount : 0);

            return Json(new { data = homeViewModel.NonEmergencyFacilityBalanceLoad, draw = HttpContext.Request.Form["draw"] , recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

               //homeViewModel.ChartBoxPropertiesLoad = await _homeService.GetBoxesList1(Start, Length, BoxName, UserID);
               //     int totalrows = (homeViewModel.ChartBoxPropertiesLoad.Count > 0 ? homeViewModel.ChartBoxPropertiesLoad.First().TotalCount : 0);
               //     int totalRowsAfterFiltering = (homeViewModel.ChartBoxPropertiesLoad.Count > 0 ? homeViewModel.ChartBoxPropertiesLoad.First().TotalCount : 0);

                //     return Json(new { data = homeViewModel.ChartBoxPropertiesLoad, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            }

        public async Task<ActionResult> GetFacilityDashboardList(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxFacilityPropertiesLoad = await _homeService.GetBoxesFacilityList(Start, Length, BoxName, UserID);
            int totalrows = (homeViewModel.ChartBoxFacilityPropertiesLoad.Count > 0 ? homeViewModel.ChartBoxFacilityPropertiesLoad.First().TotalCount : 0);
            int totalRowsAfterFiltering = (homeViewModel.ChartBoxFacilityPropertiesLoad.Count > 0 ? homeViewModel.ChartBoxFacilityPropertiesLoad.First().TotalCount : 0);

            return Json(new { data = homeViewModel.ChartBoxFacilityPropertiesLoad, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }


        //added by aajit on 23/10/2020
        [HttpPost]
        public async Task<IActionResult> DownloadDashboardData(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxPropertiesLoad = await _homeService.GetBoxesList1(Start, Length, BoxName, UserID);
            var data = homeViewModel.ChartBoxPropertiesLoad.Select(x => new { x.CompanyID, x.CompanyName, x.FacilityContractName, x.ResidencyCode, x.ContractEndDate, x.AggrementType, x.AccountExecutive }).ToList();//,x.Status
            string sFileName = @"DashboardData.xlsx";
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string UrlBase = _configuration["AppSettings:AppURL"].ToString() + sFileName;
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
                package.Dispose();
                worksheet.Dispose();

            }
            return Json(UrlBase);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadDashboardFacilityData(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxFacilityPropertiesLoad = await _homeService.GetBoxesFacilityList(Start, Length, BoxName, UserID);
            var data = homeViewModel.ChartBoxFacilityPropertiesLoad.Select(x => new { x.Name, x.ContactName, x.EmailAddress, x.Phone, x.BillType, x.Instructions, x.InvoicePreference, x.InvoiceSchedule }).ToList();//,x.Status
            string sFileName = @"DashboardData.xlsx";
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string UrlBase = _configuration["AppSettings:AppURL"].ToString() + sFileName;
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
                package.Dispose();
                worksheet.Dispose();

            }
            return Json(UrlBase);
        }

        public async Task<IActionResult> NotesManagement(string facilityName)
        {
            ViewBag.FacilityName = facilityName;

            int Start = 0;
            int Length = 0;
            string BoxName = "NMF";            
            try
            {
                HomeViewModel homeViewModel = new HomeViewModel();
                homeViewModel.NotesManagementForm = await _homeService.GetNotesManagementFormDetails(facilityName, Start, Length, BoxName);
                return View(homeViewModel);
            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
            
        }
        public async Task<JsonResult> GetDashboardNoteManagementDetailList(string facilityName)
        {
            
            string UserID = HttpContext.Session.GetString("UserID");

            var list = await _homeService.GetDashboardNoteManagementDetails(facilityName);

            return Json(new { data = list });
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCMS.ViewModels.NursingHomes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WiseX.Controllers;
using WiseX.Data;
using WiseX.Helpers;
using WiseX.Models;
using System.Linq.Dynamic.Core;
using WiseX.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using FCMS.Helpers;
using FCMS.ViewModels.Admin;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;

namespace FCMS.Controllers
{
    [SessionTimeout]
    public class NursingHomesController : Controller
    {
        String StorageRoot = null;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;

        private readonly MenuUtils _menuUtils;
        int totalrows, totalRowsAfterFiltering;//, Start, Length,  totalRowsAfterFiltering, ContractId;

        public NursingHomesController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            //Intialize

            _applicationDbContext = applicationDbContext;
            _adminService = new AdminService(applicationDbContext);
            _commonService = new CommonService(_applicationDbContext);
            _menuUtils = new MenuUtils(applicationDbContext);

            _configuration = configuration;


            _hostingEnvironment = hostingEnvironment;
            this.StorageRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:ContractUploadPath"].ToString());
        }

        public async Task<IActionResult> NursingHomes()
        {
            NursingHomes model = new NursingHomes();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                model.NursingHomeDetails = new NursingHomeDetails();
                model.NursingHomeList = await _commonService.GetNursingHomes(0);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.NursingHomeContractDetailsList = new List<NursingHomeContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();

                model.NursingHomesContacts = new NursingHomesContacts();
                model.NursingHomesContactsList = new List<NursingHomesContactsList>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchNursingHome(string Prefix)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                model.SearchNursingHomeList = await _commonService.GetSearchNursingHome(Prefix);
            }
            catch (Exception Ex) { }
            return Json(model.SearchNursingHomeList);
        }

        [HttpPost]
        public async Task<IActionResult> NursingHomeDetails(int Id)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);

                model.NursingHomeList = await _commonService.GetNursingHomes(Id);
                NursingHomeDetails res = new NursingHomeDetails();
                if (model.NursingHomeList.Count > 0)
                {
                    foreach (var item in model.NursingHomeList)
                    {
                        if (item.ID == Id)
                        {
                            res.ID = item.ID;
                            //res.PayorID = item.PayorID;
                            res.Name = item.Name;
                            res.Address = item.Address;
                            res.StateID = item.StateID;
                            res.CityID = item.CityID;
                            res.ZipCode = item.ZipCode;
                            res.Phone = item.Phone;
                            res.FaxNumber = item.FaxNumber;
                            res.EmailID = item.EmailID;
                            res.ContactName = item.ContactName;
                            res.Department = item.Department;
                            res.ClientSpecific = item.ClientSpecific;
                            res.ClientSpecificName = item.ClientSpecificName;
                            res.ContractPricing = item.ContractPricing;
                            res.InvoiceP = item.InvoiceP;
                            res.InvoiceSchedule = item.InvoiceSchedule;
                            res.LetterIncluded = item.LetterIncluded;
                            res.LetterPath = item.LetterPath;
                            res.W9 = item.W9;
                            res.W9Path = item.W9Path;
                            res.VendorLetter = item.VendorLetter;
                            res.VendorPath = item.VendorPath;
                            res.InvoiceTemplate = item.InvoiceTemplate;
                            res.InvoiceTemplatePath = item.InvoiceTemplatePath;
                            res.LastUpdatedBy = item.LastUpdatedBy;
                            res.LastUpdatedDate = item.LastUpdatedDate;
                            res.CityName = item.CityName;
                            res.StateName = item.StateName;
                            res.IsAlert = item.IsAlert;
                            res.ContactsRefID = item.ContactsRefID;
                            res.BillType = item.BillType;
                            res.Instructions = item.Instructions;


                        }
                    }
                    model.NursingHomeContractDetailsList = await _commonService.GetNursingHomeContractDetailsList(Id);
                    model.NursingHomeDetails = res;
                }
            }
            catch (Exception ex)
            { }
            return PartialView("_NursingHomeSearch", model);
        }

        [HttpPost]
        public async Task<ActionResult> NursingHomeLog(int nursingHomeId)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                model.NursingHomeLogList = await _commonService.GetNursingHomeLogList(nursingHomeId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.NursingHomeLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> NursingHomeNotesDetails(int NursingHomeId)
        {
            NursingHomes model = new NursingHomes();
            try
            {

                model.NursingHomeNotesList = await _commonService.GetNursingHomeNotes(NursingHomeId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.NursingHomeNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNursingHomeNotes(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);


            int NursingHomeId = await _commonService.DeleteNursingHomeNotes(Id, userId);

            return Json(new { NursingHomeId = NursingHomeId });
        }
        [HttpPost]
        public async Task<ActionResult> EditNotes(int Id)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                model.NursingHomeNotesList = await _commonService.GetEditNursingHomeNote(Id);
            }
            catch (Exception ex) { }
            return Json(new { data = model.NursingHomeNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveNursingHomeNotes(int Id, int NursingHomeId, string Notes)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateNursingHomeNotesDetails(Id, NursingHomeId, Notes, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveNursingHomes([FromBody] NursingHomeDetails nursingHomeDetails)
        {
            string retMessage = string.Empty;
            NursingHomes model = new NursingHomes();
            var userId = HttpContext.Session.GetString("UserID");
            int nursingHomeId = 0;

            NursingHomeOutputID nursingHomeOutputID = new NursingHomeOutputID();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                // nursingHomeDetails.LastUpdatedBy = HttpContext.Session.GetString("UserID");
                model.NursingHomeDetails = nursingHomeDetails;
                nursingHomeOutputID = await _adminService.SaveNursingHomes(model, userId);

                nursingHomeId = nursingHomeOutputID.ID;

            }
            catch (Exception ex)
            {

            }
            return Json(new { NursingHomeId = nursingHomeId });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospialFiles(int NursingHomeId, IFormFile LetterCopy, IFormFile W9Copy, IFormFile VendorLetterCopy, IFormFile InvoiceTemplateCopy)
        {
            string strMessage = "";
            var userId = HttpContext.Session.GetString("UserID");
            NursingHomes nursingHomes = new NursingHomes();
            NursingHomeDetails model = new NursingHomeDetails();

            string LetterFileName = "";
            string W9FileName = "";
            string VendorLetterFileName = "";
            string InvoiceTemplateFileName = "";

            string Letterpath = "";
            string W9path = "";
            string VendorLetterpath = "";
            string InvoiceTemplatepath = "";

            try
            {
                if (LetterCopy != null)
                {
                    LetterFileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_LetterIncluded_" + LetterCopy.FileName;
                    string Direc_FileName = NursingHomeId + "\\" + LetterFileName;

                    Letterpath = Path.Combine(StorageRoot + "NursingHomes", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "NursingHomes" + "\\" + NursingHomeId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (Directory.Exists(fullPaths))
                    {
                        using (var stream = new FileStream(Letterpath, FileMode.Create))
                        {
                            await LetterCopy.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                if (W9Copy != null)
                {
                    W9FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_W9_" + W9Copy.FileName;
                    string Direc_FileName = NursingHomeId + "\\" + W9FileName;

                    W9path = Path.Combine(StorageRoot + "NursingHomes", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "NursingHomes" + "\\" + NursingHomeId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (Directory.Exists(fullPaths))
                    {
                        using (var stream = new FileStream(W9path, FileMode.Create))
                        {
                            await W9Copy.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                if (VendorLetterCopy != null)
                {
                    VendorLetterFileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_VendorLetter_" + VendorLetterCopy.FileName;
                    string Direc_FileName = NursingHomeId + "\\" + VendorLetterFileName;

                    VendorLetterpath = Path.Combine(StorageRoot + "NursingHomes", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "NursingHomes" + "\\" + NursingHomeId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (Directory.Exists(fullPaths))
                    {
                        using (var stream = new FileStream(VendorLetterpath, FileMode.Create))
                        {
                            await VendorLetterCopy.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                if (InvoiceTemplateCopy != null)
                {
                    InvoiceTemplateFileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_InvoiceTemplate_" + InvoiceTemplateCopy.FileName;
                    string Direc_FileName = NursingHomeId + "\\" + InvoiceTemplateFileName;

                    InvoiceTemplatepath = Path.Combine(StorageRoot + "NursingHomes", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "NursingHomes" + "\\" + NursingHomeId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (Directory.Exists(fullPaths))
                    {
                        using (var stream = new FileStream(InvoiceTemplatepath, FileMode.Create))
                        {
                            await InvoiceTemplateCopy.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                await _adminService.UpdateNursingHomeFilePath(NursingHomeId, LetterFileName, W9FileName, VendorLetterFileName, InvoiceTemplateFileName);

                nursingHomes.NursingHomeList = await _commonService.GetNursingHomes(0);

                strMessage = "Success";
            }
            catch (Exception ex)
            {

            }
            return Json(new { strMessage });
        }

        public async Task<IActionResult> ViewFiles(string fileName)
        {
            return GetFileFromPath(fileName);
        }

        //public FileStreamResult GetFileFromPath(string fileName)
        //{
        //    var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "NursingHomes\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
        //    string Extn = Path.GetExtension(fileName.Trim());

        //    switch (Extn)
        //    {
        //        case ".pdf":
        //            Response.ContentType = "application/pdf";
        //            break;
        //        case ".doc":
        //            Response.ContentType = "application/msword";
        //            break;
        //        case ".docx":
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //            break;
        //        case ".xls":
        //            Response.ContentType = "application/vnd.ms-excel";
        //            break;
        //        case ".xlsx":
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            break;
        //        case ".csv":
        //            Response.ContentType = "application/text";
        //            break;
        //    }

        //    var cd = new ContentDispositionHeaderValue("inline")
        //    {
        //        FileNameStar = Path.GetFileName(fileName)
        //    };
        //    Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

        //    var fsResult = File(fileStream, Response.ContentType);
        //    return fsResult;
        //}
        public IActionResult GetFileFromPath(string fileName)
        {
            string filePath = Path.Combine(
                _configuration["AppSettings:ContractUploadPath"],
                "NursingHomes",
                HttpUtility.HtmlDecode(fileName));

            if (!System.IO.File.Exists(filePath))
            {
                string redirectUrl = Url.Action("NursingHomes", "NursingHomes");

                return Content($@"
                <script>
                    alert('File not found.');
                    window.location.href = '{redirectUrl}';
                </script>",
                            "text/html");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            string contentType = "application/octet-stream";

            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    contentType = "text/csv";
                    break;
            }

            var cd = new ContentDispositionHeaderValue("inline")
            {
                FileNameStar = Path.GetFileName(fileName)
            };

            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

            return File(fileStream, contentType);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditNursingHomes(int Id)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");

                model.NursingHomeList = await _commonService.GetNursingHomes(Id);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.NursingHomeContractDetailsList = new List<NursingHomeContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();
                model.NursingHomesContacts = new NursingHomesContacts();

                NursingHomeDetails res = new NursingHomeDetails();

                if (model.NursingHomeList.Count > 0)
                {
                    foreach (var item in model.NursingHomeList)
                    {
                        if (item.ID == Id)
                        {
                            res.ID = item.ID;
                            //res.PayorID = item.PayorID;
                            res.Name = item.Name;
                            res.Address = item.Address;
                            res.StateID = item.StateID;
                            res.CityID = item.CityID;
                            res.ZipCode = item.ZipCode;
                            res.Phone = item.Phone;
                            res.FaxNumber = item.FaxNumber;
                            res.EmailID = item.EmailID;
                            res.ContactName = item.ContactName;
                            res.Department = item.Department;
                            res.ClientSpecific = item.ClientSpecific;
                            res.ClientSpecificName = item.ClientSpecificName;
                            res.ContractPricing = item.ContractPricing;
                            res.InvoiceP = item.InvoiceP;
                            res.InvoiceSchedule = item.InvoiceSchedule;
                            res.LetterIncluded = item.LetterIncluded;
                            res.LetterPath = item.LetterPath;
                            res.W9 = item.W9;
                            res.W9Path = item.W9Path;
                            res.VendorLetter = item.VendorLetter;
                            res.VendorPath = item.VendorPath;
                            res.InvoiceTemplate = item.InvoiceTemplate;
                            res.InvoiceTemplatePath = item.InvoiceTemplatePath;
                            res.LastUpdatedBy = item.LastUpdatedBy;
                            res.LastUpdatedDate = item.LastUpdatedDate;
                            res.CityName = item.CityName;
                            res.StateName = item.StateName;
                            res.IsAlert = item.IsAlert;
                            res.ContactsRefID = item.ContactsRefID;
                            res.BillType = item.BillType;
                            res.Instructions = item.Instructions;

                            model.NursingHomesContactsList = await _adminService.GetNursingHomesContactsList(item.ContactsRefID, 0);
                        }
                    }
                    //model.NursingHomeContractDetailsList = await _commonService.GetNursingHomeContractDetailsList(Id);
                    model.NursingHomeDetails = res;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            ModelState.Clear();
            return PartialView("_NursingHomeAdd", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteNursingHomes(int NursingHomeID)
        {
            string strMessage = "";
            NursingHomes model = new NursingHomes();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                await _adminService.DeleteNursingHomes(NursingHomeID, userId);
                model.NursingHomeList = await _commonService.GetNursingHomes(0);

                strMessage = "Success";
            }
            catch (Exception Ex)
            {

            }
            return Json(new { strMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DownloadNursingHomes()
        {
            List<NursingHomeList> model = new List<NursingHomeList>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _commonService.GetNursingHomes(0);
            var data = model.Select(x => new
            {
                Name = x.Name,
                Address = x.Address,
                City = x.CityName,
                State = x.StateName,
                ZipCode = x.ZipCode,
                Phone_Number = x.Phone,
                Fax_Number = x.FaxNumber,
                Email_ID = x.EmailID,
                Contact_Name = x.ContactName,
                Department = x.Department,
                Client_Specific = x.ClientSpecificName,
                Contact_Pricing = x.ContractPricing == true ? "Yes" : "No",
                Invoice_Preference = x.InvoiceP,
                Invoice_Schedule = x.InvoiceSchedule,
                Letter_Included = x.LetterIncluded == true ? "Yes" : "No",
                W9 = x.W9 == true ? "Yes" : "No",
                Vendor_Letter = x.VendorLetter == true ? "Yes" : "No",
                Invoice_Template = x.InvoiceTemplate == true ? "Yes" : "No",
                Notes = x.Notes,
                BillType = x.BillType,
                Instructions = x.Instructions
            }).ToList();
            string sFileName = @"NursingHomes.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("NursingHomes");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            return Json(UrlBase);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddNursingHomeContacts([FromBody] NursingHomesContacts nursingHomesContacts)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.NursingHomesContactsList = await _adminService.AddNursingHomesContactsDetails(nursingHomesContacts, userId);


                model.NursingHomeList = await _commonService.GetNursingHomes(0);
                model.RefId = model.NursingHomesContactsList.Select(df => df.RefID).FirstOrDefault();
                model.ContactID = model.NursingHomesContactsList.Select(df => df.ID).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            //return PartialView("_NursingHomeContactList", model);
            return Json(new { ReferenceId = model.RefId, ContactId = model.ContactID });
        }

        [HttpPost]
        public async Task<ActionResult> GetNursingHomeContacts(int NursingHomeID, int RefID)
        {
            NursingHomes model = new NursingHomes();
            try
            {
                model.NursingHomesContactsList = await _adminService.GetNursingHomesContactsList(RefID, NursingHomeID);
            }
            catch (Exception ex) { }
            return PartialView("_NursingHomeContactList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteNursingHomesContacts(int NursingHomeContactID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteNursingHomesContacts(NursingHomeContactID, UserId);
            }
            catch (Exception Ex)
            {

            }
            return Json(new { });
        }
    }
}

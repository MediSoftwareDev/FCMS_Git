using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCMS.ViewModels.Hospices;
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
using AlanJuden.MvcReportViewer;

namespace FCMS.Controllers
{
    [SessionTimeout]
    public class HospicesController : Controller
    {
        String StorageRoot = null;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;

        private readonly MenuUtils _menuUtils;
        int totalrows, totalRowsAfterFiltering;//, Start, Length,  totalRowsAfterFiltering, ContractId;

        public HospicesController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IAuthorizationService authorizationService)
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

        public async Task<IActionResult> Hospices()
        {
            Hospices model = new Hospices();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                model.HospiceDetails = new HospiceDetails();
                model.HospiceList = await _commonService.GetHospices(0);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.HospiceContractDetailsList = new List<HospiceContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();

                model.HospicesContacts = new HospicesContacts();
                model.HospicesContactsList = new List<HospicesContactsList>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchHospice(string Prefix)
        {
            Hospices model = new Hospices();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                model.SearchHospiceList = await _commonService.GetSearchHospice(Prefix);
            }
            catch (Exception Ex) { }
            return Json(model.SearchHospiceList);
        }

        [HttpPost]
        public async Task<IActionResult> HospiceDetails(int Id)
        {
            Hospices model = new Hospices();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);

                model.HospiceList = await _commonService.GetHospices(Id);
                HospiceDetails res = new HospiceDetails();
                if (model.HospiceList.Count > 0)
                {
                    foreach (var item in model.HospiceList)
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
                    model.HospiceContractDetailsList = await _commonService.GetHospiceContractDetailsList(Id);
                    model.HospiceDetails = res;
                }
            }
            catch (Exception ex)
            { }
            return PartialView("_HospiceSearch", model);
        }

        [HttpPost]
        public async Task<ActionResult> HospiceLog(int hospiceId)
        {
            Hospices model = new Hospices();
            try
            {
                model.HospiceLogList = await _commonService.GetHospiceLogList(hospiceId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.HospiceLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> HospiceNotesDetails(int HospiceId)
        {
            Hospices model = new Hospices();
            try
            {

                model.HospiceNotesList = await _commonService.GetHospiceNotes(HospiceId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.HospiceNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHospiceNotes(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);


            int HospiceId = await _commonService.DeleteHospiceNotes(Id, userId);

            return Json(new { HospiceId = HospiceId });
        }
        [HttpPost]
        public async Task<ActionResult> EditNotes(int Id)
        {
            Hospices model = new Hospices();
            try
            {
                model.HospiceNotesList = await _commonService.GetEditHospiceNote(Id);
            }
            catch (Exception ex) { }
            return Json(new { data = model.HospiceNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospiceNotes(int Id, int HospiceId, string Notes)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateHospiceNotesDetails(Id, HospiceId, Notes, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospices([FromBody] HospiceDetails hospiceDetails)
        {
            string retMessage = string.Empty;
            Hospices model = new Hospices();
            var userId = HttpContext.Session.GetString("UserID");
            int hospiceId = 0;

            HospiceOutputID hospiceOutputID = new HospiceOutputID();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                // hospiceDetails.LastUpdatedBy = HttpContext.Session.GetString("UserID");
                model.HospiceDetails = hospiceDetails;
                hospiceOutputID = await _adminService.SaveHospices(model, userId);

                hospiceId = hospiceOutputID.ID;

            }
            catch (Exception ex)
            {

            }
            return Json(new { HospiceId = hospiceId });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospialFiles(int HospiceId, IFormFile LetterCopy, IFormFile W9Copy, IFormFile VendorLetterCopy, IFormFile InvoiceTemplateCopy)
        {
            string strMessage = "";
            var userId = HttpContext.Session.GetString("UserID");
            Hospices hospices = new Hospices();
            HospiceDetails model = new HospiceDetails();

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
                    string Direc_FileName = HospiceId + "\\" + LetterFileName;

                    Letterpath = Path.Combine(StorageRoot + "Hospices", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospices" + "\\" + HospiceId);

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
                    string Direc_FileName = HospiceId + "\\" + W9FileName;

                    W9path = Path.Combine(StorageRoot + "Hospices", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospices" + "\\" + HospiceId);

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
                    string Direc_FileName = HospiceId + "\\" + VendorLetterFileName;

                    VendorLetterpath = Path.Combine(StorageRoot + "Hospices", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospices" + "\\" + HospiceId);

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
                    string Direc_FileName = HospiceId + "\\" + InvoiceTemplateFileName;

                    InvoiceTemplatepath = Path.Combine(StorageRoot + "Hospices", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospices" + "\\" + HospiceId);

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

                await _adminService.UpdateHospiceFilePath(HospiceId, LetterFileName, W9FileName, VendorLetterFileName, InvoiceTemplateFileName);

                hospices.HospiceList = await _commonService.GetHospices(0);

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

        public FileStreamResult GetFileFromPath(string fileName)
        {
            var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "Hospices\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
            string Extn = Path.GetExtension(fileName.Trim());

            switch (Extn)
            {
                case ".pdf":
                    Response.ContentType = "application/pdf";
                    break;
                case ".doc":
                    Response.ContentType = "application/msword";
                    break;
                case ".docx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    Response.ContentType = "application/text";
                    break;
            }

            var cd = new ContentDispositionHeaderValue("inline")
            {
                FileNameStar = Path.GetFileName(fileName)
            };
            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

            var fsResult = File(fileStream, Response.ContentType);
            return fsResult;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditHospices(int Id)
        {
            Hospices model = new Hospices();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");

                model.HospiceList = await _commonService.GetHospices(Id);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.HospiceContractDetailsList = new List<HospiceContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();
                model.HospicesContacts = new HospicesContacts();

                HospiceDetails res = new HospiceDetails();

                if (model.HospiceList.Count > 0)
                {
                    foreach (var item in model.HospiceList)
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

                            model.HospicesContactsList = await _adminService.GetHospicesContactsList(item.ContactsRefID, 0);
                        }
                    }
                    //model.HospiceContractDetailsList = await _commonService.GetHospiceContractDetailsList(Id);
                    model.HospiceDetails = res;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            ModelState.Clear();
            return PartialView("_HospiceAdd", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteHospices(int HospiceID)
        {
            string strMessage = "";
            Hospices model = new Hospices();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                await _adminService.DeleteHospices(HospiceID, userId);
                model.HospiceList = await _commonService.GetHospices(0);

                strMessage = "Success";
            }
            catch (Exception Ex)
            {

            }
            return Json(new { strMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DownloadHospices()
        {
            List<HospiceList> model = new List<HospiceList>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _commonService.GetHospices(0);
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
            string sFileName = @"Hospices.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hospices");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            return Json(UrlBase);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddHospiceContacts([FromBody] HospicesContacts hospicesContacts)
        {
            Hospices model = new Hospices();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.HospicesContactsList = await _adminService.AddHospicesContactsDetails(hospicesContacts, userId);


                model.HospiceList = await _commonService.GetHospices(0);
                model.RefId = model.HospicesContactsList.Select(df => df.RefID).FirstOrDefault();
                model.ContactID = model.HospicesContactsList.Select(df => df.ID).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            //return PartialView("_HospiceContactList", model);
            return Json(new { ReferenceId = model.RefId, ContactId = model.ContactID });
        }

        [HttpPost]
        public async Task<ActionResult> GetHospiceContacts(int HospiceID, int RefID)
        {
            Hospices model = new Hospices();
            try
            {
                model.HospicesContactsList = await _adminService.GetHospicesContactsList(RefID, HospiceID);
            }
            catch (Exception ex) { }
            return PartialView("_HospiceContactList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteHospicesContacts(int HospiceContactID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteHospicesContacts(HospiceContactID, UserId);
            }
            catch (Exception Ex)
            {

            }
            return Json(new { });
        }
    }
}

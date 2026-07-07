using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCMS.ViewModels.CorrectionalFacilities;
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
    public class CorrectionalFacilitiesController : Controller
    {
        String StorageRoot = null;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;

        private readonly MenuUtils _menuUtils;
        int totalrows, totalRowsAfterFiltering;//, Start, Length,  totalRowsAfterFiltering, ContractId;

        public CorrectionalFacilitiesController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IAuthorizationService authorizationService)
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

        public async Task<IActionResult> CorrectionalFacilities()
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                model.CorrectionalFacilitieDetails = new CorrectionalFacilitieDetails();
                model.CorrectionalFacilitieList = await _commonService.GetCorrectionalFacilities(0);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.CorrectionalFacilitieContractDetailsList = new List<CorrectionalFacilitieContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();

                model.CorrectionalFacilitiesContacts = new CorrectionalFacilitiesContacts();
                model.CorrectionalFacilitiesContactsList = new List<CorrectionalFacilitiesContactsList>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchCorrectionalFacilitie(string Prefix)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                model.SearchCorrectionalFacilitieList = await _commonService.GetSearchCorrectionalFacilitie(Prefix);
            }
            catch (Exception Ex) { }
            return Json(model.SearchCorrectionalFacilitieList);
        }

        [HttpPost]
        public async Task<IActionResult> CorrectionalFacilitieDetails(int Id)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);

                model.CorrectionalFacilitieList = await _commonService.GetCorrectionalFacilities(Id);
                CorrectionalFacilitieDetails res = new CorrectionalFacilitieDetails();
                if (model.CorrectionalFacilitieList.Count > 0)
                {
                    foreach (var item in model.CorrectionalFacilitieList)
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
                    model.CorrectionalFacilitieContractDetailsList = await _commonService.GetCorrectionalFacilitieContractDetailsList(Id);
                    model.CorrectionalFacilitieDetails = res;
                }
            }
            catch (Exception ex)
            { }
            return PartialView("_CorrectionalFacilitieSearch", model);
        }

        [HttpPost]
        public async Task<ActionResult> CorrectionalFacilitieLog(int correctionalFacilitieId)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                model.CorrectionalFacilitieLogList = await _commonService.GetCorrectionalFacilitieLogList(correctionalFacilitieId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.CorrectionalFacilitieLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> CorrectionalFacilitieNotesDetails(int CorrectionalFacilitieId)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {

                model.CorrectionalFacilitieNotesList = await _commonService.GetCorrectionalFacilitieNotes(CorrectionalFacilitieId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.CorrectionalFacilitieNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCorrectionalFacilitieNotes(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);


            int CorrectionalFacilitieId = await _commonService.DeleteCorrectionalFacilitieNotes(Id, userId);

            return Json(new { CorrectionalFacilitieId = CorrectionalFacilitieId });
        }
        [HttpPost]
        public async Task<ActionResult> EditNotes(int Id)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                model.CorrectionalFacilitieNotesList = await _commonService.GetEditCorrectionalFacilitieNote(Id);
            }
            catch (Exception ex) { }
            return Json(new { data = model.CorrectionalFacilitieNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveCorrectionalFacilitieNotes(int Id, int CorrectionalFacilitieId, string Notes)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateCorrectionalFacilitieNotesDetails(Id, CorrectionalFacilitieId, Notes, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveCorrectionalFacilities([FromBody] CorrectionalFacilitieDetails correctionalFacilitieDetails)
        {
            string retMessage = string.Empty;
            CorrectionalFacilities model = new CorrectionalFacilities();
            var userId = HttpContext.Session.GetString("UserID");
            int correctionalFacilitieId = 0;

            CorrectionalFacilitieOutputID correctionalFacilitieOutputID = new CorrectionalFacilitieOutputID();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                // correctionalFacilitieDetails.LastUpdatedBy = HttpContext.Session.GetString("UserID");
                model.CorrectionalFacilitieDetails = correctionalFacilitieDetails;
                correctionalFacilitieOutputID = await _adminService.SaveCorrectionalFacilities(model, userId);

                correctionalFacilitieId = correctionalFacilitieOutputID.ID;

            }
            catch (Exception ex)
            {

            }
            return Json(new { CorrectionalFacilitieId = correctionalFacilitieId });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospialFiles(int CorrectionalFacilitieId, IFormFile LetterCopy, IFormFile W9Copy, IFormFile VendorLetterCopy, IFormFile InvoiceTemplateCopy)
        {
            string strMessage = "";
            var userId = HttpContext.Session.GetString("UserID");
            CorrectionalFacilities correctionalFacilities = new CorrectionalFacilities();
            CorrectionalFacilitieDetails model = new CorrectionalFacilitieDetails();

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
                    string Direc_FileName = CorrectionalFacilitieId + "\\" + LetterFileName;

                    Letterpath = Path.Combine(StorageRoot + "CorrectionalFacilities", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "CorrectionalFacilities" + "\\" + CorrectionalFacilitieId);

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
                    string Direc_FileName = CorrectionalFacilitieId + "\\" + W9FileName;

                    W9path = Path.Combine(StorageRoot + "CorrectionalFacilities", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "CorrectionalFacilities" + "\\" + CorrectionalFacilitieId);

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
                    string Direc_FileName = CorrectionalFacilitieId + "\\" + VendorLetterFileName;

                    VendorLetterpath = Path.Combine(StorageRoot + "CorrectionalFacilities", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "CorrectionalFacilities" + "\\" + CorrectionalFacilitieId);

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
                    string Direc_FileName = CorrectionalFacilitieId + "\\" + InvoiceTemplateFileName;

                    InvoiceTemplatepath = Path.Combine(StorageRoot + "CorrectionalFacilities", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "CorrectionalFacilities" + "\\" + CorrectionalFacilitieId);

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

                await _adminService.UpdateCorrectionalFacilitieFilePath(CorrectionalFacilitieId, LetterFileName, W9FileName, VendorLetterFileName, InvoiceTemplateFileName);

                correctionalFacilities.CorrectionalFacilitieList = await _commonService.GetCorrectionalFacilities(0);

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
            var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "CorrectionalFacilities\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
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
        public async Task<IActionResult> EditCorrectionalFacilities(int Id)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");

                model.CorrectionalFacilitieList = await _commonService.GetCorrectionalFacilities(Id);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.CorrectionalFacilitieContractDetailsList = new List<CorrectionalFacilitieContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();
                model.CorrectionalFacilitiesContacts = new CorrectionalFacilitiesContacts();

                CorrectionalFacilitieDetails res = new CorrectionalFacilitieDetails();

                if (model.CorrectionalFacilitieList.Count > 0)
                {
                    foreach (var item in model.CorrectionalFacilitieList)
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

                            model.CorrectionalFacilitiesContactsList = await _adminService.GetCorrectionalFacilitiesContactsList(item.ContactsRefID, 0);
                        }
                    }
                    //model.CorrectionalFacilitieContractDetailsList = await _commonService.GetCorrectionalFacilitieContractDetailsList(Id);
                    model.CorrectionalFacilitieDetails = res;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            ModelState.Clear();
            return PartialView("_CorrectionalFacilitieAdd", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCorrectionalFacilities(int CorrectionalFacilitieID)
        {
            string strMessage = "";
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                await _adminService.DeleteCorrectionalFacilities(CorrectionalFacilitieID, userId);
                model.CorrectionalFacilitieList = await _commonService.GetCorrectionalFacilities(0);

                strMessage = "Success";
            }
            catch (Exception Ex)
            {

            }
            return Json(new { strMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DownloadCorrectionalFacilities()
        {
            List<CorrectionalFacilitieList> model = new List<CorrectionalFacilitieList>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _commonService.GetCorrectionalFacilities(0);
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
            string sFileName = @"CorrectionalFacilities.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("CorrectionalFacilities");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            return Json(UrlBase);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddCorrectionalFacilitieContacts([FromBody] CorrectionalFacilitiesContacts correctionalFacilitiesContacts)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.CorrectionalFacilitiesContactsList = await _adminService.AddCorrectionalFacilitiesContactsDetails(correctionalFacilitiesContacts, userId);


                model.CorrectionalFacilitieList = await _commonService.GetCorrectionalFacilities(0);
                model.RefId = model.CorrectionalFacilitiesContactsList.Select(df => df.RefID).FirstOrDefault();
                model.ContactID = model.CorrectionalFacilitiesContactsList.Select(df => df.ID).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            //return PartialView("_CorrectionalFacilitieContactList", model);
            return Json(new { ReferenceId = model.RefId, ContactId = model.ContactID });
        }

        [HttpPost]
        public async Task<ActionResult> GetCorrectionalFacilitieContacts(int CorrectionalFacilitieID, int RefID)
        {
            CorrectionalFacilities model = new CorrectionalFacilities();
            try
            {
                model.CorrectionalFacilitiesContactsList = await _adminService.GetCorrectionalFacilitiesContactsList(RefID, CorrectionalFacilitieID);
            }
            catch (Exception ex) { }
            return PartialView("_CorrectionalFacilitieContactList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCorrectionalFacilitiesContacts(int CorrectionalFacilitieContactID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteCorrectionalFacilitiesContacts(CorrectionalFacilitieContactID, UserId);
            }
            catch (Exception Ex)
            {

            }
            return Json(new { });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCMS.ViewModels.Hospitals;
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
    public class HospitalsController : Controller
    {
        String StorageRoot = null;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;

        private readonly MenuUtils _menuUtils;
        int totalrows, totalRowsAfterFiltering;//, Start, Length,  totalRowsAfterFiltering, ContractId;

        public HospitalsController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IAuthorizationService authorizationService)
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

        public async Task<IActionResult> Hospitals()
        {
            Hospitals model = new Hospitals();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                model.HospitalDetails = new HospitalDetails();
                model.HospitalList = await _commonService.GetHospitals(0);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.HospitalContractDetailsList = new List<HospitalContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();

                model.HospitalsContacts = new HospitalsContacts();
                model.HospitalsContactsList = new List<HospitalsContactsList>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchHospital(string Prefix)
        {
            Hospitals model = new Hospitals();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                model.SearchHospitalList = await _commonService.GetSearchHospital(Prefix);
            }
            catch (Exception Ex) { }
            return Json(model.SearchHospitalList);
        }

        [HttpPost]
        public async Task<IActionResult> HospitalDetails(int Id)
        {
            Hospitals model = new Hospitals();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);

                model.HospitalList = await _commonService.GetHospitals(Id);
                HospitalDetails res = new HospitalDetails();
                if (model.HospitalList.Count > 0)
                {
                    foreach (var item in model.HospitalList)
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

                            
                        }
                    }
                    model.HospitalContractDetailsList = await _commonService.GetHospitalContractDetailsList(Id);
                    model.HospitalDetails = res;
                }
            }
            catch (Exception ex)
            { }
            return PartialView("_HospitalSearch", model);
        }

        [HttpPost]
        public async Task<ActionResult> HospitalLog(int hospitalId)
        {
            Hospitals model = new Hospitals();
            try
            {
                model.HospitalLogList = await _commonService.GetHospitalLogList(hospitalId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.HospitalLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> HospitalNotesDetails(int HospitalId)
        {
            Hospitals model = new Hospitals();
            try
            {

                model.HospitalNotesList = await _commonService.GetHospitalNotes(HospitalId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.HospitalNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHospitalNotes(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);


            int HospitalId = await _commonService.DeleteHospitalNotes(Id, userId);

            return Json(new { HospitalId = HospitalId });
        }
        [HttpPost]
        public async Task<ActionResult> EditNotes(int Id)
        {
            Hospitals model = new Hospitals();
            try
            {
                model.HospitalNotesList = await _commonService.GetEditHospitalNote(Id);
            }
            catch (Exception ex) { }
            return Json(new { data = model.HospitalNotesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospitalNotes(int Id, int HospitalId, string Notes)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateHospitalNotesDetails(Id, HospitalId, Notes, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospitals([FromBody] HospitalDetails hospitalDetails)
        {
            string retMessage = string.Empty;
            Hospitals model = new Hospitals();
            var userId = HttpContext.Session.GetString("UserID");
            int hospitalId = 0;

            HospitalOutputID hospitalOutputID = new HospitalOutputID();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                // hospitalDetails.LastUpdatedBy = HttpContext.Session.GetString("UserID");
                model.HospitalDetails = hospitalDetails;
                hospitalOutputID = await _adminService.SaveHospitals(model, userId);

                hospitalId = hospitalOutputID.ID;

            }
            catch (Exception ex)
            {

            }
            return Json(new { HospitalId = hospitalId });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveHospialFiles(int HospitalId, IFormFile LetterCopy, IFormFile W9Copy, IFormFile VendorLetterCopy, IFormFile InvoiceTemplateCopy)
        {
            string strMessage = "";
            var userId = HttpContext.Session.GetString("UserID");
            Hospitals hospitals = new Hospitals();
            HospitalDetails model = new HospitalDetails();

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
                    string Direc_FileName = HospitalId + "\\" + LetterFileName;

                    Letterpath = Path.Combine(StorageRoot + "Hospitals", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospitals" + "\\" + HospitalId);

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
                    string Direc_FileName = HospitalId + "\\" + W9FileName;

                    W9path = Path.Combine(StorageRoot + "Hospitals", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospitals" + "\\" + HospitalId);

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
                    string Direc_FileName = HospitalId + "\\" + VendorLetterFileName;

                    VendorLetterpath = Path.Combine(StorageRoot + "Hospitals", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospitals" + "\\" + HospitalId);

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
                    string Direc_FileName = HospitalId + "\\" + InvoiceTemplateFileName;

                    InvoiceTemplatepath = Path.Combine(StorageRoot + "Hospitals", Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "Hospitals" + "\\" + HospitalId);

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

                await _adminService.UpdateHospitalFilePath(HospitalId, LetterFileName, W9FileName, VendorLetterFileName,InvoiceTemplateFileName);

                hospitals.HospitalList = await _commonService.GetHospitals(0);

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
            var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "Hospitals\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
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
        public async Task<IActionResult> EditHospitals(int Id)
        {
            Hospitals model = new Hospitals();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");

                model.HospitalList = await _commonService.GetHospitals(Id);
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                model.HospitalContractDetailsList = new List<HospitalContractDetailsList>();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.InvoicePreferenceList = await _adminService.GetInvoicePreferenceList();
                model.InvoiceScheduleList = await _adminService.GetInvoiceScheduleList();
                model.ClientSpecificList = await _adminService.GetClientSpecificList();
                model.HospitalsContacts = new HospitalsContacts();

                HospitalDetails res = new HospitalDetails();

                if (model.HospitalList.Count > 0)
                {
                    foreach (var item in model.HospitalList)
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

                            model.HospitalsContactsList= await _adminService.GetHospitalsContactsList(item.ContactsRefID,0);
                        }
                    }
                    //model.HospitalContractDetailsList = await _commonService.GetHospitalContractDetailsList(Id);
                    model.HospitalDetails = res;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            ModelState.Clear();
            return PartialView("_HospitalAdd", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteHospitals(int HospitalID)
        {
            string strMessage = "";
            Hospitals model = new Hospitals();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                await _adminService.DeleteHospitals(HospitalID, userId);
                model.HospitalList = await _commonService.GetHospitals(0);

                strMessage = "Success";
            }
            catch (Exception Ex)
            {

            }
            return Json(new { strMessage });
        }

        [HttpPost]
        public async Task<IActionResult> DownloadHospitals()
        {
            List<HospitalList> model = new List<HospitalList>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _commonService.GetHospitals(0);
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
                Contact_Pricing = x.ContractPricing==true?"Yes":"No",
                Invoice_Preference = x.InvoiceP,
                Invoice_Schedule = x.InvoiceSchedule,
                Letter_Included = x.LetterIncluded == true ? "Yes" : "No",
                W9 = x.W9 == true ? "Yes" : "No",
                Vendor_Letter = x.VendorLetter == true ? "Yes" : "No",
                Invoice_Template = x.InvoiceTemplate == true ? "Yes" : "No",
                Notes = x.Notes
            }).ToList();
            string sFileName = @"Hospitals.xlsx";
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
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hospitals");
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            return Json(UrlBase);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddHospitalContacts([FromBody] HospitalsContacts hospitalsContacts)
        {
            Hospitals model = new Hospitals();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.HospitalsContactsList = await _adminService.AddHospitalsContactsDetails(hospitalsContacts, userId);

                
                model.HospitalList = await _commonService.GetHospitals(0);
                model.RefId = model.HospitalsContactsList.Select(df => df.RefID).FirstOrDefault();
                model.ContactID = model.HospitalsContactsList.Select(df => df.ID).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            //return PartialView("_HospitalContactList", model);
             return Json(new { ReferenceId = model.RefId, ContactId = model.ContactID });
        }

        [HttpPost]
        public async Task<ActionResult> GetHospitalContacts(int HospitalID,int RefID)
        {
            Hospitals model = new Hospitals();
            try
            {
                model.HospitalsContactsList = await _adminService.GetHospitalsContactsList(RefID, HospitalID);
            }
            catch (Exception ex) { }
            return PartialView("_HospitalContactList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteHospitalsContacts(int HospitalContactID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteHospitalsContacts(HospitalContactID,UserId);
            }
            catch (Exception Ex)
            {

            }
            return Json(new { });
        }
    }
}

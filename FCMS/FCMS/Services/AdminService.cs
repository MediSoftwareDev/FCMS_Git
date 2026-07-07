using FCMS.ViewModels.Admin;
using FCMS.ViewModels.Contract;
using FCMS.ViewModels.Hospitals;
using FCMS.ViewModels.Hospices;
using FCMS.ViewModels.NursingHomes;
using FCMS.ViewModels.CorrectionalFacilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Data;
using WiseX.Models;
using WiseX.ViewModels.Account;
using WiseX.ViewModels.Admin;

namespace WiseX.Services
{
    public class AdminService : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;
        string connectionString = string.Empty;

        public AdminService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            //connectionString = _applicationDbContext.Database.GetDbConnection().ConnectionString;
            connectionString = DBConnection.ConnectionString;
        }

        public async Task<IList<UserDetailsTemp>> GetUserDetails(string UserId)
        {
            var paramUserId = new SqlParameter("@UserID", UserId);
            IList<UserDetailsTemp> check = new List<UserDetailsTemp>();
            try
            {
                check = await _applicationDbContext.UserDetailsTemp
                .FromSql("EXEC GetUserListDetails @UserID", paramUserId)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex) { }
            return check;

        }
        public async Task<IList<GetUserRoles>> GetUserRole(string RoleId)
        {
            var paramUserId = new SqlParameter("@RoleId", RoleId);
            IList<GetUserRoles> check = new List<GetUserRoles>();
            try
            {
                check = await _applicationDbContext.CheckUserRoles.FromSql("EXEC GetUserRoles @RoleId", paramUserId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdateRoleDetails(string RoleId, string Status, int Permissions)
        {
            var RID = new SqlParameter("@RoleId", RoleId);
            var status = new SqlParameter("@Status", Status);
            var ParamPermissions = new SqlParameter("@Permissions", Permissions);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateRolesDetails @RoleId,@Status,@Permissions", RID, status, ParamPermissions);
            }
            catch (Exception Ex) { }
        }
        public async Task<List<Roles>> GetRolesDescStatus(string RoleId, int Active)
        {
            List<Roles> check = new List<Roles>();
            try
            {
                var RID = new SqlParameter("@RoleId", RoleId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.Role.FromSql("EXEC GetRolesDetails @RoleId, @Active", RID, paramActive).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteRoleDescDetails(string RoleId)
        {
            var RID = new SqlParameter("@RoleId", RoleId);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteRoleDescStatus @RoleId", RID);
            }
            catch (Exception Ex) { }
        }
        public async Task<IList<GetUserRoles>> GetAgreementType(string AggrementId)
        {
            var paramAggrementId = new SqlParameter("@AggrementId", AggrementId);
            IList<GetUserRoles> check = new List<GetUserRoles>();
            try
            {
                check = await _applicationDbContext.CheckUserRoles.FromSql("EXEC GetAggrementType @AggrementId", paramAggrementId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdateAgreementTypeDetails(AgreementType agreementType, string UserId)
        {
            try
            {
                var paramAgreementType = new SqlParameter("@AgreementType", agreementType.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateAgreementTypeDetails @AgreementType,@UId", paramAgreementType, paramUserId);
            }
            catch (Exception Ex) { }
        }
        public async Task<List<AgreementTypeDetailsList>> GetAgreementTypeDescStatus(int AggrementId, int Active)
        {
            List<AgreementTypeDetailsList> check = new List<AgreementTypeDetailsList>();
            try
            {
                var paramAggrementId = new SqlParameter("@AggrementId", AggrementId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.AgreementTypeList.FromSql("EXEC GetAggrementType @AggrementId, @Active", paramAggrementId, paramActive).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteAgreementTypeDetails(int AgreementId)
        {

            try
            {
                var paramAgreementId = new SqlParameter("@AgreementId", AgreementId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteAgreementTypeDetails @AgreementId", paramAgreementId);

            }
            catch (Exception Ex) { }
        }



        public async Task<List<Client>> GetClients()
        {
            var paramId = new SqlParameter("@Id", 2);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.Client.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task<List<Client>> GetEditClients(string UserId)
        {
            var paramId = new SqlParameter("@Id", 2);
            var paramSearchTerm = new SqlParameter("@UserID", UserId);
            return await _applicationDbContext.Client.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task<List<Client>> GetClientUserdetails(string UserId)
        {

            var paramSearchTerm = new SqlParameter("@UserID", UserId);
            return await _applicationDbContext.Client.FromSql("EXEC GetClientUserdetails @UserID", paramSearchTerm).ToListAsync();
        }


        public async Task<List<UserListInfo>> GetUserList(string UserID)
        {
            List<UserListInfo> check = new List<UserListInfo>();
            try
            {
                var paramUserid = new SqlParameter("@UserID", UserID);
                check = await _applicationDbContext.UserListInfo
                .FromSql("EXEC GetUserDetails @UserID", paramUserid).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task UpdateUserDetails(Users users, string UId)
        {
            try
            {
                var paramUsers = new SqlParameter("@Users", users.GetXml());
                var UID = new SqlParameter("@UId", UId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateUserDetails @Users, @UId", paramUsers, UID);
            }
            catch (Exception Ex) { }
        }
        public async Task<IList<MenuItem>> GetMenuList(string RoleId = "")
        {
            List<MenuItem> menulist = new List<MenuItem>();
            try
            {
                var Menus = new SqlParameter("@RoleId", string.IsNullOrWhiteSpace(RoleId) ? "" : RoleId);
                //return await _applicationDbContext.MenuItem.FromSql("EXEC GetMenuAccessList @RoleId", Menus).AsNoTracking().ToListAsync();
                menulist = await _applicationDbContext.MenuItem.FromSql("EXEC GetMenuAccessList @RoleId", Menus).ToListAsync();
            }
            catch (Exception Ex) { }
            return menulist;
        }
        public async Task InsertMenuAccess(RoleModules roleModules)
        {
            try
            {
                var MenuAccess = new SqlParameter("@RoleModules", roleModules.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC InsertMenuAccessDetails @RoleModules", MenuAccess);
            }
            catch (Exception Ex) { }
        }
        public async Task DeleteUsers(string users)
        {

            try
            {
                var paramUsers = new SqlParameter("@Users", string.IsNullOrWhiteSpace(users) ? "" : users);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteUserDetails @Users", paramUsers);

            }
            catch (Exception Ex) { }

        }
        public async Task<List<MenuAccessRole>> MenuAccessRole(string RoleID, string UserID)
        {
            List<MenuAccessRole> MenuList = new List<MenuAccessRole>();
            try
            {
                var Menus = new SqlParameter("@RoleId", string.IsNullOrWhiteSpace(RoleID) ? "" : RoleID);
                var User = new SqlParameter("@UserID", string.IsNullOrWhiteSpace(UserID) ? "" : UserID);

                MenuList = await _applicationDbContext.MenuAccessRole.FromSql("EXEC GetMenuAccessRole @RoleId, @UserID ", Menus, User).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return MenuList;
        }

        #region Clients
        public async Task<List<EmployeePositionList>> GetEmployeePositionList()
        {
            List<EmployeePositionList> check = new List<EmployeePositionList>();
            try
            {
                var paramId = new SqlParameter("@Id", 3);
                check = await _applicationDbContext.EmployeePositionList.FromSql("EXEC GetMasterTableData @Id", paramId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<ClientContract> UpdateClientsDetails(Clients clients, string userid)
        {

            ClientContract clientContract = new ClientContract();
            try
            {
                var paramclients = new SqlParameter("@Clients", clients.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                clientContract = await _applicationDbContext.ClientContract.FromSql("EXEC UpdateClients @Clients,@userid", paramclients, paramuserid).FirstOrDefaultAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return clientContract;
        }

        public async Task<List<ClientContractList>> AddClientContractDetails(ClientContractDetails contractDetails, string UserId)
        {
            List<ClientContractList> check = new List<ClientContractList>();
            try
            {
                var paramEmployee = new SqlParameter("@Contract", contractDetails.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                check = await _applicationDbContext.ClientContractList.FromSql("EXEC UpdateClientContract @Contract,@UId", paramEmployee, paramUserId).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task<List<ClientContractList>> GetClientContractList(int RefID)
        {
            List<ClientContractList> check = new List<ClientContractList>();
            try
            {
                var paramRefID = new SqlParameter("@RefID", RefID);
                check = await _applicationDbContext.ClientContractList.FromSql("EXEC GetClientContractList @RefID", paramRefID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeleteClientContract(int ContractID, string Userid)
        {

            try
            {
                var paramContractID = new SqlParameter("@ContractID", ContractID);
                var paramuserID = new SqlParameter("@UId", Userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClientContract @ContractID,@UId", paramContractID, paramuserID);

            }
            catch (Exception Ex) { }
        }


        public async Task<List<ClientsEmployeeList>> AddClientEmployeeDetails(ClientsEmployeeDetails employeeDetails, string UserId)
        {
            List<ClientsEmployeeList> check = new List<ClientsEmployeeList>();
            try
            {
                var paramEmployee = new SqlParameter("@Employee", employeeDetails.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                check = await _applicationDbContext.ClientsEmployeeList.FromSql("EXEC UpdateClientEmployee @Employee,@UId", paramEmployee, paramUserId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<ClientsEmployeeList>> GetClientEmployeeList(int RefID)
        {
            List<ClientsEmployeeList> check = new List<ClientsEmployeeList>();
            try
            {
                var paramRefID = new SqlParameter("@RefID", RefID);
                check = await _applicationDbContext.ClientsEmployeeList.FromSql("EXEC GetClientEmployeeList @RefID", paramRefID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<ClientsDetailsList>> GetClients(int ClientId, int Active)
        {
            List<ClientsDetailsList> check = new List<ClientsDetailsList>();
            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.ClientsDetailsList.FromSql("EXEC GetClients @ClientId, @Active", paramClientId, paramActive).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<ClientsViewDetails> ViewClients(int ClientId, int Active)
        {
            ClientsViewDetails check = new ClientsViewDetails();
            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.ClientsViewDetails.FromSql("EXEC GetViewClients @ClientId, @Active", paramClientId, paramActive).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteClients(int ClientId, string userId)
        {

            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClients @ClientId,@UserID", paramClientId, paramUserId);

            }
            catch (Exception Ex) { }
        }
        public async Task DeleteClientEmployee(int EmployeeID, string Userid)
        {

            try
            {
                var paramEmployeeID = new SqlParameter("@EmployeeID", EmployeeID);
                var paramuserID = new SqlParameter("@UId", Userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClientEmployee @EmployeeID,@UId", paramEmployeeID, paramuserID);

            }
            catch (Exception Ex) { }
        }
        public async Task<List<DownloadClientsDetailsList>> ExportClients()
        {
            List<DownloadClientsDetailsList> check = new List<DownloadClientsDetailsList>();
            try
            {
                check = await _applicationDbContext.DownloadClientsDetailsList.FromSql("EXEC ExportClients").ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<CitiesList>> GetCitiesList()
        {
            List<CitiesList> check = new List<CitiesList>();
            try
            {
                var ParamID = new SqlParameter("@Id", 4);
                check = await _applicationDbContext.CitiesList.FromSql("EXEC GetMasterTableData @Id", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<StatesList>> GetStatesList()
        {
            List<StatesList> check = new List<StatesList>();
            try
            {
                var ParamID = new SqlParameter("@Id", 6);
                check = await _applicationDbContext.StatesList.FromSql("EXEC GetMasterTableData @Id", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<StatesList>> GetStateList(int CityId)
        {
            List<StatesList> check = new List<StatesList>();
            try
            {
                var ParamID = new SqlParameter("@CityId", CityId);
                check = await _applicationDbContext.StatesList.FromSql("EXEC GetStates @CityId", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<CitiesList>> GetCityList(int StateId)
        {
            List<CitiesList> check = new List<CitiesList>();
            try
            {
                var ParamID = new SqlParameter("@StateId", StateId);
                check = await _applicationDbContext.CitiesList.FromSql("EXEC GetCities @StateId", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<AccountExecutiveList>> GetAccountExecutiveList()
        {
            var paramId = new SqlParameter("@Id", 5);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.AccountExecutiveList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task<List<ResidencyCodeList>> GetResidencyCodeList()
        {
            var paramId = new SqlParameter("@Id", 7);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.ResidencyCodeList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task UpdateBulkDetails(Clients bulkClientsDetails, string UId)
        {
            string retMessage = string.Empty;
            try
            {
                var paramclients = new SqlParameter("@BulkClientsDetails", bulkClientsDetails.GetXml());
                var UID = new SqlParameter("@UId", UId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateBulkClients @BulkClientsDetails, @UId", paramclients, UID);

            }
            catch (Exception Ex)
            {

            }
        }

        public async Task<List<BulkClientsDetailsValidationList>> UpdateBulkClientsValidation(Clients bulkClientsDetails, string UId)
        {
            var paramclients = new SqlParameter("@BulkClientsDetails", bulkClientsDetails.GetXml());
            var UID = new SqlParameter("@UId", UId);
            //await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateBulkClientsValidation @BulkClientsDetails, @UId", paramclients, UID);
            var list = await _applicationDbContext.BulkClientsDetailsValidationList.FromSql("EXEC UpdateBulkClientsValidation @BulkClientsDetails, @UId", paramclients, UID).ToListAsync();
            return list;
        }
        #endregion


        public async Task<List<PositionList>> GetPositionListStatus(int PositionID)
        {
            List<PositionList> check = new List<PositionList>();
            try
            {
                var paramPositionID = new SqlParameter("@PositionID", PositionID);
                check = await _applicationDbContext.PositionList.FromSql("EXEC GetEmployeePosition @PositionID", paramPositionID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task UpdateEmployeePositionDetails(Position position, string UserId)
        {
            try
            {
                var paramEmployeePosition = new SqlParameter("@EmployeePosition", position.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateEmployeePositionDetails @EmployeePosition,@UId", paramEmployeePosition, paramUserId);
            }
            catch (Exception Ex) { }
        }

        public async Task DeleteEmployeePositionDetails(int PositionID)
        {
            try
            {
                var paramPositionID = new SqlParameter("@PositionID", PositionID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteEmployeePositionDetails @PositionID", paramPositionID);

            }
            catch (Exception Ex) { }
        }
        public async Task InsertContractType(ContractStatusDet contractStatus, int id)
        {
            var Contract = new SqlParameter("@InsertContractType", contractStatus.GetXml());
            var GetId = new SqlParameter("@Id", id);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [InsertContractStatus] @InsertContractType, @Id", Contract, GetId);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<List<ContractStatusList>> GetContractTypeDetails(int id)
        {
            List<ContractStatusList> ContractStatusList = new List<ContractStatusList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                ContractStatusList = await _applicationDbContext.ContractStatusList.FromSql("EXEC [GetContractType] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return ContractStatusList;
        }
        public async Task DeleteContractDetails(int ContractID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ContractID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteContractDetails @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }
        public async Task<List<ClientsCompanyDetails>> GetCompanyList(int CId)
        {
            List<ClientsCompanyDetails> check = new List<ClientsCompanyDetails>();
            try
            {
                var paramUserid = new SqlParameter("@CompanyID", CId);
                check = await _applicationDbContext.ClientsDetailsInfo.FromSql("EXEC [dbo].[GetCompanyIdDetails] @CompanyID", paramUserid).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }
        //GetContractList
        public async Task<List<ContractStatusList>> GetContractList(String CStatus)
        {
            List<ContractStatusList> check = new List<ContractStatusList>();
            try
            {
                var paramUserid = new SqlParameter("@ContractStatus", CStatus);
                check = await _applicationDbContext.ContractStatusList.FromSql("EXEC [dbo].[GetValidContractStatus] @ContractStatus", paramUserid).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task<List<ClientSpecificList>> GetClientSpecificList()
        {
            List<ClientSpecificList> lst = new List<ClientSpecificList>();
            try
            {
                var Param = new SqlParameter("@SearchTerm", "");
                lst = await _applicationDbContext.ClientSpecificList.FromSql("EXEC GetSearchClient @SearchTerm", Param).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        #region Hospitals
        public async Task<HospitalOutputID> SaveHospitals(Hospitals hospitals, string userId)
        {
            var ID = new SqlParameter("@ID", hospitals.HospitalDetails.ID);
            var Name = new SqlParameter("@Name", hospitals.HospitalDetails.Name);
            var Address = new SqlParameter("@Address", hospitals.HospitalDetails.Address);
            var StateID = new SqlParameter("@StateID", hospitals.HospitalDetails.StateID);
            var CityID = new SqlParameter("@CityID", hospitals.HospitalDetails.CityID);
            var ZipCode = new SqlParameter("@ZipCode", hospitals.HospitalDetails.ZipCode);
            var Phone = new SqlParameter("@Phone", "");
            var FaxNumber = new SqlParameter("@FaxNumber", "");
            var EmailID = new SqlParameter("@EmailID", "");
            var ContactName = new SqlParameter("@ContactName", "");
            var Department = new SqlParameter("Department", hospitals.HospitalDetails.Department);
            var ClientSpecific = new SqlParameter("@ClientSpecific", hospitals.HospitalDetails.ClientSpecific);
            var ContractPricing = new SqlParameter("@ContractPricing", hospitals.HospitalDetails.ContractPricing);
            var InvoicePreference = new SqlParameter("@InvoicePreference", hospitals.HospitalDetails.InvoiceP);
            var InvoiceSchedule = new SqlParameter("@InvoiceSchedule", hospitals.HospitalDetails.InvoiceSchedule);
            var LetterIncluded = new SqlParameter("@LetterIncluded", hospitals.HospitalDetails.LetterIncluded);
            var W9 = new SqlParameter("@W9", hospitals.HospitalDetails.W9);
            var VendorLetter = new SqlParameter("@VendorLetter", hospitals.HospitalDetails.VendorLetter);
            var InvoiceTemplate = new SqlParameter("@InvoiceTemplate", hospitals.HospitalDetails.InvoiceTemplate);
            var IsAlert = new SqlParameter("@IsAlert", hospitals.HospitalDetails.IsAlert);
            var ContactsRefID = new SqlParameter("@ContactsRefID", hospitals.HospitalDetails.ContactsRefID);
            var LastUpdatedBy = new SqlParameter("@LastUpdatedBy", userId);

            HospitalOutputID hospitalOutputID = new HospitalOutputID();
            try
            {
                hospitalOutputID = await _applicationDbContext.HospitalOutputID.FromSql("EXEC [USP_tblHospitals_Insert] @ID,@Name, @Address,@StateID,@CityID,@ZipCode,@Phone,@FaxNumber,@EmailID,@ContactName,@Department,@ClientSpecific,@ContractPricing,@InvoicePreference,@InvoiceSchedule,@LetterIncluded,@W9,@VendorLetter,@InvoiceTemplate,@IsAlert,@ContactsRefID,@LastUpdatedBy", ID, Name, Address, StateID, CityID, ZipCode, Phone, FaxNumber, EmailID, ContactName, Department, ClientSpecific, ContractPricing, InvoicePreference, InvoiceSchedule, LetterIncluded, W9, VendorLetter, InvoiceTemplate, IsAlert, ContactsRefID, LastUpdatedBy).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return hospitalOutputID;
        }

        public async Task UpdateHospitalFilePath(int hospitalID, string letterPath, string w9Path, string vendorLetterpath, string invoiceTemplatePath)
        {
            var paramID = new SqlParameter("@ID", hospitalID);
            var paramLetterPath = new SqlParameter("@LetterPath", letterPath);
            var paramW9Path = new SqlParameter("@W9Path", w9Path);
            var paramVendorLetterPath = new SqlParameter("@VendorLetterPath", vendorLetterpath);
            var paramInvoiceTemplatePath = new SqlParameter("@InvoiceTemplatePath", invoiceTemplatePath);
            HospitalOutputID hospitalOutputID = new HospitalOutputID();
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_tblHospitals_UpdateFilePath] @ID,@LetterPath,@W9Path,@VendorLetterPath,@InvoiceTemplatePath", paramID, paramLetterPath, paramW9Path, paramVendorLetterPath, paramInvoiceTemplatePath);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task DeleteHospitals(int HospitalId, string userId)
        {

            try
            {
                var paramHospitalId = new SqlParameter("@HospitalId", HospitalId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteHospitals @HospitalId,@UserID", paramHospitalId, paramUserId);

            }
            catch (Exception Ex) { }
        }
        public async Task<List<HospitalsContactsList>> AddHospitalsContactsDetails(HospitalsContacts hospitalsContacts,string userId)
        {
            List<HospitalsContactsList> check = new List<HospitalsContactsList>();
            try
            {
                var paramID = new SqlParameter("@ID", hospitalsContacts.ID);
                var paramClientName = new SqlParameter("@ClientName", hospitalsContacts.ClientName);
                var paramPhone = new SqlParameter("@Phone", hospitalsContacts.Phone);
                var paramFaxNumber = new SqlParameter("@FaxNumber", hospitalsContacts.FaxNumber);
                var paramEmailID = new SqlParameter("@EmailID", hospitalsContacts.EmailID);
                var paramContactName = new SqlParameter("@ContactName", hospitalsContacts.ContactName);
                var paramRefID = new SqlParameter("@RefID", hospitalsContacts.RefID);
                var paramBillType = new SqlParameter("@BillType", hospitalsContacts.BillType);
                var paramInstructions = new SqlParameter("@Instructions", hospitalsContacts.Instructions);
                var paramUserId = new SqlParameter("@UserID", userId);

                check = await _applicationDbContext.HospitalsContactsList.FromSql("EXEC USP_tblHospitalsContacts_Insert @ID,@ClientName,@Phone,@FaxNumber,@EmailID,@ContactName,@RefID,@BillType,@Instructions,@UserID", paramID, paramClientName, paramPhone, paramFaxNumber, paramEmailID, paramContactName, paramRefID, paramBillType, paramInstructions,  paramUserId).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task<List<HospitalsContactsList>> GetHospitalsContactsList(int ContactsRefID,int HospitalID)
        {
            List<HospitalsContactsList> check = new List<HospitalsContactsList>();
            try
            {
                var paramID = new SqlParameter("@ContactsRefID", ContactsRefID);
                var paramHospitalID = new SqlParameter("@HospitalID", HospitalID);

                check = await _applicationDbContext.HospitalsContactsList.FromSql("EXEC USP_tblHospitalsContacts_Select @ContactsRefID,@HospitalID", paramID, paramHospitalID).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }


        public async Task DeleteHospitalsContacts(int ID, string userId)
        {

            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteHospitalsContacts @ID,@UserID", paramID, paramUserId);

            }
            catch (Exception Ex) { }
        }

        #endregion

        #region Hospices
        public async Task<HospiceOutputID> SaveHospices(Hospices Hospices, string userId)
        {
            var ID = new SqlParameter("@ID", Hospices.HospiceDetails.ID);
            var Name = new SqlParameter("@Name", Hospices.HospiceDetails.Name);
            var Address = new SqlParameter("@Address", Hospices.HospiceDetails.Address);
            var StateID = new SqlParameter("@StateID", Hospices.HospiceDetails.StateID);
            var CityID = new SqlParameter("@CityID", Hospices.HospiceDetails.CityID);
            var ZipCode = new SqlParameter("@ZipCode", Hospices.HospiceDetails.ZipCode);
            var Phone = new SqlParameter("@Phone", "");
            var FaxNumber = new SqlParameter("@FaxNumber","");
            var EmailID = new SqlParameter("@EmailID","");
            var ContactName = new SqlParameter("@ContactName","");
            var Department = new SqlParameter("Department", Hospices.HospiceDetails.Department);
            var ClientSpecific = new SqlParameter("@ClientSpecific", Hospices.HospiceDetails.ClientSpecific);
            var ContractPricing = new SqlParameter("@ContractPricing", Hospices.HospiceDetails.ContractPricing);
            var InvoicePreference = new SqlParameter("@InvoicePreference", Hospices.HospiceDetails.InvoiceP);
            var InvoiceSchedule = new SqlParameter("@InvoiceSchedule", Hospices.HospiceDetails.InvoiceSchedule);
            var LetterIncluded = new SqlParameter("@LetterIncluded", Hospices.HospiceDetails.LetterIncluded);
            var W9 = new SqlParameter("@W9", Hospices.HospiceDetails.W9);
            var VendorLetter = new SqlParameter("@VendorLetter", Hospices.HospiceDetails.VendorLetter);
            var InvoiceTemplate = new SqlParameter("@InvoiceTemplate", Hospices.HospiceDetails.InvoiceTemplate);
            var IsAlert = new SqlParameter("@IsAlert", Hospices.HospiceDetails.IsAlert);
            var ContactsRefID = new SqlParameter("@ContactsRefID", Hospices.HospiceDetails.ContactsRefID);
            var LastUpdatedBy = new SqlParameter("@LastUpdatedBy", userId);

            HospiceOutputID hospiceOutputID = new HospiceOutputID();
            try
            {
                hospiceOutputID = await _applicationDbContext.HospiceOutputID.FromSql("EXEC [USP_tblHospices_Insert] @ID,@Name, @Address,@StateID,@CityID,@ZipCode,@Phone,@FaxNumber,@EmailID,@ContactName,@Department,@ClientSpecific,@ContractPricing,@InvoicePreference,@InvoiceSchedule,@LetterIncluded,@W9,@VendorLetter,@InvoiceTemplate,@IsAlert,@ContactsRefID,@LastUpdatedBy", ID, Name, Address, StateID, CityID, ZipCode, Phone, FaxNumber, EmailID, ContactName, Department, ClientSpecific, ContractPricing, InvoicePreference, InvoiceSchedule, LetterIncluded, W9, VendorLetter, InvoiceTemplate, IsAlert, ContactsRefID, LastUpdatedBy).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return hospiceOutputID;
        }

        public async Task UpdateHospiceFilePath(int HospiceID, string letterPath, string w9Path, string vendorLetterpath, string invoiceTemplatePath)
        {
            var paramID = new SqlParameter("@ID", HospiceID);
            var paramLetterPath = new SqlParameter("@LetterPath", letterPath);
            var paramW9Path = new SqlParameter("@W9Path", w9Path);
            var paramVendorLetterPath = new SqlParameter("@VendorLetterPath", vendorLetterpath);
            var paramInvoiceTemplatePath = new SqlParameter("@InvoiceTemplatePath", invoiceTemplatePath);
            HospiceOutputID HospiceOutputID = new HospiceOutputID();
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_tblHospices_UpdateFilePath] @ID,@LetterPath,@W9Path,@VendorLetterPath,@InvoiceTemplatePath", paramID, paramLetterPath, paramW9Path, paramVendorLetterPath, paramInvoiceTemplatePath);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task DeleteHospices(int HospiceId, string userId)
        {

            try
            {
                var paramHospiceId = new SqlParameter("@HospiceId", HospiceId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteHospices @HospiceId,@UserID", paramHospiceId, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<HospicesContactsList>> AddHospicesContactsDetails(HospicesContacts hospitalsContacts, string userId)
        {
            List<HospicesContactsList> check = new List<HospicesContactsList>();
            try
            {
                var paramID = new SqlParameter("@ID", hospitalsContacts.ID);
                var paramClientName = new SqlParameter("@ClientName", hospitalsContacts.ClientName);
                var paramPhone = new SqlParameter("@Phone", hospitalsContacts.Phone);
                var paramFaxNumber = new SqlParameter("@FaxNumber", hospitalsContacts.FaxNumber);
                var paramEmailID = new SqlParameter("@EmailID", hospitalsContacts.EmailID);
                var paramContactName = new SqlParameter("@ContactName", hospitalsContacts.ContactName);
                var paramRefID = new SqlParameter("@RefID", hospitalsContacts.RefID);
                var paramBillType = new SqlParameter("@BillType", hospitalsContacts.BillType);
                var paramInstructions = new SqlParameter("@Instructions", hospitalsContacts.Instructions);
                var paramUserId = new SqlParameter("@UserID", userId);

                check = await _applicationDbContext.HospicesContactsList.FromSql("EXEC USP_tblHospicesContacts_Insert @ID,@ClientName,@Phone,@FaxNumber,@EmailID,@ContactName,@RefID,@BillType,@Instructions,@UserID", paramID, paramClientName, paramPhone, paramFaxNumber, paramEmailID, paramContactName, paramRefID, paramBillType, paramInstructions,paramUserId).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task<List<HospicesContactsList>> GetHospicesContactsList(int ContactsRefID, int HospiceID)
        {
            List<HospicesContactsList> check = new List<HospicesContactsList>();
            try
            {
                var paramID = new SqlParameter("@ContactsRefID", ContactsRefID);
                var paramHospiceID = new SqlParameter("@HospiceID", HospiceID);

                check = await _applicationDbContext.HospicesContactsList.FromSql("EXEC USP_tblHospicesContacts_Select @ContactsRefID,@HospiceID", paramID, paramHospiceID).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task DeleteHospicesContacts(int ID, string userId)
        {

            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteHospicesContacts @ID,@UserID", paramID, paramUserId);

            }
            catch (Exception Ex) { }
        }
        #endregion

        #region Nursing Home
        public async Task<NursingHomeOutputID> SaveNursingHomes(NursingHomes NursingHomes, string userId)
        {
            var ID = new SqlParameter("@ID", NursingHomes.NursingHomeDetails.ID);
            var Name = new SqlParameter("@Name", NursingHomes.NursingHomeDetails.Name);
            var Address = new SqlParameter("@Address", NursingHomes.NursingHomeDetails.Address);
            var StateID = new SqlParameter("@StateID", NursingHomes.NursingHomeDetails.StateID);
            var CityID = new SqlParameter("@CityID", NursingHomes.NursingHomeDetails.CityID);
            var ZipCode = new SqlParameter("@ZipCode", NursingHomes.NursingHomeDetails.ZipCode);
            var Phone = new SqlParameter("@Phone", "");
            var FaxNumber = new SqlParameter("@FaxNumber", "");
            var EmailID = new SqlParameter("@EmailID", "");
            var ContactName = new SqlParameter("@ContactName", "");
            var Department = new SqlParameter("Department", NursingHomes.NursingHomeDetails.Department);
            var ClientSpecific = new SqlParameter("@ClientSpecific", NursingHomes.NursingHomeDetails.ClientSpecific);
            var ContractPricing = new SqlParameter("@ContractPricing", NursingHomes.NursingHomeDetails.ContractPricing);
            var InvoicePreference = new SqlParameter("@InvoicePreference", NursingHomes.NursingHomeDetails.InvoiceP);
            var InvoiceSchedule = new SqlParameter("@InvoiceSchedule", NursingHomes.NursingHomeDetails.InvoiceSchedule);
            var LetterIncluded = new SqlParameter("@LetterIncluded", NursingHomes.NursingHomeDetails.LetterIncluded);
            var W9 = new SqlParameter("@W9", NursingHomes.NursingHomeDetails.W9);
            var VendorLetter = new SqlParameter("@VendorLetter", NursingHomes.NursingHomeDetails.VendorLetter);
            var InvoiceTemplate = new SqlParameter("@InvoiceTemplate", NursingHomes.NursingHomeDetails.InvoiceTemplate);
            var IsAlert = new SqlParameter("@IsAlert", NursingHomes.NursingHomeDetails.IsAlert);
            var ContactsRefID = new SqlParameter("@ContactsRefID", NursingHomes.NursingHomeDetails.ContactsRefID);
            var LastUpdatedBy = new SqlParameter("@LastUpdatedBy", userId);

            NursingHomeOutputID nursingHomeOutputID = new NursingHomeOutputID();
            try
            {
                nursingHomeOutputID = await _applicationDbContext.NursingHomeOutputID.FromSql("EXEC [USP_tblNursingHomes_Insert] @ID,@Name, @Address,@StateID,@CityID,@ZipCode,@Phone,@FaxNumber,@EmailID,@ContactName,@Department,@ClientSpecific,@ContractPricing,@InvoicePreference,@InvoiceSchedule,@LetterIncluded,@W9,@VendorLetter,@InvoiceTemplate,@IsAlert,@ContactsRefID,@LastUpdatedBy", ID, Name, Address, StateID, CityID, ZipCode, Phone, FaxNumber, EmailID, ContactName, Department, ClientSpecific, ContractPricing, InvoicePreference, InvoiceSchedule, LetterIncluded, W9, VendorLetter, InvoiceTemplate, IsAlert, ContactsRefID, LastUpdatedBy).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return nursingHomeOutputID;
        }
        
      
        public async Task UpdateNursingHomeFilePath(int NursingHomeID, string letterPath, string w9Path, string vendorLetterpath, string invoiceTemplatePath)
        {
            var paramID = new SqlParameter("@ID", NursingHomeID);
            var paramLetterPath = new SqlParameter("@LetterPath", letterPath);
            var paramW9Path = new SqlParameter("@W9Path", w9Path);
            var paramVendorLetterPath = new SqlParameter("@VendorLetterPath", vendorLetterpath);
            var paramInvoiceTemplatePath = new SqlParameter("@InvoiceTemplatePath", invoiceTemplatePath);
            NursingHomeOutputID NursingHomeOutputID = new NursingHomeOutputID();
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_tblNursingHomes_UpdateFilePath] @ID,@LetterPath,@W9Path,@VendorLetterPath,@InvoiceTemplatePath", paramID, paramLetterPath, paramW9Path, paramVendorLetterPath, paramInvoiceTemplatePath);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task DeleteNursingHomes(int NursingHomeId, string userId)
        {

            try
            {
                var paramNursingHomeId = new SqlParameter("@NursingHomeId", NursingHomeId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteNursingHomes @NursingHomeId,@UserID", paramNursingHomeId, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<NursingHomesContactsList>> AddNursingHomesContactsDetails(NursingHomesContacts hospitalsContacts, string userId)
        {
            List<NursingHomesContactsList> check = new List<NursingHomesContactsList>();
            try
            {
                var paramID = new SqlParameter("@ID", hospitalsContacts.ID);
                var paramClientName = new SqlParameter("@ClientName", hospitalsContacts.ClientName);
                var paramPhone = new SqlParameter("@Phone", hospitalsContacts.Phone);
                var paramFaxNumber = new SqlParameter("@FaxNumber", hospitalsContacts.FaxNumber);
                var paramEmailID = new SqlParameter("@EmailID", hospitalsContacts.EmailID);
                var paramContactName = new SqlParameter("@ContactName", hospitalsContacts.ContactName);
                var paramRefID = new SqlParameter("@RefID", hospitalsContacts.RefID);
                var paramBillType = new SqlParameter("@BillType", hospitalsContacts.BillType);
                var paramInstructions = new SqlParameter("@Instructions", hospitalsContacts.Instructions);
                var paramUserId = new SqlParameter("@UserID", userId);

                check = await _applicationDbContext.NursingHomesContactsList.FromSql("EXEC USP_tblNursingHomesContacts_Insert @ID,@ClientName,@Phone,@FaxNumber,@EmailID,@ContactName,@RefID, @BillType, @Instructions, @UserID", paramID, paramClientName, paramPhone, paramFaxNumber, paramEmailID, paramContactName, paramRefID, paramBillType, paramInstructions, paramUserId).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task<List<NursingHomesContactsList>> GetNursingHomesContactsList(int ContactsRefID, int NursingHomeID)
        {
            List<NursingHomesContactsList> check = new List<NursingHomesContactsList>();
            try
            {
                var paramID = new SqlParameter("@ContactsRefID", ContactsRefID);
                var paramNursingHomeID = new SqlParameter("@NursingHomeID", NursingHomeID);

                check = await _applicationDbContext.NursingHomesContactsList.FromSql("EXEC USP_tblNursingHomesContacts_Select @ContactsRefID,@NursingHomeID", paramID, paramNursingHomeID).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task DeleteNursingHomesContacts(int ID, string userId)
        {

            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteNursingHomesContacts @ID,@UserID", paramID, paramUserId);

            }
            catch (Exception Ex) { }
        }
        #endregion

        #region Correctional Facilitie
        public async Task<CorrectionalFacilitieOutputID> SaveCorrectionalFacilities(CorrectionalFacilities CorrectionalFacilities, string userId)
        {
            var ID = new SqlParameter("@ID", CorrectionalFacilities.CorrectionalFacilitieDetails.ID);
            var Name = new SqlParameter("@Name", CorrectionalFacilities.CorrectionalFacilitieDetails.Name);
            var Address = new SqlParameter("@Address", CorrectionalFacilities.CorrectionalFacilitieDetails.Address);
            var StateID = new SqlParameter("@StateID", CorrectionalFacilities.CorrectionalFacilitieDetails.StateID);
            var CityID = new SqlParameter("@CityID", CorrectionalFacilities.CorrectionalFacilitieDetails.CityID);
            var ZipCode = new SqlParameter("@ZipCode", CorrectionalFacilities.CorrectionalFacilitieDetails.ZipCode);
            var Phone = new SqlParameter("@Phone", "");
            var FaxNumber = new SqlParameter("@FaxNumber", "");
            var EmailID = new SqlParameter("@EmailID", "");
            var ContactName = new SqlParameter("@ContactName","");
            var Department = new SqlParameter("Department", CorrectionalFacilities.CorrectionalFacilitieDetails.Department);
            var ClientSpecific = new SqlParameter("@ClientSpecific", CorrectionalFacilities.CorrectionalFacilitieDetails.ClientSpecific);
            var ContractPricing = new SqlParameter("@ContractPricing", CorrectionalFacilities.CorrectionalFacilitieDetails.ContractPricing);
            var InvoicePreference = new SqlParameter("@InvoicePreference", CorrectionalFacilities.CorrectionalFacilitieDetails.InvoiceP);
            var InvoiceSchedule = new SqlParameter("@InvoiceSchedule", CorrectionalFacilities.CorrectionalFacilitieDetails.InvoiceSchedule);
            var LetterIncluded = new SqlParameter("@LetterIncluded", CorrectionalFacilities.CorrectionalFacilitieDetails.LetterIncluded);
            var W9 = new SqlParameter("@W9", CorrectionalFacilities.CorrectionalFacilitieDetails.W9);
            var VendorLetter = new SqlParameter("@VendorLetter", CorrectionalFacilities.CorrectionalFacilitieDetails.VendorLetter);
            var InvoiceTemplate = new SqlParameter("@InvoiceTemplate", CorrectionalFacilities.CorrectionalFacilitieDetails.InvoiceTemplate);
            var IsAlert = new SqlParameter("@IsAlert", CorrectionalFacilities.CorrectionalFacilitieDetails.IsAlert);
            var ContactsRefID = new SqlParameter("@ContactsRefID", CorrectionalFacilities.CorrectionalFacilitieDetails.ContactsRefID);
            var LastUpdatedBy = new SqlParameter("@LastUpdatedBy", userId);

            CorrectionalFacilitieOutputID correctionalFacilitieOutputID = new CorrectionalFacilitieOutputID();
            try
            {
                correctionalFacilitieOutputID = await _applicationDbContext.CorrectionalFacilitieOutputID.FromSql("EXEC [USP_tblCorrectionalFacilities_Insert] @ID,@Name, @Address,@StateID,@CityID,@ZipCode,@Phone,@FaxNumber,@EmailID,@ContactName,@Department,@ClientSpecific,@ContractPricing,@InvoicePreference,@InvoiceSchedule,@LetterIncluded,@W9,@VendorLetter,@InvoiceTemplate,@IsAlert,@ContactsRefID,@LastUpdatedBy", ID, Name, Address, StateID, CityID, ZipCode, Phone, FaxNumber, EmailID, ContactName, Department, ClientSpecific, ContractPricing, InvoicePreference, InvoiceSchedule, LetterIncluded, W9, VendorLetter, InvoiceTemplate, IsAlert, ContactsRefID, LastUpdatedBy).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

            }

            return correctionalFacilitieOutputID;
        }

        public async Task UpdateCorrectionalFacilitieFilePath(int CorrectionalFacilitieID, string letterPath, string w9Path, string vendorLetterpath, string invoiceTemplatePath)
        {
            var paramID = new SqlParameter("@ID", CorrectionalFacilitieID);
            var paramLetterPath = new SqlParameter("@LetterPath", letterPath);
            var paramW9Path = new SqlParameter("@W9Path", w9Path);
            var paramVendorLetterPath = new SqlParameter("@VendorLetterPath", vendorLetterpath);
            var paramInvoiceTemplatePath = new SqlParameter("@InvoiceTemplatePath", invoiceTemplatePath);
            CorrectionalFacilitieOutputID CorrectionalFacilitieOutputID = new CorrectionalFacilitieOutputID();
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_tblCorrectionalFacilities_UpdateFilePath] @ID,@LetterPath,@W9Path,@VendorLetterPath,@InvoiceTemplatePath", paramID, paramLetterPath, paramW9Path, paramVendorLetterPath, paramInvoiceTemplatePath);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteCorrectionalFacilities(int CorrectionalFacilitieId, string userId)
        {

            try
            {
                var paramCorrectionalFacilitieId = new SqlParameter("@CorrectionalFacilitieId", CorrectionalFacilitieId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteCorrectionalFacilities @CorrectionalFacilitieId,@UserID", paramCorrectionalFacilitieId, paramUserId);

            }
            catch (Exception Ex) { }
        }
        public async Task<List<CorrectionalFacilitiesContactsList>> AddCorrectionalFacilitiesContactsDetails(CorrectionalFacilitiesContacts hospitalsContacts, string userId)
        {
            List<CorrectionalFacilitiesContactsList> check = new List<CorrectionalFacilitiesContactsList>();
            try
            {
                var paramID = new SqlParameter("@ID", hospitalsContacts.ID);
                var paramClientName = new SqlParameter("@ClientName", hospitalsContacts.ClientName);
                var paramPhone = new SqlParameter("@Phone", hospitalsContacts.Phone);
                var paramFaxNumber = new SqlParameter("@FaxNumber", hospitalsContacts.FaxNumber);
                var paramEmailID = new SqlParameter("@EmailID", hospitalsContacts.EmailID);
                var paramContactName = new SqlParameter("@ContactName", hospitalsContacts.ContactName);
                var paramRefID = new SqlParameter("@RefID", hospitalsContacts.RefID);
                var paramBillType = new SqlParameter("@BillType", hospitalsContacts.BillType);
                var paramInstructions = new SqlParameter("@Instructions", hospitalsContacts.Instructions);
                var paramUserId = new SqlParameter("@UserID", userId);

                check = await _applicationDbContext.CorrectionalFacilitiesContactsList.FromSql("EXEC USP_tblCorrectionalFacilitiesContacts_Insert @ID,@ClientName,@Phone,@FaxNumber,@EmailID,@ContactName,@RefID, @BillType, @Instructions, @UserID", paramID, paramClientName, paramPhone, paramFaxNumber, paramEmailID, paramContactName, paramRefID, paramBillType, paramInstructions, paramUserId).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task<List<CorrectionalFacilitiesContactsList>> GetCorrectionalFacilitiesContactsList(int ContactsRefID, int CorrectionalFacilitieID)
        {
            List<CorrectionalFacilitiesContactsList> check = new List<CorrectionalFacilitiesContactsList>();
            try
            {
                var paramID = new SqlParameter("@ContactsRefID", ContactsRefID);
                var paramCorrectionalFacilitieID = new SqlParameter("@CorrectionalFacilitieID", CorrectionalFacilitieID);

                check = await _applicationDbContext.CorrectionalFacilitiesContactsList.FromSql("EXEC USP_tblCorrectionalFacilitiesContacts_Select @ContactsRefID,@CorrectionalFacilitieID", paramID, paramCorrectionalFacilitieID).ToListAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return check;
        }

        public async Task DeleteCorrectionalFacilitiesContacts(int ID, string userId)
        {

            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteCorrectionalFacilitiesContacts @ID,@UserID", paramID, paramUserId);

            }
            catch (Exception Ex) { }
        }
        #endregion

        public async Task<List<InvoicePreferenceList>> GetInvoicePreferenceList()
        {
            List<InvoicePreferenceList> check = new List<InvoicePreferenceList>();
            try
            {
                check.Add(new InvoicePreferenceList { Id = "Mail", Value = "Mail" });
                check.Add(new InvoicePreferenceList { Id = "Email", Value = "Email" });
                check.Add(new InvoicePreferenceList { Id = "Fax", Value = "Fax" });
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<InvoiceScheduleList>> GetInvoiceScheduleList()
        {
            List<InvoiceScheduleList> check = new List<InvoiceScheduleList>();
            try
            {
                check.Add(new InvoiceScheduleList { Id = "Weekly", Value = "Weekly" });
                check.Add(new InvoiceScheduleList { Id = "Bi-Monthly", Value = "Bi-Monthly" });
                check.Add(new InvoiceScheduleList { Id = "Monthly", Value = "Monthly" });
            }
            catch (Exception Ex) { }
            return check;
        }

        #region Facility Contract Name
        public async Task InsertFacilityContractName(FacilityContractNameDet FacilityContractName)
        {
            var GetName = new SqlParameter("@Name", FacilityContractName.Name);
            var GetCreatedBy = new SqlParameter("@CreatedBy", FacilityContractName.CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [InsertFacilityContractName] @Name, @CreatedBy", GetName, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<FacilityContractNameList>> GetFacilityContractName()
        {
            List<FacilityContractNameList> check = new List<FacilityContractNameList>();
            try
            {
                check = await _applicationDbContext.FacilityContractNameList.FromSql("EXEC [GetContractTitleList]").ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return check;
        }

        public async Task<List<FacilityContractNameList>> CheckFacilityContractName(String FacilityContractName)
        {
            List<FacilityContractNameList> check = new List<FacilityContractNameList>();
            try
            {
                var paramFacilityContractName = new SqlParameter("@FacilityContractName", FacilityContractName);
                check = await _applicationDbContext.FacilityContractNameList.FromSql("EXEC [dbo].[ValidFacilityContractName] @FacilityContractName", paramFacilityContractName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteFacilityContractName(int Id)
        {
            try
            {
                var paramID = new SqlParameter("@Id", Id);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteFacilityContractName @Id", paramID);

            }
            catch (Exception Ex) { }
        }

        #endregion 
        public async Task<List<ESOCompanyDetailsList>> GetESOCompanyList(string Prefix)
        {
            var Param = new SqlParameter("@SearchTerm", Prefix);
            return  await _applicationDbContext.ESOCompanyDetailsList.FromSql("EXEC GetSearchClient @SearchTerm", Param).ToListAsync();
        }

        public async Task<List<SearchClientLookup>> GetFCMSSearchClient(string Prefix)
        {
            List<SearchClientLookup> lst = new List<SearchClientLookup>();
            try
            {
                var Param = new SqlParameter("@SearchTerm", Prefix);
                lst = await _applicationDbContext.SearchClientLookup.FromSql("EXEC GetSearchClient @SearchTerm", Param).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }
        //GetSearchClientDetailsFromId
        public async Task<List<SearchClientDetails>> GetSearchClientDetailsFromId(int clientId)
        {

            List<SearchClientDetails> chkList = new List<SearchClientDetails>();
            var C_Id = new SqlParameter("@ClientId", clientId);
            try
            {
                chkList = await _applicationDbContext.SearchClientDetails.FromSql("EXEC sp_SearchClientDetailsFromClientId @ClientId", C_Id).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }
        //
        public async Task<List<AccountExecutiveList>> GetAccountExecutiveFromCompId(int comp_id)
        {
            List<AccountExecutiveList> accExcList = new List<AccountExecutiveList>();
            var paramId = new SqlParameter("@Id", comp_id);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            try
            {
                accExcList = await _applicationDbContext.AccountExecutiveList.FromSql("EXEC GetAccExecDetailFromCompanyId @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
            }
            catch(Exception Ex)
            {

            }
            return accExcList;
        }
    }
}


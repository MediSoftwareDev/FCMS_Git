using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FCMS.Models;
using FCMS.ViewModels.Admin;
using FCMS.ViewModels.Contract;
using FCMS.ViewModels.Hospitals;
using FCMS.ViewModels.Hospices;
using FCMS.ViewModels.NursingHomes;
using FCMS.ViewModels.CorrectionalFacilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WiseX.Models;
using WiseX.ViewModels.Account;
using WiseX.ViewModels.Admin;
using WiseX.ViewModels.Home;
namespace WiseX.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

        //Masters
        public virtual DbSet<ProcedureCodes> ProcedureCodes { get; set; }
        public virtual DbSet<Modifiers> Modifiers { get; set; }
        //Admin-Users
        public virtual DbSet<UserListInfo> UserListInfo { get; set; }
        public virtual DbSet<ClientsCompanyDetails> ClientsDetailsInfo { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<ProjectDetails> ProjectDetails { get; set; }
        public virtual DbSet<ProjectDetailsUsers> ProjectDetailsUsers { get; set; }
        public virtual DbSet<UserProjectRole> UserProjectRole { get; set; }
        public virtual DbSet<AllocatedList> AllocatedList { get; set; }
        public virtual DbSet<GetUserRoles> CheckUserRoles { get; set; }
        public virtual DbSet<UserDetailsTemp> UserDetailsTemp { get; set; }
        public virtual DbSet<SearchConditionFilterList> SearchConditionFilter { get; set; }
        public virtual DbSet<ChartHistoryList> ChartHistoryList { get; set; }
        public virtual DbSet<EmployeePositionList> EmployeePositionList { get; set; }

        //Common
        public virtual DbSet<ContractUSers> ContractUSersList { get; set; }
        public virtual DbSet<ContractAgreementType> ContractAgreementTypeList { get; set; }
        public virtual DbSet<CitiesList> CitiesList { get; set; }
        public virtual DbSet<StatesList> StatesList { get; set; }
        public virtual DbSet<UserOtherDetails> UserOtherDetails { get; set; }

        public virtual DbSet<ContractTitle> ContractTitleList { get; set; }




        public virtual DbSet<Contractdetails> Contractdetails { get; set; }
        public virtual DbSet<SearchClient> SearchClient { get; set; }
        public virtual DbSet<SearchClientEmployee> SearchClientEmployee { get; set; }
        public virtual DbSet<AEDetailsList> AEDetailsList { get; set; }
        public virtual DbSet<ChartProperties> ChartProperties { get; set; }
        public virtual DbSet<ChartBoxProperties> ChartBoxProperties { get; set; }
        public virtual DbSet<ChartBoxPropertiesLoad> ChartBoxPropertiesLoad { get; set; }
        public virtual DbSet<NonEmergencyFacilityBalanceLoad> NonEmergencyFacilityBalanceLoad { get; set; }
        public virtual DbSet<UserSessionDetails> UserSessionDetails { get; set; }


        public virtual DbSet<ChartBoxFacilityPropertiesLoad> ChartBoxFacilityPropertiesLoad { get; set; }

        //Admin
        public virtual DbSet<Roles> Role { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<NotificationCount> NotificationCount { get; set; }
        public virtual DbSet<AgreementTypeDetailsList> AgreementTypeList { get; set; }
        public virtual DbSet<PositionList> PositionList { get; set; }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<AccountExecutiveList> AccountExecutiveList { get; set; }
        public virtual DbSet<ResidencyCodeList> ResidencyCodeList { get; set; }

        public virtual DbSet<ClientsDetailsList> ClientsDetailsList { get; set; }
        public virtual DbSet<ClientsEmployeeList> ClientsEmployeeList { get; set; }
        public virtual DbSet<ClientContract> ClientContract { get; set; }
        public virtual DbSet<DownloadClientsDetailsList> DownloadClientsDetailsList { get; set; }
        public virtual DbSet<BulkClientsDetailsValidationList> BulkClientsDetailsValidationList { get; set; }
        public virtual DbSet<ClientsViewDetails> ClientsViewDetails { get; set; }



        //Contracts
        public virtual DbSet<ContractDetails> ContractDetails { get; set; }
        public virtual DbSet<ContractStatus> ContractStatus { get; set; }
        public virtual DbSet<CommentsDetailsList> CommentsDetailsList { get; set; }

        public virtual DbSet<ClientContractDetail> ClientContractDetail { get; set; }
        public virtual DbSet<ContractDetailApprovalList> ContractDetailApprovalList { get; set; }
        public virtual DbSet<ContractLogList> ContractLogList { get; set; }
        public virtual DbSet<ClientContractOtherDocumentDetails> ClientContractOtherDocumentDetails { get; set; }
        public virtual DbSet<ClientContractNotesDetails> ClientContractNotesDetails { get; set; }
        public virtual DbSet<NotificationDetails> NotificationDetailsList { get; set; }
        public virtual DbSet<SMTPServerDetails> SMTPServerDetails { get; set; }

        public virtual DbSet<ClientContractList> ClientContractList { get; set; }
        //public virtual DbSet<ClientContractDetails> ClientContractDetails { get; set; }
        public virtual DbSet<ClientContractsDBList> ClientContractsDBList { get; set; }


        public virtual DbSet<ClientContractFeeDetails> ClientContractFeeDetails { get; set; }


        //MenuAccess 
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<RoleModules> RoleModules { get; set; }
        public virtual DbSet<MenuAccessRole> MenuAccessRole { get; set; }
        public virtual DbSet<ContractStatusList> ContractStatusList { get; set; }


        //Hospital

        //public virtual DbSet<ContractTitleList> ClientContractTitleList { get; set; }

        public virtual DbSet<SearchHospital> SearchHospital { get; set; }
        public virtual DbSet<HospitalLogList> HospitalLogList { get; set; }
        public virtual DbSet<HospitalNotes> HospitalNotes { get; set; }
        public virtual DbSet<HospitalList> HospitalList { get; set; }
        public virtual DbSet<HospitalContractDetailsList> HospitalContractDetailsList { get; set; }

        public virtual DbSet<HospitalOutputID> HospitalOutputID { get; set; }


        public virtual DbSet<ClientSpecificList> ClientSpecificList { get; set; }


        public virtual DbSet<HospitalsContacts> HospitalsContacts { get; set; }
        public virtual DbSet<HospitalsContactsList> HospitalsContactsList { get; set; }

        //Hospice

        public virtual DbSet<SearchHospice> SearchHospice { get; set; }
        public virtual DbSet<HospiceLogList> HospiceLogList { get; set; }
        public virtual DbSet<HospiceNotes> HospiceNotes { get; set; }
        public virtual DbSet<HospiceList> HospiceList { get; set; }
        public virtual DbSet<HospiceContractDetailsList> HospiceContractDetailsList { get; set; }

        public virtual DbSet<HospiceOutputID> HospiceOutputID { get; set; }
        public virtual DbSet<HospicesContacts> HospicesContacts { get; set; }
        public virtual DbSet<HospicesContactsList> HospicesContactsList { get; set; }

        //NursingHomes

        public virtual DbSet<SearchNursingHome> SearchNursingHome { get; set; }
        public virtual DbSet<NursingHomeLogList> NursingHomeLogList { get; set; }
        public virtual DbSet<NursingHomeNotes> NursingHomeNotes { get; set; }
        public virtual DbSet<NursingHomeList> NursingHomeList { get; set; }
        public virtual DbSet<NursingHomeContractDetailsList> NursingHomeContractDetailsList { get; set; }

        public virtual DbSet<NursingHomeOutputID> NursingHomeOutputID { get; set; }
        public virtual DbSet<NursingHomesContacts> NursingHomesContacts { get; set; }
        public virtual DbSet<NursingHomesContactsList> NursingHomesContactsList { get; set; }
        //CorrectionalFacilities

        public virtual DbSet<SearchCorrectionalFacilitie> SearchCorrectionalFacilitie { get; set; }
        public virtual DbSet<CorrectionalFacilitieLogList> CorrectionalFacilitieLogList { get; set; }
        public virtual DbSet<CorrectionalFacilitieNotes> CorrectionalFacilitieNotes { get; set; }
        public virtual DbSet<CorrectionalFacilitieList> CorrectionalFacilitieList { get; set; }
        public virtual DbSet<CorrectionalFacilitieContractDetailsList> CorrectionalFacilitieContractDetailsList { get; set; }

        public virtual DbSet<CorrectionalFacilitieOutputID> CorrectionalFacilitieOutputID { get; set; }
        public virtual DbSet<CorrectionalFacilitiesContacts> CorrectionalFacilitiesContacts { get; set; }
        public virtual DbSet<CorrectionalFacilitiesContactsList> CorrectionalFacilitiesContactsList { get; set; }

        public virtual DbSet<FacilityContractNameList> FacilityContractNameList { get; set; }

        public virtual DbSet<NotesManagementFormDetails> NotesManagementFormDetails { get; set; }
        //public virtual DbSet<FCMSSearchClient> FCMSSearchClient { get; set; }
        public virtual DbSet<SearchClientDetails> SearchClientDetails { get; set; }
        public virtual DbSet<SearchClientLookup> SearchClientLookup { get; set; }
        public virtual DbSet<ESOCompanyDetailsList> ESOCompanyDetailsList { get; set; }
    }
}

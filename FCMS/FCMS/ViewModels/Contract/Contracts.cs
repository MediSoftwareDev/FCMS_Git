using FCMS.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace FCMS.ViewModels.Contract
{
    public class Contracts : EntityBase
    {
        public List<ContractDetails> ContractDetailsList;
        public ClientContractDetails ClientContractDetails;
        public List<ClientContractDetail> ClientContractDetailsList;
        public List<ContractDetailApprovalList> ContractDetailApprovalList;
        public List<ContractLogList> ContractLogList;
        public List<ContractAgreementType> ContractAgreementTypeList;
        public List<ContractUSers> ContractUSersList;
        public List<ContractStatus> ContractStatus;

        public List<Contractdetails> ContractdetailsList;
        public List<ClientContractOtherDocumentDetails> ClientContractOtherDocumentList;
        public List<ClientContractNotesDetails> ClientContractNotesDetailsList;
        public List<SearchClient> SearchClientList;
        public List<ClientsDetailsList> ClientsDetailsList;
        public ClientsDetails ClientsDetails;
        public Contractdetails Contractdetails;
        public ClientContractEditDetails ClientContractEditDetails;
        public List<SearchClientEmployee> SearchClientEmployeeList;

        public List<ContractTitle> ContractTitleList;
        public ClientContractNotesDetails ClientContractNotesDetails;
        public ClientContractFeeDetails ClientContractFeeDetails;

        public List<ClientContractsDBList> ClientContractsDBList;
    }

    public class Contract : EntityBase
    {
        [Key]
        public int ID { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string State { get; set; }
        public string Active { get; set; }
        public int TotalCount { get; set; }
    }
    public class ContractDetails
    {
        [Key]
        public int Id { get; set; }
        public int ContractID { get; set; }
        public string Version { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ResidencyCode { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public string Status { get; set; }
        public string FileName { get; set; }
        public int TotalCount { get; set; }


    }
    public class ContractStatus
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class UpdateContractStatus
    {
        [Key]
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public int ContractStatusID { get; set; }
    }
    public class ContractUSers
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class UserOtherDetails
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    //public class ClientContractDetails : EntityBase
    //{
    //    public int ID { get; set; }
    //    public int ClientId { get; set; }
    //    //public string CompanyId { get; set; }
    //    public string ContractTitle { get; set; }
    //    public int AgreementTypeId { get; set; }
    //    public string ContractSignedDate { get; set; }
    //    public string ContractEffDate { get; set; }
    //    public string ContractExpiryDate { get; set; }
    //    public string ContractLength { get; set; }
    //    public int ReferenceID { get; set; }
    //    public string Version { get; set; }

    //    //public string Stage { get; set; }
    //    //public bool Status{ get; set; }
    //    //public bool SendToAE { get; set; }
    //    //public string Agreement { get; set; }
    //    //public string Comments { get; set; }
    //    //public int Users { get; set; }
    //    //public string Sentby { get; set; }

    //    public string Active { get; set; }
    //    public string CreatedBy { get; set; }
    //    public string CreatedOn { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public string ModifiedOn { get; set; }

    //}

    //public class ClientContractList
    //{
    //    public int ID { get; set; }
    //    public string ContractTitle { get; set; }
    //    public string ContractSignedDate { get; set; }
    //    public string ContractEffDate { get; set; }
    //    public string ContractExpiryDate { get; set; }
    //    public string ContractLength { get; set; }
    //    public int ReferenceID { get; set; }
    //    public string Version { get; set; }
    //}

    public class ClientContractEditDetails : EntityBase
    {
        public int ID { get; set; }
        public int ClientId { get; set; }
        public int AgreementType { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public int ReferenceID { get; set; }
    }
    public class NotesDetails : EntityBase
    {

        public string Comments { get; set; }
    }
    public class ClientContractDetail
    {
        public int ID { get; set; }
        public int ContractTitleId { get; set; }
        public string ContractTitle { get; set; }
        public int FID { get; set; }
        //public int StatusID { get; set; }
        public string Version { get; set; }
        public int IsLatestVersion { get; set; }
        public int ContractId { get; set; }
        public string CompanyId { get; set; }
        public string AgreementType { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractExpiryDate { get; set; }
        //public string Status { get; set; }
        public string FileName { get; set; }
        public int TotalCount { get; set; }
        public string SentTo { get; set; }
        public int? Sentby { get; set; }
        public int ReferenceID { get; set; }
        //public int AgreementTypeId { get; set; }
        public string Active { get; set; }
        public string CompanyName { get; set; }
        public string AccountExecutiveName { get; set; }

    }
    public class ContractDetailApprovalList
    {
        public int ID { get; set; }
        public string AgreementType { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractExpiryDate { get; set; }
    }

    public class ContractAgreementType
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class SearchClient
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        //public int ContractRefID { get; set; }
    }

    public class SearchClientEmployee
    {
        [Key]
        public int Id { get; set; }
        public string value { get; set; }
        public string Email { get; set; }
    }


    public class ContractFileDetails : EntityBase
    {
        public int ClientID { get; set; }
        public int ContractID { get; set; }
        public string FileName { get; set; }
        public string OrgFileName { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public string UserId { get; set; }
        public string FilePath { get; set; }
    }

    public class Contractdetails
    {
        [Key]
        public int ID { get; set; }
        public int ClientId { get; set; }
        public int AggrementTypeId { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public bool Active { get; set; }
        public int ReferenceID { get; set; }
    }

    public class AEDetailsList
    {
        [Key]

        public int ClientId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

    }
    public class EmailContractDetails : EntityBase
    {
        [Key]
        public int ContractID { get; set; }
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public string UserId { get; set; }
        public int SendTo { get; set; }
        public string Comments { get; set; }
        public bool Email { get; set; }
    }

    public class ContractLogList
    {
        [Key]
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public string LogTime { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string Version { get; set; }
    }

    public class ClientContractOtherDocumentDetails
    {
        [Key]
        public int ID { get; set; }
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public string DocumentName { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedOn { get; set; }
        public string FileName { get; set; }
    }

    public class ClientContractNotesDetails
    {
        [Key]
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public string Notes { get; set; }
        public string AddedOn { get; set; }
        public string AddedBy { get; set; }
        public string UserID { get; set; }
        public string ContractTitle { get; set; }
    }

    public class SMTPServerDetails
    {
        [Key]
        public int Id { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public bool SMTPSSL { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public string SentoUserName { get; set; }
    }


    public class ContractTitle
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class ClientContractFeeDetails
    {
        [Key]
        public int ContractID { get; set; }
        public double ALS { get; set; }
        public double ALSNE { get; set; }
        public double ALS2 { get; set; }
        public double BLS { get; set; }
        public double BLSNE { get; set; }
        public double Mileage { get; set; }
        public string Notes { get; set; }
    }

    public class ClientContractsDBList
    {
        [Key]
        public int Id { get; set; }
        public int ContractTitleID { get; set; }
        public string ContractTitle { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public string AccountExecutiveName { get; set; }
        public string Active { get; set; }
    }
}

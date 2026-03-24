using FCMS.ViewModels.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace FCMS.ViewModels.Admin
{
    public class Clients : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public int ReferenceId { get; set; }
        public int ContractId { get; set; }
        public ClientsDetails ClientsDetails;
        public List<ClientsDetailsList> ClientsDetailsList;
        public List<ContractAgreementType> ContractAgreementTypeList;
        public List<AccountExecutiveList> AccountExecutiveList;
        public List<ResidencyCodeList> ResidencyCodeList;
        public List<CitiesList> CitiesList;
        public List<StatesList> StatesList;
        public List<ClientsCompanyDetails> ClientsCompanyDetails;

        public List<DownloadClientsDetailsList> DownloadClientsDetailsList;
        public List<EmployeePositionList> EmployeePositionList;
        public List<ClientsEmployeeList> ClientsEmployeeList;
        public List<BulkClientsDetails> BulkClientsDetailsList;

        public string RoleAccess { get; set; }


        public List<ClientContractList> ClientContractList;
        public ClientContractDetails ClientContractDetails;

        public ContractTitle ContractTitle;
        public List<ContractTitle> ContractTitleList;
    }
    public class ClientsDetails : EntityBase
    {

        [Key]
        public int ID { get; set; }
        public int EmpID { get; set; }
       // public int ReferenceID { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ResidencyCode { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractEffDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public string AccountExecutiveID { get; set; }
        public string AccountExecutiveName { get; set; }
        public string ContractLength { get; set; }
        public double Amount { get; set; }
        public string LocationStreetAddress { get; set; }
        public int AgreementTypeId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string ZipCode { get; set; }
        public int EmployeePosition { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public bool Active { get; set; }
        public bool Transport { get; set; }
        public int TransportCharges { get; set; }
        //public string ResidencyCode { get; set; }
        public int ResidencyCodeID { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public double Year1 { get; set; }
        public double Year2 { get; set; }
        public double Year3 { get; set; }
        public double Year4 { get; set; }
        public string FeesChangedDate { get; set; }
        public string Notes { get; set; }
        public int ContractStatusID { get; set; }
        public int ContractTitleId { get; set; }
        public int ContractID { get; set; }
        public int ContractRefID { get; set; }

        public double ALS { get; set; }
        public double ALSNE { get; set; }
        public double ALS2 { get; set; }
        public double BLS { get; set; }
        public double BLSNE { get; set; }
        public double Mileage { get; set; }

        public bool Contract { get; set; }
    }
    public class ClientsDetailsList
    {
        [Key]
        public int ID { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string AccountExecutiveID { get; set; }
        public string AccountExecutiveName { get; set; }
        public string LocationStreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Status { get; set; }
        public bool Active { get; set; }
        public int ResidencyCodeID { get; set; }
        public string ResidencyCode { get; set; }
        public double Year1 { get; set; }
        public double Year2 { get; set; }
        public double Year3 { get; set; }
        public double Year4 { get; set; }
        public int ContractRefID { get; set; }
        //public int ReferenceID { get; set; }
        //public string ContractSignedDate { get; set; }
        //public string ContractRenewedDate { get; set; }
        //public string ContractExpiryDate { get; set; }
        // public int AgreementTypeId { get; set; }
        //public string ContractLength { get; set; }
        //public double Amount { get; set; }
        //public bool Transport { get; set; }
        //public int TransportCharges { get; set; }
        //public string FeesChangedDate { get; set; }
        //public string Notes { get; set; }
        //public string ContractTitle { get; set; }
        //public string ConActive { get; set; }
        //public int ContractID { get; set; }
    }
    public class DownloadClientsDetailsList
    {
        [Key]
        public int SNo { get; set; }
        public string Company_ID { get; set; }
        public string Client_Name { get; set; }
        public string Residency_Code { get; set; }
        public string Facility_Contract_Title { get; set; }
        public string Contract_Start_Date { get; set; }
        public string Contract_Renewed_Date { get; set; }
        public string Contract_End_Date { get; set; }
        public string Account_Owner { get; set; }
        public string LocationStreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Status { get; set; }
    }

    public class AccountExecutiveList
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class ResidencyCodeList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class ClientContract
    {
        [Key]
        public int ClientId { get; set; }
        public int ContractId { get; set; }
    }
    public class EmployeePositionList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class CitiesList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class StatesList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class ClientsEmployeeDetails : EntityBase
    {

        [Key]
        public int ID { get; set; }
        public int EmployeePosition { get; set; }
        public int ReferenceID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
    public class ClientsEmployeeList
    {

        [Key]
        public int ID { get; set; }
        public int PositionId { get; set; }
        public string EmployeePosition { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int ReferenceID { get; set; }
    }

    public class BulkClientsDetails : EntityBase
    {

        [Key]
        public int ID { get; set; }
        public string Client { get; set; }
        public string Description { get; set; }
        public string ResidencyCode { get; set; }
        public string Fee1 { get; set; }
        public string Fee2 { get; set; }
        public string Fee3 { get; set; }
        public string Fee4 { get; set; }
        public string ContractStartSate { get; set; }
        public string ContractRenewed { get; set; }
        public string ContractExpires { get; set; }
        public string AccountOwner { get; set; }
        public string Chief { get; set; }
        public string Cemail { get; set; }
        public string Fiscal { get; set; }
        public string Femail { get; set; }
        public string PhysicalLocationStreet { get; set; }
        public string PhysicalLocationCity { get; set; }
        public string PhysicalLocationState { get; set; }
        public string PhysicalLocationZip { get; set; }
        public string ContractFileName { get; set; }
        public string ContractFilePath { get; set; }
        public string ContractFileMimeType { get; set; }
        public string NonTransportBilled { get; set; }
        public string NonTransportCharges { get; set; }
        public string ErrorMessage { get; set; }
        public string Notes { get; set; }
    }

    public class BulkClientsDetailsValidationList
    {
        [Key]
        public string Client { get; set; }
        public string Description { get; set; }
        public string ResidencyCode { get; set; }
        public string Fee1 { get; set; }
        public string Fee2 { get; set; }
        public string Fee3 { get; set; }
        public string Fee4 { get; set; }
        public string ContractStartSate { get; set; }
        public string ContractRenewed { get; set; }
        public string ContractExpires { get; set; }
        public string AccountOwner { get; set; }
        public string Chief { get; set; }
        public string Cemail { get; set; }
        public string Fiscal { get; set; }
        public string Femail { get; set; }
        public string PhysicalLocationStreet { get; set; }
        public string PhysicalLocationCity { get; set; }
        public string PhysicalLocationState { get; set; }
        public string PhysicalLocationZip { get; set; }
        public string ContractFileName { get; set; }
        public string ContractFilePath { get; set; }
        public string ContractFileMimeType { get; set; }
        public string NonTransportBilled { get; set; }
        public string NonTransportCharges { get; set; }
        public string ErrorMessage { get; set; }
        public string Notes { get; set; }
    }

    public class ClientsViewDetails
    {
        [Key]
        public int ID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractRenewedDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public string AccountExecutiveName { get; set; }
        public string AgreementType { get; set; }
        public string ContractLength { get; set; }
        public Decimal Amount { get; set; }
        public string LocationStreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string Status { get; set; }
        public string Active { get; set; }
        public string Transport { get; set; }
        public int TransportCharges { get; set; }
        public string ResidencyCode { get; set; }
    }
    public class ClientsCompanyDetails
    {

        [Key]
        public int ID { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
    public class ClientContractDetails : EntityBase
    {
        [Key]
        public int ID { get; set; }
        public int ClientId { get; set; }
        public string CompanyId { get; set; }
        public int ContractTitleId { get; set; }
        public int AgreementTypeId { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractEffDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public string ContractLength { get; set; }
        public int ReferenceID { get; set; }
        public string Version { get; set; }

        //public string Stage { get; set; }
        //public bool Status{ get; set; }
        //public bool SendToAE { get; set; }
        //public string Agreement { get; set; }
        public string Comments { get; set; }
        public int Users { get; set; }
        //public string Sentby { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public double ALS { get; set; }
        public double ALSNE { get; set; }
        public double ALS2 { get; set; }
        public double BLS { get; set; }
        public double BLSNE { get; set; }
        public double Mileage { get; set; }
    }

    public class ClientContractList
    {
        [Key]
        public int ID { get; set; }
        public int ContractTitleId { get; set; }
        public string ContractTitle { get; set; }
        public string ContractSignedDate { get; set; }
        public string ContractEffDate { get; set; }
        public string ContractExpiryDate { get; set; }
        public string ContractLength { get; set; }
        public int ReferenceID { get; set; }
        public string Version { get; set; }
        public bool Active { get; set; }
        public double ALS { get; set; }
        public double ALSNE { get; set; }
        public double ALS2 { get; set; }
        public double BLS { get; set; }
        public double BLSNE { get; set; }
        public double Mileage { get; set; }
        public int AgreementTypeId { get; set; }
    }
}

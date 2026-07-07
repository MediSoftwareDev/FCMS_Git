using FCMS.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WiseX.Helpers;


namespace FCMS.ViewModels.Hospitals
{
    public class Hospitals : EntityBase
    {
        public int RefId { get; set; }
        public int ContactID { get; set; }

        public List<SearchHospital> SearchHospitalList;
        public List<HospitalLogList> HospitalLogList;
        public List<HospitalNotes> HospitalNotesList;

        public List<HospitalList> HospitalList;
        public HospitalDetails HospitalDetails;
        public HospitalNotes HospitalNotes;

        public HospitalOutputID HospitalOutputID;


        public HospitalsContacts HospitalsContacts;
        public List<HospitalsContactsList> HospitalsContactsList;

        public List<CitiesList> CitiesList;
        public List<StatesList> StatesList;
        public List<ClientSpecificList> ClientSpecificList;

        public string RoleAccess { get; set; }

        public List<HospitalContractDetailsList> HospitalContractDetailsList;

        public List<InvoiceScheduleList> InvoiceScheduleList;
        public List<InvoicePreferenceList> InvoicePreferenceList;
    }

    public class InvoiceScheduleList
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
    public class InvoicePreferenceList
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }

    public class SearchHospital
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class HospitalList
    {
        [Key]
        public int ID { get; set; }
        //public string PayorID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailID { get; set; }
        public string ContactName { get; set; }
        public string Department { get; set; }
        public string ClientSpecific { get; set; }
        public string ClientSpecificName { get; set; }
        public bool ContractPricing { get; set; }
        public string InvoiceP { get; set; }
        public string InvoiceSchedule { get; set; }
        public bool LetterIncluded { get; set; }
        public string LetterPath { get; set; }
        public bool W9 { get; set; }
        public string W9Path { get; set; }
        public bool VendorLetter { get; set; }
        public string VendorPath { get; set; }
        public bool InvoiceTemplate { get; set; }
        public string InvoiceTemplatePath { get; set; }
        public bool IsAlert { get; set; }
        public int ContactsRefID { get; set; }
        public string Notes { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string BillType { get; set; }
        public string Instructions { get; set; }
    }

    public class HospitalDetails : EntityBase
    {
        [Key]
        public int ID { get; set; }
        //public string PayorID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailID { get; set; }
        public string ContactName { get; set; }
        public string Department { get; set; }
        public string ClientSpecific { get; set; }
        public string ClientSpecificName { get; set; }
        public bool ContractPricing { get; set; }
        public string InvoiceP { get; set; }
        public string InvoiceSchedule { get; set; }
        public bool LetterIncluded { get; set; }
        public string LetterPath { get; set; }
        public bool W9 { get; set; }
        public string W9Path { get; set; }
        public bool VendorLetter { get; set; }
        public string VendorPath { get; set; }
        public bool InvoiceTemplate { get; set; }
        public string InvoiceTemplatePath { get; set; }
        public bool IsAlert { get; set; }
        public int ContactsRefID { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string BillType { get; set; }
        public string Instructions { get; set; }
    }

    public class ClientSpecificList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class HospitalNotes
    {
        [Key]
        public int ID { get; set; }
        public int HospitalId { get; set; }
        public string Notes { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
    }


    public class HospitalLogList
    {
        [Key]
        public int ID { get; set; }
        public int HospitalId { get; set; }
        public string LogTime { get; set; }
        public string UserName { get; set; }
        public string Comments { get; set; }
    }

    public class HospitalOutputID
    {
        [Key]
        public int ID { get; set; }
    }

    public class HospitalContractDetailsList
    {
        [Key]
        public int ID { get; set; }
        public string Version { get; set; }
        public string CompanyName { get; set; }
        public double ALS1 { get; set; }
        public double ALS2 { get; set; }
        public double BLS { get; set; }
        public double Mileage { get; set; }
    }

    public class HospitalsContacts : EntityBase
    {
        [Key]
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailID { get; set; }
        public string ContactName { get; set; }
        public int RefID { get; set; }
        public string BillType { get; set; }
        public string Instructions { get; set; }
    }

    public class HospitalsContactsList
    {
        [Key]
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailID { get; set; }
        public string ContactName { get; set; }
        public int RefID { get; set; }
        public string BillType { get; set; }
        public string Instructions { get; set; }
    }
}

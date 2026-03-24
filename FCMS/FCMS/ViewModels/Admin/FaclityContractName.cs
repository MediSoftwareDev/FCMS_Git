using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace FCMS.ViewModels.Admin
{
    public class FacilityContractNameDetails : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public FacilityContractNameDet FacilityContractNameDet;
        public List<FacilityContractNameList> FacilityContractNameList;
    }
    public class FacilityContractNameDet : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte IsDeleted { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
    public class FacilityContractNameList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsUsed { get; set; }

    }
}

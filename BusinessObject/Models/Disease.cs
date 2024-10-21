using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Disease
    {
        public Disease()
        {
            MemberDiseases = new HashSet<MemberDisease>();
        }

        public int DiseaseId { get; set; }
        public string? DiseaseName { get; set; }
        public string? DiseaseDescription { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ChangeBy { get; set; }

        public virtual ICollection<MemberDisease> MemberDiseases { get; set; }
    }
}

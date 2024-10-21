using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class MemberDisease
    {
        public int IdMemberDisease { get; set; }
        public DateTime? DiagnosedDate { get; set; }
        public bool? Status { get; set; }
        public int? MemberId { get; set; }
        public int? DiseaseId { get; set; }

        public virtual Disease? Disease { get; set; }
        public virtual Member? Member { get; set; }
    }
}

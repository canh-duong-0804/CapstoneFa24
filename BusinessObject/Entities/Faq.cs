﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Entities
{
    public partial class Faq
    {
        public int QuestionId { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public DateTime? CreatDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public bool? Status { get; set; }
    }
}

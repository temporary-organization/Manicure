﻿using System;
using System.Collections.Generic;

namespace Manicure.Common.Domain
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public string MasterId { get; set; }

        public Master Master { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<ProcedureEntry> ProcedureEntries { get; set; }
    }
}
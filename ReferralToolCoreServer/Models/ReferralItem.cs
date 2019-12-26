using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferralToolCoreServer.Models
{
    public class ReferralItem
    {
        public string ID { get; set; }
        public string PatientName { get; set; }
        public string CAD { get; set; }
        public string CallStatus { get; set; }
        public string DateOfDischarge { get; set; }
        public string RequestedTime { get; set; }
        public string CallTaker { get; set; }
        public string Nature { get; set; }
        public string Provider { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class CaseInfo
    {
        public CaseDetails caseDetails { get; set; }
        public List<SubscriberData> subscriberDatas { get; set; }
        public List<SubscriberAddress> subscriberAddresses { get; set; }
    }
}
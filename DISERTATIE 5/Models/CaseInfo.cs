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
        public List<SubscriberPhone> subscriberPhones { get; set; }
        public List<SubscriberEmail> subscriberEmails { get; set; }
        public List<SubscriberContact> subscriberContacts { get; set; }
        public List<SubscriberEmployer> subscriberEmployers { get; set; }
        public FinAccountDetails FinAccountDetails { get; set; }
        public List<FinancialItem> financialItems { get; set; }
        public List<string> stornoReasons { get; set; }
        public AllocationsData AllocationsData { get; set; }
        public List<string> emailTemplates { get; set; }
        public List<Emails> emails { get; set; }
    }
}
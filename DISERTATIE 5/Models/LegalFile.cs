using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class LegalFile
    {
        public int legalFileID { get; set; }
        public string fileNumber { get; set; }
        public string status { get; set; }
        public DateTime startDate { get; set; }
        public string court { get; set; }
        public string bailiff { get; set; }
        public string lawyer { get; set; }
        public string notary { get; set; }
    }
}
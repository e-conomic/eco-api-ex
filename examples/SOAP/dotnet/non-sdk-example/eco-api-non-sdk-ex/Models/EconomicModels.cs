using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Api_nonSDK_example.EcoApiServiceReference;

namespace Api_nonSDK_example.Models
{
    public class EconomicModels
    {
        public static ProductGroupData ProductGroup { get; set; }
        public static ProductData[] Products { get; set; }
        public static DebtorGroupData DebtorGroup { get; set; }
        public static DebtorData Debtor { get; set; }
        public static OrderData Order { get; set; }
    }
}
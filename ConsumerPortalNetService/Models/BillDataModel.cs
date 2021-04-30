using ConsumerPortalNetService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerPortalNetService.Models
{
    public class BillDataModel
    {
        //public Data data { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string Account_number { get; set; }
        [JsonProperty(PropertyName = "consumerNumber")]
        public string Consumer_Number { get; set; }
        [JsonProperty(PropertyName = "consumerName")]
        public string Customer_Name { get; set; }
        [JsonProperty(PropertyName = "consumerAddress")]
       public string Customer_Address { get; set; }
        [JsonProperty(PropertyName = "dueDate")]
        public string Due_date { get; set; }
        [JsonProperty(PropertyName = "issueDate")]
        public string Issue_date { get; set; }
        [JsonProperty(PropertyName = "netAmt")]
        public string Rounded_net { get; set; }
        [JsonProperty(PropertyName = "Amt After Due Date")]
        public string Rounded_gross { get; set; }
        [JsonProperty(PropertyName = "billId")]
        public string Bill_id { get; set; }
        [JsonProperty(PropertyName = "billingMonth")]
        public string Billing_month { get; set; }
        [JsonProperty(PropertyName = "billType")]
        public string Bill_type { get; set; }
        [JsonProperty(PropertyName = "bocName")]
        public string BOC_NAME { get; set; }
        [JsonProperty(PropertyName = "sysInd")]
        public string System_ind { get; set; }


        //public class Data
        //{
        //    public string Message { get; set; }
        //    public string Status { get; set; }
        //    public string Account_number { get; set; }
        //    public string Consumer_Number { get; set; }
        //    public string Customer_Name { get; set; }
        //    public string Customer_Address { get; set; }
        //    public string Due_date { get; set; }
        //    public string Issue_date { get; set; }
        //    public string Rounded_net { get; set; }
        //    public string rounded_gross { get; set; }
        //    public string bill_id { get; set; }
        //    public string billing_month { get; set; }
        //    public string bill_type { get; set; }
        //    public string BOC_NAME { get; set; }
        //    public string System_ind { get; set; }
        //}
    }

}

public class BillResponse
{
    public string Message { get; set; }
    public string Status { get; set; }

    public IList<BillDataModel> duplicateBills = new List<BillDataModel>();
}

public class BillPDFResponse
{
    public string Message { get; set; }
    public string Status { get; set; }

    public string billPDFData { get; set; }

}
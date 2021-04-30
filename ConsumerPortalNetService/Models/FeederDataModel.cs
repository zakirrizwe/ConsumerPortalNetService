using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsumerPortalNetService.Models
{
    public class FeederDataModel
    {
       public Data d { get; set; }
    }
    public class Data
    {
        public string Message { get; set; }
        public string Status { get; set; }
        [JsonProperty(PropertyName = "dtsID")]
        public string DTS_ID { get; set; }
        [JsonProperty(PropertyName = "dtsName")]
        public string DTS_NAME { get; set; }
        [JsonProperty(PropertyName = "brkID")]
        public string BRK_ID { get; set; }
        [JsonProperty(PropertyName = "region")]
        public string REGION { get; set; }
        [JsonProperty(PropertyName = "grid")]
        public string GRID { get; set; }
        [JsonProperty(PropertyName = "ibc")]
        public string IBC { get; set; }
        [JsonProperty(PropertyName = "lossCategory")]
        public string LOSSCATEGORY { get; set; }
        [JsonProperty(PropertyName = "faultSource")]
        public string FAULT_SOURCE { get; set; }
        [JsonProperty(PropertyName = "outageType")]
        public string OUTAGETYPE{ get; set; }
        [JsonProperty(PropertyName = "remarks")]
        public string REMARKS { get; set; }
        [JsonProperty(PropertyName = "feederID")]
        public string FDR_ID { get; set; }
        [JsonProperty(PropertyName = "feederName")]
        public string FEEDERNAME { get; set; }
        [JsonProperty(PropertyName = "theftRatio")]
        public string THEFTRATIO { get; set; }
        [JsonProperty(PropertyName = "bfDate")]
        public string BF_DATE { get; set; }
        [JsonProperty(PropertyName = "bfTime")]
        public string BF_TIME { get; set; }
        [JsonProperty(PropertyName = "feederStatus")]
        public string FEEDERSTATUS { get; set; }
        [JsonProperty(PropertyName = "areaStatus")]
        public string AREASTATUS { get; set; }
        [JsonProperty(PropertyName = "faultReportDate")]
        public string FAULT_REPORT_DATE { get; set; }
        [JsonProperty(PropertyName = "faultReportTime")]
        public string FAULT_REPORT_TIME { get; set; }
        [JsonProperty(PropertyName = "consCount")]
        public string CONS_COUNT { get; set; }

    }
}
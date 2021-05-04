using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ConsumerPortalNetService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPC" in both code and config file together.
    [ServiceContract]
    public interface IPC
    {
        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        //Stream GetFeederData(string DTSID);


        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        //Stream GetDuplicateBills(string ContractNo, int Month = 6);


        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        Stream GetBillPDF(string ContractNo, string BillingMonth);
    }
}

using ConsumerPortalNetService.Models;
using dupbill;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
//using Paperless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsumerPortalNetService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PC" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PC.svc or PC.svc.cs at the Solution Explorer and start debugging.
    public class PC : IPC
    {
        //public Stream GetFeederData(string DTSID)
        //{
        //    FeederDataModel fdrResp = new FeederDataModel();
        //    Data dataResp = new Data();

        //    OracleConnection conn = new OracleConnection();
        //    conn.ConnectionString = ConfigurationManager.ConnectionStrings["NSISConnectionStringCurrent"].ConnectionString;
        //    conn.Open();

        //    try

        //    {

        //        //  string dtsId = "FDR-003584";
        //        string[] split = DTSID.Split('-');
        //        string numericDtsId = split[1];


        //        if (conn.State == ConnectionState.Open)
        //        {
        //            OracleCommand cmd = new OracleCommand();
        //            cmd.CommandText = "select * from fdr_status_cons_portal where DTS_ID=" + numericDtsId;
        //            cmd.Connection = conn;

        //            OracleDataReader dataReader = cmd.ExecuteReader();
        //            List<string> colNames = new List<string>();
        //            int fieldCount = dataReader.FieldCount;

        //            for (int i = 0; i < fieldCount; i++)
        //            {
        //                colNames.Add(dataReader.GetName(i));
        //            }

        //            while (dataReader.Read())
        //            {
        //                for (int j = 0; j < fieldCount; j++)
        //                {
        //                    System.Reflection.PropertyInfo info = dataResp.GetType().GetProperty(colNames[j]);
        //                    if (!dataReader.IsDBNull(j))
        //                    {

        //                        info.SetValue(dataResp, dataReader[j].ToString(), null);
        //                    }
        //                    else
        //                    {
        //                        info.SetValue(dataResp, "(null)", null);
        //                    }

        //                }

        //            }

        //            System.Reflection.PropertyInfo infoMessage = dataResp.GetType().GetProperty("Message");
        //            infoMessage.SetValue(dataResp, "Success", null);

        //            System.Reflection.PropertyInfo infoStatus = dataResp.GetType().GetProperty("Status");
        //            infoStatus.SetValue(dataResp, "0", null);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        System.Reflection.PropertyInfo infoMessage = dataResp.GetType().GetProperty("Message");
        //        infoMessage.SetValue(dataResp, "Error:" + ex.Message, null);

        //        System.Reflection.PropertyInfo infoStatus = dataResp.GetType().GetProperty("Status");
        //        infoStatus.SetValue(dataResp, "1", null);

        //    }

        //    if (conn.State == ConnectionState.Open)
        //    {
        //        conn.Close();
        //    }
        //    conn.Dispose();

        //    fdrResp.d = dataResp;

        //    string jsonClient = JsonConvert.SerializeObject(fdrResp);

        //    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
        //    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));

        //}



        //public Stream GetDuplicateBills(string ContractNo, int Month = 6)
        //{
        //    string m_connstr = ConfigurationManager.ConnectionStrings["TApplicationServices"].ToString();
        //    List<BillDataModel> billDataList = new List<BillDataModel>();
        //    RequestResponseDTO<BillResponse> data = new RequestResponseDTO<BillResponse>();
        //    BillResponse billresponse = new BillResponse();

        //    try
        //    {
        //        //Regex regex = new Regex(@" ^\d+$");
        //        bool n = Regex.IsMatch(ContractNo, @"^[0-9]*$");
        //        if (!n)
        //        {
        //            throw new Exception();

        //        }
        //        if (ContractNo.Length < 13 || ContractNo.Length > 13)
        //        {
        //            throw new Exception();
        //        }


        //        SqlConnection conn = null;
        //        SqlDataReader reader = null;
        //        conn = new SqlConnection(m_connstr);
        //        conn.Open();
        //        SqlCommand cmd = null;

        //        cmd = new SqlCommand("getByAccountNumberRedesign", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@Account_number", ContractNo));

        //        reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            System.Diagnostics.Debug.WriteLine(reader[2]);

        //            Type type = typeof(BillDataModel);
        //            BillDataModel pm = new BillDataModel();
        //            foreach (var col in type.GetProperties())
        //            {
        //                System.Reflection.PropertyInfo info = pm.GetType().GetProperty(col.Name);
        //                if (reader[col.Name] == System.DBNull.Value)
        //                {
        //                    info.SetValue(pm, "", null);
        //                }
        //                else
        //                {
        //                    info.SetValue(pm, reader[col.Name], null);
        //                }
        //            }
        //            //send data for bills after redesign
        //            if (pm.System_ind.Equals("N"))
        //            {
        //                billDataList.Add(pm);
        //            }
        //        }
        //        var count = billDataList.Count;
        //        conn.Close();
        //        conn.Dispose();

        //        //var billlist = JsonConvert.SerializeObject(billDataList);

        //        billresponse.duplicateBills = billDataList;
        //        billresponse.Status = "0";
        //        billresponse.Message = "Success";

        //    }
        //    catch (Exception ex)
        //    {
        //        billresponse.duplicateBills = billDataList;
        //        billresponse.Status = "1";
        //        billresponse.Message = "Error";
        //    }
        //    //return requestStatus;


        //    data.d = billresponse;

        //    string jsonClient = JsonConvert.SerializeObject(data);

        //    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
        //    return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
        //}

        public Stream GetBillPDF(string ContractNo, string BillingMonth)
        {
           
            ResponseDTO<string> requestStatus = new ResponseDTO<string>();
            string m_connstr = ConfigurationManager.ConnectionStrings["TApplicationServices"].ToString();
            MemoryStream ms;
            try
            {
                bool n = Regex.IsMatch(ContractNo, @"^[0-9]*$");
                if (!n)
                {
                    throw new Exception();

                }
                if (ContractNo.Length < 13 || ContractNo.Length > 13)
                {
                    throw new Exception();
                }

                ////ISK Fix to make date from MMM-yyyy to MMM-yy
                //if (BillingMonth.Length == 8)
                //{
                //    String TempBM = BillingMonth.Substring(0, 4) + BillingMonth.Substring(6, 2);
                //    BillingMonth = TempBM;
                //}
                ////Fix End

                ms = new GeneratePDF().makePDF(ContractNo, BillingMonth, "WEB");
                byte[] info = ms.ToArray();

                requestStatus.Data = Convert.ToBase64String(ms.ToArray());
                requestStatus.Status = "0";
                requestStatus.Message = "Success";


                //To check accurate convertion into Bass64
                MemoryStream ss = new MemoryStream(Convert.FromBase64String(requestStatus.Data));
                using (FileStream file = new FileStream(@"D:\ISK\Development\Bill Generation using Base64\ConsumerPortalNetService\PDFs\"
                    + string.Format(@"{0}.pdf", Guid.NewGuid()), FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[ss.Length];
                    ms.Read(bytes, 0, (int)ss.Length);
                    file.Write(bytes, 0, bytes.Length);
                }

            }
            catch (Exception ex)
            {
                requestStatus.Data = "";
                requestStatus.Status = "1";
                //CommonFunction cf = new CommonFunction();
                //cf.errorlogs("Paperless attachment email", ex.StackTrace + "\t" + ex.Message.ToString() + "\t" + ex.Source);
                requestStatus.Message = "Error" + ex.StackTrace + ex.Message + ex.Source + ex.TargetSite.ToString();
                
            }
            //return requestStatus;




            RequestResponseDTO<BillPDFResponse> data = new RequestResponseDTO<BillPDFResponse>();
            BillPDFResponse billpdfmodel = new BillPDFResponse();
            billpdfmodel.Message = requestStatus.Message;
            billpdfmodel.Status = requestStatus.Status;
            billpdfmodel.billPDFData = requestStatus.Data;

            data.d = billpdfmodel;

            string jsonClient = JsonConvert.SerializeObject(data);

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(jsonClient));
        }
    }
}

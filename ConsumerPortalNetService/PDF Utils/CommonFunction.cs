using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
//using SAP.Middleware.Connector;

namespace ConsumerPortalNetService
{
    public class CommonFunction
    {
        public DataTable dTable { get; set; }

        Encrypt ex = new Encrypt();

        public DataTable QueryData(string SqlQuery)
        {
            int i = 0;
            SqlConnection sqlConn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ConnectionString));
            SqlCommand sqlComm = new SqlCommand();
            SqlDataAdapter dA = new SqlDataAdapter();
            DataTable dTable = new DataTable();
            try
            {
                sqlComm.Connection = sqlConn;
                sqlComm.CommandText = SqlQuery;
                dA.SelectCommand = sqlComm;
                sqlConn.Open();
                dA.Fill(dTable);
            }
            catch (Exception x)
            {

                throw x;
            }
            finally
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            return dTable;
        }


        public int ExecuteSPR(string SP_Name, SqlParameter[] sqlParams)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ConnectionString));
            conn.Open();
            cmd.CommandText = SP_Name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;


            // Return value as parameter
            SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
            //returnValue.Direction = ParameterDirection.Output;
           // cmd.Parameters.Add(returnValue);

            for (int i = 0, j = sqlParams.Length; i < j; i++)
            {
                cmd.Parameters.Add(sqlParams[i]);
            }

            // Execute the stored procedure

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();

            return int.Parse("0");
        }

        public int ExecuteSql(string SqlQuery)
        {
            int i = 0;
            SqlConnection sqlConn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["ncerp_devConnectionString"].ConnectionString));
            SqlCommand sqlComm = new SqlCommand();
            try
            {
                sqlComm.Connection = sqlConn;
                sqlComm.CommandText = SqlQuery;
                sqlConn.Open();
                i = sqlComm.ExecuteNonQuery();
            }
            catch (Exception x)
            {

                throw x;
            }
            finally
            {
                sqlConn.Close();
                sqlConn.Dispose();
            }
            return i;
        }
        //SAP flag update dev
        //public RfcConfigParameters SetRFCParameters()
        //{
        //    RfcConfigParameters rfc = new RfcConfigParameters();
        //    rfc.Add(RfcConfigParameters.Name, "ISU Dev");
        //    rfc.Add(RfcConfigParameters.AppServerHost, "172.17.200.13");
        //    rfc.Add(RfcConfigParameters.SystemID, "KUD");
        //    rfc.Add(RfcConfigParameters.Client, "200");
        //    rfc.Add(RfcConfigParameters.User, "MUHSHOAIB");
        //    rfc.Add(RfcConfigParameters.Password, "Syed#2012");
        //    rfc.Add(RfcConfigParameters.SystemNumber, "0");
        //    rfc.Add(RfcConfigParameters.Language, "en");
        //    rfc.Add(RfcConfigParameters.PoolSize, "5");
        //    rfc.Add(RfcConfigParameters.PeakConnectionsLimit, "10");
        //    rfc.Add(RfcConfigParameters.IdleTimeout, "100");

        //    return rfc;
        //}

        //SAP flag update prd
        //public RfcConfigParameters SetRFCParameters()
        //{
        //    RfcConfigParameters rfc = new RfcConfigParameters();
        //    rfc.Add(RfcConfigParameters.Name, "ISU PRD");
        //    rfc.Add(RfcConfigParameters.AppServerHost, "isuprd1.ke.com.pk");
        //    rfc.Add(RfcConfigParameters.SystemID, "KUP");
        //    rfc.Add(RfcConfigParameters.Client, "100");
        //    rfc.Add(RfcConfigParameters.User, "DOTNET");
        //    rfc.Add(RfcConfigParameters.Password, "Dotnet@12345");
        //    rfc.Add(RfcConfigParameters.SystemNumber, "0");
        //    rfc.Add(RfcConfigParameters.Language, "en");
        //    rfc.Add(RfcConfigParameters.PoolSize, "5");
        //    rfc.Add(RfcConfigParameters.PeakConnectionsLimit, "10");
        //    rfc.Add(RfcConfigParameters.IdleTimeout, "100");

        //    return rfc;
        //}
        public void errorlogs(string path, string errorDump)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ToString()))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("insertCrashLog", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@path", path));
                cmd.Parameters.Add(new SqlParameter("@error_trace", errorDump));
                cmd.CommandTimeout = 150000;

                // execute the command
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    conn.Close();
                }
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using EmailDupBill.Models;
using System.Collections;
using System.Reflection;

namespace ConsumerPortalNetService
{
    //public class DBContext
    //{
    //    Encrypt ex = new Encrypt();
    //    public DateTime GetLastReadValue()
    //    {

    //        DateTime LastReadValue = DateTime.Now;
    //        using (SqlConnection conn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ToString())))
    //        {
    //            conn.Open();
    //            SqlCommand command = new SqlCommand("SELECT Info_Key, Info_Value FROM Email_Config WHERE Info_Key = @info_key", conn);
    //            command.Parameters.Add(new SqlParameter("@info_key", "LST_READ_VAL"));


    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    string daa = reader["Info_Value"].ToString();
    //                    LastReadValue = DateTime.Parse(reader["Info_Value"].ToString());
    //                }
    //            }
    //            return LastReadValue;
    //        }
    //    }
    //    public Boolean EmailAlreadyExistsAgiainstBillingMonth(string email, string bill_month)
    //    {
    //        SqlConnection sqlConn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ConnectionString));
    //        SqlCommand sqlComm = new SqlCommand();
    //        SqlDataAdapter dA = new SqlDataAdapter();
    //        DataTable dTable = new DataTable();

    //        try
    //        {
    //            sqlComm.Connection = sqlConn;
    //            sqlComm.CommandText = "select email, bill_month from paperless_logging_table where Email = @email and bill_month = @billMonth";
    //            sqlComm.Parameters.AddWithValue("@email", email);
    //            sqlComm.Parameters.AddWithValue("@billMonth", bill_month);
    //            dA.SelectCommand = sqlComm;
    //            sqlConn.Open();
    //            dA.Fill(dTable);
    //        }
    //        catch (Exception x)
    //        {
    //            throw x;
    //        }

    //        if (dTable.Rows.Count > 0)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }

    //    }
    //    public ArrayList GetListofEmails(string bill_month)
    //    {

    //        ArrayList emaillist = new ArrayList();
    //        using (SqlConnection conn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ConnectionString)))
    //        {
    //            conn.Open();
    //            SqlCommand command = new SqlCommand("select AccountNumber, ConsumerNumber, ContractNumer, Email from RegistrationInfo where isActivated = 1 and isEntered = 1 and Email not in (select email from paperless_logging_table where bill_month = @billingMonth and isSent = 1)", conn);
    //            command.CommandTimeout = 600;
    //            command.Parameters.AddWithValue("@billingMonth", bill_month);
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Type type = typeof(RegistrationInfo);
    //                    RegistrationInfo regInf = new RegistrationInfo();
    //                    foreach (var col in type.GetProperties())
    //                    {
    //                        PropertyInfo info = regInf.GetType().GetProperty(col.Name);
    //                        if (reader[col.Name] == System.DBNull.Value)
    //                        {
    //                            info.SetValue(regInf, "");
    //                        }
    //                        else if (reader[col.Name].GetType() == typeof(DateTime))
    //                        {

    //                            info.SetValue(regInf, reader[col.Name]);

    //                        }
    //                        else {
    //                            info.SetValue(regInf, reader[col.Name].ToString());
    //                        }
    //                    }
    //                    emaillist.Add(regInf);
    //                }
    //            }
    //            return emaillist;
    //        }
    //    }

    //    public Email GetLatestRecordWithConsumerandAccountNumber(string Lookup, string consumerNumber)
    //    {
    //        Email emailinfo = new Email();
    //        try
    //        {
    //            var connstr = ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ToString());
    //            SqlConnection conn = null;
    //            SqlDataReader rdr = null;
    //            conn = new SqlConnection(connstr);
    //            conn.Open();
    //            SqlCommand command = null;

    //            command = new SqlCommand("getAccountNumberAndConsumerNumberLatestPayable", conn);
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Add(new SqlParameter("@Consumer_Number", consumerNumber));
    //            command.Parameters.Add(new SqlParameter("@Account_number", Lookup));


    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Type type = typeof(Email);
    //                    var nom = type.GetProperties();
    //                    foreach (var col in type.GetProperties())
    //                    {
    //                        System.Reflection.PropertyInfo info = emailinfo.GetType().GetProperty(col.Name);
    //                        if (reader[col.Name] == System.DBNull.Value)
    //                        {
    //                            info.SetValue(emailinfo, "", null);
    //                        }
    //                        else if (reader[col.Name].GetType() == typeof(DateTime))
    //                        {

    //                            info.SetValue(emailinfo, reader[col.Name], null);
    //                        }
    //                        else
    //                        {
    //                            info.SetValue(emailinfo, reader[col.Name], null);
    //                        }
    //                        //System.Diagnostics.Debug.WriteLine(pdfvm.GetType().GetProperty(col.Name).GetValue(pdfvm));
    //                    }
    //                }
    //            }
    //            return emailinfo;
    //        }
    //        catch (Exception)
    //        {

    //            return emailinfo;
    //        }

    //    }

    //    public Email GetLatestRecordWithAccountNumber(string Lookup)
    //    {
    //        Email emailinfo = new Email();
    //        try
    //        {
    //            var connstr = ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ToString());
    //            SqlConnection conn = null;
    //            SqlDataReader rdr = null;
    //            conn = new SqlConnection(connstr);
    //            conn.Open();
    //            SqlCommand command = null;

    //            command = new SqlCommand("getByAccountNumberLatestPayable", conn);
    //            command.CommandType = CommandType.StoredProcedure;
    //            //command.Parameters.Add(new SqlParameter("@Consumer_Number", consumerNumber));
    //            command.Parameters.Add(new SqlParameter("@Account_number", Lookup));


    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    Type type = typeof(Email);
    //                    var nom = type.GetProperties();
    //                    foreach (var col in type.GetProperties())
    //                    {
    //                        System.Reflection.PropertyInfo info = emailinfo.GetType().GetProperty(col.Name);
    //                        if (reader[col.Name] == System.DBNull.Value)
    //                        {
    //                            info.SetValue(emailinfo, "", null);
    //                        }
    //                        else if (reader[col.Name].GetType() == typeof(DateTime))
    //                        {

    //                            info.SetValue(emailinfo, reader[col.Name], null);
    //                        }
    //                        else
    //                        {
    //                            info.SetValue(emailinfo, reader[col.Name], null);
    //                        }
    //                        //System.Diagnostics.Debug.WriteLine(pdfvm.GetType().GetProperty(col.Name).GetValue(pdfvm));
    //                    }
    //                }
    //            }
    //            return emailinfo;
    //        }
    //        catch (Exception)
    //        {

    //            return emailinfo;
    //        }

    //    }

    //    public void UpdateIsSent(string email, string bill_month, string flag)
    //    {
    //        try
    //        {
    //            using (SqlConnection conn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ToString())))
    //            {
    //                conn.Open();
    //                SqlCommand command = new SqlCommand("update paperless_logging_table isSent = @flag where email = @email and bill_month = @bill_month", conn);
    //                command.Parameters.Add(new SqlParameter("@flag", flag));
    //                command.Parameters.Add(new SqlParameter("@email", email));
    //                command.Parameters.Add(new SqlParameter("@bill_month", bill_month));

    //                command.ExecuteNonQuery();
    //                conn.Close();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            FileLog.Log("Unable to update Flag on "+email+" with flag "+flag+" " +"Bill Month" + bill_month, e.Message.ToString() + "\n" + e.ToString(), "id");
    //        }
           
    //    }

    //    public void InsertIsSent(string accNumber, string consumer_number,string contract_number, string email, string bill_month, string flag)
    //    {
    //        try
    //        {
    //            using (SqlConnection conn = new SqlConnection(ex.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ToString())))
    //            {
    //                conn.Open();
    //                SqlCommand command = new SqlCommand("insert into paperless_logging_table(account_number, consumer_number, contract_number, bill_month, isSent, Email) values (@account_number,@consumer_number, @contract_number, @bill_month, @isSent, @Email)", conn);
    //                command.Parameters.Add(new SqlParameter("@account_number", accNumber));
    //                if (string.IsNullOrEmpty(consumer_number))
    //                {
    //                    command.Parameters.Add(new SqlParameter("@consumer_number", "None"));
    //                }
    //                else
    //                {
    //                    command.Parameters.Add(new SqlParameter("@consumer_number", consumer_number));
    //                }

    //                command.Parameters.Add(new SqlParameter("@contract_number", contract_number));
    //                command.Parameters.Add(new SqlParameter("@bill_month", bill_month));
    //                command.Parameters.Add(new SqlParameter("@isSent", flag));
    //                command.Parameters.Add(new SqlParameter("@Email", email));
    //                command.ExecuteNonQuery();
    //                conn.Close();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            FileLog.Log("Unable to update Flag on " + email + " with flag " + flag + " " + "Bill Month" + bill_month, e.Message.ToString() + "\n" + e.ToString(), "id");
    //        }

    //    }
    //}
}

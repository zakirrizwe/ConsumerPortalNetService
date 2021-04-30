using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace dupbill.Util
{
    public class PDFViewModel
    {
        public string Tenant_Name { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Address { get; set; }
        public string Forwarding_Address { get; set; }
        public string Stax_no { get; set; }
        public string ITax_No { get; set; }
        public string NIC_Number { get; set; }
        public string Customer_Cell_Nmbr { get; set; }
        public string Bill_Serial_No { get; set; }
        public string Class_id { get; set; }
        public string Tariff { get; set; }
        public string Bill_Type { get; set; }
        public string IBC_Name { get; set; }
        public string IBC_Address { get; set; }
        public string Account_Number { get; set; }
        public string Bill_Id { get; set; }
        public string Issue_Date { get; set; }
        public string Billing_Month { get; set; }
        public string Units_billed { get; set; }
        public string P_Units_Billed { get; set; }
        public string Avg_Temp_Mnth_Cur { get; set; }
        public string Bill_Charge_Mode { get; set; }
        public string Rnd_net_Amt { get; set; }
        public string Avg_Temp_Mnth_Prv { get; set; }
        public string Per_prv_Month { get; set; }
        public string Unit_Billed_1 { get; set; }
        public string Unit_Month_13 { get; set; }
        public string Avg_Temp_Year_Cur { get; set; }
        public string Per_Prv_Year { get; set; }
        public string LPS_Amt { get; set; }
        public string Due_Date { get; set; }
        public string Rnd_Gross_Amt { get; set; }
        public string Message_Board { get; set; }
        public string Billing_payment_table { get; set; }
        public string QR_Code { get; set; }

        public string QR_Code2 { get; set; }
        public string Contract_no { get; set; }
        public string Barcode { get; set; }
        public string Meter_Number { get; set; }
        public string Meter_Reading_Date { get; set; }
        public string Consumer_Number { get; set; }
        public string Security_Deposit { get; set; }
        public string Bnk_Gurr_Amt { get; set; }
        public string Sanc_Load_kw { get; set; }
        public string No_Of_Month { get; set; }
        public string Connected_Load_kw { get; set; }
        public string Cat_id { get; set; }
        public string Electricity_usage_table { get; set; }
        public string Electricity_charges_table { get; set; }
        public string Billing_statment_table { get; set; }
        public string ChartData_table { get; set; }
        public string Customer_Info_Table_A { get; set; }
        public string Customer_Info_Table_B { get; set; }

        public string Net_Amount { get; set; }

        public string Net_Amount_Prt { get; set; }
        public string schd_no { get; set; }

        public string Billed_upto { get; set; }
        public string Units_Adjusted { get; set; }

        public string V_RC_Flag { get; set; }
        public string V_RC_Table { get; set; }

        public string V_mc_Flag { get; set; }
        public string V_mc_Table1 { get; set; }
        public string V_mc_Table2 { get; set; }
        public string V_mc_Table3 { get; set; }

        public string V_irb_Flag { get; set; }
        public string V_irb_Table1 { get; set; }
        public string V_irb_Table2 { get; set; }

        public string V_irb_Table3 { get; set; }
        public string V_irb_load { get; set; }

        public string V_SD_Flag { get; set; }

        public string V_SD_Table { get; set; }
        public string Total_Units_Billed { get; set; }


        public static string[] col_names = {"Tenant_Name",
      "Customer_Name",
      "Customer_Address",
      "Forwarding_Address",
      "Stax_no",
      "ITax_No",
      "NIC_Number",
      "Customer_Cell_Nmbr",
      "Bill_Serial_No",
      "Class_id",
      "Tariff",
      "Bill_Type",
      "IBC_Name",
      "IBC_Address",
      "Account_Number",
      "Bill_Id",
      "Issue_Date",
      "Billing_Month",
      "Units_billed",
      "P_Units_Billed",
      "Avg_Temp_Mnth_Cur",
      "Bill_Charge_Mode",
      "Rnd_net_Amt",
      "Avg_Temp_Mnth_Prv",
      "Per_prv_Month",
      "Unit_Billed_1",
      "Unit_Month_13",
      "Avg_Temp_Year_Cur",
      "Per_Prv_Year",
      "LPS_Amt",
      "Due_Date",
      "Rnd_Gross_Amt",
      "Message_Board",
      "Billing_payment_table",
      "QR_Code2",
      "Contract_no",
      "Barcode",
      "Meter_Number",
      "Meter_Reading_Date",
      "Consumer_Number",
      "Security_Deposit",
      "Bnk_Gurr_Amt",
      "Sanc_Load_kw",
      "Connected_Load_kw",
      "Cat_id",
      "Electricity_user_table",
      "Electricity_charges_table",
      "Billing_statment_table",
      "ChartData_table"};




    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace dupbill.Util
{
    public class Parser
    {

        static char lineSeperater = '$';
        static char feildSeperater = '|';
        static string Data2Parses = "Energy Import - Off Peak$30000000$35000000$1$50000000$10|Net Energy Off Peak$$35000000$1$50000000$10|Reactive Energy Off Peak$1750$2000$$250$|Energy Import - Off Peak$30000000$35000000$1$50000000$10";



        public static ArrayList parseUseagedata(string Data2Parse)
        {
            ArrayList usagemodel = new ArrayList();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                UsageModel item = new UsageModel();

                string[] feilds = line.Split(feildSeperater);
                item.Formate = feilds[0];
                item.Label = feilds[1];
                item.PreviousReading = feilds[2];
                item.CurrentReading = feilds[3];
                item.MMF = feilds[4];
                item.Units = feilds[5];
                item.MDI = feilds[6];
                usagemodel.Add(item);
            }

            return usagemodel;

        }


        public static ArrayList parseChargesdata(string Data2Parse)
        {
            ArrayList chargesmodel = new ArrayList();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                ChargesModel item = new ChargesModel();

                string[] feilds = line.Split(feildSeperater);

                item.Formate = feilds[0];
                item.Label = feilds[1];
                item.Units = feilds[2];
                item.Rates = feilds[3];
                item.Amount = feilds[4];

                chargesmodel.Add(item);
            }

            return chargesmodel;

        }


        public static ArrayList BillingHistorysdata(string Data2Parse)
        {
            ArrayList bhmodel = new ArrayList();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                BillingHistoryModel item = new BillingHistoryModel();

                string[] feilds = line.Split(feildSeperater);

                item.MMYY = feilds[0];
                item.BilledAmount = feilds[1];
                item.PayDate = feilds[2];
                item.Payment = feilds[3];

                bhmodel.Add(item);
            }

            return bhmodel;

        }

        public static ArrayList parseBillingdata(string Data2Parse)
        {
            ArrayList billingmodel = new ArrayList();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                BillingModel item = new BillingModel();
                string[] feilds = line.Split(feildSeperater);
                item.Formate = feilds[0];
                item.Label = feilds[1];
                item.Amount = feilds[2];
                billingmodel.Add(item);
            }
            return billingmodel;

        }


        public static ArrayList parsechartdata(string Data2Parse)
        {
            ArrayList chartmodel = new ArrayList();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                if (!line.Equals(""))
                {
                    ChartItemModel item = new ChartItemModel();
                    string[] feilds = line.Split(feildSeperater);
                    item.Label = feilds[0];
                    item.Unit = feilds[1];
                    chartmodel.Add(item);
                }
            }
            return chartmodel;

        }

        public static ArrayList parseCustomerInfodata(string Data2Parse)
        {
            ArrayList customerinfomodel = new ArrayList();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                CustomerInfoModel item = new CustomerInfoModel();
                string[] feilds = line.Split(feildSeperater);
                item.label = feilds[0];
                item.info = feilds[1];
                customerinfomodel.Add(item);
            }
            return customerinfomodel;
        }


        public static IList<RCVoucherModel> parseRCVoucher(string Data2Parse)
        {
            IList<RCVoucherModel> rcmodel = new List<RCVoucherModel>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                RCVoucherModel item = new RCVoucherModel();
                string[] feilds = line.Split(feildSeperater);
                item.label = feilds[0];
                item.info = feilds[1];
                rcmodel.Add(item);
            }
            return rcmodel;
        }


        public static MCVoucherTable1Model parseMCVoucherTable1(string Data2Parse)
        {
            IList<MCVoucherTable1Model> mcmodel = new List<MCVoucherTable1Model>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                MCVoucherTable1Model item = new MCVoucherTable1Model();
                string[] feilds = line.Split(feildSeperater);
                item.InvoiceNo = feilds[0];
                item.IssueDate = feilds[1];
                mcmodel.Add(item);
            }
            return mcmodel[0];
        }


        public static MCVoucherTable2Model parseMCVoucherTable2(string Data2Parse)
        {
            IList<MCVoucherTable2Model> mcmodel = new List<MCVoucherTable2Model>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                MCVoucherTable2Model item = new MCVoucherTable2Model();
                string[] feilds = line.Split(feildSeperater);
                item.ReplacementMonth = feilds[0];
                item.MeterType = feilds[1];
                item.MeterCost = feilds[2];
                mcmodel.Add(item);
            }
            return mcmodel[0];
        }


        public static IList<MCVoucherTable3Model> parseMCVoucherTable3(string Data2Parse)
        {
            IList<MCVoucherTable3Model> mcmodel = new List<MCVoucherTable3Model>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                MCVoucherTable3Model item = new MCVoucherTable3Model();
                string[] feilds = line.Split(feildSeperater);
                item.Formate = feilds[0];
                item.Label = feilds[1];
                item.Amount = feilds[2];
                mcmodel.Add(item);
            }
            return mcmodel;

        }

        public static IRBVoucherTable1Model parseIRBVoucherTable1(string Data2Parse)
        {
            IList<IRBVoucherTable1Model> irbmodel = new List<IRBVoucherTable1Model>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                IRBVoucherTable1Model item = new IRBVoucherTable1Model();
                string[] feilds = line.Split(feildSeperater);
                item.InvoiceNo = feilds[0];
                item.IssueDate = feilds[1];
                irbmodel.Add(item);
            }
            return irbmodel[0];
        }


        public static IRBVoucherTable2Model parseIRBVoucherTable2(string Data2Parse)
        {
            IList<IRBVoucherTable2Model> irbmodel = new List<IRBVoucherTable2Model>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                IRBVoucherTable2Model item = new IRBVoucherTable2Model();
                string[] feilds = line.Split(feildSeperater);
                item.BillPeriod = feilds[0];
                item.TotalUnit = feilds[1];
                item.UnitPreviouslyCharged = feilds[2];
                item.UnitCharged = feilds[3];
                irbmodel.Add(item);
            }

            return irbmodel[0];
        }

        public static IList<IRBVoucherTable3Model> parseIRBVoucherTable3(string Data2Parse)
        {
            IList<IRBVoucherTable3Model> mcmodel = new List<IRBVoucherTable3Model>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                IRBVoucherTable3Model item = new IRBVoucherTable3Model();
                string[] feilds = line.Split(feildSeperater);
                item.Formate = feilds[0];
                item.Label = feilds[1];
                item.Amount = feilds[2];
                mcmodel.Add(item);
            }
            return mcmodel;

        }

        public static IList<SDVoucherModel> parseSDVoucher(string Data2Parse)
        {
            IList<SDVoucherModel> sdmodel = new List<SDVoucherModel>();
            string[] lines = Data2Parse.Split(lineSeperater);

            foreach (var line in lines)
            {
                SDVoucherModel item = new SDVoucherModel();
                string[] feilds = line.Split(feildSeperater);
                item.label = feilds[0];
                item.info = feilds[1];
                sdmodel.Add(item);
            }
            return sdmodel;
        }

    }
}
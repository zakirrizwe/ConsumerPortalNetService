using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dupbill.Util
{
    public class UsageModel
    {
        public string Formate { get; set; }
        public string Label { get; set; }
        public string PreviousReading { get; set; }
        public string CurrentReading { get; set; }
        public string MMF { get; set; }
        public string Units { get; set; }
        public string MDI { get; set; }
    }

    public class ChargesModel
    {
        public string Formate { get; set; }
        public string Label { get; set; }
        public string Units { get; set; }
        public string Rates { get; set; }
        public string Amount { get; set; }
    }

    public class BillingModel
    {
        public string Formate { get; set; }
        public string Label { get; set; }
        public string Amount { get; set; }
    }
    public class ChartItemModel
    {
        public string Label { get; set; }
        public string Unit { get; set; }
    }

    public class BillingHistoryModel
    {
        public string MMYY { get; set; }
        public string BilledAmount { get; set; }
        public string PayDate { get; set; }
        public string Payment { get; set; }
    }

    public class CustomerInfoModel
    {

        public string label { get; set; }
        public string info { get; set; }
    }

    public class RCVoucherModel
    {

        public string label { get; set; }
        public string info { get; set; }
    }

    public class MCVoucherTable1Model
    {

        public string InvoiceNo { get; set; }
        public string IssueDate { get; set; }
    }

    public class MCVoucherTable2Model
    {

        public string ReplacementMonth { get; set; }
        public string MeterType { get; set; }
        public string MeterCost { get; set; }

    }

    public class MCVoucherTable3Model
    {
        public string Formate { get; set; }
        public string Label { get; set; }
        public string Amount { get; set; }
    }

    public class IRBVoucherTable1Model
    {
        public string InvoiceNo { get; set; }
        public string IssueDate { get; set; }
    }

    public class IRBVoucherTable2Model
    {
        public string BillPeriod { get; set; }
        public string TotalUnit { get; set; }
        public string UnitPreviouslyCharged { get; set; }
        public string UnitCharged { get; set; }
    }

    public class IRBVoucherTable3Model
    {
        public string Formate { get; set; }
        public string Label { get; set; }
        public string Amount { get; set; }
    }

    public class SDVoucherModel
    {

        public string label { get; set; }
        public string info { get; set; }
    }
}

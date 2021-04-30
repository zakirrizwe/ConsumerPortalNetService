using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using System.Web.Helpers;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;
using System.Web.Configuration;
using System.Configuration;
using dupbill.Util;
using System.Web.Hosting;

namespace ConsumerPortalNetService
{

    public class GeneratePDF
    {
        public PDFViewModel pdfvmm;
        public TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        Encrypt en = new Encrypt();

        //get from viewmodel later
        public double[] yVal;
        public string[] xName;
        public string mode;

        public const string EMAIL_MODE = "EMAIL";
        public const string WEB_MODE = "WEB";
        public const string QMETRIC_MODE = "QMATIC";
        public const string QMETRIC_WITH_BACKGROUND_MODE = "WEB";

        public string fontpath1 = "";
        public string imagepath1 = "";

        //static void main(string[] args)
        //{
        //    GeneratePDF gpdf = new GeneratePDF();
        //    gpdf.makePDF("0400004425314", "May-19","EMAIL", null, "580007143878");
        //}
        public MemoryStream makePDF(string act_number, string BillingMonth, string mode_type, string ParentPath = null, string BillId = "")
        {
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=test.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //(595 x 842 user units)

            // float rwidth = 8.3f * 0.99638554216f;
            // float rHeigth = 11.7f * 0.99914529914f;

            // var p1 = iTextSharp.text.Utilities.InchesToPoints(rwidth);//iTextSharp.text.Utilities.MillimetersToPoints(210);


            //var p2 = iTextSharp.text.Utilities.InchesToPoints(rHeigth);// iTextSharp.text.Utilities.MillimetersToPoints(297);

            // var doc1 = new Document(new iTextSharp.text.Rectangle(p1,p2), 0f,0f,0f,0f);

            var doc1 = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            MemoryStream oStream = new MemoryStream();
            MemoryStream wStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc1, wStream);
            try
            {

                //try
                //{

                // string path = HttpContext.Current.Server.MapPath("PDFs");
                //string filename = "/Doc1" + DateTime.UtcNow.Millisecond.ToString() + ".pdf";
                //PdfWriter writer = PdfWriter.GetInstance(doc1, new FileStream(path + filename.ToString(), FileMode.Create));


                // PdfWriter.GetInstance(doc1, wStream);

                writer.CloseStream = false;

                //System.Diagnostics.Debug.WriteLine(pdfvm.Reason_For_Average);
                //pdfvm.GetType().GetProperty("Reason_for_Change").SetValue(pdfvm, "My debug string here");
                //System.Diagnostics.Debug.WriteLine(pdfvm.Reason_For_Average);


                //datetime
                if (BillingMonth != "X")
                {
                    DateTime myDate = DateTime.ParseExact(BillingMonth, "MMM-yyyy",

                    System.Globalization.CultureInfo.InvariantCulture);


                    System.Diagnostics.Debug.WriteLine(myDate.ToString("yyyy"));
                }

                // getFilledObject("04000009912");

                //var sss = "343";
                //ArrayList sl = Parser.parseUseagedata(sss);
                //var row = sl.Count;

                this.mode = mode_type; // "WEB","EMAIL","QMETRIC"
                if (BillId.Equals(null) || BillId.Equals(""))
                {
                    this.pdfvmm = getFilledObject(act_number, BillingMonth);
                }
                else
                {
                    this.pdfvmm = getFilledObject(act_number, BillingMonth, BillId);
                }




                ///////////////////
                //background image
                ////////////////////
                var lm = 0; //left marging
                var rm = 0; //right marging
                var bm = 0; //bottom marging
                var tm = 0; //top marging
                var pageWidth = doc1.PageSize.Width - (lm + rm);
                var pageHeight = doc1.PageSize.Height - (bm + tm);

                //starting writing to file
                doc1.Open();
                doc1.NewPage();

                string Page1BGimageFilePath;
                string Page2BGimageFilePath;
                iTextSharp.text.Image jpg = null;
                iTextSharp.text.Image jpg2 = null;

                if (this.mode.Equals(WEB_MODE))
                {
                    Page1BGimageFilePath = HostingEnvironment.MapPath("~") + "images/Blank-front.jpg";
                    Page2BGimageFilePath = HostingEnvironment.MapPath("~") + "images/Blank-back.jpg";

                    jpg = iTextSharp.text.Image.GetInstance(Page1BGimageFilePath);
                    jpg.ScaleToFit(pageWidth, pageHeight);
                    jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg.SetAbsolutePosition(0, 0);

                    jpg2 = iTextSharp.text.Image.GetInstance(Page2BGimageFilePath);
                    jpg2.ScaleToFit(pageWidth, pageHeight);
                    jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg2.SetAbsolutePosition(0, 0);
                    doc1.Add(jpg); // adding background for the fist page


                    this.fontpath1 = HostingEnvironment.MapPath("~");
                    this.imagepath1 = HostingEnvironment.MapPath("~");


                }
                else if (this.mode.Equals(EMAIL_MODE))
                {
                    //    Page1BGimageFilePath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "/images/Blank-front.jpg";
                    //  Page2BGimageFilePath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "/images/Blank-back.jpg";
                    Page1BGimageFilePath = ParentPath + "/images/Blank-front.jpg";
                    Page2BGimageFilePath = ParentPath + "/images/Blank-back.jpg";

                    jpg = iTextSharp.text.Image.GetInstance(Page1BGimageFilePath);
                    jpg.ScaleToFit(pageWidth, pageHeight);
                    jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg.SetAbsolutePosition(0, 0);

                    jpg2 = iTextSharp.text.Image.GetInstance(Page2BGimageFilePath);
                    jpg2.ScaleToFit(pageWidth, pageHeight);
                    jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg2.SetAbsolutePosition(0, 0);
                    doc1.Add(jpg); // adding background for the fist page
                    this.fontpath1 = ParentPath;
                    this.imagepath1 = ParentPath;

                }
                else if (this.mode.Equals(QMETRIC_MODE))
                {
                    // nothing background picture

                    this.fontpath1 = HttpContext.Current.Server.MapPath(".");
                    this.imagepath1 = HttpContext.Current.Server.MapPath(".");
                }


                //prining text
                setFirstPageInfo(doc1, writer, pdfvmm);

                //adding chartimage
                getBarChartImage(doc1, writer, pdfvmm);

                //adding bottom barcode
                drawBarCode(doc1, writer, pdfvmm);


                //Make tables
                if (!(pdfvmm.Billing_payment_table.Equals(null) || pdfvmm.Billing_payment_table.Equals("")))
                {
                    drawBillingAndHistoryTable(doc1, writer, this.pdfvmm);
                }

                // adding text
                //doc1.Add(paragraph);



                doc1.NewPage();



                if (this.mode.Equals(WEB_MODE))
                {
                    doc1.Add(jpg2); // adding background for the second page
                }
                else if (this.mode.Equals(EMAIL_MODE))
                {
                    doc1.Add(jpg2); // adding background for the second page
                }


                //Set text etc for the second page
                setSecondPageInfo(doc1, writer, this.pdfvmm);

                //Developer: ISK
                //Development: ISK_D1

                //ISK_D1 comment below 2 lines and add new condition on line 233
                ////Voucher 1 RC
                //if (pdfvmm.V_RC_Flag.Equals("X")) 
                if (pdfvmm.V_RC_Table.Length > 2)
                {
                    doc1.NewPage();

                    if (this.mode.Equals(WEB_MODE))
                    {
                        string PageVBGimageFilePath = HostingEnvironment.MapPath("~") + "/images/RC-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }
                    else if (this.mode.Equals(EMAIL_MODE))
                    {
                        string PageVBGimageFilePath = ParentPath + "/images/RC-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }

                    //ISK_D1 comment below 6 lines
                    ////static text
                    //string PageStaticimageFilePath = HostingEnvironment.MapPath("~") + "/images/RC-Voucher-Static-Text.jpg";
                    //jpg2 = iTextSharp.text.Image.GetInstance(PageStaticimageFilePath);
                    //jpg2.ScaleToFit(pageWidth, 410);
                    //jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                    //jpg2.SetAbsolutePosition(0, 365);

                    //doc1.Add(jpg2);
                    addVoucherHeader(doc1, writer, this.pdfvmm);
                    addReconectionVoucherinfo(doc1, writer, this.pdfvmm);
                }

                //ISK_D1 comment below lines and add new condition on line 274
                //MC-Voucher
                //if (pdfvmm.V_mc_Flag.Equals("X"))
                if (pdfvmm.V_mc_Table1.Length > 2)
                    {
                    doc1.NewPage();

                    if (this.mode.Equals(WEB_MODE))
                    {
                        string PageVBGimageFilePath = HostingEnvironment.MapPath("~") + "/images/MC-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }
                    else if (this.mode.Equals(EMAIL_MODE))
                    {
                        string PageVBGimageFilePath = ParentPath + "/images/MC-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }

                    //ISK_D1 comment below 6 lines
                    ////static text
                    //string PageStaticimageFilePath = HostingEnvironment.MapPath("~") + "/images/MC-Voucher-Static-Text.jpg";
                    //jpg2 = iTextSharp.text.Image.GetInstance(PageStaticimageFilePath);
                    //jpg2.ScaleToFit(pageWidth, 410);
                    //jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                    //jpg2.SetAbsolutePosition(0, 520);


                    //doc1.Add(jpg2);
                    addVoucherHeader(doc1, writer, this.pdfvmm);
                    addMeterChargesVoucherinfo(doc1, writer, this.pdfvmm);
                }

                //IRB-Voucher
                //ISK_D1 change below condition
                //if (pdfvmm.V_irb_Flag.Equals("X") && pdfvmm.V_irb_Table1.Length > 0 && !pdfvmm.V_irb_Table1.Equals(" "))
                if (pdfvmm.V_irb_Table1.Length > 0 && !pdfvmm.V_irb_Table1.Equals(" "))
                {
                    doc1.NewPage();

                    if (this.mode.Equals(WEB_MODE))
                    {
                        string PageVBGimageFilePath = HostingEnvironment.MapPath("~") + "/images/Suplimentry-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }
                    else if (this.mode.Equals(EMAIL_MODE))
                    {
                        string PageVBGimageFilePath = ParentPath + "/images/Suplimentry-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }

                    //ISK_D1 comment below 6 lines
                    //static text
                    string PageStaticimageFilePath = HostingEnvironment.MapPath("~") + "/images/IRB-Voucher-Static-Text.jpg";
                    jpg2 = iTextSharp.text.Image.GetInstance(PageStaticimageFilePath);
                    jpg2.ScaleToFit(pageWidth, 410);
                    jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                    jpg2.SetAbsolutePosition(0, 490);


                    doc1.Add(jpg2);
                    addVoucherHeader(doc1, writer, this.pdfvmm);
                    addIRBVoucherinfo(doc1, writer, this.pdfvmm);
                }

                //ISK_D1 change condition below
                //SD-Voucher
                //if (pdfvmm.V_SD_Flag.Equals("X"))
                if (pdfvmm.V_SD_Table.Length>2)
                {
                    doc1.NewPage();

                    if (this.mode.Equals(WEB_MODE))
                    {
                        string PageVBGimageFilePath = HostingEnvironment.MapPath("~") + "/images/SD-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }
                    else if (this.mode.Equals(EMAIL_MODE))
                    {
                        string PageVBGimageFilePath = ParentPath + "/images/SD-voucher.jpg";
                        jpg2 = iTextSharp.text.Image.GetInstance(PageVBGimageFilePath);
                        jpg2.ScaleToFit(pageWidth, pageHeight);
                        jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                        jpg2.SetAbsolutePosition(0, 0);
                        doc1.Add(jpg2);

                    }

                    //ISK_D1 comment below 6 lines
                    ////static text
                    //string PageStaticimageFilePath = HostingEnvironment.MapPath("~") + "/images/SD-Voucher-Static-Text.jpg";
                    //jpg2 = iTextSharp.text.Image.GetInstance(PageStaticimageFilePath);
                    //jpg2.ScaleToFit(pageWidth, 410);
                    //jpg2.Alignment = iTextSharp.text.Image.UNDERLYING;
                    //jpg2.SetAbsolutePosition(0, 520);


                    //doc1.Add(jpg2);
                    addVoucherHeader(doc1, writer, this.pdfvmm);
                    addSDVoucherinfo(doc1, writer, this.pdfvmm);
                }



                var islock = new Object();
                lock (islock)
                {
                    doc1.Close();
                }


                //}
                //catch(Exception e) {

                //}

                byte[] byteInfo = wStream.ToArray();
                oStream.Write(byteInfo, 0, byteInfo.Length);
                oStream.Position = 0;


            }
            catch (Exception e)
            {
                CommonFunction cf = new CommonFunction();
                cf.errorlogs("Paperless attachment email", e.StackTrace + "\t" + e.Message.ToString() + "\t" + e.Source);
                throw e;
            }


            return oStream;
        }

        public void getBarChartImage(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {

            //getting font
            string fontpath = fontpath1 + "/fonts/Helvetica-Condensed.otf";
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(fontpath);



            fontpath = fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            //SET UP THE DATA TO PLOT  
            //double[] yVal = { 34530, 2342, 3453, 0, 23234, 43345, 3443, 5344, 786, 2332, 54556, 7366, 43224 };
            //string[] xName = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan" };

            ArrayList chartdata = Parser.parsechartdata(pdfvmm.ChartData_table);
            chartdata.Reverse();
            ArrayList val = new ArrayList();
            ArrayList names = new ArrayList();


            foreach (ChartItemModel item in chartdata)
            {

                val.Add(int.Parse(item.Unit));
                DateTime trimDate = DateTime.ParseExact(item.Label, "MMM-yy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                names.Add(trimDate.ToString("MMM"));

            }



            //double[] yVal = val.ToArray(); ;
            //string[] xName;

            //CREATE THE CHART
            System.Web.UI.DataVisualization.Charting.Chart Chart1 = new System.Web.UI.DataVisualization.Charting.Chart();

            Chart1.AntiAliasing = AntiAliasingStyles.All;
            Chart1.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            Chart1.Width = 1000;
            Chart1.Height = 375;



            //BIND THE DATA TO THE CHART
            Series series1 = new Series("spline");

            series1.ChartType = SeriesChartType.Column;
            series1.SmartLabelStyle.Enabled = false;
            //series1.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No;
            series1.LabelAngle = -90;
            series1.LabelForeColor = Color.FromArgb(51, 51, 51);

            // series1["LabelStyle"] = "Bottom";

            series1.Font = new System.Drawing.Font(pfc.Families[0], 12);


            //setting datapoints
            for (var i = 0; i <= val.Count - 1; i++)
            {
                series1.Points.AddXY(names[i], val[i]);
                //series1.IsValueShownAsLabel = true;


                //color for last column
                if (i == val.Count - 1)
                {
                    series1.Points[i].Color = System.Drawing.Color.Black;
                }
                else {
                    series1.Points[i].Color = System.Drawing.Color.FromArgb(121, 121, 121);
                }
            }


            Chart1.Series.Add(series1);

            //Chart1.Series.Add(new Series());
            //Chart1.Series[0].Points.DataBindXY(xName, yVal);


            //SET THE CHART TYPE TO BE column
            Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            //Chart1.Series[0]["PieLabelStyle"] = "Outside";
            //Chart1.Series[0]["PieStartAngle"] = "-90";



            //SET THE COLOR PALETTE FOR THE CHART TO BE A PRESET OF NONE 
            //DEFINE OUR OWN COLOR PALETTE FOR THE CHART 
            Chart1.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.None;
            //Chart1.PaletteCustomColors = new Color[] { Color.Blue, Color.Red };

            //SET THE IMAGE OUTPUT TYPE TO BE JPEG
            Chart1.ImageType = System.Web.UI.DataVisualization.Charting.ChartImageType.Png;

            //ADD A PLACE HOLDER CHART AREA TO THE CHART
            //SET THE CHART AREA TO BE 3D
            ChartArea ca = new ChartArea();
            //ca.BackColor = Color.Transparent;
            ca.AxisX.LineColor = Color.Transparent;
            ca.AxisY.LineColor = Color.Transparent;
            ca.BorderColor = Color.Transparent;
            ca.AxisX.LineWidth = 0;
            ca.AxisY.LineWidth = 0;
            ca.AxisX.LabelStyle.Enabled = true;
            ca.AxisX.IsLabelAutoFit = true;
            ca.AxisX.Interval = 1;
            ca.AxisY.LabelStyle.Enabled = false;
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            ca.AxisX.MinorGrid.Enabled = false;
            ca.AxisY.MinorGrid.Enabled = false;
            ca.AxisX.MajorTickMark.Enabled = false;
            ca.AxisY.MajorTickMark.Enabled = false;
            ca.AxisX.MinorTickMark.Enabled = false;
            ca.AxisY.MinorTickMark.Enabled = false;
            ca.AxisY.IsStartedFromZero = true;
            // ca.AxisY.IsReversed = true;
            ca.AxisY.MaximumAutoSize = 70;
            ca.AxisX.MaximumAutoSize = 100;
            ca.AxisX.IsLabelAutoFit = true;
            ca.AxisX.LabelAutoFitMinFontSize = 16;
            ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(121, 121, 121);
            ca.AxisY.LabelAutoFitMinFontSize = 15;
            ca.AxisX.LabelStyle.ForeColor = Color.FromArgb(121, 121, 121);
            //            Chart1.BackColor = Color.Transparent;




            ca.AxisX.LabelStyle.Font = new System.Drawing.Font(pfc.Families[0], 18, FontStyle.Regular);


            //adding chart area
            Chart1.ChartAreas.Add(ca);




            //ADD A PLACE HOLDER LEGEND TO THE CHART
            //DISABLE THE LEGEND
            Chart1.Legends.Add(new Legend());
            Chart1.Legends[0].Enabled = false;

            iTextSharp.text.Image chartImage;

            // Chart1.SaveImage(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + "/images/chart.jpeg", ChartImageFormat.Png);
            using (var chartimage = new MemoryStream())
            {
                Chart1.SaveImage(chartimage, ChartImageFormat.Png);
                // return chartimage.GetBuffer();
                chartImage = iTextSharp.text.Image.GetInstance(chartimage.GetBuffer());
            }

            chartImage.SetAbsolutePosition(2f, 340f);
            chartImage.ScaleAbsolute(295, 110);
            doc1.Add(chartImage);


            #region charlabels/lines
            PdfContentByte cb = writer.DirectContent;




            //find the second jan
            int monthcount = 0;
            //  int jancount = 0;
            for (int i = 0; i < names.Count; i++)
            {
                ;

                if (names[i].Equals("Jan"))
                {
                    monthcount = i;
                }
            }

            int count = 1;
            //257 :19.5
            //var startx = 237.5f;
            var startx = 23 + (monthcount * 19.5);
            var starty = 460f;
            do
            {

                cb.SetColorStroke(BaseColor.DARK_GRAY);
                cb.SetLineDash(2);
                cb.MoveTo(startx, starty - 3);
                cb.LineTo(startx, starty - 5);
                starty -= 5;
                count += 1;
                cb.ClosePathStroke();

            } while (count <= 22);



            //setting year lables on chart lines


            ChartItemModel Curr_year = (ChartItemModel)chartdata[chartdata.Count - 1];
            ChartItemModel Last_year = (ChartItemModel)chartdata[0];


            DateTime dd = DateTime.ParseExact(Last_year.Label.ToString(), "MMM-yy",
                                     System.Globalization.CultureInfo.InvariantCulture);
            cb.BeginText();
            cb.SetTextMatrix(20f, 452f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 10);
            cb.ShowText(dd.ToString("yyyy"));
            cb.EndText();


            dd = DateTime.ParseExact(Curr_year.Label.ToString(), "MMM-yy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            //Dynamic year movement
            // float startx_year = (float) startx + 2f;
            float startx_year = 255;
            cb.BeginText();
            cb.SetTextMatrix(startx_year, 452);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 10);
            cb.ShowText(dd.ToString("yyyy"));
            cb.EndText();




            #endregion


            #region Verticle_lables

            PdfPTable table = new PdfPTable(13);
            table.TotalWidth = 252f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 1;
            PdfPCell cell = new PdfPCell(new Phrase(""));

            cell.Border = 0;

            //cell.PaddingLeft = 3;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.PaddingRight = 2;
            cell.Padding = 0;

            foreach (ChartItemModel item in chartdata)
            {
                if (chartdata.IndexOf(item) == 12)
                {
                    cell.Rotation = 90;
                    cell.Phrase = new Phrase(item.Unit, new iTextSharp.text.Font(customfontNormal, 6f, 1, new BaseColor(158, 158, 158)));
                    table.AddCell(cell);
                }
                else
                {
                    cell.Rotation = 90;
                    cell.Phrase = new Phrase(item.Unit, new iTextSharp.text.Font(customfontNormal, 6f, 1, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                }

            }


            cb = writer.DirectContent;
            table.WriteSelectedRows(0, -1, 22f, 372, cb);


            #endregion
        }

        public void drawTextPage1(PdfWriter writer)
        {

            string fontpath = fontpath1;
            BaseFont customfont = BaseFont.CreateFont(fontpath + "/fonts/RobotoMono-Regular.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(customfont, 12);

        }

        public void drawBillingAndHistoryTable(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {

            string fontpath = fontpath1;
            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfPTable table = new PdfPTable(4);
            // actual width of table in point
            table.TotalWidth = 290f;
            //fix the absolute width of the table
            table.LockedWidth = true;
            //relative col widths in proportions -here equal
            float[] widths = new float[] { 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 1;
            PdfPCell cell = new PdfPCell(new Phrase(""));

            ArrayList bhdata = Parser.BillingHistorysdata(pdfvmm.Billing_payment_table);
            foreach (BillingHistoryModel item in bhdata)
            {
                cell.Border = 0;
                cell.HorizontalAlignment = 1;
                cell.PaddingRight = 0;


                cell.HorizontalAlignment = 1;
                cell.PaddingRight = 0;
                cell.Phrase = new Phrase(item.MMYY, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(cell);
                if (!item.BilledAmount.Equals(""))
                {
                    cell.PaddingRight = 18;
                    cell.HorizontalAlignment = 2;
                    cell.Phrase = new Phrase(item.BilledAmount, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                }
                else
                {

                    cell.PaddingRight = 18;
                    cell.HorizontalAlignment = 2;
                    cell.Phrase = new Phrase(" ", new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                }

                cell.HorizontalAlignment = 1;
                cell.PaddingRight = 0;
                cell.Phrase = new Phrase(item.PayDate, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(cell);

                if (!item.Payment.Equals(""))
                {
                    cell.HorizontalAlignment = 2;
                    cell.PaddingRight = 23;
                    cell.Phrase = new Phrase(item.Payment, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                }
                else
                {

                    cell.HorizontalAlignment = 2;
                    cell.PaddingRight = 23;
                    cell.Phrase = new Phrase(" ", new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                }
            }

            PdfContentByte cb = writer.DirectContent;

            table.WriteSelectedRows(0, -1, 5f, 295f, cb);
            //doc1.Add(table);


        }

        public void drawBarCode(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            string fontpath = fontpath1;
            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;

            Barcode128 barcode128 = new Barcode128();
            barcode128.Code = pdfvmm.Barcode;
            barcode128.CodeType = Barcode.CODE128;
            barcode128.AltText = "";
            //barcode128.BarHeight = 20f;
            //  barcode128.X = 200f
            iTextSharp.text.Image code128Image = barcode128.CreateImageWithBarcode(cb, BaseColor.DARK_GRAY, null);
            code128Image.SetAbsolutePosition(17f, 9f);
            code128Image.ScaleAbsolute(273f, 38f);
            doc1.Add(code128Image);

            //Barcode39 barcode128 = new Barcode39();
            //barcode128.Code = pdfvmm.Barcode;
            //barcode128.AltText = "";
            ////barcode128.BarHeight = 20f;
            ////  barcode128.X = 200f
            //iTextSharp.text.Image code128Image = barcode128.CreateImageWithBarcode(cb, BaseColor.DARK_GRAY, null);
            //code128Image.SetAbsolutePosition(17f, 9f);
            //code128Image.ScaleAbsolute(273f, 38f);
            //doc1.Add(code128Image);




            // QrCode on the bottom
            //BarcodeQRCode qrcode2 = new BarcodeQRCode(pdfvmm.QR_Code2, 90, 90, null);

            //iTextSharp.text.Image qrcodeImage = qrcode2.GetImage();
            //qrcodeImage.BackgroundColor = null;
            //qrcodeImage.SetAbsolutePosition(247f, 93f);
            //qrcodeImage.ScaleAbsolute(50f, 50f);
            //doc1.Add(qrcodeImage);


            Barcode128 barcode39 = new Barcode128();
            barcode39.Code = pdfvmm.Account_Number;
            barcode39.CodeType = Barcode.CODE128;
            barcode39.AltText = "";

            iTextSharp.text.Image code39Image = barcode39.CreateImageWithBarcode(cb, BaseColor.BLACK, null);
            code39Image.SetAbsolutePosition(432f, 646f);
            code39Image.ScaleAbsolute(90f, 40f);
            doc1.Add(code39Image);


            BarcodeQRCode qrcode1 = new BarcodeQRCode(pdfvmm.QR_Code, 90, 90, null);

            iTextSharp.text.Image qrcodeImage2 = qrcode1.GetImage();
            qrcodeImage2.BackgroundColor = null;
            qrcodeImage2.SetAbsolutePosition(445f, 765f);
            qrcodeImage2.ScaleAbsolute(35f, 35f);
            doc1.Add(qrcodeImage2);

            cb.BeginText();
            cb.SetTextMatrix(452f, 797f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 6f);
            cb.ShowText("Scan Me");
            cb.EndText();


        }

        //this function sets text/graphics based on the viewmodel data
        public void setFirstPageInfo(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            //setting 
            string fontpath = fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);

            //setting labels

            #region customerinfo

            //var PersonName = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.Tenant_Name, 50).ToLower());
            //var OwnerName = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.Customer_Name, 50).ToLower());
            //var Address = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.Customer_Address, 80).ToLower());
            //var ForwardingAddress = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.Forwarding_Address, 80).ToLower());
            //var PhoneNumber = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.Customer_Cell_Nmbr, 50).ToLower());


            var PersonName = pdfvmm.Tenant_Name;
            var OwnerName = pdfvmm.Customer_Name;
            var Address = pdfvmm.Customer_Address;
            var ForwardingAddress = pdfvmm.Forwarding_Address;
            var GST = pdfvmm.Stax_no;
            var PhoneNumber = pdfvmm.Customer_Cell_Nmbr;
            var CNIC = pdfvmm.NIC_Number;
            var iTax_No = pdfvmm.ITax_No;
            var Bill_Serial_No = pdfvmm.Bill_Serial_No;
            var ScheduleNo = pdfvmm.schd_no;
            var ConsumerNoc = pdfvmm.Consumer_Number;
            var AddressInfo = Address
                            + "\n" + ForwardingAddress
                            + "\nGST No.:" + GST
                            + "\nCNIC No.:" + CNIC
                            + "\nYour Registered Mobile Number: " + PhoneNumber
                            + "\nNTN No.: " + PhoneNumber
                            + "\nDispatch ID: " + Bill_Serial_No;
            var conNo = pdfvmm.Contract_no;

            PdfPTable maininfotable = new PdfPTable(1);
            maininfotable.TotalWidth = 265f;
            maininfotable.LockedWidth = true;
            var widthss = new float[] { 1f };
            maininfotable.SetWidths(widthss);
            maininfotable.HorizontalAlignment = 1;

            PdfPCell namecell = new PdfPCell(new Phrase(""));
            namecell.Border = 0;
            namecell.HorizontalAlignment = 0;
            namecell.PaddingTop = 1;
            namecell.PaddingBottom = 3f;


            PdfPCell labelcell = new PdfPCell(new Phrase(""));
            labelcell.Border = 0;
            labelcell.HorizontalAlignment = 0;
            labelcell.PaddingBottom = 0;


            PdfPCell infocell = new PdfPCell(new Phrase(""));
            infocell.Border = 0;
            infocell.HorizontalAlignment = 0;
            infocell.Padding = 0;
            infocell.PaddingLeft = 2;
            infocell.PaddingBottom = 2f;


            if (!(PersonName.Equals("") || PersonName.Equals(null)))
            {
                labelcell.Phrase = new Phrase("Tenant", new iTextSharp.text.Font(customfontNormal, 7f, 1, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(labelcell);

                namecell.Phrase = new Phrase(PersonName, new iTextSharp.text.Font(customfontBold, 15, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(namecell);

                labelcell.PaddingTop = 1;
                labelcell.Phrase = new Phrase("Owner", new iTextSharp.text.Font(customfontNormal, 7f, 1, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(labelcell);

            }


            if (!(OwnerName.Equals("") || OwnerName.Equals(null)))
            {
                namecell.Phrase = new Phrase(OwnerName, new iTextSharp.text.Font(customfontBold, 15, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(namecell);
            }

            if (!(Address.Equals("") || Address.Equals(null)))
            {

                infocell.Phrase = new Phrase(Address, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }
            if (!(ForwardingAddress.Equals("") || ForwardingAddress.Equals(null) || ForwardingAddress.Equals(" ")))
            {

                infocell.Phrase = new Phrase(ForwardingAddress, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }

            if (!(GST.Equals("") || GST.Equals(null) || GST.Equals(" ")))
            {

                infocell.Phrase = new Phrase("GST No. " + GST, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }
            if (!(CNIC.Equals("") || CNIC.Equals(null) || CNIC.Equals(" ")))
            {

                infocell.Phrase = new Phrase("CNIC No. " + CNIC, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }


            if (!(PhoneNumber.Equals("") || PhoneNumber.Equals(null) || PhoneNumber.Equals(" ")))
            {

                infocell.Phrase = new Phrase("Your Registered Mobile Number: " + PhoneNumber, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }

            if (!(iTax_No.Equals("") || iTax_No.Equals(null) || iTax_No.Equals(" ")))
            {

                infocell.Phrase = new Phrase("NTN No.: " + iTax_No, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }


            if (!(conNo.Equals("") || conNo.Equals(null) || conNo.Equals(" ")))
            {

                infocell.Phrase = new Phrase("Contract No. " + conNo, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }

            if (!(Bill_Serial_No.Equals("") || Bill_Serial_No.Equals(null) || Bill_Serial_No.Equals(" ")))
            {

                infocell.Phrase = new Phrase("Dispatch ID: " + Bill_Serial_No, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }


            if (!(ScheduleNo.Equals("") || ScheduleNo.Equals(null) || ScheduleNo.Equals(" ")))
            {

                infocell.Phrase = new Phrase("Schedule No. " + ScheduleNo, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }

            if (!(ConsumerNoc.Equals("") || ConsumerNoc.Equals(null) || ConsumerNoc.Equals(" ")))
            {

                infocell.Phrase = new Phrase("Consumer No.:  " + ConsumerNoc, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                maininfotable.AddCell(infocell);

            }

            maininfotable.WriteSelectedRows(0, -1, 17f, 750f, cb);


            //cb.BeginText();
            //cb.SetTextMatrix(19f, 749f);
            //cb.SetColorFill(new BaseColor(120,120, 120));
            //cb.SetFontAndSize(customfontNormal,7f);
            //cb.ShowText("Tenant");
            //cb.EndText();


            //cb.BeginText();
            //cb.SetTextMatrix(19f, 725f);
            //cb.SetColorFill(new BaseColor(120, 120, 120));
            //cb.SetFontAndSize(customfontNormal, 7f);
            //cb.ShowText("Owner");
            //cb.EndText();


            ////setname

            //cb.BeginText();
            //cb.SetTextMatrix(19f, 734f);
            //cb.SetColorFill(new BaseColor(51, 51, 51));
            //cb.SetFontAndSize(customfontBold, 14);
            //cb.ShowText(PersonName);
            //cb.EndText();
            //set ownweraddress and person details
            //cb.BeginText();
            //cb.SetTextMatrix(19f, 711f);
            //cb.SetColorFill(new BaseColor(51, 51, 51));
            //cb.SetFontAndSize(customfontBold, 14);
            //cb.ShowText(TruncateAtWord(OwnerName,50));
            //cb.EndText();


            //ct.SetLeading(0,0);
            //ct.SetSimpleColumn(new Phrase(new Chunk(AddressInfo, new iTextSharp.text.Font(customfontNormal, 7f, 1, new BaseColor(51, 51, 51)))),
            //                   20f, 709f, 250f, 36, 8f , Element.ALIGN_LEFT);
            //ct.Go();


            //set star logo
            string StarConsumer = pdfvmm.Bill_Type; //1 = star, 2 = disconnection, 3 = none
            if (StarConsumer.Equals("Star Bill"))
            {
                string starLogoimageFilePath = this.imagepath1 + "/images/star-logo.jpg";
                iTextSharp.text.Image starlogoimage = iTextSharp.text.Image.GetInstance(starLogoimageFilePath);
                starlogoimage.ScaleToFit(120, 91);
                starlogoimage.SetAbsolutePosition(165f, 620f);
                doc1.Add(starlogoimage);
            }
            else if (StarConsumer.Equals("Notice Bill"))
            {
                string starLogoimageFilePath = this.imagepath1 + "/images/disconnection-logo.jpg";
                iTextSharp.text.Image starlogoimage = iTextSharp.text.Image.GetInstance(starLogoimageFilePath);
                starlogoimage.ScaleToFit(120, 91);
                starlogoimage.SetAbsolutePosition(165f, 620f);
                doc1.Add(starlogoimage);
            }

            //set Resident Type
            var ResidentTypeLable = pdfvmm.Class_id;
            var TariffType = pdfvmm.Tariff;
            cb.BeginText();
            cb.SetTextMatrix(20f, 635f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 14);
            cb.ShowText(ResidentTypeLable);
            cb.EndText();

            cb.BeginText();
            cb.SetTextMatrix(21f, 622f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(TariffType);
            cb.EndText();
            #endregion



            //SetLabel
            if (this.mode.Equals(WEB_MODE) || this.mode.Equals(EMAIL_MODE))
            {
                var Label = "E-Bill";
                cb.BeginText();
                cb.SetTextMatrix(552, 811.3f);
                cb.SetColorFill(new BaseColor(255, 255, 255));
                cb.SetFontAndSize(customfontBold, 12);
                cb.ShowText(Label);
                cb.EndText();
            }



            //set ibc info
            var IBCName = pdfvmm.IBC_Name;
            //var IBCAddress = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.IBC_Address, 100).ToLower());
            var IBCAddress = TruncateAtWord(pdfvmm.IBC_Address, 100);
            cb.BeginText();
            cb.SetTextMatrix(313, 787f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 9);
            cb.ShowText(IBCName);
            cb.EndText();

            ct.SetSimpleColumn(new Phrase(new Chunk(IBCAddress, new iTextSharp.text.Font(customfontNormal, 6f, 0, new BaseColor(51, 51, 51)))),
                     313f, 785f, 440, 13f, 7f, Element.ALIGN_LEFT | Element.ALIGN_TOP);

            ct.Canvas.Fill();
            ct.Go();

            //set account number
            var AccountNumber = pdfvmm.Account_Number;
            cb.BeginText();
            cb.SetTextMatrix(312, 663f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 14);
            cb.ShowText(AccountNumber);
            cb.EndText();

            //set invoice number
            var InvoiceNumber = pdfvmm.Bill_Id;
            cb.BeginText();
            cb.SetTextMatrix(312, 625f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(InvoiceNumber);
            cb.EndText();

            //set issue-date
            //var IssueDate = DateTime.Now.Day.ToString() + "-" + DateTime.Now.ToString("MMM") + "-" + DateTime.Now.Year.ToString();
            var IssueDate = pdfvmm.Issue_Date;
            //DateTime IssueDatetime = DateTime.ParseExact(IssueDate, "dd-MMM-yyyy",
            //                             System.Globalization.CultureInfo.InvariantCulture);
            //IssueDate = IssueDatetime.ToString("dd") + "-" + IssueDatetime.ToString("MMM") + "-" + IssueDatetime.ToString("yy");
            cb.BeginText();
            cb.SetTextMatrix(431, 625f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(IssueDate.ToString());
            cb.EndText();

            //set bill month
             //var BillMonth = DateTime.Now.ToString("MMM") + "-" + DateTime.Now.ToString("yyyy");
            var BillMonth = pdfvmm.Billing_Month;
            cb.BeginText();
            cb.SetTextMatrix(527, 625f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(BillMonth);
            cb.EndText();

            //set avg temp
            var Avgtemp = pdfvmm.Avg_Temp_Mnth_Cur.Trim(' ');
            var AvgTempText = Avgtemp.ToString() + "°C avg temp";
            cb.BeginText();
            cb.SetTextMatrix(210, 600f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(AvgTempText);
            cb.EndText();
            //set units

            //var CurrentUnits = pdfvmm.Units_billed;
            var CurrentUnits = pdfvmm.Total_Units_Billed.Trim();

            var NetAmount = pdfvmm.Net_Amount;
            Phrase units = new Phrase(new Chunk(CurrentUnits.ToString(), new iTextSharp.text.Font(customfontBold, 18f, 0, new BaseColor(51, 51, 51))));
            units.Add(new Chunk(" Units", new iTextSharp.text.Font(customfontBold, 18f, 0, new BaseColor(51, 51, 51))));
            if (pdfvmm.Net_Amount_Prt.Equals("T"))
            {
                units.Add(new Chunk(" = Rs. " + NetAmount, new iTextSharp.text.Font(customfontBold, 18f, 0, new BaseColor(51, 51, 51))));
            }
            ct.SetSimpleColumn(units, 20f, 584f, 240, 13f, 7f, Element.ALIGN_LEFT | Element.ALIGN_TOP);
            ct.Go();

            //var fulltext = CurrentUnits + " Units";
            //if (pdfvmm.Net_Amount_Prt.Equals("T"))
            //{
            //    fulltext = fulltext + " = Rs. " + NetAmount;
            //}

            //cb.BeginText();
            //cb.SetTextMatrix(20f, 575f);
            //cb.SetColorFill(new BaseColor(51, 51, 51));
            //cb.SetFontAndSize(customfontBold, 19);
            //cb.ShowText(fulltext);
            //cb.EndText();

            //set mode
            //var Mode = pdfvmm.Bill_Charge_Mode;
            //var ModeText = "You are being charged on ";
            //Phrase modep = new Phrase(new Chunk(ModeText, new iTextSharp.text.Font(customfontNormal, 12f, 1, new BaseColor(51, 51, 51))));
            //modep.Add(new Chunk(Mode + " Mode.", new iTextSharp.text.Font(customfontBold, 12f, 1, new BaseColor(51, 51, 51))));
            //ct.SetSimpleColumn(modep, 20f, 564f, 300, 13f, 7f, Element.ALIGN_LEFT | Element.ALIGN_TOP);
            //ct.Go();

            //set amount payable

            var AmountPayable = pdfvmm.Rnd_net_Amt;
            cb.BeginText();
            cb.SetTextMatrix(315, 575f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 19);
            if (!AmountPayable.Equals("0"))
            {
                cb.ShowText("Rs. " + string.Format("{0:n0}", float.Parse(AmountPayable)));
                cb.EndText();
            }
            else
            {
                cb.ShowText("Payment Not Required");
                cb.EndText();
            }


            //set last month average temp
            var PrevMonthAvgtemp = pdfvmm.Avg_Temp_Mnth_Prv.Trim(' ');
            var PrevMonthAvgtempText = PrevMonthAvgtemp.ToString() + "°C avg temp";
            cb.BeginText();
            cb.SetTextMatrix(80, 540f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 10);
            cb.ShowText(PrevMonthAvgtempText);
            cb.EndText();


            //set last month units
            var LastMonthUnits = pdfvmm.Unit_Billed_1;
            Phrase lunits = new Phrase(new Chunk(LastMonthUnits.ToString(), new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))));
            lunits.Add(new Chunk(" Units", new iTextSharp.text.Font(customfontBold, 9f, 0, new BaseColor(51, 51, 51))));
            ct.SetSimpleColumn(lunits, 20f, 515f, 120, 13f, 7f, Element.ALIGN_LEFT | Element.ALIGN_TOP);
            ct.Go();


            //set last month persentage

            string LastMonthPersent = pdfvmm.Per_prv_Month;
            int lmp = Int32.Parse(LastMonthPersent);
            float textWidth = new Chunk(Math.Abs(lmp).ToString(), new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))).GetWidthPoint();
            textWidth = textWidth + new Chunk("%*", new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))).GetWidthPoint();
            textWidth += 8;
            Phrase lmper = new Phrase(new Chunk(Math.Abs(lmp).ToString(), new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))));
            lmper.Add(new Chunk("%*", new iTextSharp.text.Font(customfontBold, 9f, 0, new BaseColor(51, 51, 51))));
            string imageFilePath = this.imagepath1 + "/images/up-arrow.png";
            if (!(lmp == 0))
            {


                ct.SetSimpleColumn(lmper, 70f + 9f, 515f, 130f + 9f, 13f, 7f, Element.ALIGN_RIGHT);
                ct.Go();

                if (lmp > 0)
                {
                    iTextSharp.text.Image arrowimage = iTextSharp.text.Image.GetInstance(imageFilePath);
                    arrowimage.ScaleToFit(15, 13);
                    arrowimage.SetAbsolutePosition(139f - textWidth, 506f);
                    doc1.Add(arrowimage);
                }
                else if (lmp < 0)
                {
                    imageFilePath = this.imagepath1 + "/images/down-arrow.png";
                    iTextSharp.text.Image arrowimage = iTextSharp.text.Image.GetInstance(imageFilePath);
                    arrowimage.ScaleToFit(15, 13);
                    arrowimage.SetAbsolutePosition(139f - textWidth, 506f);
                    doc1.Add(arrowimage);
                }

            }
            //set last year average temp
            var LastYearAvgtemp = pdfvmm.Avg_Temp_Year_Cur.Trim(' ');
            var LastYearAvgtempText = LastYearAvgtemp.ToString() + "°C avg temp";
            cb.BeginText();
            cb.SetTextMatrix(220, 540f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontNormal, 10);
            cb.ShowText(LastYearAvgtempText);
            cb.EndText();


            //set last year units
            var LastYearUnits = pdfvmm.Unit_Month_13;
            Phrase yunits = new Phrase(new Chunk(LastYearUnits.ToString(), new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))));
            yunits.Add(new Chunk(" Units", new iTextSharp.text.Font(customfontBold, 9f, 0, new BaseColor(51, 51, 51))));
            ct.SetSimpleColumn(yunits, 157f, 515f, 217, 13f, 7f, Element.ALIGN_LEFT | Element.ALIGN_TOP);
            ct.Go();



            //set last year persentage

            string LastYearPersent = pdfvmm.Per_Prv_Year;
            int ymp = Int32.Parse(LastYearPersent);

            textWidth = new Chunk(Math.Abs(ymp).ToString(), new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))).GetWidthPoint();
            textWidth = textWidth + new Chunk("%**", new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))).GetWidthPoint();
            textWidth += 8;

            lmper = new Phrase(new Chunk(Math.Abs(ymp).ToString(), new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(51, 51, 51))));
            lmper.Add(new Chunk("%**", new iTextSharp.text.Font(customfontBold, 9f, 0, new BaseColor(51, 51, 51))));
            ct.SetSimpleColumn(lmper, 210f + 9f, 515f, 270f + 9f, 13f, 7f, Element.ALIGN_RIGHT);
            ct.Go();
            if (ymp != 0)
            {

                imageFilePath = this.imagepath1 + "/images/up-arrow.png";
                if (ymp > 0)
                {
                    imageFilePath = this.imagepath1 + "/images/up-arrow.png";
                    iTextSharp.text.Image arrowimage = iTextSharp.text.Image.GetInstance(imageFilePath);
                    arrowimage.ScaleToFit(15, 13); //13
                    arrowimage.SetAbsolutePosition(279f - textWidth, 506f);
                    doc1.Add(arrowimage);
                }
                else if (ymp < 0)
                {
                    imageFilePath = this.imagepath1 + "/images/down-arrow.png";
                    iTextSharp.text.Image arrowimage = iTextSharp.text.Image.GetInstance(imageFilePath);
                    arrowimage.ScaleToFit(15, 13); //13
                    arrowimage.SetAbsolutePosition(279f - textWidth, 506f);
                    doc1.Add(arrowimage);
                }

            }



            //set duedate saving
            var DueDateSaving = pdfvmm.LPS_Amt;
            cb.BeginText();
            cb.SetTextMatrix(338, 477f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 19);
            cb.ShowText("Rs. " + string.Format("{0:n}", float.Parse(DueDateSaving)));
            cb.EndText();



            //set due date text
            var DueDate = pdfvmm.Due_Date;
            DateTime myDate = DateTime.ParseExact(DueDate, "dd-MMM-yyyy",
                                         System.Globalization.CultureInfo.InvariantCulture);
            if (!AmountPayable.Equals("0"))
            {

                Phrase DueDatetext = new Phrase(new Chunk(myDate.Day.ToString(), new iTextSharp.text.Font(customfontBold, 19f, 0, new BaseColor(51, 51, 51))));
                var suffex = datesuffex(myDate.Day);
                DueDatetext.Add(new Chunk(suffex, new iTextSharp.text.Font(customfontBold, 14f, 0, new BaseColor(51, 51, 51))));
                DueDatetext.Add(new Chunk("\n" + myDate.ToString("MMMM"), new iTextSharp.text.Font(customfontBold, 19, 0, new BaseColor(51, 51, 51))));
                DueDatetext.Add(new Chunk("\n" + myDate.Year.ToString(), new iTextSharp.text.Font(customfontBold, 19f, 0, new BaseColor(51, 51, 51))));
                ct.SetSimpleColumn(DueDatetext, 488f, 522f, 588f, 13f, 20f, Element.ALIGN_CENTER);
                ct.Go();
            }



            //set amount payable after due date
            var DueDateAfter = pdfvmm.Rnd_Gross_Amt;
            cb.BeginText();
            cb.SetTextMatrix(315, 394f);
            cb.SetColorFill(new BaseColor(51, 51, 51));
            cb.SetFontAndSize(customfontBold, 19);
            if (!DueDateAfter.Equals("0"))
            {
                cb.ShowText("Rs. " + string.Format("{0:n0}", float.Parse(DueDateAfter)));
            }
            else
            {
                cb.ShowText("Payment Not Required");

            }
            cb.EndText();


            var Message = pdfvmm.Message_Board;
            if (pdfvmm.Message_Board.Length > 200)
            {
                Message.Substring(0, 200);
            }
            Phrase Messagetext = new Phrase(new Chunk(Message, new iTextSharp.text.Font(customfontNormal, 7f, 1, new BaseColor(51, 51, 51))));
            ct.SetSimpleColumn(Messagetext, 320f, 360f, 580f, 20f, 10f, Element.ALIGN_LEFT);
            ct.Go();


            //set contract account and set contract number


            var ContractAccount = pdfvmm.Account_Number;
            var ContractNumber = pdfvmm.Contract_no;
            PdfPTable contractinfotable = new PdfPTable(1);
            contractinfotable.TotalWidth = 190f;
            contractinfotable.LockedWidth = true;
            float[] widths = new float[] { 1f };
            contractinfotable.SetWidths(widths);
            contractinfotable.HorizontalAlignment = 1;


            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Border = 0;
            cell.HorizontalAlignment = 2;
            cell.PaddingTop = 0.5f;
            cell.Phrase = new Phrase(TruncateAtWord(OwnerName, 30), new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(51, 51, 51)));
            contractinfotable.AddCell(cell);
            cell.Phrase = new Phrase(ContractAccount, new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(51, 51, 51)));
            contractinfotable.AddCell(cell);
            cell.Phrase = new Phrase(ContractNumber, new iTextSharp.text.Font(customfontNormal, 12, 0, new BaseColor(51, 51, 51)));
            contractinfotable.AddCell(cell);

            contractinfotable.WriteSelectedRows(0, -1, 103f, 95.5f, cb);


            //set invoice number & set due date
            var DueDateFull = myDate.ToString("dd") + "-" + myDate.ToString("MMM") + "-" + myDate.ToString("yy");
            PdfPTable Invoiceinfotable = new PdfPTable(2);
            Invoiceinfotable.TotalWidth = 265f;
            Invoiceinfotable.LockedWidth = true;
            widths = new float[] { 1f, 1f };
            Invoiceinfotable.SetWidths(widths);
            Invoiceinfotable.HorizontalAlignment = 1;


            cell = new PdfPCell(new Phrase(""));
            cell.Border = 0;
            cell.HorizontalAlignment = 0;

            cell.Phrase = new Phrase(InvoiceNumber, new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(51, 51, 51)));
            Invoiceinfotable.AddCell(cell);
            cell.HorizontalAlignment = 2;



            if (!pdfvmm.Rnd_net_Amt.Equals("0"))
            {
                cell.Phrase = new Phrase(DueDateFull, new iTextSharp.text.Font(customfontNormal, 12, 0, new BaseColor(51, 51, 51)));
                Invoiceinfotable.AddCell(cell);
            }
            else
            {
                cell.Phrase = new Phrase("", new iTextSharp.text.Font(customfontNormal, 12, 0, new BaseColor(51, 51, 51)));
                Invoiceinfotable.AddCell(cell);
            }
            Invoiceinfotable.WriteSelectedRows(0, -1, 311f, 127f, cb);



            //set within due date & set after due date payment info
            PdfPTable Paymentinfotable = new PdfPTable(2);
            Paymentinfotable.TotalWidth = 265f;
            Paymentinfotable.LockedWidth = true;
            widths = new float[] { 1f, 1f };
            Paymentinfotable.SetWidths(widths);
            Paymentinfotable.HorizontalAlignment = 1;

            cell = new PdfPCell(new Phrase(""));
            cell.Border = 0;
            cell.HorizontalAlignment = 0;

            if (AmountPayable.Equals("0"))
            {
                cell.HorizontalAlignment = 1;
                cell.Colspan = 2;
                cell.Phrase = new Phrase("Payment Not Required", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(51, 51, 51)));
                Paymentinfotable.AddCell(cell);

            }
            else
            {

                cell.Phrase = new Phrase("Rs. " + string.Format("{0:n0}", float.Parse(AmountPayable)), new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(51, 51, 51)));
                Paymentinfotable.AddCell(cell);
                cell.HorizontalAlignment = 2;

                cell.Phrase = new Phrase("Rs. " + string.Format("{0:n0}", float.Parse(DueDateAfter)), new iTextSharp.text.Font(customfontNormal, 12, 0, new BaseColor(51, 51, 51)));
                Paymentinfotable.AddCell(cell);

            }

            Paymentinfotable.WriteSelectedRows(0, -1, 311f, 87f, cb);

        }

        public void setSecondPageInfo(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {

            //setting 
            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;


            //meterno & reading Date
            var MeterNo = pdfvmm.Meter_Number;
            //   var ReadingDate = DateTime.Now.Day.ToString() + "-" + DateTime.Now.ToString("MMM") + "-" + DateTime.Now.Year.ToString();

            var ReadingDate = pdfvmm.Meter_Reading_Date;
            PdfPTable metertable = new PdfPTable(1);
            metertable.TotalWidth = 150f;
            metertable.LockedWidth = true;
            float[] widths = new float[] { 1 };
            metertable.SetWidths(widths);
            PdfPCell cell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            cell.UseVariableBorders = true;
            cell.Border = 0;
            cell.PaddingBottom = 0;
            cell.HorizontalAlignment = 2;

            cell.Phrase = new Phrase("Bill Charge Mode. " + pdfvmm.Bill_Charge_Mode, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
            metertable.AddCell(cell);
            cell.Phrase = new Phrase("Meter No. " + pdfvmm.Meter_Number, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
            metertable.AddCell(cell);
            cell.Phrase = new Phrase("Reading Date " + pdfvmm.Meter_Reading_Date, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
            metertable.AddCell(cell);


            metertable.WriteSelectedRows(0, -1, 138f, 802f, cb);




            //TODO: current bill calculation table

            drawElectrcityUsageTable(doc1, writer, pdfvmm);

            //TODO: Customer Information
            drawCustomerInformationTable(doc1, writer, pdfvmm);


            //Billing Statment


            if (!(pdfvmm.Billing_statment_table.Equals(null) || pdfvmm.Billing_statment_table.Equals("")))
            {
                drawBillingStatmentTable(doc1, writer, pdfvmm);
            }

            //TODO: Set Meter Image
            //ISK_D1
            //Set Meter Image Path for validation
            string MeterImagePathYear = pdfvmm.Billing_Month.Substring(6, 2);
            string MeterImagePathMonthYear = "";
            switch (pdfvmm.Billing_Month.Substring(0, 3)) {
                case "Jan": MeterImagePathMonthYear = "01" + MeterImagePathYear; break;
                case "Feb": MeterImagePathMonthYear = "02" + MeterImagePathYear; break;
                case "Mar": MeterImagePathMonthYear = "03" + MeterImagePathYear; break;
                case "Apr": MeterImagePathMonthYear = "04" + MeterImagePathYear; break;
                case "May": MeterImagePathMonthYear = "05" + MeterImagePathYear; break;
                case "Jun": MeterImagePathMonthYear = "06" + MeterImagePathYear; break;
                case "Jul": MeterImagePathMonthYear = "07" + MeterImagePathYear; break;
                case "Aug": MeterImagePathMonthYear = "08" + MeterImagePathYear; break;
                case "Sep": MeterImagePathMonthYear = "09" + MeterImagePathYear; break;
                case "Oct": MeterImagePathMonthYear = "10" + MeterImagePathYear; break;
                case "Nov": MeterImagePathMonthYear = "11" + MeterImagePathYear; break;
                case "Dec": MeterImagePathMonthYear = "12" + MeterImagePathYear; break;
            }
            string MeterImageCycleDay = pdfvmm.Bill_Serial_No.Substring(0, 2);
            string MeterImageIBC = "" ;
            switch (pdfvmm.IBC_Name) {
                case "Customer Care Centre Bahadurabad": MeterImageIBC = "137"; break;
                case "Customer Care Centre Baldia": MeterImageIBC = "143"; break;
                case "Customer Care Centre Bin Qasim": MeterImageIBC = "142"; break;
                case "Customer Care Centre Clifton": MeterImageIBC = "125"; break;
                case "Customer Care Centre Defence": MeterImageIBC = "118"; break;
                case "Customer Care Centre F.B. Area": MeterImageIBC = "132"; break;
                case "Customer Care Centre Gadap": MeterImageIBC = "147"; break;
                case "Customer Care Centre Garden": MeterImageIBC = "138"; break;
                case "Customer Care Centre Gulshan-e-Iqbal": MeterImageIBC = "123"; break;
                case "Customer Care Centre Johar": MeterImageIBC = "130"; break;
                case "Customer Care Centre Johar-II": MeterImageIBC = "152"; break;
                case "Customer Care Centre KIMZ": MeterImageIBC = "128"; break;
                case "Customer Care Centre Korangi": MeterImageIBC = "146"; break;
                case "Customer Care Centre Landhi": MeterImageIBC = "141"; break;
                case "Customer Care Centre LYARI II": MeterImageIBC = "144"; break;
                case "Customer Care Centre Liaqatabad": MeterImageIBC = "127"; break;
                case "Customer Care Centre Lyari-I": MeterImageIBC = "140"; break;
                case "Customer Care Centre Malir": MeterImageIBC = "135"; break;
                case "Customer Care Centre Nazimabad": MeterImageIBC = "136"; break;
                case "Customer Care Centre New Karachi": MeterImageIBC = "134"; break;
                case "Customer Care Centre North Karachi": MeterImageIBC = "133"; break;
                case "Customer Care Centre North Nazimabad": MeterImageIBC = "124"; break;
                case "Customer Care Centre Orangi-I": MeterImageIBC = "150"; break;
                case "Customer Care Centre Orangi-II": MeterImageIBC = "149"; break;
                case "Customer Care Centre Saddar": MeterImageIBC = "131"; break;
                case "Customer Care Centre Shah Faisal": MeterImageIBC = "148"; break;
                case "Customer Care Centre SIMZ": MeterImageIBC = "129"; break;
                case "Customer Care Centre Surjani - II": MeterImageIBC = "153"; break;
                case "Customer Care Centre Tipu Sultan": MeterImageIBC = "145"; break;
                case "Customer Care Centre Uthal": MeterImageIBC = "151"; break;
            }
            string DynamicMeterImageFilePath = "https://mmr.ke.com.pk:44300/MMR_ImageData/" + MeterImagePathYear + "/" + MeterImagePathMonthYear + "/" + MeterImageCycleDay + "/"+ MeterImageIBC+ "/00" + pdfvmm.Contract_no;

            try
            {
                PrintMeterImage(doc1, writer, pdfvmm, DynamicMeterImageFilePath);
            }
            catch (Exception ex){
                PrintBlankMeterImage(doc1, writer, pdfvmm, DynamicMeterImageFilePath);
            }


        }

        public void drawCustomerInformationTable(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            string fontpath = this.fontpath1;
            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            #region old_fixed_formate
            //PdfPTable table = new PdfPTable(4);
            //table.TotalWidth = 200f;
            //table.LockedWidth = true;
            //float[] widths = new float[] { 1.3f, 1f, 1.3f, 1f };
            //table.SetWidths(widths);
            //table.HorizontalAlignment = 0;

            //PdfPCell cell = new PdfPCell(new Phrase("Products"));
            //cell.Border = 0;
            //cell.PaddingLeft = 2;
            //cell.HorizontalAlignment = 0;
            //cell.BorderColor = new BaseColor(51, 51, 51);
            ////row 1
            //cell.Phrase = new Phrase("Consumer No.", new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.BorderWidthRight = 1;
            //cell.Phrase = new Phrase(pdfvmm.Consumer_Number, new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.Border = 0;
            //cell.PaddingLeft = 4;
            //cell.Phrase = new Phrase("Sanc. Load", new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.PaddingLeft = 2;
            //cell.Phrase = new Phrase(pdfvmm.Sanc_Load_kw, new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);

            ////row 2
            //cell.Phrase = new Phrase("Contract No. ", new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.BorderWidthRight = 1;
            //cell.Phrase = new Phrase(pdfvmm.Contract_no, new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.Border = 0;
            //cell.PaddingLeft = 4;
            //cell.Phrase = new Phrase("Conn. Load", new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.PaddingLeft = 2;
            //cell.Phrase = new Phrase(pdfvmm.Connected_Load_kw, new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);

            ////row 3
            //cell.Phrase = new Phrase("Security Deposit", new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.BorderWidthRight = 1;
            //cell.Phrase = new Phrase(pdfvmm.Security_Deposit, new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.Border = 0;
            //cell.PaddingLeft = 4;
            //cell.Phrase = new Phrase("Premises Type", new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);
            //cell.PaddingLeft = 2;
            //cell.Phrase = new Phrase(pdfvmm.Cat_id, new iTextSharp.text.Font(customfontNormal, 8f, 1, new BaseColor(51, 51, 51)));
            //table.AddCell(cell);


            //PdfContentByte cb = writer.DirectContent;
            //table.WriteSelectedRows(0, -1, 290f, 800f, cb);
            #endregion

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 104f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1.0f, 1.1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;

            PdfPCell cell = new PdfPCell(new Phrase("Products"));
            cell.Border = 0;
            cell.PaddingLeft = 2f;
            cell.HorizontalAlignment = 0;
            cell.BorderColor = new BaseColor(51, 51, 51);
            cell.PaddingBottom = 0f;


            ArrayList CustomerTableA = Parser.parseCustomerInfodata(pdfvmm.Customer_Info_Table_A);
            foreach (CustomerInfoModel item in CustomerTableA)
            {
                cell.HorizontalAlignment = 0;
                cell.Phrase = new Phrase(item.label, new iTextSharp.text.Font(customfontNormal, 7f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(cell);

                cell.HorizontalAlignment = 2;
                cell.BorderWidthRight = 0.5f;
                cell.Phrase = new Phrase(item.info, new iTextSharp.text.Font(customfontNormal, 7f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(cell);
                cell.Border = 0;
            }

            PdfContentByte cb = writer.DirectContent;
            table.WriteSelectedRows(0, -1, 292f, 800f, cb);


            PdfPTable tableb = new PdfPTable(2);

            tableb.TotalWidth = 95f;
            tableb.LockedWidth = true;
            widths = new float[] { 1.3f, 1f };
            tableb.SetWidths(widths);
            tableb.HorizontalAlignment = 0;

            ArrayList CustomerTableB = Parser.parseCustomerInfodata(pdfvmm.Customer_Info_Table_B);
            foreach (CustomerInfoModel item in CustomerTableB)

            {
                cell.HorizontalAlignment = 0;
                cell.PaddingLeft = 4;
                cell.PaddingRight = 0;
                cell.Phrase = new Phrase(item.label, new iTextSharp.text.Font(customfontNormal, 7f, 0, new BaseColor(51, 51, 51)));
                tableb.AddCell(cell);

                cell.PaddingLeft = 0;
                cell.PaddingRight = 8;
                cell.HorizontalAlignment = 2;
                cell.Phrase = new Phrase(item.info, new iTextSharp.text.Font(customfontNormal, 7f, 0, new BaseColor(51, 51, 51)));
                tableb.AddCell(cell);

            }

            cb = writer.DirectContent;
            tableb.WriteSelectedRows(0, -1, 394f, 800f, cb);


        }

        public void drawBillingStatmentTable(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {

            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            // normal cell
            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Border = 0;
            cell.PaddingBottom = 2;
            cell.PaddingTop = 1;
            cell.HorizontalAlignment = 1;
            cell.BorderColor = new BaseColor(51, 51, 51);

            // bold cell
            PdfPCell boldcell = new PdfPCell(new Phrase(""));
            boldcell.Border = 0;
            boldcell.PaddingTop = 3;
            boldcell.PaddingBottom = 3;
            boldcell.HorizontalAlignment = 0;
            boldcell.BorderColor = new BaseColor(51, 51, 51);

            PdfPCell darkcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            boldcell.Border = 0;
            boldcell.PaddingTop = 3;
            boldcell.PaddingBottom = 3;
            boldcell.HorizontalAlignment = 0;

            PdfPCell greycell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            greycell.UseVariableBorders = true;
            greycell.BackgroundColor = new BaseColor(204, 204, 204);
            greycell.BorderColor = new BaseColor(204, 204, 204);
            greycell.BorderWidthLeft = 0;
            greycell.BorderWidthRight = 0;
            greycell.PaddingTop = 3;
            greycell.PaddingBottom = 4;
            greycell.BorderColor = new BaseColor(255, 255, 255);

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 190f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3f, 1.3f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;



            ArrayList billingstatmentdata = Parser.parseBillingdata(pdfvmm.Billing_statment_table);
            string[] boldcheck = { "Previous Balance", "Current Balance", "Amount Payable within Due Date", "Amount Payable after Due Date", "As of Arrears", "No. of Balance Instalment" };
            foreach (BillingModel item in billingstatmentdata)
            {

                if (item.Formate.Equals("G"))//boldcheck.Contains(item.Label)
                {

                    greycell.HorizontalAlignment = 0;
                    greycell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(greycell);

                    greycell.HorizontalAlignment = 2;
                    greycell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(greycell);

                }
                else if (item.Formate.Equals("B"))//boldcheck.Contains(item.Label)
                {
                    boldcell.HorizontalAlignment = 0;
                    boldcell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(boldcell);

                    boldcell.HorizontalAlignment = 2;
                    boldcell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(boldcell);
                }
                else {
                    cell.HorizontalAlignment = 0;
                    cell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                    cell.HorizontalAlignment = 2;
                    cell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                    table.AddCell(cell);
                }
            }

            PdfContentByte cb = writer.DirectContent;
            table.WriteSelectedRows(0, -1, 292f, 715f, cb);

        }

        public void PrintBlankMeterImage(Document doc1, PdfWriter writer, PDFViewModel pdfvmm, string DynamicMeterImageFilePath)
        {
            string MeterImageFilePath = this.imagepath1 + "/images/Blank-meter.jpg";
            iTextSharp.text.Image MeterImage = iTextSharp.text.Image.GetInstance(MeterImageFilePath);
            MeterImage.ScaleToFit(190, 95);
            MeterImage.SetAbsolutePosition(292f, 450f);
            doc1.Add(MeterImage);
        }

        public void PrintMeterImage(Document doc1, PdfWriter writer, PDFViewModel pdfvmm,string DynamicMeterImageFilePath) {
            if (pdfvmm.P_Units_Billed.Equals(" "))
            {
                // Meter image path : https:\\mmr.ke.com.pk:44300\MMR_ImageData\21\0321\01\118\0030081830.jpg

                //string MeterImageFilePath = this.imagepath1 + "/images/0030081830.jpg";

                //string MeterImageFilePath = "https://mmr.ke.com.pk:44300/MMR_ImageData/21/0321/01/118/0030081830.jpg";
                string MeterImageFilePath = DynamicMeterImageFilePath + ".jpg";
                iTextSharp.text.Image MeterImage = iTextSharp.text.Image.GetInstance(MeterImageFilePath);
                MeterImage.ScaleToFit(190, 95);
                MeterImage.SetAbsolutePosition(292f, 450f);
                doc1.Add(MeterImage);
            }
            else
            {
                //    For TOU customers:
                //https:\\mmr.ke.com.pk:44300\MMR_ImageData\21\0321\01\118\0030073299K.jpg
                //https:\\mmr.ke.com.pk:44300\MMR_ImageData\21\0321\01\118\0030073299P.jpg

                //K Image
                //string KMeterImageFilePath = "https://mmr.ke.com.pk:44300/MMR_ImageData/21/0321/01/118/0030081830.jpg"; DynamicMeterImageFilePath + ".jpg";
                string KMeterImageFilePath = DynamicMeterImageFilePath + "K.jpg";
                iTextSharp.text.Image KMeterImage = iTextSharp.text.Image.GetInstance(KMeterImageFilePath);
                KMeterImage.ScaleToFit(190, 95);
                KMeterImage.SetAbsolutePosition(292f, 470f);
                doc1.Add(KMeterImage);

                //PImage
                string PMeterImageFilePath = DynamicMeterImageFilePath + "P.jpg";
                iTextSharp.text.Image PMeterImage = iTextSharp.text.Image.GetInstance(PMeterImageFilePath);
                PMeterImage.ScaleToFit(190, 95);
                PMeterImage.SetAbsolutePosition(292f, 395f);
                doc1.Add(PMeterImage);
            }
        }

        public void drawElectrcityUsageTable(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 265f;
            table.LockedWidth = true;
            float[] widths = new float[] { 4f, 2f, 2f, 1f, 2f, 1f };
            table.SetWidths(widths);

            var tableheight = 15f;
            var firsttableprint = false;

            PdfPCell headingcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            PdfPCell rowCell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            PdfPCell boldrowCell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            PdfPCell darkcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            PdfPCell greycell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };

            greycell.UseVariableBorders = true;
            greycell.BackgroundColor = new BaseColor(204, 204, 204);
            greycell.BorderColor = new BaseColor(204, 204, 204);
            greycell.BorderWidthLeft = 0;
            greycell.BorderWidthRight = 0;
            greycell.PaddingTop = 3;
            greycell.PaddingBottom = 4;
            greycell.BorderColor = new BaseColor(255, 255, 255);

            darkcell.UseVariableBorders = true;
            darkcell.BackgroundColor = new BaseColor(51, 51, 51);
            darkcell.BorderColor = new BaseColor(51, 51, 51);
            darkcell.BorderWidthLeft = 0;
            darkcell.BorderWidthRight = 0;
            darkcell.PaddingTop = 3;
            darkcell.PaddingBottom = 4;
            darkcell.BorderColor = new BaseColor(255, 255, 255);



            //BUILDING HEADER

            headingcell.UseVariableBorders = true;
            headingcell.BorderWidthLeft = 0f;
            headingcell.BorderWidthRight = 0f;
            headingcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingcell.BorderColor = BaseColor.DARK_GRAY;
            table.AddCell(headingcell);
            headingcell.PaddingBottom = 3f;
            headingcell.BorderWidthLeft = 0f;
            headingcell.BorderWidthRight = 0.5f;


            //creating row cell


            rowCell.PaddingBottom = 0f;
            boldrowCell.PaddingBottom = 0f;
            boldrowCell.Border = 0;
            rowCell.Border = 0;


            if (!(pdfvmm.Electricity_usage_table.Equals(null) || pdfvmm.Electricity_usage_table.Equals("") || pdfvmm.Electricity_usage_table.Equals(" ")))
            {


                firsttableprint = true;
                ArrayList usageData = Parser.parseUseagedata(pdfvmm.Electricity_usage_table);

                cb.BeginText();
                cb.SetTextMatrix(21f, 771f);
                cb.SetColorFill(new BaseColor(51, 51, 51));
                cb.SetFontAndSize(customfontBold, 10);
                cb.ShowText("The electricity you have used");
                cb.EndText();




                headingcell.Phrase = new Phrase("Previous Reading", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(headingcell);
                headingcell.Phrase = new Phrase("Current Reading", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(headingcell);
                headingcell.VerticalAlignment = Element.ALIGN_TOP;
                headingcell.Phrase = new Phrase("MMF", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(headingcell);
                headingcell.Phrase = new Phrase("Units\n(KWh)", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(headingcell);
                headingcell.BorderWidthRight = 0f;
                headingcell.Phrase = new Phrase("MDI\n(KW)", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table.AddCell(headingcell);


                //creating row cell


                rowCell.PaddingBottom = 0f;
                boldrowCell.PaddingBottom = 0f;
                boldrowCell.Border = 0;

                string[] boldlist = { "Net Energy Off Peak", "Net Energy Peak" };

                //TODO: setreal data
                foreach (UsageModel item in usageData)
                {

                    if (item.Formate.Equals("B"))//boldlist.Contains(item.Label)
                    {
                        boldrowCell.Border = 0;
                        boldrowCell.HorizontalAlignment = 0;
                        boldrowCell.PaddingRight = 0;
                        boldrowCell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 2;
                        boldrowCell.PaddingRight = 9;
                        boldrowCell.Phrase = new Phrase(item.PreviousReading, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 2;
                        boldrowCell.PaddingRight = 9;
                        boldrowCell.Phrase = new Phrase(item.CurrentReading, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 1;
                        boldrowCell.PaddingRight = 0;
                        boldrowCell.Phrase = new Phrase(item.MMF, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 2;
                        boldrowCell.PaddingRight = 10;
                        boldrowCell.Phrase = new Phrase(item.Units, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 1;
                        boldrowCell.PaddingRight = 0;
                        boldrowCell.Phrase = new Phrase(item.MDI, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(boldrowCell);
                    }
                    else {

                        rowCell.Border = 0;
                        rowCell.HorizontalAlignment = 0;
                        rowCell.PaddingRight = 0;
                        rowCell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 2;
                        //rowCell.PaddingRight = 9;
                        rowCell.Phrase = new Phrase(item.PreviousReading, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 2;
                        rowCell.PaddingRight = 9;
                        rowCell.Phrase = new Phrase(item.CurrentReading, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 1;
                        rowCell.PaddingRight = 0;
                        rowCell.Phrase = new Phrase(item.MMF, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 2;
                        rowCell.PaddingRight = 10;
                        rowCell.Phrase = new Phrase(item.Units, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 1;
                        rowCell.PaddingRight = 0;
                        rowCell.Phrase = new Phrase(item.MDI, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table.AddCell(rowCell);
                    }
                }


                table.WriteSelectedRows(0, -1, 21f, 769f, cb);
            }

            if (firsttableprint == true)
            {
                tableheight = table.CalculateHeights() + table.GetRowHeight(0) + 15f;
            }

            if (!(pdfvmm.Electricity_charges_table.Equals(null) || pdfvmm.Electricity_charges_table.Equals("")))
            {

                //heading
                cb.BeginText();
                cb.SetTextMatrix(21f, 775f - tableheight);
                cb.SetColorFill(new BaseColor(51, 51, 51));
                cb.SetFontAndSize(customfontBold, 10);
                cb.ShowText("Your electricity charges for the period");
                cb.EndText();

                //no_of_months
                var no_of_months = pdfvmm.No_Of_Month;
                cb.BeginText();
                cb.SetTextMatrix(221f, 775f - tableheight);
                cb.SetColorFill(new BaseColor(51, 51, 51));
                cb.SetFontAndSize(customfontNormal, 9f);
                cb.ShowText("No. of Month(s): " + no_of_months.ToString());
                cb.EndText();



                var Upto = pdfvmm.Billed_upto;
                if (!(Upto.Equals("") || Upto.Equals(" ") || Upto.Equals("0")))
                {
                    cb.BeginText();
                    cb.SetTextMatrix(149f, 775f - tableheight + 15);
                    cb.SetColorFill(new BaseColor(51, 51, 51));
                    cb.SetFontAndSize(customfontNormal, 8f);
                    cb.ShowText("Billed Upto: " + Upto.ToString());
                    cb.EndText();
                }


                var UnitAdj = pdfvmm.Units_Adjusted;
                if (!(UnitAdj.Equals("") || UnitAdj.Equals(" ") || UnitAdj.Equals("0")))
                {
                    cb.BeginText();
                    cb.SetTextMatrix(221f, 775f - tableheight + 15);
                    cb.SetColorFill(new BaseColor(51, 51, 51));
                    cb.SetFontAndSize(customfontNormal, 8f);
                    cb.ShowText("Unit Adj: " + UnitAdj.ToString());
                    cb.EndText();
                }


                //starting second table
                PdfPTable table2 = new PdfPTable(4);
                table2.TotalWidth = 265f;
                table2.LockedWidth = true;
                widths = new float[] { 2f, 1f, 0.8f, 1.2f };
                table2.SetWidths(widths);

                //setting headers
                headingcell.UseVariableBorders = true;
                headingcell.BorderWidthLeft = 0f;
                headingcell.BorderWidthRight = 0f;
                headingcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headingcell.BorderColor = BaseColor.DARK_GRAY;
                headingcell.Phrase = new Phrase("", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table2.AddCell(headingcell);
                headingcell.PaddingBottom = 3f;
                headingcell.BorderWidthLeft = 0f;
                headingcell.BorderWidthRight = 0.5f;
                headingcell.Phrase = new Phrase("Units", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table2.AddCell(headingcell);
                headingcell.Phrase = new Phrase("Rate / Unit", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table2.AddCell(headingcell);
                headingcell.BorderWidthRight = 0f;
                headingcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headingcell.Phrase = new Phrase("Amount", new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                table2.AddCell(headingcell);

                rowCell.PaddingTop = 1;
                rowCell.PaddingBottom = 1;
                rowCell.PaddingRight = 2;
                boldrowCell.PaddingBottom = 1;
                boldrowCell.PaddingTop = 1;
                boldrowCell.PaddingRight = 2;


                ArrayList chargesdata = Parser.parseChargesdata(pdfvmm.Electricity_charges_table);
                string[] darkcellarray = { "Your Electricity Charges for the Period" };
                string[] greycellarray = { "Energy Charges", "Government Charges" };
                string[] sigarry = { "Fixed Charges (kW)", "Variable Charges", "Surcharge", "PF Penalty", "Fuel Surcharge Adjustment", "Meter Rent" };


                foreach (ChargesModel item in chargesdata)
                {
                    if (item.Formate.Equals("D"))//darkcellarray.Contains(item.Label)
                    {

                        darkcell.HorizontalAlignment = 0;
                        darkcell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 9f, 0, new BaseColor(255, 255, 255)));
                        darkcell.Colspan = 3;
                        table2.AddCell(darkcell);


                        darkcell.HorizontalAlignment = 2;
                        darkcell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 9f, 0, new BaseColor(255, 255, 255)));
                        table2.AddCell(darkcell);
                    }
                    else if (item.Formate.Equals("G")) //greycellarray.Contains(item.Label)
                    {
                        greycell.HorizontalAlignment = 0;
                        greycell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(greycell);

                        greycell.HorizontalAlignment = 1;
                        greycell.Phrase = new Phrase(item.Units, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(greycell);

                        greycell.HorizontalAlignment = 1;
                        greycell.Phrase = new Phrase(item.Rates, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(greycell);

                        greycell.HorizontalAlignment = 2;
                        greycell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(greycell);
                    }
                    else if (item.Formate.Equals("B"))//sigarry.Contains(item.Label)
                    {

                        boldrowCell.Border = 0;
                        boldrowCell.HorizontalAlignment = 0;
                        boldrowCell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 1;
                        boldrowCell.Phrase = new Phrase(item.Units, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 1;
                        boldrowCell.Phrase = new Phrase(item.Rates, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(boldrowCell);

                        boldrowCell.HorizontalAlignment = 2;
                        boldrowCell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(boldrowCell);
                    }
                    else
                    {
                        rowCell.HorizontalAlignment = 0;
                        rowCell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 1;
                        rowCell.Phrase = new Phrase(item.Units, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 1;
                        rowCell.Phrase = new Phrase(item.Rates, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(rowCell);

                        rowCell.HorizontalAlignment = 2;
                        rowCell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(51, 51, 51)));
                        table2.AddCell(rowCell);
                    }
                }
                table2.WriteSelectedRows(0, -1, 21f, 772f - (tableheight), cb);
            }
        }

        public void addVoucherHeader(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);

            //IList<RCVoucherModel> rcList = Parser.parseRCVoucher(pdfvmm.V_RC_Table);



            var PersonName = pdfvmm.Tenant_Name;
            var OwnerName = pdfvmm.Customer_Name;
            var Address = pdfvmm.Customer_Address;
            var ForwardingAddress = pdfvmm.Forwarding_Address;
            var GST = pdfvmm.Stax_no;
            var PhoneNumber = pdfvmm.Customer_Cell_Nmbr;
            var CNIC = pdfvmm.NIC_Number;
            var iTax_No = pdfvmm.ITax_No;
            var Bill_Serial_No = pdfvmm.Bill_Serial_No;
            var ScheduleNo = pdfvmm.schd_no;
            var ConsumerNoc = pdfvmm.Consumer_Number;
            var AddressInfo = Address
                            + "\n" + ForwardingAddress
                            + "\nGST No.:" + GST
                            + "\nCNIC No.:" + CNIC
                            + "\nYour Registered Mobile Number: " + PhoneNumber
                            + "\nNTN No.: " + PhoneNumber
                            + "\nDispatch ID: " + Bill_Serial_No;
            var conNo = pdfvmm.Contract_no;

            PdfPTable maininfotable = new PdfPTable(1);
            maininfotable.TotalWidth = 265f;
            maininfotable.LockedWidth = true;
            var widthss = new float[] { 1f };
            maininfotable.SetWidths(widthss);
            maininfotable.HorizontalAlignment = 1;

            PdfPCell namecell = new PdfPCell(new Phrase(""));
            namecell.Border = 0;
            namecell.HorizontalAlignment = 0;
            namecell.PaddingTop = 1;
            namecell.PaddingBottom = 3f;


            PdfPCell labelcell = new PdfPCell(new Phrase(""));
            labelcell.Border = 0;
            labelcell.HorizontalAlignment = 0;
            labelcell.PaddingBottom = 0;


            PdfPCell infocell = new PdfPCell(new Phrase(""));
            infocell.Border = 0;
            infocell.HorizontalAlignment = 0;
            infocell.Padding = 0;
            infocell.PaddingLeft = 2;
            infocell.PaddingBottom = 2f;


            if (!(PersonName.Equals("") || PersonName.Equals(null)))
            {
                labelcell.Phrase = new Phrase("Tenant", new iTextSharp.text.Font(customfontNormal, 7f, 1, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(labelcell);

                namecell.Phrase = new Phrase(PersonName, new iTextSharp.text.Font(customfontBold, 15, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(namecell);

                labelcell.PaddingTop = 1;
                labelcell.Phrase = new Phrase("Owner", new iTextSharp.text.Font(customfontNormal, 7f, 1, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(labelcell);
            }


            if (!(OwnerName.Equals("") || OwnerName.Equals(null)))
            {
                namecell.Phrase = new Phrase(OwnerName, new iTextSharp.text.Font(customfontBold, 15, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(namecell);
            }

            if (!(Address.Equals("") || Address.Equals(null)))
            {
                infocell.Phrase = new Phrase(Address, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }
            if (!(ForwardingAddress.Equals("") || ForwardingAddress.Equals(null) || ForwardingAddress.Equals(" ")))
            {
                infocell.Phrase = new Phrase(ForwardingAddress, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }

            if (!(GST.Equals("") || GST.Equals(null) || GST.Equals(" ")))
            {
                infocell.Phrase = new Phrase("GST No. " + GST, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }
            if (!(CNIC.Equals("") || CNIC.Equals(null) || CNIC.Equals(" ")))
            {
                infocell.Phrase = new Phrase("CNIC No. " + CNIC, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }


            if (!(PhoneNumber.Equals("") || PhoneNumber.Equals(null) || PhoneNumber.Equals(" ")))
            {
                infocell.Phrase = new Phrase("Your Registered Mobile Number: " + PhoneNumber, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }

            if (!(iTax_No.Equals("") || iTax_No.Equals(null) || iTax_No.Equals(" ")))
            {
                infocell.Phrase = new Phrase("NTN No.: " + iTax_No, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }


            if (!(conNo.Equals("") || conNo.Equals(null) || conNo.Equals(" ")))
            {
                infocell.Phrase = new Phrase("Contract No. " + conNo, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }

            if (!(Bill_Serial_No.Equals("") || Bill_Serial_No.Equals(null) || Bill_Serial_No.Equals(" ")))
            {
                infocell.Phrase = new Phrase("Dispatch ID: " + Bill_Serial_No, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }


            if (!(ScheduleNo.Equals("") || ScheduleNo.Equals(null) || ScheduleNo.Equals(" ")))
            {
                infocell.Phrase = new Phrase("Schedule No. " + ScheduleNo, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }

            if (!(ConsumerNoc.Equals("") || ConsumerNoc.Equals(null) || ConsumerNoc.Equals(" ")))
            {
                infocell.Phrase = new Phrase("Consumer No.:  " + ConsumerNoc, new iTextSharp.text.Font(customfontNormal, 8f, 0, new BaseColor(84, 84, 84)));
                maininfotable.AddCell(infocell);
            }

            //maininfotable.WriteSelectedRows(0, -1, 17f, 750f, cb);
            maininfotable.WriteSelectedRows(0, -1, 17f, 706f, cb);


            //set account number
            var AccountNumber = pdfvmm.Account_Number;
            cb.BeginText();
            cb.SetTextMatrix(312, 676f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontBold, 14);
            //cb.ShowText("Account Number \n" + AccountNumber); //ISK_D1
            cb.ShowText(AccountNumber); 
            cb.EndText();






            //ibc info
            var IBCName = pdfvmm.IBC_Name;
            //var IBCAddress = textInfo.ToTitleCase(TruncateAtWord(pdfvmm.IBC_Address, 100).ToLower());
            var IBCAddress = TruncateAtWord(pdfvmm.IBC_Address, 100);
            cb.BeginText();
            cb.SetTextMatrix(313, 787f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontBold, 9);
            cb.ShowText(IBCName);
            cb.EndText();

            ct.SetSimpleColumn(new Phrase(new Chunk(IBCAddress, new iTextSharp.text.Font(customfontNormal, 6f, 0, new BaseColor(84, 84, 84)))),
                     313f, 785f, 440, 13f, 7f, Element.ALIGN_LEFT | Element.ALIGN_TOP);

            ct.Canvas.Fill();
            ct.Go();


            //barcode QR Code
            Barcode128 barcode39 = new Barcode128();
            barcode39.Code = pdfvmm.Account_Number;
            barcode39.CodeType = Barcode.CODE128;
            barcode39.AltText = "";

            iTextSharp.text.Image code39Image = barcode39.CreateImageWithBarcode(cb, BaseColor.BLACK, null);
            code39Image.SetAbsolutePosition(432f, 659f);
            code39Image.ScaleAbsolute(90f, 40f);
            doc1.Add(code39Image);


            BarcodeQRCode qrcode1 = new BarcodeQRCode(pdfvmm.QR_Code, 90, 90, null);

            iTextSharp.text.Image qrcodeImage2 = qrcode1.GetImage();
            qrcodeImage2.BackgroundColor = null;
            qrcodeImage2.SetAbsolutePosition(445f, 767f);
            qrcodeImage2.ScaleAbsolute(35f, 35f);
            doc1.Add(qrcodeImage2);

            cb.BeginText();
            cb.SetTextMatrix(452f, 798f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 6f);
            cb.ShowText("Scan Me");
            cb.EndText();

        }
        public void addReconectionVoucherinfo(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {


            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            IList<RCVoucherModel> rcList = Parser.parseRCVoucher(pdfvmm.V_RC_Table);



            //set invoice number
            var InvoiceNumber = rcList[0].info;
            cb.BeginText();
            cb.SetTextMatrix(312, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(InvoiceNumber);
            cb.EndText();

            //set issue-date
            var IssueDate = rcList[1].info;
            cb.BeginText();
            cb.SetTextMatrix(431, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(IssueDate.ToString());
            cb.EndText();



            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 440f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1f, 2f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;

            // bold cell
            PdfPCell boldcell = new PdfPCell(new Phrase(""));
            //  boldcell.Border = 0;
            boldcell.PaddingTop = 3;
            boldcell.PaddingBottom = 3;
            boldcell.BorderColor = new BaseColor(86, 86, 86);
            boldcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            boldcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //BUILDING HEADER
            PdfPCell headingcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            headingcell.UseVariableBorders = true;
            headingcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingcell.BorderColor = new BaseColor(86, 86, 86);

            headingcell.PaddingTop = 3;
            headingcell.PaddingBottom = 6;
            headingcell.PaddingLeft = 0;
            headingcell.PaddingRight = 0;


            headingcell.BackgroundColor = new BaseColor(204, 204, 204);
            headingcell.Phrase = new Phrase("Dues", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase("Slab", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase("RC Charge", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);


            boldcell.Phrase = new Phrase("Rs. " + rcList[2].info, new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(boldcell);
            boldcell.Phrase = new Phrase(rcList[3].info, new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(boldcell);

            boldcell.Phrase = new Phrase("Rs. " + rcList[4].info, new iTextSharp.text.Font(customfontBold, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(boldcell);

            table.WriteSelectedRows(0, -1, 72f, 327, cb);

        }

        public void addMeterChargesVoucherinfo(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {


            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            MCVoucherTable1Model mcvtable1 = Parser.parseMCVoucherTable1(pdfvmm.V_mc_Table1);
            MCVoucherTable2Model mcvtable2 = Parser.parseMCVoucherTable2(pdfvmm.V_mc_Table2);
            IList<MCVoucherTable3Model> mcvtable3list = Parser.parseMCVoucherTable3(pdfvmm.V_mc_Table3);



            //set invoice number
            var InvoiceNumber = mcvtable1.InvoiceNo;
            cb.BeginText();
            cb.SetTextMatrix(312, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(InvoiceNumber);
            cb.EndText();

            //set issue-date
            var IssueDate = mcvtable1.IssueDate;
            cb.BeginText();
            cb.SetTextMatrix(431, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(IssueDate.ToString());
            cb.EndText();


            //MCTable2
            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 410f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1f, 1f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;
            //table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            // bold cell
            PdfPCell simplecell = new PdfPCell(new Phrase(""));
            simplecell.PaddingTop = 2;
            simplecell.PaddingBottom = 6;
            simplecell.PaddingRight = 6;
            //simplecell.BorderColor = new BaseColor(84, 84, 84);
            simplecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            simplecell.VerticalAlignment = Element.ALIGN_MIDDLE;
            simplecell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //BUILDING HEADER
            PdfPCell headingcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            headingcell.UseVariableBorders = true;
            headingcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //headingcell.BorderColor = BaseColor.DARK_GRAY;
            headingcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            headingcell.PaddingTop = 3;
            headingcell.PaddingBottom = 6;
            headingcell.PaddingLeft = 0;
            headingcell.PaddingRight = 0;

            
            //headingcell.BackgroundColor = new BaseColor(204, 204, 204);
            
            //headingcell.Phrase = new Phrase("Replacement Month", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase(" \n \n ", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase(" \n \n ", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);

            //ISK_D1
            //simplecell.Phrase = new Phrase(mcvtable2.ReplacementMonth, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);
            simplecell.Phrase = new Phrase(mcvtable2.MeterType, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);

            simplecell.Phrase = new Phrase("Rs. " + mcvtable2.MeterCost, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);

            table.WriteSelectedRows(0, -1, 92f, 485, cb);



            // MC Table 3
            PdfPTable table3 = new PdfPTable(2);
            table3.TotalWidth = 410f;
            table3.LockedWidth = true;
            widths = new float[] { 1f, 1f };
            table3.SetWidths(widths);
            //table3.HorizontalAlignment = 0;

            // bold cell
            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.PaddingTop = 3;
            cell.PaddingBottom = 6;
            cell.PaddingRight = 7f;
            cell.BorderColor = new BaseColor(84, 84, 84);
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //Bar cell
            PdfPCell barcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            barcell.UseVariableBorders = true;
            barcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            barcell.BorderColor = new BaseColor(84, 84, 84);

            barcell.PaddingTop = 5f;
            barcell.PaddingBottom = 6f;
            barcell.PaddingRight = 7f;
            barcell.BorderWidthLeft = 0f;
            barcell.BorderWidthRight = 0f;
            barcell.BorderWidthTop = 1.5f;
            barcell.BorderWidthBottom = 1.5f;


            foreach (MCVoucherTable3Model item in mcvtable3list)
            {
                if (item.Formate.Equals("N"))
                {
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    cell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(cell);
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    cell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(cell);
                }
                else if (item.Formate.Equals("B"))
                {
                    barcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    barcell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(barcell);
                    barcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    barcell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(barcell);
                }
            }

            var spacer = table.CalculateHeights() + table.GetRowHeight(0) / 2;

            //static height 435
            table3.WriteSelectedRows(0, -1, 92f, 485 - (spacer), cb);

        }

        public void addIRBVoucherinfo(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            IRBVoucherTable1Model irbvtable1 = Parser.parseIRBVoucherTable1(pdfvmm.V_irb_Table1);
            IRBVoucherTable2Model irbvtable2 = Parser.parseIRBVoucherTable2(pdfvmm.V_irb_Table2);
            IList<IRBVoucherTable3Model> irbtable3list = Parser.parseIRBVoucherTable3(pdfvmm.V_irb_Table3);



            //set invoice number
            var InvoiceNumber = irbvtable1.InvoiceNo;
            cb.BeginText();
            cb.SetTextMatrix(312, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(InvoiceNumber);
            cb.EndText();

            //set issue-date
            var IssueDate = irbvtable1.IssueDate;
            cb.BeginText();
            cb.SetTextMatrix(431, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(IssueDate.ToString());
            cb.EndText();


            //Load 
            var Load = pdfvmm.V_irb_load;
            cb.BeginText();
            cb.SetTextMatrix(23f, 498f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(Load + "KV");
            cb.EndText();

            float textWidth = new Chunk(Load + "KV", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84))).GetWidthPoint();

            cb.BeginText();
            cb.SetTextMatrix(575f - textWidth, 498f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(Load + "KV");
            cb.EndText();


            //IRBtable1
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 430f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1f, 1f, 1.3f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;

            // bold cell
            PdfPCell simplecell = new PdfPCell(new Phrase(""));
            simplecell.PaddingTop = 3;
            simplecell.PaddingBottom = 6;
            simplecell.BorderColor = new BaseColor(84, 84, 84);
            simplecell.VerticalAlignment = Element.ALIGN_MIDDLE;
            simplecell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //BUILDING HEADER
            PdfPCell headingcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            headingcell.UseVariableBorders = true;
            headingcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingcell.BorderColor = BaseColor.DARK_GRAY;

            headingcell.PaddingTop = 3;
            headingcell.PaddingBottom = 6;
            headingcell.PaddingLeft = 0;
            headingcell.PaddingRight = 0;

            headingcell.BackgroundColor = new BaseColor(204, 204, 204);
            headingcell.Phrase = new Phrase("Bill Period", new iTextSharp.text.Font(customfontNormal, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase("Total Unit Assessed", new iTextSharp.text.Font(customfontNormal, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase("Units Previously Charged", new iTextSharp.text.Font(customfontNormal, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase("Units Charged", new iTextSharp.text.Font(customfontNormal, 11f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);



            simplecell.Phrase = new Phrase(irbvtable2.BillPeriod, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);
            simplecell.Phrase = new Phrase(irbvtable2.TotalUnit, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);
            simplecell.Phrase = new Phrase(irbvtable2.UnitPreviouslyCharged, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);
            simplecell.Phrase = new Phrase(irbvtable2.UnitCharged, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);

            table.WriteSelectedRows(0, -1, 82f, 470, cb);

            //ibrtable2

            PdfPTable table3 = new PdfPTable(2);
            table3.TotalWidth = 300f;
            table3.LockedWidth = true;
            widths = new float[] { 1f, 1f };
            table3.SetWidths(widths);
            //table3.HorizontalAlignment = 0;

            // bold cell
            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.PaddingTop = 1;
            cell.PaddingBottom = 1;
            //cell.PaddingRight = 7f;
            cell.BorderColor = new BaseColor(84, 84, 84);
            cell.BorderWidth = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //Bar cell
            PdfPCell barcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            barcell.UseVariableBorders = true;
            barcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            barcell.BorderColor = new BaseColor(84, 84, 84);

            barcell.PaddingTop = 10f;
            barcell.PaddingBottom = 15f;
            //barcell.PaddingRight = 7f;
            barcell.BorderWidthLeft = 0f;
            barcell.BorderWidthRight = 0f;
            barcell.BorderWidthTop = 0f;
            barcell.BorderWidthBottom = 0f;


            barcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
            barcell.Phrase = new Phrase("Bill Summary", new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table3.AddCell(barcell);
            barcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
            barcell.Phrase = new Phrase("Rs.", new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table3.AddCell(barcell);

            foreach (IRBVoucherTable3Model item in irbtable3list)
            {
                if (item.Formate.Equals("N"))
                {
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    cell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(cell);
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    cell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(cell);
                }
                else if (item.Formate.Equals("B"))
                {
                    barcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    barcell.Phrase = new Phrase(item.Label, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(barcell);
                    barcell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    barcell.Phrase = new Phrase(item.Amount, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                    table3.AddCell(barcell);
                }
            }

            var spacer = table.CalculateHeights() + table.GetRowHeight(0);

            //static height 435
            table3.WriteSelectedRows(0, -1, 21f, 485 - (spacer), cb);

        }

        public void addSDVoucherinfo(Document doc1, PdfWriter writer, PDFViewModel pdfvmm)
        {
            string fontpath = this.fontpath1;
            BaseFont customfontBold = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed-Bold.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Boldfont = new iTextSharp.text.Font(customfontBold, 12);

            BaseFont customfontNormal = BaseFont.CreateFont(fontpath + "/fonts/Helvetica-Condensed.otf", BaseFont.CP1252, BaseFont.EMBEDDED);
            iTextSharp.text.Font Mediumfont = new iTextSharp.text.Font(customfontNormal, 12);

            PdfContentByte cb = writer.DirectContent;
            ColumnText ct = new ColumnText(cb);
            IList<SDVoucherModel> sdinfo = Parser.parseSDVoucher(pdfvmm.V_SD_Table);

            //set invoice number
            var InvoiceNumber = sdinfo[0].info;
            cb.BeginText();
            cb.SetTextMatrix(312, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(InvoiceNumber);
            cb.EndText();

            //set issue-date
            var IssueDate = sdinfo[1].info;
            cb.BeginText();
            cb.SetTextMatrix(431, 638f);
            cb.SetColorFill(new BaseColor(84, 84, 84));
            cb.SetFontAndSize(customfontNormal, 12);
            cb.ShowText(IssueDate.ToString());
            cb.EndText();

            //SDtable1
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 553f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1f, 1f, 1f, 1f, 1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;

            // bold cell
            PdfPCell simplecell = new PdfPCell(new Phrase(""));
            simplecell.PaddingTop = 3;
            simplecell.PaddingBottom = 6;
            simplecell.BorderColor = new BaseColor(84, 84, 84);
            simplecell.VerticalAlignment = Element.ALIGN_MIDDLE;
            simplecell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            //BUILDING HEADER
            PdfPCell headingcell = new PdfPCell(new Phrase("")) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
            headingcell.UseVariableBorders = true;
            headingcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headingcell.BorderColor = BaseColor.DARK_GRAY;

            headingcell.PaddingTop = 3;
            headingcell.PaddingBottom = 6;
            headingcell.PaddingLeft = 0;
            headingcell.PaddingRight = 0;
            headingcell.BorderWidthRight = 0;

            headingcell.BackgroundColor = new BaseColor(204, 204, 204);
            headingcell.Phrase = new Phrase("Load (KW)", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            headingcell.Phrase = new Phrase("Charges/KW", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);
            //headingcell.Phrase = new Phrase("SD Charges", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            headingcell.Phrase = new Phrase(" ", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));

            table.AddCell(headingcell);
            //headingcell.Phrase = new Phrase("SD Already Paid", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            headingcell.Phrase = new Phrase(" ", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));

            table.AddCell(headingcell);
            headingcell.BorderWidthRight = 0.5f;
            headingcell.Phrase = new Phrase("SD Payable", new iTextSharp.text.Font(customfontNormal, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(headingcell);

            //Dealing for with and without Security Deposit
            simplecell.Phrase = new Phrase(sdinfo[2].info, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);
            simplecell.Phrase = new Phrase(sdinfo[3].info, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            table.AddCell(simplecell);
            //simplecell.Phrase = new Phrase(sdinfo[4].info.Trim(), new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
            simplecell.Phrase = new Phrase("", new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));

            table.AddCell(simplecell);
            if (sdinfo.Count > 5)
            {
                //simplecell.Phrase = new Phrase(sdinfo[5].info, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                simplecell.Phrase = new Phrase(" ", new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                table.AddCell(simplecell);
            }
            else
            {
                simplecell.Phrase = new Phrase(" ", new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                table.AddCell(simplecell);
            }
            if (sdinfo.Count > 6)
            {
                simplecell.Phrase = new Phrase(sdinfo[6].info, new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));

                table.AddCell(simplecell);
            }
            else
            {
                simplecell.Phrase = new Phrase("", new iTextSharp.text.Font(customfontBold, 12f, 0, new BaseColor(84, 84, 84)));
                table.AddCell(simplecell);
            }

            table.WriteSelectedRows(0, -1, 22f, 487, cb);


        }

        public string datesuffex(int Duedate)
        {
            switch (Duedate)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public string TruncateAtWord(string input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }


        public PDFViewModel getFilledObject(string account_number, string BillingMonth)
        {
            PDFViewModel pdfvm = new PDFViewModel();

            //using (SqlConnection conn = new SqlConnection())
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ConnectionString))
            {
                //conn.ConnectionString = "Data Source = NB129111622; Initial Catalog = DuplicateBill; Integrated Security = True";

                // Adding if condition to check if latest bill should be returned or not. X denotes latest bill
                if (BillingMonth == "X") { 
                conn.Open();

                    //SqlCommand command = new SqlCommand("SELECT * FROM DUP_BILL_NEW WHERE Account_Number = @account_number AND Billing_Month = @billing_month", conn);
                    //SqlCommand command = new SqlCommand("SELECT [blcd],[Bill_Id],LEFT(Billing_Month,4)+'20'+RIGHT(Billing_Month,2) as [Billing_Month],[Delivered_Date],[Account_Number],[Consumer_Number],[Contract_no],[File_Type],[Specific_Ind],[Bill_Type],[Region],[Clustor],[IBC_Name],[IBC_Address],[ÌBC_PH],[Tariff],[Cust_Class],[Bill_Charge_Mode],[Security_Deposit],[Issue_Date],[Due_Date],[Units_billed],[P_Units_Billed],[Net_Amount],[Net_Amount_Prt],[Rnd_net_Amt],[LPS_Amt],[Rnd_Gross_Amt],[Tenant_Name],[Owner_Name],[Customer_Name],[Customer_Address],[Forwarding_Address],[Meter_Number],[Meter_Reading_Date],[Sanc_Load_kw],[No_Of_Month],[Connected_Load_kw],[Cat_id],[Employ_No],[Stax_no],[ITax_No],[NIC_Number],[schd_no],[Customer_Cell_Nmbr],[Bill_Serial_No],[Class_id],[methd_delvry],[Avg_Temp_Mnth_Cur] ,[Avg_Temp_Mnth_Prv],[Avg_Temp_Year_Cur],[Per_prv_Month],[Per_Prv_Year],[Unit_Billed_1]      ,[Unit_Month_13]      ,[Billed_upto]      ,[Units_Adjusted]      ,[Message_Board]      ,[Barcode]      ,[QR_Code]      ,[QR_Code2]      ,[Billing_payment_table]      ,[Bnk_Gurr_Amt]      ,[Electricity_usage_table]      ,[Electricity_charges_table]      ,[Billing_statment_table]      ,[Customer_Info_Table_A]      ,[Customer_Info_Table_B]      ,[ChartData_table]      ,[V_SD_Flag]      ,[V_SD_Table]      ,[V_RC_Flag]      ,[V_RC_Table]      ,[File_Loading_Date_Time]      ,[V_irb_Flag]      ,[V_irb_Table1]      ,[V_irb_load]      ,[V_irb_Table2]      ,[V_mc_Flag]      ,[V_mc_Table1]      ,[V_mc_Table2]      ,[V_irb_Table3]      ,[V_mc_Table3]      ,[Total_Units_Billed] FROM [KESCDUPBILL].[dbo].[DUP_BILL_NEW] WHERE Account_Number = @account_number and Billing_Month in (select top 1(Billing_Month) from DUP_BILL_NEW where Account_Number = @account_number order by convert(date,Meter_Reading_Date) desc)", conn);
                    SqlCommand command = new SqlCommand("SELECT [blcd],[Bill_Id],LEFT(Billing_Month,4)+'20'+RIGHT(Billing_Month,2) as [Billing_Month],[Delivered_Date],[Account_Number],[Consumer_Number],[Contract_no],[File_Type],[Specific_Ind], [Bill_Type],[Region],[Clustor],[IBC_Name],[IBC_Address],[ÌBC_PH],[Tariff],[Cust_Class],[Bill_Charge_Mode], [Security_Deposit],[Issue_Date],[Due_Date],[Units_billed],[P_Units_Billed],[Net_Amount],[Net_Amount_Prt],[Rnd_net_Amt],[LPS_Amt],[Rnd_Gross_Amt],[Tenant_Name],[Owner_Name],[Customer_Name],[Customer_Address],[Forwarding_Address],[Meter_Number],[Meter_Reading_Date],[Sanc_Load_kw],[No_Of_Month],[Connected_Load_kw],[Cat_id],[Employ_No],[Stax_no],[ITax_No],[NIC_Number],[schd_no],[Customer_Cell_Nmbr],[Bill_Serial_No],[Class_id],[methd_delvry],[Avg_Temp_Mnth_Cur],[Avg_Temp_Mnth_Prv],[Avg_Temp_Year_Cur],[Per_prv_Month],[Per_Prv_Year],[Unit_Billed_1],[Unit_Month_13]    ,[Billed_upto],[Units_Adjusted],[Message_Board],[Barcode],[QR_Code],[QR_Code2],[Billing_payment_table],[Bnk_Gurr_Amt],[Electricity_usage_table],[Electricity_charges_table],[Billing_statment_table],[Customer_Info_Table_A],[Customer_Info_Table_B],[ChartData_table],[V_SD_Flag],[V_SD_Table],[V_RC_Flag],[V_RC_Table],[File_Loading_Date_Time],[V_irb_Flag],[V_irb_Table1],[V_irb_load],[V_irb_Table2],[V_mc_Flag],[V_mc_Table1],[V_mc_Table2],[V_irb_Table3],[V_mc_Table3],[Total_Units_Billed] FROM DUP_BILL_NEW WHERE Account_Number = '0400005472782' and Billing_Month in (select top 1(Billing_Month) from[KESCDUPBILL].[dbo].[DUP_BILL_NEW] where Account_Number = '" + account_number + "' order by convert(date, Meter_Reading_Date) desc)", conn);
                     //Adding command timeout
                     command.CommandTimeout = 600;
                 command.Parameters.Add(new SqlParameter("@account_number", account_number));

                 

                    using (SqlDataReader reader = command.ExecuteReader())
                {
                    //DataReaderDebugUtilities.DataReaderExtensions.ToStringAsDataSet(reader);
                    while (reader.Read())
                    {
                        Type type = typeof(PDFViewModel);
                        foreach (var col in type.GetProperties())
                        {
                            PropertyInfo info = pdfvm.GetType().GetProperty(col.Name);
                            if (reader[col.Name] == System.DBNull.Value)
                            {
                                info.SetValue(pdfvm, "", null);
                            }
                            else
                            {
                                info.SetValue(pdfvm, reader[col.Name], null);
                            }
                            //System.Diagnostics.Debug.WriteLine(pdfvm.GetType().GetProperty(col.Name).GetValue(pdfvm));
                        }
                    }
                }
            }
                else
                { 
                conn.Open();

                //SqlCommand command = new SqlCommand("SELECT * FROM DUP_BILL_NEW WHERE Account_Number = @account_number AND Billing_Month = @billing_month", conn);
                SqlCommand command = new SqlCommand("SELECT [blcd],[Bill_Id],LEFT(Billing_Month,4)+'20'+RIGHT(Billing_Month,2) as [Billing_Month],[Delivered_Date],[Account_Number],[Consumer_Number],[Contract_no],[File_Type],[Specific_Ind],[Bill_Type],[Region],[Clustor],[IBC_Name],[IBC_Address],[ÌBC_PH],[Tariff],[Cust_Class],[Bill_Charge_Mode],[Security_Deposit],[Issue_Date],[Due_Date],[Units_billed],[P_Units_Billed],[Net_Amount],[Net_Amount_Prt],[Rnd_net_Amt],[LPS_Amt],[Rnd_Gross_Amt],[Tenant_Name],[Owner_Name],[Customer_Name],[Customer_Address],[Forwarding_Address],[Meter_Number],[Meter_Reading_Date],[Sanc_Load_kw],[No_Of_Month],[Connected_Load_kw],[Cat_id],[Employ_No],[Stax_no],[ITax_No],[NIC_Number],[schd_no],[Customer_Cell_Nmbr],[Bill_Serial_No],[Class_id],[methd_delvry],[Avg_Temp_Mnth_Cur] ,[Avg_Temp_Mnth_Prv],[Avg_Temp_Year_Cur],[Per_prv_Month],[Per_Prv_Year],[Unit_Billed_1]      ,[Unit_Month_13]      ,[Billed_upto]      ,[Units_Adjusted]      ,[Message_Board]      ,[Barcode]      ,[QR_Code]      ,[QR_Code2]      ,[Billing_payment_table]      ,[Bnk_Gurr_Amt]      ,[Electricity_usage_table]      ,[Electricity_charges_table]      ,[Billing_statment_table]      ,[Customer_Info_Table_A]      ,[Customer_Info_Table_B]      ,[ChartData_table]      ,[V_SD_Flag]      ,[V_SD_Table]      ,[V_RC_Flag]      ,[V_RC_Table]      ,[File_Loading_Date_Time]      ,[V_irb_Flag]      ,[V_irb_Table1]      ,[V_irb_load]      ,[V_irb_Table2]      ,[V_mc_Flag]      ,[V_mc_Table1]      ,[V_mc_Table2]      ,[V_irb_Table3]      ,[V_mc_Table3]      ,[Total_Units_Billed] FROM DUP_BILL_NEW WHERE Account_Number = '" +account_number+"' AND LEFT(Billing_Month, 4) + '20' + RIGHT(Billing_Month, 2) ='"+ BillingMonth + "'", conn);
                    command.CommandTimeout = 600;
                    command.Parameters.Add(new SqlParameter("@account_number", account_number));

                command.Parameters.Add(new SqlParameter("@billing_month", BillingMonth));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //DataReaderDebugUtilities.DataReaderExtensions.ToStringAsDataSet(reader);
                    while (reader.Read())
                    {
                        Type type = typeof(PDFViewModel);
                        foreach (var col in type.GetProperties())
                        {
                            PropertyInfo info = pdfvm.GetType().GetProperty(col.Name);
                            if (reader[col.Name] == System.DBNull.Value)
                            {
                                info.SetValue(pdfvm, "", null);
                            }
                            else
                            {
                                info.SetValue(pdfvm, reader[col.Name], null);
                            }
                            //System.Diagnostics.Debug.WriteLine(pdfvm.GetType().GetProperty(col.Name).GetValue(pdfvm));
                        }
                    }
                }
            }

        }
            return pdfvm;

        }

        public PDFViewModel getFilledObject(string account_number, string BillingMonth, string Billid)
        {
            PDFViewModel pdfvm = new PDFViewModel();

            //using (SqlConnection conn = new SqlConnection())
            using (SqlConnection conn = new SqlConnection(en.DecryptText(ConfigurationManager.ConnectionStrings["KESCDUPBILLConnectionString"].ConnectionString)))
            {
                //conn.ConnectionString = "Data Source = NB129111622; Initial Catalog = DuplicateBill; Integrated Security = True";

                conn.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM DUP_BILL_NEW WHERE Account_Number = @account_number AND Billing_Month = @billing_month AND Bill_Id = @bill_id", conn);
                command.Parameters.Add(new SqlParameter("@account_number", account_number));
                command.Parameters.Add(new SqlParameter("@billing_month", BillingMonth));
                command.Parameters.Add(new SqlParameter("@bill_id", Billid));
                

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Type type = typeof(PDFViewModel);
                        foreach (var col in type.GetProperties())
                        {
                            PropertyInfo info = pdfvm.GetType().GetProperty(col.Name);
                            if (reader[col.Name] == System.DBNull.Value)
                            {
                                info.SetValue(pdfvm, "", null);
                            }
                            else
                            {
                                info.SetValue(pdfvm, reader[col.Name], null);
                            }
                            //System.Diagnostics.Debug.WriteLine(pdfvm.GetType().GetProperty(col.Name).GetValue(pdfvm));
                        }
                    }
                }
            }
            return pdfvm;

        }


    }

}
using com.newtronics.Common;
using PackingTracker.Common;
using PackingTracker.Entity;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZXing;

namespace com.newtronics.UI
{
    public partial class PrintFrm : Form
    {
        private int TopMargin
        {
            get;
            set;
        }
        private int LeftMargin
        {
            get;
            set;
        }
        private int LineSpace
        {
            get;
            set;
        }

        private int WidthInPixel
        {
            get;
            set;
        }

        private int HeightInPixel
        {
            get;
            set;
        }

        private Bitmap ImageToConvert
        {
            get;
            set;
        }

        string BoxSN
        {
            get;
            set;
        }

        OrderDetail OrderDetail
        {
            get;
            set;
        }

        BoxDetail BoxDetail
        {
            get;
            set;
        }

        private RestClient client;

        private readonly SynchronizationContext synchronizationContext;

        public PrintFrm(string boxSN, OrderDetail orderDetail)
        {
            BoxSN = boxSN;
            OrderDetail = orderDetail;
            synchronizationContext = SynchronizationContext.Current;

            InitializeComponent();

            InitializeFonts();

            InitializePrintPreview();

            InitializeCustomItems();

            InitializeRest();
        }

        void InitializeRest()
        {
            string host = Constants.Host;
            client = new RestClient(host);
            //5 seconds
            client.ReadWriteTimeout = 5000;
        }

        private void InitializePrintPreview()
        {
            WidthInPixel = 799;// 100 * 200 * 10 / 254;
            HeightInPixel = 559;// 70 * 200 * 10 / 254;
            ImageToConvert = new Bitmap(WidthInPixel, HeightInPixel);
            PreviewLabel.Image = ImageToConvert;
        }

        private void InitializeCustomItems()
        {
            Custom1Label.Text = "自定义1";
            Custom2Label.Text = "自定义2";
            Custom3Label.Text = "自定义3";
            Custom4Label.Text = "自定义4";
            Custom5Label.Text = "自定义5";


        }
        void InitializeFonts()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            string def = Properties.Settings.Default["HeadFontName"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "楷体";
            }

            foreach (FontFamily ff in fonts.Families)
            {
                HeadFontList.Items.Add(ff.Name);
                TitleFontList.Items.Add(ff.Name);
                FieldFontList.Items.Add(ff.Name);

                if (ff.Name == def)
                {
                    HeadFontList.SelectedItem = def;
                    TitleFontList.SelectedItem = def;
                    FieldFontList.SelectedItem = def;
                }
            }

            def = Properties.Settings.Default["HeadFontSize"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "24";
            }
            HeadFontSizeList.SelectedItem = def;

            def = Properties.Settings.Default["TitleFontSize"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "20";
            }
            TitleFontSizeList.SelectedItem = def;

            def = Properties.Settings.Default["FieldFontSize"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "20";
            }
            FieldFontSizeList.SelectedItem = def;

            def = Properties.Settings.Default["HeadFontStyle"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "粗体";
            }
            HeadFontTypeList.SelectedItem = def;

            def = Properties.Settings.Default["TitleFontStyle"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "粗体";
            }
            TitleFontTypeList.SelectedItem = def;

            def = Properties.Settings.Default["FieldFontStyle"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "粗体";
            }
            FieldFontTypeList.SelectedItem = def;

            def = Properties.Settings.Default["LineSpace"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "0";
            }
            LineSpaceTB.Text = def;

            def = Properties.Settings.Default["LeftMargin"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "10";
            }
            LeftMarginTB.Text = def;

            def = Properties.Settings.Default["TopMargin"] as string;
            if (String.IsNullOrEmpty(def))
            {
                def = "15";
            }

            TopMarginTB.Text = def;

            TopMargin = Int32.Parse(TopMarginTB.Text);
            LeftMargin = Int32.Parse(LeftMarginTB.Text);
            LineSpace = Int32.Parse(LineSpaceTB.Text);
        }

        private void PrintFrm_Load(object sender, EventArgs e)
        {
            SearchBox();
        }

        void SearchBox()
        {
            var request = new RestRequest("dlicense/v2/manu/boxing/queryboxinfo", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("reqUserId", SharedApp.Instance.Login.Userid);
            request.AddHeader("reqUserSession", SharedApp.Instance.Login.Loginsession);
            request.AddHeader("grouptype", SharedApp.Instance.AccountDetail.Grouptype);
            request.AddHeader("OemfactoryId", SharedApp.Instance.AccountDetail.OemfactoryId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"boxsn\":");
            sb.Append("\"").Append(this.BoxSN).Append("\"");
            sb.Append("}");

            string body = sb.ToString();

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            var asyncHandle = client.ExecuteAsync<BoxQuery>(request, response =>
            {
                BoxQuery boxQuery = response.Data;
                if (response.IsSuccessful && boxQuery != null && boxQuery.retdata != null)
                {
                    BoxDetail = boxQuery.retdata.BoxInfo;
                    
                    synchronizationContext.Post(new SendOrPostCallback(o =>
                    {
                        ShowBoxDetail();
                        ShowOrderDetail();
                        ShowLabel();
                    }), null);
                }
                else
                {
                    ShowError("无法取得箱子详细信息:" + response.ErrorMessage);
                }
            });
        }

        void ShowBoxDetail()
        {
            BoxTypeTextBox.Text = BoxDetail.BoxType;
            BoxSNTextBox.Text = BoxDetail.BoxSN;
            BoxCapacityTextBox.Text = string.Format("{0}", BoxDetail.Capacity);
            BoxRealCountTextBox.Text = string.Format("{0}", BoxDetail.RealCount);
            BoxCreateTimeTextBox.Text = BoxDetail.CreateTime;
            BoxStatusTextBox.Text = BoxDetail.Status;
        }

        void ShowOrderDetail()
        {
            OrderIdTB.Text = OrderDetail.AgreementId;
            WorkformTB.Text = OrderDetail.Workform;
            //古北产品编码
            GubeiNoTB.Text = OrderDetail.MaterialCode;
            //客户产品编码
            KehuNoTB.Text = OrderDetail.KehuCode;
            //产品型号
            ProdModelTB.Text = OrderDetail.ProductModel;
            //产品描述
            ProductDescTB.Text = OrderDetail.ProductDesc;
            //装箱数量
            //供应商
            SupplierTB.Text = OrderDetail.Supplier;
            //批次
            BatchTB.Text = OrderDetail.BatchNo;
            //Version
            VersionTB.Text = OrderDetail.ProductVerTag;
            //固件
            FirmwareTB.Text = OrderDetail.Firmware;
        }

        void ShowLabel()
        {
            #region 字体设置
            if (!String.IsNullOrEmpty(LineSpaceTB.Text))
            {
                LineSpace = Int32.Parse(LineSpaceTB.Text);
            }

            string headFontName = HeadFontList.SelectedItem as string;
            if (headFontName == null)
            {
                headFontName = "楷体";
            }

            string titleFontName = TitleFontList.SelectedItem as string;
            if (titleFontName == null)
            {
                titleFontName = "楷体";
            }

            string fieldFontName = FieldFontList.SelectedItem as string;
            if (fieldFontName == null)
            {
                fieldFontName = "楷体";
            }

            int headFontSize = 20;
            string tmp = HeadFontSizeList.SelectedItem as string;
            if (tmp != null)
            {
                headFontSize = Int32.Parse(tmp);
            }

            int titleFontSize = 16;
            tmp = TitleFontSizeList.SelectedItem as string;
            if (tmp != null)
            {
                titleFontSize = Int32.Parse(tmp);
            }

            int fieldFontSize = 16;
            tmp = FieldFontSizeList.SelectedItem as string;
            if (tmp != null)
            {
                fieldFontSize = Int32.Parse(tmp);
            }

            FontStyle headFs = FontStyle.Bold;
            string fontStyle = HeadFontTypeList.SelectedItem as string;
            if (fontStyle == "粗体")
            {
                headFs = FontStyle.Bold;
            }
            else if (fontStyle == "斜体")
            {
                headFs = FontStyle.Italic;
            }
            else if (fontStyle == "普通")
            {
                headFs = FontStyle.Regular;
            }

            FontStyle titleFs = FontStyle.Bold;
            fontStyle = TitleFontTypeList.SelectedItem as string;
            if (fontStyle == "粗体")
            {
                titleFs = FontStyle.Bold;
            }
            else if (fontStyle == "斜体")
            {
                titleFs = FontStyle.Italic;
            }
            else if (fontStyle == "普通")
            {
                titleFs = FontStyle.Regular;
            }

            FontStyle fieldFs = FontStyle.Bold;
            fontStyle = FieldFontTypeList.SelectedItem as string;
            if (fontStyle == "粗体")
            {
                fieldFs = FontStyle.Bold;
            }
            else if (fontStyle == "斜体")
            {
                fieldFs = FontStyle.Italic;
            }
            else if (fontStyle == "普通")
            {
                fieldFs = FontStyle.Regular;
            }
            if (!String.IsNullOrEmpty(TopMarginTB.Text))
            {
                TopMargin = Int32.Parse(TopMarginTB.Text);
            }
            if (!String.IsNullOrEmpty(LeftMarginTB.Text))
            {
                LeftMargin = Int32.Parse(LeftMarginTB.Text);
            }

            Font headFont = new Font(headFontName, headFontSize, headFs, GraphicsUnit.Point);
            Font titleFont = new Font(titleFontName, titleFontSize, titleFs, GraphicsUnit.Point);
            Font fieldFont = new Font(fieldFontName, fieldFontSize, fieldFs, GraphicsUnit.Point);

            StringFormat headFormat = new StringFormat();
            headFormat.Alignment = StringAlignment.Center;
            headFormat.LineAlignment = StringAlignment.Near;

            StringFormat titleFormat = new StringFormat();
            titleFormat.Alignment = StringAlignment.Near;
            titleFormat.LineAlignment = StringAlignment.Near;

            StringFormat fieldFormat = new StringFormat();
            fieldFormat.Alignment = StringAlignment.Center;
            fieldFormat.LineAlignment = StringAlignment.Near;

            SolidBrush textBrush = new SolidBrush(Color.Black);
            #endregion

            #region 取得标签项目
            List<string> items = GetPrintItems(OrderDetail.OrderId);
            if (items == null || items.Count == 0)
            {
                //MessageBox.Show("无打印项目", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PrintItemFrm printItemFrm = new PrintItemFrm();
                printItemFrm.OrderID = OrderDetail.OrderId;
                DialogResult result = printItemFrm.ShowDialog(this);
                if(result == DialogResult.OK)
                {
                    items = GetPrintItems(OrderDetail.OrderId);
                }
            }

            if (items == null || items.Count == 0)
            {
                MessageBox.Show("无打印项目", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            Graphics g = Graphics.FromImage(ImageToConvert);

            //background
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, WidthInPixel, HeightInPixel);

            int top = TopMargin;
            int left = LeftMargin;

            #region 打印标题
            string title = "产品装箱单";
            g.DrawString(title, headFont, textBrush, new RectangleF(left, top, WidthInPixel, HeightInPixel), headFormat);
            Size tmpSize = TextRenderer.MeasureText(g, title, headFont);
            top += tmpSize.Height;
            top += LineSpace;
            #endregion

            #region 打印项目
            foreach (string pr in items)
            {
                string[] tmpArr = pr.Split('\t');
                string name = tmpArr[0];
                string disp = tmpArr[1];
                string isBar = tmpArr[2];

                //取得打印文字长度
                string Text = "";
                if (name == "OrderCode")
                {
                    //订单编码
                    Text = OrderDetail.AgreementId;
                }
                else if (name == "WorkFlow")
                {
                    //工单
                    Text = OrderDetail.Workform;
                }
                else if (name == "Batch")
                {
                    //批次号
                    Text = OrderDetail.BatchNo;
                }
                else if (name == "GubeiNo")
                {
                    //古北产品编码
                    Text = OrderDetail.MaterialCode;
                }
                else if (Name == "CustomerNo")
                {
                    //客户产品编码
                    Text = OrderDetail.KehuCode;
                }
                else if (name == "ProdModel")
                {
                    //产品型号
                    Text = OrderDetail.ProductModel;
                }
                else if (name == "ProdDesc")
                {
                    //产品描述
                    Text = OrderDetail.ProductDesc;
                }
                else if (name == "Ver")
                {
                    //Ver
                    Text = OrderDetail.ProductVerTag;
                }
                else if (name == "RealCount")
                {
                    //装箱数量
                    Text = string.Format("{0} PCS", BoxDetail.RealCount);
                }
                else if (name == "BoxNo")
                {
                    //箱号
                    Text = BoxDetail.BoxSN;
                }
                else if (name == "Supplier")
                {
                    //供应商
                    Text = OrderDetail.Supplier;
                }
                else if (name == "QC")
                {
                    //QC
                    Text = "";
                }
                else if (name == "ManufactureDate")
                {
                    //制造日期
                    Text = DateTime.Now.ToString("yyyy.MM.dd");
                }
                else if (name == "Firmware")
                {
                    //固件
                    Text = OrderDetail.Firmware;
                }
                else if (name == "Custom1")
                {
                    //Custom
                    Text = OrderDetail.Custom1;
                }
                else if (name == "Custom2")
                {
                    //Custom
                    Text = OrderDetail.Custom2;
                }
                else if (name == "Custom3")
                {
                    //Custom
                    Text = OrderDetail.Custom3;
                }
                else if (name == "Custom4")
                {
                    //Custom
                    Text = OrderDetail.Custom4;
                }
                else if (name == "Custom5")
                {
                    //Custom
                    Text = OrderDetail.Custom5;
                }

                g.DrawString(disp, titleFont, textBrush, new RectangleF(left, top, WidthInPixel, HeightInPixel), titleFormat);
                tmpSize = TextRenderer.MeasureText(g, disp, titleFont);
                int titleWidth = tmpSize.Width;
                int titleHeight = tmpSize.Height;
                //可用空间
                int w = WidthInPixel - left * 2 - titleWidth * 2 - 2;
                if (String.IsNullOrEmpty(Text))
                {
                    top += tmpSize.Height;
                }
                else
                {
                    if ("1" == isBar)
                    {
                        var barcodeWriter = new BarcodeWriter();
                        barcodeWriter.Format = BarcodeFormat.CODE_128;
                        barcodeWriter.Options.Height = 80;
                        barcodeWriter.Options.Width = w;
                        Bitmap barcode = barcodeWriter.Write(Text);

                        g.DrawImage(barcode, left + tmpSize.Width, top);
                        top += barcodeWriter.Options.Height;
                    }
                    else
                    {
                        tmpSize = TextRenderer.MeasureText(g, Text, fieldFont);
                        if (tmpSize.Width <= w)
                        {
                            //居中
                            g.DrawString(Text, fieldFont, textBrush, new RectangleF(0, top, WidthInPixel, HeightInPixel), fieldFormat);
                            top += tmpSize.Height > titleHeight ? tmpSize.Height : titleHeight;
                        }
                        else
                        {
                            //打不下
                            SizeF tmpSizeF = g.MeasureString(Text, fieldFont, w + titleWidth);
                            g.DrawString(Text, fieldFont, textBrush, new RectangleF(left + titleWidth, top, w + titleWidth, HeightInPixel), fieldFormat);
                            int tmpInt = (int)Math.Ceiling(tmpSizeF.Height);
                            top += tmpInt > titleHeight ? tmpInt : titleHeight;
                        }

                    }
                }

                top += LineSpace;
            }
            #endregion

            PreviewLabel.Image = ImageToConvert;
        }

        List<string> GetPrintItems(string orderId)
        {
            List<string> items = new List<string>();
            using (var conn = DBHelper.Instance.Open())
            {
                if(conn != null)
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT  Name, DispSeq, DispText, BarCode from Print where Id=@Id and DispSeq > 0 order by DispSeq Asc";
                        command.Parameters.Add("@Id", DbType.String);
                        command.Parameters["@Id"].Value = orderId;

                        using (SQLiteDataReader dr = command.ExecuteReader())
                        {
                            while (dr != null && dr.Read())
                            {
                                string name = dr.GetString(0);
                                string dispText = dr.GetString(2);
                                string barCode = dr.GetString(3);

                                string tmp = string.Format("{0}\t{1}\t{2}", name, dispText == null ? "" : dispText, barCode == null ? "" : barCode);

                                items.Add(tmp);
                            }

                        }
                    }
                }                
            }
            return items;
        }

        void ShowError(string errMsg)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                string msg = o as string;
                if (msg == null)
                {
                    msg = "未知错误，请重试";
                }

                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }), errMsg);
        }

        private void PrintLabelButton_Click(object sender, EventArgs e)
        {
            LPPrint printer = null;
            try
            {
                string zplCmd = com.newtronics.Common.Convert.BitmapToZPLII(new Bitmap(799,559), 0, 0);

                StringBuilder sb = new StringBuilder();
                sb.Append("^XA^LH0,0~SD20^PR3,3,3^XZ");
                sb.Append("^XA");
                sb.Append("^PW799");
                sb.Append("^LL0559");
                sb.Append(zplCmd);
                sb.Append("^PQ1,0,1,Y^PS^XZ");

                printer = new LPPrint();

                bool o = printer.Open();

                if (o)
                {
                    bool ret = printer.Write(sb.ToString());
                    if (!ret)
                    {
                        MessageBox.Show("无法向打印机发生命令", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("无法连接打印机", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("打印错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (printer != null && printer.IsOpened)
                {
                    printer.Close();
                }
            }
        }
    }
}

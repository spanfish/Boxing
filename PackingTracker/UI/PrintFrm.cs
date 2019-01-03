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
        #region Local Variables
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
        #endregion

        public PrintFrm(string boxSN, OrderDetail orderDetail)
        {
            BoxSN = boxSN;
            OrderDetail = orderDetail;
#if DEBUG
            //OrderDetail = new OrderDetail();
            //OrderDetail.OrderId = "B-MK-18082201-2";
            //OrderDetail.AgreementId = "B-MK-18082201-2";
            //OrderDetail.ProductModel = "BL5015-HBSG011002";
            //OrderDetail.ProductName = "ProductName";
            //OrderDetail.ProductDesc = "Opple_Control_WiFi_MTK7698_3.3V_U. FL接口外置天线_stamp_PC BA_V1.1_new模块(欧普WIFI控制盒去谐波205100010281)";
            //OrderDetail.Workform = "20181105162108-221-1";
            //OrderDetail.FactoryName = "12345678901234560";
            //OrderDetail.MaterialCode = "12345678901234560";
            //OrderDetail.KehuCode = "12345678901234560";
            //OrderDetail.Supplier = "杭州古北电子科技有限公司";
            //OrderDetail.ProductVerTag = "12345678901234560";
            //OrderDetail.BatchNo = "12345678901234560";
            //OrderDetail.FactoryName = "12345678901234560";

            //BoxSN = "IBC20181208000013";

            //BoxDetail = new BoxDetail();
            //BoxDetail.Oemfactoryid = "12345678901234560";
            //BoxDetail.RealCount = 1000;
            //BoxDetail.OrderId = "12345678901234560";
            //BoxDetail.BoxSN = "IBC20181208000013";
            //BoxDetail.BoxType = "Inner";
            
#endif
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
#if DEBUG
            //ShowBoxDetail();
            //ShowOrderDetail();
            //ShowLabel();
#else
            
#endif
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

        /// <summary>
        /// 读取打印默认值，已订单为单位
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private string GetDef(string Id, string Name)
        {
            string def = "";
            using (var conn = DBHelper.Instance.Open())
            {
                if (conn != null)
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT DefValue FROM Setting WHERE Id = @Id AND Name=@Name";
                        command.Parameters.Add("@Id", DbType.String);
                        command.Parameters.Add("@Name", DbType.String);

                        command.Parameters["@Id"].Value = Id;
                        command.Parameters["@Name"].Value = Name;

                        string v = command.ExecuteScalar() as string;
                        if (v != null)
                        {
                            def = v;
                        }
                    }
                }
            }
            return def;
        }

        void ShowBoxDetail()
        {
            if (String.IsNullOrEmpty(OrderDetail.Workform))
            {
                OrderDetail.Workform = GetDef(OrderDetail.OrderId, "Workform");
            }

            if (String.IsNullOrEmpty(OrderDetail.MaterialCode))
            {
                OrderDetail.MaterialCode = GetDef(OrderDetail.OrderId, "GuBeiNo");
            }

            if (String.IsNullOrEmpty(OrderDetail.KehuCode))
            {
                OrderDetail.KehuCode = GetDef(OrderDetail.OrderId, "KehuNo");
            }

            if (String.IsNullOrEmpty(OrderDetail.ProductModel))
            {
                OrderDetail.ProductModel = GetDef(OrderDetail.OrderId, "ProdModel");
            }

            if (String.IsNullOrEmpty(OrderDetail.ProductDesc))
            {
                OrderDetail.ProductDesc = GetDef(OrderDetail.OrderId, "ProdDesc");
            }

            if (String.IsNullOrEmpty(OrderDetail.Supplier))
            {
                OrderDetail.Supplier = GetDef(OrderDetail.OrderId, "Supplier");
            }

            if (String.IsNullOrEmpty(OrderDetail.Supplier))
            {
                OrderDetail.Supplier = "杭州古北电子科技有限公司";
            }

            if (String.IsNullOrEmpty(OrderDetail.BatchNo))
            {
                OrderDetail.BatchNo = GetDef(OrderDetail.OrderId, "BatchNo");
            }

            if (String.IsNullOrEmpty(OrderDetail.ProductVerTag))
            {
                OrderDetail.ProductVerTag = GetDef(OrderDetail.OrderId, "Version");
            }

            if (String.IsNullOrEmpty(OrderDetail.Firmware))
            {
                OrderDetail.Firmware = GetDef(OrderDetail.OrderId, "Firmware");
            }
            OrderDetail.Custom1 = GetDef(OrderDetail.OrderId, "Custom1");
            OrderDetail.Custom2 = GetDef(OrderDetail.OrderId, "Custom2");
            OrderDetail.Custom3 = GetDef(OrderDetail.OrderId, "Custom3");
            OrderDetail.Custom4 = GetDef(OrderDetail.OrderId, "Custom4");
            OrderDetail.Custom5 = GetDef(OrderDetail.OrderId, "Custom5");

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

            Custom1TB.Text = OrderDetail.Custom1;
            Custom2TB.Text = OrderDetail.Custom2;
            Custom3TB.Text = OrderDetail.Custom3;
            Custom4TB.Text = OrderDetail.Custom4;
            Custom5TB.Text = OrderDetail.Custom5;
        }

        /// <summary>
        /// 显示标签
        /// </summary>
        void ShowLabel()
        {
            if(ImageToConvert == null)
            {
                return;
            }
#region 字体间距等设置
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

#region 打印标题-产品装箱单
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
                else if (name == "CustomerNo")
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
                else
                {
                    throw new NotImplementedException("");
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
                string zplCmd = Image2ZPL.Convert.BitmapToZPLII(ImageToConvert, 0, 0);

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
            catch(Exception ex)
            {
                MessageBox.Show("打印错误:" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (printer != null && printer.IsOpened)
                {
                    printer.Close();
                }
            }
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {
            ShowLabel();
        }

        void SaveDefValue(string name, string value)
        {
            using (var conn = DBHelper.Instance.Open())
            {
                if (conn != null)
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "UPDATE Setting SET DefValue = @DefValue WHERE Id = @Id AND Name=@Name";
                        command.Parameters.Add("@DefValue", DbType.String);
                        command.Parameters.Add("@ID", DbType.String);
                        command.Parameters.Add("@Name", DbType.String);

                        command.Parameters["@ID"].Value = OrderDetail.OrderId;
                        command.Parameters["@Name"].Value = name;
                        command.Parameters["@DefValue"].Value = value;
                        if (command.ExecuteNonQuery() == 0)
                        {
                            using (SQLiteCommand insCmd = conn.CreateCommand())
                            {
                                insCmd.CommandText = "INSERT INTO Setting(Id, Name, DefValue) VALUES(@Id, @Name, @DefValue)";

                                insCmd.Parameters.Add("@DefValue", DbType.String);
                                insCmd.Parameters.Add("@Name", DbType.String);
                                insCmd.Parameters.Add("@ID", DbType.String);

                                insCmd.Parameters["@ID"].Value = OrderDetail.OrderId;
                                insCmd.Parameters["@Name"].Value = name;
                                insCmd.Parameters["@DefValue"].Value = value;
                                insCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        private void WorkformTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if(!String.Equals(OrderDetail.Workform, tb.Text))
            {
                OrderDetail.Workform = tb.Text;
                SaveDefValue("Workform", tb.Text);
                ShowLabel();
            }            
        }

        private void GubeiNoTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.MaterialCode, tb.Text))
            {
                OrderDetail.MaterialCode = tb.Text;
                SaveDefValue("GuBeiNo", tb.Text);
                ShowLabel();
            }
        }

        private void KehuNoTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!String.Equals(OrderDetail.KehuCode, tb.Text))
            {
                OrderDetail.KehuCode = tb.Text;
                SaveDefValue("KehuNo", tb.Text);
                ShowLabel();
            }
        }

        private void ProdModelTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.ProductModel, tb.Text))
            {
                OrderDetail.ProductModel = tb.Text;
                SaveDefValue("ProdModel", tb.Text);
                ShowLabel();
            }
        }

        private void ProductDescTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.ProductDesc, tb.Text))
            {
                OrderDetail.ProductDesc = tb.Text;
                SaveDefValue("ProdDesc", tb.Text);
                ShowLabel();
            }
        }

        private void SupplierTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Supplier, tb.Text))
            {
                OrderDetail.Supplier = tb.Text;
                SaveDefValue("Supplier", tb.Text);
                ShowLabel();
            }
        }

        private void BatchTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.BatchNo, tb.Text))
            {
                OrderDetail.BatchNo = tb.Text;
                SaveDefValue("BatchNo", tb.Text);
                ShowLabel();
            }
        }

        private void VersionTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.ProductVerTag, tb.Text))
            {
                OrderDetail.ProductVerTag = tb.Text;
                SaveDefValue("Version", tb.Text);
                ShowLabel();
            }
        }

        private void FirmwareTB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Firmware, tb.Text))
            {
                OrderDetail.Firmware = tb.Text;
                SaveDefValue("Firmware", tb.Text);
                ShowLabel();
            }
        }

        private void Custom1TB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Custom1, tb.Text))
            {
                OrderDetail.Custom1 = tb.Text;
                SaveDefValue("Custom1", tb.Text);
                ShowLabel();
            }
        }

        private void Custom2TB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Custom2, tb.Text))
            {
                OrderDetail.Custom2 = tb.Text;
                SaveDefValue("Custom2", tb.Text);
                ShowLabel();
            }
        }

        private void Custom3TB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Custom3, tb.Text))
            {
                OrderDetail.Custom3 = tb.Text;
                SaveDefValue("Custom3", tb.Text);
                ShowLabel();
            }
        }

        private void Custom4TB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Custom4, tb.Text))
            {
                OrderDetail.Custom4 = tb.Text;
                SaveDefValue("Custom4", tb.Text);
                ShowLabel();
            }
        }

        private void Custom5TB_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.Equals(OrderDetail.Custom5, tb.Text))
            {
                OrderDetail.Custom5 = tb.Text;
                SaveDefValue("Custom5", tb.Text);
                ShowLabel();
            }
        }

        private void SelectPrintItemButton_Click(object sender, EventArgs e)
        {
            if (OrderDetail == null || String.IsNullOrEmpty(OrderDetail.OrderId))
            {
                MessageBox.Show("无订单，请先检索箱子", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PrintItemFrm frm = new PrintItemFrm();
            frm.OrderID = OrderDetail.OrderId;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                InitializeCustomItems();
                ShowLabel();
            }            
        }

        private void InitializeCustomItems()
        {
            Custom1Label.Text = "自定义1";
            Custom2Label.Text = "自定义2";
            Custom3Label.Text = "自定义3";
            Custom4Label.Text = "自定义4";
            Custom5Label.Text = "自定义5";

            using (var conn = DBHelper.Instance.Open())
            {
                if (conn != null)
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT Name, DispText FROM Print WHERE Id = @Id AND Name like 'Custom%' and DispSeq > 0";
                        command.Parameters.Add("@Id", DbType.String);
                        command.Parameters.Add("@Name", DbType.String);
                        command.Parameters["@Id"].Value = OrderDetail.OrderId;

                        using (SQLiteDataReader dr = command.ExecuteReader())
                        {
                            while (dr != null && dr.Read())
                            {
                                string name = dr.GetString(0);
                                string dispText = dr.GetString(1);
                                //string defValue = dr.GetString(1);
                                if (name == "Custom1")
                                {
                                    if (!String.IsNullOrEmpty(dispText))
                                    {
                                        Custom1Label.Text = dispText;
                                    }
                                }
                                else if (name == "Custom2")
                                {
                                    if (!String.IsNullOrEmpty(dispText))
                                    {
                                        Custom2Label.Text = dispText;
                                    }
                                }
                                else if (name == "Custom3")
                                {
                                    if (!String.IsNullOrEmpty(dispText))
                                    {
                                        Custom3Label.Text = dispText;
                                    }
                                }
                                else if (name == "Custom4")
                                {
                                    if (!String.IsNullOrEmpty(dispText))
                                    {
                                        Custom4Label.Text = dispText;
                                    }
                                }
                                else if (name == "Custom5")
                                {
                                    if (!String.IsNullOrEmpty(dispText))
                                    {
                                        Custom5Label.Text = dispText;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void HeadFontList_SelectedValueChanged(object sender, EventArgs e)
        {
            ShowLabel();
        }

        

        private void HeadFontList_SelectedValueChanged_1(object sender, EventArgs e)
        {
            string HeadFontName = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["HeadFontName"] as string;
            if (!String.IsNullOrEmpty(HeadFontName) && !String.Equals(def, HeadFontName))
            {
                Properties.Settings.Default["HeadFontName"] = HeadFontName;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void HeadFontSizeList_SelectedValueChanged(object sender, EventArgs e)
        {
            string HeadFontSize = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["HeadFontSize"] as string;
            if (!String.IsNullOrEmpty(HeadFontSize) && !String.Equals(def, HeadFontSize))
            {
                Properties.Settings.Default["HeadFontSize"] = HeadFontSize;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void HeadFontTypeList_SelectedValueChanged(object sender, EventArgs e)
        {
            string HeadFonStyle = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["HeadFontStyle"] as string;

            if (!String.IsNullOrEmpty(HeadFonStyle) && !String.Equals(def, HeadFonStyle))
            {
                Properties.Settings.Default["HeadFontStyle"] = HeadFonStyle;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void TitleFontList_SelectedValueChanged(object sender, EventArgs e)
        {
            string TitleFontName = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["TitleFontName"] as string;

            if (!String.IsNullOrEmpty(TitleFontName) && !String.Equals(def, TitleFontName))
            {
                Properties.Settings.Default["TitleFontName"] = TitleFontName;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void TitleFontSizeList_SelectedValueChanged(object sender, EventArgs e)
        {
            string TitleFontSize = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["TitleFontSize"] as string;

            if (!String.IsNullOrEmpty(TitleFontSize) && !String.Equals(def, TitleFontSize))
            {
                Properties.Settings.Default["TitleFontSize"] = TitleFontSize;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void TitleFontTypeList_SelectedValueChanged(object sender, EventArgs e)
        {
            string TitleFontStyle = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["TitleFontStyle"] as string;
            if (!String.IsNullOrEmpty(TitleFontStyle) && !String.Equals(def, TitleFontStyle))
            {
                Properties.Settings.Default["TitleFontStyle"] = TitleFontStyle;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void FieldFontList_SelectedValueChanged(object sender, EventArgs e)
        {
            string FieldFontName = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["FieldFontName"] as string;

            if (!String.IsNullOrEmpty(FieldFontName) && !String.Equals(def, FieldFontName))
            {
                Properties.Settings.Default["FieldFontName"] = FieldFontName;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void FieldFontSizeList_SelectedValueChanged(object sender, EventArgs e)
        {
            string TitleFontSize = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["TitleFontSize"] as string;

            if (!String.IsNullOrEmpty(TitleFontSize) && !String.Equals(def, TitleFontSize))
            {
                Properties.Settings.Default["TitleFontSize"] = TitleFontSize;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void FieldFontTypeList_SelectedValueChanged(object sender, EventArgs e)
        {
            string TitleFontStyle = (sender as ComboBox).SelectedItem as string;
            string def = Properties.Settings.Default["TitleFontStyle"] as string;

            if (!String.IsNullOrEmpty(TitleFontStyle) && !String.Equals(def, TitleFontStyle))
            {
                Properties.Settings.Default["TitleFontStyle"] = TitleFontStyle;
                Properties.Settings.Default.Save();
                ShowLabel();
            }
        }

        private void LineSpaceTB_Leave(object sender, EventArgs e)
        {
            string v = (sender as TextBox).Text;
            int Result;
            if (Int32.TryParse(v, out Result))
            {
                string def = Properties.Settings.Default["LineSpace"] as string;
                if(!String.Equals(def, v))
                {
                    Properties.Settings.Default["LineSpace"] = v;
                    Properties.Settings.Default.Save();
                    ShowLabel();
                }                
            }
            else
            {
                MessageBox.Show("必须为数字", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LeftMarginTB_Leave(object sender, EventArgs e)
        {
            string v = (sender as TextBox).Text;
            int Result;
            if (Int32.TryParse(v, out Result))
            {
                string def = Properties.Settings.Default["LeftMargin"] as string;
                if (!String.Equals(def, v))
                {
                    Properties.Settings.Default["LeftMargin"] = v;
                    Properties.Settings.Default.Save();
                    ShowLabel();
                }                    
            }
            else
            {
                MessageBox.Show("必须为数字", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TopMarginTB_Leave(object sender, EventArgs e)
        {
            string v = (sender as TextBox).Text;
            int Result;
            if (Int32.TryParse(v, out Result))
            {
                string def = Properties.Settings.Default["TopMargin"] as string;
                if (!String.Equals(def, v))
                {
                    Properties.Settings.Default["TopMargin"] = v;
                    Properties.Settings.Default.Save();
                    ShowLabel();
                }                    
            }
            else
            {
                MessageBox.Show("必须为数字", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                PrintLabelButton_Click(null, null);
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}

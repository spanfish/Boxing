using PackingTracker.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.newtronics.UI
{
    public partial class PrintItemFrm : Form
    {
        #region 打印项目
        //订单号
        public string OrderID
        {
            get;
            set;
        }

        //订单编码顺序
        public string OrderCodeSeq
        {
            get;
            set;
        }
        //订单编码名称
        public string OrderCode
        {
            get;
            set;
        }
        public string OrderCodeBar
        {
            get;
            set;
        }

        //工单编码顺序
        public string WorkFlowSeq
        {
            get;
            set;
        }

        public string WorkFlow
        {
            get;
            set;
        }
        public string WorkFlowBar
        {
            get;
            set;
        }

        //批次号
        public string BatchSeq
        {
            get;
            set;
        }
        //批次号名称
        public string Batch
        {
            get;
            set;
        }
        public string BatchBar
        {
            get;
            set;
        }
        //古北产品编码
        public string GubeiNoSeq
        {
            get;
            set;
        }
        //古北产品编码
        public string GubeiNo
        {
            get;
            set;
        }
        public string GubeiNoBar
        {
            get;
            set;
        }

        //客户产品编码
        public string CustomerNoSeq
        {
            get;
            set;
        }
        //客户产品编码
        public string CustomerNo
        {
            get;
            set;
        }
        public string CustomerNoBar
        {
            get;
            set;
        }

        //产品型号
        public string ProdModelSeq
        {
            get;
            set;
        }

        //产品型号
        public string ProdModel
        {
            get;
            set;
        }
        public string ProdModelBar
        {
            get;
            set;
        }
        //产品描述
        public string ProdDescSeq
        {
            get;
            set;
        }
        //产品描述
        public string ProdDesc
        {
            get;
            set;
        }
        public string ProdDescBar
        {
            get;
            set;
        }
        //Ver
        public string VerSeq
        {
            get;
            set;
        }
        //Ver
        public string Ver
        {
            get;
            set;
        }
        public string VerBar
        {
            get;
            set;
        }
        //装箱数量
        public string RealCountSeq
        {
            get;
            set;
        }
        //装箱数量
        public string RealCount
        {
            get;
            set;
        }
        public string RealCountBar
        {
            get;
            set;
        }
        //箱号
        public string BoxNoSeq
        {
            get;
            set;
        }
        //箱号
        public string BoxNo
        {
            get;
            set;
        }
        public string BoxNoBar
        {
            get;
            set;
        }
        //供应商
        public string SupplierSeq
        {
            get;
            set;
        }
        //供应商
        public string Supplier
        {
            get;
            set;
        }
        public string SupplierBar
        {
            get;
            set;
        }
        //QC
        public string QCSeq
        {
            get;
            set;
        }
        //QC
        public string QC
        {
            get;
            set;
        }
        public string QCBar
        {
            get;
            set;
        }
        //制造日期
        public string ManufactureDateSeq
        {
            get;
            set;
        }
        //制造日期
        public string ManufactureDate
        {
            get;
            set;
        }
        public string ManufactureDateBar
        {
            get;
            set;
        }


        public string FirmwareSeq
        {
            get;
            set;
        }

        public string Firmware
        {
            get;
            set;
        }
        public string FirmwareBar
        {
            get;
            set;
        }

        public string Custom1Seq
        {
            get;
            set;
        }

        public string Custom1
        {
            get;
            set;
        }
        public string Custom1Bar
        {
            get;
            set;
        }

        public string Custom2Seq
        {
            get;
            set;
        }

        public string Custom2
        {
            get;
            set;
        }
        public string Custom2Bar
        {
            get;
            set;
        }

        public string Custom3Seq
        {
            get;
            set;
        }

        public string Custom3
        {
            get;
            set;
        }
        public string Custom3Bar
        {
            get;
            set;
        }

        public string Custom4Seq
        {
            get;
            set;
        }

        public string Custom4
        {
            get;
            set;
        }
        public string Custom4Bar
        {
            get;
            set;
        }

        public string Custom5Seq
        {
            get;
            set;
        }

        public string Custom5
        {
            get;
            set;
        }
        public string Custom5Bar
        {
            get;
            set;
        }
        #endregion
        public PrintItemFrm()
        {
            InitializeComponent();
        }

        private void PrintItemFrm_Load(object sender, EventArgs e)
        {
            OrderIdTB.Text = OrderID;
            LoadPrintItems();
        }

        private void LoadPrintItems()
        {
            using (var conn = DBHelper.Instance.Open())
            {
                try
                {
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT  Name, DispSeq, DispText, BarCode from Print where Id=@Id";
                        command.Parameters.Add("@Id", DbType.String);
                        command.Parameters["@Id"].Value = OrderID;

                        using (SQLiteDataReader dr = command.ExecuteReader())
                        {
                            while (dr != null && dr.Read())
                            {
                                string Name = dr.GetString(0);
                                int DispSeq = dr.GetInt32(1);
                                string DispText = dr.GetString(2);
                                string BarCode = dr.GetString(3);
                                if (!String.IsNullOrEmpty(Name))
                                {
                                    var property = this.GetType().GetProperty(Name + "Seq");
                                    property.SetValue(this, DispSeq > 0 ? string.Format("{0}", DispSeq) : "");

                                    property = this.GetType().GetProperty(Name + "Bar");
                                    property.SetValue(this, BarCode);

                                    property = this.GetType().GetProperty(Name);
                                    property.SetValue(this, String.IsNullOrEmpty(DispText) ? Name : DispText);
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    DBHelper.Instance.Close(conn);
                }
            }

            OrderCodeSeqTB.Text = OrderCodeSeq;
            if (!String.IsNullOrEmpty(OrderCode))
            {
                OrderCodeTB.Text = OrderCode;
            }
            OrderCodeCK.Checked = OrderCodeBar == "1";

            WorkFlowSeqTB.Text = WorkFlowSeq;
            if (!String.IsNullOrEmpty(WorkFlow))
            {
                WorkFlowTB.Text = WorkFlow;
            }
            WorkFlowCK.Checked = WorkFlowBar == "1";

            BatchSeqTB.Text = BatchSeq;
            if (!String.IsNullOrEmpty(Batch))
            {
                BatchTB.Text = Batch;
            }
            BatchCK.Checked = BatchBar == "1";


            GubeiNoSeqTB.Text = GubeiNoSeq;
            if (!String.IsNullOrEmpty(GubeiNo))
            {
                GubeiNoTB.Text = GubeiNo;
            }

            GubeiNoCK.Checked = GubeiNoBar == "1";

            CustomerNoSeqTB.Text = CustomerNoSeq;

            if (!String.IsNullOrEmpty(CustomerNo))
            {
                CustomerNoTB.Text = CustomerNo;
            }
            CustomerNoCK.Checked = CustomerNoBar == "1";

            ProdModelSeqTB.Text = ProdModelSeq;

            if (!String.IsNullOrEmpty(ProdModel))
            {
                ProdModelTB.Text = ProdModel;
            }
            ProdModelCK.Checked = ProdModelBar == "1";

            ProdDescSeqTB.Text = ProdDescSeq;

            if (!String.IsNullOrEmpty(ProdDesc))
            {
                ProdDescTB.Text = ProdDesc;
            }
            ProdDescCK.Checked = ProdDescBar == "1";

            VerSeqTB.Text = VerSeq;

            if (!String.IsNullOrEmpty(Ver))
            {
                VerTB.Text = Ver;
            }
            VerCK.Checked = VerBar == "1";

            RealCountSeqTB.Text = RealCountSeq;
            if (!String.IsNullOrEmpty(RealCount))
            {
                RealCountTB.Text = RealCount;
            }
            RealCountCK.Checked = RealCountBar == "1";

            BoxNoSeqTB.Text = BoxNoSeq;
            if (!String.IsNullOrEmpty(BoxNo))
            {
                BoxNoTB.Text = BoxNo;
            }
            BoxNoCK.Checked = BoxNoBar == "1";

            SupplierSeqTB.Text = SupplierSeq;
            if (!String.IsNullOrEmpty(Supplier))
            {
                SupplierTB.Text = Supplier;
            }

            SupplierCK.Checked = SupplierBar == "1";

            QCSeqTB.Text = QCSeq;

            if (!String.IsNullOrEmpty(QC))
            {
                QCTB.Text = QC;
            }
            QCCK.Checked = QCBar == "1";

            ManufactureDateSeqTB.Text = ManufactureDateSeq;

            if (!String.IsNullOrEmpty(ManufactureDate))
            {
                ManufactureDateTB.Text = ManufactureDate;
            }
            ManufactureDateCK.Checked = ManufactureDateBar == "1";

            FirmwareSeqTB.Text = FirmwareSeq;
            if (!String.IsNullOrEmpty(Firmware))
            {
                FirmwareTB.Text = Firmware;
            }
            FirmwareCK.Checked = FirmwareBar == "1";

            Custom1SeqTB.Text = Custom1Seq;
            if (!String.IsNullOrEmpty(Custom1))
            {
                Custom1TB.Text = Custom1;
            }
            Custom1CK.Checked = Custom1Bar == "1";

            Custom2SeqTB.Text = Custom2Seq;
            if (!String.IsNullOrEmpty(Custom2))
            {
                Custom2TB.Text = Custom2;
            }
            Custom2CK.Checked = Custom2Bar == "1";

            Custom3SeqTB.Text = Custom3Seq;
            if (!String.IsNullOrEmpty(Custom3))
            {
                Custom3TB.Text = Custom3;
            }
            Custom3CK.Checked = Custom3Bar == "1";

            Custom4SeqTB.Text = Custom4Seq;
            if (!String.IsNullOrEmpty(Custom4))
            {
                Custom4TB.Text = Custom4;
            }
            Custom4CK.Checked = Custom4Bar == "1";

            Custom5SeqTB.Text = Custom5Seq;
            if (!String.IsNullOrEmpty(Custom5))
            {
                Custom5TB.Text = Custom5;
            }
            Custom5CK.Checked = Custom5Bar == "1";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            OrderCodeSeq = OrderCodeSeqTB.Text;
            OrderCode = OrderCodeTB.Text;
            OrderCodeBar = OrderCodeCK.Checked ? "1" : "0";

            BatchSeq = BatchSeqTB.Text;
            Batch = BatchTB.Text;
            BatchBar = BatchCK.Checked ? "1" : "0";

            WorkFlowSeq = WorkFlowSeqTB.Text;
            WorkFlow = WorkFlowTB.Text;
            WorkFlowBar = WorkFlowCK.Checked ? "1" : "0";

            GubeiNoSeq = GubeiNoSeqTB.Text;
            GubeiNo = GubeiNoTB.Text;
            GubeiNoBar = GubeiNoCK.Checked ? "1" : "0";

            CustomerNoSeq = CustomerNoSeqTB.Text;
            CustomerNo = CustomerNoTB.Text;
            CustomerNoBar = CustomerNoCK.Checked ? "1" : "0";

            ProdModelSeq = ProdModelSeqTB.Text;
            ProdModel = ProdModelTB.Text;
            ProdModelBar = ProdModelCK.Checked ? "1" : "0";

            ProdDescSeq = ProdDescSeqTB.Text;
            ProdDesc = ProdDescTB.Text;
            ProdDescBar = ProdDescCK.Checked ? "1" : "0";

            VerSeq = VerSeqTB.Text;
            Ver = VerTB.Text;
            VerBar = VerCK.Checked ? "1" : "0";

            RealCountSeq = RealCountSeqTB.Text;
            RealCount = RealCountTB.Text;
            RealCountBar = RealCountCK.Checked ? "1" : "0";

            BoxNoSeq = BoxNoSeqTB.Text;
            BoxNo = BoxNoTB.Text;
            BoxNoBar = BoxNoCK.Checked ? "1" : "0";

            SupplierSeq = SupplierSeqTB.Text;
            Supplier = SupplierTB.Text;
            SupplierBar = SupplierCK.Checked ? "1" : "0";

            QCSeq = QCSeqTB.Text;
            QC = QCTB.Text;
            QCBar = QCCK.Checked ? "1" : "0";

            ManufactureDateSeq = ManufactureDateSeqTB.Text;
            ManufactureDate = ManufactureDateTB.Text;
            ManufactureDateBar = ManufactureDateCK.Checked ? "1" : "0";

            FirmwareSeq = FirmwareSeqTB.Text;
            Firmware = FirmwareTB.Text;
            FirmwareBar = FirmwareCK.Checked ? "1" : "0";

            Custom1Seq = Custom1SeqTB.Text;
            Custom1 = Custom1TB.Text;
            Custom1Bar = Custom1CK.Checked ? "1" : "0";

            Custom2Seq = Custom2SeqTB.Text;
            Custom2 = Custom2TB.Text;
            Custom2Bar = Custom2CK.Checked ? "1" : "0";

            Custom3Seq = Custom3SeqTB.Text;
            Custom3 = Custom3TB.Text;
            Custom3Bar = Custom3CK.Checked ? "1" : "0";

            Custom4Seq = Custom4SeqTB.Text;
            Custom4 = Custom4TB.Text;
            Custom4Bar = Custom4CK.Checked ? "1" : "0";

            Custom5Seq = Custom5SeqTB.Text;
            Custom5 = Custom5TB.Text;
            Custom5Bar = Custom5CK.Checked ? "1" : "0";

            SavePrintItem();
        }

        void SavePrintItem()
        {
            using (var conn = DBHelper.Instance.Open())
            {
                SQLiteTransaction trans = null;
                try
                {
                    trans = conn.BeginTransaction();
                    using (SQLiteCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "DELETE from Print where Id=@Id";
                        command.Parameters.Add("@Id", DbType.String);
                        command.Parameters["@Id"].Value = OrderID;
                        command.ExecuteNonQuery();

                        command.CommandText = "INSERT INTO Print(Id, Name, DispSeq, DispText, BarCode) VALUES(@Id, @Name, @DispSeq, @DispText, @BarCode)";
                        command.Parameters.Add("@Name", DbType.String);
                        command.Parameters.Add("@DispSeq", DbType.Int32);
                        command.Parameters.Add("@DispText", DbType.String);
                        command.Parameters.Add("@BarCode", DbType.String);

                        command.Parameters["@Name"].Value = "OrderCode";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(OrderCodeSeq) ? 0 : Int32.Parse(OrderCodeSeq);
                        command.Parameters["@DispText"].Value = OrderCode;
                        command.Parameters["@BarCode"].Value = OrderCodeBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "WorkFlow";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(WorkFlowSeq) ? 0 : Int32.Parse(WorkFlowSeq);
                        command.Parameters["@DispText"].Value = WorkFlow;
                        command.Parameters["@BarCode"].Value = WorkFlowBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Batch";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(BatchSeq) ? 0 : Int32.Parse(BatchSeq);
                        command.Parameters["@DispText"].Value = Batch;
                        command.Parameters["@BarCode"].Value = BatchBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "GubeiNo";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(GubeiNoSeq) ? 0 : Int32.Parse(GubeiNoSeq);
                        command.Parameters["@DispText"].Value = GubeiNo;
                        command.Parameters["@BarCode"].Value = GubeiNoBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "CustomerNo";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(CustomerNoSeq) ? 0 : Int32.Parse(CustomerNoSeq);
                        command.Parameters["@DispText"].Value = CustomerNo;
                        command.Parameters["@BarCode"].Value = CustomerNoBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "ProdModel";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(ProdModelSeq) ? 0 : Int32.Parse(ProdModelSeq);
                        command.Parameters["@DispText"].Value = ProdModel;
                        command.Parameters["@BarCode"].Value = ProdModelBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "ProdDesc";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(ProdDescSeq) ? 0 : Int32.Parse(ProdDescSeq);
                        command.Parameters["@DispText"].Value = ProdDesc;
                        command.Parameters["@BarCode"].Value = ProdDescBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Ver";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(VerSeq) ? 0 : Int32.Parse(VerSeq);
                        command.Parameters["@DispText"].Value = Ver;
                        command.Parameters["@BarCode"].Value = VerBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "RealCount";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(RealCountSeq) ? 0 : Int32.Parse(RealCountSeq);
                        command.Parameters["@DispText"].Value = RealCount;
                        command.Parameters["@BarCode"].Value = RealCountBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "BoxNo";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(BoxNoSeq) ? 0 : Int32.Parse(BoxNoSeq);
                        command.Parameters["@DispText"].Value = BoxNo;
                        command.Parameters["@BarCode"].Value = BoxNoBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Id"].Value = OrderID;
                        command.Parameters["@Name"].Value = "Supplier";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(SupplierSeq) ? 0 : Int32.Parse(SupplierSeq);
                        command.Parameters["@DispText"].Value = Supplier;
                        command.Parameters["@BarCode"].Value = SupplierBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "QC";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(QCSeq) ? 0 : Int32.Parse(QCSeq);
                        command.Parameters["@DispText"].Value = QC;
                        command.Parameters["@BarCode"].Value = QCBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "ManufactureDate";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(ManufactureDateSeq) ? 0 : Int32.Parse(ManufactureDateSeq);
                        command.Parameters["@DispText"].Value = ManufactureDate;
                        command.Parameters["@BarCode"].Value = ManufactureDateBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Firmware";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(FirmwareSeq) ? 0 : Int32.Parse(FirmwareSeq);
                        command.Parameters["@DispText"].Value = Firmware;
                        command.Parameters["@BarCode"].Value = FirmwareBar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Custom1";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(Custom1Seq) ? 0 : Int32.Parse(Custom1Seq);
                        command.Parameters["@DispText"].Value = Custom1;
                        command.Parameters["@BarCode"].Value = Custom1Bar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Custom2";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(Custom2Seq) ? 0 : Int32.Parse(Custom2Seq);
                        command.Parameters["@DispText"].Value = Custom2;
                        command.Parameters["@BarCode"].Value = Custom2Bar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Custom3";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(Custom3Seq) ? 0 : Int32.Parse(Custom3Seq);
                        command.Parameters["@DispText"].Value = Custom3;
                        command.Parameters["@BarCode"].Value = Custom3Bar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Custom4";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(Custom4Seq) ? 0 : Int32.Parse(Custom4Seq);
                        command.Parameters["@DispText"].Value = Custom4;
                        command.Parameters["@BarCode"].Value = Custom4Bar;
                        command.ExecuteNonQuery();

                        command.Parameters["@Name"].Value = "Custom5";
                        command.Parameters["@DispSeq"].Value = String.IsNullOrEmpty(Custom5Seq) ? 0 : Int32.Parse(Custom5Seq);
                        command.Parameters["@DispText"].Value = Custom5;
                        command.Parameters["@BarCode"].Value = Custom5Bar;
                        command.ExecuteNonQuery();
                    }
                    trans.Commit();

                    //MessageBox.Show("成功保存", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    DBHelper.Instance.Close(conn);
                }
            }
        }
    }
}

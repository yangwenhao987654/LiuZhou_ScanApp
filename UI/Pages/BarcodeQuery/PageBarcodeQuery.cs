﻿using CSharpFormApplication;
using Microsoft.Extensions.DependencyInjection;
using ScanApp.DAL.Entity;
using SJTU_UI.ExcleUtil;
using Sunny.UI;
using UI.DAL.BLL;

namespace DWZ_Scada.Pages
{
    public partial class PageBarcodeQuery : UIPage
    {
        private static PageBarcodeQuery _instance;
        public static PageBarcodeQuery Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(PageBarcodeQuery))
                    {
                        if (_instance == null)
                        {
                            _instance = new PageBarcodeQuery();
                        }
                    }
                }
                return _instance;
            }
        }
        AutoResizeForm asc = new AutoResizeForm();

        private IBarcodeRecordBLL barcodeRecordBll;
        public PageBarcodeQuery()
        {
            InitializeComponent();
            barcodeRecordBll = Global.ServiceProvider.GetRequiredService<IBarcodeRecordBLL>();
        }

        private void Formula_set_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        public new void Show()
        {
            base.Show();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void Page_Formula_Set_Load(object sender, EventArgs e)
        {
            //asc.controllInitializeSize(this);
            dtStart.Value = DateTime.Today;
            dtEnd.Value = DateTime.Now;
            Task.Run(SelectByTime);

            //uiPanel1.Refresh();
        }

        private void ReflashTable(List<BarcodeRecordEntity> list)
        {
            if (InvokeRequired)
            {
                dgv.Invoke(new Action<List<BarcodeRecordEntity>>(ReflashTable), list);
                return;
            }
            int okCount = 0;
            int NGCount = 0;
            dgv.Rows.Clear();
            if (list == null)
            {
                return;
            }
            dgv.SuspendLayout();
            int id = 1;
            foreach (var item in list)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv);
                row.Cells[clmOrderNum.Index].Value = id++;
                row.Cells[clmRowID.Index].Value = item.Id;
                row.Cells[clmBarcode.Index].Value = item.Barcode;
                row.Cells[clmCode.Index].Value = item.ProductCode;
                row.Cells[clmMsg.Index].Value = item.ErrInfo;
                row.Cells[clmMadeDate.Index].Value = item.UseDateStr;
                row.Cells[clmScanTime.Index].Value = item.ScanTime.ToString("yyyy-MM-dd HH:mm:ss fff");
                row.Cells[clmResult.Index].Value = item.Result;
                if (item.Result)
                {
                    okCount++;
                }
                else
                {
                    NGCount++;
                }
                dgv.Rows.Add(row);
            }
            dgv.ResumeLayout();
            dgv.ClearSelection();
            dgv.CurrentCell = null;
            lbl_OKCount.Text = okCount.ToString();
            lbl_NGCount.Text = NGCount.ToString();
        }

        private void Page_Formula_Set_SizeChanged(object sender, EventArgs e)
        {
            //asc.controlAutoSize(this);
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            string input = tbx_input.Text;
            if (input.IsNullOrEmpty())
            {
                return;
            }
            SelectByBarcode(input);
        }

        /// <summary>
        /// 根据条码查询
        /// </summary>
        /// <param name="barcode"></param>
        private void SelectByBarcode(string barcode)
        {
            List<BarcodeRecordEntity> list = barcodeRecordBll.SelectByBarcode(barcode);
            ReflashTable(list);
        }

        private void SelectByTime()
        {
            DateTime start = dtStart.Value;
            DateTime end = dtEnd.Value;
            List<BarcodeRecordEntity> list = null;

            if (rbtb_NG.Checked)
            {
                list = barcodeRecordBll.SelectNgList(start, end);
            }
            else if (rbtn_OK.Checked)
            {
                list = barcodeRecordBll.SelectOkList(start, end);
            }
            else
            {
                list = barcodeRecordBll.SelectByScanTime(start, end);
            }
            ReflashTable(list);
        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {
            SelectByTime();
        }

        private void uiPanel1_Click(object sender, EventArgs e)
        {

        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            ExcleHelper.DataGridViewToCSV(dgv);
        }
    }
}

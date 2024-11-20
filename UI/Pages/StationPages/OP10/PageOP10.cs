﻿using DWZ_Scada.ctrls.LogCtrl;
using DWZ_Scada.Forms.ProductFormula;
using DWZ_Scada.UIUtil;
using LogTool;
using ScanApp.DAL.DBContext;
using ScanApp.DAL.Entity;
using Sunny.UI;
using UI.BarcodeCheck;
using UI.Forms.BarcodeRules;
using UI.Validator;

namespace DWZ_Scada.Pages.StationPages.OP10
{
    public partial class PageOP10 : UIPage
    {

        private readonly Action _clearAlarmDelegate;


        private static PageOP10 _instance;
        public static PageOP10 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(PageOP10))
                    {
                        if (_instance == null)
                        {
                            _instance = new PageOP10();
                        }
                    }
                }
                return _instance;
            }
        }

        public int SelectCodeType { get; set; } = -1;

        public List<ProductFormulaEntity> list { get; set; } = new List<ProductFormulaEntity>();

        public ProductFormulaEntity SelectProduct { get; set; }

        private PageOP10()
        {
            InitializeComponent();
            _instance = this;
        }

        private async void Page_Load(object sender, EventArgs e)
        {
            //LogMgr.Instance.SetCtrl(listViewEx_Log1);
            LogMgr.Instance.Debug("打开扫码对比软件");

            //OP10MainFunc.Instance.StartAsync();

            myLogCtrl1.BindingControl = uiPanel1;
            Mylog.Instance.Init(myLogCtrl1);

            await Task.Run(async () =>
            {
                using (MyDbContext db = new MyDbContext())
                {
                    list = db.tbProductFormula.ToList();
                }
            });
            uiComboBox1.DataSource = list;
            uiComboBox1.DisplayMember = "ProductName";
        }


        private void Instance_OnEntryStateChanged(string sn, int result, string msg = "")
        {
            MyUIControler.UpdateEntryStateCtrl(userCtrlEntry1, sn, result, msg);
        }


        private void uiLabel1_Click(object sender, EventArgs e)
        {

        }

        private void PageOP10_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogMgr.Instance.Info("关闭OP10-HttpServer");
            if (!OP10MainFunc.IsInstanceNull)
            {
                OP10MainFunc.Instance?.Dispose();
            }
            LogMgr.Instance.Info("关闭OP10程序");
            _instance = null;
            //调用 Close() 方法,先进入  FormClosing 事件 ，之后再调用Designer类的Dispose
        }


        private void uiButton1_Click(object sender, EventArgs e)
        {

        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            FormRulesQuery form = new FormRulesQuery();
            form.ShowDialog();
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiComboBox1.SelectedIndex == -1)
            {
                return;
            }

            SelectProduct = uiComboBox1.SelectedItem as ProductFormulaEntity;
            if (SelectProduct == null)
            {
                UIMessageBox.ShowInfo("获取产品信息错误");
                return;
            }

            #region 清除原内容

            tbx_Spy.Text = "";
            tbx_Code.Text = "";
            tbx_CodeType.Text = "";
            tbx_DateFormat.Text = "";
            tbx_Part.Text = "";
            tbx_FixValue1.Text = "";
            #endregion

            //TODO 根据条码类型解析产品
            switch (SelectProduct.BarcodeType)
            {
                case CodeType.Code14:
                    tbx_FixValue1.Text = SelectProduct.FixedValue1;
                    tbx_DateFormat.Text = "yyMMdd";
                    break;
                case CodeType.Code31:
                    tbx_Spy.Text = SelectProduct.SupplierCode;
                    tbx_DateFormat.Text = "yyMMdd";
                    break;
                case CodeType.Code40:
                    tbx_Spy.Text = SelectProduct.SupplierCode;
                    tbx_Part.Text = SelectProduct.PartCode;
                    tbx_DateFormat.Text = "yyMMdd";
                    break;
                case CodeType.Code43:
                    tbx_Spy.Text = SelectProduct.SupplierCode;
                    tbx_DateFormat.Text = "yyyyMMdd";
                    break;
                default:
                    UIMessageBox.ShowError("未知的条码类型");
                    return;
                    break;
            }
            SelectCodeType = SelectProduct.BarcodeType;
            //获取当前选中项的信息
            tbx_CodeType.Text = $"{SelectCodeType}位码";
            tbx_Code.Text = $"{SelectProduct.ProductCode}";

            //清空其他内容
        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {
            if (SelectProduct==null)
            {
                UIMessageBox.ShowError("请先选择产品");
                return;
            }

            try
            {
                string input = tbx_Input.Text;
                DateTime dt = uiDatePicker1.Value;
                string dateStr = dt.ToString(tbx_DateFormat.Text);
                bool b = BarcodeValidator.Validate(input, SelectProduct, dateStr, out var err);
                if (b)
                {
                    Mylog.Instance.Info($"[{input}]校验成功");
                }
                else
                {
                    Mylog.Instance.Error($"[{input}]校验失败,{err}");
                }
            }
            catch (Exception exception)
            {
                UIMessageBox.ShowError($"校验异常:{exception.Message}");
            }
          
        }
    }
}
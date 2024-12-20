﻿using CSharpFormApplication;
using DWZ_Scada.Forms.ProductFormula;
using LogTool;
using Microsoft.Extensions.DependencyInjection;
using ScanApp.DAL.Entity;
using Sunny.UI;
using UI.DAL.DAL;

namespace DWZ_Scada.Pages
{
    public partial class PageFormulaQuery : UIPage
    {
        private static PageFormulaQuery _instance;
        public static PageFormulaQuery Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(PageFormulaQuery))
                    {
                        if (_instance == null)
                        {
                            _instance = new PageFormulaQuery();
                        }
                    }
                }
                return _instance;
            }
        }
        AutoResizeForm asc = new AutoResizeForm();

        public static event Action ProductFormulaChanged;

        private IProductFormulaDAL productFormulaDAL;
        public PageFormulaQuery()
        {
            InitializeComponent();
            productFormulaDAL = Global.ServiceProvider.GetRequiredService<IProductFormulaDAL>();
        }

        private void Formula_set_FormClosing(object sender, FormClosingEventArgs e)
        {
            PageFormulaQuery.Instance?.Dispose();
        }

        private void Page_Formula_Set_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
            Task.Run(SelectAll);
        }

        private void ReflashTable(List<ProductFormulaEntity> list)
        {
            if (InvokeRequired)
            {
                dgv.Invoke(new Action<List<ProductFormulaEntity>>(ReflashTable), list);
                return;
            }
            dgv.Rows.Clear();
            dgv.SuspendLayout();
            int id = 1;
            foreach (var item in list)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv);
                row.Cells[clmRowID.Index].Value =item.ID ;
                row.Cells[clmOrderNum.Index].Value = id++;
                row.Cells[clmName.Index].Value = item.ProductName;
                row.Cells[clmCode.Index].Value = item.ProductCode;

                row.Cells[clmBarcodeType.Index].Value = item.BarcodeType;
                row.Cells[clmSupplierCode.Index].Value = item.SupplierCode;
                row.Cells[clmPartNum.Index].Value = item.PartCode;
                row.Cells[clmFixValue1.Index].Value = item.FixedValue1;
                dgv.Rows.Add(row);
            }
            dgv.ResumeLayout();
            dgv.ClearSelection();
            dgv.CurrentCell =null;
        }

        private void Page_Formula_Set_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            string input = tbx_input.Text;
            if (input.IsNullOrEmpty())
            {
                SelectAll();
            }
            else
            {
                SelectByProdCode(input);
            }
        }

        private void SelectByProdCode(string code)
        {
            List<ProductFormulaEntity> list = productFormulaDAL.SelectAllByProdCode(code);
            ReflashTable(list);
        }

        private void SelectAll()
        {
            List<ProductFormulaEntity> list = productFormulaDAL.SelectAll();
            ReflashTable(list);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            //弹出一个添加的form
            FormProductFormulaAdd form = new FormProductFormulaAdd();
            form.ShowDialog();
            SelectAll();
            ProductFormulaChanged?.Invoke();
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            //获取选中项 弹出菜单
            int index = dgv.SelectedIndex;
            if (index==-1)
            {
                return;
            }
            try
            {
                int id = (int)dgv.Rows[index].Cells[clmRowID.Index].Value;
                FormProductFormulaSetting form = new FormProductFormulaSetting(id);
                form.ShowDialog();
                SelectAll();
            }
            catch (Exception ex)
            {
                LogMgr.Instance.Error($"错误:{ex.Message}");
            }
            ProductFormulaChanged?.Invoke();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            int index = dgv.SelectedIndex;
            if (index == -1)
            {
                return;
            }
            try
            {
                bool b = UIMessageBox.ShowAsk($"确定要删除产品配方吗?\n" +
                                                        $"名称:{dgv.Rows[index].Cells[2].Value}\n " +
                                                        $"编号:{dgv.Rows[index].Cells[3].Value}");

                if (!b)
                {
                    return;
                }

                int id = (int)dgv.Rows[index].Cells[clmRowID.Index].Value;
                bool flag = productFormulaDAL.RemoveById(id);
                if (!flag)
                {
                    UIMessageBox.ShowError($"删除失败:ID[{id}]");
                }
                else
                {
                    UIMessageBox.Show("删除成功!");
                }
                SelectAll();
            }
            catch (Exception ex)
            {
                LogMgr.Instance.Error($"错误:{ex.Message}");
            }
            ProductFormulaChanged?.Invoke();
        }
    }
}

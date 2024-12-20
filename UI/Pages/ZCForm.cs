﻿using AutoTF;
using DWZ_Scada.Pages.StationPages.OP10;
using LogTool;
using Sunny.UI;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using UtilUIYwh.ControlWrapper;
using Timer = System.Threading.Timer;

namespace DWZ_Scada.Pages
{
    public partial class ZCForm : UIForm
    {
        private Form mainForm;
        private static ZCForm _instance;

        private Timer timer;

        private ZCForm()
        {
            InitializeComponent();
        }

        public static ZCForm Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(ZCForm))
                    {
                        if (_instance == null)
                        {
                            _instance = new ZCForm();
                        }
                    }
                }
                return _instance;
            }
        }

        private void ZCForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            bool flag = UIMessageBox.ShowAsk("确定要退出吗?");
            if (flag)
            {
                try
                {
                    LogMgr.Instance.Info("退出程序");
                    //释放资源
                    timer?.Dispose();
                    foreach (Control ctrl in uiPanel1.Controls)
                    {
                        ctrl?.Dispose();
                    }
                    e.Cancel = false;
                    Environment.Exit(0);
                }
                catch (Exception exception)
                {

                }
            }
        }

        public void UpdatePlcState(PlcState state)
        {
            if (state.ToString() == uiLight1.Tag.ToString())
            {
                return;
            }
            if (InvokeRequired)
            {
                Invoke(new Action<PlcState>(UpdatePlcState), state);
                return;
            }
            lbl_PLCConn.Text = "已连接";
            switch (state)
            {
                case PlcState.OffLine:
                    lbl_PLCState.Text = "离线";
                    lbl_PLCConn.Text = "未连接";
                    uiLight1.OnColor = Color.LightGray;
                    break;

                case PlcState.Online:
                    lbl_PLCState.Text = "在线";
                    uiLight1.OnColor = Color.Green;
                    break;
                case PlcState.Alarm:
                    lbl_PLCState.Text = "报警";
                    uiLight1.OnColor = Color.Red;
                    break;
                case PlcState.Stop:
                    lbl_PLCState.Text = "停止";
                    uiLight1.OnColor = Color.DimGray;
                    break;
                case PlcState.Running:
                    lbl_PLCState.Text = "运行中";
                    uiLight1.OnColor = Color.GreenYellow;
                    break;
            }
        }

        private void ZCForm_Load(object sender, EventArgs e)
        {
            timer = new Timer(TimerElapsed, null, 0, 1000);
            // 设置KeyPreview为true以捕获键盘事件
            KeyPreview = true;
            // 添加事件处理程序
            KeyDown += Form_KeyDown;

            lblLoginUserName.Text = Global.LoginUser;
            lblLoginTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            InitializeLogo();
            if (!string.IsNullOrEmpty(SystemParams.Instance.CompanyName))
            {
                lbl_CompanyName.Text = SystemParams.Instance.CompanyName;
            }

            lbl_DeviceCompany.Text = SystemParams.Instance.DeviceCompany;
            lbl_DeviceName.Text = SystemParams.Instance.DeviceName;
           
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            lbl_Version.Text = version.ToString();
            SetAutoStart();
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    SetAutoStart();
                    break;
                case Keys.F2:
                    AddFormTopanel(PageBarcodeQuery.Instance);
                    break;
                case Keys.F3:
                    AddFormTopanel(PageFormulaQuery.Instance);
                    break;
                case Keys.F4:
                    //AddFormTopanel(DeviceControlPage.Instance);
                    break;
                case Keys.F5:
                    //AddFormTopanel(Page_PLCAddress.Instance);
                    break;
                case Keys.F11:
                    OpenPageProperty();
                    break;
                case Keys.F12:
                    break;
            }
        }

        private void ZCForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                uiButton6_Click(sender, e);
            }

            if (e.KeyCode == Keys.F5)
            {
            }
        }

        public void SetAutoSize()
        {
            Rectangle size1 = Screen.PrimaryScreen.Bounds;
            Rectangle size2 = Screen.PrimaryScreen.WorkingArea;
            MaximumSize = new Size(size2.Width, size2.Height);
            WindowState = FormWindowState.Maximized;
        }

        private void SetMainPage(Form form)
        {
            if (mainForm != form)
            {
                mainForm?.Close();
            }
            uiPanel1.Controls.Clear();
            form.Dock = DockStyle.Fill;
            //PageOp20的Show方法 很慢才进来
            form.Show();
            uiPanel1.Controls.Add(form);
            mainForm = form;
            //this.Text = "数据采集系统-" + mainForm.Text;
        }

        private void TimerElapsed(object state)
        {
            if (lblTime.InvokeRequired)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                lblTime.Invoke(() =>
                {
                    TimerElapsed(id);
                });
                return;
            }
            lblTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
        }

        public void AddFormTopanel(Control ctrl)
        {
            //LogMgr.Instance.Debug($"当前线程ID:{Thread.CurrentThread.ManagedThreadId.ToString()}");
            uiPanel1.Controls.Clear();
            ctrl.Dock = DockStyle.Fill;
           /* ControlWrapper controlWrapper = new ControlWrapper(ctrl);
            controlWrapper.Show();*/
            ctrl.Show();
            uiPanel1.Controls.Add(ctrl);
         
        }

        private static void OpenPageProperty()
        {
            bool flag = SystemParams.Instance.OpLvl == 0;
            if (!flag)
            {
                UIMessageBox.ShowError("当前登录账号权限不足！");
                return;
            }

            PageProperty form = new(SystemParams.Instance);
            form.ShowDialog();
            /*FormCustom form = new(pageProperty, "系统配置参数");
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();*/
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            OpenPageProperty();
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
           // AddFormTopanel(Page_PLCAddress.Instance);
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            //AddFormTopanel(PageUserQuery.Instance);
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            AddFormTopanel(PageFormulaQuery.Instance);
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
            AddFormTopanel(PageTabMenu_Debug.Instance);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            AddFormTopanel(PageBarcodeQuery.Instance);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            SetAutoStart();
        }

        private void SetAutoStart()
        {
            AddFormTopanel(PageOP10.Instance);
        }

        private void uiButton8_Click(object sender, EventArgs e)
        {
            this.OnFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, true));
        }


        private void uiLight1_Click(object sender, EventArgs e)
        {
        }

        private void InitializeLogo()
        {
            try
            {
                string logoDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Global.LogoFolder);
                if (!Directory.Exists(logoDirectory))
                {
                    Directory.CreateDirectory(logoDirectory);
                }

                // 检查配置文件中是否存储了路径
                string imagePath = Path.Combine(logoDirectory, SystemParams.Instance.LogoFilePath);
                if (File.Exists(imagePath))
                {
                    pictureBox1.ImageLocation = imagePath;
                }
            }
            catch (Exception e)
            {
               LogMgr.Instance.Error($"加载Logo文件错误:{e.Message}");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //1.点击之后弹出对话框 更换Logo 弹出文件选择器;
            //2.获取到图片路径 把图片备份一份 放到当前程序集的Debug/Logo 下面
            //3.保存图片的路径 下次启动的时候根据路径加载图片
            //4.设置PictureBox显示当前选择路径下的图片
            try
            {
                string sourcePath = "";
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.ico";

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    sourcePath = openFileDialog.FileName;
                }

                if (sourcePath=="")
                {
                    return;
                }
                string logoDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Global.LogoFolder);
                string destinationPath = Path.Combine(logoDirectory, Path.GetFileName(sourcePath));

                // 复制图片到目标路径
                if (sourcePath!=destinationPath)
                {
                    //如果路径相同 直接复制会被占用 报错
                    File.Copy(sourcePath, destinationPath, overwrite: true);
                }
             

                // 保存路径到配置文件
                SystemParams.Instance.LogoFilePath =Path.GetFileName(sourcePath);
                // 更新 PictureBox 显示图片
                pictureBox1.ImageLocation = destinationPath;
                SystemParams.Save();
                MessageBox.Show("Logo 已成功更换并保存！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                LogMgr.Instance.Error($"更新Logo错误:{exception.Message}\n {exception.StackTrace}");
            }
        }
    }
}
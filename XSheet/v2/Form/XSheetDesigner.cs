﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using DevExpress.XtraEditors;
using System.Drawing;
using DevExpress.Utils.Menu;
using XSheet.v2.Control;
using XSheet.v2.Data;
using XSheet.Util;
using XSheet.v2.Util;
using XSheet.v2.Privilege;

namespace XSheet.v2.Form
{
    public partial class XSheetDesigner : RibbonForm//,Observer
    {
        private Dictionary<String, SimpleButton> buttons { get; set; }
        private XSheetControl control { get; set; }
        private Dictionary<String, Label> labels { get; set; }
        Dictionary<String, PopupMenu> menus = new Dictionary<string, PopupMenu>();
        /*public XCfgData cfgData { get; set; }
        public XApp app { get; set; }
        public XSheet.Data.XSheet currentSheet{ get; set;}
        public XNamed currentXNamed { get; set; }
        
        public string executeState { get; set; }
        public string appstatu { get; set; }
        private CommandExecuter executer;
        private AreasCollection oldSelected { get; set; }*/
        public XSheetDesigner(XSheetUser user)
        {
            user.logAsDesigner = true;
            DateTime date = DateTime.Now;
            StreamWriter sw = new StreamWriter(@"ConsoleOutput.txt", true);
            TextWriter temp = Console.Out;
            InitializeComponent();
            InitSkinGallery();
            setDefaultParam();
            Console.SetOut(sw);
            Console.WriteLine("beforeDsp:" + date.ToString());
            sw.Close();
            Console.SetOut(temp);
            this.control = new XSheetControl(spreadsheetMain, buttons, labels,  menus, rightClickBarManager, this, alertcontrolMain,user);
            timer100ms.Start();
            date = DateTime.Now;
            sw = new StreamWriter(@"ConsoleOutput.txt", true);
            Console.SetOut(sw);
            Console.WriteLine("end:" + date.ToString());
            sw.Close();
            Console.SetOut(temp);
            this.Show();
        }

        private void setDefaultParam()
        {
            buttons = new Dictionary<string, SimpleButton>();
            /*提取按钮，将按钮与自定义事件名称绑定*/
            buttons.Add("BTN_SEARCH".ToUpper(), dbtn_Search);
            buttons.Add("BTN_EXECUTE".ToUpper(), dbtn_Execute);
            buttons.Add("BTN_DELETE".ToUpper(), btn_Delete);
            buttons.Add("BTN_EDIT".ToUpper(), dbtn_Edit);
            buttons.Add("BTN_NEW".ToUpper(), dbtn_New);
            buttons.Add("BTN_CANCEL".ToUpper(), btn_Cancel);
            buttons.Add("BTN_SAVE", btn_Save);
            buttons.Add("BTN_DASHBOARD", btn_Dashboard);
            labels = new Dictionary<String, Label>();
            labels.Add("lbl_AppName", this.lbl_AppName);
            labels.Add("lbl_Time", this.lbl_Time);
            //CELLCHANGE

            /*executer = new CommandExecuter();
            executer.Attach(this);
            executeState = "OK";
            //加载文档，后续根据不同设置配置，待修改TODO
            spreadsheetMain.Document.LoadDocument("\\\\ichart3d\\XSheetModel\\XSheet模板设计.xlsx");*/
            
            menus.Add("Normal", popupSpread);
            menus.Add("CfgData", popupDataCfg);
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        private void spreadsheetMain_SelectionChanged(object sender, EventArgs e)
        {
            control.spreadsheetMain_SelectionChanged(sender, e);
        }
        
        private void spreadsheetMain_DocumentLoaded(object sender, EventArgs e)
        {
            control.spreadsheetMain_DocumentLoaded(sender, e);
            this.Text = control.getTitle();
            this.BringToFront();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Search,0);
        }
        
        private void btn_New_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_New,0);
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Edit,0);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Delete,0);
        }

        private void btn_Exe_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Exe,0);
        }

        private void spreadsheetMain_CellValueChanged(object sender, SpreadsheetCellEventArgs e)
        {
            control.spreadsheetMain_CellValueChanged(sender, e);
            /*spreadsheetMain.Document.Calculate();
            setSelectedNamed();
            if (e.OldValue != e.Value)
            {
                executer.excueteCmd(currentXNamed, "Cell_Change");
            }*/
        }

        private void spreadsheetMain_MouseUp(object sender, MouseEventArgs e)
        {
            control.spreadsheetMain_MouseUp(sender, e);
            //oldSelected = null;
        }

        private void spreadsheetMain_ActiveSheetChanged(object sender, ActiveSheetChangedEventArgs e)
        {
            control.spreadsheetMain_ActiveSheetChanged(sender, e);
        }

        private void spreadsheetMain_HyperlinkClick(object sender, HyperlinkClickEventArgs e)
        {
            control.spreadsheetMain_HyperlinkClick(sender, e);
        }

        private void btn_Config_Click(object sender, EventArgs e)
        {
            control.btn_Config_Click(sender, e);
        }

        private void XSheetDesigner_FormClosed(object sender, FormClosedEventArgs e)
        {
            control.Closed();
            //MessageBox.Show("Close");
        }

        private void spreadsheetMain_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
        private void OnItemClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            XtraMessageBox.Show(item.Caption);
        }

        private void ts_multiSelect_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //alertcontrolMain.Show(this,"notice",ts_multiSelect.Checked.ToString());
            control.ChangeMuiltSingle(ts_multiSelect.Checked);
        }

        private void btn_DesignSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            control.EventCall(SysEvent.Event_Search, 0);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Cancel,0);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Save,0);
        }

        private void dbtn_Execute_Click(object sender, EventArgs e)
        {
            control.EventCall(SysEvent.Btn_Exe,0);
        }

        private void timer100ms_Tick(object sender, EventArgs e)
        {
            this.lbl_Time.Text = DateTime.Now.ToLongTimeString();
        }

        private void btn_Flash_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btn_Encode_ItemClick(object sender, ItemClickEventArgs e)
        {
            String str = spreadsheetMain.ActiveCell.DisplayText;
            String key = DESUtil.GenerateKey();
            String[] strs = new string[] {
            "Provider=IBMDA400;Data Source=172.31.96.210;User Id=ITSDTS;Password = STD008;",
            "Data Source=srf-sql;User Id=MARS_E;Password = rs@996t!ty",
            "Data Source=ichart3d;User Id=MARS_E;Password = rs@996t!ty",
            "Data Source=ichart3d;User Id=MARS_E;Password = rs@996t!ty"};
            str = DESUtil.EncryptString(str, key);
            Console.WriteLine(str);
            foreach (string item in strs)
            {
                Console.WriteLine("//////////////////////");
                Console.WriteLine(item);
                Console.WriteLine(" ");
                String tmp = DESUtil.EncryptString(item, key);
                Console.WriteLine(tmp);
                Console.WriteLine(" ");
                Console.WriteLine(DESUtil.DecryptString(tmp, key));

            }
        }

        private void btn_Exel_Click(object sender, EventArgs e)
        {
            spreadsheetMain.Document.SaveDocument("tmp.xlsx");
            System.Diagnostics.Process.Start("tmp.xlsx");
        }

        private void XSheetDesigner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Show();
                this.Owner.Refresh();
            }
        }

        private void btn_Dashboard_Click(object sender, EventArgs e)
        {
            control.dashboard_Click(sender, e);
        }

        private void bitDashBoardDesign_ItemClick(object sender, ItemClickEventArgs e)
        {
            DashboardDesigner form = new DashboardDesigner();
        }

        private void bitSnap_ItemClick(object sender, ItemClickEventArgs e)
        {
            SnapForm form = new SnapForm();
            
        }

        private void spreadsheetMain_DoubleClick(object sender, EventArgs e)
        {
            //control.EventCall(SysEvent.Btn_Edit, 0);
        }

        private void spreadsheetMain_CellBeginEdit(object sender, SpreadsheetCellCancelEventArgs e)
        {
            //control.EventCall(SysEvent.Btn_Edit, 0);
        }
    }
}
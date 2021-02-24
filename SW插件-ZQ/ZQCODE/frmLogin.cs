using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EPDM.Interop.epdm;
using EPDM.Interop.EPDMResultCode;

namespace ZQCode
{
    public partial class frmLogin : Form
    {
        //全局变量
        DrawingService objDrawingService = new DrawingService();
        MaterialService objMaterialService = new MaterialService();

        public frmLogin()
        {
            InitializeComponent();
            Globals.FileName = swAppHelper.GetFileName();
            //Globals.FileName = @"D:\PDM\001\PART001.sldprt";//测试数据
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            #region 【1】插件版本控制
            //20200523
            string serverVersion = objMaterialService.GetSWCodeVersion("SWCODE-ADDIN");
            try
            {
                if (Globals.DEF_PROGRAM_VERSION != serverVersion)
                {
                    Msg.ShowInformation("此版本需要更新，请到PDM系统【PDM附件中】下载最新版：\r\n版本号：" + serverVersion);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Msg.ShowError(ex.Message);
                this.Close();
            }
            //检查通过自动登录
            this.btnCheck_Click(null, null);
            #endregion
        }

        //检查
        private void btnCheck_Click(object sender, EventArgs e)
        {
            #region （三步验证）文件名、PDM登录、PDM内【暂无：检出状态、已有编码】
            try
            {
                //验证1：检查是否正常获取文件名
                if (Globals.FileName == null || Globals.FileName.Length == 0)
                {
                    this.lblErrorInfo.Text = "该文件没有保存，无法获取文件名！";
                    Msg.ShowError(this.lblErrorInfo.Text);
                    return;
                }
                //验证2：检查是否登录PDM系统
                if (!PDMHelper.IsLoginPDM())
                {
                    this.lblErrorInfo.Text = "PDM未正常登录！";
                    Msg.ShowError(this.lblErrorInfo.Text);
                    return;
                }
                //验证3：检查文件是否在PDM中
                if (!CheckInPDM(Globals.FileName))
                {
                    this.lblErrorInfo.Text = "该文件没有保存到PDM中！";
                    Msg.ShowError(this.lblErrorInfo.Text);
                    return;
                }
                //验证4：检查文件是否在PDM中检出（暂时不验证）
                //if (!PDMHelper.IsLock(Globals.FileName))
                //{
                //    this.lblErrorInfo.Text = "该文件没有在PDM中检出！";
                //    return;
                //}
                //验证5：检查文件ID是否已经有图号信息，如果有则覆盖
                //if (ExistedMaterialCode())
                //{
                //    this.lblErrorInfo.Text = "该文件的图号编码已经存在！";
                //    return;
                //}
            }
            catch (Exception ex)
            {
                this.lblErrorInfo.ForeColor = Color.Red;
                this.lblErrorInfo.Text = "PDM未正常登录！";
                Msg.ShowError(ex.Message);
                return;
            }
            this.DialogResult = DialogResult.OK;
            #endregion
        }

        /// <summary>
        /// 检查文件是否在PDM中
        /// </summary>
        private bool CheckInPDM(string fileName)
        {
            //PDM根目录
            string rootpath = PDMHelper.LoginPDM();
            if (rootpath.Length > 0 && fileName.Length > 0)
            {
                if (fileName.Contains(rootpath))//判断文件是否是PDM中文件
                {
                    //获取文件在PDM中的ID
                    Globals.FileID = PDMHelper.GetFileID(fileName).ToString();
                    return true;
                }
                else
                {
                    this.lblErrorInfo.Text = "请先将文件保存至PDM!";
                    return false;
                }
            }
            else
            {
                this.lblErrorInfo.Text = "请先将文件保存至PDM!";
                return false;
            }
        }

        /// <summary>
        /// 判断是否已经存在的图号信息
        /// </summary>
        /// <returns></returns>
        private bool ExistedMaterialCode()
        {
            ////判断是否文件ID已经有图号编码信息（20190306添加，测试ID：34231）
            //DrawingModel objDrawing = objDrawingService.GetFileDrawingInfo(Globals.FileID);
            //if (objDrawing != null)
            //{
            //    StringBuilder strBuilder = new StringBuilder().Append("此文件在PDM已有图号信息如下：");
            //    strBuilder.Append("\r\n图号\t{0}");
            //    strBuilder.Append("\r\n名称\t{1}");
            //    strBuilder.Append("\r\n型号规格\t{2}");
            //    strBuilder.Append("\r\n文件ID\t{3}\r\n");
            //    strBuilder.Append("\r\n单击确定，上述信息将会自动写入模型...");
            //    string strInfo = string.Format(strBuilder.ToString(), objDrawing.DrawingCode, objDrawing.MaterialName, objDrawing.MaterialSpec, objDrawing.DocIdModel);
            //    DialogResult result = MessageBox.Show(strInfo, "此文件无需申请编码", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    if (result == DialogResult.OK)
            //    {
            //        swAppHelper.UpdateProperty(objDrawing);
            //    }
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}

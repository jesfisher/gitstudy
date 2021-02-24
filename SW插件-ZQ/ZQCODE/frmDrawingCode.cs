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
    public partial class frmDrawingCode : Form
    {
        #region 全局变量和初始化
        DrawingService objDrawingService = new DrawingService();
        IEdmVault11 vault;

        /// <summary>
        /// 构造方法
        /// </summary>
        public frmDrawingCode()
        {
            InitializeComponent();
            try
            {
                #region 下拉框数据初始化
                this.cboDrawingClass.SelectedIndexChanged -= new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
                //初始化图号分类下拉框
                List<DrawingClassModel> dclasslist = objDrawingService.GetDrawingClass();
                this.cboDrawingClass.DataSource = dclasslist;
                this.cboDrawingClass.ValueMember = "DrawingClassId";
                this.cboDrawingClass.DisplayMember = "DrawingClassName";
                this.cboDrawingClass.SelectedIndex = -1;
                //初始化主单位下拉框
                List<UnitModel> unitlist = objDrawingService.GetUnit();
                this.cboUnit.DataSource = unitlist;
                this.cboUnit.ValueMember = "UnitCode";
                this.cboUnit.DisplayMember = "UnitName";
                this.cboUnit.SelectedIndex = -1;
                ////初始化采购分类下拉框
                //List<PurchaseTypeModel> purchaselist = objDrawingService.GetPurchaseType();
                //this.cboPurchaseType.DataSource = purchaselist;
                //this.cboPurchaseType.ValueMember = "PurchaseTypeId";
                //this.cboPurchaseType.DisplayMember = "PurchaseTypeName";
                //this.cboPurchaseType.SelectedIndex = -1;
                //初始化选型分类下拉框
                List<SelectionTypeModel> selectionlist = objDrawingService.GetSelectionType();
                this.cboSelectionType.DataSource = selectionlist;
                this.cboSelectionType.ValueMember = "SelectionTypeId";
                this.cboSelectionType.DisplayMember = "SelectionTypeName";
                this.cboSelectionType.SelectedIndex = -1;
                this.cboDrawingClass.SelectedIndexChanged += new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
                #endregion
                //获取文件名及文件ID
                Globals.FileName = swAppHelper.GetFileName();
                Globals.FileID = PDMHelper.GetFileID(Globals.FileName).ToString();
                this.btnAddMaterial.Enabled = true;//启动提交数据按钮
                ShowPDMInfo();
                DefaultWorking();
            }
            catch (Exception ex)
            {
                Msg.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 显示PDM视图
        /// </summary>
        private void ShowPDMInfo()
        {
            try
            {
                IEdmVault5 vault1 = new EdmVault5();
                vault = (IEdmVault11)vault1;
                EdmViewInfo[] Views = null;
                vault.GetVaultViews(out Views, false);
                VaultsComboBox.Items.Clear();
                this.lblUserName.Text = "";
                this.VaultsComboBox.Items.Add(Globals.VaultName);
                this.VaultsComboBox.Text = Globals.VaultName;
                this.lblVaultName.Text = string.Format("PDM库：{0}({1})    ", Globals.VaultName, Globals.DbServerIP);
                //获取并显示PDM登录信息
                GetPDMLoginInfo();
                this.Text = Globals.DEF_PROGRAM_NAME + " UserId=" + Globals.UserID;//程序名称
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                MessageBox.Show("HRESULT = 0x" + ex.ErrorCode.ToString("X") + " " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 初始化操作及各种条件判断
        /// </summary>
        private void DefaultWorking()
        {
            //【1】获取基本信息：文件ID、规格型号(配置名)           
            string configName = swAppHelper.GetConfigName();
            Globals.ConfigName = configName;
            //【2】输入两个参数(文件ID、规格型号)判断图号申请类型(1-新图号生成；2-旧图号保持；3-新规格生成)
            List<string> requestList = objDrawingService.GetDrawingRequestStyle(Globals.FileID, Globals.ConfigName);
            Globals.DrawingID = requestList[0];
            Globals.RequestStyle = requestList[2];
            switch (Globals.RequestStyle)
            {
                case "1":
                    //1-新图号申请，则从SW模型中读取属性
                    Globals.CurrentDrawing = objDrawingService.GetDrawingFromSW();
                    this.lblInfo.Text = "新图号申请：从模型读取属性...";
                    break;
                case "2":
                    //2-旧图号保持，则从SQL中读取属性
                    Globals.CurrentDrawing = objDrawingService.GetDrawingFromSQL(Globals.DrawingID);
                    this.cboDrawingClass.Enabled = false;//图号分类不允许更改
                    this.txtDrawingCode.Enabled = false;//图号不允许更改
                    this.txtMaterialName.Enabled = false;//名称不允许更改
                    this.txtMaterialSpec.Enabled = false;//规格型号不允许更改
                    this.lblInfo.Text = "旧图号保持：从数据库读取属性...";
                    Msg.ShowInformation("该图号修改原有属性值！");
                    break;
                case "3":
                    //3-新规格申请，则从SQL中读取属性
                    Globals.CurrentDrawing = objDrawingService.GetDrawingFromSQL(Globals.DrawingID);
                    this.cboDrawingClass.Enabled = false;//图号分类不允许更改
                    this.txtDrawingCode.Enabled = false;//图号不允许更改
                    this.txtMaterialName.Enabled = false;//名称不允许更改
                    this.txtMaterialSpec.Enabled = false;//规格型号不允许更改
                    Globals.DrawingID = string.Empty;//将系统DrawingId设置为null
                    this.lblInfo.Text = "新规格申请：从数据库读取属性...";
                    Msg.ShowInformation("该图号申请新的规格型号！");
                    break;
                default:
                    break;
            }
            //【3】在窗体中显示默认值(From：Globals.CurrentDrawing)
            this.txtDocumentId.Text = Globals.FileID;//文件ID
            this.txtDrawingId.Text = Globals.DrawingID;//图号ID
            this.txtConfigName.Text = Globals.ConfigName;//当前配置
            this.txtDocumentId.Enabled = false;
            this.txtDrawingId.Enabled = false;
            this.txtConfigName.Enabled = false;
            this.cboDrawingClass.SelectedIndexChanged -= new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
            if (Globals.CurrentDrawing.DrawingClassId != null)
            {
                this.cboDrawingClass.SelectedValue = Globals.CurrentDrawing.DrawingClassId;//图号分类
            }
            this.txtDrawingCode.Text = Globals.CurrentDrawing.DrawingCode;//图号
            this.txtMaterialName.Text = Globals.CurrentDrawing.MaterialName;//名称
            this.txtMaterialSpec.Text = Globals.ConfigName;//规格型号=当前配置名
            this.cboUnit.SelectedValue = Globals.CurrentDrawing.Unit;//单位
            this.cboPurchaseType.SelectedValue = Globals.CurrentDrawing.PurchaseTypeId;
            this.cboSelectionType.SelectedValue = Globals.CurrentDrawing.SelectionTypeId;
            this.txtWeight.Text = Globals.CurrentDrawing.Weight.ToString();
            this.txtHeatTreatment.Text = Globals.CurrentDrawing.HeatTreatment;
            this.txtSurfaceTreatment.Text = Globals.CurrentDrawing.SurfaceTreatment;
            this.txtBrand.Text = Globals.CurrentDrawing.Brand;
            this.txtReMark.Text = Globals.CurrentDrawing.ReMark;
            this.cboDrawingClass.SelectedIndexChanged += new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
        }

        /// <summary>
        /// 切换图号类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboDrawingClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classValue = cboDrawingClass.SelectedValue.ToString();
            if (classValue == "G")
            {
                this.txtDrawingCode.Enabled = true;//图号可以更改
                this.txtMaterialSpec.Enabled = false;//规格型号不允许更改
                this.txtDrawingCode.SelectAll();
                this.lblInfo.Text = "企标件：图号需要手动填写...";
            }
            else
            {
                this.txtDrawingCode.Text = objDrawingService.GetDrawingCode(classValue);//自动流水
                this.txtDrawingCode.Enabled = false;//图号不允许更改
                this.txtMaterialSpec.Enabled = false;//规格型号不允许更改
                this.lblInfo.Text = "图号流水码自动生成...";
            }
            if (classValue == "A")
            {
                this.cboPurchaseType.SelectedValue = "199";//默认显示：无
                this.cboSelectionType.SelectedValue = "299";//默认显示：无
                this.lblInfo.Text = "产品图：采购分类和选型分类默认是[无]...";
            }
        }

        #endregion

        #region 封装图号属性并提交到编码数据库
        //提交数据（新增或修改）
        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            //【1】数据填写完整性验证（非空/图号查重）
            if (!ValidatingData()) return;
            //图号
            string drawingCode = this.txtDrawingCode.Text.Trim();
            //规格型号
            string materialSpec = this.txtMaterialSpec.Text.Trim();
            //【2】根据三种情况判断，新增图号、新增规格或修改图号
            switch (Globals.RequestStyle)
            {
                case "1":
                    //1-新图号申请
                    //★判断图号是否存在(DrawingCode)★
                    if (objDrawingService.IsExistDrawingCode(drawingCode)) return;
                    //执行新增图号操作（默认）
                    AddDrawing();
                    break;
                case "2":
                    //2-旧图号保持
                    //执行修改图号操作
                    ModifyDrawing(Globals.DrawingID);
                    break;
                case "3":
                    //3-新规格申请
                    //★判断图号+规格是否已经存在★
                    if (!objDrawingService.IsExistDrawingCodeAndMaterialSpec(drawingCode, materialSpec))
                    {
                        //执行新增图号操作（默认）
                        AddDrawing();
                    }
                    break;
                default:
                    break;
            }
        }

        //新增图号
        private void AddDrawing()
        {
            #region 【1】（新增）封装属性并保存到模型中
            //1-封装图号对象
            DrawingModel objDrawing = new DrawingModel()
            {
                DrawingClassId = this.cboDrawingClass.SelectedValue == null ? null : this.cboDrawingClass.SelectedValue.ToString(),
                DrawingCode = this.txtDrawingCode.Text.Trim(),
                MaterialName = this.txtMaterialName.Text.Trim(),
                MaterialSpec = this.txtMaterialSpec.Text.Trim(),
                Unit = this.cboUnit.SelectedValue == null ? null : this.cboUnit.SelectedValue.ToString(),
                PurchaseTypeId = this.cboPurchaseType.SelectedValue == null ? null : this.cboPurchaseType.SelectedValue.ToString(),
                SelectionTypeId = this.cboSelectionType.SelectedValue == null ? null : this.cboSelectionType.SelectedValue.ToString(),
                HeatTreatment = this.txtHeatTreatment.Text.Trim(),
                SurfaceTreatment = this.txtSurfaceTreatment.Text.Trim(),
                Revision = 1,
                DrawingStatusId = 0,//申请状态-0
                Weight = ConvertEx.ToFloat(this.txtWeight.Text.Trim()).ToString("F2"),
                Brand = this.txtBrand.Text.Trim(),
                ReMark = this.txtReMark.Text.Trim(),
                DocIdModel = ConvertEx.ToInt(Globals.FileID),
                CreateFrom = "来自SW插件",
                CreateId = Globals.DEF_CreateId,
                CreateUser = Globals.DEF_CreateUser,
                CreateDate = objDrawingService.GetDBServerTime(),
                CreateInfo = Globals.DEF_CreateInfo
            };
            //2-无论数据库是否保存，都将填写的内容更新到本模型属性中            
            swAppHelper.UpdateProperty(objDrawing);
            //Msg.ShowInformation("文件属性已更新并保存！");
            Globals.CurrentDrawing = objDrawing;//给全局变量赋值                
            #endregion

            #region 【2】（新增）将数据提交到数据库
            try
            {
                if (objDrawingService.AddDrawing(objDrawing))
                {
                    this.DialogResult = DialogResult.OK;
                    Msg.ShowInformation("添加成功，请保存文件！");
                    //只允许提交一次
                    this.btnAddMaterial.Enabled = false;
                }
                else
                {
                    Msg.ShowError("添加失败！");
                }
            }
            catch (Exception ex)
            {
                Msg.ShowError(ex.Message);
            }
            #endregion
        }

        //修改图号
        private void ModifyDrawing(string drawingId)
        {
            //询问是否修改现有图号信息
            if (Msg.AskQuestion("该图号信息已经存在，需要修改吗？"))
            {
                #region 【1】（修改）封装属性并保存到模型中
                //1-封装图号对象
                DrawingModel objDrawing = new DrawingModel()
                {
                    DrawingId = drawingId,
                    DrawingClassId = this.cboDrawingClass.SelectedValue == null ? null : this.cboDrawingClass.SelectedValue.ToString(),
                    DrawingCode = this.txtDrawingCode.Text.Trim(),
                    MaterialName = this.txtMaterialName.Text.Trim(),
                    MaterialSpec = this.txtMaterialSpec.Text.Trim(),
                    Unit = this.cboUnit.SelectedValue == null ? null : this.cboUnit.SelectedValue.ToString(),
                    PurchaseTypeId = this.cboPurchaseType.SelectedValue == null ? null : this.cboPurchaseType.SelectedValue.ToString(),
                    SelectionTypeId = this.cboSelectionType.SelectedValue == null ? null : this.cboSelectionType.SelectedValue.ToString(),
                    HeatTreatment = this.txtHeatTreatment.Text.Trim(),
                    SurfaceTreatment = this.txtSurfaceTreatment.Text.Trim(),
                    Weight = ConvertEx.ToFloat(this.txtWeight.Text.Trim()).ToString("F2"),
                    Brand = this.txtBrand.Text.Trim(),
                    ReMark = this.txtReMark.Text.Trim(),
                    //修改时增加更改人信息
                    UpdateId = Globals.DEF_CreateId,
                    UpdateUser = Globals.DEF_CreateUser,
                    UpdateDate = objDrawingService.GetDBServerTime(),
                    UpdateInfo = Globals.DEF_CreateInfo
                    //修改时不改变原来的信息(版本、状态、文件ID等)
                    //Revision = 1,
                    //DrawingStatusId = 0,//申请状态-0
                    //DocIdModel = ConvertEx.ToInt(Globals.FileID),
                    //CreateFrom = "来自SW插件",
                    //CreateId = Globals.DEF_CreateId,
                    //CreateUser = Globals.DEF_CreateUser,
                    //CreateDate = objDrawingService.GetDBServerTime(),
                    //CreateInfo = Globals.DEF_CreateInfo
                };
                //2-无论数据库是否保存，都将填写的内容更新到本模型属性中            
                swAppHelper.UpdateProperty(objDrawing);
                //Msg.ShowInformation("文件属性已更新并保存！");
                Globals.CurrentDrawing = objDrawing;//给全局变量赋值                
                #endregion

                #region 【2】（修改）将数据提交到数据库
                try
                {
                    if (objDrawingService.ModifyMaterial(objDrawing))
                    {
                        this.DialogResult = DialogResult.OK;
                        Msg.ShowInformation("修改成功，请保存文件！");
                        //只允许提交一次
                        this.btnAddMaterial.Enabled = false;
                    }
                    else
                    {
                        Msg.ShowError("修改失败！");
                    }
                }
                catch (Exception ex)
                {
                    Msg.ShowError(ex.Message);
                }
                #endregion
            }
            else
            {
                return;
            }
        }

        #region 【1】数据填写完整性验证（非空验证）
        //检查主表数据是否完整或合法
        private bool ValidatingData()
        {
            //判断PDM用户名是否包含在CODE系统用户中
            if (!objDrawingService.IsExistUserName(Globals.DEF_CreateId))
            {
                string info = string.Format("PDM用户名({0})在编码系统中不存在！", Globals.DEF_CreateId);
                Msg.Warning(info);
                return false;
            }
            //PDM属性验证（共有验证项目）
            if (cboDrawingClass.SelectedIndex == -1)
            {
                Msg.Warning("图号类别不能为空!");
                cboDrawingClass.Focus();
                return false;
            }
            if (txtDrawingCode.Text == string.Empty)
            {
                Msg.Warning("图号不能为空!");
                txtDrawingCode.Focus();
                return false;
            }
            if (txtMaterialName.Text == string.Empty)
            {
                Msg.Warning("名称不能为空!");
                txtMaterialName.Focus();
                return false;
            }
            if (txtMaterialSpec.Text == string.Empty)
            {
                Msg.Warning("规格型号不能为空!");
                txtMaterialSpec.Focus();
                return false;
            }
            if (cboUnit.SelectedIndex == -1)
            {
                Msg.Warning("主单位不能为空!");
                cboUnit.Focus();
                return false;
            }
            if (cboPurchaseType.SelectedIndex == -1)
            {
                Msg.Warning("采购分类不能为空!");
                cboPurchaseType.Focus();
                return false;
            }
            if (cboSelectionType.SelectedIndex == -1)
            {
                Msg.Warning("选型分类不能为空!");
                cboSelectionType.Focus();
                return false;
            }
            if (Globals.FileID.Length == 0)
            {
                Msg.Warning("未获取PDM文件ID!");
                return false;
            }
            return true;
        }

        #endregion

        #region 【3】PDM、文件、编码三步验证
        //登录PDM库视图
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PDMHelper.IsLoginPDM())
                {
                    Msg.ShowInformation("请先登录PDM系统！");
                    return;
                }
                else
                {
                    //获取并显示PDM登录信息
                    GetPDMLoginInfo();
                }
            }
            catch (Exception ex)
            {
                this.lblUserName.ForeColor = Color.Red;
                this.lblUserName.Text = "PDM未正常登录！";
                Msg.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 获取并显示PDM登录信息
        /// </summary>
        private void GetPDMLoginInfo()
        {
            //获取登录信息
            Globals.UserID = PDMHelper.GetLoginID();
            Globals.UserName = PDMHelper.GetLoginFullName();
            //显示登录信息
            this.lblUserName.ForeColor = Color.Black;
            this.lblUserName.Text = string.Format("当前用户：{0}({1})", Globals.DEF_CreateId, Globals.UserName);
        }
        #endregion

        #endregion

        #region 体验优化（更新重量、自动填写、清除数据）
        /// <summary>
        /// 更新重量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWeight_Click(object sender, EventArgs e)
        {
            DrawingModel objModel = objDrawingService.GetDrawingFromSW();
            this.txtWeight.Clear();
            this.txtWeight.Text = objModel.Weight;
        }

        //清除数据
        private void btnClear_Click(object sender, EventArgs e)
        {
            //EnableAllControls();
            this.cboDrawingClass.SelectedIndexChanged -= new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
            foreach (Control item in this.splitContainer1.Panel2.Controls)
            {
                if (item is TextBox)
                {
                    ((TextBox)item).Clear();
                }
                if (item is ComboBox)
                {
                    ((ComboBox)item).SelectedIndex = -1;
                }
            }
            this.VaultsComboBox.SelectedIndex = 0;
            this.lblUserName.Text = "";
            this.txtDocumentId.Text = Globals.FileID;//还原文件ID
            this.cboDrawingClass.SelectedIndexChanged += new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
        }
        //启用所有控件
        private void EnableAllControls()
        {
            foreach (Control item in this.splitContainer1.Panel2.Controls)
            {
                item.Enabled = true;
            }
        }
        #endregion

    }
}

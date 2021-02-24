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
    public partial class frmMaterialCode : Form
    {
        #region 全局变量和初始化
        DrawingService objDrawingService = new DrawingService();
        MaterialService objMaterialService = new MaterialService();
        IEdmVault11 vault;

        /// <summary>
        /// 构造方法
        /// </summary>
        public frmMaterialCode()
        {
            InitializeComponent();
            try
            {
                #region 下拉框数据初始化
                //this.cboDrawingClass.SelectedIndexChanged -= new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
                //料件类别下拉框
                List<MaterialCategoryModel> mcategoryList = objMaterialService.GetMaterialCategory();
                this.cboMaterialCategoryId.DataSource = mcategoryList;
                this.cboMaterialCategoryId.ValueMember = "MaterialCategoryId";
                this.cboMaterialCategoryId.DisplayMember = "MaterialCategoryName";
                this.cboMaterialCategoryId.SelectedIndex = -1;
                //单位下拉框
                List<UnitModel> unitList = objMaterialService.GetUnit();
                this.cboUnit.DataSource = unitList;
                this.cboUnit.ValueMember = "Unit";
                this.cboUnit.DisplayMember = "UnitName";
                this.cboUnit.SelectedIndex = -1;
                //加工属性表下拉框
                List<MachiningPropertyModel> mpropertyList = objMaterialService.GetMachiningProperty();
                this.cboMachiningProperty.DataSource = mpropertyList;
                this.cboMachiningProperty.ValueMember = "MachiningPropertyId";
                this.cboMachiningProperty.DisplayMember = "MachiningPropertyName";
                this.cboMachiningProperty.SelectedIndex = -1;
                //涂装颜色
                List<PaintingColorModel> paintingcolorList = objMaterialService.GetPaintingColor();
                this.cboPaintingColor.DataSource = paintingcolorList;
                this.cboPaintingColor.ValueMember = "PaintingColorId";
                this.cboPaintingColor.DisplayMember = "PaintingColorId";
                this.cboPaintingColor.SelectedIndex = -1;

                //品牌编号
                List<BrandModel> brandList = objMaterialService.GetBrand();
                this.cboBrandId.DataSource = brandList;
                this.cboBrandId.ValueMember = "BrandId";
                this.cboBrandId.DisplayMember = "BrandName";
                this.cboBrandId.SelectedIndex = -1;

                //图样特征代号
                List<DraftFeatureModel> draftfeatureList = objMaterialService.GetDraftFeature();
                this.cboDraftFeatureId.DataSource = draftfeatureList;
                this.cboDraftFeatureId.ValueMember = "DraftFeatureId";
                this.cboDraftFeatureId.DisplayMember = "DraftFeatureName";
                this.cboDraftFeatureId.SelectedIndex = -1;
                #endregion
                InitializeGlobalsVariables();
                ShowPDMInfo();
                DefaultWorking();
                this.tsbAddMaterial.Enabled = true;//启动提交数据按钮
            }
            catch (Exception ex)
            {
                Msg.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 初始化全局变量(即Globals中的变量)
        /// </summary>
        private void InitializeGlobalsVariables()
        {
            //获取SolidWorks文件名及文件ID
            Globals.FileName = swAppHelper.GetFileName();
            Globals.FileID = PDMHelper.GetFileID(Globals.FileName).ToString();
            //获取PDM登录信息
            Globals.PDM_UserID = PDMHelper.GetLoginID();
            Globals.PDM_UserAccount = PDMHelper.GetLoginUserName();
            Globals.PDM_UserFullName = PDMHelper.GetLoginFullName();
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
                this.Text = Globals.DEF_PROGRAM_NAME + " UserId=" + Globals.PDM_UserID;//程序名称
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
            //【2】输入两个参数(文件ID、工厂编码)判断图号申请类型(1-新图号生成；2-旧图号保持)
            //Globals.RequestStyle = objMaterialService.GetDrawingRequestStyle(Globals.FileID, Globals.FactoryCode);
            DataRow dr = objMaterialService.GetMaterialInfo(Globals.FileID, Globals.FactoryCode);
            if (dr == null)
            {
                //1-新物料申请，则从SW模型中读取属性
                Globals.RequestStyle = 1;
                Globals.CurrentMaterial = objMaterialService.GetMaterialFromSW();
                this.lblInfo.Text = "新图号申请：从模型读取属性...";
            }
            else
            {
                //2-旧物料保持，则从SQL中读取属性
                Globals.RequestStyle = 2;
                Globals.CurrentMaterial = objMaterialService.GetMaterialFromSQL();
                this.btnSelectMClass.Enabled = false;//物料编码分类不允许更改
                this.txtDrawingCode.Enabled = false;//图号不允许更改
                this.lblInfo.Text = "旧图号保持：从数据库读取属性...";
                Msg.ShowInformation("该图号修改原有属性值！");
            }
            //【3】在窗体中显示默认值(From：Globals.CurrentMaterial)
            this.txtDocumentId.Text = Globals.FileID;//文件ID  
            this.txtMaterialId.Text = Globals.CurrentMaterial.MaterialId;
            this.txtIsPublic.Text = Globals.CurrentMaterial.IsPublic ? "1" : "0";
            this.txtMaterialCode.Text = Globals.CurrentMaterial.MaterialCode;
            this.txtFactoryCode.Text = Globals.FactoryCode;
            this.txtMaterialClassId.Text = Globals.CurrentMaterial.MaterialClassId;//物料编码分类
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.MaterialCategoryId)) this.cboMaterialCategoryId.SelectedValue = Globals.CurrentMaterial.MaterialCategoryId;
            this.txtDrawingCode.Text = Globals.CurrentMaterial.DrawingCode;//图号
            this.txtMaterialName.Text = Globals.CurrentMaterial.MaterialName;//名称
            this.txtMaterialSpec.Text = Globals.CurrentMaterial.MaterialSpec;//规格
            this.txtMaterialType.Text = Globals.CurrentMaterial.MaterialType;//型号
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.Unit)) this.cboUnit.SelectedValue = Globals.CurrentMaterial.Unit;//单位
            this.txtMquality.Text = Globals.CurrentMaterial.Mquality;
            this.txtWeight.Text = Globals.CurrentMaterial.Weight;
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.MachiningPropertyId)) this.cboMachiningProperty.SelectedValue = Globals.CurrentMaterial.MachiningPropertyId;
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.PaintingColor)) this.cboPaintingColor.SelectedValue = Globals.CurrentMaterial.PaintingColor;
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.BrandId)) this.cboBrandId.SelectedValue = Globals.CurrentMaterial.BrandId;
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.ImportanceGrade)) this.cboImportanceGrade.Text = Globals.CurrentMaterial.ImportanceGrade;
            if (!string.IsNullOrEmpty(Globals.CurrentMaterial.DraftFeatureId)) this.cboDraftFeatureId.SelectedValue = Globals.CurrentMaterial.DraftFeatureId;
            this.txtHeatTreatment.Text = Globals.CurrentMaterial.HeatTreatment;
            this.txtSurfaceTreatment.Text = Globals.CurrentMaterial.SurfaceTreatment;
            this.txtReMark.Text = Globals.CurrentMaterial.ReMark;
        }

        #endregion

        #region 封装图号属性并提交到编码数据库
        //提交数据（新增或修改）
        private void tsbAddMaterial_Click(object sender, EventArgs e)
        {
            //【1】数据填写完整性验证（非空/图号查重）
            if (!ValidatingData()) return;
            //图号
            string drawingCode = this.txtDrawingCode.Text.Trim();
            //【2】新增物料编码或修改物料编码
            switch (Globals.RequestStyle)
            {
                case 1:
                    //1-新物料编码申请
                    //★判断图号是否存在(DrawingCode)★
                    if (objMaterialService.IsExistDrawingCode(drawingCode))
                    {
                        Msg.ShowError("物料编码或图号已经存在！");
                        return;
                    }
                    //执行新增图号操作（默认）
                    AddDrawing();
                    break;
                case 2:
                    //2-旧物料编码保持
                    //执行修改图号操作
                    ModifyDrawing(Globals.MaterialID);
                    break;
                default:
                    break;
            }
        }

        //新增图号（物料编码）
        private void AddDrawing()
        {
            #region 【1】（新增）封装属性并保存到模型中
            //1-封装图号对象
            MaterialModel objMaterial = new MaterialModel()
            {
                DocIdModel = ConvertEx.ToInt(Globals.FileID),
                MaterialId = this.txtMaterialId.Text.Trim(),
                IsPublic = ConvertEx.ToBoolean(this.txtIsPublic.Text.Trim()),
                MaterialCode = this.txtMaterialCode.Text.Trim(),
                FactoryCode = this.txtFactoryCode.Text.Trim(),
                MaterialClassId = this.txtMaterialClassId.Text.Trim(),
                MaterialCategoryId = this.cboMaterialCategoryId.SelectedValue.ToString(),
                DrawingCode = this.txtDrawingCode.Text.Trim(),
                MaterialName = this.txtMaterialName.Text.Trim(),
                MaterialSpec = this.txtMaterialSpec.Text.Trim(),
                MaterialType = this.txtMaterialType.Text.Trim(),
                Unit = this.cboUnit.SelectedValue.ToString(),
                Mquality = this.txtMquality.Text.Trim(),
                Weight = ConvertEx.ToFloat(this.txtWeight.Text.Trim()).ToString("F2"),
                MachiningPropertyId = this.cboMachiningProperty.SelectedValue.ToString(),
                PaintingColor = this.cboPaintingColor.SelectedValue == null ? null : this.cboPaintingColor.SelectedValue.ToString(),
                BrandId = this.cboBrandId.SelectedValue == null ? null : this.cboBrandId.SelectedValue.ToString(),
                ImportanceGrade = this.cboImportanceGrade.Text,
                DraftFeatureId = this.cboDraftFeatureId.SelectedValue == null ? null : this.cboDraftFeatureId.SelectedValue.ToString(),
                HeatTreatment = this.txtHeatTreatment.Text.Trim(),
                SurfaceTreatment = this.txtSurfaceTreatment.Text.Trim(),
                ReMark = this.txtReMark.Text.Trim(),
                Revision = 1,
                StatusId = 0,//申请状态-0
                CreateFrom = "来自SW插件",
                CreateId = Globals.DEF_CreateId,
                CreateUser = Globals.DEF_CreateUser,
                CreateDate = objDrawingService.GetDBServerTime(),
                CreateInfo = Globals.DEF_CreateInfo
            };
            //★★保存之前从数据库获取最新流水码★★
            objMaterial.MaterialCode = objMaterialService.GetNewMaterialCode(objMaterial.MaterialClassId);
            //2-无论数据库是否保存，都将填写的内容更新到本模型属性中            
            swAppHelper.UpdateProperty(objMaterial);
            //Msg.ShowInformation("文件属性已更新并保存！");
            Globals.CurrentMaterial = objMaterial;//给全局变量赋值                
            #endregion

            #region 【2】（新增）将数据提交到数据库
            try
            {
                if (objMaterialService.AddMaterial(objMaterial))
                {
                    this.DialogResult = DialogResult.OK;
                    Msg.ShowInformation("添加成功，请保存文件！");
                    //只允许提交一次
                    this.tsbAddMaterial.Enabled = false;
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
                MaterialModel objMaterial = new MaterialModel()
                {
                    MaterialId = this.txtMaterialId.Text.Trim(),
                    MaterialCategoryId = this.cboMaterialCategoryId.SelectedValue.ToString(),
                    MaterialName = this.txtMaterialName.Text.Trim(),
                    MaterialSpec = this.txtMaterialSpec.Text.Trim(),
                    MaterialType = this.txtMaterialType.Text.Trim(),
                    Unit = this.cboUnit.SelectedValue.ToString(),
                    Mquality = this.txtMquality.Text.Trim(),
                    Weight = ConvertEx.ToFloat(this.txtWeight.Text.Trim()).ToString("F2"),
                    MachiningPropertyId = this.cboMachiningProperty.SelectedValue.ToString(),
                    PaintingColor = this.cboPaintingColor.SelectedValue == null ? null : this.cboPaintingColor.SelectedValue.ToString(),
                    BrandId = this.cboBrandId.SelectedValue == null ? null : this.cboBrandId.SelectedValue.ToString(),
                    ImportanceGrade = this.cboImportanceGrade.Text,
                    DraftFeatureId = this.cboDraftFeatureId.SelectedValue == null ? null : this.cboDraftFeatureId.SelectedValue.ToString(),
                    HeatTreatment = this.txtHeatTreatment.Text.Trim(),
                    SurfaceTreatment = this.txtSurfaceTreatment.Text.Trim(),
                    ReMark = this.txtReMark.Text.Trim(),
                    //修改时增加更改人信息
                    UpdateId = Globals.DEF_CreateId,
                    UpdateUser = Globals.DEF_CreateUser,
                    UpdateDate = objDrawingService.GetDBServerTime(),
                    UpdateInfo = Globals.DEF_CreateInfo,
                    //修改时不改变原来的信息(版本、状态、文件ID等)
                    MaterialCode = this.txtMaterialCode.Text.Trim(),
                    //Revision = 1,
                    //DrawingStatusId = 0,//申请状态-0
                    //DocIdModel = ConvertEx.ToInt(Globals.FileID),
                    //CreateFrom = "来自SW插件",
                    //CreateId = Globals.DEF_CreateId,
                    //CreateUser = Globals.DEF_CreateUser,
                    //CreateDate = objDrawingService.GetDBServerTime(),
                    //CreateInfo = Globals.DEF_CreateInfo
                };

                //Msg.ShowInformation(this.cboImportanceGrade.Text);
                //2-无论数据库是否保存，都将填写的内容更新到本模型属性中            
                swAppHelper.UpdateProperty(objMaterial);
                //Msg.ShowInformation("文件属性已更新并保存！");
                Globals.CurrentMaterial = objMaterial;//给全局变量赋值                
                #endregion

                #region 【2】（修改）将数据提交到数据库
                try
                {
                    if (objMaterialService.ModifyMaterial(objMaterial))
                    {
                        this.DialogResult = DialogResult.OK;
                        Msg.ShowInformation("修改成功，请保存文件！");
                        //只允许提交一次
                        this.tsbAddMaterial.Enabled = false;
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

        //#region 【1】数据填写完整性验证（非空验证）
        ////检查主表数据是否完整或合法
        //private bool ValidatingData()
        //{
        //    //判断PDM用户名是否包含在CODE系统用户中
        //    DataRow dr = objMaterialService.GetUserInfo(Globals.PDM_UserAccount);
        //    if (dr == null)
        //    {
        //        string info = string.Format("PDM用户名({0})在编码系统中不存在！", Globals.PDM_UserAccount);
        //        Msg.Warning(info);
        //        return false;
        //    }
        //    else
        //    {
        //        Globals.DEF_CreateId = dr["Account"].ToString();
        //        Globals.DEF_CreateUser = dr["UserName"].ToString();
        //    }
        //    ////PDM属性验证（共有验证项目）
        //    //if (cboDrawingClass.SelectedIndex == -1)
        //    //{
        //    //    Msg.Warning("图号类别不能为空!");
        //    //    cboDrawingClass.Focus();
        //    //    return false;
        //    //}
        //    if (txtDrawingCode.Text == string.Empty)
        //    {
        //        Msg.Warning("图号不能为空!");
        //        txtDrawingCode.Focus();
        //        return false;
        //    }
        //    if (txtMaterialName.Text == string.Empty)
        //    {
        //        Msg.Warning("名称不能为空!");
        //        txtMaterialName.Focus();
        //        return false;
        //    }
        //    if (txtMaterialSpec.Text == string.Empty)
        //    {
        //        Msg.Warning("规格型号不能为空!");
        //        txtMaterialSpec.Focus();
        //        return false;
        //    }
        //    if (cboUnit.SelectedIndex == -1)
        //    {
        //        Msg.Warning("主单位不能为空!");
        //        cboUnit.Focus();
        //        return false;
        //    }
        //    //if (cboPurchaseType.SelectedIndex == -1)
        //    //{
        //    //    Msg.Warning("采购分类不能为空!");
        //    //    cboPurchaseType.Focus();
        //    //    return false;
        //    //}
        //    //if (cboSelectionType.SelectedIndex == -1)
        //    //{
        //    //    Msg.Warning("选型分类不能为空!");
        //    //    cboSelectionType.Focus();
        //    //    return false;
        //    //}
        //    if (Globals.FileID.Length == 0)
        //    {
        //        Msg.Warning("未获取PDM文件ID!");
        //        return false;
        //    }
        //    return true;
        //}

        //#endregion

        #region 【1】数据填写完整性验证（非空验证）
        //检查主表数据是否完整或合法
        private bool ValidatingData()
        {
            //判断PDM用户名是否包含在CODE系统用户中
            DataRow dr = objMaterialService.GetUserInfo(Globals.PDM_UserAccount);
            if (dr == null)
            {
                string info = string.Format("PDM用户名({0})在编码系统中不存在！", Globals.PDM_UserAccount);
                Msg.Warning(info);
                return false;
            }
            else
            {
                Globals.DEF_CreateId = dr["Account"].ToString();
                Globals.DEF_CreateUser = dr["UserName"].ToString();
            }
            //数据非空验证
            if (Globals.FileID.Length == 0)
            {
                Msg.Warning("未获取PDM文件ID!");
                return false;
            }
            if (txtIsPublic.Text == string.Empty)
            {
                Msg.Warning("集团物料标识不能为空!");
                txtIsPublic.Focus();
                return false;
            }
            if (txtFactoryCode.Text == string.Empty)
            {
                Msg.Warning("事业部编码不能为空!");
                txtFactoryCode.Focus();
                return false;
            }
            if (txtMaterialClassId.Text == string.Empty)
            {
                Msg.Warning("物料分类不能为空!");
                txtMaterialClassId.Focus();
                return false;
            }
            if (txtMaterialClassId.Text.Trim().Length != 4)
            {
                Msg.Warning("物料分类不是4位整数!");
                txtMaterialClassId.Focus();
                return false;
            }
            if (txtMaterialCode.Text == string.Empty)
            {
                Msg.Warning("物料编码不能为空!");
                txtMaterialClassId.Focus();
                return false;
            }
            if (txtMaterialCode.Text.Trim().Length != 8)
            {
                Msg.Warning("物料编码不是8位整数!");
                txtMaterialClassId.Focus();
                return false;
            }
            if (cboMaterialCategoryId.SelectedIndex == -1)
            {
                Msg.Warning("料件类别不能为空!");
                cboMaterialCategoryId.Focus();
                return false;
            }
            //if (txtDrawingCode.Text == string.Empty)
            //{
            //    Msg.Warning("图号不能为空!");
            //    txtDrawingCode.Focus();
            //    return false;
            //}
            if (txtMaterialName.Text == string.Empty)
            {
                Msg.Warning("名称不能为空!");
                txtMaterialName.Focus();
                return false;
            }
            //if (txtMaterialType.Text == string.Empty)
            //{
            //    Msg.Warning("型号不能为空!");
            //    txtMaterialType.Focus();
            //    return false;
            //}
            if (cboUnit.SelectedIndex == -1)
            {
                Msg.Warning("主单位不能为空!");
                cboUnit.Focus();
                return false;
            }
            //根据MaterialClassId不同，提示不同的信息
            string materialclassId = txtMaterialClassId.Text.Trim();
            //【1】非空验证
            //2021年1月14日 04:32:14
            //2开头的"加工属性"不是必填项
            if (!materialclassId.StartsWith("2"))
            {
                if (cboMachiningProperty.SelectedIndex == -1)
                {
                    Msg.Warning("加工属性不能为空!");
                    cboMachiningProperty.Focus();
                    return false;
                }
            }
            //【2】查重验证
            if (CheckUnique(materialclassId)) return true;
            else return false;
        }

        /// <summary>
        /// 根据物料分类不同，验证不同的信息
        /// </summary>
        /// <param name="materialclassId"></param>
        /// <returns></returns>
        private bool CheckUnique(string materialclassId)
        {
            DataRow dr = null;
            string parameter1 = string.Empty;
            string parameter2 = string.Empty;
            string materialId = txtMaterialId.Text.Trim();
            string waringInfo = string.Empty;

            //2021年1月14日 04:32:14
            //2和3开头的"图号"和"型号"不是必填项
            if (materialclassId.StartsWith("2") || materialclassId.StartsWith("3"))//自制件3开头，图号DrawingCode+颜色PaintingColor
            {
                //if (txtDrawingCode.Text == string.Empty)
                //{
                //    Msg.Warning("自制件3开头：图号不能为空!");
                //    txtDrawingCode.Focus();
                //    return false;
                //}
                //parameter1 = txtDrawingCode.Text.Trim();
                //if (cboPaintingColor.SelectedIndex != -1)
                //{
                //    parameter2 = cboPaintingColor.SelectedValue.ToString();
                //}
                return true;
            }
            else if (materialclassId.StartsWith("50") || materialclassId.StartsWith("51"))//外购件50、51开头，型号MaterialType+品牌BrandId
            {
                if (txtMaterialType.Text == string.Empty)
                {
                    Msg.Warning("外购件50、51开头：型号不能为空!");
                    txtMaterialType.Focus();
                    return false;
                }
                if (cboBrandId.SelectedIndex == -1)
                {
                    Msg.Warning("外购件50、51开头：品牌不能为空!");
                    cboBrandId.Focus();
                    return false;
                }
                parameter1 = txtMaterialType.Text.Trim();
                parameter2 = cboBrandId.SelectedValue.ToString();
            }
            else if (materialclassId.StartsWith("52"))//标准件52开头，型号MaterialType+名称MaterialName
            {
                if (txtMaterialType.Text == string.Empty)
                {
                    Msg.Warning("标准件52开头：型号不能为空!");
                    txtMaterialType.Focus();
                    return false;
                }
                if (txtMaterialName.Text == string.Empty)
                {
                    Msg.Warning("标准件52开头：名称不能为空!");
                    txtMaterialName.Focus();
                    return false;
                }
                parameter1 = txtMaterialType.Text.Trim();
                parameter2 = txtMaterialName.Text.Trim();
            }
            else//其他的，图号DrawingCode+名称MaterialName
            {
                if (txtDrawingCode.Text == string.Empty)
                {
                    Msg.Warning("其他分类：图号不能为空!");
                    txtDrawingCode.Focus();
                    return false;
                }
                if (txtMaterialName.Text == string.Empty)
                {
                    Msg.Warning("其他分类：名称不能为空!");
                    txtMaterialName.Focus();
                    return false;
                }
                parameter1 = txtDrawingCode.Text.Trim();
                parameter2 = txtMaterialName.Text.Trim();
            }

            //输出重复信息
            dr = objMaterialService.IsUniqueMaterial(materialclassId, parameter1, parameter2, materialId);
            if (dr == null)
            {
                return true;
            }
            else
            {
                if (materialclassId.StartsWith("3"))//自制件3开头，图号DrawingCode+颜色PaintingColor
                {
                    waringInfo = string.Format("自制件3开头：图号和颜色查重验证\r\n与库中的物料{0}重复！", dr["MaterialCode"]);
                }
                else if (materialclassId.StartsWith("50") || materialclassId.StartsWith("51"))//外购件50、51开头，型号MaterialType+品牌BrandId
                {
                    waringInfo = string.Format("外购件50、51开头：型号和品牌查重验证\r\n与库中的物料{0}重复！", dr["MaterialCode"]);
                }
                else if (materialclassId.StartsWith("52"))//标准件52开头，型号MaterialType+名称MaterialName
                {
                    waringInfo = string.Format("标准件52开头：型号和名称查重验证\r\n与库中的物料{0}重复！", dr["MaterialCode"]);
                }
                else//其他的，图号DrawingCode+名称MaterialName
                {
                    waringInfo = string.Format("其他分类：图号和名称查重验证\r\n与库中的物料{0}重复！", dr["MaterialCode"]);
                }
                Msg.Warning(waringInfo);
                return false;
            }
        }

        #endregion

        #region 【3】PDM、文件、编码三步验证
        //登录PDM库视图
        private void tsbLogin_Click(object sender, EventArgs e)
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
            //显示登录信息
            this.lblUserName.ForeColor = Color.Black;
            this.lblUserName.Text = string.Format("当前用户：{0}({1})", Globals.PDM_UserAccount, Globals.PDM_UserFullName);
        }
        #endregion

        #endregion

        #region 体验优化（更新重量、自动填写、清除数据）
        /// <summary>
        /// 获取材质和重量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetWeight_Click(object sender, EventArgs e)
        {
            MaterialModel objMaterial = objMaterialService.GetMaterialFromSW();
            this.txtWeight.Clear();
            this.txtWeight.Text = objMaterial.Weight;
            this.txtMquality.Clear();
            this.txtMquality.Text = objMaterial.Mquality;
        }

        //清除数据
        private void tsbClear_Click(object sender, EventArgs e)
        {
            EnableAllControls();
            //this.cboDrawingClass.SelectedIndexChanged -= new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
            foreach (Control item in this.Controls)
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
            //this.cboDrawingClass.SelectedIndexChanged += new System.EventHandler(this.cboDrawingClass_SelectedIndexChanged);
        }

        //启用所有控件
        private void EnableAllControls()
        {
            foreach (Control item in this.Controls)
            {
                item.Enabled = true;
            }
        }

        /// <summary>
        /// 选择物料编码分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMClass_Click(object sender, EventArgs e)
        {
            frmMaterialClass objFrmMaterialClass = new frmMaterialClass();
            DialogResult result = objFrmMaterialClass.ShowDialog();
            if (result == DialogResult.Yes)
            {
                this.txtMaterialClassId.Text = Globals.CurrentMaterial.MaterialClassId.ToString();
                this.txtIsPublic.Text = Globals.CurrentMaterial.IsPublic ? "1" : "0";
                //【2】获取带流水码的物料编码（预显示）
                string materialCode = objMaterialService.GetNewMaterialCode(Globals.CurrentMaterial.MaterialClassId.ToString());
                this.txtMaterialCode.Text = materialCode;
                //this.txtDrawingCode.Text = materialCode;
                //this.txtFactoryCode.Text = Globals.CurrentMaterial.FactoryCode;
            }
        }

        #region 清空选中comboBox的值
        //料件类别
        private void cboMaterialCategoryId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboMaterialCategoryId.SelectedIndex = -1;
            }
        }

        //单位
        private void cboUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboUnit.SelectedIndex = -1;
            }
        }

        //加工属性
        private void cboMachiningProperty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboMachiningProperty.SelectedIndex = -1;
            }
        }

        ////材料
        //private void cboMquality_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyValue == 46)//Delete键
        //    {
        //        this.cboMquality.SelectedIndex = -1;
        //    }
        //}

        //涂装颜色
        private void cboPaintingColor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboPaintingColor.SelectedIndex = -1;
            }
        }

        //品牌
        private void cboBrandId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboBrandId.SelectedIndex = -1;
            }
        }

        //重要等级
        private void cboImportanceGrade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboImportanceGrade.SelectedIndex = -1;
            }
        }

        //图样特征代号
        private void cboDraftFeatureId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)//Delete键
            {
                this.cboDraftFeatureId.SelectedIndex = -1;
            }
        }

        #endregion
        #endregion

        ///// <summary>
        ///// 选择物料编码分类
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnSelectMClass_Click(object sender, EventArgs e)
        //{
        //    frmMaterialClass objFrmMaterialClass = new frmMaterialClass();
        //    DialogResult result = objFrmMaterialClass.ShowDialog();
        //    if (result == DialogResult.Yes)
        //    {
        //        this.txtMaterialClassId.Text = Globals.CurrentMaterial.MaterialClassId.ToString();
        //        this.txtIsPublic.Text = Globals.CurrentMaterial.IsPublic ? "1" : "0";
        //        //【2】获取带流水码的物料编码（预显示）
        //        string materialCode = objMaterialService.GetNewMaterialCode(Globals.CurrentMaterial.MaterialClassId.ToString());
        //        this.txtMaterialCode.Text = materialCode;
        //        this.txtDrawingCode.Text = materialCode;
        //        //this.txtFactoryCode.Text = Globals.CurrentMaterial.FactoryCode;
        //    }
        //}
    }
}

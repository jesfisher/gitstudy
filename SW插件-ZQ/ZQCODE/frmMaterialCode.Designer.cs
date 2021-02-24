namespace ZQCode
{
    partial class frmMaterialCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMaterialCode));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbAddMaterial = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tsbLogin = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblVaultName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMaterialId = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMquality = new System.Windows.Forms.TextBox();
            this.cboMachiningProperty = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDocumentId = new System.Windows.Forms.TextBox();
            this.lblReMark = new System.Windows.Forms.Label();
            this.txtReMark = new System.Windows.Forms.TextBox();
            this.lblSupplyName = new System.Windows.Forms.Label();
            this.txtHeatTreatment = new System.Windows.Forms.TextBox();
            this.lblSurfaceTreatment = new System.Windows.Forms.Label();
            this.txtSurfaceTreatment = new System.Windows.Forms.TextBox();
            this.cboPaintingColor = new System.Windows.Forms.ComboBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboMaterialCategoryId = new System.Windows.Forms.ComboBox();
            this.lblMaterialCategory = new System.Windows.Forms.Label();
            this.lblDrawingCode = new System.Windows.Forms.Label();
            this.txtMaterialSpec = new System.Windows.Forms.TextBox();
            this.txtDrawingCode = new System.Windows.Forms.TextBox();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.lblMaterialSpec = new System.Windows.Forms.Label();
            this.txtMaterialName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.VaultsComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIsPublic = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtFactoryCode = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtMaterialClassId = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtMaterialCode = new System.Windows.Forms.TextBox();
            this.txtMaterialType = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.cboBrandId = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cboImportanceGrade = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cboDraftFeatureId = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSelectMClass = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnGetWeight = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddMaterial,
            this.tsbClear,
            this.tsbLogin});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(685, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbAddMaterial
            // 
            this.tsbAddMaterial.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddMaterial.Image")));
            this.tsbAddMaterial.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAddMaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddMaterial.Name = "tsbAddMaterial";
            this.tsbAddMaterial.Size = new System.Drawing.Size(76, 22);
            this.tsbAddMaterial.Text = "提交数据";
            this.tsbAddMaterial.Click += new System.EventHandler(this.tsbAddMaterial_Click);
            // 
            // tsbClear
            // 
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(76, 22);
            this.tsbClear.Text = "清除数据";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // tsbLogin
            // 
            this.tsbLogin.Image = ((System.Drawing.Image)(resources.GetObject("tsbLogin.Image")));
            this.tsbLogin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbLogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLogin.Name = "tsbLogin";
            this.tsbLogin.Size = new System.Drawing.Size(80, 22);
            this.tsbLogin.Text = "登录PDM";
            this.tsbLogin.Click += new System.EventHandler(this.tsbLogin_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblVaultName,
            this.lblUserName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 570);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(685, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblVaultName
            // 
            this.lblVaultName.Name = "lblVaultName";
            this.lblVaultName.Size = new System.Drawing.Size(88, 17);
            this.lblVaultName.Text = "PDM库：       ";
            // 
            // lblUserName
            // 
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(92, 17);
            this.lblUserName.Text = "当前登录用户：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(52, 83);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 122;
            this.label14.Text = "编码ID：";
            // 
            // txtMaterialId
            // 
            this.txtMaterialId.Enabled = false;
            this.txtMaterialId.Location = new System.Drawing.Point(110, 79);
            this.txtMaterialId.Name = "txtMaterialId";
            this.txtMaterialId.Size = new System.Drawing.Size(195, 21);
            this.txtMaterialId.TabIndex = 121;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Location = new System.Drawing.Point(104, 508);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(89, 22);
            this.lblInfo.TabIndex = 120;
            this.lblInfo.Text = "提示信息...";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(64, 306);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 119;
            this.label11.Text = "材质：";
            // 
            // txtMquality
            // 
            this.txtMquality.Location = new System.Drawing.Point(110, 302);
            this.txtMquality.Name = "txtMquality";
            this.txtMquality.Size = new System.Drawing.Size(195, 21);
            this.txtMquality.TabIndex = 118;
            // 
            // cboMachiningProperty
            // 
            this.cboMachiningProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMachiningProperty.FormattingEnabled = true;
            this.cboMachiningProperty.Location = new System.Drawing.Point(443, 273);
            this.cboMachiningProperty.Name = "cboMachiningProperty";
            this.cboMachiningProperty.Size = new System.Drawing.Size(195, 20);
            this.cboMachiningProperty.TabIndex = 113;
            this.cboMachiningProperty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboMachiningProperty_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(373, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 114;
            this.label2.Text = "加工属性：";
            // 
            // cboUnit
            // 
            this.cboUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnit.FormattingEnabled = true;
            this.cboUnit.Location = new System.Drawing.Point(110, 273);
            this.cboUnit.Name = "cboUnit";
            this.cboUnit.Size = new System.Drawing.Size(195, 20);
            this.cboUnit.TabIndex = 111;
            this.cboUnit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboUnit_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 112;
            this.label1.Text = "单位：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(387, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 110;
            this.label6.Text = "文件ID：";
            // 
            // txtDocumentId
            // 
            this.txtDocumentId.Enabled = false;
            this.txtDocumentId.Location = new System.Drawing.Point(444, 50);
            this.txtDocumentId.Name = "txtDocumentId";
            this.txtDocumentId.Size = new System.Drawing.Size(195, 21);
            this.txtDocumentId.TabIndex = 81;
            // 
            // lblReMark
            // 
            this.lblReMark.AutoSize = true;
            this.lblReMark.Location = new System.Drawing.Point(62, 445);
            this.lblReMark.Name = "lblReMark";
            this.lblReMark.Size = new System.Drawing.Size(41, 12);
            this.lblReMark.TabIndex = 108;
            this.lblReMark.Text = "备注：";
            // 
            // txtReMark
            // 
            this.txtReMark.Location = new System.Drawing.Point(109, 442);
            this.txtReMark.Multiline = true;
            this.txtReMark.Name = "txtReMark";
            this.txtReMark.Size = new System.Drawing.Size(530, 50);
            this.txtReMark.TabIndex = 91;
            // 
            // lblSupplyName
            // 
            this.lblSupplyName.AutoSize = true;
            this.lblSupplyName.Location = new System.Drawing.Point(51, 416);
            this.lblSupplyName.Name = "lblSupplyName";
            this.lblSupplyName.Size = new System.Drawing.Size(53, 12);
            this.lblSupplyName.TabIndex = 107;
            this.lblSupplyName.Text = "热处理：";
            // 
            // txtHeatTreatment
            // 
            this.txtHeatTreatment.Location = new System.Drawing.Point(109, 412);
            this.txtHeatTreatment.Name = "txtHeatTreatment";
            this.txtHeatTreatment.Size = new System.Drawing.Size(195, 21);
            this.txtHeatTreatment.TabIndex = 88;
            // 
            // lblSurfaceTreatment
            // 
            this.lblSurfaceTreatment.AutoSize = true;
            this.lblSurfaceTreatment.Location = new System.Drawing.Point(374, 416);
            this.lblSurfaceTreatment.Name = "lblSurfaceTreatment";
            this.lblSurfaceTreatment.Size = new System.Drawing.Size(65, 12);
            this.lblSurfaceTreatment.TabIndex = 104;
            this.lblSurfaceTreatment.Text = "表面处理：";
            // 
            // txtSurfaceTreatment
            // 
            this.txtSurfaceTreatment.Location = new System.Drawing.Point(443, 412);
            this.txtSurfaceTreatment.Name = "txtSurfaceTreatment";
            this.txtSurfaceTreatment.Size = new System.Drawing.Size(195, 21);
            this.txtSurfaceTreatment.TabIndex = 89;
            // 
            // cboPaintingColor
            // 
            this.cboPaintingColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPaintingColor.FormattingEnabled = true;
            this.cboPaintingColor.Location = new System.Drawing.Point(443, 302);
            this.cboPaintingColor.Name = "cboPaintingColor";
            this.cboPaintingColor.Size = new System.Drawing.Size(195, 20);
            this.cboPaintingColor.TabIndex = 86;
            this.cboPaintingColor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboPaintingColor_KeyDown);
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(374, 306);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(65, 12);
            this.lblUnit.TabIndex = 103;
            this.lblUnit.Text = "涂装颜色：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(307, 193);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 14);
            this.label9.TabIndex = 100;
            this.label9.Text = "*";
            // 
            // cboMaterialCategoryId
            // 
            this.cboMaterialCategoryId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterialCategoryId.FormattingEnabled = true;
            this.cboMaterialCategoryId.Location = new System.Drawing.Point(444, 160);
            this.cboMaterialCategoryId.Name = "cboMaterialCategoryId";
            this.cboMaterialCategoryId.Size = new System.Drawing.Size(195, 20);
            this.cboMaterialCategoryId.TabIndex = 82;
            this.cboMaterialCategoryId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboMaterialCategoryId_KeyDown);
            // 
            // lblMaterialCategory
            // 
            this.lblMaterialCategory.AutoSize = true;
            this.lblMaterialCategory.Location = new System.Drawing.Point(375, 164);
            this.lblMaterialCategory.Name = "lblMaterialCategory";
            this.lblMaterialCategory.Size = new System.Drawing.Size(65, 12);
            this.lblMaterialCategory.TabIndex = 96;
            this.lblMaterialCategory.Text = "料件类别：";
            // 
            // lblDrawingCode
            // 
            this.lblDrawingCode.AutoSize = true;
            this.lblDrawingCode.Location = new System.Drawing.Point(64, 194);
            this.lblDrawingCode.Name = "lblDrawingCode";
            this.lblDrawingCode.Size = new System.Drawing.Size(41, 12);
            this.lblDrawingCode.TabIndex = 95;
            this.lblDrawingCode.Text = "图号：";
            // 
            // txtMaterialSpec
            // 
            this.txtMaterialSpec.Location = new System.Drawing.Point(110, 220);
            this.txtMaterialSpec.Name = "txtMaterialSpec";
            this.txtMaterialSpec.Size = new System.Drawing.Size(195, 21);
            this.txtMaterialSpec.TabIndex = 85;
            // 
            // txtDrawingCode
            // 
            this.txtDrawingCode.Location = new System.Drawing.Point(110, 190);
            this.txtDrawingCode.Name = "txtDrawingCode";
            this.txtDrawingCode.Size = new System.Drawing.Size(195, 21);
            this.txtDrawingCode.TabIndex = 83;
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.Location = new System.Drawing.Point(399, 194);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(41, 12);
            this.lblMaterialName.TabIndex = 98;
            this.lblMaterialName.Text = "名称：";
            // 
            // lblMaterialSpec
            // 
            this.lblMaterialSpec.AutoSize = true;
            this.lblMaterialSpec.Location = new System.Drawing.Point(64, 224);
            this.lblMaterialSpec.Name = "lblMaterialSpec";
            this.lblMaterialSpec.Size = new System.Drawing.Size(41, 12);
            this.lblMaterialSpec.TabIndex = 99;
            this.lblMaterialSpec.Text = "规格：";
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Location = new System.Drawing.Point(444, 190);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size(195, 21);
            this.txtMaterialName.TabIndex = 84;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(58, 54);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 93;
            this.label4.Text = "PDM库：";
            // 
            // VaultsComboBox
            // 
            this.VaultsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VaultsComboBox.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VaultsComboBox.FormattingEnabled = true;
            this.VaultsComboBox.Location = new System.Drawing.Point(110, 50);
            this.VaultsComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.VaultsComboBox.Name = "VaultsComboBox";
            this.VaultsComboBox.Size = new System.Drawing.Size(195, 20);
            this.VaultsComboBox.TabIndex = 80;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(351, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 124;
            this.label5.Text = "集团物料标识：";
            // 
            // txtIsPublic
            // 
            this.txtIsPublic.Enabled = false;
            this.txtIsPublic.Location = new System.Drawing.Point(444, 79);
            this.txtIsPublic.Name = "txtIsPublic";
            this.txtIsPublic.Size = new System.Drawing.Size(195, 21);
            this.txtIsPublic.TabIndex = 123;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(363, 113);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 126;
            this.label15.Text = "事业部编码：";
            // 
            // txtFactoryCode
            // 
            this.txtFactoryCode.Enabled = false;
            this.txtFactoryCode.Location = new System.Drawing.Point(444, 109);
            this.txtFactoryCode.Name = "txtFactoryCode";
            this.txtFactoryCode.Size = new System.Drawing.Size(195, 21);
            this.txtFactoryCode.TabIndex = 125;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(40, 164);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 128;
            this.label16.Text = "物料分类：";
            // 
            // txtMaterialClassId
            // 
            this.txtMaterialClassId.Enabled = false;
            this.txtMaterialClassId.Location = new System.Drawing.Point(110, 160);
            this.txtMaterialClassId.Name = "txtMaterialClassId";
            this.txtMaterialClassId.Size = new System.Drawing.Size(195, 21);
            this.txtMaterialClassId.TabIndex = 127;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(40, 113);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 130;
            this.label17.Text = "物料编码：";
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Enabled = false;
            this.txtMaterialCode.Location = new System.Drawing.Point(110, 109);
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(195, 21);
            this.txtMaterialCode.TabIndex = 129;
            // 
            // txtMaterialType
            // 
            this.txtMaterialType.Location = new System.Drawing.Point(444, 220);
            this.txtMaterialType.Name = "txtMaterialType";
            this.txtMaterialType.Size = new System.Drawing.Size(195, 21);
            this.txtMaterialType.TabIndex = 131;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(399, 224);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 12);
            this.label18.TabIndex = 132;
            this.label18.Text = "型号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(533, 12);
            this.label3.TabIndex = 133;
            this.label3.Text = "---------------------------------------------------------------------------------" +
    "-------";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(65, 335);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 137;
            this.label8.Text = "重量：";
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(110, 331);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(195, 21);
            this.txtWeight.TabIndex = 136;
            // 
            // cboBrandId
            // 
            this.cboBrandId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBrandId.FormattingEnabled = true;
            this.cboBrandId.Location = new System.Drawing.Point(444, 331);
            this.cboBrandId.Name = "cboBrandId";
            this.cboBrandId.Size = new System.Drawing.Size(195, 20);
            this.cboBrandId.TabIndex = 138;
            this.cboBrandId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboBrandId_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(374, 335);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 139;
            this.label10.Text = "品牌编号：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(109, 362);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(533, 12);
            this.label12.TabIndex = 140;
            this.label12.Text = "---------------------------------------------------------------------------------" +
    "-------";
            // 
            // cboImportanceGrade
            // 
            this.cboImportanceGrade.AutoCompleteCustomSource.AddRange(new string[] {
            "A",
            "B",
            "C"});
            this.cboImportanceGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImportanceGrade.FormattingEnabled = true;
            this.cboImportanceGrade.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.cboImportanceGrade.Location = new System.Drawing.Point(109, 383);
            this.cboImportanceGrade.Name = "cboImportanceGrade";
            this.cboImportanceGrade.Size = new System.Drawing.Size(195, 20);
            this.cboImportanceGrade.TabIndex = 143;
            this.cboImportanceGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboImportanceGrade_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(53, 387);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 144;
            this.label13.Text = "分类码：";
            // 
            // cboDraftFeatureId
            // 
            this.cboDraftFeatureId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDraftFeatureId.FormattingEnabled = true;
            this.cboDraftFeatureId.Location = new System.Drawing.Point(444, 383);
            this.cboDraftFeatureId.Name = "cboDraftFeatureId";
            this.cboDraftFeatureId.Size = new System.Drawing.Size(195, 20);
            this.cboDraftFeatureId.TabIndex = 141;
            this.cboDraftFeatureId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboDraftFeatureId_KeyDown);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(349, 387);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 12);
            this.label19.TabIndex = 142;
            this.label19.Text = "图样特征代号：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(109, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(533, 12);
            this.label7.TabIndex = 145;
            this.label7.Text = "---------------------------------------------------------------------------------" +
    "-------";
            // 
            // btnSelectMClass
            // 
            this.btnSelectMClass.Location = new System.Drawing.Point(310, 160);
            this.btnSelectMClass.Name = "btnSelectMClass";
            this.btnSelectMClass.Size = new System.Drawing.Size(51, 21);
            this.btnSelectMClass.TabIndex = 146;
            this.btnSelectMClass.Text = "选择";
            this.btnSelectMClass.UseVisualStyleBackColor = true;
            this.btnSelectMClass.Click += new System.EventHandler(this.btnSelectMClass_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(642, 163);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 14);
            this.label20.TabIndex = 147;
            this.label20.Text = "*";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(307, 274);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(14, 14);
            this.label21.TabIndex = 148;
            this.label21.Text = "*";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(642, 274);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(14, 14);
            this.label22.TabIndex = 149;
            this.label22.Text = "*";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.ForeColor = System.Drawing.Color.Red;
            this.label23.Location = new System.Drawing.Point(642, 193);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(14, 14);
            this.label23.TabIndex = 150;
            this.label23.Text = "*";
            // 
            // btnGetWeight
            // 
            this.btnGetWeight.Location = new System.Drawing.Point(310, 302);
            this.btnGetWeight.Name = "btnGetWeight";
            this.btnGetWeight.Size = new System.Drawing.Size(51, 21);
            this.btnGetWeight.TabIndex = 151;
            this.btnGetWeight.Text = "选择";
            this.btnGetWeight.UseVisualStyleBackColor = true;
            this.btnGetWeight.Click += new System.EventHandler(this.btnGetWeight_Click);
            // 
            // frmMaterialCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 592);
            this.Controls.Add(this.btnGetWeight);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.btnSelectMClass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboImportanceGrade);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cboDraftFeatureId);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cboBrandId);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMaterialType);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtMaterialCode);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtMaterialClassId);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtFactoryCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtIsPublic);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtMaterialId);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtMquality);
            this.Controls.Add(this.cboMachiningProperty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboUnit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDocumentId);
            this.Controls.Add(this.lblReMark);
            this.Controls.Add(this.txtReMark);
            this.Controls.Add(this.lblSupplyName);
            this.Controls.Add(this.txtHeatTreatment);
            this.Controls.Add(this.lblSurfaceTreatment);
            this.Controls.Add(this.txtSurfaceTreatment);
            this.Controls.Add(this.cboPaintingColor);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboMaterialCategoryId);
            this.Controls.Add(this.lblMaterialCategory);
            this.Controls.Add(this.lblDrawingCode);
            this.Controls.Add(this.txtMaterialSpec);
            this.Controls.Add(this.txtDrawingCode);
            this.Controls.Add(this.lblMaterialName);
            this.Controls.Add(this.lblMaterialSpec);
            this.Controls.Add(this.txtMaterialName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.VaultsComboBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMaterialCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "见全局变量Globals";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbAddMaterial;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblVaultName;
        private System.Windows.Forms.ToolStripStatusLabel lblUserName;
        private System.Windows.Forms.ToolStripButton tsbLogin;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox txtMaterialId;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox txtMquality;
        private System.Windows.Forms.ComboBox cboMachiningProperty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtDocumentId;
        private System.Windows.Forms.Label lblReMark;
        public System.Windows.Forms.TextBox txtReMark;
        private System.Windows.Forms.Label lblSupplyName;
        public System.Windows.Forms.TextBox txtHeatTreatment;
        private System.Windows.Forms.Label lblSurfaceTreatment;
        public System.Windows.Forms.TextBox txtSurfaceTreatment;
        private System.Windows.Forms.ComboBox cboPaintingColor;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboMaterialCategoryId;
        private System.Windows.Forms.Label lblMaterialCategory;
        private System.Windows.Forms.Label lblDrawingCode;
        public System.Windows.Forms.TextBox txtMaterialSpec;
        public System.Windows.Forms.TextBox txtDrawingCode;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.Label lblMaterialSpec;
        public System.Windows.Forms.TextBox txtMaterialName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox VaultsComboBox;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtIsPublic;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.TextBox txtFactoryCode;
        private System.Windows.Forms.Label label16;
        public System.Windows.Forms.TextBox txtMaterialClassId;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox txtMaterialCode;
        public System.Windows.Forms.TextBox txtMaterialType;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.ComboBox cboBrandId;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboImportanceGrade;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboDraftFeatureId;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSelectMClass;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnGetWeight;
    }
}
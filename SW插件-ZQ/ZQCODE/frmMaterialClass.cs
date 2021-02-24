using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZQCode
{
    public partial class frmMaterialClass : Form
    {
        private MaterialService objMaterialService = new MaterialService();
        private DataTable dtMaterialClass = new DataTable();

        public frmMaterialClass()
        {
            InitializeComponent();
            AddMaterialTree();
        }

        #region 添加物料编码分类树结构（dtMaterialClass）

        //添加物料编码分类树
        private void AddMaterialTree()
        {
            //【仅显示集团和本事业部的物料分类】获取树节点需要的数据
            dtMaterialClass = objMaterialService.GetFactoryMaterialClass();
            DataView dv = new DataView(dtMaterialClass);
            //创建一个根节点：默认根节点的值=0
            this.tvMaterialClass.Nodes.Clear();
            TreeNode rootNode = new TreeNode();
            rootNode.Text = "物料分类";
            rootNode.Tag = "0_0";
            rootNode.Name = "0";
            this.tvMaterialClass.Nodes.Add(rootNode);//添加根节点
            CreateChildNode(rootNode, dtMaterialClass, "0");//递归方法调用
            this.tvMaterialClass.Nodes[0].Expand(); //将递归树的一级目录展开
        }

        //递归
        private void CreateChildNode(TreeNode parentNode, DataTable dt, string parentId)
        {
            //找到所有以该节点为父节点的子项
            DataRow[] rowList = dt.Select(string.Format("ParentId='{0}'", parentId));
            //循环创建该节点的所有子节点
            foreach (DataRow dr in rowList)
            {
                //创建新的节点并设置属性
                TreeNode childNode = new TreeNode();
                childNode.Text = string.Format("【{0}】{1}", dr["MaterialClassId"].ToString(), dr["MaterialClassName"].ToString());
                childNode.Tag = dr["MaterialClassId"].ToString() + "_" + dr["ParentId"].ToString();
                childNode.Name = dr["MaterialClassId"].ToString();
                parentNode.Nodes.Add(childNode);  //父节点加入该子节点
                childNode.ImageIndex = childNode.Level;//设置节点图片(来自ImageList)
                //递归调用，以此子节点为父节点创建该子节点的其他节点               
                CreateChildNode(childNode, dt, dr["MaterialClassId"].ToString());
            }
        }

        #endregion

        //选择正确的子节点，并给全局变量赋值
        private void tsbConfirm_Click(object sender, EventArgs e)
        {
            //未选中任何节点，提示
            if (tvMaterialClass.SelectedNode == null)
            {
                Msg.ShowError("未选中任何节点，无法操作！");
                return;
            }
            //选中节点的ID【MaterialClassId】
            string selectedNodeId = tvMaterialClass.SelectedNode.Tag.ToString().Split('_')[0];
            //选中最后一层，不允许添加节点
            if (selectedNodeId.Length < 4)//最后一层，位数为4位，如：1001
            {
                Msg.ShowError("请选择最后一级节点！");
                return;
            }
            //从数据库获取选中节点的完整信息
            DataRow dr = objMaterialService.GetMaterialClassById(selectedNodeId);
            MaterialModel objMaterial = new MaterialModel()
            {
                MaterialClassId = selectedNodeId,
                IsPublic = ConvertEx.ToBoolean(dr["IsPublic"]),
                FactoryCode = ConvertEx.ToString(dr["FactoryCode"])
            };

            //Globals.CurrentMaterial.MaterialClassId = ConvertEx.ToInt(selectedNodeId);
            //Globals.CurrentMaterial.IsPublic = ConvertEx.ToBoolean(dr["IsPublic"]);
            //Globals.CurrentMaterial.FactoryCode = ConvertEx.ToString(dr["FactoryCode"]);
            if (selectedNodeId.Length == 4)
            {
                Globals.CurrentMaterial = objMaterial;
                this.DialogResult = DialogResult.Yes;
            }
            else
            {
                this.DialogResult = DialogResult.No;
            }
            //Msg.ShowInformation(objMaterial.MaterialClassId.ToString());
        }
    }
}

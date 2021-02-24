using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using EdmLib;
using System.Windows.Forms;
using Microsoft.VisualBasic;
namespace ZQCode
{
    public class pdmInterface : IEdmAddIn5
    {
        public void GetAddInInfo(ref EdmAddInInfo poInfo, IEdmVault5 poVault, IEdmCmdMgr5 poCmdMgr)
        {
            poInfo.mbsAddInName = "编码器v1";
            poInfo.mbsCompany = "JES Company";
            //poInfo.mbsDescription = "产品和零部件编码生成器；服务器：" + Utility.SQLHelper.strConn.Substring(7, Utility.SQLHelper.strConn.IndexOf(";", 0) - 7);
            poInfo.mbsDescription = "产品和零部件编码生成器";
            poInfo.mlAddInVersion = 2;
            poInfo.mlRequiredVersionMajor = 5;
            poInfo.mlRequiredVersionMinor = 2;
            poCmdMgr.AddHook(EdmCmdType.EdmCmd_CardButton);
        }
        public void OnCmd(ref EdmCmd poCmd, ref Array ppoData)
        {
            //if (Strings.Left(poCmd.mbsComment, 4) == "JES")
            //{
            //    IEdmEnumeratorVariable5 vars = (IEdmEnumeratorVariable5)poCmd.mpoExtra;
            //    EdmVault5 vault5 = (EdmVault5)poCmd.mpoVault;
            //    IEdmVault11 valut11 = (IEdmVault11)vault5;
            //    frmMain frmMain = new frmMain(new List<string> { "", "" });
            //    if (frmMain.ShowDialog() == DialogResult.OK)
            //    {
            //        vars.SetVar("A代号", "", frmMain.ResultCode);
            //    }
            //}
        }
    }
}

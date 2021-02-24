using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ZQCode
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //显示登录窗体
            //程序验证检查-测试时可注释20200523
            if (new frmLogin().ShowDialog() == DialogResult.OK)
            {
                Application.Run(new frmMaterialCode());
            }
            else
            {
                new frmLogin().ShowDialog();
                Msg.ShowError("检查项不通过，程序无法正常启动...");
                Application.Exit();
            }
        }
    }
}

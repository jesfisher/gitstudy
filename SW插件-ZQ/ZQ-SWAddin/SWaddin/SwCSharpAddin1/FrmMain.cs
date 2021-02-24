using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProductCode;

namespace SwCSharpAddin1
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmProductCode objFrmCode = new frmProductCode("Manage");
            DialogResult result = objFrmCode.ShowDialog();
            if (result== DialogResult.OK)
            {
                MessageBox.Show(objFrmCode.TargetCode+" "+objFrmCode.TargetName);
            }
        }
    }
}

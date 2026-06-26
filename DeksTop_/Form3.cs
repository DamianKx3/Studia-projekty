using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Line
{
    public partial class Form3 : Form
    {
        public Form1 Main;
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.PlayTest(Application.StartupPath + "\\PreMade\\poziom11\\" + "" + "save.lvl");
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.Closesel();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TModLoaderHelper
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            this.textBox1.Text = folderBrowserDialog.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };
            folderBrowserDialog.ShowDialog();
            this.textBox2.Text = folderBrowserDialog.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XNBBuilder xnbbuilder = new XNBBuilder();
            xnbbuilder.PackageContent(Directory.GetFiles(this.textBox1.Text), this.textBox2.Text, false, this.textBox1.Text, out this.log);
        }
        public bool log = false;
    }
}

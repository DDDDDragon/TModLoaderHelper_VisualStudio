using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.CodeDom;
using System.IO;
using System.CodeDom.Compiler;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Win32;
using CefSharp;
using CefSharp.WinForms;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using EnvDTE80;
using System.Net;
using System.Runtime.InteropServices;
using ArmorHelper;

namespace TModLoaderHelper
{
    public partial class ModHelper : Form
    {
        ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            CefSettings settings = new CefSettings();
            string url = "https://github.com/search?q=tModLoader&type=";
            if(!Cef.IsInitialized) Cef.Initialize(settings);
            browser = new ChromiumWebBrowser(url);
            browser.FrameLoadEnd += ChromiumContainer_FrameLoadEnd;
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.Show();
        }
        public ModHelper()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
        }
        public string PageSource = "";
        private void ChromiumContainer_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var task = e.Frame.GetSourceAsync();
            task.ContinueWith(t =>
            {
                if (!t.IsFaulted)
                {
                    PageSource = t.Result;
                }
            });
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        CodeDomProvider provider = null;
        CodeGeneratorOptions options = null;
        CodeCompileUnit unit = null;
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void ItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> SolutionThings = MyCommand.GetSlnPath();
            unit = new CodeCompileUnit();
            System.CodeDom.CodeNamespace codeNamespace = new System.CodeDom.CodeNamespace(SolutionThings[0].Substring(0, SolutionThings[0].Length - 4));
            codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Terraria"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Terraria.ModLoader"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Terraria.ID"));
            codeNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.Xna.Framework"));
            CodeTypeDeclaration myClass = new CodeTypeDeclaration("把这个改成你的物品名称");
            myClass.IsClass = true;
            myClass.TypeAttributes = TypeAttributes.Public;
            CodeMemberMethod setDefault = new CodeMemberMethod();
            setDefault.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            setDefault.ReturnType = new CodeTypeReference("void");
            setDefault.Name = "SetDefaults";
            setDefault.Statements.Add(new CodeSnippetStatement("            item.width = 10;//把这个改成你物品贴图的宽度"));
            setDefault.Statements.Add(new CodeSnippetStatement("            item.height = 10;//把这个改成你物品贴图的高度"));
            CodeMemberMethod setStaticDefault = new CodeMemberMethod();
            setStaticDefault.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            setStaticDefault.ReturnType = new CodeTypeReference("void");
            setStaticDefault.Name = "SetStaticDefaults";
            setStaticDefault.Statements.Add(new CodeSnippetStatement("            DisplayName.SetDefault(\"把这个改成你物品的名称\");"));
            setStaticDefault.Statements.Add(new CodeSnippetStatement("            Tooltip.SetDefault(\"把这个改成你物品的描述\");"));
            myClass.Members.Add(setDefault);
            myClass.Members.Add(setStaticDefault);
            codeNamespace.Types.Add(myClass);
            unit.Namespaces.Add(codeNamespace);
            provider = CodeDomProvider.CreateProvider("CSharp");  //创建一个代码生成器并指定语言为C sharp
            options = new CodeGeneratorOptions();    //代码生成的方法
            options.BracingStyle = "C"; //C风格
            options.BlankLinesBetweenMembers = true;
            StreamWriter sw = null;
            StreamReader sr = null;
            string saveName = SolutionThings[1] + "\\NewItem.cs";
            MessageBox.Show(saveName);
            using (sw = new StreamWriter(saveName, false, System.Text.Encoding.UTF8))
            {
                provider.GenerateCodeFromCompileUnit(unit, sw, options);
            }
            sr = new StreamReader(saveName, System.Text.Encoding.UTF8);
            string Code = sr.ReadToEnd().Replace("\t", "    ").Replace("public class 把这个改成你的物品名称", "public class 把这个改成你的物品名称 : ModItem")
                .Replace("@void", "void").Replace("@bool", "bool").Replace("@int", "int");
            sw.Close();
            sr.Close();
            StreamWriter sw2 = new StreamWriter(saveName, false, System.Text.Encoding.UTF8);
            sw2.Write(Code);
            sw2.Close();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        public static void SetWebBrowserFeatures(int Version)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) throw new Exception();
            var appName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, Version, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);

        }

        private void zaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/search?q=" + toolStripTextBox1.Text + "&type=";
            browser.LoadUrl(url);
        }

        private void 在tModLoader中搜索ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/tModLoader/tModLoader/search?q=" + toolStripTextBox1.Text + "&type=";
            browser.LoadUrl(url);
        }

        private void 复制此页面代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!browser.Address.Contains("blob"))
            {
                MessageBox.Show("本页不存在代码");
                return;
            }
            List<string> SolutionThings = MyCommand.GetSlnPath();
            string saveName = SolutionThings[1] + "\\PageSource.cs";
            var dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
            WebClient myWeb = new WebClient();
            myWeb.Credentials = CredentialCache.DefaultCredentials;
            Byte[] pageData = myWeb.DownloadData(browser.Address.Replace("blob", "raw"));
            StreamWriter sw = new StreamWriter(saveName, false, System.Text.Encoding.UTF8);
            string Code = Encoding.Default.GetString(pageData);
            sw.Write(Code);
            sw.Close();
            dte2.ItemOperations.OpenFile(saveName);
        }

        private void armorHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog Oppf = new OpenFileDialog();
            Oppf.ShowDialog();
            if (Oppf.FileName != "")
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Oppf.FileName;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.Start();
                while (p.MainWindowHandle.ToInt32() == 0)
                {
                    System.Threading.Thread.Sleep(100);
                }
                ShowWindow(p.MainWindowHandle, (int)ProcessWindowStyle.Maximized);
                SetWindowText(p.MainWindowHandle, "Aseprite(监视中)");
            }*/
            ArmorHelperWindow form = new ArmorHelperWindow(new string[] { });
            form.Size = new Size(form.Size.Width + 50, form.Size.Height + 50);
            form.InitWindow();
            form.creatorLink.Size = new Size(form.creatorLink.Size.Width + 40, form.creatorLink.Size.Height);
            form.outputGroup.Size = new Size(form.outputGroup.Size.Width, form.outputGroup.Size.Height + 10);
            form.inputGroup.Size = new Size(form.inputGroup.Size.Width, form.inputGroup.Size.Height + 10);
            form.Show();
        }
        [DllImport("User32.dll", EntryPoint = "SetWindowText")]
        private static extern void SetWindowText(IntPtr hwnd, string lpString);

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        private void xNBCompilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}

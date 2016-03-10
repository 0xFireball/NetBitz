using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Linq;
using OmniBean.PowerCrypt4;
using OmniBean.PowerCrypt4.Utilities;

namespace NetBitz
{
    public partial class MainWindow : MetroForm
    {
        List<string> encFiles = new List<string>();
        string mainExecutable = "";
        string key;
        public MainWindow()
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy; // Okay
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            encFiles.AddRange((string[])e.Data.GetData(DataFormats.FileDrop));
            RefreshList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            key = textBox1.Text;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            label3.Text = "Initializing...";
            GenerateCLFile();
            label3.Text = "Done.";
        }
        void SetStatus(string format, params object[] args)
        {
            label3.Text = string.Format(format, args);
        }

        public async void GenerateCLFile()
        {
            string fn = "";
            var sfd = new SaveFileDialog()
            {
                DefaultExt = "exe",
                AddExtension = true,
                Title = "Save Packed File",
                Filter = "Executable Files (.exe)|*.*",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fn = sfd.FileName;
            }
            if (encFiles.Count == 0)
            {
                MessageBox.Show("Please select an input file.");
                return;
            }
            if (mainExecutable == "")
        		mainExecutable = encFiles[0];
            await Task.Run(() =>
            {
                SetStatus("NetBitz v1.0.4");
                SetStatus("Generating application...");
                var f = new AssemblyFactory();
                encFiles.Remove(mainExecutable);
                MemoryStream ms = f.CreateSFXModuleEx(encFiles, mainExecutable);
                SetStatus("Generation completed.");
                using (var fs = File.Create(sfd.FileName))
                {
                    ms.WriteTo(fs);
                }
            });
        }
        bool encryptApp = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            encryptApp = checkBox1.Checked;
        }
        void Button2Click(object sender, EventArgs e)
        {
            textBox1.Text = PowerAES.GenerateRandomString(32);
        }
        void MainWindowLoad(object sender, EventArgs e)
        {

        }
        void ListBox1Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                Title = "Add Files",
                Filter = ".NET Assemblies (*.exe,*.dll)|*.exe;*.dll",
            };
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (string fileName in ofd.FileNames)
                {
                    encFiles.Add(fileName);
                }
            }
            RefreshList();
        }
        void RefreshList()
        {
            listBox1.Items.Clear();
            comboBox1.Items.Clear();
            if (encFiles == null)
                encFiles = new List<string>();
            foreach (string fn in encFiles)
            {
                string it = Path.GetFileName(fn);
                listBox1.Items.Add(it);
                comboBox1.Items.Add(it);
            }
        }
        void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
        {
        	mainExecutable = comboBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            encFiles = new List<string>();
            RefreshList();
        }
    }
}

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
        string encFile;
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
            encFile = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            UpdateBox();
        }
        void UpdateBox()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add(Path.GetFileName(encFile));
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
        	var sfd = new SaveFileDialog(){
            	DefaultExt = "exe",
            	AddExtension = true,
            	Title = "Save Packed File",
            	Filter = "Executable Files (.exe)|*.*",
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
            	fn = sfd.FileName;
            }
        	if (String.IsNullOrWhiteSpace(encFile))
        	{
        		MessageBox.Show("Please select an input file.");
        		return;
        	}
        	await Task.Run(() => {
	            SetStatus("NetBitz v1.0.4");
	            SetStatus("Generating application...");
	            var f = new AssemblyFactory();
	            MemoryStream ms = f.CreateSFXModule(File.ReadAllBytes(encFile).GetString(), encFile);
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
    }
}

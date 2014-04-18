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
using System.Drawing.Text;

namespace DoAnything
{
    public partial class Form1 : Form
    {

        private Boolean isSaved = true;
        private String curPath = "";

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fontOptions.Items.AddRange(new InstalledFontCollection().Families.Select(f => f.Name).ToArray());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isSaved)
                Application.Exit();
            else
            {
                DialogResult Bail = MessageBox.Show("Do you want to save your work?", "Exit Without Saving", MessageBoxButtons.YesNo);
                if (Bail.Equals(DialogResult.Yes))
                    Application.Exit();
                else
                {
                    saveToolStripMenuItem_Click(null, null);
                    Application.Exit();
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Inspired by the ever beautiful Bilbert\nWritten by the wondrous KC");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isSaved = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isSaved)
                richTextBox1.Clear();
            else
            {
                DialogResult Bail = MessageBox.Show("Do you want to save your work before opening a new file?", "Save Before Load", MessageBoxButtons.YesNo);
                if (Bail.Equals(DialogResult.Yes))
                    saveToolStripMenuItem_Click(null, null);
                else
                    richTextBox1.Clear();
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    curPath = openFileDialog1.FileName;
                    richTextBox1.LoadFile(curPath, RichTextBoxStreamType.RichText);
                    isSaved = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curPath != "")
            {
                if (!isSaved)
                {
                    richTextBox1.SaveFile(curPath, RichTextBoxStreamType.RichText);
                    isSaved = true;
                }
            }
            else
            {
                saveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                curPath = saveFileDialog1.FileName;
                richTextBox1.SaveFile(curPath, RichTextBoxStreamType.RichText);
                isSaved = true;
            }
        }

        private void upFontSize_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size + 1);
            richTextBox1.DeselectAll();
            isSaved = false;
        }

        private void downFontSize_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionFont = new System.Drawing.Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size - 1);
            richTextBox1.DeselectAll();
            isSaved = false;
        }
    }
}

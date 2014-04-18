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
            //Set standard open and save location to desktop.
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Get starting font size
            fontSize.Text = richTextBox1.SelectionFont.Size.ToString();
            //Add all local font families into fontOptions combobox
            fontOptions.Items.AddRange(new InstalledFontCollection().Families.Select(f => f.Name).ToArray());
            //Set font options and style to zero
            fontOptions.SelectedIndex = 0;
            fontStyles.SelectedIndex = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If it's saved or nothing is loaded, close.
            if (isSaved || richTextBox1.Text == String.Empty)
                Application.Exit();
            else
            {
                //Otherwise prompt user to save
                DialogResult Bail = MessageBox.Show("Do you want to save your work?", "Exit Without Saving", MessageBoxButtons.YesNo);
                if (Bail.Equals(DialogResult.No))
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
            //Beautiful about message!
            MessageBox.Show("Inspired by the ever beautiful Bilbert\nWritten by the wondrous KC");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Watches for changes
            isSaved = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isSaved)
            {
                DialogResult Bail = MessageBox.Show("Do you want to save your work before opening a new file?", "Save Before Load", MessageBoxButtons.YesNo);
                if (Bail.Equals(DialogResult.Yes))
                    saveToolStripMenuItem_Click(null, null);
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    curPath = openFileDialog1.FileName;
                    richTextBox1.LoadFile(curPath, RichTextBoxStreamType.RichText);
                    fontOptions.Text = richTextBox1.SelectionFont.FontFamily.ToString();
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
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size + 1);
            fontSize.Text = richTextBox1.SelectionFont.Size.ToString();
            richTextBox1.DeselectAll();
            isSaved = false;
        }

        private void downFontSize_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size - 1);
            fontSize.Text = richTextBox1.SelectionFont.Size.ToString();
            richTextBox1.DeselectAll();
            isSaved = false;
        }

        private void fontOptions_SelectedValueChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            try { richTextBox1.SelectionFont = new Font(fontOptions.SelectedItem.ToString(), richTextBox1.SelectionFont.Size); } catch {}
            richTextBox1.DeselectAll();
            isSaved = false;
            fontStyles.SelectedIndex = 0;
        }

        private void fontStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            try
            {
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size, (FontStyle)Enum.Parse(typeof(FontStyle), fontStyles.SelectedItem.ToString(), true));
            }
            catch { }
            ;
            richTextBox1.DeselectAll();
            isSaved = false;
        }

        private void fontSize_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
            try { richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont.FontFamily, Convert.ToSingle(fontSize.Text)); }
            catch { }
            richTextBox1.DeselectAll();
            isSaved = false;
        }

        private void fontSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '.')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && fontSize.Text.Count(c => c == '.') > 0)
            {
                e.Handled = true;
            }
        }
    }
}

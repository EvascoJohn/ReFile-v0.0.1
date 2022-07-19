using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ReFile_2;

namespace ReFile_2
{
    public partial class Form1 : Form
    {
        private static Helper helper = new Helper();
        string[] populated_filetypes;
        bool textbox_scan_has_value  = false;
        bool textbox_dest_has_value = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button_run.Enabled = false;
        }

        // pass in list CheckListBox = clb, string[] string_array, to add each extension to the checklistbox.
        private void populate_checklistbox(CheckedListBox clb,string[] string_array)
        {
            foreach (string extension in string_array)
            {
                clb.Items.Add(extension, true);
            }
        }

        // pass in listbox, checkedlistbox to return the checked indices from the checklistbox.
        private string[] get_checked_indices(CheckedListBox checklistbox)
        {
            List<string> temp_check_box_list = new List<string>();
            int[] indices = checklistbox.CheckedIndices.Cast<int>().ToArray();
            foreach(string checkitem in checklistbox.CheckedItems)
            {
                temp_check_box_list.Add(checkitem);
            }
            return temp_check_box_list.ToArray();
        }

        private void check_both_textbox()
        {
            if(textbox_scan_has_value && textbox_dest_has_value)
            {
                button_run.Enabled = true;
            }
            else
            {
                button_run.Enabled = false;
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

        }

        // Scan Button
        private void iconButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog temp_folder_brower = new FolderBrowserDialog();
            temp_folder_brower.Description = "Select a folder to scan its files.";
            if(temp_folder_brower.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textbox_scandir.Text = temp_folder_brower.SelectedPath;
                helper.arrange_files_to_dict(textbox_scandir.Text);
                foreach (string filetype in helper.get_populated_filetypes_from_dictionary())
                {
                    checkedListBox1.Items.Add(filetype, false);
                }
            }
        }

        // Destination Button
        private void iconButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog temp_folder_brower = new FolderBrowserDialog();
            temp_folder_brower.Description = "Select a folder to set it as a destination for the scanned files.";
            if (temp_folder_brower.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textbox_destinationdir.Text = temp_folder_brower.SelectedPath;
                helper.create_folders(textbox_destinationdir.Text);
            }
        }


        // Run Button
        private void iconButton3_Click(object sender, EventArgs e)
        {
            // Get the strings inside the text boxes.
            string temp_scan_directory = textbox_scandir.Text;
            string temp_dest_directory = textbox_destinationdir.Text;
            bool success = false;

            // Check if the directory scan in empty.
            if (helper.get_populated_filetypes_from_dictionary().Count() > 0)
            {
                try
                {
                    // Create Folder.
                    helper.create_folders(textbox_destinationdir.Text);
                    // Move Files.
                    helper.move_files(textbox_scandir.Text, textbox_destinationdir.Text, get_checked_indices(checkedListBox1));
                    success = true;
                }
                catch (Exception exception)
                {
                    new MyMessageBox(exception.ToString()).Show();
                    success = false;
                }
                if (success)
                {
                    MyMessageBox msgbox = new MyMessageBox("Files were moved succesfully.");
                    msgbox.Text = "Success";
                    msgbox.Show();
                }
            }
            else
            {
                MyMessageBox msgbox = new MyMessageBox("No files were recognized..");
                msgbox.Text = "No Files";
                msgbox.Show();
            }
        }


        private void textbox_scandir_changed(object sender, EventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (textbox_scandir.Text != String.Empty)
            {
                // Check If the paths exists.
                if (Directory.Exists(textbox_scandir.Text))
                {
                    textbox_scan_has_value = true;
                }
            }
            else if (textbox_scandir.Text == String.Empty)
            {
                textbox_scan_has_value = false;
            }
            check_both_textbox();
        }


        private void TextBox_Destination(object sender, EventArgs e)
        {
            if (textbox_destinationdir.Text != String.Empty)
            {
                // Check If the paths exists.
                if (Directory.Exists(textbox_destinationdir.Text))
                {
                    textbox_dest_has_value = true;
                }
            }
            else if (textbox_destinationdir.Text == String.Empty)
            {
                textbox_dest_has_value = false;
            }
            check_both_textbox();
        }

        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            MyMessageBox msgbox = new MyMessageBox("Software Version: v0.0.1\nBy: John Carlo Evasco\nGithub: https://github.com/EvascoJohn");
            msgbox.Text = "Abouts";
            msgbox.Show();
        }
    }
}

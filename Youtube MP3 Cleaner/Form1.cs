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
using TagLib;

namespace Youtube_MP3_Cleaner
{
    public partial class Form1 : Form
    {
        string folderPath = "";
        List<string> failList = new List<string>();
        public Form1()
        {
            InitializeComponent();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                folderPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (string fileName in Directory.GetFiles(folderPath))
                {
                    string trueFileName = Path.GetFileName(fileName);
                    string[] fileParts = trueFileName.Split('-');

                    if (fileParts.Count() == 1)
                    {
                        failList.Add(fileName);
                    }
                    if (fileParts.Count() == 2)
                    {
                        TagLib.File f = TagLib.File.Create(fileName);
                        string[] performers = new string[1];
                        performers[0] = fileParts[0].Trim();
                        f.Tag.Performers = performers;
                        string title = Path.GetFileNameWithoutExtension(fileParts[1]).Trim();
                        f.Tag.Title = title;
                        f.Save();
                        System.IO.File.Move(fileName, folderPath + "\\" + title + ".mp3");
                    }
                    if (fileParts.Count() == 3)
                    {
                        if (fileParts[0].Trim()[0] == '[')
                        {
                            TagLib.File f = TagLib.File.Create(fileName);

                            string[] performers = new string[1];
                            performers[0] = fileParts[1].Trim();
                            f.Tag.Performers = performers;
                            string[] genres = new string[1];
                            genres[0] = fileParts[0].Trim();
                            f.Tag.Genres = genres;
                            string title = Path.GetFileNameWithoutExtension(fileParts[2]).Trim();
                            f.Tag.Title = title;
                            f.Save();
                            System.IO.File.Move(fileName, folderPath + "\\" + title + ".mp3");
                        }
                        else
                        {
                            failList.Add(fileName);
                        }
                    }
                    if (fileParts.Count() > 3)
                    {
                        failList.Add(fileName);
                    }




                }
                foreach (string file in failList)
                {
                    listBox1.Items.Add(Path.GetFileNameWithoutExtension(file));
                }

            }
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string fileName = folderPath + "\\" + listBox1.SelectedItem.ToString() + ".mp3";
                TagLib.File f = TagLib.File.Create(fileName);

                textBox2.Text = f.Tag.Title;
                try
                {
                    textBox3.Text = f.Tag.Performers[0];
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    textBox3.Text = "";
                }
                try
                {
                    textBox4.Text = f.Tag.Genres[0];
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    textBox4.Text = "";
                }
            }
            catch (System.NullReferenceException ex)
            {
                listBox1.ClearSelected();
            }




        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName = folderPath + "\\" + listBox1.SelectedItem.ToString() + ".mp3";
            TagLib.File f = TagLib.File.Create(fileName);
            f.Tag.Title = textBox2.Text;
            if (textBox3.Text != "")
            {
                string[] performers = new string[1];
                performers[0] = textBox3.Text;
                f.Tag.Performers = performers;
            }
            if (textBox4.Text != "")
            {
                string[] genres = new string[1];
                genres[0] = textBox4.Text;
                f.Tag.Genres = genres;
            }
            f.Save();
            System.IO.File.Move(fileName, folderPath + "\\" + textBox2.Text + ".mp3");
            listBox1.Items.Remove(listBox1.SelectedItem);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
                textBox2.Select();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
                textBox2.Select();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.PerformClick();
                textBox2.Select();
            }
        }
    }
}

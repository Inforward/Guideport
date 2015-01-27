using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace DbScriptCombine
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (browser.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = browser.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (browser.ShowDialog() == DialogResult.OK)
            {
                txtDestination.Text = browser.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var sourcePath = txtSource.Text;
                var destinationPath = txtDestination.Text;

                if (!destinationPath.EndsWith("\\"))
                    destinationPath += "\\";

                ClearPath(destinationPath);

                CheckPath(sourcePath);
                CheckPath(destinationPath);

                var folderCompileOrder = new List<string>()
                    {
                        "Tables",
                        "Table Alters",
                        "Stored Procedures",
                        "Functions",
                        "User Defined Functions",
                        "Views",
                        "Initial Load"
                    };

                // Loop through servers
                foreach (var serverDirectory in new DirectoryInfo(sourcePath).GetDirectories())
                {
                    var outputDirectoryPath = destinationPath + serverDirectory.Name;

                    Directory.CreateDirectory(outputDirectoryPath);

                    // Loop through databases
                    foreach (var databaseDirectory in serverDirectory.GetDirectories())
                    {
                        var outputFileName = string.Format("{0}\\{1}.sql", outputDirectoryPath, databaseDirectory.Name);

                        using (var sw = new StreamWriter(outputFileName, false))
                        {
                            // Loop through files
                            foreach (var folder in folderCompileOrder)
                            {
                                var folderPath = databaseDirectory.FullName + "\\" + folder;

                                if (Directory.Exists(folderPath))
                                {
                                    foreach (var file in Directory.GetFiles(folderPath))
                                    {
                                        using (var sr = new StreamReader(file))
                                        {
                                            var contents = sr.ReadToEnd();
                                            sw.Write(contents);
                                            sw.WriteLine("");
                                            sw.WriteLine("GO");
                                            sw.WriteLine("");
                                            
                                            sr.Close();
                                        }
                                    }
                                }
                            }

                            sw.Close();
                        }
                    }
                }

                MessageBox.Show("Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearPath(string path)
        {
            if (!Directory.Exists(path))
                return;

            foreach (var directory in new DirectoryInfo(path).GetDirectories())
            {
                directory.Delete(true);
            }
        }

        private void CheckPath(string path)
        {
            if(string.IsNullOrEmpty(path))
                throw new Exception("Invalid path");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}

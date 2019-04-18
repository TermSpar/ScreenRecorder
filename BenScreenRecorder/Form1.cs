using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace BenScreenRecorder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Filing variables:
        string outputPath = "";
        bool pathSelected = false;
        string finalVidName = "FinalVideo.mp4";
        int fRate = 10;

        // Screen recorder object:
        ScreenRecorder screenRec = new ScreenRecorder(new Rectangle(), "", 10);

        private void button4_Click(object sender, EventArgs e)
        {
            //Create output path:
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Select an Output Folder";

            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputPath = @folderBrowser.SelectedPath;
                pathSelected = true;

                //Finish screen recorder object:
                Rectangle bounds = Screen.FromControl(this).Bounds;
                if(txtFrameRate.Text != "")
                {
                    screenRec = new ScreenRecorder(bounds, outputPath, fRate);
                } else
                {
                    txtFrameRate.Text = "10";
                    fRate = 10;
                    screenRec = new ScreenRecorder(bounds, outputPath, fRate);
                }
            }
            else
            {
                MessageBox.Show("Please select an output folder.", "Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool containsMP4 = finalVidName.Contains(".mp4");

            if (pathSelected && containsMP4)
            {
                screenRec.setVideoName(finalVidName);
                tmrRecord.Start();
                pbRec.Show();
            }
            else if(!pathSelected && containsMP4)
            {
                MessageBox.Show("You must select an output path first", "Error");
            }
            else if(pathSelected && !containsMP4)
            {
                MessageBox.Show("You must select video name that ends in '.mp4'", "Error");
                txtSetName.Text = "FinalVideo.mp4";
                finalVidName = "FinalVideo.mp4";
            }
            else
            {
                MessageBox.Show("You must select video name that ends in '.mp4' " +
                    "and you must select an output path", "Error");
                txtSetName.Text = "FinalVideo.mp4";
                finalVidName = "FinalVideo.mp4";
            }
        }

        private void tmrRecord_Tick(object sender, EventArgs e)
        {
            screenRec.RecordVideo();
            screenRec.RecordAudio();
            lblTimer.Text = screenRec.getElapsed();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbRec.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tmrRecord.Stop();
            screenRec.Stop();
            pbRec.Hide();
            Application.Restart();
        }

        private void txtSetName_TextChanged(object sender, EventArgs e)
        {
            finalVidName = txtSetName.Text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            screenRec.cleanUp();
        }

        private void txtFrameRate_TextChanged(object sender, EventArgs e)
        {
            fRate = Convert.ToInt32(txtFrameRate.Text);
        }
    }
}

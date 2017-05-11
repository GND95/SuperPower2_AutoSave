using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace SP2Saver
{
    public partial class Form1 : Form
    {
        decimal autosaveTime = 5;
        decimal backupTime = 15;
        //decimal autosaveTimeConvert;
        //decimal backupTimeConvert;
        decimal autosaveTimeRemaining;
        decimal backupTimeRemaining;

        DateTime saveStartTime;
        DateTime backupStartTime;
        TimeSpan saveTimeDifference;
        TimeSpan backupTimeDifference;
        String saveSplitTime; //used so that i can split the hours mins and seconds into an array (eg: 00:00:00 into 00)
        String backupSplitTime;

        int progressBarIncrementer = 0;
        decimal progressBarValue;
        int saveCount = 1; //used for appending the incremented number to the backup file path so it doesn't overwrite the same file and creates a few one every time

        public Form1()
        {
            InitializeComponent();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (label8.Text == "-1") //once the minutes label goes to -1 that means it has reached its elapsed time and will restart to the input minutes and run again
            {
                saveStartTime = DateTime.Now;
                SendKeys.Send(textBox1.Text);
                SendKeys.Send("{Enter}");
            }
            if (label12.Text == "-1")
            {
                backupStartTime = DateTime.Now;
                SendKeys.Send(textBox4.Text + " " + saveCount + "\"");
                SendKeys.Send("{Enter}");
                saveCount++;
            }

            saveTimeDifference = DateTime.Now - saveStartTime; //difference in the current time and the time of the button press
            backupTimeDifference = DateTime.Now - backupStartTime; //difference in the current time and the time of the button press
            saveSplitTime = saveTimeDifference.ToString();
            backupSplitTime = backupTimeDifference.ToString();
            string[] saveHourMinSec = saveSplitTime.Split(':'); //splits the string (time) into hrs:mins:seconds
            string[] backupHourMinSec = backupSplitTime.Split(':'); //splits the string (time) into hrs:mins:seconds
            decimal saveSeconds = Convert.ToDecimal(saveHourMinSec[2]); //puts the seconds from the array into a decimal
            decimal backupSeconds = Convert.ToDecimal(backupHourMinSec[2]); //puts the seconds from the array into a decimal
            int saveMinutes = Convert.ToInt32(saveHourMinSec[1]); //puts the minutes from the array into an int
            int backupMinutes = Convert.ToInt32(backupHourMinSec[1]); //puts the minutes from the array into an int
            saveSeconds = (Math.Round(saveSeconds, 1)); // limits the amount of numbers after the decimal place to 1 digit
            backupSeconds = (Math.Round(backupSeconds, 1)); // limits the amount of numbers after the decimal place to 1 digit

            label3.Text = (60 - saveSeconds).ToString();
            label8.Text = (autosaveTimeRemaining - saveMinutes - 1).ToString();
            label14.Text = (60 - backupSeconds).ToString();
            label12.Text = (backupTimeRemaining - backupMinutes - 1).ToString();

            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.ReadOnly = true;
                textBox4.ReadOnly = true;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                textBox1.ReadOnly = false;
                textBox4.ReadOnly = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.PerformClick();
            saveStartTime = DateTime.Now; //time at the click of the button
            backupStartTime = DateTime.Now; //time at the click of the button
            timer2.Interval = Convert.ToInt32(1);
            timer1.Interval = Convert.ToInt32(autosaveTime);
            timer3.Interval = Convert.ToInt32(backupTime);

            timer1.Start();
            timer2.Start();
            timer3.Start();
            timer4.Start();
            progressBar1.Value = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            timer4.Stop();
            progressBar1.Value = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            autosaveTime = Convert.ToDecimal(textBox3.Text);
            autosaveTime = autosaveTime * 60 * 1000;
            backupTime = Convert.ToDecimal(textBox5.Text);
            backupTime = backupTime * 60 * 1000;

            autosaveTimeRemaining = Convert.ToDecimal(textBox3.Text);
            backupTimeRemaining = Convert.ToDecimal(textBox5.Text);

            progressBarValue = 300 / ((Convert.ToInt32(textBox3.Text)) * 60);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("True");
            comboBox1.Items.Add("False");
            comboBox1.SelectedIndex = 1;
            textBox1.ReadOnly = true;
            textBox4.ReadOnly = true;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            progressBarIncrementer++;
            if (progressBarIncrementer % 3 == 0) //every 3 seconds, increment the progress bar .99% (basically 1% because the .999 continues infinitely)
            {
                progressBar1.Increment(Convert.ToInt32(progressBarValue));
            }
            if (label8.Text == "0" && label3.Text == "0.0")
            {
                progressBar1.Value = 0;
            }
        }
    }
}

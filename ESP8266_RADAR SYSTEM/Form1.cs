using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace ESP8266_RADAR_SYSTEM
{
    public partial class Form1 : Form
    {
        SerialPort IR_sensor = new SerialPort("COM7", 9600); //Specify the COM port number for the computer where the ESP8266 is connected
        BackgroundWorker work = new BackgroundWorker(); //A background worker to read data from the sensor.
        public Form1()
        {
            InitializeComponent();
        }

       

        public void Form1_Load(object sender, EventArgs e)
        {
            try
            {
           
                if (IR_sensor.IsOpen == false) //check if the COM serial port is open.
                {
                    IR_sensor.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //function creates the blinking icon with the help of Windows forms timer.
            try {

                if (label1.BackColor == Color.Red)
                {
                    label1.BackColor = Color.Blue;

                }

                else
                {
                    label1.BackColor = Color.Red;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void button1_Click_1(object sender, EventArgs e)
        {
            // When the start button is clicked, a threat runs in the background and reads and displays the radar information.
            
            work.WorkerSupportsCancellation = true;
            work.DoWork += new DoWorkEventHandler(work_DoWork);

            timer1.Enabled = true;
            work.RunWorkerAsync();

            button1.Enabled = false;
            button2.Enabled = true;
            

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //Events stop when the stop button is clicked
            timer1.Enabled = false;
            if (work.IsBusy == true)//check if background thread is running.
            {
                work.CancelAsync(); //cancel the running thread and stop program execution.
                work.Dispose();
            }

            button1.Enabled = true;
            button2.Enabled = false;

        }

 
        void work_DoWork(object sender, DoWorkEventArgs e)
        {
            //this is the background Thread which reads data from the IO stream COM port of the ESP8266 Module. 

            try {
                while (!work.CancellationPending) //check if there is no request to cancel the thread.
                {
                    string proximity = IR_sensor.ReadLine();

                    Invoke(new Action(() =>
                    {
                        label6.Text = proximity;
                    }));



                    int proximity2 = Int32.Parse(proximity);


                    if (proximity2 <= 500)
                    {
                        Invoke(new Action(() =>
                        {
                            label1.Location = new Point(390, 85);
                        }));

                    }

                    else if (proximity2 > 500 && proximity2 <= 900)
                    {
                        Invoke(new Action(() =>
                        {
                            label1.Location = new Point(275, 103);
                        }));

                    }

                    else if (proximity2 > 900 && proximity2 <= 1025)
                    {
                        Invoke(new Action(() =>
                        {
                            label1.Location = new Point(136, 133);
                        }));

                    }
                    System.Threading.Thread.Sleep(5);  // Wait five minutes

                    if (work.CancellationPending == true)
                    {
                        e.Cancel = true;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

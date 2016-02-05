using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunFaderUI
{
    public partial class frmMain : Form
    {
        SerialFader serial = new SerialFader();

        public frmMain()
        {
            InitializeComponent();

            System.Threading.TimerCallback callback = new TimerCallback(StartSunrise);

            //first occurrence at
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);

            if (DateTime.Now < dt)
            {
                var timer = new System.Threading.Timer(callback, null,
                                //other occurrences every 24 hours
                                dt - DateTime.Now, TimeSpan.FromHours(24));
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Pins destPin;

            if (radWarm.Checked)
                destPin = Pins.Warm;
            else
                destPin = Pins.Cold;


            serial.SendCommand(new FadeCommand(
               destPin
                , Convert.ToByte(nudMax.Value) 
                , Convert.ToByte(nudStep.Value)
                , Convert.ToByte(nudDelay.Value)));
        }


        private void StartSunrise(object obj)
        {
            serial.SendCommand(new FadeCommand(
    Pins.Warm
    , Convert.ToByte(255) 
    , Convert.ToByte(1)
    , Convert.ToByte(255)));

        }
    }
}

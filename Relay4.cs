using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;


namespace pep.KeyStudio
{
    public class Relay4
    {
        int[] HWpinValues = new int[4] { 26, 6, 22, 4 };
        GpioPin[] pins = new GpioPin[4];
        GpioPinValue[] pinvalues = new GpioPinValue[4];
        string status;
        public Relay4()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pins[0] = null;
                pins[1] = null;
                pins[2] = null;
                pins[3] = null;
                status = "There is no GPIO controller on this device.";
                return;
            }
            for(var i = 0; i <= 3; i++)
            {
                pins[i] = gpio.OpenPin(HWpinValues[i]);
                pinvalues[i] = GpioPinValue.Low;
                pins[i].Write(GpioPinValue.Low);
                pins[i].SetDriveMode(GpioPinDriveMode.Output);

            }
            
            status = "GPIO pin initialized correctly.";
        }
        public void SetRelays(int relays)
        {
            if(relays > 255)
            {
                relays = 255;
            }
            string relayBit;
            relayBit = Reverse(Convert.ToString(relays, 2).PadLeft(4,'0'));
            SetRelays(relayBit);
        }
        public Boolean GetStatusBool(int relay)
        {
            if (pins[relay].Read() is GpioPinValue.Low)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void ToggleRelay(int relay)
        {
            if(pins[relay].Read() is GpioPinValue.Low)
            {
                pins[relay].Write(GpioPinValue.High);
            } else
            {
                pins[relay].Write(GpioPinValue.Low);
            }
        }
        public void SetRelays(string relayBit)
        {
            for (var i = 0; i <= 3; i++)
            {
                if (relayBit.Substring(i, 1) == "1")
                {
                    RelayON(i);
                }
                else
                {
                    RelayOFF(i);
                }
            }

        }
        public void RelayON(int relay)
        {
            pins[relay].Write(GpioPinValue.High);
            pinvalues[relay] = GpioPinValue.High;
        }
        public void RelayOFF(int relay)
        {
            pins[relay].Write(GpioPinValue.Low);
            pinvalues[relay] = GpioPinValue.Low;
        }
        public GpioPinValue getStatus(int relay)
        {
            return pinvalues[relay];
        }
        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

    }
}

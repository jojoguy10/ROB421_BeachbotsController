using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using SharpDX.XInput;

namespace BB_Controller
{
    public partial class Form1 : Form
    {
        MqttClient client = new MqttClient(IPAddress.Parse("192.168.0.104"));

        Controller controller = null;
        Guid controllerGUID = new Guid();
        State prevControllerState;

        public Form1()
        {
            InitializeComponent();

            try
            {
                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            controller = new Controller(UserIndex.One);

            try
            {
                if (controller.IsConnected)
                {
                    prevControllerState = controller.GetState();
                    timerJoystick.Start();
                }
            }
            catch
            {
                Console.WriteLine("No controller found!");
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            string strValue = Convert.ToString("Hello from Visual Studio!");
            switch (((System.Windows.Forms.Button)sender).Name)
            {
                case "btnUp":
                    Console.WriteLine("Up");
                    strValue = "Up";
                    break;

                case "btnDown":
                    Console.WriteLine("Down");
                    strValue = "Down";
                    break;

                case "btnLeft":
                    Console.WriteLine("Left");
                    strValue = "Left";
                    break;

                case "btnRight":
                    Console.WriteLine("Right");
                    strValue = "Right";
                    break;

                case "btnFire":
                    Console.WriteLine("FIRE!");
                    strValue = "FIRE!";
                    break;

                default:
                    break;
            }
            PublishMQTTMsg(strValue);
        }

        private void PublishMQTTMsg(string msg)
        {
            client.Publish("test_channel", Encoding.UTF8.GetBytes(msg));
        }

        private void timerJoystickPoll_Tick(object sender, EventArgs e)
        {
            State state = controller.GetState();
            if (prevControllerState.PacketNumber != state.PacketNumber)
            {
                Console.WriteLine(state.Gamepad.Buttons);
                if(state.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    Console.WriteLine("YOU HIT THE A BUTTON!");
                }
            }
            Thread.Sleep(10);
            prevControllerState = state;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(client.IsConnected)
            {
                client.Disconnect();
            }
        }
    }
}

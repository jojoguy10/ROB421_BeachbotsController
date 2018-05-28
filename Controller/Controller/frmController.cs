using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using SharpDX.XInput;

namespace BB_Controller
{
    public partial class frmController : Form
    {

        MqttClient client = new MqttClient(IPAddress.Parse("192.168.4.1"));

        Controller controller = null;
        Guid controllerGUID = new Guid();
        State prevControllerState;
        int[] prevStickValues = { 128, 128 };

        public frmController()
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

        private void PublishMQTTMsg(string msg, string channel = "test_channel")
        {
            client.Publish(channel, Encoding.UTF8.GetBytes(msg));
        }

        private void timerJoystickPoll_Tick(object sender, EventArgs e)
        {
            State state = controller.GetState();
            if (prevControllerState.PacketNumber != state.PacketNumber)
            {
                Console.WriteLine(state.Gamepad);
                if (state.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    PublishMQTTMsg("F");
                }

                if (state.Gamepad.Buttons == GamepadButtonFlags.B)
                {
                    PublishMQTTMsg("S");
                }
                

                // UP/DOWN
                if (state.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
                {
                    PublishMQTTMsg("U250");
                }
                else if (state.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
                {
                    PublishMQTTMsg("D250");
                }
                else
                {
                    PublishMQTTMsg("D0");
                    PublishMQTTMsg("U0");
                }



                // LEFT/RIGHT
                //if (state.Gamepad.LeftTrigger != prevStickValues[1])
                if (state.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
                {
                    //if (state.Gamepad.LeftTrigger < 128)
                    //{
                    //    //PublishMQTTMsg("L" + Map(state.Gamepad.LeftTrigger, 128, 0, 0, 300));
                    //}
                    //else if (state.Gamepad.LeftTrigger > 128)
                    //{
                    //    //PublishMQTTMsg("R" + Map(state.Gamepad.LeftTrigger, 128, 256, 0, 300));
                    //}
                    PublishMQTTMsg("L50");
                }
                else if (state.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
                {
                    PublishMQTTMsg("R50");
                }
                else
                {
                    PublishMQTTMsg("L0");
                    PublishMQTTMsg("R0");
                }
                    //prevStickValues[1] = state.Gamepad.LeftTrigger;
                //}

            }
            //Thread.Sleep(10);
            prevControllerState = state;
        }

        private void frmController_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
        }

        private long Map(long x, long in_min, long in_max, long out_min, long out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

    }
}

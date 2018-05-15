namespace BB_Controller
{
    partial class frmController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerJoystick = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerJoystick
            // 
            this.timerJoystick.Interval = 1;
            this.timerJoystick.Tick += new System.EventHandler(this.timerJoystickPoll_Tick);
            // 
            // frmController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 698);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmController";
            this.Text = "BeachBots Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmController_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerJoystick;
    }
}


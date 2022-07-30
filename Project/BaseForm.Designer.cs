using System.ComponentModel;

namespace Project
{
    partial class BaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.movementTimer = new System.Timers.Timer();
            this.miscTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize) (this.movementTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.miscTimer)).BeginInit();
            this.SuspendLayout();
            // 
            // movementTimer
            // 
            this.movementTimer.Enabled = true;
            this.movementTimer.Interval = 10D;
            this.movementTimer.SynchronizingObject = this;
            this.movementTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.MovementTimerElapsed);
            // 
            // miscTimer
            // 
            this.miscTimer.Enabled = true;
            this.miscTimer.Interval = 10D;
            this.miscTimer.SynchronizingObject = this;
            this.miscTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.MiscTimerElapsed);
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "BaseForm";
            this.Text = "Game";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClose);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyUp);
            this.Resize += new System.EventHandler(this.OnFormResize);
            ((System.ComponentModel.ISupportInitialize) (this.movementTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.miscTimer)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Timers.Timer movementTimer;
        
        private System.Timers.Timer miscTimer;

        #endregion
    }
}
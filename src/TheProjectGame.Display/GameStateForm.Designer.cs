namespace TheProjectGame.Display
{
    partial class GameStateForm
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
            this.gameStateControl = new TheProjectGame.Display.GameStateDisplayControl();
            this.SuspendLayout();
            // 
            // gameStateControl
            // 
            this.gameStateControl.DesiredGameSizeInPixels = new System.Drawing.Size(0, 0);
            this.gameStateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameStateControl.Location = new System.Drawing.Point(0, 0);
            this.gameStateControl.Name = "gameStateControl";
            this.gameStateControl.Size = new System.Drawing.Size(284, 262);
            this.gameStateControl.TabIndex = 0;
            // 
            // GameStateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.gameStateControl);
            this.DoubleBuffered = true;
            this.Name = "GameStateForm";
            this.Text = "Game State";
            this.ResumeLayout(false);

        }

        #endregion

        private GameStateDisplayControl gameStateControl;
    }
}


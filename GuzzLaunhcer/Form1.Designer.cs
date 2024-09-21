
namespace GuzzLaunhcer
{
    partial class GuzzLauncher
    {

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(925, 551);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "Change Download Path";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GuzzLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 611);
            this.Controls.Add(this.button1);
            this.Name = "GuzzLauncher";
            this.Text = "GuzzLauncher";
            this.Load += new System.EventHandler(this.GuzzLauncher_Load);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Button button1;





        /*
        private void InitializeGameBoxes()
        {

        }
        private void InitializeGameBox(GameBox gameBox)
        {
            gameBox.gameImage.Image = global::GuzzLaunhcer.Properties.Resources.artworks_000013535097_9rz0uo_t500x500;
            gameBox.gameImage.Location = new System.Drawing.Point(12, 12);
            gameBox.gameImage.Name = "pictureBox1";
            gameBox.gameImage.Size = new System.Drawing.Size(128, 128);
            gameBox.gameImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            gameBox.gameImage.TabIndex = 1;
            gameBox.gameImage.TabStop = false;
            gameBox.gameImage.Click += new System.EventHandler(this.pictureBox1_Click);





        }
    }

    
    

    public class GameBox
    {
        public System.Windows.Forms.PictureBox gameImage;
        public System.Windows.Forms.TextBox gameText;
    }

        */
    }
}


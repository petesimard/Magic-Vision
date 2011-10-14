namespace PoolVision
{
    partial class Form1
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
            this.hashCalcButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.image_output = new System.Windows.Forms.PictureBox();
            this.cam = new System.Windows.Forms.PictureBox();
            this.cardImage = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cardArtImage = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.camWindow = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cardInfo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.image_output)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardArtImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camWindow)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hashCalcButton
            // 
            this.hashCalcButton.Location = new System.Drawing.Point(660, 482);
            this.hashCalcButton.Name = "hashCalcButton";
            this.hashCalcButton.Size = new System.Drawing.Size(149, 23);
            this.hashCalcButton.TabIndex = 0;
            this.hashCalcButton.Text = "Calculate Hashes";
            this.hashCalcButton.UseVisualStyleBackColor = true;
            this.hashCalcButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Original Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(671, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Detected";
            // 
            // image_output
            // 
            this.image_output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.image_output.Location = new System.Drawing.Point(660, 25);
            this.image_output.Name = "image_output";
            this.image_output.Size = new System.Drawing.Size(320, 240);
            this.image_output.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.image_output.TabIndex = 4;
            this.image_output.TabStop = false;
            // 
            // cam
            // 
            this.cam.Location = new System.Drawing.Point(566, 12);
            this.cam.Name = "cam";
            this.cam.Size = new System.Drawing.Size(33, 37);
            this.cam.TabIndex = 7;
            this.cam.TabStop = false;
            this.cam.Visible = false;
            // 
            // cardImage
            // 
            this.cardImage.Location = new System.Drawing.Point(986, 25);
            this.cardImage.Name = "cardImage";
            this.cardImage.Size = new System.Drawing.Size(211, 298);
            this.cardImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cardImage.TabIndex = 11;
            this.cardImage.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(998, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Card";
            // 
            // cardArtImage
            // 
            this.cardArtImage.Location = new System.Drawing.Point(986, 351);
            this.cardArtImage.Name = "cardArtImage";
            this.cardArtImage.Size = new System.Drawing.Size(183, 133);
            this.cardArtImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cardArtImage.TabIndex = 13;
            this.cardArtImage.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(992, 335);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Artwork";
            // 
            // camWindow
            // 
            this.camWindow.Location = new System.Drawing.Point(15, 25);
            this.camWindow.Name = "camWindow";
            this.camWindow.Size = new System.Drawing.Size(640, 480);
            this.camWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.camWindow.TabIndex = 15;
            this.camWindow.TabStop = false;
            this.camWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.camWindow_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cardInfo);
            this.groupBox1.Location = new System.Drawing.Point(661, 347);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 129);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Card Information";
            // 
            // cardInfo
            // 
            this.cardInfo.Location = new System.Drawing.Point(6, 15);
            this.cardInfo.Multiline = true;
            this.cardInfo.Name = "cardInfo";
            this.cardInfo.Size = new System.Drawing.Size(235, 107);
            this.cardInfo.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 538);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.camWindow);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cardArtImage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cardImage);
            this.Controls.Add(this.cam);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.image_output);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hashCalcButton);
            this.Name = "Form1";
            this.Text = "Magic Vision";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.image_output)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardArtImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camWindow)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button hashCalcButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox image_output;
        private System.Windows.Forms.PictureBox cam;
        private System.Windows.Forms.PictureBox cardImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox cardArtImage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox camWindow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox cardInfo;
    }
}


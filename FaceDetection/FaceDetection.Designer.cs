namespace FaceDetection
{
    partial class FaceDetection
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
            this.faceImageBox = new Emgu.CV.UI.ImageBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.btnPicRecog = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.faceImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // faceImageBox
            // 
            this.faceImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.faceImageBox.Location = new System.Drawing.Point(3, 12);
            this.faceImageBox.Name = "faceImageBox";
            this.faceImageBox.Size = new System.Drawing.Size(364, 319);
            this.faceImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.faceImageBox.TabIndex = 2;
            this.faceImageBox.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(441, 399);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(434, 278);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(74, 23);
            this.btnBrowse.TabIndex = 18;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(421, 192);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 21;
            // 
            // imageBox1
            // 
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox1.Location = new System.Drawing.Point(373, 12);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(197, 172);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox1.TabIndex = 22;
            this.imageBox1.TabStop = false;
            // 
            // btnPicRecog
            // 
            this.btnPicRecog.Location = new System.Drawing.Point(480, 232);
            this.btnPicRecog.Name = "btnPicRecog";
            this.btnPicRecog.Size = new System.Drawing.Size(75, 23);
            this.btnPicRecog.TabIndex = 24;
            this.btnPicRecog.Text = "Recognize";
            this.btnPicRecog.UseVisualStyleBackColor = true;
            this.btnPicRecog.Click += new System.EventHandler(this.btnPicRecog_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(389, 232);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 26;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FaceDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 343);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnPicRecog);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.faceImageBox);
            this.MaximizeBox = false;
            this.Name = "FaceDetection";
            this.Text = "Face Detection and Recognition";
            this.Load += new System.EventHandler(this.FaceCap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.faceImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox faceImageBox;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox textBox1;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button btnPicRecog;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}


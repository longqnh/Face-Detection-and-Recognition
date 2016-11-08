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
            this.btn_Prev = new System.Windows.Forms.Button();
            this.btn_Next = new System.Windows.Forms.Button();
            this.btnLoadDB = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.btnNNRecognizer = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
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
            this.btnBrowse.Location = new System.Drawing.Point(431, 316);
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
            this.textBox1.ReadOnly = true;
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
            this.btnPicRecog.Location = new System.Drawing.Point(461, 259);
            this.btnPicRecog.Name = "btnPicRecog";
            this.btnPicRecog.Size = new System.Drawing.Size(97, 23);
            this.btnPicRecog.TabIndex = 24;
            this.btnPicRecog.Text = "Recognize (PCA)";
            this.btnPicRecog.UseVisualStyleBackColor = true;
            this.btnPicRecog.Click += new System.EventHandler(this.btnPicRecog_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(380, 271);
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
            // btn_Prev
            // 
            this.btn_Prev.Enabled = false;
            this.btn_Prev.Location = new System.Drawing.Point(389, 192);
            this.btn_Prev.Name = "btn_Prev";
            this.btn_Prev.Size = new System.Drawing.Size(26, 20);
            this.btn_Prev.TabIndex = 27;
            this.btn_Prev.Text = "<";
            this.btn_Prev.UseVisualStyleBackColor = true;
            this.btn_Prev.Click += new System.EventHandler(this.btn_Prev_Click);
            // 
            // btn_Next
            // 
            this.btn_Next.Enabled = false;
            this.btn_Next.Location = new System.Drawing.Point(527, 192);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(26, 20);
            this.btn_Next.TabIndex = 28;
            this.btn_Next.Text = ">";
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // btnLoadDB
            // 
            this.btnLoadDB.Location = new System.Drawing.Point(421, 228);
            this.btnLoadDB.Name = "btnLoadDB";
            this.btnLoadDB.Size = new System.Drawing.Size(100, 23);
            this.btnLoadDB.TabIndex = 29;
            this.btnLoadDB.Text = "Load Database";
            this.btnLoadDB.UseVisualStyleBackColor = true;
            this.btnLoadDB.Click += new System.EventHandler(this.btnLoadDB_Click);
            // 
            // tbStatus
            // 
            this.tbStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tbStatus.Location = new System.Drawing.Point(181, 337);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(220, 16);
            this.tbStatus.TabIndex = 30;
            this.tbStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnNNRecognizer
            // 
            this.btnNNRecognizer.Location = new System.Drawing.Point(460, 288);
            this.btnNNRecognizer.Name = "btnNNRecognizer";
            this.btnNNRecognizer.Size = new System.Drawing.Size(105, 23);
            this.btnNNRecognizer.TabIndex = 31;
            this.btnNNRecognizer.Text = "Recognize(Neural)";
            this.btnNNRecognizer.UseVisualStyleBackColor = true;
            this.btnNNRecognizer.Click += new System.EventHandler(this.btnNNRecognizer_Click);
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBox2.Location = new System.Drawing.Point(394, 344);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(172, 13);
            this.textBox2.TabIndex = 32;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FaceDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 371);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnNNRecognizer);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.btnLoadDB);
            this.Controls.Add(this.btn_Next);
            this.Controls.Add(this.btn_Prev);
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
        private System.Windows.Forms.Button btn_Prev;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.Button btnLoadDB;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button btnNNRecognizer;
        private System.Windows.Forms.TextBox textBox2;
    }
}


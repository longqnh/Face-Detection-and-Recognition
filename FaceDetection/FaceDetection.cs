using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using System.IO;
using System.Threading;

namespace FaceDetection
{
    public partial class FaceDetection : Form
    {
        private HaarCascade haar;

        Image<Bgr, byte> ImageFrame;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name;

        //Default values of haarCascade
        private int windowSize = 25;
        private Double ScaleRate = 1.1;
        private int minNeighbors = 3;

        public FaceDetection()
        {
            InitializeComponent();
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;
                int tf;
                for (tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void FaceCap_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_alt_tree.xml");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image InputImg = Image.FromFile(openFileDialog1.FileName);
                ImageFrame = new Image<Bgr, byte>(new Bitmap(InputImg));
                faceImageBox.Image = ImageFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                DetectFaces();
            }
        }

        private void btn_Prev_Click(object sender, EventArgs e)
        {

        }

        private void btn_Next_Click(object sender, EventArgs e)
        {

        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox1.Text = text;
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread((ThreadStart) =>
            {
                string path = Application.StartupPath + @"\Database";
                List<string> FileArray = new List<string>();
                foreach (string s in Directory.GetDirectories(path))
                {
                    FileArray.Add(s.Remove(0, s.LastIndexOf('\\') + 1));
                }

                for (int l = 0; l < FileArray.Count; l++)
                {
                    for (int j = 0; j < 11; j++)
                    {                        
                        Thread.Sleep(100);
                        string fil = path + "/" + FileArray[l] + "/" + FileArray[l] + "." + (j + 1) + ".jpg";
                        Image InputImg = Image.FromFile(fil);
                        ImageFrame = new Image<Bgr, byte>(new Bitmap(InputImg));

                        //Get a gray frame
                        gray = ImageFrame.Convert<Gray, byte>();

                        //Face Detector
                        var faces = gray.DetectHaarCascade(haar, ScaleRate, minNeighbors, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(windowSize, windowSize))[0];

                        //Action for each element detected
                        foreach (var face in faces)
                        {
                            TrainedFace = ImageFrame.Copy(face.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                            ImageFrame.Draw(face.rect, new Bgr(Color.Blue), 2);
                            trainingImages.Add(TrainedFace);
                            SetText(FileArray[l].ToString());
                            labels.Add(textBox1.Text);
                            break;
                        }

                        //Show face added in gray scale
                        imageBox1.Image = TrainedFace;

                        //Write the number of triained faces in a file text for further load
                        File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");
                        for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                        {
                            trainingImages.ToArray()[i - 1].Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC).Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                            File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                        }
                    }
                }
                MessageBox.Show("Training Successfully", "Message");
                btn_Next.Enabled = btn_Prev.Enabled = true;
                btnTrain.Enabled = false;
            });
            t1.Start();
    }

    private void DetectFaces()
        {
            Image<Gray, byte> grayframe = ImageFrame.Convert<Gray, byte>();

            var faces = grayframe.DetectHaarCascade(haar, ScaleRate, minNeighbors, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(windowSize, windowSize))[0];

            int n = 0;
            foreach (var face in faces)
            {
                ImageFrame.Draw(face.rect, new Bgr(Color.Red), 3);
                ++n;

            }
            //Display the detected faces in imagebox
            faceImageBox.Image = ImageFrame.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

        }

        private void btnPicRecog_Click(object sender, EventArgs e)
        {
            if (faceImageBox.Image != null)
            {
                NamePersons.Add("");

                //Convert it to Grayscale
                gray = ImageFrame.Convert<Gray, Byte>();

                //Face Detector
                var faces = gray.DetectHaarCascade(haar, ScaleRate, minNeighbors, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(windowSize, windowSize))[0];

                //Action for each element detected
                foreach (var face in faces)
                {
                    t = t + 1;
                    result = ImageFrame.Copy(face.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //draw the face detected in the 0th (gray) channel with blue color
                    //ImageFrame.Draw(f.rect, new Bgr(Color.Blue), 2);

                    try
                    {
                        if (trainingImages.ToArray().Length != 0)
                        {
                            //TermCriteria for face recognition with numbr of trained images like maxIteration
                            MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                            //Eigen face recognizer
                            EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                            trainingImages.ToArray(),
                            labels.ToArray(),
                            3000,
                            ref termCrit);

                            name = recognizer.Recognize(result);

                            //Draw the label for each face detected and recognized
                            //private Font font = new Font("Arial", 12, FontStyle.Bold); //creates new font
                            
                            ImageFrame.Draw(name, ref font, new Point(face.rect.X - 2, face.rect.Y - 2), new Bgr(Color.Blue));

                        }
                    }
                    catch (Exception ie)
                    {
                        MessageBox.Show(ie.ToString());
                    }
                    NamePersons[t - 1] = name;
                    NamePersons.Add("");
                }
                t = 0;

                //Show the faces procesed and recognized
                faceImageBox.Image = ImageFrame;

                //Clear the list(vector) of names
                NamePersons.Clear();
            }
            else
            {
                MessageBox.Show("Please load Image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

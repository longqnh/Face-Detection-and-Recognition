using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace FaceDetection
{
    public partial class FaceDetection : Form
    {
        private HaarCascade haar;        
        Image<Bgr, byte> ImageFrame;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, t;
        int CurrentFaceID;
        string name;

        //Default values of haarCascade
        private int windowSize = 25;
        private Double ScaleRate = 1.1;
        private int minNeighbors = 3;

        //Tuan's
        private List<Person> Persons;
        private PCA pca;
        private NNRecognizer recognizer;
        private bool DtbLoaded = false;

        public FaceDetection()
        {
            InitializeComponent();
            try
            {
                //Load of previus trainned faces and labels for each image
                //string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                //string[] Labels = Labelsinfo.Split('%');
                //NumLabels = Convert.ToInt16(Labels[0]);
                //ContTrain = NumLabels;
                //string LoadFaces;
                //int tf;
                //for (tf = 0; tf < NumLabels; tf++)
                //{
                //    LoadFaces = "face" + tf + ".bmp";
                //    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                //    labels.Add(Labels[tf]);
                //}

                using (var reader = new StreamReader(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt"))
                {
                    Persons = new List<Person>(Int32.Parse(reader.ReadLine()));
                    int imgID = 0;
                    for (int i = 0; i < Persons.Capacity; i++)
                    {
                        var NamenIDs = reader.ReadLine().Split('_');
                        Person tmpPerson = new Person();
                        tmpPerson.ID = i;
                        tmpPerson.Name = NamenIDs[0];
                        for (int j = 0; j < Int32.Parse(NamenIDs[1]); j++)
                        {
                            string imgFile = "face" + imgID + ".bmp";
                            trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + imgFile));
                            labels.Add(tmpPerson.Name);
                            tmpPerson.TrainingImagesID.Add(imgID++);
                        }
                        Persons.Add(tmpPerson);
                    }

                    ContTrain = Int32.Parse(reader.ReadLine());

                    //load pca
                    using (Stream stream = File.Open("PCA.bin", FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        pca = (PCA)bin.Deserialize(stream);
                    }

                    //load neural networks
                    using (Stream stream = File.Open("NN.bin", FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        recognizer = (NNRecognizer)bin.Deserialize(stream);
                    }
                    DtbLoaded = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Dtb reload Error, click Train to train again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Events
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
            if (CurrentFaceID > 0)
            {
                //CurrentFaceID--;
                //imageBox1.Image = trainingImages[CurrentFaceID];
                //textBox1.Text = Persons[CurrentFaceID / Persons.Count].Name;

                CurrentFaceID--;
                imageBox1.Image = trainingImages[CurrentFaceID];
                textBox1.Text = labels[CurrentFaceID];
            }
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (CurrentFaceID < trainingImages.Count - 1)
            {
                //CurrentFaceID++;
                //imageBox1.Image = trainingImages[CurrentFaceID];
                //textBox1.Text = Persons[CurrentFaceID / Persons.Count].Name;

                CurrentFaceID++;
                imageBox1.Image = trainingImages[CurrentFaceID];
                textBox1.Text = labels[CurrentFaceID];
            }
        }

        bool btnLoadDB_clicked = false;
        private void btnLoadDB_Click(object sender, EventArgs e)
        {
            if (trainingImages.Count > 0)
            {
                btn_Next.Enabled = btn_Prev.Enabled = true;
                CurrentFaceID = 0;
                imageBox1.Image = trainingImages[CurrentFaceID];
                textBox1.Text = Persons[CurrentFaceID / Persons.Count].Name;
                btnLoadDB_clicked = true;
            }
            else
            {
                MessageBox.Show("Database is now empty, please train some images", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        delegate void SetTextCallback(string text, int tb2);        

        private void btnTrain_Click(object sender, EventArgs e)
        {
            if (!DtbLoaded)
            {
                Train();
            }
            else
            {
                if(MessageBox.Show("Database trained, do you want to retrain?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Train();
                }
            }
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
                            ImageFrame.Draw(name, ref font, new Point(face.rect.X - 2, face.rect.Y - 2), new Bgr(Color.Blue));

                            for (int i = 0; i < labels.Count; i++)
                            {
                                if (labels[i] == name)
                                {
                                    imageBox1.Image = trainingImages[i];
                                    CurrentFaceID = i;
                                    textBox1.Text = name;
                                    if (!btnLoadDB_clicked)
                                    {
                                        btn_Next.Enabled = true;
                                        btn_Prev.Enabled = true;
                                    }
                                    break;
                                }
                            }

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

        private void btnNNRecognizer_Click(object sender, EventArgs e)
        {
            if (faceImageBox.Image != null)
            {
                NamePersons.Add("");

                //Convert it to Grayscale
                gray = ImageFrame.Convert<Gray, Byte>();

                //Face Detector
                var faces = gray.DetectHaarCascade(haar, ScaleRate, minNeighbors, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(windowSize, windowSize))[0];

                //try to recognize found faces
                foreach (var face in faces)
                {
                    var grayImg = ImageFrame.Copy(face.rect).Convert<Gray, byte>().Resize(50, 50, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    var grayImgVector = Get_ImgVector_From_GrayImg(grayImg);
                    var input = pca.CalculateOmegeImgInput(grayImgVector);
                    int i = recognizer.Run(input, Persons.Count);

                    //Draw the label for each face detected and recognized
                    ImageFrame.Draw(Persons[i].Name, ref font, new Point(face.rect.X - 2, face.rect.Y - 5), new Bgr(Color.Blue));
                    faceImageBox.Image = ImageFrame;

                    imageBox1.Image = trainingImages[Persons[i].TrainingImagesID[0]];
                    CurrentFaceID = Persons[i].TrainingImagesID[0];
                    textBox1.Text = Persons[i].Name;

                    //if (!btnLoadDB_clicked)
                    //{
                    //    btn_Next.Enabled = true;
                    //    btn_Prev.Enabled = true;
                    //}
                }
            }
            else
            {
                MessageBox.Show("Please load Image", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Methods
        private void Train()
        {
            string path = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                Thread t1 = new Thread(() =>
                {
                    Reset();

                    var Names = Directory.GetDirectories(path);
                    for (int i = 0; i < Names.Length; i++)
                    {
                        Person tmpPerson = new Person();
                        tmpPerson.ID = i;
                        tmpPerson.Name = Names[i].Remove(0, Names[i].LastIndexOf('\\') + 1);
                        Persons.Add(tmpPerson);
                    }

                    for (int i = 0; i < Persons.Count; i++)
                    {
                        var Files = Directory.GetFiles(path + "/" + Persons[i].Name);
                        for (int j = 0; j < Files.Length; j++)
                        {
                            Thread.Sleep(100);
                            ImageFrame = new Image<Bgr, byte>(new Bitmap(Image.FromFile(Files[j])));

                            //Get a gray frame
                            gray = ImageFrame.Convert<Gray, byte>();
                            //Face Detector
                            var faces = gray.DetectHaarCascade(haar, ScaleRate, minNeighbors, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(windowSize, windowSize))[0];

                            //Action for each element detected
                            foreach (var face in faces)
                            {
                                //convert detected face to gray img
                                var TrainedFace = ImageFrame.Copy(face.rect).Convert<Gray, byte>().Resize(50, 50, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                                //Save images of ith person
                                Persons[i].TrainingImagesID.Add(trainingImages.Count);
                                //Save detected face
                                //TrainedFace.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC)
                                TrainedFace.Save(Application.StartupPath + "/TrainedFaces/face" + trainingImages.Count + ".bmp");
                                //Show face added in gray scale
                                imageBox1.Image = TrainedFace;
                                SetText(Persons[i].Name);

                                //
                                trainingImages.Add(TrainedFace);
                                labels.Add(textBox1.Text);//
                            }
                        }
                    }
                    CurrentFaceID = trainingImages.Count - 1;
                    WriteText();

                    //PCA
                    SetText("PCA Running, please wait", 0);
                    pca = new PCA();
                    foreach (var img in trainingImages)
                    {
                        pca.AddImage(Get_ImgVector_From_GrayImg(img));
                    }
                    pca.CalculateEigenFaces();
                    SavePCA();

                    //Neural
                    SetText("GA Training in process, please wait", 0);
                    recognizer = new NNRecognizer(Persons.Count, pca.K, 1);
                    recognizer.Train(pca, Persons, this);
                    SaveNN();

                    SetText("", 0);
                    MessageBox.Show("Training Successfully", "Message");
                });

                t1.Start();
                btnTrain.Enabled = false;
                btn_Next.Enabled = btn_Prev.Enabled = true;
            }
        }

        private void SavePCA()
        {
            using (Stream stream = File.Open("PCA.bin", FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                try
                {
                    bin.Serialize(stream, pca);
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        private void SaveNN()
        {
            using (Stream stream = File.Open("NN.bin", FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                try
                {
                    bin.Serialize(stream, recognizer);
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        private void Reset()
        {
            if (Persons == null)
                Persons = new List<Person>();
            else
                Persons.Clear();

            trainingImages.Clear();
        }

        private void WriteText()
        {
            string path = Application.StartupPath + "/TrainedFaces/TrainedLabels.txt";
            File.WriteAllText(path, Persons.Count.ToString() + "\r\n");
            int sum = 0;
            foreach (var person in Persons)
            {
                File.AppendAllText(path, person.Name + "_" + person.TrainingImagesID.Count.ToString() + "\r\n");
                sum += person.TrainingImagesID.Count;
            }
            File.AppendAllText(path, sum.ToString());
        }

        public void SetText(string text, int tb2 = -1)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, tb2});
            }
            else
            {                
                if (tb2 == 0)
                {
                    tbStatus.Text = text;
                }
                else if (tb2==1)
                {
                    textBox2.Text = text;
                }
                else
                    this.textBox1.Text = text;
            }
        }

        private double[,] Get_ImgVector_From_GrayImg(Image<Gray, byte> GrayImg)
        {
            Matrix<byte> mat = new Matrix<byte>(GrayImg.Height, GrayImg.Width, 1);
            GrayImg.CopyTo(mat);

            double[,] ImgVector = new double[GrayImg.Height * GrayImg.Width, 1];
            int i = 0;
            for (int col = 0; col < mat.Cols; col++)
            {
                for (int row = 0; row < mat.Rows; row++)
                {
                    ImgVector[i++, 0] = mat[row, col];
                }
            }
            return ImgVector;
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
        #endregion
    }
}

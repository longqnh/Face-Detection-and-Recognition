using NeuronDotNet.Core;
using NeuronDotNet.Core.Backpropagation;
using NeuronDotNet.Core.Initializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Runtime.Serialization;

namespace FaceDetection
{
    [Serializable]
    class NeuralNetwork : ISerializable
    {
        public BackpropagationNetwork network;
        private int Epochs;

        #region Constructors
        public NeuralNetwork(int inputcount, int outputcount, int hiddencount, double learnrate, int epochs)
        {
            var inputLayer = new SigmoidLayer(inputcount);
            var hiddenLayer = new SigmoidLayer(hiddencount);
            var outputLayer = new SigmoidLayer(outputcount);

            new BackpropagationConnector(inputLayer, hiddenLayer).Initializer = new NguyenWidrowFunction();
            new BackpropagationConnector(hiddenLayer, outputLayer).Initializer = new RandomFunction(0d, 0.3d);

            network = new BackpropagationNetwork(inputLayer, outputLayer);
            network.SetLearningRate(learnrate);

            Epochs = epochs;
        }

        //Deserialization constructor
        public NeuralNetwork(SerializationInfo info, StreamingContext ctxt)
        {
            network = (BackpropagationNetwork)info.GetValue("Net", typeof(BackpropagationNetwork));
            Epochs = (int)info.GetValue("epo", typeof(int));
        }
        #endregion

        #region Methods
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Net", network);
            info.AddValue("epo", Epochs);
        }

        public void Learn(TrainingSet trainingset)
        {
            network.Learn(trainingset, Epochs);
        }

        public double Run(double[] input)
        {
            var result = network.Run(input);

            return result[0];
        }
        #endregion
    }

    [Serializable]
    class NNRecognizer: ISerializable
    {
        public List<NeuralNetwork> Networks;
        private int n;

        #region Constructor
        public NNRecognizer(int n, int inputcount, int outputcount, int hiddencount = 10, double learnrate = 0.35, int epochs = 1000)
        {
            Networks = new List<NeuralNetwork>();
            for (int i = 0; i < n; i++)
            {
                NeuralNetwork tmpNet = new NeuralNetwork(inputcount, outputcount, hiddencount, learnrate, epochs);
                Networks.Add(tmpNet);
            }

            this.n = n;
        }

        //Deserialization constructor
        public NNRecognizer(SerializationInfo info, StreamingContext ctxt)
        {
            Networks = (List<NeuralNetwork>)info.GetValue("Nets", typeof(List<NeuralNetwork>));
            n = (int)info.GetValue("no", typeof(int));
        }
        #endregion

        #region Methods
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Nets", Networks);
            info.AddValue("no", n);
        }

        private void Train_ith_NN(int i, PCA pca, List<Person> Persons, FaceDetection main)
        {
            //create trainingset
            var trainingset = new TrainingSet(pca.K, 1);
            int trainedtmp = 1;

            //add training samples
            //train full
            for (int j = 0; j < pca.PiImages.Count; j++)
            {
                if (Persons[i].TrainingImagesID.Contains(j))
                {
                    trainingset.Add(new TrainingSample(pca.GetOmegaImg(j), new double[] { 1 }));
                    //main.SetText(i.ToString() + "th person - " + (trainedtmp++).ToString(), 1);
                }
                else
                {
                    trainingset.Add(new TrainingSample(pca.GetOmegaImg(j), new double[] { 0 }));
                    //main.SetText(i.ToString() + "th person - " + (trainedtmp++).ToString(), 1);
                }
            }
            //learn
            Networks[i].Learn(trainingset);


            ////train 1/3 images
            //int imgID = 0;
            //for (int p = 0; p < Persons.Count; p++)
            //{
            //    int imgofperson = 0;
            //    for (; imgID < Persons[p].TrainingImagesID.Max(); imgID++) //imgID still <= imagesID of p-th person
            //    {
            //        if (p == i) //if p-th person is the i-th person (the one need to recognize)
            //        {
            //            trainingset.Add(new TrainingSample(pca.GetOmegaImg(imgID), new double[] { 1 }));
            //            Networks[i].Learn(trainingset);
            //            trainingset.Clear();
            //            main.SetText(i.ToString() + "th person - " + (trainedtmp++).ToString());
            //        }
            //        else
            //        {
            //            trainingset.Add(new TrainingSample(pca.GetOmegaImg(imgID), new double[] { 0 }));
            //            Networks[i].Learn(trainingset);
            //            trainingset.Clear();
            //            main.SetText(i.ToString() + "th person - " + (trainedtmp++).ToString());
            //        }

            //        imgofperson++;
            //        if (imgofperson >= Persons[p].TrainingImagesID.Count / 3)
            //        //if (imgofperson >= 1)
            //        {
            //            if ((p + 1) < Persons.Count)
            //            {
            //                imgID = Persons[p + 1].TrainingImagesID[0]; //set imgID to the first training img of next person
            //            }
            //            break;
            //        }
            //    }
            //}
        }

        private void trainfirstperson(PCA pca)
        {
            //create trainingset
            var trainingset = new TrainingSet(pca.K, 1);
            trainingset.Add(new TrainingSample(pca.GetOmegaImg(0), new double[] { 1 }));
            Networks[0].Learn(trainingset);
        }

        public void Train(PCA pca, List<Person> Persons, FaceDetection main)
        {
            //train n networks
            for (int i = 0; i < Persons.Count; i++)
            {
                Train_ith_NN(i, pca, Persons, main);
            }
            //trainfirstperson(pca);
        }        

        public int Run(double[] input, int n)
        {
            List<double> outputs = new List<double>();
            for (int i = 0; i < n; i++)
            {
                outputs.Add(Networks[i].Run(input));
            }

            return outputs.IndexOf(outputs.Max());
        }
        #endregion
    }
}

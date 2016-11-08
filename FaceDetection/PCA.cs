using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Runtime.Serialization;

namespace FaceDetection
{
    [Serializable]
    class PCA: ISerializable
    {
        public List<double[,]> Images;
        public List<double[,]> PiImages;
        public List<double[,]> EigenFaces;
        public int K
        {
            get
            {
                return EigenFaces.Count;
                //var tmp = EigenFaces[0];
                //return tmp.GetLength(0);
            }
        }

        #region Constructor
        public PCA()
        {
            Images = new List<double[,]>();
            PiImages = new List<double[,]>();
            EigenFaces = new List<double[,]>();
        }

        //Deserialization constructor
        public PCA(SerializationInfo info, StreamingContext ctxt)
        {
            Images = (List<double[,]>)info.GetValue("Imgs", typeof(List<double[,]>));
            PiImages = (List<double[,]>)info.GetValue("PiImgs", typeof(List<double[,]>));
            EigenFaces = (List<double[,]>)info.GetValue("EigenVector", typeof(List<double[,]>));
        }
        #endregion

        #region Methods
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Imgs", Images);
            info.AddValue("PiImgs", PiImages);
            info.AddValue("EigenVector", EigenFaces);
        }

        private Matrix<double> ConvertPhiImagesToMatrix()
        {
            int Rows = PiImages[0].GetLength(0);
            int Cols = PiImages.Count;

            double[,] tmp = new double[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    tmp[i, j] = PiImages[j][i, 0];
                }
            }
            return DenseMatrix.OfArray(tmp);
        }
        
        private double[,] GetAveVector()
        {
            int rows = Images[0].GetLength(0);
            double[,] Ave = new double[rows, 1]; //alwasy N^2x1

            for (int i = 0; i < rows; i++)
            {
                double sum = 0;
                foreach (var img in Images)
                {
                    sum += img[i, 0];
                }
                Ave[i, 0] = (sum / Images.Count); //sum / M_images
            }

            return Ave;
        }

        private void GetPhiImages(double[,] AveVector)
        {
            foreach (var img in Images)
            {
                int rows = img.GetLength(0);
                double[,] tmpPhiImg = new double[rows, 1];

                for (int i = 0; i < rows; i++)
                {
                    tmpPhiImg[i, 0] = (img[i, 0] - AveVector[i, 0]);
                }

                PiImages.Add(tmpPhiImg);
            }
        }

        public double[] GetOmegaImg(int j)
        {
            var jth_PiImg = PiImages[j];
            var PiImgMatrix = DenseMatrix.OfArray(jth_PiImg);
            
            double[] OmegaImg = new double[K];            
            for (int i = 0; i < K; i++)
            {
                OmegaImg[i] = (DenseMatrix.OfArray(EigenFaces[i]).Transpose() * PiImgMatrix)[0, 0];
            }

            return OmegaImg;
        }

        private void GetEigenFaces()
        {
            var matrixA = ConvertPhiImagesToMatrix();

            var Covariance = matrixA * matrixA.Transpose();

            var evd = Covariance.Evd();

            var eigenMatrix = evd.EigenVectors;

            //convert matrix to vectors
            //take last column in the matrix which is the largest eigenvalue
            //take images sqrt(n) images
            //for (int col = eigenMatrix.ColumnCount - 1; EigenFaces.Count < Math.Sqrt(eigenMatrix.ColumnCount); col--)
            for (int col = eigenMatrix.ColumnCount - 1; EigenFaces.Count < 50; col--)
            {
                double[,] tmpEigenVector = new double[eigenMatrix.RowCount, 1];
                for (int i = 0; i < eigenMatrix.RowCount; i++)
                {
                    tmpEigenVector[i, 0] = eigenMatrix[i, col];
                }
                EigenFaces.Add(tmpEigenVector);
            }
        }

        public void AddImage(double[,] img)
        {
            Images.Add(img);
        }

        public void CalculateEigenFaces()
        {
            var AveVector = GetAveVector();
            GetPhiImages(AveVector);
            GetEigenFaces();
        }

        public double[] CalculateOmegeImgInput(double[,] GrayImgVector)
        {
            //calculate ImgVector to piImg 
            var ave = GetAveVector();
            int rows = GrayImgVector.GetLength(0);
            //
            double[,] piImg = new double[rows, 1];
            for (int i = 0; i < rows; i++)
            {
                piImg[i, 0] = GrayImgVector[i, 0] - ave[i, 0];
            }

            //convert to matrix
            var PiImgMatrix = DenseMatrix.OfArray(piImg);

            //calculate OmegaImg
            double[] OmegaImg = new double[K];
            for (int i = 0; i < K; i++)
            {
                OmegaImg[i] = (DenseMatrix.OfArray(EigenFaces[i]).Transpose() * PiImgMatrix)[0, 0];
            }

            return OmegaImg;
        }
        #endregion
    }
}

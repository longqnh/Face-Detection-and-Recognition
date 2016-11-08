using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceDetection
{
    class Person
    {
        public int ID;
        public string Name;
        public List<int> TrainingImagesID;

        public Person()
        {
            TrainingImagesID = new List<int>();
        }
    }
}

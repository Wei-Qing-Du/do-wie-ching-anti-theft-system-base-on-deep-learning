using NUnit.Framework;
using WpfCamera;
using Emgu.CV;
using System.IO;
using Emgu.CV.Structure;
using System;

namespace ShowDataTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {

            DirectoryInfo dir = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            string currentPath = dir.Parent.Parent.FullName;
            currentPath += @"\TestData\Test.jpg";

            using (var mat = CvInvoke.Imread(currentPath, Emgu.CV.CvEnum.LoadImageType.AnyColor))
            {
                Image<Bgr, Byte> img = mat.ToImage<Bgr, Byte>();
                using (var bmp = img.ToBitmap())
                {
                    Assert.IsTrue( ProcessIMG.ProcessImage(bmp));
                }
            }
                       
        }
    }
}
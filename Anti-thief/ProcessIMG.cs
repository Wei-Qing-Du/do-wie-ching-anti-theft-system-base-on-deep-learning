using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;

namespace WpfCamera
{
    public class ProcessIMG
    {
        static public bool ProcessImage(Bitmap bmp)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
            string currentPath = dir.Parent.Parent.FullName;
            currentPath += @"\DetectData\haarcascade_frontalface_alt.xml";

            CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;
            var faceCascade = new CascadeClassifier(currentPath);
            var img = new Image<Bgr, byte>(bmp);
            var img2 = new Image<Gray, byte>(img.ToBitmap());
            CvInvoke.EqualizeHist(img2, img2);
            var faces = faceCascade.DetectMultiScale(img2, 1.1, 10, new System.Drawing.Size(80, 80));

            return faces.Length > 0 ? true : false;
        }
    }
}

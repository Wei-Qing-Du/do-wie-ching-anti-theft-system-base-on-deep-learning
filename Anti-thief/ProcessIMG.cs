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
            Bitmap resize_bmp = new Bitmap(bmp, new Size(bmp.Width / 4, bmp.Height / 4));
            var img = new Image<Bgr, byte>(resize_bmp);
            var img2 = new Image<Gray, byte>(img.ToBitmap());
            CvInvoke.EqualizeHist(img2, img2);
            var faces = faceCascade.DetectMultiScale(img2, 1.1, 10, new System.Drawing.Size(80, 80));

#if DEBUG
            foreach (var face in faces)
            {
                int x = face.X;
                int y = face.Y;
                int w = face.Width;
                int h = face.Height;

                Rectangle rect = new Rectangle(x, y, w, h);
                MCvScalar color = new MCvScalar(0, 0, 255);
                CvInvoke.Rectangle(img, rect, color, 5);
            }

            CvInvoke.Imshow("My Window", img);
            CvInvoke.WaitKey();
#endif

            return faces.Length > 0 ? true : false;
        }
    }
}

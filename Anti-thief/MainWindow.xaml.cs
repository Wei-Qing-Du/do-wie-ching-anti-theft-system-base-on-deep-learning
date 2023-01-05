using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace WpfCamera
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MediaCapture _mediaCapture = new MediaCapture();
		private readonly CaptureElement _captureElement;
		private StorageFolder _captureFolder;
		private bool _initialized = false;
		private ShowDataWindow _showData = new ShowDataWindow();


		public Bitmap bmp { get; private set; }

        public MainWindow()
		{
			InitializeComponent();

			_captureElement = new CaptureElement
			{
				Stretch = Windows.UI.Xaml.Media.Stretch.Uniform
			};
			_captureElement.Loaded += CaptureElement_Loaded;
			_captureElement.Unloaded += CaptureElement_Unloaded;

			XamlHost.Child = _captureElement;
		}

		private async void CaptureElement_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			await _mediaCapture.StopPreviewAsync();
		}

		private async void CaptureElement_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if (!_initialized)
			{
				var picturesLibrary = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
				// Fall back to the local app storage if the Pictures Library is not available
				_captureFolder = picturesLibrary.SaveFolder ?? ApplicationData.Current.LocalFolder;

				// Get available devices for capturing pictures
				var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

				if(allVideoDevices.Count > 0)
				{
					// try to find back camera
					DeviceInformation desiredDevice = allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

					// If there is no device mounted on the back panel, return the first device found
					var device = desiredDevice ?? allVideoDevices.FirstOrDefault();

					await _mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings() { VideoDeviceId = device.Id });
					_captureElement.Source = _mediaCapture;

					_initialized = true;
				}
			}

			if (_initialized)
			{
				await _mediaCapture.StartPreviewAsync();
			}
		}

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow = _showData;

            this.Close();

			_showData.Show();
        }
        private async void Photo_Click(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
			{
				return;
			}

			try
			{
				// Prepare and capture photo
				var lowLagCapture = await _mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

				var capturedPhoto = await lowLagCapture.CaptureAsync();
				var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

				using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
				{
					BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
					encoder.SetSoftwareBitmap(softwareBitmap);
					await encoder.FlushAsync();
					bmp = new System.Drawing.Bitmap(stream.AsStream());
				}
					await lowLagCapture.FinishAsync();
#if false
				string filePath = @"C:\Users\Willy\Desktop\MyImage.png";
				bmp.Save(filePath, ImageFormat.Png);
#endif
				ProcessImage(bmp);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ProcessImage(Bitmap bmp)
		{
			DirectoryInfo dir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
			string currentPath = dir.Parent.Parent.Parent.FullName;
			currentPath += @"\DetectData\haarcascade_frontalface_alt.xml";

			CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;
			var faceCascade = new CascadeClassifier(currentPath);
			var img = new Image<Bgr, byte>(bmp);
			var img2 = new Image<Gray, byte>(img.ToBitmap());
			CvInvoke.EqualizeHist(img2, img2);
			var faces = faceCascade.DetectMultiScale(img2, 1.1, 10, new System.Drawing.Size(80, 80));

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
#if false
			CvInvoke.Imshow("My Window", img);
			CvInvoke.WaitKey();
#endif
		}
	}
}

using System;
using System.Collections.Generic;
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
    public partial class ShowDataWindow : Window
    {
        public List<ListResult> items { get; set; }
        public ShowDataWindow()
        {
            InitializeComponent();
            items = new List<ListResult>();
            items.Add(new ListResult { Time = DateTime.Now.ToString(), Detect = "Yes", Intruder = "No" });
            items.Add(new ListResult { Time = DateTime.Now.ToString(), Detect = "Yes", Intruder = "Yes" });
            DataContext = this;
        }
    }

    public class ListResult
    {
        public string Time { get; set; }

        public string Detect { get; set; }
        public string Intruder { get; set; }
    }
}

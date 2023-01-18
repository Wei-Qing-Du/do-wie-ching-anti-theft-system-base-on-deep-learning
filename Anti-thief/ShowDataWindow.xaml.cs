using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Data.SQLite;
using System.IO;
using System;

namespace WpfCamera
{
    public partial class ShowDataWindow : Window
    {
        public List<ListResult> items { get; set; }
        public ShowDataWindow()
        {
            InitializeComponent();
            items = new List<ListResult>();
           // items.Add(new ListResult { Time = DateTime.Now.ToString(), Detect = "Yes", Intruder = "No" });
            //items.Add(new ListResult { Time = DateTime.Now.ToString(), Detect = "Yes", Intruder = "Yes" });
            DataContext = this;
        }

        public void AddData2DataBase()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
                string currentPath = dir.Parent.Parent.FullName;
                currentPath += @"\DetectData\ShowData.db";

                string dbFile = currentPath;
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFile)))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO Detect (Time, Detect, Intruder) VALUES (@value1, @value2, @value3)", connection))
                    {
                        ListResult lastItem = items.Last();
                        command.Parameters.AddWithValue("@value1", lastItem.Time);
                        command.Parameters.AddWithValue("@value2", lastItem.Detect);
                        command.Parameters.AddWithValue("@value3", lastItem.Intruder);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
    }

    public class ListResult
    {
        public string Time { get; set; }

        public string Detect { get; set; }
        public string Intruder { get; set; }
    }
}

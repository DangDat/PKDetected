using ParkinsonDetected.Model;
using Microsoft.Band.Sensors;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Xml.Linq;
using Windows.Storage;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using ParkinsonDetected.Common;
using Windows.UI.Xaml.Media;
using Windows.UI;


namespace ParkinsonDetected.Pages
{
    public partial class Drawing
    {
        private DrawingModel model;
        //private Task updateUI = null;
        private List<string> AccList = new List<string>();
        private String name_set;
        private String lable_set;
        private double changing_time = 0.1;
        private double delay_time = 1;
        
        string myTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");
        public Drawing()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.model = new DrawingModel();
            this.DataContext = this.model;
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            PassedData data = e.NavigationParameter as PassedData;
            string name = data.Name_Patient.ToString();
            string lable = data.Disease_Status.ToString();
            //string name = e.NavigationParameter as string;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name_set = name + ".txt";
            }
            else
            {
                name_set = "Fail_data.txt";
            }

            if (!string.IsNullOrWhiteSpace(lable))
            {
                lable_set = lable;
            }
            else
            {
                lable_set = "None";
            }
        }

      
        private async void Button_Click_Begin(object sender, RoutedEventArgs e)
        {
            btn_begin.IsEnabled = false;
            
            try
            {
                //change Color
                changeColor(); 
                model.Main.BandClient.SensorManager.Accelerometer.ReportingInterval = TimeSpan.FromMilliseconds(128.0);

                //subscribe to Accelerometer
                model.Main.BandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                await model.Main.BandClient.SensorManager.Accelerometer.StartReadingsAsync();
                
                // Receive Accelerometer data for a while.
                await Task.Delay(TimeSpan.FromMinutes(delay_time));
                
                await model.Main.BandClient.SensorManager.Accelerometer.StopReadingsAsync();
               // ProjectFile(AccList, "LyingData.txt");
               
               ProjectFile(AccList, name_set);
               this.textBlock.Text = "You Can Return";
               
            }
            catch (Exception ex)
            {
                btn_begin.IsEnabled = true;
            }

        }

        private void Button_Click_End(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
            this.InitializeComponent();
            this.model = new DrawingModel();
            this.DataContext = this.model;
           
          
        }

        private async void Accelerometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        {
            IBandAccelerometerReading accel = e.SensorReading;
            //ProjectFile(accel.AccelerationX, accel.AccelerationY, accel.AccelerationZ, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
            string text = string.Format("X = {0}\nY = {1}\nZ = {2}\n Label ={3}\n DateTime = {4}", accel.AccelerationX, accel.AccelerationY, accel.AccelerationZ, lable_set,  DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
            string text1 = string.Format("{0} {1} {2} {3} \n", accel.AccelerationX, accel.AccelerationY, accel.AccelerationZ, lable_set);
            AccList.Add(text1);
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.textBlock.Text = text; }).AsTask();
        }

        private async void ProjectFile(List<string> list, String nameFileData)
        {
            var localAppFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            // Create a new folder name DataFolder.
            var dataFolder = await localAppFolder.CreateFolderAsync("Data", CreationCollisionOption.OpenIfExists);

            var fileHandle = await dataFolder.CreateFileAsync(nameFileData, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            foreach (string text_file in list)
            {
                await Windows.Storage.FileIO.AppendTextAsync(fileHandle, text_file);
            }
        }

        async Task changeColor()
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values.  
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(120, 0, 255, 0);
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip1.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip2.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip3.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip4.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip5.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip6.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip7.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip8.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip9.Fill = mySolidColorBrush;
            await Task.Delay(TimeSpan.FromMinutes(changing_time));
            Ellip10.Fill = mySolidColorBrush;
        }
    }
}

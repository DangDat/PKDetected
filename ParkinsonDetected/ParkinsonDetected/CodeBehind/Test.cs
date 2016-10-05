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

namespace ParkinsonDetected.Pages
{
    public partial class Test
    {
        private TestModel model;
        //private Task updateUI = null;
        //private List<string> AccList = new List<string>();

        public string name_data;
       

        //string myTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");
        public Test()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.model = new TestModel();
            this.DataContext = this.model;
           
        }


        private void Button_Click_Lying(object sender, RoutedEventArgs e)
        {
           
            try
            {
                // Navigate to the appropriate destination page, configuring the new page
                // by passing required information as a navigation parameter
                name_data = txt_name.Text;

               // ((Frame)Window.Current.Content).Navigate(typeof(Pages.Drawing));
                ((Frame)Window.Current.Content).Navigate(typeof(Pages.Drawing), new PassedData { Name_Patient = txt_name.Text, Disease_Status = txt_label.Text });
                //this.Frame.Navigate(typeof(Drawing), txt_name.Text);
                

                //model.Main.BandClient.SensorManager.Accelerometer.ReportingInterval = TimeSpan.FromMilliseconds(128.0);

                ////subscribe to Accelerometer
                //model.Main.BandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                //await model.Main.BandClient.SensorManager.Accelerometer.StartReadingsAsync();

                //// Receive Accelerometer data for a while.
                //await Task.Delay(TimeSpan.FromMinutes(1));
                //await model.Main.BandClient.SensorManager.Accelerometer.StopReadingsAsync();
                //ProjectFile(AccList, "LyingData.txt");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

       
            
        }

        //private async void Button_Click_Result(object sender, RoutedEventArgs e)
        //{
        //    btnStart.IsEnabled = true;
        //   // await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.textBlock.Text = "Parkinson Possitive: 89%"; }).AsTask();
        //}

        
        //private async void Accelerometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        //{
        //    IBandAccelerometerReading accel = e.SensorReading;
        //    //ProjectFile(accel.AccelerationX, accel.AccelerationY, accel.AccelerationZ, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
        //    string text = string.Format("X = {0}\nY = {1}\nZ = {2}\nDateTime = {3}", accel.AccelerationX, accel.AccelerationY, accel.AccelerationZ, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
        //    string text1 = string.Format("{0} {1} {2} {3}\n", accel.AccelerationX, accel.AccelerationY, accel.AccelerationZ, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
        //    AccList.Add(text1);
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { this.textBlock.Text = text; }).AsTask();
        //}

        //private async void ProjectFile(List<string> list, String nameFileData)
        //{
        //    var localAppFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        //    // Create a new folder name DataFolder.
        //    var dataFolder = await localAppFolder.CreateFolderAsync("Data",CreationCollisionOption.OpenIfExists);

        //    var fileHandle = await dataFolder.CreateFileAsync(nameFileData, Windows.Storage.CreationCollisionOption.ReplaceExisting);

        //    foreach (string text_file in list)
        //    {
        //    await Windows.Storage.FileIO.AppendTextAsync(fileHandle,text_file);
        //    }
        //}
    }

    // let's assume that you have a simple class:
    public class PassedData
    {
        public string Name_Patient { get; set; }
        public string Disease_Status { get; set; }
    }
}

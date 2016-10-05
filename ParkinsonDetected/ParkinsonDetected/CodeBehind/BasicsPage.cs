/*
    Copyright (c) Microsoft Corporation All rights reserved.  
 
    MIT License: 
 
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    documentation files (the  "Software"), to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
 
    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 
 
    THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using ParkinsonDetected.Model;
using Microsoft.Band;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using System.Net.Http;
using Windows.Storage.FileProperties;
using Windows.Networking.BackgroundTransfer;
using Windows.Web.Http.Headers;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;




namespace ParkinsonDetected.Pages
{
    public partial class BasicsPage
    {
        public string[,] twoD;
        private BasicsModel model;
        
        public BasicsPage()
        {
            this.InitializeComponent();

            this.model = new BasicsModel();
            this.DataContext = this.model;

            App.Current.Resuming += App_Resuming;

            var t = this.FindDevicesAsync();
        }

        private async void ConnectDisconnect_Click(object sender, RoutedEventArgs e)
        {
            if (model.Main.BandClient == null)
            {
                using (new DisposableAction(() => model.Connecting = true, () => model.Connecting = false))
                {
                    try
                    {
                        // This method will throw an exception upon failure for a veriety of reasons,
                        // such as Band out of range or turned off.
                        model.Main.BandClient = await BandClientManager.Instance.ConnectAsync(model.SelectedDevice);
                    }
                    catch (Exception ex)
                    {
                        var t = new MessageDialog(ex.Message, "Failed to Connect").ShowAsync();
                    }
                }
            }
            else
            {
                model.Main.DisconnectDevice();
            }
        }
      
        private async Task FindDevicesAsync()
        {
            IBandInfo selected = model.SelectedDevice;                        

            IBandInfo[] bands = await BandClientManager.Instance.GetBandsAsync();

            model.Devices = new ObservableCollection<IBandInfo>(bands);

            if (selected != null)
            {
                model.SelectedDevice =model.Devices.SingleOrDefault((i) => { return (i.Name == selected.Name); } );
            }
            else if (model.Devices.Count > 0)
            {
                model.SelectedDevice = model.Devices[0];
            }
        }

        async void App_Resuming(object sender, object e)
        {
            await this.FindDevicesAsync();
        }

        private void Button_Click_Record(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Pages.Drawing), new PassedData { Name_Patient = "Data_Test", Disease_Status = " "});
            this.txt_result.Text = "";
        }

        private async void Button_Click_Result(object sender, RoutedEventArgs e)
        {
            //this.txt_result.Text = "Parkinson Possitive: 89%";
            await ReadFile();
            
        }
        private async Task ReadFile()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync("Data");

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync("Data_test.txt");
                string content = null;
                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    content = streamReader.ReadToEnd();
                    //this.TxtPrice.Text = content;
                    var res = content.Split('\n').Select(p => Regex.Split(p, "(?<=\\ )")).ToArray();

                    twoD = new String[res.Length, res[0].Length - 1];
                    for (int i = 0; i < res.Length - 1; i++)
                        for (int j = 0; j < res[0].Length - 2; j++)
                        {
                            twoD[i, j] = res[i][j];
                            twoD[i, res[0].Length - 2] = "";
                            // this.TxtPrice.Text += twoD[i, j].ToString();
                        }
                    try
                    {

                       
                        string result = await AutoPredictionMLAzure.InvokeRequestResponseService(twoD);
                        JObject obj = JObject.Parse(result);
                        string[][] values = JsonConvert.DeserializeObject<string[][]>(obj["Results"]["output1"]["value"]["Values"].ToString());

                        //TxtPrice.Text = "";
                        double total_nega = 0;
                        double total_posi = 0;
                        double sum_nega = 0;
                        double sum_posi = 0;
                        double total_result = 0;

                        //foreach (string[] item in values)
                        //{
                        //    if (Convert.ToDouble(item[4]) == -1)
                        //    {
                        //        total_nega += 1;
                        //        total_result += 1;
                        //        sum_nega += Convert.ToDouble(item[5]);
                        //    }

                        //    else if ((Convert.ToDouble(item[4]) == 1))
                        //    {
                        //        total_posi += 1;
                        //        total_result += 1;
                        //        sum_posi += Convert.ToDouble(item[5]);
                        //    }

                        //    //TxtPrice.Text += item[4] + " : " + item[5] + Environment.NewLine;

                        //}
                        foreach (string[] item in values)
                        {
                            if (Convert.ToDouble(item[4]) == -1)
                            {
                                total_nega += 1;
                                total_result += 1;
                                double accuracy = Convert.ToDouble(item[5]);
                                sum_nega += accuracy;
                                sum_posi += (1 - (accuracy));
                            }

                            else if ((Convert.ToDouble(item[4]) == 1))
                            {
                                total_posi += 1;
                                total_result += 1;
                                double accuracy = Convert.ToDouble(item[5]);
                                sum_posi += accuracy;
                                sum_nega += (1 - accuracy);
                            }

                            //TxtPrice.Text += item[4] + " : " + item[5] + Environment.NewLine;

                        }

  
                        //txt_result.Text = "Negative Num:" + total_nega.ToString() + "Positive Num:" + total_posi.ToString() + "\n" + "accuracy nega:" + (sum_nega / total_result) + "accuracy posi:" + (sum_posi / total_result) + total_result.ToString();
                        //int total_result = total_posi + total_nega;
                        //txt_result.Text = "Parkinson Negative:" + ((total_nega / total_result)).ToString("0.0%") + "\n" + "Parkinson Positive:" + ((total_posi / total_result)).ToString("0.0%") + "\n";
                        txt_result.Text = "Parkinson Negative:" + ((sum_nega / total_result)).ToString("0.0%") + "\n" + "Parkinson Positive:" + ((sum_posi / total_result)).ToString("0.0%") + "\n";
                            //+ "accuracy nega:" + (sum_nega / total_result).ToString("0.0%") + "accuracy posi:" + (sum_posi / total_result).ToString("0.0%");
                        //"accuracy nega:" + (sum_nega / total_nega) + "accuracy posi:" + (sum_posi / total_posi);

                    }
                    catch (Exception ex)
                    {
                        Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(ex.Message);
                        md.ShowAsync();
                    }
                    //  this.TxtPrice.Text = streamReader.ReadToEnd();
                }
            }
        }



        
       
    }
}

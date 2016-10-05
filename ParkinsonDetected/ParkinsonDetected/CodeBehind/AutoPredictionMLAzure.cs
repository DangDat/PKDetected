﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ParkinsonDetected.Pages
{
    public class AutoPredictionMLAzure
    {
        public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }

        public static async Task<string> InvokeRequestResponseService(string[,] values)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"X_axis", "Y_axis", "Z_axis", "Label"},
                                Values = values
                                //Values = new string[,] {  
                                //                          { "-0.1", "0.1", "0.1", "" },
                                //                          { "-0.01", "-0.11", "-0.01", "" },
                                //                          {"-0.34","0.45","-0.99",""}
                                //                        }
                            }
                        },
                                        },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };


                //Drawing Test
                //const string apiKey = "2wquoba9kdSu8jCVwfmjSDExy43A45EJQ9IBJNaZLgq8mXvoVhNwPlUYGRzCFY1xR8LznUaqqQkcT20HXUqfcQ=="; // Replace this with the API key for the web service
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                //client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/fb7ca60e21874824a86e3cc4eea95c81/services/dd59ff3435724461aaee5ed095c69640/execute?api-version=2.0&details=true");
                //HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                //Resting Tremor
                const string apiKey = "2wquoba9kdSu8jCVwfmjSDExy43A45EJQ9IBJNaZLgq8mXvoVhNwPlUYGRzCFY1xR8LznUaqqQkcT20HXUqfcQ=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/fb7ca60e21874824a86e3cc4eea95c81/services/dd59ff3435724461aaee5ed095c69640/execute?api-version=2.0&details=true");
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    return jsonString;
                }
                else
                {
                    throw new Exception(string.Format("Failed with status code: {0}", response.StatusCode));
                }
            }
        }
    }

}






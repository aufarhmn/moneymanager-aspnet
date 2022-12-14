using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Nancy.Json;
using static frontend.Pages.SignUp;
using System.IO;

namespace frontend
{
    /// <summary>
    /// Interaction logic for CreatePage.xaml
    /// </summary>
    public partial class CreatePage : Page
    {
        public CreatePage()
        {
            InitializeComponent();
        }

        private void AddLog(object sender, RoutedEventArgs e)
        {

            var mainWindow = ((MainWindow)Application.Current.MainWindow);

            List<MainWindow.Log> currentUserLog = mainWindow.clientLog;

            try
            {
                string newLogTitle;
                int newLogAmount;
                if (LogTitleTextBox.Text == "")
                {
                    throw new Exception("Log title cannot be empty");
                }
                else
                {
                    newLogTitle = LogTitleTextBox.Text;
                }

                if (int.TryParse(LogAmountTextBox.Text, out int n))
                {
                    newLogAmount = n;
                }
                else
                {
                    throw new Exception("Log amount must be a number");
                }

                //Setup new log
                MainWindow.Log newLog = new MainWindow.Log();
                newLog.Title = newLogTitle;
                newLog.Amount = newLogAmount;

                //Add the new log to user's
                currentUserLog.Add(newLog);

                // make a new httpwebrequest
                string apiUrl = $"https://localhost:7118/api/Clients/putById/{mainWindow.UserId}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "PUT";
                request.ContentType = "application/json";


                // Create a new StringWriter object
                StringWriter stringWriter = new StringWriter();

                // Create a new JsonTextWriter object
                using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
                {
                    // Set the QuoteChar property to '
                    jsonWriter.QuoteChar = '\'';

                    // Serialize the object to the JsonTextWriter
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, currentUserLog);
                }

                // Get the JSON string from the StringWriter
                string currentUserLogJson = stringWriter.ToString();

                if (newLogAmount < 0 && mainWindow.ClientBalance + newLogAmount < 0)
                {
                    throw new Exception("You don't have enough money to spend that much");
                }

                if(newLogAmount > 0)
                {
                    mainWindow.ClientBalance += newLogAmount;
                } else
                {
                    mainWindow.ClientExpense -= newLogAmount;
                    mainWindow.ClientBalance += newLogAmount;
                }


                string json = new JavaScriptSerializer().Serialize(new
                {
                    id = mainWindow.UserId,
                    clientName = $"{mainWindow.Username}",
                    clientPass = $"{mainWindow.Password}",
                    clientBalance = mainWindow.ClientBalance,
                    clientExpense = mainWindow.ClientExpense,
                    clientLog = $"{currentUserLogJson}"
                }); ;
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(jsonBytes, 0, jsonBytes.Length);
                }
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Read the response as a string
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();
                    }
                }

                mainWindow.Navigate("LogsPage");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

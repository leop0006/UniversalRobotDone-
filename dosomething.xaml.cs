using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Net;
using System.Net.Sockets;

namespace WpfApp1
{
    public partial class dosomething : Window
    {
        private RobotConnectionManager connectionManager;
        private string selectedFilePath;

        public dosomething()
        {
            InitializeComponent();
            connectionManager = new RobotConnectionManager();
            IpTextBox.Text = "172.20.254.205"; // Default IP
            PortTextBox.Text = "30002"; // Default Port
        }

        // Connect button event handler
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ipAddress = IpTextBox.Text;
                int port = int.Parse(PortTextBox.Text); // Let the exception be caught if not a valid number

                await connectionManager.ConnectAsync(ipAddress, port);
                UpdateStatusLight(Colors.Green);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
                UpdateStatusLight(Colors.Red);
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (!connectionManager.IsConnected)
            {
                MessageBox.Show("No active connection. Please connect to the robot first.");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                try
                {
                    // Read the contents of the selected file
                    string fileContent = File.ReadAllText(selectedFilePath);

                    // Display the contents in the TextBox
                    txtFileContent.Text = fileContent;
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error reading the file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            //Get the content from the Textbox 
            string contentToExecute = txtFileContent.Text;

            // send the content to the Universal Robot
            SendScriptToUniversalRobot(contentToExecute);

        }

        private void SendScriptToUniversalRobot(String content)
        {

            string ipAddress = IpTextBox.Text;
            int port = int.Parse(PortTextBox.Text); // Let the exception be caught if not a valid number

            using TcpClient client = new TcpClient(ipAddress, port);
            using (NetworkStream stream = client.GetStream())

                try
                {

                    byte[] data = Encoding.ASCII.GetBytes(content);
                    stream.Write(data, 0, data.Length);

                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }

        }


        // Disconnect button event handler
        private async void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            await connectionManager.DisconnectAsync();
            UpdateStatusLight(Colors.Red);
        }

        // Update the status light based on connection status
        private void UpdateStatusLight(Color color)
        {
            StatusLight.Fill = new SolidColorBrush(color);
        }

        // Override OnClosed to ensure network resources are released
        protected override async void OnClosed(EventArgs e)
        {
            await connectionManager.DisconnectAsync();
            base.OnClosed(e);
        }


    }
}


  

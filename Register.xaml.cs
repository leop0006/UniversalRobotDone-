using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Btt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\wapse\OneDrive\Skrivebord\UniversalRobot\UniversalRobot 1\UniversalRobot\UniversalRobot\WpfApp1\WpfApp1\Database1.mdf"";Integrated Security=True
");
                con.Open();
                string add_data = "INSERT INTO [dbo].[Table] VALUES(@Username, @Password)";
                SqlCommand cmd = new SqlCommand(add_data, con);


                cmd.Parameters.AddWithValue("@Username", Username.Text);
                cmd.Parameters.AddWithValue("@Password", Password.Password);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {

            }
        }   

    }
}

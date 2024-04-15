using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

namespace ZooDemo
{

    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        SqlConnection sqlConnection;
        public LoginScreen()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["ZooDemo.Properties.Settings.WPFZOOConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);


        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE UserName = @Username AND pass_word = @Password";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@UserName", userTxt.Text);
                sqlCommand.Parameters.AddWithValue("@Password", pwdTxt.Password);
                int count = (int)sqlCommand.ExecuteScalar();
                sqlConnection.Close();

                if (count > 0)
                {
                    CheckUserRole();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                    pwdTxt.Clear();
                    pwdTxt.Focus();
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }finally
            {
                sqlConnection.Close();
            }
            
 
        }
        private void CheckUserRole()
        {

           string userRole = GetUserRole(); 

            if (userRole == "admin")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.tbAdmin.Visibility = Visibility.Collapsed; 
                mainWindow.Show();
                this.Close();
            }
        }
        private string GetUserRole()
        {
                 string useraccess = null;
                string query = "select AccessLevel from Users where UserName=@UserName";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@UserName", userTxt.Text);
                    useraccess = (string)sqlCommand.ExecuteScalar();
                sqlConnection.Close();

            return useraccess;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled=!string.IsNullOrEmpty(pwdTxt.Password);
        }
    }
}

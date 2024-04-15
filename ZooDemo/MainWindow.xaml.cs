using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Configuration;
using System.Collections;

namespace ZooDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

 
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["ZooDemo.Properties.Settings.WPFZOOConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            
            ShowZoos();
            ShowAllAnimals();
            GetUserslist();
            
        }
        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);
                    listZoos.ItemsSource = zooTable.DefaultView;
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    //Zoo Tab Data Update
                    cmbZooLocations.ItemsSource = zooTable.DefaultView;
                    cmbZooLocations.DisplayMemberPath = "Location";
                    cmbZooLocations.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZoo();
        }
        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "select DISTINCT a.* from Animal a inner join ZooAnimal za on a.Id=za.AnimalId where za.ZooId=@ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    listAssociatedAnimals.SelectedValuePath = "Id";

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        private void ShowAllAnimals()
        {
            try
            {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalsTable = new DataTable();
                    sqlDataAdapter.Fill(animalsTable);
                    listAnimals.ItemsSource = animalsTable.DefaultView;
                    listAnimals.DisplayMemberPath = "Name";
                    listAnimals.SelectedValuePath = "Id";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            DeleteZoo();
        }
        private void DeleteZoo()
        {
            if (MessageBox.Show("Are you sure you want to delete ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "delete from Zoo where Id=@Id";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Id", listZoos.SelectedValue);
                    sqlCommand.ExecuteScalar();
                    ShowZoos();
                    MessageBox.Show("Zoo Deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();
                }
            }
        }

            private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            AddZoo();
        }
        private void AddZoo()
        {
            if (MessageBox.Show("Are you sure you want to Add ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "insert into Zoo Values(@Location)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Location", InputText.Text);
                    sqlCommand.ExecuteScalar();
                    ShowZoos();
                    MessageBox.Show("Zoo Added successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    InputText.Clear();
                    ShowZoos();
                }
            }
        }

        private void AddAnimaltoZoo_Click(object sender, RoutedEventArgs e)
        {
            AddAnimaltoZoo();
            GetAnimalCountsByZoo();
        }
        private void AddAnimaltoZoo()
        {
            if (MessageBox.Show("Are you sure you want to add the Animal to the Zoo ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "insert into ZooAnimal Values(@ZooId,@AnimalId)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAnimals.SelectedValue);
                    sqlCommand.ExecuteScalar();
                    ShowAssociatedAnimals();
                    MessageBox.Show("Animal List Updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAssociatedAnimals();
                }
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            DeleteAnimal();
        }
        private void DeleteAnimal()
        {
            if (MessageBox.Show("Are you sure you want to Delete ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "delete from Animal where Id=@Id";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Id", listAnimals.SelectedValue);
                    sqlCommand.ExecuteScalar();
                    ShowAllAnimals();
                    ShowAssociatedAnimals();
                    MessageBox.Show("Animal deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAllAnimals();
                    ShowAssociatedAnimals();
                }
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            AddAnimal();
        }
        private void AddAnimal()
        {
            if (MessageBox.Show("Are you sure you want to add the Animal ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "insert into Animal Values(@Name)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Name", InputText.Text);
                    sqlCommand.ExecuteScalar();
                    ShowAllAnimals();
                    MessageBox.Show("Animal Added successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    InputText.Clear();
                    ShowAllAnimals();
                }
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            RemoveAnimal();
            GetAnimalCountsByZoo();
        }
        private void RemoveAnimal()
        {
            if (MessageBox.Show("Are you sure you want to remove ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "delete from ZooAnimal where AnimalId=@AnimalId";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAssociatedAnimals.SelectedValue);
                    sqlCommand.ExecuteScalar();
                    ShowAssociatedAnimals();
                    MessageBox.Show("Animal removed successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    sqlConnection.Close();
                    ShowAssociatedAnimals();
                }
            }
        }
        private void ShowSelectedZoo()
        {
            try
            {
                string query = "select Location from Zoo where Id=@Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", listZoos.SelectedValue);
                    DataTable ZooTable = new DataTable();
                    sqlDataAdapter.Fill(ZooTable);
                    InputText.Text = ZooTable.Rows[0]["Location"].ToString();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        private void ShowSelectedAnimal()
        {
            try
            {
                string query = "select Name from Animal where Id=@Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@Id", listAnimals.SelectedValue);
                    DataTable AllAnimalTable = new DataTable();
                    sqlDataAdapter.Fill(AllAnimalTable);
                    InputText.Text = AllAnimalTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        private void listAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimal();
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoo();
        }
        private void UpdateZoo()
        {
            if (MessageBox.Show("Are you sure you want to update ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "update Zoo set Location=@Location where Id=@Id";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Location", InputText.Text);
                    sqlCommand.Parameters.AddWithValue("@Id", listZoos.SelectedValue);
                    sqlCommand.ExecuteScalar();
                    ShowZoos();
                    MessageBox.Show("Zoo Updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();
                }
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            UpdateAnimal();
        }
        private void UpdateAnimal()
        {
            if (MessageBox.Show("Are you sure you want to update ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "update Animal set Name=@Name where Id=@Id";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@Name", InputText.Text);
                    sqlCommand.Parameters.AddWithValue("@Id", listAnimals.SelectedValue);
                    sqlCommand.ExecuteScalar();
                    ShowAllAnimals();
                    ShowAssociatedAnimals();
                    MessageBox.Show("Animal updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowAllAnimals();
                    ShowAssociatedAnimals();
                }
            }
        }

        private void GetAnimalCountsByZoo()
        {
            DataTable dt = new DataTable();
            try
            { 
            string query = "SELECT Animal.Name, COUNT(ZooAnimal.AnimalId) AS Count " +
                   "FROM Animal " +
                   "INNER JOIN ZooAnimal ON Animal.Id = ZooAnimal.AnimalId " +
                   "INNER JOIN Zoo ON Zoo.Id = ZooAnimal.ZooId " +
                   "WHERE Zoo.Id = @Id " +
                   "GROUP BY Animal.Name";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlDataAdapter)
            {
                    sqlCommand.Parameters.AddWithValue("@Id", cmbZooLocations.SelectedValue);
                    sqlDataAdapter.Fill(dt);
                    dgAnimalCounts.ItemsSource = dt.DefaultView;
                }   
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
 
        }

        private void cmbZooLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                GetAnimalCountsByZoo();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Logout ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LoginScreen loginScreen = new LoginScreen();
                loginScreen.Show();
                this.Close();
            }
        }
        private void GetUserslist()
        {

            dguserlist.ItemsSource = GetDataFromDatabase();

        }
        private List<UserData> GetDataFromDatabase()
        {
            List<UserData> userDataList = new List<UserData>();

            // SQL Server connection string
            string connectionString = ConfigurationManager.ConnectionStrings["ZooDemo.Properties.Settings.WPFZOOConnectionString"].ConnectionString;

            // SQL query to retrieve data from Users table
            string query = "SELECT Id, UserName, AccessLevel FROM Users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Create UserData object and populate it with values from database
                    UserData userData = new UserData
                    {
                        UserId = Convert.ToInt32(reader["Id"]),
                        UserName = reader["UserName"].ToString(),
                        Access = reader["AccessLevel"].ToString(),
                        Permissions = GetPermissions() // Populate permissions (dropdown items)
                    };

                    userDataList.Add(userData);
                }

                reader.Close();
            }

            return userDataList;
        }
        public class UserData
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Access { get; set; }
            public string Permission { get; set; }
            public List<string> Permissions { get; set; }
        }
        private List<string> GetPermissions()
        {
            List<string> permissions = new List<string>();

            // SQL Server connection string
            string connectionString = ConfigurationManager.ConnectionStrings["ZooDemo.Properties.Settings.WPFZOOConnectionString"].ConnectionString;

            // SQL query to retrieve distinct access levels from Users table
            string query = "SELECT DISTINCT AccessLevel FROM Users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    permissions.Add(reader["AccessLevel"].ToString());
                }

                reader.Close();
            }

            return permissions;
        }
        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the UserId from the CommandParameter
            Button button = sender as Button;
            int userId = (int)button.CommandParameter;
            
            // Get the UserData based on the UserId
            UserData userData = GetUserData(userId);
            if (userData == null)
                return;

            try
            {
                // Update the database with the new AccessLevel value
                string updateQuery = "UPDATE Users SET AccessLevel = @AccessLevel WHERE Id = @Id";
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ZooDemo.Properties.Settings.WPFZOOConnectionString"].ConnectionString))
                using (SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection))
                {
                    updateCommand.Parameters.AddWithValue("@AccessLevel", selectedPermission);
                    updateCommand.Parameters.AddWithValue("@Id", userId);

                    sqlConnection.Open();
                    updateCommand.ExecuteScalar();
                }

                MessageBox.Show(userData.UserName + " access level updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating access level: " + ex.Message);
            }
            finally
            {
                // Refresh the DataGrid
                sqlConnection.Close();
                GetUserslist();
            }
        }

        // Method to get UserData based on UserId
        private UserData GetUserData(int userId)
        {
            return dguserlist.Items.OfType<UserData>().FirstOrDefault(user => user.UserId == userId);
        }

        private string selectedPermission; // Field to store the selected permission


        // Property if needed to access selected permission

        private void AccessComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox == null)
                return;
            selectedPermission = comboBox.SelectedItem as string;
            // Get the parent DataGridRow
            DataGridRow row = FindVisualParent<DataGridRow>(comboBox);

            // Find the SaveButton within the row
            Button saveButton = FindChild<Button>(row, "SaveButton");

            if (saveButton != null)
            {
                saveButton.IsEnabled = !string.IsNullOrEmpty(selectedPermission);
            }
        }

        // Helper method to find a parent of a certain type
        private T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T)
                {
                    return (T)obj;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }

        // Helper method to find a child control of a certain type with a given name
        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the requested type or not with the requested name, recursively search children
                if (!(child is T) || (child as FrameworkElement)?.Name != childName)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }


    }
}

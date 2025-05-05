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
using System.Windows.Shapes;

namespace NorthvilleLibrary
{
    /// <summary>
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {
        LINQDataContext db = new LINQDataContext(Properties.Settings.Default.NorthvilleLibraryConnectionString);
        private string staffEmail;

        public StaffWindow(string email)
        {
            InitializeComponent();
            staffEmail = email;
            ChangeName();
        }

        private void ChangeName()
        {
            lbl_Name.Content = $"Welcome back {GetName()}!";
        }

        private string GetName()
        {
            var user = (from u in db.Staffs
                        where u.Staff_Email == staffEmail
                        select u).FirstOrDefault();

            return user.Staff_FirstName + " " + user.Staff_Surname;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (tbx_Search_Student_ID.Text == "")
            {
                MessageBox.Show("Please enter a user ID.");
                return;
            }


        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

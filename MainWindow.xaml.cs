using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace NorthvilleLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isStudent = true;
        LINQDataContext db = new LINQDataContext(Properties.Settings.Default.NorthvilleLibraryConnectionString1);

        public MainWindow()
        {
            InitializeComponent();

            ////for testing only
            StaffWindow staffWindow = new StaffWindow("ST001@northville.edu.ph");
            staffWindow.Show();
            this.Close();

            //StudentWindow studentWindow = new StudentWindow("S001@ngen.edu.ph");
            //studentWindow.Show();
            //this.Close();

            //vid for the login
            mediaElement.Source = new Uri(@"D:\Visual Studio\Visual Studio repos\NorthvilleLibrary\assets\portrait.mp4", UriKind.Relative);
            mediaElement.Play();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.Zero;
            mediaElement.Play();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            string passwordLogin = pass_txt.Password;

            if (user_txt.Text == "" || passwordLogin == "") //if textboxes are empty
            {
                MessageBox.Show("Please enter username and password");
                return;
            }

            DetermineUserType(user_txt.Text);
            string password = GetPassword();

            if (passComparison(password) == 0)
            {
                MessageBox.Show("Login Successful", "Welcome Back!", MessageBoxButton.OK, MessageBoxImage.None);

                string email = user_txt.Text;

                if (isStudent)
                {
                    StudentWindow studentWindow = new StudentWindow(email);
                    studentWindow.Show();
                }
                else
                {
                    StaffWindow staffWindow = new StaffWindow(email);
                    staffWindow.Show();
                }

                mediaElement.Stop();
                this.Close();
            }
            else if (passComparison(password) == -1)
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string GetPassword()
        {
            string uPass = "";

            if (isStudent)
            {
                var users = (from u in db.Students
                             where u.Student_Email == user_txt.Text
                             select u).FirstOrDefault();

                if (users == null)
                {
                    MessageBox.Show("User not found.");
                    return "";
                }

                uPass = users.Student_Password;
                return uPass;
            }
            else
            {
                var users = (from u in db.Staffs
                             where u.Staff_Email == user_txt.Text
                             select u).FirstOrDefault();

                if (users == null)
                {
                    MessageBox.Show("User not found.");
                    return "";
                }

                uPass = users.Staff_Password;
                return uPass;
            }
        }

        private int passComparison(string uPass)
        {
            if (pass_txt.Password == uPass)
            {
                return 0;
            }
            else if (uPass == null)
            {
                return -1;
            }
            else
                return 1;
        }

        private void DetermineUserType(string uName)
        {
            //changes the value of isStudent

            if (uName.Contains("ngen.edu.ph"))
                isStudent = true;
            else
                isStudent = false;
        }
    }
}

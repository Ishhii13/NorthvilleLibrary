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
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        LINQDataContext db = new LINQDataContext(Properties.Settings.Default.NorthvilleLibraryConnectionString);
        private string studentEmail;

        public StudentWindow(string email)
        {
            InitializeComponent();
            studentEmail = email;
            ChangeName();
        }

        private void ChangeName()
        {
            lbl_Name.Content = $"Welcome back {GetName()}!";
        }

        private string GetName()
        {
            var user = (from u in db.Students
                        where u.Student_Email == studentEmail
                        select u).FirstOrDefault();

            return user.Student_FirstName + " " + user.Student_Surname;
        }
    }
}

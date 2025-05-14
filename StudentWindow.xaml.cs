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
        LINQDataContext db = new LINQDataContext(Properties.Settings.Default.NorthvilleLibraryConnectionString1);
        private string studentEmail;

        public StudentWindow(string email)
        {
            InitializeComponent();
            studentEmail = email;
            ChangeName();
            Populate();
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

        private void Populate()
        {
            List<string> booksLb = new List<string>();
            var student = (from u in db.Students
                           where u.Student_Email == studentEmail
                           select u).FirstOrDefault();

            tbx_Student_ID.Text = student.Student_ID;
            tbx_Student_Surname.Text = student.Student_Surname;
            tbx_Student_FirstName.Text = student.Student_FirstName;
            tbx_Student_Course.Text = student.Student_Course_ID;
            tbx_Student_ContactNo.Text = student.Student_ContactNo;
            tbx_Student_Email.Text = student.Student_Email;

            var transaction = (from u in db.Transactions
                           where u.Transaction_Student_ID == student.Student_ID
                           select u).FirstOrDefault();

            var borrow = (from u in db.Borrows
                           where u.Borrow_ID == transaction.Transaction_Borrow_ID
                           select u).FirstOrDefault();

            var book = (from u in db.Books
                        where u.Book_ID == borrow.Borrow_Book_ID
                        select u).FirstOrDefault();

            if (book != null)
            {
                booksLb.Add(book.Book_Title);
            }

            lb_BookTitles.ItemsSource = booksLb;
        }        


    }
}

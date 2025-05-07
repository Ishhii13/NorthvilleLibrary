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
        private Student selectedStudent;

        public StaffWindow(string email)
        {
            InitializeComponent();
            staffEmail = email;
            ChangeName();
            PopulateStudentList();
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

            if (StudentIDExists())
            {
                var student = (from u in db.Students
                               where u.Student_ID == tbx_Search_Student_ID.Text
                               select u).FirstOrDefault();

                ClearTextboxes();
                tbx_Student_ID.Text = student.Student_ID;
                tbx_Student_Surname.Text = student.Student_Surname;
                tbx_Student_FirstName.Text = student.Student_FirstName;
                tbx_Student_Course_ID.Text = student.Student_Course_ID;
                tbx_Student_ContactNo.Text = student.Student_ContactNo;
                tbx_Student_Email.Text = student.Student_Email;
                tbx_Student_Password.Text = student.Student_Password;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudent != null)
            {
                selectedStudent.Student_ID = tbx_Student_ID.Text;
                selectedStudent.Student_Surname = tbx_Student_Surname.Text;
                selectedStudent.Student_FirstName = tbx_Student_FirstName.Text;
                selectedStudent.Student_Course_ID = tbx_Student_Course_ID.Text;
                selectedStudent.Student_ContactNo = tbx_Student_ContactNo.Text;
                selectedStudent.Student_Email = tbx_Student_Email.Text;
                selectedStudent.Student_Password = tbx_Student_Password.Text;

                db.SubmitChanges();

                MessageBox.Show("Student information updated successfully.");

                PopulateStudentList();
            }
            else
                MessageBox.Show("Please select a student to edit.");
        }

        private void btn_LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private bool StudentIDExists()
        {
            var users = (from u in db.Students
                         where u.Student_ID == tbx_Search_Student_ID.Text
                         select u).FirstOrDefault();

            if (users == null)
            {
                MessageBox.Show("User not found.");
                return false;
            }

            return true;
        }

        private void ClearTextboxes()
        {
            tbx_Student_ID.Text = "";
            tbx_Student_Surname.Text = "";
            tbx_Student_FirstName.Text = "";
            tbx_Student_Course_ID.Text = "";
            tbx_Student_ContactNo.Text = "";
            tbx_Student_Email.Text = "";
            tbx_Student_Password.Text = "";
        }

        private void PopulateStudentList()
        {
            var students = db.Students.ToList();

            Dictionary<string, Student> studentNames = new Dictionary<string, Student>();

            foreach (var student in students)
            {
                string displayName = $"{student.Student_FirstName} {student.Student_Surname}";
                studentNames[displayName] = student;
            }

            lbx_AllStudents.ItemsSource = studentNames.Keys.ToList();
            lbx_AllStudents.Tag = studentNames;
        }

        private void lbx_AllStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedKey = lbx_AllStudents.SelectedItem as string;
            if (selectedKey == null) return;

            var studentMap = lbx_AllStudents.Tag as Dictionary<string, Student>;
            if (studentMap != null && studentMap.ContainsKey(selectedKey))
            {
                selectedStudent = studentMap[selectedKey];

                tbx_Student_ID.Text = selectedStudent.Student_ID;
                tbx_Student_Surname.Text = selectedStudent.Student_Surname;
                tbx_Student_FirstName.Text = selectedStudent.Student_FirstName;
                tbx_Student_Course_ID.Text = selectedStudent.Student_Course_ID;
                tbx_Student_ContactNo.Text = selectedStudent.Student_ContactNo;
                tbx_Student_Email.Text = selectedStudent.Student_Email;
                tbx_Student_Password.Text = selectedStudent.Student_Password;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Add.Content == "Add")
            {
                Add.Content = "Save";
                Edit.IsEnabled = false;
                Delete.IsEnabled = false;
                
                ClearTextboxes();
            }
            else if ((string)Add.Content == "Save")
            {
                if (string.IsNullOrWhiteSpace(tbx_Student_Surname.Text) ||
                    string.IsNullOrWhiteSpace(tbx_Student_FirstName.Text) ||
                    string.IsNullOrWhiteSpace(tbx_Student_Course_ID.Text) ||
                    string.IsNullOrWhiteSpace(tbx_Student_ContactNo.Text) ||
                    string.IsNullOrWhiteSpace(tbx_Student_Email.Text) ||
                    string.IsNullOrWhiteSpace(tbx_Student_Password.Text))
                {
                    MessageBox.Show("All fields must contain a value.");
                    return;
                }

                var newStudent = new Student
                {
                    Student_ID = tbx_Student_ID.Text,
                    Student_Surname = tbx_Student_Surname.Text,
                    Student_FirstName = tbx_Student_FirstName.Text,
                    Student_Course_ID = tbx_Student_Course_ID.Text,
                    Student_ContactNo = tbx_Student_ContactNo.Text,
                    Student_Email = tbx_Student_Email.Text,
                    Student_Password = tbx_Student_Password.Text
                };

                db.Students.InsertOnSubmit(newStudent);
                db.SubmitChanges();

                MessageBox.Show("Student added successfully.");

                PopulateStudentList();
                Add.Content = "Add";
                Edit.IsEnabled = true;
                Delete.IsEnabled = true;
                ClearTextboxes();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStudent != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this student?", "Confirm", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    db.Students.DeleteOnSubmit(selectedStudent);
                    db.SubmitChanges();

                    MessageBox.Show("Student deleted.");

                    ClearTextboxes();
                    PopulateStudentList();
                    selectedStudent = null;
                }
            }
            else
                MessageBox.Show("Please select a student to delete.");
        }
    }
}

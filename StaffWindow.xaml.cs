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
        LINQDataContext db = new LINQDataContext(Properties.Settings.Default.NorthvilleLibraryConnectionString1);
        private string staffEmail;
        private Student selectedStudent;
        private Book selectedBook;
        private Borrow selectedBorrow;
        private Transaction selectedTransaction;
        Grid currentGrid;

        public StaffWindow(string email)
        {
            InitializeComponent();
            staffEmail = email;
            ChangeName();
            currentGrid = grid_Students;
            PopulateListBox();
        }

        private void btn_Student_Grid_Click(object sender, RoutedEventArgs e)
        {
            currentGrid.Visibility = Visibility.Collapsed;
            grid_Students.Visibility = Visibility.Visible;
            currentGrid = grid_Students;
            PopulateListBox();
        }

        private void ChangeName()
        {
            var user = (from u in db.Staffs
                        where u.Staff_Email == staffEmail
                        select u).FirstOrDefault();

            string userName = user.Staff_FirstName + " " + user.Staff_Surname;

            lbl_Name.Content = $"Welcome back {userName}!";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (tbx_Search_Student_ID.Text == "")
            {
                MessageBox.Show("Please enter a user ID.");
                return;
            }

            if (currentGrid == grid_Students && IDExists())
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
            else if (currentGrid == grid_Books && IDExists())
            {
                var book = (from u in db.Books
                            where u.Book_ID == tbx_Search_Book_ID.Text
                            select u).FirstOrDefault();

                ClearTextboxes();
                tbx_Book_ID.Text = book.Book_ID;
                tbx_Book.Text = book.Book_Title;
                tbx_Book_Author.Text = book.Book_Author;
                tbx_Book_ISBN.Text = book.Book_ISBN;
                tbx_Book_Publication_Date.Text = book.Book_Publication_Date.ToString();
                tbx_Book_Genre.Text = book.Book_Genre;
                tbx_Book_Copies.Text = book.Book_Copies.ToString();
            }
            else if (IDExists())
            {
                var transaction = (from u in db.Transactions
                                   where u.Transaction_ID == tbx_Search_Transaction_ID.Text
                                   select u).FirstOrDefault();

                var borrow = (from u in db.Borrows
                                   where u.Borrow_ID == transaction.Transaction_Borrow_ID
                                   select u).FirstOrDefault();

                ClearTextboxes();
                tbx_Borrow_ID.Text = borrow.Borrow_ID;
                tbx_Borrow_Book_ID.Text = borrow.Borrow_Book_ID;
                tbx_Borrow_Date.Text = borrow.Borrow_Date.ToString();
                tbx_Borrow_Due_Date.Text = borrow.Borrow_Due_Date.ToString();
                tbx_Borrow_Return_Date.Text = borrow.Borrow_Return_Date.ToString();
                tbx_Borrow_Fee.Text = borrow.Borrow_Fee.ToString();
                tbx_Transaction_ID.Text = transaction.Transaction_ID;
                tbx_Transaction_Student_ID.Text = transaction.Transaction_Student_ID;
            }
        }

        private bool IDExists()
        {
            if (currentGrid == grid_Students)
            {
                var users = (from u in db.Students
                             where u.Student_ID == tbx_Search_Student_ID.Text
                             select u).FirstOrDefault();

                if (users == null)
                {
                    MessageBox.Show("Student not found.");
                    return false;
                }

                return true;
            }
            else if (currentGrid == grid_Books)
            {
                var users = (from u in db.Books
                             where u.Book_ID == tbx_Search_Book_ID.Text
                             select u).FirstOrDefault();

                if (users == null)
                {
                    MessageBox.Show("Book not found.");
                    return false;
                }

                return true;
            }
            else
            {
                var users = (from u in db.Borrows
                             where u.Borrow_ID == tbx_Search_Transaction_ID.Text
                             select u).FirstOrDefault();

                if (users == null)
                {
                    MessageBox.Show("Book not found.");
                    return false;
                }

                return true;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (currentGrid == grid_Students)
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

                    PopulateListBox();
                }
                else
                    MessageBox.Show("Please select a student to edit.");
            }
            else if (currentGrid == grid_Books)
            {
                if (selectedBook != null)
                {
                    selectedBook.Book_ID = tbx_Book_ID.Text;
                    selectedBook.Book_Title = tbx_Book.Text;
                    selectedBook.Book_Author = tbx_Book_Author.Text;
                    selectedBook.Book_ISBN = tbx_Book_ISBN.Text;
                    selectedBook.Book_Publication_Date = DateTime.Parse(tbx_Book_Publication_Date.Text);
                    selectedBook.Book_Genre = tbx_Book_Genre.Text;
                    selectedBook.Book_Copies = int.Parse(tbx_Book_Copies.Text);

                    db.SubmitChanges();

                    MessageBox.Show("Book information updated successfully.");

                    PopulateListBox();
                }
                else
                    MessageBox.Show("Please select a book to edit.");
            }
            else //WTF IS HAPPENING HERE
            {
                if (selectedBorrow != null)
                {
                    selectedBorrow.Borrow_ID = tbx_Borrow_ID.Text;
                    selectedBorrow.Borrow_Book_ID = tbx_Borrow_Book_ID.Text;
                    selectedBorrow.Borrow_Date = DateTime.Parse(tbx_Borrow_Date.Text);
                    selectedBorrow.Borrow_Due_Date = DateTime.Parse(tbx_Borrow_Due_Date.Text);
                    selectedBorrow.Borrow_Return_Date = DateTime.Parse(tbx_Borrow_Return_Date.Text);
                    selectedBorrow.Borrow_Fee = int.Parse(tbx_Borrow_Fee.Text);

                    var transaction = db.Transactions.FirstOrDefault(t => t.Transaction_ID == tbx_Transaction_ID.Text);
                    if (transaction != null)
                    {
                        transaction.Transaction_ID = tbx_Transaction_ID.Text;
                        transaction.Transaction_Student_ID = tbx_Transaction_Student_ID.Text;
                    }

                    db.SubmitChanges();

                    MessageBox.Show("Transaction information updated successfully.");

                    PopulateListBox();
                }
                else
                    MessageBox.Show("Please select a transaction to edit.");
            }
        }

        private void btn_LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ClearTextboxes()
        {
            if (currentGrid == grid_Students)
            {
                tbx_Student_ID.Text = "";
                tbx_Student_Surname.Text = "";
                tbx_Student_FirstName.Text = "";
                tbx_Student_Course_ID.Text = "";
                tbx_Student_ContactNo.Text = "";
                tbx_Student_Email.Text = "";
                tbx_Student_Password.Text = "";
            }
            else if (currentGrid == grid_Books)
            {
                tbx_Book_ID.Text = "";
                tbx_Book.Text = "";
                tbx_Book_Author.Text = "";
                tbx_Book_ISBN.Text = "";
                tbx_Book_Publication_Date.Text = "";
                tbx_Book_Genre.Text = "";
                tbx_Book_Copies.Text = "";
            }
            else
            {
                tbx_Borrow_ID.Text = "";
                tbx_Borrow_Book_ID.Text = "";
                tbx_Borrow_Date.Text = "";
                tbx_Borrow_Due_Date.Text = "";
                tbx_Borrow_Return_Date.Text = "";
                tbx_Borrow_Fee.Text = "";
                tbx_Transaction_ID.Text = "";
                tbx_Transaction_Student_ID.Text = "";
            }
        }

        private void PopulateListBox()
        {
            if (currentGrid == grid_Students)
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
            else if (currentGrid == grid_Books)
            {
                var books = db.Books.ToList();
                Dictionary<string, Book> bookTitles = new Dictionary<string, Book>();
                foreach (var book in books)
                {
                    string displayTitle = $"{book.Book_Title} by {book.Book_Author}";
                    bookTitles[displayTitle] = book;
                }
                lbx_AllBooks.ItemsSource = bookTitles.Keys.ToList();
                lbx_AllBooks.Tag = bookTitles;
            }
            else //FIX THIS IT'S NOT SHOWING THE RIGHT INFO
            {
                var borrows = db.Borrows.ToList();
                var transaction = db.Transactions.ToList();
                Dictionary<string, Borrow> borrowRecords = new Dictionary<string, Borrow>();
                foreach (var borrow in borrows)
                {
                    string record = $"Borrowed {borrow.Borrow_Book_ID}";
                    borrowRecords[record] = borrow;
                }
                lbx_AllTransaction.ItemsSource = borrowRecords.Keys.ToList();
                lbx_AllTransaction.Tag = borrowRecords;
            }
        }

        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentGrid == grid_Students)
            {
                string selectedKey = lbx_AllStudents.SelectedItem as string;
                if (selectedKey == null) return;

                var studentInfo = lbx_AllStudents.Tag as Dictionary<string, Student>;
                if (studentInfo != null && studentInfo.ContainsKey(selectedKey))
                {
                    selectedStudent = studentInfo[selectedKey];

                    tbx_Student_ID.Text = selectedStudent.Student_ID;
                    tbx_Student_Surname.Text = selectedStudent.Student_Surname;
                    tbx_Student_FirstName.Text = selectedStudent.Student_FirstName;
                    tbx_Student_Course_ID.Text = selectedStudent.Student_Course_ID;
                    tbx_Student_ContactNo.Text = selectedStudent.Student_ContactNo;
                    tbx_Student_Email.Text = selectedStudent.Student_Email;
                    tbx_Student_Password.Text = selectedStudent.Student_Password;
                }
            }
            else if (currentGrid == grid_Books)
            {
                string selectedKey = lbx_AllBooks.SelectedItem as string;
                if (selectedKey == null) return;

                var bookInfo = lbx_AllBooks.Tag as Dictionary<string, Book>;
                if (bookInfo != null && bookInfo.ContainsKey(selectedKey))
                {
                    selectedBook = bookInfo[selectedKey];

                    tbx_Book_ID.Text = selectedBook.Book_ID;
                    tbx_Book.Text = selectedBook.Book_Title;
                    tbx_Book_Author.Text = selectedBook.Book_Author;
                    tbx_Book_ISBN.Text = selectedBook.Book_ISBN;
                    tbx_Book_Publication_Date.Text = selectedBook.Book_Publication_Date.ToString("yyyy-MM-dd");
                    tbx_Book_Genre.Text = selectedBook.Book_Genre;
                    tbx_Book_Copies.Text = selectedBook.Book_Copies.ToString();
                }
            }
            else //Transaction ID and Student ID aren't showing
            {
                if (currentGrid == grid_Transaction_Borrow)
                {
                    string selectedKey = lbx_AllTransaction.SelectedItem as string;
                    if (selectedKey == null) return;

                    var borrowInfo = lbx_AllTransaction.Tag as Dictionary<string, Borrow>;
                    if (borrowInfo != null && borrowInfo.ContainsKey(selectedKey))
                    {
                        selectedBorrow = borrowInfo[selectedKey];

                        tbx_Borrow_ID.Text = selectedBorrow.Borrow_ID;
                        tbx_Borrow_Book_ID.Text = selectedBorrow.Borrow_Book_ID;
                        tbx_Borrow_Date.Text = selectedBorrow.Borrow_Date.ToString("yyyy-MM-dd");
                        tbx_Borrow_Due_Date.Text = selectedBorrow.Borrow_Due_Date.ToString("yyyy-MM-dd");
                        tbx_Borrow_Fee.Text = selectedBorrow.Borrow_Fee.ToString();

                        if (selectedBorrow.Borrow_Return_Date != null)
                            tbx_Borrow_Return_Date.Text = selectedBorrow.Borrow_Return_Date.ToString();
                        else
                            tbx_Borrow_Return_Date.Text = "";

                        var transactionInfo = db.Transactions.ToDictionary(t => t.Transaction_ID);

                        var selectedTransaction = transactionInfo.Values
                                .FirstOrDefault(t => t.Transaction_Borrow_ID == selectedBorrow.Borrow_ID);

                        if (selectedTransaction != null)
                        {
                            tbx_Transaction_ID.Text = selectedTransaction.Transaction_ID;
                            tbx_Transaction_Student_ID.Text = selectedTransaction.Transaction_Student_ID;
                        }
                        else //REMOVE THIS LATER ITS FOR TESTING
                        {
                            tbx_Transaction_ID.Text = "";
                            tbx_Transaction_Student_ID.Text = "";
                        }
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Add.Content == "Add" || (string)btn_Add_Book.Content == "Add" || (string)btn_Add_Transaction.Content == "Add")
            {
                if (currentGrid == grid_Students)
                {
                    Add.Content = "Save";
                    Edit.IsEnabled = false;
                    Delete.IsEnabled = false;
                }
                else if (currentGrid == grid_Books)
                {
                    btn_Add_Book.Content = "Save";
                    btn_Edit_Book.IsEnabled = false;
                    btn_Delete_Book.IsEnabled = false;
                }
                else
                {
                    btn_Add_Transaction.Content = "Save";
                    btn_Edit_Transaction.IsEnabled = false;
                    btn_Delete_Transaction.IsEnabled = false;
                }

                ClearTextboxes();
            }
            else if ((string)Add.Content == "Save" || (string)btn_Add_Book.Content == "Save" || (string)btn_Add_Transaction.Content == "Save")
            {
                if (currentGrid == grid_Students)
                {
                    if (string.IsNullOrWhiteSpace(tbx_Student_Surname.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Student_FirstName.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Student_Course_ID.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Student_ContactNo.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Student_Email.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Student_Password.Text))
                    {
                        MessageBox.Show("All student fields must contain a value.");
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
                }
                else if (currentGrid == grid_Books)
                {
                    if (string.IsNullOrWhiteSpace(tbx_Book.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Book_Author.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Book_ISBN.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Book_Publication_Date.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Book_Genre.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Book_Copies.Text))
                    {
                        MessageBox.Show("All fields must contain a value.");
                        return;
                    }

                    var newBook = new Book
                    {
                        Book_ID = tbx_Book_ID.Text,
                        Book_Title = tbx_Book.Text,
                        Book_Author = tbx_Book_Author.Text,
                        Book_ISBN = tbx_Book_ISBN.Text,
                        Book_Publication_Date = DateTime.Parse(tbx_Book_Publication_Date.Text),
                        Book_Genre = tbx_Book_Genre.Text,
                        Book_Copies = int.Parse(tbx_Book_Copies.Text)
                    };

                    db.Books.InsertOnSubmit(newBook);
                    db.SubmitChanges();
                    MessageBox.Show("Book added successfully.");
                }
                else if (currentGrid == grid_Transaction_Borrow)
                {
                    if (string.IsNullOrWhiteSpace(tbx_Borrow_Book_ID.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Borrow_Date.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Borrow_Due_Date.Text) ||
                        string.IsNullOrWhiteSpace(tbx_Transaction_Student_ID.Text))
                    {
                        MessageBox.Show("All transaction fields must contain a value.");
                        return;
                    }

                    var newBorrow = new Borrow
                    {

                    };

                    db.Borrows.InsertOnSubmit(newBorrow);
                    db.SubmitChanges();
                    MessageBox.Show("Transaction added successfully.");
                }

                PopulateListBox();
                Add.Content = "Add";
                Edit.IsEnabled = true;
                Delete.IsEnabled = true;
                ClearTextboxes();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentGrid == grid_Students)
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
                        PopulateListBox();
                        selectedStudent = null;
                    }
                }
                else
                    MessageBox.Show("Please select a student to delete.");
            }
            else if (currentGrid == grid_Books)
            {
                if (selectedBook != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        db.Books.DeleteOnSubmit(selectedBook);
                        db.SubmitChanges();

                        MessageBox.Show("Book deleted.");

                        ClearTextboxes();
                        PopulateListBox();
                        selectedBook = null;
                    }
                }
                else
                    MessageBox.Show("Please select a book to delete.");
            }
            else
            {
                if (selectedBorrow != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this transaction?", "Confirm", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        db.Borrows.DeleteOnSubmit(selectedBorrow);
                        db.SubmitChanges();

                        MessageBox.Show("Transaction deleted.");

                        ClearTextboxes();
                        PopulateListBox();
                        selectedBorrow = null;
                    }
                }
                else
                    MessageBox.Show("Please select a transaction to delete.");
            }
        }

        private void btn_Book_Grid_Click(object sender, RoutedEventArgs e)
        {
            currentGrid.Visibility = Visibility.Collapsed;
            grid_Books.Visibility = Visibility.Visible;
            currentGrid = grid_Books;
            PopulateListBox();
        }

        private void btn_Transaction_Borrow_Grid_Click(object sender, RoutedEventArgs e)
        {
            currentGrid.Visibility = Visibility.Collapsed;
            grid_Transaction_Borrow.Visibility = Visibility.Visible;
            currentGrid = grid_Transaction_Borrow;
            PopulateListBox();
        }
    }
}

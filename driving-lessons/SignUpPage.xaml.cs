using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Controls;
using Model;
using ViewModel;

namespace driving_lessons
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private readonly MainWindow _mainWindow;
        public SignUpPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

            string name = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = new User
            {
                Name = name,
                Password = password
            };

            UserDB userDB = new UserDB();
            userDB.Insert(user);
            UserDB.SaveChanges();

            if (StudentSignUp.IsChecked == true)
            {
                HandleStudentSignUp(user);
            }
            else if (TeacherSignUp.IsChecked == true)
            {
                HandleTeacherSignUp(user);
            }
        }

        private void HandleStudentSignUp(User user)
        {
            var age = int.TryParse(AgeTextBox.Text, out int parsedAge) ? parsedAge : (int?)null;
            if (age == null)
            {
                MessageBox.Show("Enter a valid age number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AgeTextBox.Clear();
                return;
            }
            if (age < 17)
            {
                MessageBox.Show("You must be at least 17 years old to sign up as a student.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            Student student = new Student
            {
                UserId = user.Id, // Linking the two tables
                Age = int.Parse(AgeTextBox.Text)
            };

            StudentDB studentDB = new StudentDB();
            studentDB.Insert(student);
            StudentDB.SaveChanges();
        }

        private void HandleTeacherSignUp(User user)
        {
            Teacher teacher = new Teacher
            {
                UserId = user.Id,
            };

            TeacherDB teacherDB = new TeacherDB();
            teacherDB.Insert(teacher);
            TeacherDB.SaveChanges();
        }

        private void UserType_Checked(object sender, RoutedEventArgs e)
        {
            if (StudentFields == null) return;

            // Only show Age if it's a Student
            StudentFields.Visibility = StudentSignUp.IsChecked == true
                                       ? Visibility.Visible
                                       : Visibility.Collapsed;
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new LoginPage(_mainWindow));
        }
    }
}

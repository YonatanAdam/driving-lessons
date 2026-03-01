using System.Windows;
using System.Windows.Controls;
using ViewModel;

namespace driving_lessons
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        private readonly MainWindow _mainWindow;
        public LoginPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserDB userDb = new UserDB();
            var user = userDb.SelectByName(username);

            if (user == null || user.Password != password)
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StudentDB studentDb = new StudentDB();
            var student = studentDb.SelectById(user.Id);

            if (student != null)
            {
                MessageBox.Show($"Welcome {student.Name}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainWindow.MainFrame.Navigate(new StudentPage(user, student, _mainWindow));
                return;
            }
            
            TeacherDB teacherDb = new TeacherDB();
            var teacher = teacherDb.SelectByUserId(user.Id);

            if (teacher != null)
            {
                MessageBox.Show($"Welcome {teacher.Name}!",  "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainWindow.MainFrame.Navigate(new TeacherPage(user, teacher));
                return;
            }
            
            MessageBox.Show($"User id: {user.Id} was not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new SignUpPage(_mainWindow));
        }
    }
}

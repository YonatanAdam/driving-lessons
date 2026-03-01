using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Model;
using ViewModel;

namespace driving_lessons;

public partial class StudentPage : Page {
    private Student _student;
    private User _user;
    private MainWindow _mainWindow;
    public StudentPage(User user, Student student, MainWindow mainWindow)
    {
        InitializeComponent();
        _student = student;
        _user = user;
        _mainWindow  = mainWindow;
        
        HideAllPanels();
        DashboardPanel.Visibility =  Visibility.Visible;

        LoadTeachersData();
    }

    private void LoadTeachersData()
    {
        TeacherDB teacherDb = new TeacherDB();
        var teachers = teacherDb.SelectAll();
        TeachersDataGrid.ItemsSource = teachers;

        SearchTextBox.TextChanged += ApplyFilters;
        LocationFilter.SelectionChanged += ApplyFilters;
        TransmissionFilter.SelectionChanged += ApplyFilters;
        MaxPriceTextBox.TextChanged += ApplyFilters;
    }

    private void ApplyFilters(object sender, EventArgs e)
    {
        TeacherDB teacherDb = new TeacherDB();
        var allTeachers = teacherDb.SelectAll();

        var filtered = allTeachers.AsEnumerable();

        if (!string.IsNullOrEmpty(SearchTextBox.Text) && SearchTextBox.Text != "Search teachers...")
        {
            filtered = filtered.Where(t => t.Name.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase));
        }

        if (LocationFilter.SelectedIndex > 0)
        {
            string? location = ((ComboBoxItem)LocationFilter.SelectedItem).Content.ToString();
            if (location != "All Cities")
                filtered = filtered.Where(t => t.Location == location);
        }

        if (TransmissionFilter.SelectedIndex > 0)
        {
            string? transmission = ((ComboBoxItem)TransmissionFilter.SelectedItem).Content.ToString();
            if (transmission != "Both")
                filtered = filtered.Where(t => t.TransmissionType == transmission);
        }

        if (double.TryParse(MaxPriceTextBox.Text, out double maxPrice))
        {
            filtered = filtered.Where(t => t.PricePerLesson <=  maxPrice);
        }

        TeachersDataGrid.ItemsSource = filtered.ToList();
    }

    private void ViewProfile_OnClick(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        Teacher selectedTeacher = (Teacher)btn.DataContext;

        TeacherProfileWindow profileWindow = new TeacherProfileWindow(selectedTeacher);
        profileWindow.ShowDialog();
    }

    private void BookLesson_OnClick(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        Teacher selectedTeacher = (Teacher)btn.DataContext;

        BookingDialog bookingDialog = new BookingDialog(_student, selectedTeacher);
        bool? result = bookingDialog.ShowDialog();

        if (result == true)
        {
            MessageBox.Show("Lesson booked successfully!", "Success",  MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void ClearFilters_OnClick(object sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = "";
        LocationFilter.SelectedIndex = 0;
        TransmissionFilter.SelectedIndex = 0;
        MaxPriceTextBox.Text = "";
    }
    
    private void HideAllPanels()
    {
        DashboardPanel.Visibility = Visibility.Collapsed;
        SearchPanel.Visibility = Visibility.Collapsed;
        TeachersDataGrid.Visibility = Visibility.Collapsed;
        MyLessonsPanel.Visibility = Visibility.Collapsed;
        MyProgressPanel.Visibility = Visibility.Collapsed;
        MyProfilePanel.Visibility = Visibility.Collapsed;
        FiltersGrid.Visibility = Visibility.Collapsed;
    }

    private void DashboardButton_OnClick(object sender, RoutedEventArgs e)
    {
        HideAllPanels();
        DashboardPanel.Visibility =  Visibility.Visible;
    }

    private void BrowseTeachersButton_OnClick(object sender, RoutedEventArgs e)
    {
        HideAllPanels();
        SearchPanel.Visibility = Visibility.Visible;
        FiltersGrid.Visibility = Visibility.Visible;
        TeachersDataGrid.Visibility = Visibility.Visible;
    }

    private void MyLessonsButton_OnClick(object sender, RoutedEventArgs e)
    {
        HideAllPanels();
        MyLessonsPanel.Visibility = Visibility.Visible;
    }

    private void MyProgressButton_OnClick(object sender, RoutedEventArgs e)
    {
        HideAllPanels();
        MyProgressPanel.Visibility =  Visibility.Visible;
    }

    private void LoadProfileData()
    {
        ProfileNameTextBox.Text = _user.Name;
        AgeTextBox.Text = _student.Age.ToString();
        EmailTextBox.Text = _user.Email;
        StudentDB studentDb = new StudentDB();
        if (_student.TeacherId != null)
        {
            AssignedTeacherTextBox.Text = studentDb.GetTeacherName(_student.TeacherId.Value);
        }
        else
        {
            AssignedTeacherTextBox.Text = "None";
        }
    }
    private void MyProfileButton_OnClick(object sender, RoutedEventArgs e)
    {
        HideAllPanels();
        MyProfilePanel.Visibility = Visibility.Visible;
        LoadProfileData();
    }

    private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to sign out?",
            "Confirm Sign Out",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            // Clear the current user
            _user = null;
            _student = null;

            // Navigate back to the login page
            _mainWindow.MainFrame.Navigate(new LoginPage(_mainWindow));
        }
    }
    
    private void profileHideStatus()
    {
        ProfileStatusBorder.Visibility = Visibility.Collapsed;
    }

    private void ExitEditMode()
    {
        ProfileCancelButton.Visibility = Visibility.Collapsed;
        ProfileSaveButton.Visibility = Visibility.Collapsed;
        ProfileEditButton.Visibility = Visibility.Visible;
        ProfileNameTextBox.IsReadOnly = true;
        AgeTextBox.IsReadOnly = true;
        EmailTextBox.IsReadOnly = true;
        ProfileNameTextBox.IsEnabled = false;
        AgeTextBox.IsEnabled = false;
        EmailTextBox.IsEnabled = false;
    }

    private void ProfileEditButton_OnClick(object sender, RoutedEventArgs e)
    {
        ProfileCancelButton.Visibility = Visibility.Visible;
        ProfileSaveButton.Visibility = Visibility.Visible;
        ProfileEditButton.Visibility = Visibility.Collapsed;
        ProfileNameTextBox.IsReadOnly = false;
        AgeTextBox.IsReadOnly = false;
        EmailTextBox.IsReadOnly = false;
        ProfileNameTextBox.IsEnabled = true;
        AgeTextBox.IsEnabled = true;
        EmailTextBox.IsEnabled = true;
    }

    private void ProfileCancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        ExitEditMode();
        profileHideStatus();
    }

    private void profileShowSucess()
    {
        ProfileStatusBorder.Visibility = Visibility.Visible;
        ProfileStatusBorder.Background = new SolidColorBrush(Color.FromRgb(232, 245, 233));
        ProfileStatusLabel.Foreground = new SolidColorBrush(Color.FromRgb(80, 125, 50));
        ProfileStatusLabel.Text = "Changes saved successfully!";
    }

    private void profileShowError(string msg = "")
    {
        ProfileStatusBorder.Visibility = Visibility.Visible;
        ProfileStatusBorder.Background = new SolidColorBrush(Color.FromRgb(255, 235, 238));
        ProfileStatusLabel.Foreground = new SolidColorBrush(Color.FromRgb(214, 76, 40));
        ProfileStatusLabel.Text = "Error: " + msg;
    }
    private bool ValidateAndApplyPasswordChange()
    {
        // Password fields hidden — nothing to do
        if (PasswordFieldsPanel.Visibility != Visibility.Visible)
            return true;

        if (CurrentPasswordBox.Password != _user.Password)
        {
            profileShowError("Current password is incorrect");
            return false;
        }
        if (NewPasswordBox.Password.Length < 8)
        {
            profileShowError("New password must be at least 8 characters");
            return false;
        }
        if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
        {
            profileShowError("Passwords do not match");
            return false;
        }

        // Don't save yet — just stage the new password on the object
        _user.Password = NewPasswordBox.Password;
        return true;
    }

    private void ProfileSaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(AgeTextBox.Text, out int age) || age <= 0 || age >= 99)
        {
            profileShowError("Invalid Age");
            return;
        }
        if (string.IsNullOrEmpty(ProfileNameTextBox.Text.Trim()) || ProfileNameTextBox.Text.Length < 3)
        {
            profileShowError("Invalid Name");
            return;
        }

        if (string.IsNullOrEmpty(EmailTextBox.Text.Trim()) || !UserDB.IsValidEmail(EmailTextBox.Text))
        {
            profileShowError("Invalid Email");
            return;
        }
        
        _user.Name = ProfileNameTextBox.Text;
        _user.Email = EmailTextBox.Text;
        _student.Age = age;
        
        // ...

        try
        {
            var studentDb = new StudentDB();
            var userDb = new UserDB();
            
            studentDb.Update(_student);
            userDb.Update(_user);
            
            int rowsChanged = BaseDB.SaveChanges();
            Console.WriteLine($"Lines modified: {rowsChanged}");
            
            if (rowsChanged <= 0)
            {
                profileShowError("Could not update details");
                ProfileCancelButton_OnClick(sender, e);
                return;
            }
            
            profileShowSucess();
            ExitEditMode();
            PasswordFieldsPanel.Visibility =  Visibility.Collapsed;
        }
        catch (Exception ex)
        {
            profileShowError($"Could not update details: {ex.Message}");
            LoadProfileData();
        }
    }

    private void ChangePasswordToggleButton_OnClick(object sender, RoutedEventArgs e)
    {
        bool opening = PasswordFieldsPanel.Visibility == Visibility.Collapsed;
        PasswordFieldsPanel.Visibility = opening ? Visibility.Visible : Visibility.Collapsed;
        ChangePasswordToggleButton.Content = opening ? "Cancel" : "Change Password";
        CurrentPasswordBox.Clear();
        NewPasswordBox.Clear();
        ConfirmPasswordBox.Clear();
        profileHideStatus();
    }

    private void ConfirmChangePassword_OnClick(object sender, RoutedEventArgs e)
    {
        if (CurrentPasswordBox.Password != _user.Password)
        {
            profileShowError("Current password is incorrect");
            return;
        }
        if (NewPasswordBox.Password.Length < 8)
        {
            profileShowError("New password must be at least 8 characters");
            return;
        }
        if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
        {
            profileShowError("Passwords do not match");
            return;
        }

        try
        {
            _user.Password = NewPasswordBox.Password;
            var userDb = new UserDB();
            userDb.Update(_user);

            if (BaseDB.SaveChanges() <= 0)
            {
                profileShowError("Could not update password");
                return;
            }

            // Collapse the panel and reset
            PasswordFieldsPanel.Visibility = Visibility.Collapsed;
            ChangePasswordToggleButton.Content = "Change Password";
            CurrentPasswordBox.Clear();
            NewPasswordBox.Clear();
            ConfirmPasswordBox.Clear();
            profileShowSucess();
        }
        catch (Exception ex)
        {
            profileShowError($"Could not update password: {ex.Message}");
        }
    }
}
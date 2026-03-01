using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace driving_lessons;

public enum UserType {
    None,
    Student,
    Teacher
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public UserType currentUserType = UserType.None;
    public MainWindow()
    {
        InitializeComponent();
        // MainFrame.NavigationService.Navigated += NavigationService_Navigated;
        MainFrame.Navigate(new LoginPage(this));
    }

    private void NavigationService_Navigated(object sender, NavigationEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void SignOut()
    {
        MainFrame.Navigate(new LoginPage(this));

        while (MainFrame.CanGoBack)
        {
            MainFrame.RemoveBackEntry();
        }
    }
}
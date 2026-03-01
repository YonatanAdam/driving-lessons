using System.Windows;
using System.Windows.Controls;
using Model;
using ViewModel;

namespace driving_lessons;

public partial class TeacherPage : Page {
    private Teacher _teacher;
    private User _user;
    public TeacherPage(User user, Teacher teacher)
    {
        InitializeComponent();
        _user = user;
        _teacher = teacher;
        
        DisplayInfo();
        BioTextBox.Text = _teacher.Bio ?? "";
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var newBio = BioTextBox.Text;
        
        TeacherDB teacherDb = new TeacherDB();
        teacherDb.UpdateBio(_teacher, newBio);
        
        BioTextBox.Text =  newBio;
    }

    private void DisplayInfo()
    {
        IdTextBlock.Text = _teacher.Id.ToString();
        UserIdTextBlock.Text = _teacher.UserId.ToString();
        NameTextBlock.Text = _user.Name;
        BioTextBlock.Text = _teacher.Bio ?? "No bio available";
    }
}
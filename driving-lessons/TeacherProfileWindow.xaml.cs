using System.Windows;
using Model;

namespace driving_lessons;

public partial class TeacherProfileWindow : Window {
    private Teacher _teacher;
    public TeacherProfileWindow(Teacher teacher)
    {
        InitializeComponent();
        _teacher = teacher;

        this.DataContext = _teacher;
    }

    private void LoadTeacherData()
    {
        
    }
}
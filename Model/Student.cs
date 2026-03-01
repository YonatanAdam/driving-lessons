namespace Model
{
    public class Student : User
    {
        private int age;
        private int? teacherId;
        private int userId;
        private int totalLessonsCompleted;
        private int totalHoursCompleted;

        public override string ToString()
        {
            return base.ToString() + $"Age: {age}, ";
        }

        public int Age
        {
            get => age;
            set => age = value;
        }

        public int? TeacherId
        {
            get => teacherId;
            set => teacherId = value;
        }

        public int UserId
        {
            get => userId;
            set => userId = value;
        }

        public int TotalLessonsCompleted
        {
            get => totalLessonsCompleted;
            set => totalLessonsCompleted = value;
        }

        public int TotalHoursCompleted
        {
            get => totalHoursCompleted;
            set => totalHoursCompleted = value;
        }
    }
}

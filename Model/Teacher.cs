namespace Model
{
    public class Teacher : User
    {
        private int? userId;
        private string? bio;
        private int? yearsExperience;
        private double? pricePerLesson;
        private string? carMake;
        private string? carModel;
        private string? transmissionType;
        private string? location;
        private string? languages;
        private double averageRating;
        private int totalReviews;
        private bool isAvailableForBooking;
        private UserList students = new UserList();

        public override string ToString()
        {
            return base.ToString() +
                   $"Bio: {bio}\n" +
                   $"Students: {(Students.Any()
                       ? string.Join(", ", Students.Select(s => s.Name))
                       : "0 students")}";
        }

        public int? UserId
        {
            get => userId;
            set => userId = value;
        }

        public string? Bio
        {
            get => bio;
            set => bio = value;
        }

        public UserList Students
        {
            get => students;
            set => students = value;
        }

        public int? YearsExperience
        {
            get => yearsExperience;
            set => yearsExperience = value;
        }

        public double? PricePerLesson
        {
            get => pricePerLesson;
            set => pricePerLesson = value;
        }

        public string? CarMake
        {
            get => carMake;
            set => carMake = value;
        }
        
        public string? CarModel
        {
            get => carModel;
            set => carModel = value;
        }

        public string? TransmissionType
        {
            get => transmissionType;
            set => transmissionType = value;
        }
        
        public string? Location
        {
            get => location;
            set => location = value;
        }
        
        public string? Languages
        {
            get => languages;
            set => languages = value;
        }
        
        public double AverageRating
        {
            get => averageRating;
            set => averageRating = value;
        }
        
        public int TotalReviews
        {
            get => totalReviews;
            set => totalReviews = value;
        }
        
        public bool IsAvailableForBooking
        {
            get => isAvailableForBooking;
            set => isAvailableForBooking = value;
        }
    }
}

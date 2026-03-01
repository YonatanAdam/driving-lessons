namespace Model
{
    public class User : BaseEntity
    {
        private string name;
        private string password;
        private string email;

        public override string ToString()
        {
            return base.ToString() + $"Name: {name}, Password: {password}, ";
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public string Email
        {
            get => email;
            set => email = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}

namespace Model
{
    public class UserList : List<User>
    {
        public UserList()
        {
        }

        public UserList(IEnumerable<User> list) : base(list)
        {
        }

        public UserList(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList())
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.Select(user => user.ToString()));
        }
    }
}

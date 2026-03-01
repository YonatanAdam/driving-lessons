namespace Model
{
    public class TeacherList : List<Teacher>
    {
        public TeacherList()
        {
        }

        public TeacherList(IEnumerable<Teacher> list) : base(list)
        {
        }

        public TeacherList(IEnumerable<BaseEntity> list) : base(list.Cast<Teacher>().ToList())
        {
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.Select(teacher => teacher.ToString()));
        }
    }
}

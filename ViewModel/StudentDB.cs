using Model;

namespace ViewModel
{
    public class StudentDB : BaseDB
    {
        protected override string CreateDeleteSQL(BaseEntity entity)
        {
            return $"DELETE FROM StudentTbl WHERE Id = {entity.Id}";
        }

        protected override string CreateInsertSQL(BaseEntity entity)
        {
            Student s = (Student)entity;
            string teacherId = s.TeacherId.HasValue ? s.TeacherId.Value.ToString() : "NULL";
            return $@"INSERT INTO StudentTbl (Age, TotalLessonsCompleted, UserId, TeacherId)
              VALUES({s.Age}, {s.TotalLessonsCompleted}, {s.Id}, {teacherId})";
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            Student s = (Student)entity;

            // StudentTbl columns
            s.Age                   = Convert.ToInt32(reader["Age"]);
            s.TeacherId             = reader["TeacherId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["TeacherId"]);
            s.TotalLessonsCompleted = Convert.ToInt32(reader["TotalLessonsCompleted"]);
            s.TotalHoursCompleted   = Convert.ToInt32(reader["TotalHoursCompleted"]);

            // UserTbl columns — s.Id must be the UserTbl Id so UserDB.Update works correctly
            s.Id    = Convert.ToInt32(reader["UserId"]);  // UserId is unambiguous — it's only in StudentTbl
            s.Name  = reader["Name"].ToString();
            s.Email = reader["Email"].ToString();

            return s;
        }

        protected override string CreateUpdateSQL(BaseEntity entity)
        {
            Student s = (Student)entity;
            string teacherId = s.TeacherId.HasValue ? s.TeacherId.Value.ToString() : "NULL";
            return $@"UPDATE StudentTbl SET Age = {s.Age}, TeacherId = {teacherId} 
              WHERE UserId = {s.Id}";
        }

        public StudentList SelectAll()
        {
            command.CommandText = @"
            SELECT s.Id    AS StudentPK,
                               s.Age, s.TotalLessonsCompleted, s.TotalHoursCompleted,
                               s.TeacherId, s.UserId,
                               u.Name, u.Email, u.Password
                        FROM StudentTbl s 
                        JOIN UserTbl u ON s.UserId = u.Id
            ";

            return new StudentList(Select());
        }

        protected override BaseEntity newEntity() => new Student();

        public override void Insert(BaseEntity entity)
        {
            if (entity is Student student)
            {
                inserted.Add(new ChangeEntity(CreateInsertSQL, student));
            }
        }

        public override void Delete(BaseEntity entity)
        {
            if (entity is Student student)
            {
                deleted.Add(new ChangeEntity(CreateDeleteSQL, student));
            }
        }

        public override void Update(BaseEntity entity)
        {
            if (entity is Student student)
            {
                updated.Add(new ChangeEntity(CreateUpdateSQL, student));
            }

        }

        public Student? SelectById(int id) => SelectAll().FirstOrDefault(s => s.Id == id);

        public string GetTeacherName(int teacherId)
        {
            TeacherDB teacherDb = new  TeacherDB();
            var teacher = teacherDb.SelectByUserId(teacherId);
            return teacher != null ? teacher.Name : "NULL";
        }
    }
}

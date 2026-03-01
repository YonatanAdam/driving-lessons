using Model;

namespace ViewModel
{
    public class TeacherDB : BaseDB
    {
        protected override string CreateDeleteSQL(BaseEntity entity)
        {
            return $"DELETE FROM TeacherTbl WHERE Id = {entity.Id}";
        }

        protected override string CreateInsertSQL(BaseEntity entity)
        {
            Teacher t = (Teacher)entity;
            return @$"INSERT INTO TeacherTbl (UserId, Bio, YearsExperience,
                                  PricePerLesson, CarMake, CarModel, TransmissionType,
                                  Location, Languages, AverageRating, TotalReviews, IsAvailableForBooking)
                      VALUES ({t.UserId},
                              '{EscapeString(t.Bio ?? "")}',
                               {t.YearsExperience},
                               {t.PricePerLesson},
                              '{EscapeString(t.CarMake)}',
                              '{EscapeString(t.CarModel)}',
                              '{EscapeString(t.TransmissionType)}',
                              '{EscapeString(t.Location)}',
                              '{EscapeString(t.Languages)}',
                               {t.TotalReviews},
                               {t.IsAvailableForBooking})";
        }

        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            var teacher = (Teacher)entity;
            
            teacher.Id = Convert.ToInt32(reader["Id"]);
            teacher.UserId = Convert.ToInt32(reader["UserId"]);
            teacher.Bio = reader["Bio"].ToString()  ?? string.Empty;
            teacher.Name = reader["Name"].ToString() ?? string.Empty;
            teacher.YearsExperience = Convert.ToInt32(reader["YearsExperience"]);
            teacher.PricePerLesson = Convert.ToInt32(reader["PricePerLesson"]);
            teacher.CarMake = reader["CarMake"].ToString() ?? string.Empty;
            teacher.CarModel = reader["CarModel"].ToString() ?? string.Empty;
            teacher.TransmissionType = reader["TransmissionType"].ToString() ?? string.Empty;
            teacher.Location = reader["Location"].ToString() ?? string.Empty;
            teacher.Languages = reader["Languages"].ToString() ?? string.Empty;
            teacher.AverageRating = Convert.ToDouble(reader["AverageRating"]);
            teacher.TotalReviews = Convert.ToInt32(reader["TotalReviews"]);
            
            // for the user
            teacher.Email = reader["Email"].ToString() ?? string.Empty;

            return teacher;
        }

        protected override string CreateUpdateSQL(BaseEntity entity)
        {
            Teacher t = (Teacher)entity;

            return @$"UPDATE TeacherTbl SET
                UserId = {t.UserId},
                Bio = '{EscapeString(t.Bio ?? "")}',
                YearsExperience = {t.YearsExperience},
                PricePerLesson = {t.PricePerLesson},
                CarMake = '{EscapeString(t.CarMake ?? "")}',
                CarModel = '{EscapeString(t.CarModel ?? "")}',
                TransmissionType = '{EscapeString(t.TransmissionType ?? "")}',
                Location = '{EscapeString(t.Location ?? "")}',
                Languages = '{EscapeString(t.Languages ?? "")}',
                AverageRating = {t.AverageRating},
                TotalReviews = {t.TotalReviews},
                IsAvailableForBooking = {t.IsAvailableForBooking}
              WHERE Id = {t.Id}";
        }


        public TeacherList SelectAll()
        {
            command.CommandText = @" SELECT 
                                        t.Id,
                                        t.UserId,
                                        t.Bio,
                                        t.YearsExperience,
                                        t.PricePerLesson,
                                        t.CarMake,
                                        t.CarModel,
                                        t.TransmissionType,
                                        t.Location,
                                        t.Languages,
                                        t.AverageRating,
                                        t.TotalReviews,
                                        t.IsAvailableForBooking,
                                        u.Name,
                                        u.Email
                                    FROM TeacherTbl t 
                                    JOIN UserTbl u ON t.UserId = u.Id";
            return new TeacherList(Select());
        }


        protected override BaseEntity newEntity() => new Teacher();
        
        public Teacher? SelectByUserId(int userId) => SelectAll().FirstOrDefault(t => t.UserId == userId);

        public void UpdateBio(Teacher teacher, string bio)
        {
            teacher.Bio = bio;
            Update(teacher);
            var rows = SaveChanges();
        }
    }
}

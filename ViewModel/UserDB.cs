using Model;
using System.Text;
using System.Text.RegularExpressions;

namespace ViewModel
{
    /// <summary>
    /// Provides database operations for the User entity, including CRUD operations, authentication, and group membership management.
    /// </summary>
    public class UserDB : BaseDB
    {
        /// <summary>
        /// Adds a User entity to the list of entities to be inserted into the database.
        /// </summary>
        /// <param name="entity">The user entity to insert.</param>
        public override void Insert(BaseEntity entity)
        {
            if (entity is User user)
            {
                inserted.Add(new ChangeEntity(CreateInsertSQL, user));
            }
        }

        /// <summary>
        /// Adds a User entity to the list of entities to be deleted from the database.
        /// </summary>
        /// <param name="entity">The User entity to delete.</param>
        public override void Delete(BaseEntity entity)
        {
            if (entity is User user)
            {
                deleted.Add(new ChangeEntity(CreateDeleteSQL, user));
            }
        }

        /// <summary>
        /// Adds a User entity to the list of entities to be updated in the database.
        /// </summary>
        /// <param name="entity">The User entity to update.</param>
        public override void Update(BaseEntity entity)
        {
            if (entity is User user)
            {
                updated.Add(new ChangeEntity(CreateUpdateSQL, user));
            }
        }

        /// <summary>
        /// Creates the SQL statement for deleting a User entity.
        /// </summary>
        /// <param name="entity">The User entity to delete.</param>
        /// <returns>The SQL delete statement.</returns>
        protected override string CreateDeleteSQL(BaseEntity entity)
        {
            User user = (User)entity;
            StringBuilder sql_builder = new StringBuilder();
            sql_builder.AppendFormat("DELETE FROM UserTbl WHERE Id = {0}", user.Id);
            return sql_builder.ToString();
        }

        /// <summary>
        /// Creates the SQL statement for inserting a User entity.
        /// Note: Ensure your BaseDB execution logic handles parameters added to the command.
        /// </summary>
        protected override string CreateInsertSQL(BaseEntity entity)
        {
            User user = (User)entity;

            return $@"INSERT INTO UserTbl (Name, Email, Password) 
             VALUES ('{EscapeString(user.Name)}', '{EscapeString(user.Email)}' , '{EscapeString(user.Password)}')";
        }

        /// <summary>
        /// Creates a User model from the current data reader row.
        /// </summary>
        /// <param name="entity">The base entity to populate as a User.</param>
        /// <returns>A populated User object.</returns>
        protected override BaseEntity CreateModel(BaseEntity entity)
        {
            User userEntity = (User)entity;
            userEntity.Id = Convert.ToInt32(reader["Id"]);
            userEntity.Name = reader["Name"].ToString()!;
            userEntity.Email = reader["Email"].ToString()!;
            userEntity.Password = reader["Password"].ToString()!;
            return userEntity;
        }

        /// <summary>
        /// Creates the SQL statement for updating a User entity.
        /// </summary>
        protected override string CreateUpdateSQL(BaseEntity entity)
        {
            User user = (User)entity;

            return $@"UPDATE UserTbl 
             SET Name = '{EscapeString(user.Name)}', Email = '{EscapeString(user.Email)}', Password = '{EscapeString(user.Password)}' WHERE Id = {user.Id}";
        }

        protected override BaseEntity newEntity()
        {
            return new User();
        }

        public UserList SelectAll()
        {
            string sqlStmt = $"SELECT * FROM UserTbl";
            command.CommandText = sqlStmt;
            var users = new UserList(Select());

            return users;
        }

        public User? SelectById(int id)
        {
            command.Parameters.Clear();
            command.CommandText = $"SELECT * FROM UserTbl WHERE Id = {id}";

            var users = new UserList(Select());
            return users.FirstOrDefault();
        }

        public void DeleteAll()
        {
            var allUsers = SelectAll();
            foreach (var user in allUsers)
            {
                Delete(user);
            }
        }

        public void DeleteById(int id)
        {
            var user = SelectById(id);

            if (user != null)
            {
                Delete(user);
            }
            else
            {
                Console.WriteLine($"User id {id} not found.");
            }
        }

        public User? SelectByName(string name) => SelectAll().FirstOrDefault(s => s.Name == name);

        public static bool IsValidEmail(string email) => Regex.IsMatch(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
    }
}

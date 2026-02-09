using Model;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using System.Data;

namespace ViewModel
{
    /// <summary>
    /// Abstract base class for database access objects, providing common CRUD operations and change tracking for entities.
    /// </summary>
    public abstract class BaseDB
    {
        private static string? connectionString;
        protected SqliteConnection connection;
        protected SqliteCommand command;
        protected SqliteDataReader reader;

        /// <summary>
        /// List of entities to be inserted on SaveChages
        /// </summary>
        protected static List<ChangeEntity> inserted = new List<ChangeEntity>();

        /// <summary>
        /// List of entities to be deleted on SaveChages
        /// </summary>
        protected static List<ChangeEntity> deleted = new List<ChangeEntity>();

        /// <summary>
        /// List of entities to be updated on SaveChages
        /// </summary>
        protected static List<ChangeEntity> updated = new List<ChangeEntity>();

        /// <summary>
        /// Creates the SQL statement for inserting an entity
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The SQL insert statement.</returns>
        protected abstract string CreateInsertSQL(BaseEntity entity);

        /// <summary>
        /// Creates the SQL statement for updating an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The SQL update statement.</returns>
        protected abstract string CreateUpdateSQL(BaseEntity entity);

        /// <summary>
        /// Creates the SQL statement for deleting an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>The SQL delete statement.</returns>
        protected abstract string CreateDeleteSQL(BaseEntity entity);

        /// <summary>
        /// Create a new instance of the entity type handled by this database class.
        /// </summary>
        /// <returns>A new entity instance.</returns>
        protected abstract BaseEntity newEntity();

        /// <summary>
        /// Populates an entity from the current data reader row.
        /// </summary>
        /// <param name="entity">The entity to populate.</param>
        /// <returns>The populated entity.</returns>
        protected abstract BaseEntity CreateModel(BaseEntity entity);

        public BaseDB()
        {
            if (connectionString == null)
            {
                string dbPath = Path.GetFullPath(@"..\\..\\..\\..\\ViewModel\\yourname.db");
                connectionString = $"Data Source={dbPath};";
            }

            connection = new SqliteConnection(connectionString);
            command = connection.CreateCommand();
        }

        /// <summary>
        /// Executes the current command and returns a list of entities from the result set.
        /// </summary>
        /// <returns>A list of entitites from the query result.</returns>
        protected List<BaseEntity> Select()
        {
            List<BaseEntity> list = new List<BaseEntity>();

            try
            {
                command.Connection = connection;
                connection.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BaseEntity entity = newEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\nSQL" + command.CommandText);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return list;
        }

        /// <summary>
        /// Selects all entities fomr the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to select from.</param>
        /// <returns>A lsit of entities from the specified table.</returns>
        public List<BaseEntity> Select(string tableName)
        {
            List<BaseEntity> list = new List<BaseEntity>();

            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(tableName, @"^[a-zA-Z0-9_]+$"))
                {
                    throw new ArgumentException("Invalid table name.");
                }

                command.Connection = connection;
                connection.Open();

                command.CommandText = $"SELECT * FROM {tableName}";

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BaseEntity entity = newEntity();
                    list.Add(CreateModel(entity));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\nError executing SQL: {command.CommandText}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return list;
        }

        /// <summary>
        /// Commits all pending insert, update, and delete operations to the database.
        /// </summary>
        /// <returns>The numder of records affected</returns>
        public static int SaveChanges()
        {
            int records_affected = 0;

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    using (SqliteCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;

                        try
                        {
                            foreach (var item in inserted)
                            {
                                cmd.CommandText = item.CreateSQL(item.Entity);
                                records_affected += cmd.ExecuteNonQuery();

                                cmd.CommandText = "SELECT last_insert_rowid()"; // get last id
                                item.Entity.Id = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            foreach (var item in updated)
                            {
                                cmd.CommandText = item.CreateSQL(item.Entity);
                                records_affected += cmd.ExecuteNonQuery();
                            }

                            foreach (var item in deleted)
                            {
                                cmd.CommandText = item.CreateSQL(item.Entity);
                                records_affected += cmd.ExecuteNonQuery();
                            }

                            transaction.Commit(); // Success
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            Debug.WriteLine($"Database Error: {e.Message}");
                            throw;
                        }
                        finally
                        {
                            inserted.Clear();
                            updated.Clear();
                            deleted.Clear();
                        }
                    }
                }
            }
            return records_affected;
        }

        /// <summary>
        /// Adds an entity to the list of entities to be inserted.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public virtual void Insert(BaseEntity entity)
        {
            BaseEntity reqEntity = newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                inserted.Add(new ChangeEntity(CreateInsertSQL, entity));
            }
        }
        /// <summary>
        /// Adds an entity to the list of entities to be updated.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(BaseEntity entity)
        {
            BaseEntity reqEntity = newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                updated.Add(new ChangeEntity(CreateUpdateSQL, entity));
            }
        }
        /// <summary>
        /// Adds an entity to the list of entities to be deleted..
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(BaseEntity entity)
        {
            BaseEntity reqEntity = newEntity();
            if (entity != null && entity.GetType() == reqEntity.GetType())
            {
                deleted.Add(new ChangeEntity(CreateDeleteSQL, entity));
            }
        }
    }
}

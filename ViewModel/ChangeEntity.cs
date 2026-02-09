using Model;

namespace ViewModel
{
    public delegate string CreateSQL(BaseEntity entity);
    public class ChangeEntity
    {
        private BaseEntity entity;
        private CreateSQL createSql;

        public ChangeEntity(CreateSQL createSql, BaseEntity entity)
        {
            this.entity = entity;
            this.createSql = createSql;
        }

        public BaseEntity Entity { get => entity; set => entity = value; }
        public CreateSQL CreateSQL { get => createSql; set => createSql = value; }
    }
}

using Domain.Entities.Common;

namespace Domain.Entities
{
    public class EntityForDapper
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DapperBool { get; set; }
    }
}

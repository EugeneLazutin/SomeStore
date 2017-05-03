using System.ComponentModel.DataAnnotations;

namespace SomeStore.Infrustructure.DbEntities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}

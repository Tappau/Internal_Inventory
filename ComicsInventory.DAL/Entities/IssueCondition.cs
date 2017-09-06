using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicsInventory.DAL.Entities
{
    [Table("IssueCondition")]
    public class IssueCondition
    {
        [Key]
        public int IssueCondition_ID { get; set; }

        public int? Issue_ID { get; set; }

        public int? Grade_ID { get; set; }

        public int? quantity { get; set; }

        public virtual Grade Grade { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
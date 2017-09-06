using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ComicsInventory.DAL.Entities
{
    [Table("Grade")]
    public class Grade
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grade()
        {
            IssueConditions = new HashSet<IssueCondition>();
        }

        public int GradeID { get; set; }

        [StringLength(6)]
        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IssueCondition> IssueConditions { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ComicsInventory.DAL.Entities
{
    public class Series
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Series()
        {
            Issues = new HashSet<Issue>();
        }

        [Key]
        public int Series_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Series_Name { get; set; }

        public int? Year_Began { get; set; }

        public int? Year_End { get; set; }

        [StringLength(255)]
        public string dimensions { get; set; }

        [StringLength(255)]
        public string paperStock { get; set; }

        public int? Publisher_ID { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
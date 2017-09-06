using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ComicsInventory.DAL.Entities
{
    [Table("Publisher")]
    public class Publisher
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Publisher()
        {
            Series = new HashSet<Series>();
        }

        [Key]
        public int Publisher_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Pub_Name { get; set; }

        public int? Year_Began { get; set; }

        public string notes { get; set; }

        [StringLength(255)]
        public string URL { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Series> Series { get; set; }
    }
}
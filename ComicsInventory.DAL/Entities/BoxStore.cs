using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ComicsInventory.DAL.Entities
{
    [Table("BoxStore")]
    public class BoxStore
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BoxStore()
        {
            Issues = new HashSet<Issue>();
        }

        [Key]
        public int BoxID { get; set; }

        [StringLength(255)]
        public string BoxName { get; set; }

        [StringLength(500)]
        public string QR_Data { get; set; }

        public bool isActive { get; set; }

        public string Notes { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
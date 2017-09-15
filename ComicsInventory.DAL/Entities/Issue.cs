using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ComicsInventory.DAL.Entities
{
    [Table("Issue")]
    public class Issue
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Issue()
        {
            IssueConditions = new HashSet<IssueCondition>();
        }

        [Key]
        public int Issue_ID { get; set; }

        public int? Series_ID { get; set; }

        public int? Box_ID { get; set; }

        [Required]
        [StringLength(6)]
        public string Number { get; set; }

        [StringLength(255)]
        public string publication_date { get; set; }

        public decimal? page_count { get; set; }

        [StringLength(255)]
        public string frequency { get; set; }

        [StringLength(1600)]
        [Display(Name = "Editor")]
        public string editor { get; set; }

        [StringLength(13)]

        public string ISBN { get; set; }

        [StringLength(38)]
        public string barcode { get; set; }

        public bool? IsActive { get; set; }

        [Display(Name = "Added On")]
        public DateTime? AddedOn { get; set; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public int? GCDIssueNumber { get; set; }


        public virtual BoxStore BoxStore { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IssueCondition> IssueConditions { get; set; }

        public virtual Series Series { get; set; }
    }
}
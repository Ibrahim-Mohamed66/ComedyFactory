using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public abstract class BaseEntity
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;

        [Display(Name = "Updated On Utc")]
        public DateTime? UpdatedOnUtc { get; set; }

        [Display(Name = "Display Order")]
        [DefaultValue(10)]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Hidden")]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool Hidden { get; set; }

    }
}
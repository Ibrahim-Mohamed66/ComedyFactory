using Fingers10.ExcelExport.Attributes;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.ViewModels
{
    public class ProfessorViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "ArName")]
        public string ArName { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [MaxLength(255, ErrorMessage = "Maximum characters is 255 character")]
        [Display(Name = "EnName")]
        public string EnName { get; set; }


        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Arabic Picture")]
        [UIHint("PicUploader")]
        public string ArPicture { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "English Picture")]
        [UIHint("PicUploader")]
        public string EnPicture { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "Arabic Content")]
        [UIHint("Editor")]
        public string ArContent { get; set; }

        [Required(ErrorMessage = "This {0} field is required")]
        [Display(Name = "English Content")]
        [UIHint("Editor")]
        public string EnContent { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "*")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Display(Name = "Created On Utc")]
        public DateTime? CreatedOnUtc { get; set; }

        [Display(Name = "Updated On Utc")]
        public DateTime? UpdatedOnUtc { get; set; }

        [Display(Name = "Display Order")]
        [DefaultValue(1)]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Hidden")]
        [DefaultValue(false)]
        public bool Hidden { get; set; }
        [Display(Name = "Master Category")]
        public int? MasterCategoryId { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> MasterCategories { get; set; }
    }
}

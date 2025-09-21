using Application.DTOS;

namespace WebUI.Models;

public class HomeViewModel
{
    public IEnumerable<BlockDto> Blocks { get; set; }
    public IEnumerable<MasterCategoryDto> MasterCategories { get; set; }
    public IEnumerable<ProfessorDto> Professors { get; set; }
    public IEnumerable<ActivityDto> Activities { get; set; }
    public BlockDto HomePage { get; set; }

}

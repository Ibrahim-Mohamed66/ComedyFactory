using Application.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IConfigurationService
    {
        Task<ConfigurationDto> AddConfigurationAsync(ConfigurationDto configurationDto);
        Task<ConfigurationDto?> GetConfigurationAsync(int id);
        Task<ConfigurationDto?> UpdateConfigurationAsync(int id,ConfigurationDto configurationDto);
        Task<bool> AnyAsync();
        Task<ConfigurationDto?> GetFirstOrDefaultAsync();
    }
}

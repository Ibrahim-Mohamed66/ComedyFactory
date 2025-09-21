using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories;
using Data.Repositories.IRepositories;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ConfigurationService: IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMapper _mapper;
        public ConfigurationService(IConfigurationRepository configurationRepository, IMapper mapper)
        {
            _configurationRepository = configurationRepository;
            _mapper = mapper;
        }

        public async Task<ConfigurationDto> AddConfigurationAsync(ConfigurationDto configurationDto)
        {
            var entity = _mapper.Map<Configuration>(configurationDto);
            var result = await _configurationRepository.AddConfigurationAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to add configuration");
            }
            return _mapper.Map<ConfigurationDto>(entity);

        }

        public Task<bool> AnyAsync()
        {
            return _configurationRepository.AnyAsync();
        }

        public async Task<ConfigurationDto?> GetConfigurationAsync(int id)
        {
            var entity = await _configurationRepository.GetConfigurationAsync(id);
            return entity == null ? null : _mapper.Map<ConfigurationDto>(entity);
        }

        public async Task<ConfigurationDto?> GetFirstOrDefaultAsync()
        {
            var entity = await  _configurationRepository.GetFirstOrDefaultAsync();
            return entity == null ? null : _mapper.Map<ConfigurationDto>(entity);

        }

        public async Task<ConfigurationDto?> UpdateConfigurationAsync(int id, ConfigurationDto configurationDto)
        {
            var entity = await _configurationRepository.GetConfigurationAsync(id);
            if (entity == null) return null;

            _mapper.Map(configurationDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _configurationRepository.UpdateConfiguration(entity);
            return _mapper.Map<ConfigurationDto>(entity);
        }
    }
}

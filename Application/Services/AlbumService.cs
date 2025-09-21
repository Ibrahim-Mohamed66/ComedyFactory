using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System.Linq.Expressions;


namespace Application.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;


        public AlbumService(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<AlbumDto> CreateAlbumAsync(AlbumDto albumDto)
        {
            var entity = _mapper.Map<Album>(albumDto);

            var result = await _albumRepository.AddAlbumAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create Album");
            }
            return _mapper.Map<AlbumDto>(entity);
        }

        public async Task<bool> DeleteAlbumAsync(int id)
        {
            return await _albumRepository.DeleteAlbumAsync(id);
        }

        public async Task<JqueryDataTablesPagedResults<AlbumDto>> GetAlbumsDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<Album, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }
            // Fix: Specify the navigation property name for Include (e.g., "Country")
            var albums = (await _albumRepository.GetAllAlbumsAsync(
                filter
            )).AsQueryable();


            albums = SearchOptionsProcessor<AlbumDto, Album>.Apply(albums, table.Columns);
            albums = SortOptionsProcessor<AlbumDto, Album>.Apply(albums, table);

            var totalRecords = albums.Count();
            var filteredRecords = await _albumRepository.GetCountAsync(filter);

            var data = albums
                .Skip(table.Start)
                .Take(table.Length)
                .Select(a => _mapper.Map<AlbumDto>(a))
                .ToList();

            return new JqueryDataTablesPagedResults<AlbumDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }

        public async Task<AlbumDto?> GetAlbumByIdAsync(int id)
        {
            var entity = await _albumRepository.GetAlbumByIdAsync(id);
            return entity == null ? null : _mapper.Map<AlbumDto>(entity);
        }

        public async Task<AlbumDto?> UpdateAlbumAsync(int id, AlbumDto albumDto)
        {
            var entity = await _albumRepository.GetAlbumByIdAsync(id);
            if (entity == null) return null;

            _mapper.Map(albumDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _albumRepository.UpdateAlbumAsync(entity);
            return _mapper.Map<AlbumDto>(entity);
        }

        public async Task<IEnumerable<AlbumDto>> GetAllAlbumsAsync()
        {
            var entities = await _albumRepository.GetAllAlbumsAsync();
            return _mapper.Map<IEnumerable<AlbumDto>>(entities);
        }
    }
}

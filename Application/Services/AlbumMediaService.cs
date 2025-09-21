using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories.IRepositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Services;

public class AlbumMediaService : IAlbumMediaService
{
    private readonly IAlbumMediaRepository _albumMediaRepository;
    private readonly IMapper _mapper;
    public AlbumMediaService(IAlbumMediaRepository albumMediaRepository, IMapper mapper)
    {
        _albumMediaRepository = albumMediaRepository;
        _mapper = mapper;
    }
    public async Task<AlbumMediaDto> CreateAlbumMediaAsync(AlbumMediaDto albumMediaDto)
    {
        var entity = _mapper.Map<AlbumMedia>(albumMediaDto);

        var result = await _albumMediaRepository.AddAlbumMediaAsync(entity);
        if (!result)
        {
            throw new Exception("Failed to create AlbumMedia");
        }
        return _mapper.Map<AlbumMediaDto>(entity);
    }

    public async Task<bool> DeleteAlbumMediaAsync(int id)
    {
        return await _albumMediaRepository.DeleteAlbumMediaAsync(id);
    }

    public async Task<JqueryDataTablesPagedResults<AlbumMediaDto>> GetAlbumMediasDataTableAsync(int albumId, JqueryDataTablesParameters table)
    {
        Expression<Func<AlbumMedia, bool>>? filter = null;
        if (!string.IsNullOrEmpty(table.Search?.Value))
        {
            var search = table.Search.Value.ToLower();
            filter = c => !c.Deleted &&
                          (c.MediaType.ToString().ToLower().Contains(search) ||
                           c.Album.EnName.ToLower().Contains(search) ||

                           c.Album.ArName.ToLower().Contains(search));

        }

        var albumMedias = (await _albumMediaRepository.GetAlbumMediasByAlbumIdAsync(
            albumId,
            filter,
            include: a => a.Include(am => am.Album)
        )).AsQueryable();

        var mappedResult = _mapper.ProjectTo<AlbumMediaDto>(albumMedias);
        mappedResult = SearchOptionsProcessor<AlbumMediaDto, AlbumMediaDto>.Apply(mappedResult, table.Columns);
        mappedResult = SortOptionsProcessor<AlbumMediaDto, AlbumMediaDto>.Apply(mappedResult, table);

        var totalRecords = mappedResult.Count();
        var filteredRecords = await _albumMediaRepository.GetCountAsync(filter);

        var data = mappedResult
            .Skip(table.Start)
            .Take(table.Length)
            .ToList();

        return new JqueryDataTablesPagedResults<AlbumMediaDto>
        {
            Items = data,
            TotalSize = totalRecords,
        };
    }

    public async Task<AlbumMediaDto?> GetAlbumMediaByIdAsync(int id)
    {
        var entity = await _albumMediaRepository.GetAlbumMediaByIdAsync(id);
        return entity == null ? null : _mapper.Map<AlbumMediaDto>(entity);
    }

    public async Task<AlbumMediaDto?> UpdateAlbumMediaAsync(int id, AlbumMediaDto albumDto)
    {
        var entity = await _albumMediaRepository.GetAlbumMediaByIdAsync(id);
        if (entity == null) return null;

        _mapper.Map(albumDto, entity);
        entity.UpdatedOnUtc = DateTime.UtcNow;

        await _albumMediaRepository.UpdateAlbumMediaAsync(entity);
        return _mapper.Map<AlbumMediaDto>(entity);
    }

    public async Task<IEnumerable<AlbumMediaDto>> GetAllAlbumMediasAsync()
    {
        var entities = await _albumMediaRepository.GetAllAlbumMediaAsync();
        return _mapper.Map<IEnumerable<AlbumMediaDto>>(entities);
    }

}

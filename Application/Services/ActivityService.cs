using Application.DTOS;
using Application.IServices;
using AutoMapper;
using Data.Repositories.IRepositories;
using Domain.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System.Linq.Expressions;

namespace Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IMapper _mapper;
        public ActivityService(IActivityRepository activityRepo, IMapper mapper)
        {
            _activityRepo = activityRepo;
            _mapper = mapper;
        }
        public async Task<ActivityDto> CreateActivityAsync(ActivityDto activityDto)
        {
            var entity = _mapper.Map<Activity>(activityDto);

            var result = await _activityRepo.AddActivityAsync(entity);
            if (!result)
            {
                throw new Exception("Failed to create Activity");
            }
            return _mapper.Map<ActivityDto>(entity);
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            return await _activityRepo.DeleteActivityAsync(id);
        }

        public async Task<JqueryDataTablesPagedResults<ActivityDto>> GetActivitiesDataTableAsync(JqueryDataTablesParameters table)
        {
            Expression<Func<Activity, bool>>? filter = null;
            if (!string.IsNullOrEmpty(table.Search?.Value))
            {
                var search = table.Search.Value.ToLower();
                filter = c => !c.Deleted &&
                              (c.EnName.ToLower().Contains(search) ||
                               c.ArName.ToLower().Contains(search));
            }


            var activities = (await _activityRepo.GetAllActivitiesAsync(filter)).AsQueryable();
            activities = SearchOptionsProcessor<ActivityDto, Activity>.Apply(activities, table.Columns);
            activities = SortOptionsProcessor<ActivityDto, Activity>.Apply(activities, table);


            var totalRecords = activities.Count();
            var filteredRecords = await _activityRepo.GetCountAsync(filter);

            var data = activities
                .Skip(table.Start)
                .Take(table.Length)
                .Select(d => _mapper.Map<ActivityDto>(d))
                .ToList();

            return new JqueryDataTablesPagedResults<ActivityDto>
            {
                Items = data,
                TotalSize = totalRecords,
            };
        }


        public async Task<ActivityDto?> GetActivityByIdAsync(int id)
        {
            var entity = await _activityRepo.GetActivityByIdAsync(id);
            return entity == null ? null : _mapper.Map<ActivityDto>(entity);
        }

        public async Task<IEnumerable<ActivityDto>> GetAllActivitiesAsync()
        {
            var entities = await _activityRepo.GetAllActivitiesAsync();
            return _mapper.Map<IEnumerable<ActivityDto>>(entities);
        }

        public async Task<ActivityDto?> UpdateActivityAsync(int id, ActivityDto activityDto)
        {
            var entity = await _activityRepo.GetActivityForUpdateAsync(id);
            if (entity == null) return null;

            _mapper.Map(activityDto, entity);
            entity.UpdatedOnUtc = DateTime.UtcNow;

            await _activityRepo.UpdateActivityAsync(entity);
            return _mapper.Map<ActivityDto>(entity);
        }

    }
}

namespace Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks;

using Microsoft.EntityFrameworkCore;
using OysterFx.Infra.Auth.UserServices;
using OysterFx.Infra.Persistence.RDB.Queries;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.GetAllWorkItems;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries.GetWorkItemByBusinessKey;
using System.Collections.Generic;

public class WorkItemQueryRepository : QueryRepository<AppQueryDbContext>, IWorkItemQueryRepository
{
    private readonly IUserInfoService _userInfoService;

    public WorkItemQueryRepository(AppQueryDbContext dbContext, IUserInfoService userInfoService) : base(dbContext)
    {
        _userInfoService = userInfoService;
    }

    public async Task<GetWorkItemByBusinessKeyQueryResult?> QueryAsync(GetWorkItemByBusinessKeyQuery query)
    {
        var workItemQuery = _dbContext.WorkItems.AsNoTracking();
        var workItemScheduleQuery = _dbContext.WorkItemSchedule.AsNoTracking();
        var queryResult = await (from workItem in workItemQuery
                                 join schedule in workItemScheduleQuery on workItem.BusinessKey equals schedule.WorkItemBusinessKey into schedules
                                 where query.BusinessKey == workItem.BusinessKey
                                 select new GetWorkItemByBusinessKeyQueryResult()
                                 {
                                     Id = workItem.Id,
                                     BusinessKey = workItem.BusinessKey,
                                     RecurrenceType = workItem.RecurrenceType,
                                     Status = workItem.Status,
                                     Title = workItem.Title,
                                     AlarmTime = workItem.AlarmTime,
                                     HasAlarm = workItem.HasAlarm,
                                     Schedules = schedules.Select(c => new WorkItemScheduleDto()
                                     {
                                         Id = c.Id,
                                         BusinessKey = c.BusinessKey,
                                         IsDone = c.IsDone,
                                         ScheduledDate = c.ScheduledDate,
                                         WorkItemBusinessKey = workItem.BusinessKey,
                                     }).ToList()
                                 }).FirstOrDefaultAsync();

        return await Task.FromResult(queryResult);
    }

    public async Task<List<GetAllWorkItemsQueryResult?>> QueryAsync(GetAllWorkItemsQuery query)
    {
        var workItemQuery = _dbContext.WorkItems.AsNoTracking();
        var workItemScheduleQuery = _dbContext.WorkItemSchedule.AsNoTracking();
        var queryResult = await(from workItem in workItemQuery
                                join schedule in workItemScheduleQuery on workItem.BusinessKey equals schedule.WorkItemBusinessKey into schedules
                                select new GetAllWorkItemsQueryResult()
                                {
                                    Id = workItem.Id,
                                    BusinessKey = workItem.BusinessKey,
                                    RecurrenceType = workItem.RecurrenceType,
                                    Status = workItem.Status,
                                    Title = workItem.Title,
                                    AlarmTime = workItem.AlarmTime,
                                    HasAlarm = workItem.HasAlarm,
                                    Schedules = schedules.Select(c => new WorkItemScheduleDto()
                                    {
                                        Id = c.Id,
                                        BusinessKey = c.BusinessKey,
                                        IsDone = c.IsDone,
                                        ScheduledDate = c.ScheduledDate,
                                        WorkItemBusinessKey = workItem.BusinessKey,
                                    }).ToList()
                                }).ToListAsync();

        return await Task.FromResult(queryResult);
    }
}
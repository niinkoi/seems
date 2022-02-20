using Microsoft.EntityFrameworkCore;
using SEEMS.Contexts;
using SEEMS.Data.Entities.RequestFeatures;
using SEEMS.Models;
using SEEMS.Services;

namespace SEEMS.Data.Repositories.Implements;

public class ChainOfEventRepository : RepositoryBase<ChainOfEvent>, IChainOfEventsRepository
{

    public ChainOfEventRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<PaginatedList<ChainOfEvent>> GetAllChainOfEventsAsync(int userId, ChainOfEventsPagination args, bool trackChanges)
    {
        var listChainOfEvents = await FindByCondition(e => e.CreatedBy == userId, false) 
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
        
        return PaginatedList<ChainOfEvent>.Create(listChainOfEvents, args.PageNumber, args.PageSize);
    }

    public async Task<ChainOfEvent> GetChainOfEventsAsync(int theId, bool trackChanges) =>
        await FindByCondition(e => e.Id == theId, trackChanges)
            .SingleOrDefaultAsync();

    public void CreateChainOfEvent(int userId, ChainOfEvent chainOfEvent)
    {
        chainOfEvent.CreatedBy = userId;
        Create(chainOfEvent);
    }

    public void DeleteChainOfEvent(ChainOfEvent chainOfEvent) => Delete(chainOfEvent);
}
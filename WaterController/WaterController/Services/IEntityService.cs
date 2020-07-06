using System.Collections.Generic;
using System.Threading.Tasks;
using ContextBrokerLibrary.Model;

namespace WaterController.Services
{
    public interface IEntityService
    {
        Task<object> GetFlowerBed(string entityId);
        Task<List<ListEntitiesResponse>> GetMoistureLevels(string bedId);
    }
}
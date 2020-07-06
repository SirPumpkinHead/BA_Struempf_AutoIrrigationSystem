using System.Collections.Generic;
using System.Threading.Tasks;
using ContextBrokerLibrary.Api;
using ContextBrokerLibrary.Model;

namespace WaterController.Services.Impl
{
    public class EntityService : IEntityService
    {
        private readonly IEntitiesApi _entitiesApi;

        public EntityService(IEntitiesApi entitiesApi)
        {
            _entitiesApi = entitiesApi;
        }

        public async Task<object> GetFlowerBed(string entityId)
        {
            return await _entitiesApi.RetrieveEntityAsync(entityId, "FlowerBed");
        }

        public async Task<List<ListEntitiesResponse>> GetMoistureLevels(string bedId)
        {
            return await _entitiesApi.ListEntitiesAsync(null, "Sensor", null, null, $"refBed=={bedId}");
        }
    }
}
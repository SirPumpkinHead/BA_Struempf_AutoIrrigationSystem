using System.Threading.Tasks;
using ContextBrokerLibrary.Api;
using Microsoft.Extensions.Configuration;

namespace WaterController.Services.Impl
{
    public class EntityService : IEntityService
    {
        private readonly IConfiguration _configuration;
        private readonly IEntitiesApi _entitiesApi;

        public EntityService(IConfiguration configuration, IEntitiesApi entitiesApi)
        {
            _configuration = configuration;
            _entitiesApi = entitiesApi;
        }

        public async Task<object> GetEntity(string entityId)
        {
            return await _entitiesApi.RetrieveEntityAsync(entityId);
        }
    }
}
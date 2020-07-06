using System.Threading.Tasks;

namespace WaterController.Services
{
    public interface IEntityService
    {
        Task<object> GetEntity(string entityId);
    }
}
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ITradingInstrumentsAvatarRepository
    {
        Task UpdateAsync(string id, string avatar, ImageTypes imageType);

        Task<ITradingInstrumentsAvatar> GetInstrumentsAvatarAsync(
            string id,
            ImageTypes imageType);
    }
}
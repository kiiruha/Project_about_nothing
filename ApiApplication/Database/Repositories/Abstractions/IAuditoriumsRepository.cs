using ApiApplication.Database.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IAuditoriumsRepository
    {
        #region Public methods

        Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel);

        #endregion
    }
}
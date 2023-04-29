using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        #region Private fields

        private readonly CinemaContext _context;

        #endregion

        #region Public constructors

        public AuditoriumsRepository(CinemaContext context)
        {
            _context = context;
        }

        #endregion

        #region Public methods

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }

        #endregion
    }
}
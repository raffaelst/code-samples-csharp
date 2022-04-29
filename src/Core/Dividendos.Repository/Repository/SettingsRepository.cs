using Dapper.Contrib.Extensions;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.GenericRepository;
using Dividendos.Repository.Interface.UoW;
using Dapper;
using System.Linq;

namespace Dividendos.Repository.Repository
{
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        private IUnitOfWork _unitOfWork;

        public SettingsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Settings UpdateAutoSync(Settings settings)
        {
            _unitOfWork.Connection.Update<Settings>(settings, _unitOfWork.Transaction);

            return settings;
        }

        public Settings GetByIdUser(string idUser)
        {
            string sql = $"SELECT * FROM Settings WHERE IdUser = @IdUser";

            var settings = _unitOfWork.Connection.Query<Settings>(sql, new { IdUser = idUser }, _unitOfWork.Transaction);

            return settings.FirstOrDefault();
        }
    }
}

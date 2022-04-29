using Dapper;
using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class CeiLogRepository : Repository<CeiLog>, ICeiLogRepository
    {
        private IUnitOfWork _unitOfWork;

        public CeiLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void RemoveAll()
        {
            string sql = $"DELETE FROM ceilog";
            _unitOfWork.Connection.Execute(sql, null, _unitOfWork.Transaction);

        }
    }
}

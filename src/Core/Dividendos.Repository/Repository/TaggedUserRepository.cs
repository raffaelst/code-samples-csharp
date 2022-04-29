using Dividendos.Entity.Entities;
using Dividendos.Repository.GenericRepository;
using Dividendos.Repository.Interface;
using Dividendos.Repository.Interface.UoW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Repository
{
    public class TaggedUserRepository : Repository<TaggedUser>, ITaggedUserRepository
    {
        private IUnitOfWork _unitOfWork;

        public TaggedUserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}

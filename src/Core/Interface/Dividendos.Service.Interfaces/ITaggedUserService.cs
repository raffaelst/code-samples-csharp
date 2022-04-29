using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Entity.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface ITaggedUserService : IBaseService
    {
        ResultServiceObject<TaggedUser> Add(TaggedUser taggedUser);
    }
}

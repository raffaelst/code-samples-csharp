using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IVideoTutorialService : IBaseService
    {
        ResultServiceObject<IEnumerable<VideoTutorial>> GetAll();
    }
}

using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using Dividendos.Repository.Interface.UoW;
using Dividendos.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dividendos.Service
{
    public class VideoTutorialService : BaseService, IVideoTutorialService
    {
        public VideoTutorialService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ResultServiceObject<IEnumerable<VideoTutorial>> GetAll()
        {
            ResultServiceObject<IEnumerable<VideoTutorial>> resultService = new ResultServiceObject<IEnumerable<VideoTutorial>>();

            IEnumerable<VideoTutorial> tutorials = _uow.VideoTutorialRepository.Select(item => item.Active == true);

            resultService.Value = tutorials;

            return resultService;
        }

    }
}

using AutoMapper;
using Dividendos.API.Model.Common;
using System.Collections.Generic;

namespace Dividendos.Application.Base
{
    public class BaseApp
    {
        protected PagedResult<TReturn> CreatePagedResults<T, TReturn>(List<T> lstPagedResults, int? pageIndex, int? pageSize, long totalAmountOfItens, IMapper mapper)
        {
            //Calculate Pagination
            int index = 0;
            int size = 0;
            long totalPageCount = 0;
            long totalNumberOfRecords = 0;
            CalculatePagination(pageIndex, pageSize, totalAmountOfItens, lstPagedResults.Count, out index, out size, out totalPageCount, out totalNumberOfRecords);

            //Mapper
            List<TReturn> mapperResultList = mapper.Map<List<T>, List<TReturn>>(lstPagedResults);

            return new PagedResult<TReturn>
            {
                Results = mapperResultList,
                PageNumber = index,
                PageSize = size,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords,
            };
        }

        private void CalculatePagination(int? pageIndex, int? pageSize, long totalAmountOfItens, int countList, out int index, out int size, out long totalPageCount, out long totalNumberOfRecords)
        {
            index = 0;
            size = 0;
            totalPageCount = 0;
            totalNumberOfRecords = totalAmountOfItens;

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                index = pageIndex.Value;
                size = pageSize.Value;
            }
            else
            {
                index = 1;
                size = countList;
                totalNumberOfRecords = countList;
            }

            var mod = 0.0;
            if (size > 0)
            {
                mod = totalNumberOfRecords % size;
                totalPageCount = (totalNumberOfRecords / size) + (mod == 0 ? 0 : 1);
            }
        }
    }
}

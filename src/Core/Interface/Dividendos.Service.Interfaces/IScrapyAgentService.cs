using Dividendos.Entity.Entities;
using Dividendos.Entity.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Service.Interface
{
    public interface IScrapyAgentService : IBaseService
    {
        ResultServiceObject<ScrapyAgent> Add(ScrapyAgent scrapyAgent);
        ResultServiceObject<ScrapyAgent> Update(ScrapyAgent scrapyAgent);
        ResultServiceObject<ScrapyAgent> GetByTrader(long idTrader);
        ResultServiceObject<IEnumerable<ScrapyAgent>> GetAll();
        ResultServiceObject<bool> Delete(ScrapyAgent scrapyAgent);
    }
}

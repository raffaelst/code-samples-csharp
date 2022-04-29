using System.Collections.Generic;
using System.Linq;
using Dividendos.Entity.Enum;
using FluentValidation.Results;
using K.UnitOfWorkBase;

namespace Dividendos.Entity.Model
{ 
    public class PushRedirect
    {
        public string ExternalRedirectURL { get; set; }

        public PushRedirectTypeEnum PushRedirectType { get; set; }

        public AppScreenNameEnum? AppScreenName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Repository.Context
{
    public static class SetupResolvers
    {
        public static void Setup()
        {
            K.Repository.KTableNameResolver.SetTableNameResolver();
            K.Repository.KKeyNameResolver.SetKeyPropertyResolver();
        }
    }
}

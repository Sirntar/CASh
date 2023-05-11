using System;
using System.Collections.Generic;

namespace CASh.Core.DBClient
{
    public interface ModelInterface
    {
        public static void Add(object obj) => throw new NotImplementedException();
        public bool Update(Dictionary<string, string> changes);
        public bool Delete();
    }
}

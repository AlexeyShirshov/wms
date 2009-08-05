using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Repository
{
    public interface IModificationTracker : IDisposable
    {
        void Add(params object[] entities);
        void Delete(params object[] entities);

        void AcceptModifications();
    }
}

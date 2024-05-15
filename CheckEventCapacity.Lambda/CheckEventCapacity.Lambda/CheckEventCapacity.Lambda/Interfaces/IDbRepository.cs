using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda.Interfaces
{
    public interface IEventRepository
    {
        Task<Event?> Get(string id);

    }
}

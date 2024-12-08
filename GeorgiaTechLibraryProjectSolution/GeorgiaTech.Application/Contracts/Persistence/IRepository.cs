using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Domain.Common;

namespace GeorgiaTech.Application.Contracts.Persistence
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task CreateAsync(T entity);
    }
}

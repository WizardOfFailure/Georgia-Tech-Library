using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeorgiaTech.Domain.Common;

namespace GeorgiaTech.Application.Contracts
{
    public interface IDispatcher
    {
        Task<Result<T>> Dispatch<T>(IQuery<T> query);
        Task<Result> Dispatch(ICommand command);
    }
}

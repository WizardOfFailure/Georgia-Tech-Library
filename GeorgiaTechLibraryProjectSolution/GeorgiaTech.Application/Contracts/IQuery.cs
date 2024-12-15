using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Domain.Common;

namespace GeorgiaTech.Application.Contracts
{
    public interface IQuery<T> : IRequest<Result<T>>
    {

    }
}

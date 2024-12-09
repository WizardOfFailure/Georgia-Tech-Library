using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeorgiaTech.Domain.Common;
using MediatR;

namespace GeorgiaTech.Application.Contracts
{
    public interface ICommand : IRequest<Result>
    {
    }
}

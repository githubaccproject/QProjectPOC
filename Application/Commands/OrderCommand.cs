using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UpdateOrderCommand : IRequest<int>
    {

        public int Id { get; set; }
        public UpdateOrderDto Order { get; set; }
    }

    public class CreateOrderCommand : IRequest<int>
    {
        public CreateOrderDto Order { get; set; }
    }

    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

}

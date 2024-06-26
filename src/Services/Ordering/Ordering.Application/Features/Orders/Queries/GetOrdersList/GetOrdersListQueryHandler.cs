﻿using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVM>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
   
    public async Task<List<OrdersVM>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetOrdersByUserName(request.UserName);
        return _mapper.Map<List<OrdersVM>>(orders);
    }
}

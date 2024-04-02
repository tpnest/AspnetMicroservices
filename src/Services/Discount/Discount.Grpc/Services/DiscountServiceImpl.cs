using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountServiceImpl : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly ILogger<DiscountServiceImpl> _logger;
    private readonly IMapper _mapper;

    public DiscountServiceImpl(IDiscountRepository repository, ILogger<DiscountServiceImpl> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName);
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with ProductName = {request.ProductName} not found"));
        }
        return _mapper.Map<CouponModel>(coupon);
    }
}
using Discount.Grpc.Protos;

namespace Basket.Api.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        this.discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        return await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
    }
}

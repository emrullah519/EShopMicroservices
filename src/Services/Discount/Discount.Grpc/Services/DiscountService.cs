using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext ctx, ILogger<DiscountService> log)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var discount = await ctx.Coupons.FirstOrDefaultAsync(f => f.ProductName == request.ProductName)
                ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            log.LogInformation(
                "Discount is retrived for product name:{productName}, amount:{amount}, description:{description}",
                discount.ProductName, discount.Amount, discount.Description);
            var response = discount.Adapt<CouponModel>();
            return response;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            await ctx.AddAsync(coupon);
            await ctx.SaveChangesAsync();
            log.LogInformation("Discount created {productName} {discount} {description}", coupon.ProductName, coupon.Amount, coupon.Description);
            var couponModal = coupon.Adapt<CouponModel>();
            return couponModal;

        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null) throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            ctx.Update(coupon);
            await ctx.SaveChangesAsync();
            log.LogInformation("Discount updated {productName} {discount} {description}", coupon.ProductName, coupon.Amount, coupon.Description);
            var couponModal = coupon.Adapt<CouponModel>();
            return couponModal;

        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await ctx.Coupons.FirstOrDefaultAsync(f => f.ProductName == request.Productname)
                ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name = {request.Productname} not found"));
            ctx.Coupons.Remove(coupon);
            await ctx.SaveChangesAsync();
            log.LogInformation("Discount is successfuly deleted. Product name : {productName}", coupon.ProductName);
            return new DeleteDiscountResponse { Success = true };
        }
    }
}

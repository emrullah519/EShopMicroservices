﻿
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext ctx)
        : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;
            var totalCount = await ctx.Orders.LongCountAsync(cancellationToken);
            var orders = await ctx.Orders
                .Include(i => i.OrderItems)
                .OrderBy(i => i.OrderName.Value)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orders.ToOrderDtoList()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecuringWebApiUsingJwtAuthentication.Interfaces;

namespace SecuringWebApiUsingJwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet()]
        [Authorize(Policy = "OnlyNonBlockedCustomer")]
        public async Task<IActionResult> Get()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if (claim == null)
            {
                return Unauthorized("Invalid customer");
            }

            var orders = await orderService.GetOrdersByCustomerId(int.Parse(claim.Value));

            if (orders == null || !orders.Any())
            {
                return BadRequest($"No order was found");
            }

            return Ok(orders);
        }
    }
}

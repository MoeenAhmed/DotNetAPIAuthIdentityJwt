using DotNetIdentityAPI.Attributes;
using DotNetIdentityAPI.Models;
using DotNetIdentityShared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private ApplicationDbContext _context;
        public OrderProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize("ValidClient1AudiencePolicy")]
        [Authorize("CreateOrderPolicy")]
        //[Authorize("CustomCreateOrderPolicy")]
        [HttpPost("CreateOrders")]
        public async Task<IActionResult> CreateOrders()
        {
            return Ok("order created successfully");
        }

        // [RequireClaimAttribute(ClaimTypes.Role, "ProductCreator")]
        [Authorize("ValidClient2AudiencePolicy")]
        [Authorize(policy: "CreateProductPolicy")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            return Ok("product create successfully");
        }
    }
}

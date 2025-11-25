using API.Data;
using API.Modeles;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly INotificationService _notificationService;

        public OrdersController(StoreContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            // Calculate totals (server-side validation)
            decimal total = 0;
            foreach (var item in order.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    item.UnitPrice = product.Price;
                    total += item.Quantity * item.UnitPrice;
                }
            }

            // Get shipping cost
            var rate = await _context.ShippingRates.FirstOrDefaultAsync(r => r.BaladiyaId == order.BaladiyaId);
            
            if (rate != null)
            {
                order.ShippingCost = order.DeliveryType == "Desk" ? rate.DeskPrice : rate.HomePrice;
            }
            else
            {
                order.ShippingCost = 0;
            }

            order.TotalAmount = total + order.ShippingCost;
            order.OrderDate = DateTime.UtcNow;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Send Notification (Telegram)
            var message = $"📦 طلب جديد #{order.Id}\n👤 الاسم: {order.CustomerName}\n💰 المجموع: {order.TotalAmount} دج";
            await _notificationService.SendMessageAsync(message);

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}

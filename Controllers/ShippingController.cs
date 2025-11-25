using API.Data;
using API.Modeles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly StoreContext _context;

        public ShippingController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("wilayas")]
        public async Task<ActionResult<IEnumerable<Wilaya>>> GetWilayas()
        {
            return await _context.Wilayas.ToListAsync();
        }

        [HttpGet("baladiyas/{wilayaId}")]
        public async Task<ActionResult<IEnumerable<Baladiya>>> GetBaladiyas(int wilayaId)
        {
            return await _context.Baladiyas.Where(b => b.WilayaId == wilayaId).ToListAsync();
        }

        [HttpGet("rate/{baladiyaId}")]
        public async Task<ActionResult<ShippingRate>> GetRate(int baladiyaId)
        {
            var rate = await _context.ShippingRates.FirstOrDefaultAsync(r => r.BaladiyaId == baladiyaId);
            if (rate == null)
            {
                return NotFound();
            }
            return rate;
        }

        [HttpPost("wilayas")]
        public async Task<ActionResult<Wilaya>> PostWilaya(Wilaya wilaya)
        {
            _context.Wilayas.Add(wilaya);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWilayas), new { id = wilaya.Id }, wilaya);
        }

        [HttpPost("baladiyas")]
        public async Task<ActionResult<Baladiya>> PostBaladiya(Baladiya baladiya)
        {
            _context.Baladiyas.Add(baladiya);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBaladiyas), new { wilayaId = baladiya.WilayaId }, baladiya);
        }

        [HttpPost("rates")]
        public async Task<ActionResult<ShippingRate>> PostRate(ShippingRate rate)
        {
            var existingRate = await _context.ShippingRates.FirstOrDefaultAsync(r => r.BaladiyaId == rate.BaladiyaId);
            if (existingRate != null)
            {
                existingRate.HomePrice = rate.HomePrice;
                existingRate.DeskPrice = rate.DeskPrice;
            }
            else
            {
                _context.ShippingRates.Add(rate);
            }
            await _context.SaveChangesAsync();
            return Ok(rate);
        }

        // PUT: api/Shipping/wilayas/5
        [HttpPut("wilayas/{id}")]
        public async Task<IActionResult> PutWilaya(int id, Wilaya wilaya)
        {
            if (id != wilaya.Id) return BadRequest();
            _context.Entry(wilaya).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Shipping/wilayas/5
        [HttpDelete("wilayas/{id}")]
        public async Task<IActionResult> DeleteWilaya(int id)
        {
            var wilaya = await _context.Wilayas.FindAsync(id);
            if (wilaya == null) return NotFound();
            _context.Wilayas.Remove(wilaya);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/Shipping/baladiyas/5
        [HttpPut("baladiyas/{id}")]
        public async Task<IActionResult> PutBaladiya(int id, Baladiya baladiya)
        {
            if (id != baladiya.Id) return BadRequest();
            _context.Entry(baladiya).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Shipping/baladiyas/5
        [HttpDelete("baladiyas/{id}")]
        public async Task<IActionResult> DeleteBaladiya(int id)
        {
            var baladiya = await _context.Baladiyas.FindAsync(id);
            if (baladiya == null) return NotFound();
            _context.Baladiyas.Remove(baladiya);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

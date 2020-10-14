using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.Models;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OddsBoostsController : ControllerBase
    {
        private readonly Context context;

        public OddsBoostsController(Context context)
        {
            this.context = context;
        }

        // GET: api/OddsBoosts
        [HttpGet]
        public ActionResult<IEnumerable<OddsBoost>> GetOddsBoost()
        {
            return context.OddsBoostRepository.Read().Where(o => o.IsAvailable).ToList();
        }
    }
}

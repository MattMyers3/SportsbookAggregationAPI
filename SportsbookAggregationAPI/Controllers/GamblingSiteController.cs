using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamblingSiteController : ControllerBase
    {
        private readonly Context context;

        public GamblingSiteController(Context context)
        {
            this.context = context;
        }

        public ActionResult<IEnumerable<GamblingSite>> GetSites()
        {
            return context.GamblingSiteRepository.Read().ToList();
        }

    }
}

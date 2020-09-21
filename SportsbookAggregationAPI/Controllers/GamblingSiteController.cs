using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.Models;

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

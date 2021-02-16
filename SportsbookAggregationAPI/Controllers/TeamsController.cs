using Microsoft.AspNetCore.Mvc;
using SportsbookAggregationAPI.Data;
using SportsbookAggregationAPI.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsbookAggregationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly Context context;

        public TeamsController(Context context)
        {
            this.context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public ActionResult<IEnumerable<Team>> GetTeams()
        {
            return context.TeamRepository.Read().ToList();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public ActionResult<Team> GetTeam(Guid id)
        {
            var team = context.TeamRepository.Read().FirstOrDefault(r => r.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            return team;
        }
    }
}
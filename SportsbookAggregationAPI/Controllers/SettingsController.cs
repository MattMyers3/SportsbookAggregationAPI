using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class SettingsController : ControllerBase
    {
        private readonly Context context;

        public SettingsController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<GamblingSite>> Get()
        {
            var userId = HttpContext.User.Claims.Single(c => c.Type == "uid").Value;
            var userSettings = context.UserSettingsRepository.Read().SingleOrDefault(u => u.UserId == userId);
            var gamblingSites = context.GamblingSiteRepository.Read().ToList();

            if (userSettings != null)
                return gamblingSites.Where(g => userSettings.DefaultBooks.Contains(g.Name)).ToList();

            return gamblingSites;
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put(string[] gamblingsites)
        {
            var userId = HttpContext.User.Claims.Single(c => c.Type == "uid").Value;
            var selectedGamblingSites = context.GamblingSiteRepository.Read().Where(g => gamblingsites.Contains(g.Name));
            var userSettings = context.UserSettingsRepository.Read().SingleOrDefault(u => u.UserId == userId);

            if (userSettings != null)
            {
                userSettings.DefaultBooks = selectedGamblingSites.Select(g => g.Name).ToArray();
                context.UserSettingsRepository.Update(userSettings);
            }
            else
            {
                userSettings = new UserSettings() { UserId = userId, DefaultBooks = selectedGamblingSites.Select(g => g.Name).ToArray() };
                context.UserSettingsRepository.Create(userSettings);
            }

            return NoContent();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContestProject.Services;

namespace ContestProject.Controllers
{
    [ApiController]
    [Route("api/leaderboard")]
    public class LeaderboardController : Controller
    {
        IDataService dataService;
        public LeaderboardController(IDataService service)
        {
            dataService = service;
        }
        [HttpGet]
        public IEnumerable<UserTaskGroup> Get()
        {
            return dataService.GetTop3UserTasks();
        }
    }
}
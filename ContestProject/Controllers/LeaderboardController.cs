using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContestProject.Models;
using System;
using Microsoft.EntityFrameworkCore;
using ContestProject.Services;

namespace ContestProject.Controllers
{
    [ApiController]
    [Route("api/leaderboard")]
    public class LeaderboardController : Controller
    {
        DataService dataService;
        public LeaderboardController(DataService service)
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
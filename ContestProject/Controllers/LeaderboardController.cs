using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContestProject.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace ContestProject.Controllers
{
    [ApiController]
    [Route("api/leaderboard")]
    public class LeaderboardController : Controller
    {
        ApplicationContext db;
        const int numberOfTOP = 3;
        public LeaderboardController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public IEnumerable<UserTaskGroup> Get()
        {
            var users = db.Users
                    .Include(u => u.UserTasks)  
                        .ThenInclude(ut => ut.Task);
            //TODO: Store the amount of successful tasks in the DB and make here an explicit load through .Where() only for top3
            var top3UserTasks = users.Select(u => new UserTaskGroup(
                                                            u.Name,
                                                            u.UserTasks.Count(),
                                                            u.UserTasks.Select(ut => ut.Task.Name)
                                                            ))
                                      .ToList()
                                      .OrderByDescending(utg => utg.SolutionsNumber)
                                      .Take(numberOfTOP);
            return top3UserTasks;
        }
    }
}
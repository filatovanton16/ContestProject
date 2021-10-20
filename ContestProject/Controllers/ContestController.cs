using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContestProject.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContestProject.Controllers
{
    [ApiController]
    [Route("api/contest")]
    public class ContestController : Controller
    {
        ApplicationContext db;
        public ContestController(ApplicationContext context)
        {
            db = context;
            FillDatabase();
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            IEnumerable<string> allTasks = db.ContestTasks.Select(x => x.Name);
            return allTasks.ToList();
        }

        [HttpGet("{taskName}")]
        public ContestTask Get(string taskName)
        {
            ContestTask contestTask = db.ContestTasks.FirstOrDefault(contestTask => contestTask.Name == taskName);
            return contestTask;
        }

        [HttpPost]
        public async Task<bool> Post(UserTaskCode userTaskCode)
        {
            bool isSuccess = await JDoodleConnector.TryCompilate(userTaskCode);
            if (isSuccess)
            {

                ContestTask contestTask = db.ContestTasks.FirstOrDefault(contestTask => contestTask.Name == userTaskCode.Task);

                User user = db.Users.FirstOrDefault(user => user.Name == userTaskCode.Name);
                if (user == null)
                {
                    user = new User { Name = userTaskCode.Name };
                    db.Users.Add(user);
                }

                UserTask userTask = db.UserTasks.FirstOrDefault(userTask => userTask.User.Name == userTaskCode.Name && userTask.Task.Name == userTaskCode.Task);
                if (userTask == null)
                {
                    db.UserTasks.Add(new UserTask { User = user, Task = contestTask });
                }

                db.SaveChanges();
            }

            return isSuccess;
        }

        //[HttpPost]
        //public string Post(string dgf)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return "ok";
        //    }
        //    return "neok";
        //}

        private void FillDatabase() //If the database empty. If not empty => this code could be deleted
        {
            ContestTask[] initialContestTasks = new ContestTask[4]
            {
                new ContestTask { Name = "TheSimpliestTask", Description = "You need to just return 1", InputParameter = 0, OutputParameter = 1 },
                new ContestTask { Name = "VerySimpleTask", Description = "You need to return double input value", InputParameter = 2, OutputParameter = 4 },
                new ContestTask { Name = "SimpleTask", Description = "You need to return the factorial of the input number", InputParameter = 5, OutputParameter = 120 },
                new ContestTask { Name = "NormalTask", Description = "You need to get a Fibonaccisequence number by the input index", InputParameter = 5, OutputParameter = 5 }
            };

            User[] initialUsers = new User[4]
            {
                new User { Name = "Anna" },
                new User { Name = "Bartosz" },
                new User { Name = "Adam" },
                new User { Name = "Simon" }
            };

            if (!db.Users.Any()) db.Users.AddRange(initialUsers);

            if (!db.ContestTasks.Any()) db.ContestTasks.AddRange(initialContestTasks);

            if (!db.UserTasks.Any())
            {
                db.UserTasks.Add(new UserTask { User = initialUsers[0], Task = initialContestTasks[3] });
                db.UserTasks.Add(new UserTask { User = initialUsers[0], Task = initialContestTasks[2] });
                db.UserTasks.Add(new UserTask { User = initialUsers[0], Task = initialContestTasks[1] });
                db.UserTasks.Add(new UserTask { User = initialUsers[0], Task = initialContestTasks[0] });
                db.UserTasks.Add(new UserTask { User = initialUsers[1], Task = initialContestTasks[3] });
                db.UserTasks.Add(new UserTask { User = initialUsers[1], Task = initialContestTasks[2] });
                db.UserTasks.Add(new UserTask { User = initialUsers[1], Task = initialContestTasks[1] });
                db.UserTasks.Add(new UserTask { User = initialUsers[2], Task = initialContestTasks[3] });
                db.UserTasks.Add(new UserTask { User = initialUsers[2], Task = initialContestTasks[2] });
                db.UserTasks.Add(new UserTask { User = initialUsers[3], Task = initialContestTasks[3] });
            }

            db.SaveChanges();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ContestProject.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace ContestProject.Controllers
{
    [ApiController]
    [Route("api/contest")]
    public class ContestController : Controller
    {
        private ApplicationContext db;

        private IJDoodleService _JDoodleService;

        public ContestController(ApplicationContext context, IJDoodleService jDoodleService)
        {
            db = context;
            _JDoodleService = jDoodleService;
            FillDatabase();
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            IEnumerable<string> allTasks = db.ContestTasks.Select(x => x.Name);
            return allTasks.ToList();
        }

        [HttpGet("{taskName}")]
        public string Get(string taskName)
        {
            ContestTask contestTask = db.ContestTasks.FirstOrDefault(contestTask => contestTask.Name == taskName);
            return JsonConvert.SerializeObject(contestTask.Description);
        }

        [HttpPost]
        public async Task<string> Post(UserTaskCode userTaskCode)
        {
            string result;
            try
            {
                ContestTask contestTask = db.ContestTasks.FirstOrDefault(contestTask => contestTask.Name == userTaskCode.TaskName);
                dynamic response = await _JDoodleService.TryCompilate(userTaskCode.Code, contestTask.InputParameter);

                if (response.statusCode == "200")
                {
                    SaveUserTaskToDB(userTaskCode, contestTask);
                    try
                    {
                        result = Convert.ToInt32(response.output) == contestTask.OutputParameter ?
                                                "Success! Memory: " + response.memory + " CPU: " + response.cpuTime :
                                                "Wrong:(";
                    }
                    catch
                    {
                        result = "Error: " + response.output;
                    }
                }
                else
                {
                    result = "Error: " + response.error;
                }
            }
            catch
            {
                result = "Something went wrong. Please try again.";
            }
            return JsonConvert.SerializeObject(result);
        }

        private void FillDatabase() //In case if the database empty, instead of migration.
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

        private void SaveUserTaskToDB(UserTaskCode userTaskCode, ContestTask contestTask)
        {
            User user = db.Users.FirstOrDefault(user => user.Name == userTaskCode.UserName);
            if (user == null)
            {
                user = new User { Name = userTaskCode.UserName };
                db.Users.Add(user);
            }

            UserTask userTask = db.UserTasks.FirstOrDefault(userTask => userTask.User.Name == userTaskCode.UserName && userTask.Task.Name == userTaskCode.TaskName);
            if (userTask == null)
            {
                db.UserTasks.Add(new UserTask { User = user, Task = contestTask });
            }

            db.SaveChanges();
        }

    }
}
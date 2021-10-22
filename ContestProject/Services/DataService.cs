using ContestProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContestProject.Services
{
    public class DataService
    {
        const int numberOfTOP = 3;
        private ApplicationContext db;
        private IMemoryCache cache;

        public DataService(ApplicationContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
        }

        public void Initialize()
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

        public IEnumerable<UserTaskGroup> GetTop3UserTasks()
        {
            var users = db.Users
                    .Include(u => u.UserTasks)
                        .ThenInclude(ut => ut.Task);
            //TODO: Store the amount of successful tasks in the DB and make here an explicit load through .Where() only for top3
            return users.Select(u => new UserTaskGroup(
                                                   u.Name,
                                                   u.UserTasks.Count(),
                                                   u.UserTasks.Select(ut => ut.Task.Name)
                                                   ))
                                      .ToList()
                                      .OrderByDescending(utg => utg.SolutionsNumber)
                                      .Take(numberOfTOP);
        }

        public void SaveUserTask(UserTaskCode userTaskCode, ContestTask contestTask)
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

        public ContestTask GetContestTask(string taskName)
        {
            return db.ContestTasks.FirstOrDefault(contestTask => contestTask.Name == taskName);
        }

        public IEnumerable<string> GetAllTasks()
        {
            return db.ContestTasks.Select(x => x.Name);
        }
    }
}

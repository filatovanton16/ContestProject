using ContestProject.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

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
            ContestTask[] initialContestTasks = new ContestTask[4];
            User[] initialUsers = new User[4];

            if (!db.Users.Any())
            {
                initialUsers[0] = new User { Name = "Simon", NumberOfSolved = 1 };
                initialUsers[1] = new User { Name = "Bartosz", NumberOfSolved = 2 };
                initialUsers[2] = new User { Name = "Adam", NumberOfSolved = 3 };
                initialUsers[3] = new User { Name = "Anna", NumberOfSolved = 4 };
                db.Users.AddRange(initialUsers);
            }

            if (!db.ContestTasks.Any())
            {
                initialContestTasks[0] = new ContestTask { Name = "TheSimpliestTask", Description = "You need to just return 1", InputParameter = 0, OutputParameter = 1 };
                initialContestTasks[1] = new ContestTask { Name = "VerySimpleTask", Description = "You need to return double input value", InputParameter = 2, OutputParameter = 4 };
                initialContestTasks[2] = new ContestTask { Name = "SimpleTask", Description = "You need to return the factorial of the input number", InputParameter = 5, OutputParameter = 120 };
                initialContestTasks[3] = new ContestTask { Name = "NormalTask", Description = "You need to get a Fibonaccisequence number by the input index", InputParameter = 5, OutputParameter = 5 };
                db.ContestTasks.AddRange(initialContestTasks);
            }

            if (!db.UserTasks.Any())
            {
                for(int i = 0; i <= 3; i++)
                {
                    for(int j = 0; j <= i; j++)
                    {
                        db.UserTasks.Add(new UserTask { User = initialUsers[i], Task = initialContestTasks[j] });
                    }
                }
            }

            db.SaveChanges();
        }

        public IEnumerable<UserTaskGroup> GetTop3UserTasks()
        {
            var users = db.Users.OrderByDescending(u => u.NumberOfSolved)
                                .Take(numberOfTOP);

            return users.Select(u =>
                        new UserTaskGroup(u.Name, u.NumberOfSolved, u.UserTasks.Select(ut => ut.Task.Name)));
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
                user.NumberOfSolved++;
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

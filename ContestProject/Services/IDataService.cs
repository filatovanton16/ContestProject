using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IDataService
{
    public IEnumerable<string> GetAllTasks();
    public void Initialize();
    public IEnumerable<UserTaskGroup> GetTop3UserTasks();
    public void SaveUserTask(UserTaskCode userTaskCode, ContestTask contestTask);
    public ContestTask GetContestTask(string taskName);
}

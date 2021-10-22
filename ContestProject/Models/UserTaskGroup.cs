using System.Collections.Generic;

public class UserTaskGroup
{
    public string Name { get; set; }
    public int SolutionsNumber { get; set; }
    public IEnumerable<string> ContestTasks { get; set; }

    public UserTaskGroup(string name, int solutionsNumber, IEnumerable<string> allTasksString)
    {
        Name = name;
        SolutionsNumber = solutionsNumber;
        ContestTasks = allTasksString;
    }
}
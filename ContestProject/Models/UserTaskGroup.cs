using System.Collections.Generic;
using System.Linq;

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

    public override bool Equals(object obj)
    {
        var secondVar = obj as UserTaskGroup;
        return secondVar.Name == this.Name
        && secondVar.SolutionsNumber == this.SolutionsNumber
        && secondVar.ContestTasks.SequenceEqual(secondVar.ContestTasks);
    }
}
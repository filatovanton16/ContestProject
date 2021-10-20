public class UserTaskGroup
{
    public string Name { get; set; }
    public int SolutionsNumber { get; set; }
    public string AllTasksString { get; set; }

    public UserTaskGroup(string name, int solutionsNumber, string allTasksString)
    {
        Name = name;
        SolutionsNumber = solutionsNumber;
        AllTasksString = allTasksString;
    }
}
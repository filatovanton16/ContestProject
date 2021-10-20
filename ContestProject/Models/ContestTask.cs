using System.Collections.Generic;

public class ContestTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int InputParameter { get; set; }
    public int OutputParameter { get; set; }
    public List<UserTask> UserTasks { get; set; }

    public ContestTask()
    {
        UserTasks = new List<UserTask>();
    }
}
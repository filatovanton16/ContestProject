using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSolved { get; set; }
    public List<UserTask> UserTasks { get; set; }

    public User()
    {
        UserTasks = new List<UserTask>();
        NumberOfSolved = 0;
    }
}
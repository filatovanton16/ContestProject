using System.Threading.Tasks;

public interface IJDoodleService
{
    Task<dynamic> TryCompilateAsync(string code, int input);
}

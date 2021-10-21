using System.Threading.Tasks;

public interface IJDoodleService
{
    Task<dynamic> TryCompilate(string code, int input);
}

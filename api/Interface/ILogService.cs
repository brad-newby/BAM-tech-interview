namespace StargateAPI.Interface
{
    public interface ILogService
    {
        Task<int> Log(string message);
    }
}

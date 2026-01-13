namespace AuthenticationModule.Services
{
    public interface IHistory
    {
         Task SaveLoginHistory(Guid? userId, bool isSuccess);
    }
}

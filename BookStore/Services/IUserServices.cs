namespace BookStore.Services
{
    public interface IUserServices
    {
        string GetUserId();
        bool IsAuthencated();
    }
}
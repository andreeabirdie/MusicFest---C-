namespace MusicFest.repository
{
    public interface IUserRepository
    {
        bool FindOne(string username, string password);
    }
}
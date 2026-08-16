
namespace myshop.Web.Extensions;

public static class SessionExtensions
{
    extension(ISession session)
    {
        public void SetObject<T>(string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public T? GetObject<T>(string key)
        {
            var value = session.GetString(key);
            return value is null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}

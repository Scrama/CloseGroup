namespace CloseGroup
{
    public interface ISettings
    {
        T Get<T>(string key);
    }
}

namespace HashTables_Lab4;

public interface IHashTable
{
    int Size { get; }
    void Add(string key, object value);
    object Get(string key);
    void Delete(string key);
}
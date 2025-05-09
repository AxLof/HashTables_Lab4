using System.Security.Cryptography.X509Certificates;

namespace HashTables_Lab4;

public class HashTableChaining : IHashTable
{
    public int Size { get; private set; }
    private int OccupiedBuckets { get; set; }
    private LinkedList<KeyValuePair<string, object>>[] _buckets;
    public Func<string, int, int> HashingAlgorithm { get; set; }
    private bool IsResizing { get; set; }
    
    // Har delfault size på 10.
    public HashTableChaining(int size = 10)
    {
        // Antalet buckets
        this.Size = size;
        // Array bestående av listor/buckets
        this._buckets = new LinkedList<KeyValuePair<string, object>>[size];
        
        // Skapar en ny lista i varje bucket
        for(int i = 0; i < size; i++)
        {
            _buckets[i] = new LinkedList<KeyValuePair<string, object>>();
        }

        // Sätter default hashing algorithm
        this.HashingAlgorithm = HashingAlgoritms.GetASCIIHash;
        this.IsResizing = false; 
    }
    
    public void Add(string key, object value)
    {
        int hash = HashingAlgorithm(key, Size);
        _buckets[hash].AddLast(new KeyValuePair<string, object>(key, value));
        Console.WriteLine($"{key} has been added to the table!");
        if (!(_buckets[hash].Count > 1))
        {
            // Öka endast om bucketen verkligen var tom
            OccupiedBuckets++;
        }
        if (!IsResizing) Resize();
    }

    public object Get(string key)
    {
        int hash = HashingAlgorithm(key, Size);

        // Ittererar alla key-value pairs i bucketen. 
        foreach (var kvp in _buckets[hash])
        {
            if (kvp.Key.Equals(key))
            {
                return kvp.Value;
            }
        }

        // Slänger ett error om ingen nyckel matchar
        throw new KeyNotFoundException("No key matching the specified argument");
    }

    public void Delete(string key)
    {
        int hash = HashingAlgorithm(key, Size);

        var bucket = _buckets[hash];
        
        /* Sparar kvp som ska tas bort och sätter flaggan foundKvp till true om rätt kvp hittats.
           detta görs då man inte kan ta bort värden i en lista man ittererar över.*/
        KeyValuePair<string, object> objectToRemove = default;
        bool foundKvp = false;
        
        foreach (var kvp in bucket)
        {
            if (kvp.Key.Equals(key))
            {
                objectToRemove = kvp;
                foundKvp = true;
            }
        }

        if (!foundKvp)
        {
            // Slänger ett error om ingen nyckel matchar
            throw new KeyNotFoundException("No key matching the given argument");
        }
        
        bucket.Remove(objectToRemove);
        OccupiedBuckets--;
        Console.WriteLine($"{key} has been removed from the table!");
        Resize();
    }

    // Kör efter insert och delete
    private void Resize()
    {
        if (((float)OccupiedBuckets / Size) > 0.75)
        {
            // Dubblar storleken
            LinkedList<KeyValuePair<string, object>>[] oldBuckets = _buckets;
            Size *= 2;
            _buckets = new LinkedList<KeyValuePair<string, object>>[Size];
            
            for(int i = 0; i < Size; i++)
            {
                _buckets[i] = new LinkedList<KeyValuePair<string, object>>();
            }

            OccupiedBuckets = 0;

            IsResizing = true;
            // Nästlad for loop, dålig tidskomplexitet
            foreach (var bucket in oldBuckets)
            {
                foreach (var kvp in bucket)
                {
                    Add(kvp.Key, kvp.Value);
                }
            }

            IsResizing = false;

        }
        return;
    }
}
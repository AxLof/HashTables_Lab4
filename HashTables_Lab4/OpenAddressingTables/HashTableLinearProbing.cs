namespace HashTables_Lab4;

public class HashTableLinearProbing : IHashTable
{
    public int Size { get; private set; }
    private int OccupiedBuckets { get; set; }
    private KeyValuePair<string?, object?>[] _buckets;
    public Func<string, int, int> HashingAlgorithm { get; set; }
    private bool IsResizing { get; set; }
    
    // Har delfault size på 10.
    public HashTableLinearProbing(Func<string,int,int> hashingAlgorithm, int size = 20)
    {
        // Antalet buckets
        this.Size = size;
        // Array bestående av listor/buckets
        this._buckets = new KeyValuePair<string?, object?>[size];

        // Sätter default hashing algorithm
        this.HashingAlgorithm = hashingAlgorithm;
        this.IsResizing = false;
    }
    
    // Skickar hash funktion som parameter.
    public void Add(string key, object value)
    {
        int hash = HashingAlgorithm(key, Size);

        while (hash < Size)
        {
            if (_buckets[hash].Key == null)
            {
                _buckets[hash] = new KeyValuePair<string?, object?>(key, value);
                Console.WriteLine($"{key} has been added to the table!");
                OccupiedBuckets++;
                if(!IsResizing) Resize();
                return;
            }

            hash++;
        }
    }

    public object Get(string key)
    {
        int hash = HashingAlgorithm(key, Size);

        while (hash < Size)
        {
            if(hash > Size-1)
            {
                // Slänger ett error om programmet gått igenom alla nycklar
                throw new KeyNotFoundException("No key matching the specified argument");
            }
            
            var currentBucket = _buckets[hash];
            
            if (currentBucket.Key == key)
            {
                return currentBucket.Value;
            }

            hash++;
        }
        // Slänger ett error om programmet gått igenom alla nycklar
        throw new KeyNotFoundException("No key matching the specified argument");
    }

    public void Delete(string key)
    {
        int hash = HashingAlgorithm(key, Size);
        
        while (true)
        {
            var currentBucket = _buckets[hash];
            if(hash > Size-1)
            {
                // Slänger ett error om programmet gått igenom alla nycklar
                throw new KeyNotFoundException("No key matching the specified argument");
            }
            else if (currentBucket.Key == key)
            {
                // Kunde inte sätta key och value till null manuellt så man får skapa ett nytt null par. 
                _buckets[hash] = new KeyValuePair<string?, object?>(null, null);
                OccupiedBuckets--;
                return;
            }

            hash++;
        }
    }

    // Kör efter insert och delete
    private void Resize()
    {
        if (((float)OccupiedBuckets / Size) > 0.75)
        {
            // Dubblar storleken
            KeyValuePair<string?, object?>[] oldBuckets = _buckets;
            Size *= 2;
            _buckets = new KeyValuePair<string?, object?>[Size];

            OccupiedBuckets = 0;
            IsResizing = true;
            foreach (var kvp in oldBuckets)
            {
                if (kvp.Key != null)
                {
                    Add(kvp.Key, kvp.Value);
                }
            }

            IsResizing = false;

        }
        return;
    }
}
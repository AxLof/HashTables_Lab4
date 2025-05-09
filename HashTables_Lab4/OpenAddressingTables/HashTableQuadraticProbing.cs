namespace HashTables_Lab4;

public class HashTableQuadraticProbing : IHashTable
{
     public int Size { get; private set; }
    private int OccupiedBuckets { get; set; }
    private KeyValuePair<string?, object?>[] _buckets;
    public Func<string, int, int> HashingAlgorithm { get; set; }
    private bool IsResizing { get; set; }
    
    // Har delfault size på 10.
    public HashTableQuadraticProbing(Func<string,int,int> hashingAlgorithm, int size = 20)
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
        int initialHash = HashingAlgorithm(key, Size);
        int probedHash = initialHash;
        int probe = 1;
        
        // Stannar när den loopat "ett varv". 
        while (probedHash >= initialHash)
        {
            if (_buckets[probedHash].Key == null)
            {
                _buckets[probedHash] = new KeyValuePair<string?, object?>(key, value);
                Console.WriteLine($"{key} has been added to the table!");
                OccupiedBuckets++;
                if(!IsResizing) Resize();
                return;
            }

            probedHash = (probedHash + (probe * probe)) % Size;
            probe++;
        }
        // Slänger ett error om programmet gått igenom alla nycklar
        throw new KeyNotFoundException("No key matching the specified argument");
    }

    public object Get(string key)
    {
        int initialHash = HashingAlgorithm(key, Size);
        int probedHash = initialHash;
        int probe = 1;
        
        while (probedHash >= initialHash)
        {
            if(probedHash > Size-1)
            {
                // Slänger ett error om programmet gått igenom alla nycklar
                throw new KeyNotFoundException("No key matching the specified argument");
            }
            
            var currentBucket = _buckets[probedHash];
            
            if (currentBucket.Key == key)
            {
                return currentBucket.Value;
            }

            probedHash = (probedHash + (probe * probe)) % Size;
            probe++;
        }
        // Slänger ett error om programmet gått igenom alla nycklar
        throw new KeyNotFoundException("No key matching the specified argument");
    }

    public void Delete(string key)
    {
        int initialHash = HashingAlgorithm(key, Size);
        int probedHash = initialHash;
        int probe = 1;
        
        while (probedHash >= initialHash)
        {
            if(probedHash > Size-1)
            {
                // Slänger ett error om programmet gått igenom alla nycklar
                throw new KeyNotFoundException("No key matching the specified argument");
            }
            
            var currentBucket = _buckets[probedHash];
            
            if (currentBucket.Key == key)
            {
                // Kunde inte sätta key och value till null manuellt så man får skapa ett nytt null par. 
                _buckets[probedHash] = new KeyValuePair<string?, object?>(null, null);
                OccupiedBuckets--;
                return;
            }
            
            probedHash = (probedHash + (probe * probe)) % Size;
            probe++;
        }
        // Slänger ett error om programmet gått igenom alla nycklar
        throw new KeyNotFoundException("No key matching the specified argument");
    }

    // Kör efter insert och delete
    private void Resize()
    {
        if (((float)OccupiedBuckets / Size) > 0.75)
        {
            // Ökar storlek med 1.5
            KeyValuePair<string?, object?>[] oldBuckets = _buckets;
            Size = (int)Math.Round(Size * 1.5);
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
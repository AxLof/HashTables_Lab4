using System.Security.Cryptography.X509Certificates;

namespace HashTables_Lab4;

public class HashTableChainingStack : IHashTable
{
    public int Size { get; private set; }
    private int OccupiedBuckets { get; set; }
    private Stack<KeyValuePair<string, object>>[] _buckets;
    public Func<string, int, int> HashingAlgorithm { get; set; }
    private bool IsResizing { get; set; }
    
    // Har delfault size på 10.
    public HashTableChainingStack(int size = 10)
    {
        // Antalet buckets
        this.Size = size;
        // Array bestående av listor/buckets
        this._buckets = new Stack<KeyValuePair<string, object>>[size];
        
        // Skapar en ny lista i varje bucket
        for(int i = 0; i < size; i++)
        {
            _buckets[i] = new Stack<KeyValuePair<string, object>>();
        }

        // Sätter default hashing algorithm
        this.HashingAlgorithm = HashingAlgoritms.GetASCIIHash;
        this.IsResizing = false;
    }
    
    public void Add(string key, object value)
    {
        int hash = HashingAlgorithm(key, Size);
        _buckets[hash].Push(new KeyValuePair<string, object>(key, value));
        Console.WriteLine($"{key} has been added to the table!");
        if (!(_buckets[hash].Count > 1))
        {
            // Öka endast om bucketen verkligen var tom
            OccupiedBuckets++;
        }

        if (!IsResizing)
        {
            Resize();
        }
    }

    public object Get(string key)
    {
        int hash = HashingAlgorithm(key, Size);
        var bucket = _buckets[hash];

        // Måste kolla så att bucket är större än 0 annars krashar det
        if (bucket.Count > 0)
        {
            // Kollar om rätt element ligger överst i stacken, siåfall är det O(1)
            if (bucket.Peek().Key == key)
            {
                return bucket.Peek().Value;
            }
        }
        
        var stackArray = bucket.ToArray();
        
        // Annars är det O(N) om man måste itterera över hela stacken (som kopierats till array) 
        foreach (var kvp in stackArray)
        {
            if (kvp.Key == key) return kvp.Value;
        }
        
        // Slänger ett error om ingen nyckel matchar
        throw new KeyNotFoundException("No key matching the specified argument");
    }

    public void Delete(string key)
    {
        int hash = HashingAlgorithm(key, Size);
        var bucket = _buckets[hash];
        bool hasDeleted = false;

        // O(1)
        if (bucket.Count > 0)
        {
            if (bucket.Peek().Key == key)
            {
                bucket.Pop();
                if (_buckets[hash].Count == 0)
                {
                    // Minska endast om bucketen verkligen är tom
                    OccupiedBuckets--;
                }
                return;
            }
        }
        
        var stackArray = bucket.ToArray();
        Stack<KeyValuePair<string, object>> newStack = new Stack<KeyValuePair<string, object>>();

        // O(N)
        foreach (var kvp in stackArray)
        {
            if (kvp.Key != key)
            {
                newStack.Push(kvp);
            }
            else
            {
                hasDeleted = true;
            }
        }

        if (!hasDeleted)
        {
            // Slänger ett error om ingen nyckel matchar
            throw new KeyNotFoundException("No key matching the given argument");
        }
        if (bucket.Count > 0 && newStack.Count == 0)
        {
            OccupiedBuckets--;
        }

        _buckets[hash] = newStack;
        Console.WriteLine($"{key} has been removed from the table!");
        Resize();
        
    }

    // Kör efter insert och delete
    // Krymper när den är mindre än 50% full och växer vid mer än 75% full.
    private void Resize()
    {
        if (((float)OccupiedBuckets / Size) > 0.75)
        {
            // Ökar storkelen med 1.25 
            Stack<KeyValuePair<string, object>>[] oldBuckets = _buckets;
            Size = (int)(Size * 1.25);
            _buckets = new Stack<KeyValuePair<string, object>>[Size];
            UpdateBuckets(oldBuckets);
            
        }

        if (((float)OccupiedBuckets / Size) < 0.5)
        {
            // Minskar storleken med 0.8
            Stack<KeyValuePair<string, object>>[] oldBuckets = _buckets;
            Size = (int)(Size * 0.8);
            _buckets = new Stack<KeyValuePair<string, object>>[Size];
            UpdateBuckets(oldBuckets);
        }

        void UpdateBuckets(Stack<KeyValuePair<string, object>>[] oldBuckets)
        {
            IsResizing = true;
            for(int i = 0; i < Size; i++)
            {
                _buckets[i] = new Stack<KeyValuePair<string, object>>();
            }

            OccupiedBuckets = 0;
            
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
        
    }
}
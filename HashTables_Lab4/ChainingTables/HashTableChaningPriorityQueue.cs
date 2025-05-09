
namespace HashTables_Lab4;

public class HashTableChainingPriority : IHashTable
{
    public int Size { get; private set; }
    private int OccupiedBuckets { get; set; }
    // Priority queue för colision handling, int som priority
    public PriorityQueue<PriorityNode, int>[] _buckets { get; private set; }
    public Func<string, int, int> HashingAlgorithm { get; set; }
    public bool IsResizing { get; set; }
    
    // Har delfault size på 10.
    public HashTableChainingPriority(int size = 10)
    {
        // Antalet buckets
        this.Size = size;
        // Array bestående av listor/buckets
        this._buckets = new PriorityQueue<PriorityNode, int>[size];
        
        // Skapar en ny lista i varje bucket
        for(int i = 0; i < size; i++)
        {
            _buckets[i] = new PriorityQueue<PriorityNode, int>();
        }

        // Sätter default hashing algorithm
        this.HashingAlgorithm = HashingAlgoritms.GetASCIIHash;
        this.IsResizing = false;
    }

    public void Add(string key, object value)
    {
        // Overloading för att följa interfacet
        // Fungerar även som en "default" när man inte har något värde på priority. 
        Add(key, value, 0);
    }
    public void Add(string key, object value, int priority)
    {
        int hash = HashingAlgorithm(key, Size);
        _buckets[hash].Enqueue(new PriorityNode(key, value, priority), priority);
        Console.WriteLine($"{key} has been added to the table!");
        if (!(_buckets[hash].Count > 1))
        {
            // Öka endast om bucketen verkligen var tom
            OccupiedBuckets++;
        }
        if(!IsResizing) Resize();
    }

    public object Get(string key)
    {
        // Måste skapa en ny queue för att man kan inte itterera genom en priority queue
        int hash = HashingAlgorithm(key, Size);
        PriorityQueue<PriorityNode, int> newQueue =
            new PriorityQueue<PriorityNode, int>();

        PriorityNode correctItem = null;

        while (_buckets[hash].Count > 0)
        {
            PriorityNode selectedItem = _buckets[hash].Dequeue();
            if (selectedItem.Key == key)
            {
                correctItem = selectedItem;
                newQueue.Enqueue(correctItem, correctItem.Priority - 1);
                
                /*Ifall att rätt item ligger först i kön, returnera direkt utan att
                  skapa en ny kö och kopiera den, mycket bättre tidskomplexitet.*/
                if (newQueue.Count == 0)
                {
                    correctItem.Priority -= 1;
                    _buckets[hash].Enqueue(correctItem, correctItem.Priority);
                    return correctItem.Value;
                }
            }
            else
            {
                newQueue.Enqueue(selectedItem, selectedItem.Priority); 
            }
        }

        _buckets[hash] = newQueue;

        if (correctItem != null)
        {
            return correctItem.Value;
        }
        else
        {
            // Slänger ett error om ingen nyckel matchar
            throw new KeyNotFoundException("No key matching the specified argument");
        }
    }

    public void Delete(string key)
    {
        // Måste skapa en ny queue för att man kan inte itterera genom en priority queue
        int hash = HashingAlgorithm(key, Size);
        PriorityQueue<PriorityNode, int> newQueue =
            new PriorityQueue<PriorityNode, int>();

        object? correctItem = null;

        while (_buckets[hash].Count > 0)
        {
            var selectedItem = _buckets[hash].Dequeue();
            if (selectedItem.Key == key)
            {
                correctItem = selectedItem;
                /*Ifall att rätt item ligger först i kön, returnera direkt utan att
                  skapa en ny kö och kopiera den, mycket bättre tidskomplexitet.*/
                if (newQueue.Count == 0)
                {
                    if (_buckets[hash].Count == 0)
                    {
                        // Minska endast om bucketen verkligen är tom
                        OccupiedBuckets--;
                    }
                    return;
                }
            }
            else
            {
                newQueue.Enqueue(selectedItem, selectedItem.Priority);
            }
        }
        
        if (correctItem != null)
        {
            _buckets[hash] = newQueue;
            if (_buckets[hash].Count == 0)
            {
                // Minska endast om bucketen verkligen är tom
                OccupiedBuckets--;
            }
        }
        else
        {
            // Slänger ett error om ingen nyckel matchar
            throw new KeyNotFoundException("No key matching the specified argument");
        }
    }

    // Kör efter insert och delete
    private void Resize()
    {
        if (((float)OccupiedBuckets / Size) > 0.75)
        {
            // Dubblar storleken
            PriorityQueue<PriorityNode, int>[] oldBuckets = _buckets;
            Size *= 2;
            _buckets = new PriorityQueue<PriorityNode, int>[Size];
            
            for(int i = 0; i < Size; i++)
            {
                _buckets[i] = new PriorityQueue<PriorityNode, int>();
            }

            OccupiedBuckets = 0;
            IsResizing = true;
            // Nästlad for loop, dålig tidskomplexitet
            foreach (var bucket in oldBuckets)
            {
                while (bucket.Count >0)
                {
                    Add(bucket.Dequeue().Key, bucket.Dequeue().Value, bucket.Dequeue().Priority);
                }
            }

            IsResizing = false;
        }
        return;
    }
}
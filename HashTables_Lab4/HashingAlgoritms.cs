namespace HashTables_Lab4;

public abstract class HashingAlgoritms
{
    public static int GetASCIIHash(string key, int tableSize)
    {
        /* Väldigt simpel hash som jag gjorde själv,
           adderar alla ASCII värden i strängen.*/
        long hash = 0;
        foreach (char character in key)
        {
            // Konverterar bokstav till ASCII kod. 
            hash += (int)character;
        }
        return (int)hash % tableSize;
    }

    
    // Bad ChatGPT att ge tips om olika hashfunktioner som genererar få collisions
    
    // Svår att förstå
    // https://theartincode.stanis.me/008-djb2/
    public static int GetDJB2Hash(string key, int tableSize)
    {
        int hash = 5381;

        foreach (char character in key)
        {
            hash += hash * 33 + (int)character;
        }

        return hash % tableSize;
    }

    // FNV-1
    // https://en.wikipedia.org/wiki/Fowler–Noll–Vo_hash_function
    public static int GetFNVHash(string key, int tableSize)
    {
        uint prime = 16777619; // Speciellt utvalt primtal för denna algoritmen
        uint hash = 2166136261; // (offset basis)

        foreach (char character in key)
        {
            hash *= prime;
            hash ^= (uint)character; // XOR operation
        }

        return (int)(hash % tableSize);

    }
}
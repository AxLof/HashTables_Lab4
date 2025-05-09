namespace HashTables_Lab4;

public class PriorityNode
{
    public string Key { get; set; }
    public object Value { get; set; }
    public int Priority { get; set; }

    public PriorityNode(string key, object value, int priority)
    {
        this.Key = key;
        this.Value = value;
        this.Priority = priority;
    }
}
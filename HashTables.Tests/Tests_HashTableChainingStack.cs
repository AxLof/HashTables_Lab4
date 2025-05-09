using HashTables_Lab4;
using Xunit;

namespace TestHashTables;

public class Tests_HashTableChainingStack
{
    [Theory]
    [InlineData("h20adada")]
    [InlineData("h22beber")]
    [InlineData("h23cajoh")]
    [InlineData("h23daåke")]
    public void ReturnsCorrectStudent(string student)
    {
        HashTableChainingStack testTable = CreateTestTable();
        
        object result = testTable.Get(student);
        
        Assert.Equal(student, ((Student)result).StudentId);
    }
    
    [Theory]
    [InlineData("h20adada")]
    [InlineData("h22beber")]
    [InlineData("h23cajoh")]
    [InlineData("h23daåke")]
    public void RemoveStudent(string student)
    {
        HashTableChainingStack testTable = CreateTestTable();
        
        testTable.Delete(student);
        
        Assert.Throws<KeyNotFoundException>(() => testTable.Get(student));
    }
    
    [Fact]
    public void TestResizing()
    {
        HashTableChainingStack testTable = new HashTableChainingStack();

        int currentTableSize = testTable.Size;
            
        Student Erik = new Student("h24erlöf", "Erik Löf", 22);
        
        testTable.Add(Erik.StudentId, Erik);
        
        // Ska krympa med 0.8 eftersom att den är mindre än 50% full. 
        Assert.Equal(currentTableSize*0.8, testTable.Size);
        
    }

    private HashTableChainingStack CreateTestTable()
    {
        HashTableChainingStack testTable = new HashTableChainingStack();
        Student Adam = new Student("h20adada", "Adam Adamson", 22);
        Student Bertil = new Student("h22beber", "Bertil Bertilsson", 41);
        Student Caesar = new Student("h23cajoh", "Caesar Johansson", 28);
        Student David = new Student("h23daåke", "David Åkesson", 23);
        
        testTable.Add(Adam.StudentId, Adam);
        testTable.Add(Bertil.StudentId, Bertil);
        testTable.Add(Caesar.StudentId, Caesar);
        testTable.Add(David.StudentId, David);

        return testTable;
    }
}
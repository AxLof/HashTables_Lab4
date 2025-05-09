using HashTables_Lab4;
using Xunit;

namespace TestHashTables;

public class Tests_HashTablePriorityQueue
{
    [Theory]
    [InlineData("h20adada")]
    [InlineData("h22beber")]
    [InlineData("h23cajoh")]
    [InlineData("h23daåke")]
    public void ReturnsCorrectStudent(string student)
    {
        HashTableChainingPriority testTable = CreateTestTable(10);
        
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
        HashTableChainingPriority testTable = CreateTestTable(10);
        
        testTable.Delete(student);
        
        Assert.Throws<KeyNotFoundException>(() => testTable.Get(student));
    }

    /* Testar så att objektet som nyss hämtats hamnar längst fram i kön.
       Skapar en hashmap med endast en bucket för att säkerställa att alla objekt
       hamnar i samma bucket för att kunna testa att prioritet fungerar. */
    [Fact]
    public void TestPriority()
    {
        HashTableChainingPriority testTable = new HashTableChainingPriority(1);
        // Måste sätta flaggan IsResizing till true så att den inte ändrar storlek. 
        testTable.IsResizing = true;
        Student Adam = new Student("h20adada", "Adam Adamson", 22);
        Student Bertil = new Student("h22beber", "Bertil Bertilsson", 41);
        Student Caesar = new Student("h23cajoh", "Caesar Johansson", 28);
        Student David = new Student("h23daåke", "David Åkesson", 23);
        testTable.Add(Adam.StudentId, Adam);
        testTable.Add(Bertil.StudentId, Bertil);
        testTable.Add(Caesar.StudentId, Caesar);
        testTable.Add(David.StudentId, David);
        
        testTable.Get("h23cajoh");
        Assert.Equal("h23cajoh", testTable._buckets[0].Peek().Key);
    }

    private HashTableChainingPriority CreateTestTable(int size)
    {
        HashTableChainingPriority testTable = new HashTableChainingPriority(size);
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
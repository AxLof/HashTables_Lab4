using HashTables_Lab4;
using Xunit;
namespace TestHashTables;

public class Tests_HashTableLinearProbing
{
    [Theory]
    [InlineData("h20adada")]
    [InlineData("h22beber")]
    [InlineData("h23cajoh")]
    [InlineData("h23daåke")]
    public void ReturnsCorrectStudent(string student)
    {
        HashTableLinearProbing testTable = CreateTestTable();
        
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
        HashTableLinearProbing testTable = CreateTestTable();
        
        testTable.Delete(student);
        
        Assert.Throws<KeyNotFoundException>(() => testTable.Get(student));
    }

    private HashTableLinearProbing CreateTestTable()
    {
        HashTableLinearProbing testTable = new HashTableLinearProbing(HashingAlgoritms.GetFNVHash);
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
using System.Runtime.InteropServices;
using HashTables_Lab4;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace TestHashTables;

public class Tests_HashTableChaining
{
    [Theory]
    [InlineData("h20adada")]
    [InlineData("h22beber")]
    [InlineData("h23cajoh")]
    [InlineData("h23daåke")]
    public void ReturnsCorrectStudent(string student)
    {
        HashTableChaining testTable = CreateTestTable(10);
        
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
        HashTableChaining testTable = CreateTestTable(10);
        
        testTable.Delete(student);
        
        Assert.Throws<KeyNotFoundException>(() => testTable.Get(student));
    }

    [Fact]
    public void TestResizing()
    {
        HashTableChaining testTable = CreateTestTable(6);

        int currentTableSize = testTable.Size;
            
        Student Erik = new Student("h24erlöf", "Erik Löf", 22);
        Student Filip = new Student("h22fiper", "Filip Persson", 41);
        Student Gustav = new Student("h17gufil", "Gustav Filipsson", 28);
        Student Helge = new Student("h20hedav", "Helge Davidsson", 23);
        
        testTable.Add(Erik.StudentId, Erik);
        testTable.Add(Filip.StudentId, Filip);
        testTable.Add(Gustav.StudentId, Gustav);
        testTable.Add(Helge.StudentId, Helge);
        
        // Nu ska table vara min 80% full och bör därför dubbla i storlek. 
        Assert.Equal(currentTableSize*2, testTable.Size);
        
    }

    private HashTableChaining CreateTestTable(int size)
    {
        HashTableChaining testTable = new HashTableChaining(size);
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
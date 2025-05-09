namespace HashTables_Lab4;

public class Student
{
    public string StudentId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public Student(string studentId, string name, int age)
    {
        this.StudentId = studentId;
        this.Name = name;
        this.Age = age;
    }

    public void PrintStudentInfo()
    {
        Console.WriteLine($"StudentID: {StudentId}\n" +
                          $"Full Name: {Name}\n" +
                          $"Age: {Age.ToString()}\n");
    }
}
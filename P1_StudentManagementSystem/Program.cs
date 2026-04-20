List<Student> StorestudentData = new List<Student>();

while (true)
{
    Console.WriteLine("\n--- Student Management System ---");
    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. View Student");
    Console.WriteLine("3. Update Student");
    Console.WriteLine("4. Delete Student");
    Console.WriteLine("5. Exit");
    Console.WriteLine("Choose an option: ");

    int userInput = Convert.ToInt32(Console.ReadLine());

    switch (userInput)
    {
        case 1:
            addStudent(StorestudentData);
            break;
        case 2:
            viewStudent(StorestudentData);
            break;
        case 3:
            updateStudent(StorestudentData);
            break;
        case 4:
            deleteStudent(StorestudentData);
            break;
        case 5:
            return;
        default:
            Console.WriteLine("Sorry, please enter a correct choice!");
            break;
    }
}

static void addStudent(List<Student> StorestudentData)
{
    Student student = new Student();

    Console.WriteLine("Enter Id: ");
    student.Id = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Enter Name: ");
    student.Name = Console.ReadLine();

    Console.WriteLine("Enter Age: ");
    student.Age = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Enter Course: ");
    student.Course = Console.ReadLine();

    StorestudentData.Add(student);
    Console.WriteLine("Student added successfully!");
}

static void viewStudent(List<Student> StorestudentData)
{
    if (StorestudentData.Count == 0)
    {
        Console.WriteLine("No students found.");
        return;
    }

    foreach (var student in StorestudentData)
    {
        Console.WriteLine($"Id: {student.Id}, Name: {student.Name}, Age: {student.Age}, Course: {student.Course}");
    }
}

static void updateStudent(List<Student> StorestudentData) 
{
    Console.WriteLine("Enter Student Id to update: ");
    int id = Convert.ToInt32(Console.ReadLine());
    var student = StorestudentData.FirstOrDefault(s => s.Id == id);
    if (student == null)
    {
        Console.WriteLine("Student not found.");
        return;
    }
    Console.WriteLine("Enter new Name: ");
    student.Name = Console.ReadLine();
    Console.WriteLine("Enter new Age: ");
    student.Age = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Enter new Course: ");
    student.Course = Console.ReadLine();
    Console.WriteLine("Student updated successfully!");
}

static void deleteStudent(List<Student> StorestudentData) 
{
    Console.WriteLine("Enter Student Id to delete: ");
    int id = Convert.ToInt32(Console.ReadLine());
    var student = StorestudentData.FirstOrDefault(s => s.Id == id);
    if (student == null)
    {
        Console.WriteLine("Student not found.");
        return;
    }
    StorestudentData.Remove(student);
    Console.WriteLine("Student deleted successfully!");
}
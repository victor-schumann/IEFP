﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

/* TODO:
 * Implement binary files for everything (validate and handle potential issues, if they occur, fix them).
 * Implement text files.
 * Instead of "yes," allow options like "1 - yes" and "2 - no," possibly in a menu format as with everything else.
 * When asking "do you want to go back to the previous menu," make the option to exit the program explicit when choosing "no."
 */

/* How to update git:
 * 1. Open command line tools (Git Bash)
 * 2. OneDrive/Área\ de\ Trabalho/Samanta/CURSO\ PROGRAMAÇÃO/IEFP/
 * 3. git add .
 * 4. git commit -m "your commit message here"]
 * 5. git push
 */

class Program
{   // Esse bloco serve para declarar as listas que são estáticas
    static List<Ufcd> ufcds = new List<Ufcd>(); //ufcds = lista de objetos do tipo ufcd
    static List<Class> classes = new List<Class>();
    static List<Student> students = new List<Student>();
    static List<Professor> professors = new List<Professor>();
    static List<Turma> turmas = new List<Turma>();

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, welcome to the IEFP Leiria UFCDs Enrollment Management Project");
        MainMenu();
    }

    static bool ValidatePath(string Path, boolean ValidatePathResult)
    {
        string Pattern = @"^@""/[a-zA-Z0-9_/]+\.bin""$";

        while (!ValidatePathResult) {
            if (BinaryFilePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0) {
                Console.WriteLine("Invalid characters in path.");
                Console.WriteLine(Path);
            }
            else if (!BinaryFilePath.EndsWith(".bin")) {
                Console.WriteLine("File must end with .bin extension.");
                Console.WriteLine(Path);
            }
             else if (!BinaryFilePath.StartsWith("@")) {
                Console.WriteLine("File must starts with @ symbol.");
                Console.WriteLine(Path);
            }
            else if (!Regex.IsMatch(Path, Pattern)) {
                Console.WriteLine("The path is not in the valid format.");
                Console.WriteLine(Path);
            }
            else {
                ValidatePathResult = true;
                Console.WriteLine(Path);
                Console.WriteLine("\nTRUE");
            }
        }
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("---\nWhich option do you want?\n" +
                "\n1) UFCDs" +
                "\n2) Classes" +
                "\n3) Manage Students" +
                "\n4) Manage Professors" +
                "\n\n5) Save Data" +
                "\n6) Load Data" +
                "\n7) Set Binary Path" +
                "\n8) Exit\n");

            string decisionMainMenu = Console.ReadLine();
            string BinarySaveFile = @"/home/schu/Downloads/samanta/IEFP/Projeto de Programação/Projeto_UFCDs_Sonia/Projeto_UFCDs_Sonia/binaries_test.bin";
            string BinaryFilePath = "";
            bool ValidatePathResult = false;
            // make the user input define the above
            // print the above variable to see if the @ is included as expected

            switch (decisionMainMenu)
            {
                case "1":
                    UfcdMenu();
                    break;
                case "2":
                    ClassesMenu();
                    break;
                case "3":
                    ManageStudents();
                    break;
                case "4":
                    ManageProfessors();
                    break;
                case "5":
                    SaveData(BinarySaveFile);
                    Console.WriteLine("Data was saved!");
                    MainMenu();
                    break;
                case "6":
                    LoadData(BinarySaveFile);
                    Console.WriteLine("Data was loaded!");
                    MainMenu();
                    break;
                case "7":
                    Console.Write("Please input the path to save your binary files. Follow this prompt: \n");
                    Console.Write("\t@\"test/your/path/BINARY_FILE_NAME.bin\"\n");
                    Console.Write("Don't forget the '@' at the begginning, the position of the \"\", and to specify the name and extension of the binary file at the end of the path.\n");                    
                    
                    ValidatePathResult = ValidatePath(BinaryFilePath);
                    Console.WriteLine(result);
                    break;
                case "8":
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0); // Força a saída do programa
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    MainMenu();
                    break;
            }
        }
    }


    // UFCD
    static void UfcdMenu()
    {
        Console.WriteLine("---\nUFCD Menu" +
            "\n1) List UFCDs" +
            "\n2) Create New UFCD" +
            "\n3) Return to main menu\n");

        string choseUfcdMenu = Console.ReadLine();

        switch (choseUfcdMenu)
        {
            case "1":
                ListUfcds();
                break;
            case "2":
                CreateUfcd();
                break;
            case "3":
                MainMenu();
                break;
            default:
                Console.WriteLine("Invalid option");
                UfcdMenu();
                break;
        }
    }


    static void CreateUfcd()
    {
        Console.WriteLine("Creating a new UFCD...");

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Code: ");
        string code = Console.ReadLine();
        Console.Write("Enter Description: ");
        string description = Console.ReadLine();
        Console.Write("Enter Duration: ");
        int duration = int.Parse(Console.ReadLine());
        Console.Write("Enter Modality: ");
        string modality = Console.ReadLine();
        Console.Write("Enter Schedule: ");
        string schedule = Console.ReadLine();
        Console.Write("Enter Course: ");
        string course = Console.ReadLine();

        ufcds.Add(new Ufcd
        {
            Name = name,
            Code = code,
            Description = description,
            Duration = duration,
            Modality = modality,
            Schedule = schedule,
            Course = course
        });

        Console.WriteLine("UFCD created successfully!");

        Console.WriteLine("Do you want to return to the main menu?" +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToMainMenu = Console.ReadLine();
        if (returnToMainMenu.ToLower() == "1")
            MainMenu();
    }

    static void GiveGradesToStudents()
    {
        Console.WriteLine("---\nGive Grades to Students");

        // Listar nomes das turmas existentes
        Console.WriteLine("Available Classes:");
        ListClassNames();

        Console.Write("Enter Class Name: ");
        string className = Console.ReadLine();

        Class selectedClass = classes.FirstOrDefault(cls => cls.Name == className); // Busca a classe selecionada na lista de classes

        if (selectedClass == null)
        {
            Console.WriteLine("Class not found.");
            GiveGradesToStudents();
            return; // Volta para evitar processamento adicional
        }

        Console.WriteLine($"Students in {selectedClass.Name}:");

        foreach (var student in selectedClass.Students)
        {
            Console.WriteLine($"ID: {student.ID}, Name: {student.Name}");
            student.Grades = new List<string>(); // Inicializa a lista Grades para cada aluno
        }
       
        Console.Write("Enter Grades for Students (comma-separated): ");
        string gradesInput = Console.ReadLine();
        string[] grades = gradesInput.Split(',');

        if (grades.Length != selectedClass.Students.Count)
        {
            Console.WriteLine("Invalid number of grades. Please provide a grade for each student.");
            GiveGradesToStudents();
            return; // Volta para evitar processamento adicional
        }

        for (int i = 0; i < selectedClass.Students.Count; i++)
        {
            selectedClass.Students[i].Grades.Add(grades[i].Trim());
        }

        Console.WriteLine("Grades added successfully!");

        // Atualizar a descrição da turma com as notas
        selectedClass.Description += $"\nGrades for Class '{selectedClass.Name}': {string.Join(", ", grades)}";

        Console.WriteLine("Do you want to return to the main menu? (yes/no)");
        string returnToMainMenu = Console.ReadLine();
        if (returnToMainMenu.ToLower() == "yes")
            MainMenu();
    }


    static void ListClassNames() // essa função é usada apenas para mostrar os nomes das classes durante a atribuição das notas
    {
        Console.WriteLine("---\nList of Classes:");

        foreach (var cls in classes)
        {
            Console.WriteLine($"Class Name: {cls.Name}");
        }
    }

    static void ListUfcds()
    {
        Console.WriteLine("---\nList of UFCDs:");

        foreach (var ufcd in ufcds)
        {
            Console.WriteLine($"Name: {ufcd.Name}");
            Console.WriteLine($"Code: {ufcd.Code}");
            Console.WriteLine($"Description: {ufcd.Description}");
            Console.WriteLine($"Duration: {ufcd.Duration}");
            Console.WriteLine($"Modality: {ufcd.Modality}");
            Console.WriteLine($"Schedule: {ufcd.Schedule}");
            Console.WriteLine($"Course: {ufcd.Course}\n");
        }

        Console.WriteLine("Do you want to return to the main menu ? " +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToUfcdMenu = Console.ReadLine();
        if (returnToUfcdMenu.ToLower() == "1")
            UfcdMenu();
    }

    // Students

    static void ManageStudents()
    {
        Console.WriteLine("---\nManage Students Menu" +
            "\n1) Enroll Student" +
            "\n2) List Student Information" +
            "\n3) Return to main menu\n");

        string choseManageStudents = Console.ReadLine();

        switch (choseManageStudents)
        {
            case "1":
                EnrollStudent();
                break;
            case "2":
                ListStudents();
                break;
            case "3":
                MainMenu();
                break;
            default:
                Console.WriteLine("Invalid option");
                ManageStudents();
                break;
        }
    }

    static void EnrollStudent()
    {
        Console.WriteLine("Enrolling a new student...");

        Console.Write("Enter ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Date of Birth: ");
        string dateOfBirth = Console.ReadLine();
        Console.Write("Enter NIF: ");
        string nif = Console.ReadLine();
        Console.Write("Enter Email: ");
        string email = Console.ReadLine();
        Console.Write("Enter Address: ");
        string address = Console.ReadLine();
        Console.Write("Enter Phone: ");
        string phone = Console.ReadLine();
        Console.Write("Enter Availability: ");
        string availability = Console.ReadLine();

        students.Add(new Student
        {
            ID = id,
            Name = name,
            DateOfBirth = dateOfBirth,
            NIF = nif,
            Email = email,
            Address = address,
            Phone = phone,
            Availability = availability
        });

        Console.WriteLine("Where do you want to go next? " +
            "\n1) Main Menu" +
            "\n2) Student Menu" +
            "\n3) Enrol a new student");

        string choseManageStudents = Console.ReadLine();

        switch (choseManageStudents)
        {
            case "1":
                MainMenu();
                    break;
            case "2":
                ManageStudents();
                break;
            case "3":
                EnrollStudent();
                break;
            default:
                Console.WriteLine("Invalid option, redirecting to Main Menu");
                MainMenu();
                break;
        }

        Console.WriteLine("Student enrolled successfully!");
    }

    static void ListStudents()
    {
        Console.WriteLine("---\nList of Students:");

        foreach (var student in students)
        {
            Console.WriteLine($"Name: {student.Name}");
            Console.WriteLine($"ID: {student.ID}");
            Console.WriteLine($"Date of Birth: {student.DateOfBirth}");
            Console.WriteLine($"NIF: {student.NIF}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"Address: {student.Address}");
            Console.WriteLine($"Phone: {student.Phone}");
            Console.WriteLine($"Availability: {student.Availability}\n");
        }

        Console.WriteLine("Do you want to return to the main menu?" +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToManageStudentsMenu = Console.ReadLine();
        if (returnToManageStudentsMenu.ToLower() == "1")
            ManageStudents();
    }

    // Professors

    static void ManageProfessors()
    {
        Console.WriteLine("---\nManage Professors Menu" +
            "\n1) Enroll Professor" +
            "\n2) List Professor Information" +
            "\n3) Return to main menu\n");

        string choseManageProfessors = Console.ReadLine();

        switch (choseManageProfessors)
        {
            case "1":
                EnrollProfessor();
                break;
            case "2":
                ListProfessors();
                break;
            case "3":
                MainMenu();
                break;
            default:
                Console.WriteLine("Invalid option");
                ManageProfessors();
                break;
        }
    }

    static void EnrollProfessor()
    {
        Console.WriteLine("Enrolling a new professor...");

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Date of Birth: ");
        string dateOfBirth = Console.ReadLine();
        Console.Write("Enter NIF: ");
        string nif = Console.ReadLine();
        Console.Write("Enter Email: ");
        string email = Console.ReadLine();
        Console.Write("Enter Address: ");
        string address = Console.ReadLine();
        Console.Write("Enter Phone: ");
        string phone = Console.ReadLine();
        Console.Write("Enter Qualifications: ");
        string qualifications = Console.ReadLine();
        Console.Write("Enter Availability: ");
        string availability = Console.ReadLine();

        professors.Add(new Professor
        {
            Name = name,
            DateOfBirth = dateOfBirth,
            NIF = nif,
            Email = email,
            Address = address,
            Phone = phone,
            Qualifications = qualifications,
            Availability = availability
        });

        Console.WriteLine("Professor enrolled successfully!");

        Console.WriteLine("Do you want to return to the main menu ? " +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToManageProfessorsMenu = Console.ReadLine();
        if (returnToManageProfessorsMenu.ToLower() == "1")
            ManageProfessors();
    }

    static void ListProfessors()
    {
        Console.WriteLine("---\nList of Professors:");

        foreach (var professor in professors)
        {
            Console.WriteLine($"Name: {professor.Name}");
            Console.WriteLine($"Date of Birth: {professor.DateOfBirth}");
            Console.WriteLine($"NIF: {professor.NIF}");
            Console.WriteLine($"Email: {professor.Email}");
            Console.WriteLine($"Address: {professor.Address}");
            Console.WriteLine($"Phone: {professor.Phone}");
            Console.WriteLine($"Qualifications: {professor.Qualifications}");
            Console.WriteLine($"Availability: {professor.Availability}\n");
        }

        Console.WriteLine("Do you want to return to the main menu ? " +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToManageProfessorsMenu = Console.ReadLine();
        if (returnToManageProfessorsMenu.ToLower() == "1")
            ManageProfessors();
    }

    // Classes

    static void ClassesMenu()
    {
        Console.WriteLine("---\nClasses Menu" +
            "\n1) List Classes" +
            "\n2) Create New Class" +
            "\n3) Edit Class" +
            "\n4) Add UFCDs to Class" +
            "\n5) Add Student to Class" +
            "\n6) Give Grades to Students" +
            "\n7) Return to main menu\n");

        string choseClassesMenu = Console.ReadLine();

        switch (choseClassesMenu)
        {
            case "1":
                ListClasses();
                break;
            case "2":
                CreateClass();
                break;
            case "3":
                EditClass();
                break;
            case "4":
                AddUFCDsToClass();
                break;
            case "5":
                AddStudentsToClass();
                break;
            case "6":
                GiveGradesToStudents();
                break;
            case "7":
                MainMenu();
                break;
            default:
                Console.WriteLine("Invalid option");
                ClassesMenu();
                break;
        }
    }

    static void AddStudentsToClass()
    {
        Console.WriteLine("Enter the code of the class to which you want to add students:");
        string classCode = Console.ReadLine();

        Class selectedClass = classes.FirstOrDefault(cls => cls.Code == classCode);

        if (selectedClass == null)
        {
            Console.WriteLine("Class not found.");
            ClassesMenu();
        }

        Console.WriteLine("Available Students:");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.ID}, Name: {student.Name}, NIF: {student.NIF}");
        }

        Console.WriteLine("Enter the IDs of the students you want to add (comma-separated):");
        string studentIdsInput = Console.ReadLine();
        string[] studentIds = studentIdsInput.Split(',');

        foreach (var studentId in studentIds)
        {
            int id = int.Parse(studentId.Trim());
            Student selectedStudent = students.FirstOrDefault(std => std.ID == id);

            if (selectedStudent != null)
            {
                selectedClass.AddStudent(selectedStudent);
            }
        }

        Console.WriteLine("Students added to the class successfully!");

        Console.WriteLine("Do you want to return to the main menu ? " +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToClassesMenu = Console.ReadLine();
        if (returnToClassesMenu.ToLower() == "1")
            ClassesMenu();
    }

    static void AddUFCDsToClass()
    {
        Console.WriteLine("Enter the code of the class to which you want to add UFCDs:");
        string classCode = Console.ReadLine();

        Class selectedClass = classes.FirstOrDefault(cls => cls.Code == classCode);

        if (selectedClass == null)
        {
            Console.WriteLine("Class not found.");
            ClassesMenu();
        }

        Console.WriteLine("Available UFCDs:");
        foreach (var ufcd in ufcds)
        {
            Console.WriteLine($"Code: {ufcd.Code}, Name: {ufcd.Name}");
        }

        Console.WriteLine("Enter the codes of the UFCDs you want to add (comma-separated):");
        string ufcdCodesInput = Console.ReadLine();
        string[] ufcdCodes = ufcdCodesInput.Split(',');

        foreach (var ufcdCode in ufcdCodes)
        {
            Ufcd selectedUfcd = ufcds.FirstOrDefault(ufcd => ufcd.Code == ufcdCode.Trim());

            if (selectedUfcd != null)
            {
                selectedClass.AddUfcd(selectedUfcd);
            }
        }

        Console.WriteLine("UFCDs added to the class successfully!");

        Console.WriteLine("Do you want to return to the main menu?" +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToClassesMenu = Console.ReadLine();
        if (returnToClassesMenu.ToLower() == "1")
            ClassesMenu();
    }

    static void ListClasses()

    {
        Console.WriteLine("---\nList of Classes:");

        foreach (var cls in classes)
        {
            Console.WriteLine($"Class Name: {cls.Name}");
            Console.WriteLine($"Class Code: {cls.Code}");
            Console.WriteLine($"Class Schedule: {cls.Schedule}");
            Console.WriteLine($"Professor: {cls.Professor}");
            Console.WriteLine("UFCDs:");

            foreach (var ufcd in cls.Ufcds)
            {
                Console.WriteLine($"  Name: {ufcd.Name}");
                Console.WriteLine($"  Code: {ufcd.Code}");
            }

            Console.WriteLine("Students in the Class:");
            foreach (var student in cls.Students)
            {
                Console.WriteLine($"  Name: {student.Name}");
                Console.WriteLine($"  ID: {student.ID}");
                Console.WriteLine($"  NIF: {student.NIF}");
                Console.WriteLine($"  Grade: {string.Join(", ", student.Grades)}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("Do you want to return to the main menu?" +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToClassesMenu = Console.ReadLine();
        if (returnToClassesMenu.ToLower() == "1")
            ClassesMenu();
    }

    static void CreateClass()
    {
        Console.WriteLine("Creating a new class...");

        Console.Write("Enter Class Name: ");
        string className = Console.ReadLine();
        Console.Write("Enter Class Code: ");
        string classCode = Console.ReadLine();
        Console.Write("Enter Class Schedule: ");
        string classSchedule = Console.ReadLine();
        Console.Write("Enter Professor Name: ");
        string professorName = Console.ReadLine();

        classes.Add(new Class
        {
            Name = className,
            Code = classCode,
            Schedule = classSchedule,
            Professor = professorName
        });

        Console.WriteLine("Class created successfully!");

        Console.WriteLine("Do you want to return to the main menu?" +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToClassesMenu = Console.ReadLine();
        if (returnToClassesMenu.ToLower() == "1")
            ClassesMenu();
    }

    static void EditClass()
    {
        Console.WriteLine("Enter the code of the class you want to edit:");
        string classCode = Console.ReadLine();

        Class classToEdit = classes.FirstOrDefault(cls => cls.Code == classCode);

        if (classToEdit == null)
        {
            Console.WriteLine("Class not found.");
            ClassesMenu();
        }

        Console.WriteLine("Editing Class...");

        Console.Write("Enter New Class Name: ");
        string newClassName = Console.ReadLine();
        Console.Write("Enter New Class Schedule: ");
        string newClassSchedule = Console.ReadLine();
        Console.Write("Enter New Professor Name: ");
        string newProfessorName = Console.ReadLine();

        classToEdit.Name = newClassName;
        classToEdit.Schedule = newClassSchedule;
        classToEdit.Professor = newProfessorName;

        Console.WriteLine("Class edited successfully!");

        Console.WriteLine("Do you want to return to the main menu?" +
            "\n1) yes" +
            "\n2) no, exit the program");
        string returnToClassesMenu = Console.ReadLine();
        if (returnToClassesMenu.ToLower() == "1")
            ClassesMenu();
    }

    // Others
    static void SaveData(string BinarySaveFile)
    {
        if (!File.Exists(BinarySaveFile))
        {
            using (File.Create(BinarySaveFile)) { }
        }
        try
        {
            using (FileStream fs = new FileStream(BinarySaveFile, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, ufcds); // Salva a lista ufcds no arquivo
                formatter.Serialize(fs, classes); // Salva a lista classes no arquivo
                formatter.Serialize(fs, professors); // Salva a lista professors no arquivo
                formatter.Serialize(fs, turmas); // Salva a lista turmas no arquivo
                formatter.Serialize(fs, students);
                Console.WriteLine("Data saved successfully!");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error saving data: " + e.Message);
        }
    }

    static void LoadData(string BinarySaveFile)
    {
        try
        {
            using (FileStream fs = new FileStream(BinarySaveFile, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                ufcds = (List<Ufcd>)formatter.Deserialize(fs); // Carrega a lista ufcds do arquivo
                classes = (List<Class>)formatter.Deserialize(fs); // Carrega a lista classes do arquivo
                professors = (List<Professor>)formatter.Deserialize(fs); // Carrega a lista professors do arquivo
                turmas = (List<Turma>)formatter.Deserialize(fs); // Carrega a lista turmas do arquivo

                students = (List<Student>)formatter.Deserialize(fs); // Carrega a lista students do arquivo
                Console.WriteLine("Data loaded successfully!");
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("Error loading data: " + e.Message);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Error deserializing data: " + e.Message);
        }

    }

    // Classes2
    [Serializable]
    class Class
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Schedule { get; set; }
        public string Professor { get; set; }
        public List<Ufcd> Ufcds { get; set; } = new List<Ufcd>();
        public List<Student> Students { get; set; } = new List<Student>();
        public string Description { get; set; } = string.Empty;

        public Class()
        {
            Students = new List<Student>();
        }
        public void AddUfcd(Ufcd ufcd)
        {
            Ufcds.Add(ufcd);
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }
    }

    [Serializable]
    class Ufcd
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Modality { get; set; }
        public string Schedule { get; set; }
        public string Course { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
    }

    [Serializable]
    class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string NIF { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Availability { get; set; }
        public List<string> Grades { get; set; } = new List<string>();
    }

    [Serializable]
    class Professor
    {
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string NIF { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Qualifications { get; set; }
        public string Availability { get; set; }
    }

    [Serializable]
    class Turma
    {
        public string Name { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
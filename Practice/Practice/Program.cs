using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<Task> tasks = new List<Task>();
    const string FilePath = "tasks.txt";

    class Task
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    static void Main()
    {
        LoadTasks();
        
        while (true)
        {
            Console.WriteLine("\nМеню ToDo List:");
            Console.WriteLine("1 - Добавить задачу");
            Console.WriteLine("2 - Показать все задачи");
            Console.WriteLine("3 - Отметить задачу как выполненную");
            Console.WriteLine("4 - Удалить задачу");
            Console.WriteLine("5 - Сохранить и выйти");
            Console.Write("Выберите опцию: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Некорректный ввод!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddTask();
                    break;
                case 2:
                    ShowTasks();
                    break;
                case 3:
                    MarkTaskCompleted();
                    break;
                case 4:
                    DeleteTask();
                    break;
                case 5:
                    SaveTasks();
                    return;
                default:
                    Console.WriteLine("Неизвестная команда!");
                    break;
            }
        }
    }

    static void AddTask()
    {
        Console.Write("Введите новую задачу: ");
        string description = Console.ReadLine();
        
        if (!string.IsNullOrWhiteSpace(description))
        {
            tasks.Add(new Task { Description = description });
            Console.WriteLine("Задача добавлена!");
        }
        else
        {
            Console.WriteLine("Описание задачи не может быть пустым!");
        }
    }

    static void ShowTasks()
    {
        if (!tasks.Any())
        {
            Console.WriteLine("Список задач пуст!");
            return;
        }

        Console.WriteLine("\nСписок задач:");
        for (int i = 0; i < tasks.Count; i++)
        {
            string status = tasks[i].IsCompleted ? "[x]" : "[ ]";
            Console.WriteLine($"{i + 1}. {status} {tasks[i].Description}");
        }
    }

    static void MarkTaskCompleted()
    {
        if (!tasks.Any())
        {
            Console.WriteLine("Список задач пуст!");
            return;
        }

        ShowTasks();
        Console.Write("Введите номер выполненной задачи: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && 
            taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks[taskNumber - 1].IsCompleted = true;
            Console.WriteLine("Задача отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Некорректный номер задачи!");
        }
    }

    static void DeleteTask()
    {
        if (!tasks.Any())
        {
            Console.WriteLine("Список задач пуст!");
            return;
        }

        ShowTasks();
        Console.Write("Введите номер задачи для удаления: ");
        
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && 
            taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks.RemoveAt(taskNumber - 1);
            Console.WriteLine("Задача удалена!");
        }
        else
        {
            Console.WriteLine("Некорректный номер задачи!");
        }
    }

    static void LoadTasks()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(FilePath);
                foreach (string line in lines)
                {
                    if (line.Length >= 4)
                    {
                        bool isCompleted = line.Substring(0, 3) == "[x]";
                        string description = line[4..];
                        tasks.Add(new Task 
                        { 
                            Description = description, 
                            IsCompleted = isCompleted 
                        });
                    }
                }
                Console.WriteLine("Задачи успешно загружены!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
            }
        }
    }

    static void SaveTasks()
    {
        try
        {
            List<string> lines = new List<string>();
            foreach (Task task in tasks)
            {
                string status = task.IsCompleted ? "[x]" : "[ ]";
                lines.Add($"{status} {task.Description}");
            }
            File.WriteAllLines(FilePath, lines);
            Console.WriteLine("Задачи сохранены в файл!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении задач: {ex.Message}");
        }
    }
}
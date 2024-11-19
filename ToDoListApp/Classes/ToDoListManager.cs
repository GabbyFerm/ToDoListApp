using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToDoListApp.Classes
{
    public class ToDoListManager
    {
        private List<ToDoItem> _toDoList = new();
        private readonly string _filePath;
        private readonly ToDoValidator _validator;
        private readonly HistoryManager _historyManager;

        public ToDoListManager(string filePath, HistoryManager historyManager)
        {
            _filePath = filePath;
            _validator = new ToDoValidator();
            _historyManager = historyManager;
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    _toDoList = JsonSerializer.Deserialize<List<ToDoItem>>(json) ?? new List<ToDoItem>();
                }
                else
                {
                    _toDoList = new List<ToDoItem>(); 
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading JSON: {ex.Message}");
                _toDoList = new List<ToDoItem>(); 
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File error: {ex.Message}");
            }
        }

        private void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_toDoList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Permission error: {ex.Message}");
            }
        }
        public void AddToDo(string title, string description)
        {
            var newItem = new ToDoItem { Title = title, Description = description };

            var validationResult = _validator.Validate(newItem);

            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    Console.WriteLine($"Validation failed: {failure.ErrorMessage}");
                }
                return;
            }

            int newId = _toDoList.Count > 0 ? _toDoList.Max(item => item.Id) + 1 : 1;
            newItem.Id = newId;
            _toDoList.Add(newItem);
            SaveToFile();
            _historyManager.AddHistory($"Added To-Do: {newItem.Title} (ID: {newId})");
        }
        public void UpdateToDo(int id, string? newTitle = null, string? newDescription = null, bool? isCompleted = null)
        {
            var item = _toDoList.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                var newItem = new ToDoItem
                {
                    Title = newTitle ?? item.Title,
                    Description = newDescription ?? item.Description
                };

                var validationResult = _validator.Validate(newItem);

                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        Console.WriteLine($"Validation failed: {failure.ErrorMessage}");
                    }
                    return;
                }

                if (newTitle != null) item.Title = newTitle;
                if (newDescription != null) item.Description = newDescription;
                if (isCompleted.HasValue)
                {
                    item.IsCompleted = isCompleted.Value;
                    item.CompletedAt = isCompleted.Value ? DateTime.Now : null;
                }

                SaveToFile();
                _historyManager.AddHistory($"Updated To-Do: {item.Title} (ID: {id})");  
            }
        }

        public bool DeleteToDo(int id)
        {
            var item = _toDoList.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                _toDoList.Remove(item);
                SaveToFile();
                _historyManager.AddHistory($"Deleted To-Do: {item.Title} (ID: {id})");
                return true; 
            }
            return false; 
        }

        public void MarkToDoAsCompleted(int id)
        {
            var item = _toDoList.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.IsCompleted = true;
                SaveToFile();
                _historyManager.AddHistory($"Marked as complete To-Do: {item.Title} (ID: {id})");
                Console.WriteLine($"To-Do with ID {id} marked as completed.");
            }
            else
            {
                Console.WriteLine($"No To-Do item found with ID: {id}");
            }
        }

        public List<ToDoItem> GetAllToDos()
        {
            return _toDoList ?? new List<ToDoItem>();
        }
    }
}
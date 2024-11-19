using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Classes
{
    public class HistoryManager
    {
        private readonly List<string> _history = new();

        public void AddHistory(string message)
        {
            _history.Add($"[{DateTime.Now}] {message}");
        }

        public void ShowHistory()
        {
            if (_history.Count == 0) 
            {
                AnsiConsole.MarkupLine("[red]There is no To Do history.[/]");
            }
            else
            {
                foreach (var log in _history)
                {
                    Console.WriteLine(log);
                }
            }
        }
    }
}

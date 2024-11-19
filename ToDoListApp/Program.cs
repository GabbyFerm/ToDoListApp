using Spectre.Console;
using Figgle;
using ToDoListApp.Classes;

class Program : ToDoMenuHandlers
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine("Json", "ToDoList.json");
        var historyManager = new HistoryManager();
        var toDoManager = new ToDoListManager(filePath, historyManager);
        var toDoMenuHandler = new ToDoMenuHandlers();

        while (true)
        {
            string choice = MainMenuAndHeader.ShowMenuAndHeader();

            switch (choice)
            {
                case "[pink3 italic]View All To-Dos[/]":
                    toDoMenuHandler.ShowToDoList(toDoManager);
                    break;

                case "[pink3 italic]Add To-Do[/]":
                    toDoMenuHandler.AddNewToDo(toDoManager);
                    break;

                case "[pink3 italic]Update To-Do[/]":
                    toDoMenuHandler.UpdateToDoItem(toDoManager);
                    break;

                case "[pink3 italic]Delete To-Do[/]":
                    toDoMenuHandler.DeleteToDoItem(toDoManager);
                    break;

                case "[pink3 italic]Mark To-Do as Complete[/]":
                    toDoMenuHandler.MarkToDosAsComplete(toDoManager);
                    break;

                case "[pink3 italic]View History[/]":
                    historyManager.ShowHistory();
                    break;

                case "[pink3 italic]Exit[/]":
                    return;
            }

            AnsiConsole.MarkupLine("\n[gold3_1]Press any key to return to the menu...[/]");
            Console.ReadKey();
        }
    }
}
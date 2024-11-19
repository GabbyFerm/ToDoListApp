using Spectre.Console;
using Figgle;
using ToDoListApp.Classes;

class Program
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine("Json", "ToDoList.json");
        var historyManager = new HistoryManager();
        var toDoManager = new ToDoListManager(filePath, historyManager);

        while (true)
        {
            Console.Clear();
            ShowHeader();

            var menuOptions = new string[]
            {
                "[pink3 italic]View All To-Dos[/]",
                "[pink3 italic]Add To-Do[/]",
                "[pink3 italic]Update To-Do[/]",
                "[pink3 italic]Delete To-Do[/]",
                "[pink3 italic]Mark To-Do as Complete[/]",
                "[pink3 italic]View History[/]",
                "[pink3 italic]Exit[/]"
            };

            var menuSelection = new SelectionPrompt<string>()
            .Title("[bold gold3_1]What would you like to do?[/]")
            .PageSize(7)
            .AddChoices(menuOptions)
            .HighlightStyle(new Style(Color.LightSeaGreen));

            // Låt användaren välja ett alternativ från menyn
            string choice = AnsiConsole.Prompt(menuSelection);

            //colours: pink3 seagreen3 alt lightseagreen gold3_1

            switch (choice)
            {
                case "[pink3 italic]View All To-Dos[/]":
                    ShowToDoList(toDoManager);
                    break;

                case "[pink3 italic]Add To-Do[/]":
                    AddNewToDo(toDoManager);
                    break;

                case "[pink3 italic]Update To-Do[/]":
                    UpdateToDoItem(toDoManager);
                    break;

                case "[pink3 italic]Delete To-Do[/]":
                    DeleteToDoItem(toDoManager);
                    break;

                case "[pink3 italic]Mark To-Do as Complete[/]":
                    MarkToDosAsComplete(toDoManager);
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

    static void ShowHeader()
    {
        var header = FiggleFonts.Big.Render("To-Do List");
        AnsiConsole.Markup($"[bold pink3]{header}[/]");
    }

    static void ShowToDoList(ToDoListManager toDoManager)
    {
        var todos = toDoManager.GetAllToDos();

        if (todos == null || todos.Count == 0)
        {
            AnsiConsole.MarkupLine("[gold3_1]No To-Dos available.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey39)
            .AddColumn("[pink3]ID[/]")
            .AddColumn("[pink3]Title[/]")
            .AddColumn("[pink3]Completed[/]");

        foreach (var todo in todos)
        {
            table.AddRow(todo.Id.ToString(), todo.Title, todo.IsCompleted ? "[lightseagreen]Yes[/]" : "[red]No[/]");
        }

        AnsiConsole.Write(table);
    }

    static void AddNewToDo(ToDoListManager toDoManager)
    {
        var title = AnsiConsole.Ask<string>("Enter the [lightseagreen]title[/]:");
        var description = AnsiConsole.Ask<string>("Enter the [lightseagreen]description[/]:");
        toDoManager.AddToDo(title, description);
    }

    static void UpdateToDoItem(ToDoListManager toDoManager)
    {
        var todos = toDoManager.GetAllToDos();

        var selectedToDo = AnsiConsole.Prompt(
            new SelectionPrompt<ToDoItem>()
                .Title("[gold3_1]Select a To-Do to update:[/]")
                .AddChoices(todos)
                .HighlightStyle(new Style(Color.LightSeaGreen))
                .UseConverter(todo => $"[pink3 italic]{todo.Id}: {todo.Title}[/]") 
        );

        AnsiConsole.MarkupLine($"[gold3_1]Selected To Do:[/] [pink3 italic]{selectedToDo.Title}[/]\n");
        var newTitle = AnsiConsole.Ask<string?>("Enter the [lightseagreen]new title[/]:");

        if (string.IsNullOrWhiteSpace(newTitle))
        {
            AnsiConsole.MarkupLine("[red]No updates made. Title cannot be empty.[/]");
            return;
        }

        toDoManager.UpdateToDo(selectedToDo.Id, newTitle);
    }

    static void DeleteToDoItem(ToDoListManager toDoManager)
    {
        var id = AnsiConsole.Ask<int>("Enter the [lightseagreen]ID[/] of the To-Do to delete:");
        if (!toDoManager.DeleteToDo(id))
        {
            AnsiConsole.MarkupLine($"[red]To-Do with ID {id} not found.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[lightseagreen]To-Do with ID {id} deleted successfully.[/]");
        }
    }

    static void MarkToDosAsComplete(ToDoListManager toDoManager)
    {
        var todos = toDoManager.GetAllToDos();

        var incompleteTodos = todos.Where(todo => !todo.IsCompleted).ToList();

        if (incompleteTodos.Count == 0)
        {
            AnsiConsole.MarkupLine("[gold3_1]No To-Dos available to mark as complete.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new MultiSelectionPrompt<ToDoItem>()
                .Title("[lightseagreen]Select To-Dos to mark as complete:[/]")
                .HighlightStyle(new Style(foreground: Color.Gold3_1))
                .NotRequired()
                .PageSize(10)
                .UseConverter(todo =>
                {
                    if (todo.IsCompleted)
                    {
                        return $"[lightseagreen]{todo.Id}: {todo.Title} (Completed)[/]"; 
                    }
                    else
                    {
                        return $"[pink3 italic]{todo.Id}: {todo.Title}[/]"; 
                    }
                })
                .AddChoices(incompleteTodos));

        foreach (var todo in selected)
        {
            toDoManager.MarkToDoAsCompleted(todo.Id);
        }
    }
}
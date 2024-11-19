using Figgle;
using Spectre.Console;
using ToDoListApp.Classes;

public class ToDoMenuHandlers
{

    public void AddNewToDo(ToDoListManager toDoManager)
    {
        var title = AnsiConsole.Ask<string>("Enter the [lightseagreen]title[/]:");
        var description = AnsiConsole.Ask<string>("Enter the [lightseagreen]description[/]:");
        toDoManager.AddToDo(title, description);
    }

    public void DeleteToDoItem(ToDoListManager toDoManager)
    {
        var todos = toDoManager.GetAllToDos();

        if (todos.Count == 0)
        {
            AnsiConsole.MarkupLine("[gold3_1]No To-Dos available to delete.[/]");
            return;
        }

        var selectedToDo = AnsiConsole.Prompt(
            new SelectionPrompt<ToDoItem>()
                .Title("[gold3_1]Select a To-Do to delete:[/]")
                .AddChoices(todos)
                .HighlightStyle(new Style(Color.LightSeaGreen))
                .UseConverter(todo => $"[pink3 italic]{todo.Id}: {todo.Title}[/]")
        );

        if (!toDoManager.DeleteToDo(selectedToDo.Id))
        {
            AnsiConsole.MarkupLine($"[red]To-Do with ID {selectedToDo.Id} could not be deleted. Validation failed or item not found.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[lightseagreen]To-Do {selectedToDo.Title} deleted successfully.[/]");
        }
    }

    public void MarkToDosAsComplete(ToDoListManager toDoManager)
    {
        var todos = toDoManager.GetAllToDos();

        var incompleteTodos = todos.Where(todo => !todo.IsCompleted).ToList();

        if (incompleteTodos.Count == 0)
        {
            AnsiConsole.MarkupLine("[lightseagreen]No To-Dos available to mark as complete.[/]");
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

    public void ShowToDoList(ToDoListManager toDoManager)
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

    public void UpdateToDoItem(ToDoListManager toDoManager)
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
}
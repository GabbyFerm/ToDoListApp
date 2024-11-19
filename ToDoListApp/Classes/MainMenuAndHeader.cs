using Figgle;
using Spectre.Console;

public static class MainMenuAndHeader
{
    public static void ShowHeader()
    {
        var header = FiggleFonts.Big.Render("To-Do List");
        AnsiConsole.Markup($"[bold pink3]{header}[/]");
    }

    public static string ShowMenuAndHeader()
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

        return AnsiConsole.Prompt(menuSelection);
    }
}
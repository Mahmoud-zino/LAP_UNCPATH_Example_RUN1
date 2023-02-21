using Spectre.Console;
using UNCPath.AccessLayer.Models;
using UNCPath.BusinessLayer.Repositories;
using UNCPath.Presentation.Console;

UNCRepo uncRepo = new();
List<Folder> folders = new();
Folder? selectedFolder;

//to always display the menu until exit is chosen
while(true)
{
    int firstLevelSelection = ConsoleManager.PrintTopLevelMenu();

    switch(firstLevelSelection)
    {
        //Scan new Path Command
        case 0:
            string path = ConsoleManager.ScanNewPath();
            //Loading spinner
            await AnsiConsole.Status()
                .Start("Scanning paths...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    await uncRepo.ScanAndSavePath(path);
                });
            break;
        //Get Path Statistics
        case 1:
            //Loading spinner
            await AnsiConsole.Status()
                .Start("Getting paths...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    folders = await uncRepo.GetFolders();
                });
            //display only root folders
            folders = folders.Where(f => f.RootFolder).ToList();

            int selection = ConsoleManager.PrintAvailableRootPaths(folders);
            //Back Command was selected
            if (selection == -1) break;

            selectedFolder = folders[selection];

            int statisticSelection = ConsoleManager.PrintStatisticsMenu(selectedFolder.Path);

            HandleStatisticSelection(statisticSelection);
            break;
        //Exit
        case 2:
            return;
    }
}

void HandleStatisticSelection(int seleciton)
{
    if (selectedFolder == null) return;
    switch(seleciton)
    {
        //top month
        case 0:
            ConsoleManager.PrintTopMonth(selectedFolder.Path, uncRepo.GetTopMonth(selectedFolder));
            Console.WriteLine("\n\npress Enter to go back");
            Console.ReadLine();
            break;
        //bar diagram
        case 1:
            ConsoleManager.PrintTop10FoldersBarDiagram(selectedFolder.Path, uncRepo.GetTop10FoldersAfterSize(selectedFolder));
            Console.WriteLine("\n\npress Enter to go back");
            Console.ReadLine();
            break;
        //tree structure
        case 2:
            ConsoleManager.PrintFolderTreeStructure(selectedFolder);
            Console.WriteLine("\n\npress Enter to go back");
            Console.ReadLine();
            break;
    }
}
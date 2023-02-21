using Spectre.Console;
using System.IO;
using UNCPath.AccessLayer.Models;

namespace UNCPath.Presentation.Console
{
    internal static class ConsoleManager
    {
        public static void PrintTitle()
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup(@"[blue]
  _    _ _   _  _____   _____      _   _        _____                                 
 | |  | | \ | |/ ____| |  __ \    | | | |      / ____|                                
 | |  | |  \| | |      | |__) |_ _| |_| |__   | (___   ___ __ _ _ __  _ __   ___ _ __ 
 | |  | | . ` | |      |  ___/ _` | __| '_ \   \___ \ / __/ _` | '_ \| '_ \ / _ \ '__|
 | |__| | |\  | |____  | |  | (_| | |_| | | |  ____) | (_| (_| | | | | | | |  __/ |   
  \____/|_| \_|\_____| |_|   \__,_|\__|_| |_| |_____/ \___\__,_|_| |_|_| |_|\___|_|   
                                                                                      
                                                                                      
[/]");
        }

        private static readonly string[] FIRST_LEVEL_OPTIONS = new[] { "Scan new Path",
                                                                       "Get Path Statistics", 
                                                                       "Exit"};

        /// <summary>
        /// Displays Top Level Selectable options
        /// </summary>
        /// <returns>index of selected Option</returns>
        public static int PrintTopLevelMenu()
        {
            PrintTitle();
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(FIRST_LEVEL_OPTIONS));

            return FIRST_LEVEL_OPTIONS.ToList().IndexOf(selection);
        }

        /// <summary>
        /// Takes User Input to scan the path
        /// </summary>
        /// <returns>User Input Path</returns>
        public static string ScanNewPath()
        {
            PrintTitle();
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Please Enter the full path to the folder you want to scan:")
                .PromptStyle("blue")
                .ValidationErrorMessage("[red]Invalid Path! please make sure to enter a valid path[/]")
                .Validate(path =>
                {
                    if (string.IsNullOrEmpty(path))
                        return ValidationResult.Error("[red]Empty Path! please make sure to enter a path to scan.[/]");
                    else if (!Directory.Exists(path))
                        return ValidationResult.Error("[red]Invalid Path! please make sure to enter a valid path[/]");
                    else return ValidationResult.Success();
                }));
        }

        /// <summary>
        /// Prints all available root paths
        /// </summary>
        /// <param name="folders">Database Folders list</param>
        /// <returns>user selected Path or -1 for Back Command</returns>
        public static int PrintAvailableRootPaths(List<Folder> folders)
        {
            PrintTitle();

            List<string> options = folders.Select(f => f.Path).ToList();
            options.Add("Back");

            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select a path to display it\'s statistics")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more paths)[/]")
                .AddChoices(options));

            if (selection == "Back") return -1;

            return options.ToList().IndexOf(selection);
        } 


        private static readonly string[] STATISTICS_OPTIONS = new[] { "Find top month (size + files created)",
                                                                       "Display top 10 Folders diagram",
                                                                       "Display Structur tree view",
                                                                       "Back"};
        /// <summary>
        /// Print Statistics Menu Options
        /// </summary>
        /// <param name="path">Selected Path</param>
        /// <returns>user selected option index</returns>
        public static int PrintStatisticsMenu(string path)
        {
            PrintTitle();
            AnsiConsole.Markup("[green]<<" + path + ">>[/]\n\n");

            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(STATISTICS_OPTIONS));

            return STATISTICS_OPTIONS.ToList().IndexOf(selection);
        }

        /// <summary>
        /// Print Top Month statement
        /// </summary>
        /// <param name="path">Selected Folder Path</param>
        /// <param name="month">Month To display</param>
        public static void PrintTopMonth(string path, string month)
        {
            PrintTitle();
            AnsiConsole.Markup("[green]<<" + path + ">>[/]\n\n");

            AnsiConsole.Markup("Top Month (size + files created): [green]" + month + "[/]");
        }

        /// <summary>
        /// Print Top 10 Folders Bar Diagram
        /// </summary>
        /// <param name="path">Selected Folder Path</param>
        /// <param name="folders">Top 10 Folders List</param>
        public static void PrintTop10FoldersBarDiagram(string path, List<Folder> folders)
        {
            PrintTitle();
            AnsiConsole.Markup("[green]<<" + path + ">>[/]\n\n");

            int index = 0;
            AnsiConsole.Write(new BarChart()
                .Width(60)
                .Label("[blue underline] Top 10 Folders Sizes[/]")
                .CenterLabel()
                .AddItems(folders, (folder) => new BarChartItem(
                    folder.Name, folder.Size, ++index % 2 == 0 ? Color.White : Color.Blue)));
        }

        /// <summary>
        /// Print Folder Tree Structure
        /// </summary>
        /// <param name="folder">root Folder to display</param>
        public static void PrintFolderTreeStructure(Folder folder)
        {
            PrintTitle();
            AnsiConsole.Markup("[green]<<" + folder.Path + ">>[/]\n\n");

            Tree root = new(folder.Name);
            TreeNode emptyNode = root.AddNode("");

            AddNodesToFolder(emptyNode, folder);

            // Render the tree
            AnsiConsole.Write(root);
        }

        /// <summary>
        /// Add Tree Nodes to a Folder
        /// </summary>
        /// <param name="root">Root NodeTree element</param>
        /// <param name="folder">current folder</param>
        private static void AddNodesToFolder(TreeNode root,Folder folder)
        {
            foreach (AccessLayer.Models.File file in folder.Files)
            {
                root.AddNode(file.Name);
            }

            foreach (Folder subFolder in folder.Folders)
            {
                var subRoot = root.AddNode(subFolder.Name);
                AddNodesToFolder(subRoot, subFolder);
            }
        }
    }
}

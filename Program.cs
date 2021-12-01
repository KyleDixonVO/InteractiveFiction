using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace InteractiveFiction
{
    class Program
    {
        static string[] story; //stores story pages
        static string[] sentences; //used to split story pages
        static ConsoleKey playerInput; //stores char of player input
        static int choiceA; //stores the page number of the first choice
        static int choiceB; //stores the page number of the second choice
        static bool gameOver = false; //checks if the game should end
        static char delimiter = ':'; //where story pages are split
        static string[] keyWords = new string[3]; //for coloring the text
        static int page; //defaults page number to zero
        static int optionSelected = 0;
        static bool waitingForEnter = false;
        static string useablePath;

        //Some story options will utilize inventory items/tracked gamestate events (to be implemented later).
        //I plan to make my story require multiple loops through the choices, where some options are only available after certain conditions are met.



        static void PlayerActions() //gets the key pressed by the user and performs a task if the input is valid.
        {
            Console.WriteLine("");
            Console.WriteLine("Awaiting choice. A/B: Story Options. C: Save Game. D: Load Game. Esc: Exit Game.");
            playerInput = Console.ReadKey(true).Key;

            if (playerInput == ConsoleKey.A)
            {
                page = choiceA;
                Console.Beep(625, 100);
            }
            else if (playerInput == ConsoleKey.B)
            {
                page = choiceB;
                Console.Beep(525, 100);
            }
            else if (playerInput == ConsoleKey.C)
            {
                SaveGame();
            }
            else if (playerInput == ConsoleKey.D)
            {
                LoadGame();
            }
            else if (playerInput == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input, please choose from a valid input");
                PlayerActions();
            }

        }
        static void GetPageNumbers() //splits story pages at the delimter, parses page numbers into ints to be used in the player input method.
        {
            Console.WriteLine();
            Console.WriteLine("Page: " + (page));
            sentences = story[page].Split(delimiter); //splits the current story page at every colon and stores it in the sentences array
            string testA = sentences[sentences.Length - 2]; //stores the value of the second last sentence as a string to be parsed
            bool isIntA = int.TryParse(testA, out int hasIntA);
            if (isIntA == true)
            {
                choiceA = int.Parse(testA); //parses the second last string into int
            }
            else
            {
                gameOver = true;
            }
            string testB = sentences[sentences.Length - 1]; //stores the value of the last sentence as a string to be parsed
            bool isIntB = int.TryParse(testB, out int hasIntB);
            if (isIntB == true)
            {
                choiceB = int.Parse(testB); //parses the last string into int
            }
            else
            {
                gameOver = true;
            }
        }
        static void TextManager() //Checks sentences against a set of keywords to determine if they should receive color, then writes sentences.
        {
            keyWords[0] = "#A";
            keyWords[1] = "#B";
            keyWords[2] = "#A or B";
            for (int i = 0; i < sentences.Length-2; i++) //checking each line in the array lines[]
            {
                foreach (string keyWord in keyWords) //comparing the line against each keyword in keywords
                {
                    bool containsKeyword = sentences[i].Contains(keyWord); //the check
                    if (containsKeyword == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green; //sets text color to green
                        break;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                }
                Console.WriteLine(sentences[i]);
                Console.ResetColor();
            }
        }
        static void MainMenu() //Splash screen with simple cursor input.
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();
            TitleText();

            Console.WriteLine("     New Game");
            Console.WriteLine("     Load Game");
            Console.WriteLine("     Quit Game");
            Console.SetCursorPosition(2, 11);
            Console.Write("->");


            while (waitingForEnter == false)
            {
                Console.CursorVisible = false;
                ConsoleKey moveCursor = Console.ReadKey(true).Key;
                if (moveCursor == ConsoleKey.W)
                {
                    if (optionSelected > 0)
                    {
                        Console.Beep(625, 100);
                        optionSelected--;
                    }
                }
                else if (moveCursor == ConsoleKey.S)
                {
                    if (optionSelected < 2)
                    {
                        Console.Beep(525, 100);
                        optionSelected++;
                    }
                }

                if (optionSelected == 0)
                {
                    Console.SetCursorPosition(2, 12);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 11);
                    Console.Write("->");
                }
                else if (optionSelected == 1)
                {
                    Console.SetCursorPosition(2, 11);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 13);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 12);
                    Console.Write("->");
                }
                else if (optionSelected == 2)
                {
                    Console.SetCursorPosition(2, 12);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 13);
                    Console.Write("->");
                }

                ConsoleKey enterPress = moveCursor;
                if(enterPress == ConsoleKey.Enter)
                { 
                    if (optionSelected == 0)
                    {
                        break;
                    }
                    else if (optionSelected == 1)
                    {
                        Console.ResetColor();
                        Console.Clear();
                        LoadGame();
                        break;
                    }
                    else if (optionSelected == 2)
                    {
                        gameOver = true;
                        break;
                    }
                }
            }
            Console.ResetColor();
            Console.Clear();
        }
        static void GameOver() //Displays game over message and ends the game.
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over");
            Console.ResetColor();
            Console.ReadKey(true);
            MainMenu();
        }
        static void Main() //Main game loop.
        {
            GetFilePath();
            MainMenu();
            while (gameOver == false) //gameplay loop, gets page data, parses the page numbers from the data, writes the relevant text to the screen, then waits for player input.
            {
                ReadStoryTxt();
                GetPageNumbers();
                TextManager();
                PlayerActions();
            }
            GameOver();
        }
        static void TitleText()
        {
            Console.WriteLine("");
            Console.WriteLine("▄█     █▄     ▄█    █▄     ▄█     ▄████████    ▄███████▄    ▄████████    ▄████████    ▄████████");
            Console.WriteLine("███     ███   ███    ███   ███    ███    ███   ███    ███   ███    ███   ███    ███   ███    ███ ");
            Console.WriteLine("███     ███   ███    ███   ███▌   ███    █▀    ███    ███   ███    █▀    ███    ███   ███    █▀ ");
            Console.WriteLine("███     ███  ▄███▄▄▄▄███▄▄ ███▌   ███          ███    ███  ▄███▄▄▄      ▄███▄▄▄▄██▀   ███        ");
            Console.WriteLine("███     ███ ▀▀███▀▀▀▀███▀  ███▌ ▀███████████ ▀█████████▀  ▀▀███▀▀▀     ▀▀███▀▀▀▀▀   ▀███████████ ");
            Console.WriteLine("███     ███   ███    ███   ███           ███   ███          ███    █▄  ▀███████████          ███");
            Console.WriteLine("███ ▄█▄ ███   ███    ███   ███     ▄█    ███   ███          ███    ███   ███    ███    ▄█    ███");
            Console.WriteLine(" ▀███▀███▀    ███    █▀    █▀    ▄████████▀   ▄████▀        ██████████   ███    ███  ▄████████▀");
            Console.WriteLine("                                                                         ███    ███             ");
            Console.WriteLine("");
        } //Holds ASCII art for the main menu.
        static void ReadStoryTxt()
        {
            string storyTXT = "InteractiveFiction\\story.txt";
            if (!File.Exists(@useablePath + storyTXT))
            {
                Console.WriteLine("story.txt cannot be found @" + useablePath + storyTXT + ". Please ensure story.txt has not been moved or renamed.");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
            else
            {
                story = File.ReadLines(@useablePath + storyTXT).Skip(4).ToArray();
            }    
        } //Reads pages from story.txt.
        static void LoadGame()
        {
            Console.WriteLine("Load Game? Any unsaved progress will be lost. Y/N");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {

                string[] savePoint = File.ReadAllLines(@useablePath + "InteractiveFiction\\savegame.txt").ToArray();
                bool containsInt = int.TryParse(savePoint[1], out int savedPage);
                if (containsInt == true)
                {
                    page = int.Parse(savePoint[1]);
                    Console.WriteLine("Loaded Game!");
                    Console.ReadKey(true);
                }
                else
                {
                    Console.WriteLine("Cannot find page number in savegame.txt");
                    Console.ReadKey(true);
                    Main();
                }
            }
            else if (input == ConsoleKey.N)
            {
                PlayerActions();
            }
            else
            {
                Console.WriteLine("Invalid input, input must be 'Y' or 'N'");
                LoadGame();
            } 
        } //Loads a previous saved game from savegame.txt.
        static void SaveGame()
        {
            Console.WriteLine("Save Game? This will overwrite current save. Y/N");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {
                string SavePoint = @useablePath + "InteractiveFiction\\savegame.txt";

                
                string[] SaveData = { "Whispers Save Data", Convert.ToString(page) };
                File.WriteAllLines(SavePoint, SaveData);

                
                Console.WriteLine("Saved Game!");
                Console.ReadKey(true);
            }
            else if (input == ConsoleKey.N)
            {
                PlayerActions();
            }
            else
            {
                Console.WriteLine("Invalid input, input must be 'Y' or 'N'");
                SaveGame();
            }  
        } //Creates or overwrites savegame.txt with save data.
        static void GetFilePath()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;
            string[] subdirectories = filePath.Split('\\');
            foreach (string directory in subdirectories)
            {
                if (directory.Contains("InteractiveFiction"))
                {
                    break;
                }
                else
                {
                    useablePath = useablePath + directory + "\\";
                }
            }
        } //Gets local file path for SaveGame, LoadGame, and ReadStoryText.
    }
}

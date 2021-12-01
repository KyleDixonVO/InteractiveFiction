using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        static int page = 1; //defaults page number to one
        static int optionSelected = 0; //default main menu option
        static bool waitingForEnter = false; //used to confirm main menu input
        static string useablePath; //stores the path of the game folder
        static string testA; //stores second last sentence in split
        static bool isIntA; //used to test if second last split contains an integer
        static bool isIntB; //used to test if last split contains an integer

        static void PlayerActions() //gets the key pressed by the user and performs a task if the input is valid.
        {
           if (isIntA && isIntB)
           {
                if(choiceB == choiceA)
                {
                    QuickMenuColor();
                    Console.WriteLine("");
                    Console.WriteLine("Awaiting Choice. C: Save Game. D: Load Game. Esc: Exit Game. Any other key: Continue.");
                    Console.ResetColor();
                    playerInput = Console.ReadKey(true).Key;

                    if(playerInput == ConsoleKey.C)
                    {
                        Console.Beep(575, 100);
                        SaveGame();
                    }    
                    else if(playerInput == ConsoleKey.D)
                    {
                        Console.Beep(575, 100);
                        LoadGame();
                    }
                    else if(playerInput == ConsoleKey.Escape)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.Beep(575, 100);
                        page = choiceA;
                    }

                }
                else
                {
                    QuickMenuColor();
                    Console.WriteLine("");
                    Console.WriteLine("Awaiting choice. A: First Choice. B: Second Choice. C: Save Game. D: Load Game. Esc: Exit Game.");
                    Console.ResetColor();
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
                        Console.Beep(575, 100);
                        SaveGame();
                    }
                    else if (playerInput == ConsoleKey.D)
                    {
                        Console.Beep(575, 100);
                        LoadGame();
                    }
                    else if (playerInput == ConsoleKey.Escape)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input, please choose from a valid input");
                        Console.ResetColor();
                        PlayerActions();
                    }
                }
                
           }
           else
           {
                QuickMenuColor();
                Console.WriteLine("Press Any Key to Continue");
                Console.ResetColor();
                Console.ReadKey(true);
                Console.Beep(575, 100);
                gameOver = true;
           }
           

        }
        static void GetPageNumbers() //splits story pages at the delimter, parses page numbers into ints to be used in the player input method.
        {   
            try
            {
             sentences = story[page].Split(delimiter); //splits the current story page at every colon and stores it in the sentences array
            }
            catch
            {
              page++;
            }
           
            try
            {
            testA = sentences[sentences.Length - 2]; //stores the value of the second last sentence as a string to be parsed
            }
            catch
            {
                isIntA = false;
                return;
            }
            isIntA = int.TryParse(testA, out int hasIntA);
            if (isIntA == true)
            {
                choiceA = int.Parse(testA); //parses the second last string into int
            }
            string testB = sentences[sentences.Length - 1]; //stores the value of the last sentence as a string to be parsed
            isIntB = int.TryParse(testB, out int hasIntB);
            if (isIntB == true)
            {
                choiceB = int.Parse(testB); //parses the last string into int
            }
        }
        static void TextManager() //writes story text and manages text color
        {
            Console.Clear();
            if (page != 0) //checks if the title page is up, if not, writes page number
            {
                Console.WriteLine("");
                QuickMenuColor();
                Console.WriteLine("Page: " + (page));
                Console.ResetColor();
                Console.WriteLine("");
            }

            if(!(isIntA == true && isIntB == true))
            {
                for (int k = 0; k < sentences.Length - 2; k++)
                {
                    Console.WriteLine(sentences[k]);
                    Thread.Sleep(100);
                }
                Console.WriteLine("");
            }
            else if(choiceA == choiceB) //checks if there is only one option available
            { 
                 for (int i = 0; i < sentences.Length-3; i++) //writes all sentences up to the third from last (only choice)
                 {
                    Console.WriteLine(sentences[i]);
                    Thread.Sleep(100);
                 }

                Console.WriteLine(""); //changes text color and writes third from last sentence (only choice)
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(sentences[sentences.Length-3]);
                Console.ResetColor();
            }
            else
            {
                for (int j = 0; j < sentences.Length-4; j++) //writes all sentences up to fourth from last (first choice)
                {
                    Console.WriteLine(sentences[j]);
                    Thread.Sleep(100);
                }
                Console.WriteLine(""); //writes last two sentences after changing color
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("First Choice: " + sentences[sentences.Length-4]);
                Console.ResetColor();
                Thread.Sleep(100);
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Second Choice: " + sentences[sentences.Length-3]);
                Console.ResetColor();
                Thread.Sleep(100);
            }
           
        }
        static void MainMenu() //Splash screen with simple cursor input.
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta; //Changes background color
            Console.Clear();
            TitleText();
            Console.WriteLine("Shotty Game Studios\n--------------------");

            Console.WriteLine("     New Game");  //available game options
            Console.WriteLine("     Load Game");
            Console.WriteLine("     Quit Game");
            Console.SetCursorPosition(2, 13); //setting cursor position behind "new game"
            Console.Write("->");


            while (waitingForEnter == false) //checking if enter has been pressed
            {
                Console.CursorVisible = false; //sets cursor false
                ConsoleKey moveCursor = Console.ReadKey(true).Key; //gets key pressed
                if (moveCursor == ConsoleKey.W) //moves cursor up on "W" press, plays beep
                {
                    if (optionSelected > 0)
                    {
                        Console.Beep(625, 100);
                        optionSelected--;
                    }
                }
                else if (moveCursor == ConsoleKey.S) //moves cursor down on "S" press, plays beep
                {
                    if (optionSelected < 2)
                    {
                        Console.Beep(525, 100);
                        optionSelected++;
                    }
                }

                if (optionSelected == 0) //defines cursor positions for the three available options
                {
                    Console.SetCursorPosition(2, 14);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 13);
                    Console.Write("->");
                }
                else if (optionSelected == 1)
                {
                    Console.SetCursorPosition(2, 13);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 14);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 15);
                    Console.Write("->");
                }
                else if (optionSelected == 2)
                {
                    Console.SetCursorPosition(2, 14);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 15);
                    Console.Write("->");
                }

                ConsoleKey enterPress = moveCursor;
                if(enterPress == ConsoleKey.Enter) //when enter is pressed, executes highlighted option, plays beep
                { 
                    Console.Beep(575, 100);
                    if (optionSelected == 0) //starts new game
                    {
                        break;
                    }
                    else if (optionSelected == 1) //goes to load menu
                    {
                        Console.ResetColor();
                        Console.Clear();
                        LoadGame();
                        break;
                    }
                    else if (optionSelected == 2) //closes program
                    {
                        gameOver = true;
                        break;
                    }
                }
            }
            Console.ResetColor(); //resets color and clears screen
            Console.Clear();
        }
        static void GameOver() //Displays game over message and exits to main menu.
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
            while (gameOver == false) //gameplay loop: gets page data, parses the page numbers from the data, writes the relevant text to the screen, then waits for player input.
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
        static void QuickMenuColor() //Sets foreground color to green
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}

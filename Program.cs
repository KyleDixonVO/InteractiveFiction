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
        static char playerInput; //stores char of player input
        static int choiceA; //stores the page number of the first choice
        static int choiceB; //stores the page number of the second choice
        static bool gameOver = false; //checks if the game should end
        static char delimiter = ':'; //where story pages are split
        static string[] keyWords = new string[3]; //for coloring the text
        static string[] lines; //used to split sentences
        static int page; //defaults page number to zero

        //Some story options will utilize inventory items/tracked gamestate events (to be implemented later).
        //I plan to make my story require multiple loops through the choices, where some options are only available after certain conditions are met.



        static void PlayerActions() //gets the key pressed by the user and performs a task if the input is valid.
        {
            Console.WriteLine("");
            Console.WriteLine("Awaiting choice. A/B: Story Options. C: Save Game. D: Load Game.");
            playerInput = Console.ReadKey(true).KeyChar;

            if (playerInput == 'a' || playerInput == 'A')
            {
                page = choiceA;
                Console.Beep(625, 100);
            }
            else if (playerInput == 'b' || playerInput == 'B')
            {
                page = choiceB;
                Console.Beep(525, 100);
            }
            else if (playerInput == 'c' || playerInput == 'C')
            {
                SaveGame();
            }
            else if (playerInput == 'd' || playerInput == 'D')
            {
                LoadGame();
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

        static void Scribe() //writes the appropriate story page, needs to be reworked to properly display color
        {
            for (int i = 0; i < (sentences.Length - 2); i++)
            {
                Console.WriteLine(sentences[i]);
            }
        }

        static void ChangeTextColor() //method to color individual lines based on keywords (work in progress)
        {
            lines = sentences[0].Split('\n'); //splitting the sentence at each new line
            keyWords[0] = "#A";
            keyWords[1] = "#B";
            keyWords[2] = "#A or B";
            foreach (string line in lines) //checking each line in the array lines[]
            {
                string scraper = line;
                foreach (string keyWord in keyWords) //comparing the line against each keyword in keywords
                {
                    bool containsKeyword = scraper.Contains(keyWord); //the check
                    if (containsKeyword == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Green; //sets text color to green
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                }
                Console.WriteLine(line);
            }
        }

        static void MainMenu() //simple main menu, want to add load and save functionality later
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();
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
            Console.WriteLine();
            Console.WriteLine("Press any key to start.");
            Console.ReadKey(true);
            Console.ResetColor();
            Console.Clear();
        }

        static void EndGame() //Displays game over message and ends the game.
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over!");
            Console.ResetColor();
            Console.ReadKey(true);
        }
        static void Main()
        {
            MainMenu();
            while (gameOver == false) //gameplay loop, gets page data, parses the page numbers from the data, writes the relevant text to the screen, then waits for player input.
            {
                ReadStoryTxt();
                GetPageNumbers();
                //ChangeTextColor();
                Scribe();
                PlayerActions();
            }
            EndGame();
        }

        static void ReadStoryTxt()
        {
            story = File.ReadLines(@"C:\Users\Kyle\Desktop\GitHub\InteractiveFiction\story.txt").Skip(4).ToArray();
        }

        static void LoadGame()
        {
            Console.WriteLine("Load Game? Any unsaved progress will be lost. Y/N");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {

                string[] savePoint = File.ReadAllLines(@"C:\Users\Kyle\Desktop\GitHub\InteractiveFiction\savegame.txt").ToArray();
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
        }

        static void SaveGame()
        {
            Console.WriteLine("Save Game? This will overwrite current save. Y/N");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {
                string SavePoint = @"C:\Users\Kyle\Desktop\GitHub\InteractiveFiction\savegame.txt";

                
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
        }
    }
}

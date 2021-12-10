using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

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
        static string storyTXT = "story.txt";
        static string defaultStoryHash = "644d13591ae3c5acd514037049ad8b700b205ee6ac38356c752b30cf3d164161";
        static string sgHash;
        static StringBuilder sb = new StringBuilder();
        static StringBuilder hashBuilder = new StringBuilder();
        static string lastSaveHash;
        static StringBuilder newSaveHash = new StringBuilder();

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
                GameOver();
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
                if (choiceA > story.Length || choiceA < 0)
                {
                    Console.WriteLine("Page "+ page +" reference numbers are invalid, please check the integrity of story.txt");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
            }
            string testB = sentences[sentences.Length - 1]; //stores the value of the last sentence as a string to be parsed
            isIntB = int.TryParse(testB, out int hasIntB);
            if (isIntB == true)
            {
                choiceB = int.Parse(testB); //parses the last string into int
                if (choiceB > story.Length || choiceB < 0)
                {
                    Console.WriteLine("Page "+ page +" reference numbers are invalid, please check the integrity of story.txt");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
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
            gameOver = false;
            optionSelected = 0;
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
                    Console.SetCursorPosition(2, 15);
                    Console.Write("  ");
                    Console.SetCursorPosition(2, 14);
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
                        Console.ResetColor();
                        Console.Clear();
                        page = 1;
                        GameplayLoop();
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
                        Environment.Exit(0);
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

        static void Main()
        { 
            GetHash();
            VerifyFileIntegrity();
            ReadStoryTxt();
            MainMenu();
            GameplayLoop();
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
            if (!File.Exists(storyTXT))
            {
                Console.WriteLine("story.txt cannot be found @"+storyTXT+". Please ensure story.txt has not been moved or renamed.");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
            else
            {
                story = File.ReadLines(storyTXT).Skip(4).Where(line => line != "").ToArray();
            }    
        } //Reads pages from story.txt.

        static void LoadGame()
        {
            Console.WriteLine("Load Game? Any unsaved progress will be lost. Y/N");
            ConsoleKey input = Console.ReadKey(true).Key;
            if (input == ConsoleKey.Y)
            {
                if (File.Exists("savegame.txt"))
                {
                    string[] savePoint = File.ReadAllLines("savegame.txt").Where(line => line != "").ToArray();
                    if (savePoint.Length < 3)
                    {
                        Console.WriteLine("savegame.txt is missing data, ensure save file has not been modified.");
                        Console.ReadKey(true);
                        MainMenu();
                    }
                    
                    
                        bool containsInt = int.TryParse(savePoint[1], out int savedPage);
                        if (containsInt == true)
                        {
                            page = int.Parse(savePoint[1]);

                            if (page >= story.Length || page < 0)
                            {
                                Console.WriteLine("savegame.txt contains corrupted data, please check file integrity or delete savegame.txt");
                                Console.ReadKey(true);
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("Loaded Game!");
                                Console.ReadKey(true);
                                GameplayLoop();
                            }     
                        }
                        else
                        {
                            Console.WriteLine("Cannot find page number in savegame.txt, ensure save file has not been modified.");
                            Console.ReadKey(true);
                            MainMenu();
                        }
                    
                }
                else
                {
                    Console.WriteLine("savegame.txt cannot be found, ensure location has not been modified.");
                    Console.ReadKey(true);
                    MainMenu();
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
                string SavePoint = "savegame.txt";

                MakeSaveHash();
                string[] SaveData = new string [] { "Whispers Save Data", Convert.ToString(page), sgHash};
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

        static void GameplayLoop() //Stores gameloop
        {
            while (gameOver == false) //gameplay loop: gets page data, parses the page numbers from the data, writes the relevant text to the screen, then waits for player input.
            {
                ReadStoryTxt();
                GetPageNumbers();
                TextManager();
                PlayerActions();
            }  
        }

        static void VerifyFileIntegrity()
        {
            Console.WriteLine("\nVerifying story.txt integrity.");
            if (!File.Exists(storyTXT))
            {
                Console.WriteLine("\nstory.txt cannot be found, ensure file location has not been modified.");
            }
            else if (defaultStoryHash != hashBuilder.ToString())
            {
                Console.WriteLine("\nThe contents of story.txt have been modified, this may impact the stability of this application.");
            }
            else
            {
                Console.WriteLine("\nstory.txt OK.");
            }
            
            Console.WriteLine("\nVerifying savegame.txt integrity."); 
            if (!File.Exists("savegame.txt"))
            {
                Console.WriteLine("\nsavegame.txt cannot be found, ensure file location has not been modified.");
            }
            else if (lastSaveHash != newSaveHash.ToString())
            {
                Console.WriteLine("\nThe contents of savegame.txt have been modified, or no save exists.");
            }
            else
            {
                Console.WriteLine("\nsavegame.txt OK.");
            }

            Console.ReadKey(true);
        } //checks story.txt and savegame.txt integrity (File exists/file has been modified)

        static void GetHash() //generates hashes from file data and compares it to existing or previously generated hashes
        {
            if(File.Exists(storyTXT))
            {
                string[] storyFilePlainText = File.ReadLines(storyTXT).ToArray(); //gets plain text from story.txt, including blank lines.
                foreach (string line in storyFilePlainText)
                {
                    sb.Append(line);                                             //appends all lines together to form one long sequence.
                }                                                                
                var sha = new SHA256Managed();                                  //declaring a new sha256 hash manager
                var allText = sb.ToString();                                    //converts long sequence into a string
                var allTextBytes = Encoding.ASCII.GetBytes(allText);            //converts string into bytes by using ascii
                var storyHash = sha.ComputeHash(allTextBytes);                  //generates hash characters from each byte
                foreach(byte b in storyHash)
                {
                    hashBuilder.Append(b.ToString("x2"));                      //appends all bytes together to form a hash
                }
            }
            
            if(File.Exists("savegame.txt"))
            {
                string[] saveGamePlainText = File.ReadLines("savegame.txt").ToArray(); //gets plain text from savegame.txt, including blank lines.
                StringBuilder sb3 = new StringBuilder();                              //declaring a new stringbuilder to manage the sequence
                for (int l = 0; l <= saveGamePlainText.Length-2; l++)       
                {
                    sb3.Append(saveGamePlainText[l]);                                //forming the sequence from every line except the last one
                }

                lastSaveHash = saveGamePlainText[saveGamePlainText.Length-1];       //grabbing the last line, this will be a previously generated hash if the user has saved before

                var sha2 = new SHA256Managed();                                     //new sha256 manager
                var saveText = sb3.ToString();                                     //sequence to string
                var saveTextBytes = Encoding.ASCII.GetBytes(saveText);             //converting to bytes with ascii
                var tempHash = sha2.ComputeHash(saveTextBytes);                    //generating hash characters
                foreach (byte c in tempHash)
                {
                    newSaveHash.Append(c.ToString("x2"));                          //forming hash
                }

                
                //the code below is for debugging and should be disabled by default

                //Console.WriteLine(lastSaveHash);
                //Console.WriteLine(newSaveHash.ToString());
                //Console.WriteLine(hashBuilder.ToString());
                //Console.ReadKey(true);
            }
            
        } //Reads all text in story.txt and makes a hash, then compares it against the default hash. Reads the first two lines in savegame.txt, and converts them to hash, then compares that hash to last line: the hash generated when the player saved last.

        static void MakeSaveHash() //Makes a new hash to write to savegame.txt, see GetHash() for code explanation
        {
            var sha3 = new SHA256Managed();
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("Whispers Save Data");   //no loop, this is not generated from file data, instead the hasher uses what the first two lines of the save will be to generate this hash.
            sb2.Append(Convert.ToString(page));
            var toConvert = sb2.ToString();
            var toBytes = Encoding.ASCII.GetBytes(toConvert);
            var tempHash = sha3.ComputeHash(toBytes);
            StringBuilder outGoingHash = new StringBuilder();
            foreach (byte d in tempHash)
            {
                outGoingHash.Append(d.ToString("x2"));
            }

            sgHash = outGoingHash.ToString();
        }
    }
}

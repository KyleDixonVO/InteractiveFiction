using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace InteractiveFiction
{
    class Program
    {
        static string[] story = new string[64]; //stores story pages
        static string[] sentences; //used to split story pages
        static char playerInput; //stores char of player input
        static int choiceA; //stores the page number of the first choice
        static int choiceB; //stores the page number of the second choice
        static bool gameOver = false; //checks if the game should end
        static char delimiter = ':'; //where story pages are split
        static string[] keyWords = new string[3]; //for coloring the text
        static string[] lines; //used to split sentences
        static int page = 0; //defaults page number to zero
        



        static void PageList() //list of all story pages, will be moved to a file later.
        {
            story[0] = "Shotty Game Studios\nWhispers\n--------------------\n#A or B. Start Game.:1:1";
            story[1] = "You wake on a damp bed of leaves. Your head pounds, how did you get here?\nTo the north you see a satchel on the ground.To the south you see a rope leading over the edge of a cliff.\n#A. Go North\n#B. Go South:2:3";
            story[2] = "You walk over to the bag, it smells of potent rot,the leather is slimy.\nThe bag pulses slightly.\n#A. Take The Bag?\n#B.Leave The Bag? :4:5";
            story[3] = "You walk to the edge of the cliff and grab the rope, you descend to the base of the cliff.\n A or B. Descend.:8:8";
            story[4] = "You pick up the bag, it is warm to the touch, the leather ripples under your fingertips. You go to open the bag, but are met with a roiling sense of unease. Maybe the bag should stay closed.\n#A or B. Go to cliff edge.:3:3";
            story[5] = "You decide to leave the bag, it’s gross and you’re pretty sure it’s not yours. You turn to leave, and you begin to hear whispers behind you. /Don’t leave us./\n#A. Turn Around.\n#B. Run.:6:7";
            story[6] = "You turn around to investigate the source of the whispers. You don’t see anyone. The whispers grow louder, your head starts to hurt… A familiar bag rests at your side.\n#A or B. Go to cliff edge.:3:3";
            story[7] = "You run from the whispers, you run to the edge of the cliff and grab the rope. You descend in a hurry.\n#A or B. Descend.:8:8";
            story[8] = "The forest is thick here, mist hangs close to the ground. In the distance you hear a low hum.\n#A. To Be Implemented.\n#B. To Be Implemented.:63:63";


            story[63] = "You've died horribly. That really sucks.\nA or B. Try again.:1:1";
        }
        static void PlayerActions() //gets the key pressed by the user and performs a task if the input is valid.
        {

            playerInput = Console.ReadKey(true).KeyChar;

            if (playerInput == 'a' || playerInput == 'A')
            {
                page = choiceA;
            }
            else if (playerInput == 'b' || playerInput == 'B')
            {
                page = choiceB;
            }
            else if (playerInput == 'c' || playerInput == 'C')
            {
                //Save Game
            }
            else if (playerInput == 'd' || playerInput == 'D')
            {
               //Load Game
            }
            else
            {
                Console.WriteLine("Invalid input, please choose from a valid input");
                PlayerActions();
            }
            
        }
        
        static void GetPageNumbers() //splits story pages at the delimter, parses page numbers into ints to be used in the player input method.
        {                           //These writelines are for debugging and should be disabled before creating a build
            Console.WriteLine();
            Console.WriteLine("Page: " + (page));
            sentences = story[page].Split(delimiter); //splits the current story page at every colon and stores it in the sentences array
            string testA = sentences[sentences.Length-2]; //stores the value of the second last sentence as a string to be parsed
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
            if(isIntB == true)
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
            Console.WriteLine(sentences[0]);
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
                        Console.ForegroundColor = ConsoleColor.Green;
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

        static void EndGame()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over!");
            Console.ResetColor();
            Console.ReadKey(true);
        }
        static void Main(string[] args)
        {
           MainMenu();
           while(gameOver == false)
           {
                PageList();
                GetPageNumbers();
                //ChangeTextColor();
                Scribe();
                PlayerActions();  
           }
            EndGame();
        }
    }
}

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
        
        //Some story options will utilize inventory items/tracked gamestate events (to be implemented later).
        //I plan to make my story require multiple loops through the choices, where some options are only available after certain conditions are met.


        static void PageList() //list of all story pages, will be moved to a file later.
        {
            story[0] = "Shotty Game Studios\nWhispers\n--------------------\n#A or B. Start Game.:1:1";
            story[1] = "You wake on a damp bed of leaves. Your head pounds, how did you get here?\nTo the north you see a satchel on the ground.To the south you see a rope leading over the edge of a cliff.\n#A. Go North\n#B. Go South:2:3"; //the option to take the satchel will not be there after it has been used in a previous loop.
            story[2] = "You walk over to the bag, it smells of potent rot,the leather is slimy.\nThe bag pulses slightly.\n#A. Take The Bag?\n#B. Leave The Bag? :4:5";
            story[3] = "You walk to the edge of the cliff and grab the rope, you descend to the base of the cliff.\n#A or B. Descend.:8:8";
            story[4] = "You pick up the bag, it is warm to the touch, the leather ripples under your fingertips. You go to open the bag, but are met with a roiling sense of unease. Maybe the bag should stay closed.\n#A or B. Go to cliff edge.:3:3";
            story[5] = "You decide to leave the bag, it’s gross and you’re pretty sure it’s not yours. You turn to leave, and you begin to hear whispers behind you. /Don’t leave us./\n#A. Turn Around.\n#B. Run.:6:7";
            story[6] = "You turn around to investigate the source of the whispers. You don’t see anyone. The whispers grow louder, your head starts to hurt… A familiar bag rests at your side.\n#A or B. Go to cliff edge.:3:3";
            story[7] = "You run from the whispers, you run to the edge of the cliff and grab the rope. You descend in a hurry.\n#A or B. Descend.:8:8";
            story[8] = "The forest is thick here, mist hangs close to the ground. In the distance you hear a low hum.\n#A. Go East.\n#B. Go West.:9:10";
            story[9] = "You walk east, and eventually come upon a small pond, the mist makes it hard to see, but you can make out your reflection in the water.\n#A. Look Closer.\n#B. Walk Around The Bank.:11:12";
            story[10] = "You walk west, the humming begins to grow louder, and louder, until your form resonates with it. You travel further west, you see an altar nestled amongst the roots and underbrush, upon it rests a basin of fluid. The Hum is intoxicating. You step up to the altar.\n#A. Drink. \n#B. Kneel.:15:16";
            story[11] = "You peer closer to the water, trying to get a better look at your reflection. You get closer and closer to the water, and stop. The face does not stop, and the creature below pulls you into the murky embrace of the pond.::";
            story[12] = "You walk around the bank of the pond, noticing scattered fragments of bone at your feet. You hear something in the water. You turn to see a warped lanky humanoid, it looks at you hungrily.\n#A. Throw the satchel.\n#B. Flee.:13:14"; //when the inventory is implemented, throwing the satchel will only be available if the player picked it up.
            story[13] = "You reach for the satchel, it pulses faster now. Is it afraid? You throw the satchel, it hits the humanoid and lands at it's feet. The creature picks up the bag and takes a bite, your head throbs in pain.:63:63"; //this sends the player back to the start, to begin another loop. When the player returns they will find a an old photo.
            story[14] = "You tear off into the forest, you can hear it crashing through the trees behind you. Eventually you think you've lost it. You hear the hum to your west. You can also make out a standing stone to your north.\n#A. Go West. \n#B. Go North. :10:17";
            story[15] = "You drink the fluid, but this communion does not absolve your guilt. You start to remember, your head pounds.:1:1"; //This sends the player back to begin another loop
            story[16] = "You kneel at the altar, head bowed... You have not atoned. The basin spills. Your guilt eats you alive.::"; //if the player has discovered the truth, this option will be replaced, the truth has not been implemented.
            story[17] = "You walk towards the standing stone, it marks a simple grave, recently filled.\n#A. Read the headstone. \n#B. Mourn. :18:19"; //if the player has the photo from the pond, they will be able to mourn.
            story[18] = "The stone is simply cut, there are no dates on the stone, just a name. Your name. Your vision blurs, your head burns with searing pain.:63:63"; //this sends the player back to begin another loop.
            story[19] = "You look at the photo, and you're reminded of simpler times. You lay the photo on the headstone, the pain in your head eases slightly.:63:63"; //the player must mourn in order to discover the truth. //The photo will no longer be available in subsequent loops.
            story[20] = "";


            story[63] = "The truth whispers to you.\nA or B. Try again.:1:1";
        }
        static void PlayerActions() //gets the key pressed by the user and performs a task if the input is valid.
        {

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
        {                           
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
        static void Main(string[] args)
        {
           MainMenu();
           while(gameOver == false) //gameplay loop, gets page data, parses the page numbers from the data, writes the relevant text to the screen, then waits for player input.
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

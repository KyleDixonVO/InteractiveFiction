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
        static string[] story = new string[64];
        static string[] sentences;
        static char playerInput;
        static int choiceA;
        static int choiceB;
        static bool gameOver = false;
        static char delimiter = ':';
        static string[] keyWords = new string[3];
        static string[] lines;
        static int page = 0;
        



        static void PageList()
        {
            story[0] = "Shotty Game Studios\nInteractive Fiction\n--------------------\n#A or B. Start Game.:1:1";
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
        static void PlayerActions()
        {

            playerInput = Console.ReadKey(true).KeyChar;

            if (playerInput == 'a')
            {
                page = choiceA;
            }
            else if (playerInput == 'b')
            {
                page = choiceB;
            }
            else if (playerInput == 'c')
            {
                //Save Game
            }
            else if (playerInput == 'd')
            {
               //Load Game
            }
            else
            {
                Console.WriteLine("Invalid input, please choose from a valid input");
                PlayerActions();
            }
            
        }
        
        static void Bookmark()
        {
            Console.WriteLine();
            Console.WriteLine("Page: " + (page));
            sentences = story[page].Split(delimiter);
            Console.WriteLine("Sentence Length:" + (sentences.Length));
            Console.WriteLine("Page:" + page);
            Console.WriteLine("A Mod:" + (sentences.Length-2));
            Console.WriteLine("B Mod:" + (sentences.Length - 1));
            Console.WriteLine("A Before:" + choiceA);
            Console.WriteLine("B Before: " + choiceB);
            string testA = sentences[1];
            choiceA = int.Parse(testA);
            Console.WriteLine("Choice A:" + choiceA);
            string testB = sentences[2];
            choiceB = int.Parse(testB);
            Console.WriteLine("Choice B:" + choiceB);
            Console.WriteLine();
        }

        static void Scribe() //writes the appropriate story page
        {
            Console.WriteLine(sentences[0]);
        }

        static void ChangeTextColor() //method to color individual lines based on keywords (in progress)
        {
            lines = sentences[0].Split('\n'); //splitting the string at each new line
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

        static void Main(string[] args)
        {
           while(gameOver == false)
           {
                PageList();
                Bookmark();
                ChangeTextColor();
                Scribe();
                PlayerActions();
                
           }   
        }
    }
}

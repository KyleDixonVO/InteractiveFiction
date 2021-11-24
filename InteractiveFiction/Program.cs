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
        static char playerInput;
        static int choiceA;
        static int choiceB;
        static bool gameOver = false;
        static char delimiter = ':';
        static int page;
        



        static void PageList()
        {
            story[0] = "Shotty Game Studios:--------------------";
            story[1] = "You wake on a damp bed of leaves. Your head pounds, how did you get here?:To the north you see a satchel on the ground.:To the south you see a rope leading over the edge of a cliff.:2-3";
            story[2] = "You walk over to the bag, it smells of potent rot,the leather is slimy. The bag pulses slightly.: Take The Bag? Leave The Bag?:4-5";
            story[3] = "You walk to the edge of the cliff and grab the rope, you descend to the base of the cliff.:";
            story[4] = "You pick up the bag, it is warm to the touch, the leather ripples under your fingertips. You go to open the bag, but are met with a roiling sense of unease. Maybe the bag should stay closed.";
            story[5] = "You decide to leave the bag, it’s gross and you’re pretty sure it’s not yours. You turn to leave, and you begin to hear whispers behind you. Don’t leave us.";
        }

        static void BookMark(int page)
        {
            int[] FindNumbers = System.Array.ConvertAll(story[page].Split('-'), new System.Converter<string, int>(int.Parse));
            foreach (var element in FindNumbers)
            {
                Console.WriteLine(element);
            }
            Console.WriteLine("Bookmark");
        }
        static void PlayerActions()
        {
            playerInput = Console.ReadKey(true).KeyChar;
            if (playerInput == 'a')
            {
               
            }
            else if (playerInput == 'b')
            {
 
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

        static void Scribe(int page)
        {
            Console.WriteLine("Page: " + page);

            string[] sentences = story[page].Split(delimiter);
            foreach (var sentence in sentences)
            {
                Console.WriteLine(sentence);
            }
        }

        static void Main(string[] args)
        {
            PageList();
            Scribe(0);
            Console.ReadKey(true);
            Scribe(1);
            BookMark(page);

            while (gameOver == false)
            {       
                PageList(); 
                PlayerActions();
                Scribe(page);
                
            }
        }
    }
}

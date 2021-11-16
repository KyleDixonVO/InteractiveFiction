using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFiction
{
    class Program
    {
        static string[,,] story = new string[64,64,64];
        static char playerInput;
        static int pageRoot =0;
        static int pageBranch = 0;
        static int pageChoice =0;
        static int choiceA = 1;
        static int choiceB = 2;
        static bool gameOver = false;
        static char delimiter = ':';



        static void PageList()
        {
            story[0,0,0] = "Shotty Game Studios:--------------------";
            story[1,0,0] = "You wake on a damp bed of leaves. Your head pounds, how did you get here?:To the north you see a satchel on the ground.:To the south you see a rope leading over the edge of a cliff.";
            story[1,0,1] = "You walk over to the bag, it smells of potent rot,the leather is slimy. The bag pulses slightly.:";
            story[1,0,2] = "You walk to the edge of the cliff and grab the rope, you descend to the base of the cliff.:";
            story[1,1,1] = "You pick up the bag, it is warm to the touch, the leather ripples under your fingertips.You go to open the bag, but are met with a roiling sense of unease. Maybe the bag should stay closed.";
            story[1,1,2] = "You decide to leave the bag, it’s gross and you’re pretty sure it’s not yours. You turn to leave, and you begin to hear whispers behind you. Don’t leave us.";
        }

        static void PlayerActions()
        {
            playerInput = Console.ReadKey(true).KeyChar;
            if (playerInput == 'a')
            {
                pageChoice = pageChoice + choiceA;
            }
            else if (playerInput == 'b')
            {
                pageChoice = pageChoice + choiceB;
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

        static void Scribe()
        {
            Console.WriteLine("Stats: " + pageRoot + "," + pageBranch + "," + pageChoice);

            string[] sentences = story[pageRoot, pageBranch, pageChoice].Split(delimiter);
            foreach (var sentence in sentences)
            {
                Console.WriteLine(sentence);
            }
        }

        static void Narrator(int x, int y, int z)
        {
            if (x == 0)
            {
                pageRoot++;
            }
            if (x ==1 && y ==0 && z == 1)
            {
                pageBranch++;
            }
        }

        static void Main(string[] args)
        {
            PageList();
            Scribe();
            Narrator(pageRoot, pageBranch, pageChoice);
            Scribe();
            while (gameOver == false)
            {       
                PageList(); 
                PlayerActions();
                Scribe();
                Narrator(pageRoot,pageBranch,pageChoice);
                
            }   
        }
    }
}

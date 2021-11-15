using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFiction
{
    class Program
    {
        static string[,] story = new string[64,64];
        static char playerInput;
        static int pageRoot;


        static void PageList()
        {
            story[0,0] = "You wake on a damp bed of leaves. Your head pounds, how did you get here? To the north you see a satchel on the ground. To the south you see a rope leading over the edge of a cliff.";
            story[0,1] = "page 1";
            story[0,2] = "page 2";
            story[0,3] = "page 3";
        }

        static void PlayerActions()
        {
            playerInput = Console.ReadKey();
            if (playerInput == 'a')
            {

            }
            
            
        }
        
        static void Main(string[] args)
        {
            PageList();
            Console.WriteLine(story[0,0]);
            PlayerActions();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabyrinthOfDoom;
using System.Diagnostics;

namespace LabyrinthOfDoom
{
    class LabyrinthOfDoom
    {
        
        private const char wallchar = '\u2588';
        private const char mazechar = '\u0020';


        static void Main(string[] args)
        {
            Console.SetWindowSize(60, 36);
            Console.Title = "Labyrinth Of Doom";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n");
            Console.WriteLine("      Game: Labyrinth Of Doom ");
            Console.WriteLine("      Target: Find the way out \n\n");
            Console.WriteLine("      Controllers: \n        Up: W Down: S \n        Left: A Right: D");
            Console.WriteLine("\n\n      Press enter to start:");
            Console.WriteLine("\n\n\n\n\n\n");
            Console.WriteLine("    Developed by: Svetlin Krastanov");
            Console.SetCursorPosition(15, 16);
            Console.ReadLine();
            Console.Clear();
            for (int level = 0; level < 3; level++)
            {
                bool[][] mazeLayout = Level1Matrix.GetMatrix();
                switch (level)
                {
                    case 1: mazeLayout = Level2Matrix.GetMatrix(); break;
                    case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                    default: break;
                }
                Console.WriteLine("");
                //Make maze
                for (int i = 0; i < mazeLayout.Length; i++)
                {
                    for (int j = 0; j < mazeLayout[i].Length; j++)
                    {
                        Console.Write(( mazeLayout[i][j] ? wallchar : mazechar));
                    }
                    Console.Write("\n");
                }
                Movements.Move(level);



            }
            Console.WriteLine("\n\n\n\n   Yeaaaa You beat the game! \n\n  You should be proud of yourself \n             :)");
            Console.ReadLine();
        }

    }
}



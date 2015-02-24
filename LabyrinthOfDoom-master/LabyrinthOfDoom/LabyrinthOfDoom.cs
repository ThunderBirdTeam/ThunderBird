using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabyrinthOfDoom;
using System.Diagnostics;
using System.Timers;
namespace LabyrinthOfDoom
{
    class LabyrinthOfDoom
    {

        private const char wallchar = '\u2588';
        private const char mazechar = '\u0020';


        static void Main()
        {
            Console.SetWindowSize(60, 36);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Title = "Labyrinth Of Doom";
            Console.Clear();
            Console.WriteLine("\n\n\n");
            Console.WriteLine("{0, 15} Game: Labyrinth Of Doom ", ' ');
            Console.WriteLine("\n {0, 5}Target: Find the way out \n", ' ');
            Console.WriteLine("{0,10}Controllers: \n{0,10}Up arrow: Up Down arrow: Down \n{0,10}LEft arrow: Left \n{0,10}Right arrow: Right (believe it or not) ", " ");
            Console.WriteLine("\n\n      Press any key to start: \n\n");

            bool[][] arr = Level1Matrix.GetMatrix();
            for (int i = 0; i < 15; i++)
            {
                Console.Write("{0, 19}", ' ');
                for (int j = 0; j < 20; j++)
                {
                    Console.Write(arr[i][j] ? wallchar : mazechar);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine("\n\n {0, 13}Developed by: Svetlin Krastanov", ' ');
            Console.SetCursorPosition(30, 14);
            Console.ReadKey();
            Console.Beep(800, 80);
            Console.Clear();

            PrintArray();
            Console.WriteLine("\n\n\n\n   Yeaaaa You beat the game! \n\n  You should be proud of yourself \n         {0}    :)");
            Console.ReadLine();
        }



        public static void PrintArray()
        {


            for (int level = 0; level < 4; level++)
            {

                Console.Write("");
                bool[][] mazeLayout = Level1Matrix.GetMatrix();
                switch (level)
                {
                    case 1: mazeLayout = Level2Matrix.GetMatrix(); break;
                    case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                    case 3: mazeLayout = Level4Matrix.GetMatrix(); break;
                    default: break;
                }
                Console.WriteLine("");
                //Make maze width Gold

                for (int i = 0; i < mazeLayout.Length; i++)
                {
                    for (int j = 0; j < mazeLayout[i].Length; j++)
                    {
                        if (mazeLayout[i][j])
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(wallchar);
                        }
                        else
                        {
                            if (!mazeLayout[i][j] && (j % 4 == 0))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write("$");

                            }
                            else
                            {
                                Console.Write(mazechar);
                            }

                        }

                    }
                    Console.Write("\n");
                }

                Console.Beep(800, 100);
                Console.Beep(1200, 100);
                Console.Beep(800, 100);
                Console.Beep(1200, 100);
                Console.Beep(800, 100);
                Console.SetWindowSize(60, 40);

                Movements.Move(level);



            }

        }

    }

}

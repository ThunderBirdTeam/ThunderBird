 using System;
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
            Console.BufferHeight = Console.WindowHeight = 36;
            Console.BufferWidth = Console.WindowWidth = 60;
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


            
            Movements.Move();
        

            Movements.PrintBeatTheGame();
            #region Music
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(698, 350);
            Console.Beep(523, 150);
            Console.Beep(415, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            #endregion
            Console.ReadLine();
        }









    }
}

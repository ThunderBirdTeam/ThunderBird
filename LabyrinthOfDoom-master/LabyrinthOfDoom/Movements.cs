using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LabyrinthOfDoom
{
    class Movements
    {
        public static void Move(int counter)
        {
            bool[][] mazeLayout = Level1Matrix.GetMatrix();
            switch (counter)
            {
                case 1: mazeLayout = Level2Matrix.GetMatrix(); break;
                case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                default: break;
            }
           

            Console.WriteLine("");
            int col = 1;
            int row = 3;

            Console.SetCursorPosition(col, row);
            Console.Write("*");
            Console.SetCursorPosition(col, row);

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.W && !mazeLayout[row - 2][col])
                {
                    Console.Write(" ");
                    Debug.Print("W");
                    row--;
                }
                if (info.Key == ConsoleKey.S && !mazeLayout[row][col])
                {
                    Console.Write(" ");
                    Debug.Print("S");
                    row++;
                }

                if (info.Key == ConsoleKey.A && !mazeLayout[row - 1][col - 1])
                {
                    Console.Write(" ");
                    Debug.Print("A");
                    col--;
                }

                if (info.Key == ConsoleKey.D && !mazeLayout[row - 1][col + 1])
                {
                    Console.Write(" ");
                    Debug.Print("D");
                    col++;
                }

                Console.SetCursorPosition(col, row);
                Console.Write("*");
                Console.SetCursorPosition(col, row);
                if (col == 32 && row == 27 && counter == 0)
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on first level :)");
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    Console.Clear();
                    break;
                }
                if (col == 31 && row == 9 && counter == 1)
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on second level :)");
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    Console.Clear();
                    break;
                }
                if (col == 33 && row == 24 && counter == 2)
                {
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on third level :)");
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    Console.Clear();
                    break;
                }

            }
        }
    }
}


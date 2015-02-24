
namespace LabyrinthOfDoom
{
    using System;
    using System.Diagnostics;
    using System.Timers;

    class Movements
    {
        //Time for each level
        static Timer timer = new Timer(1000);
        public static int sec = 35;
        public static int time = 0;
        //Where the cursor should be in begining in each level
        public static int col = 1;
        public static int row = 3;
        //variable for gold
        public static int gold = 0;



        public static void Move(int counter)
        {
            if (time == 0)
            {
                timer.Elapsed += timer_Elapsed;
                time++;
            }
            sec = 35;
            timer.Start();

            //Make mask for movements
            bool[][] mazeLayout = Level1Matrix.GetMatrix();
            switch (counter)
            {
                case 1: mazeLayout = Level2Matrix.GetMatrix();
                    break;
                case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                case 3: mazeLayout = Level4Matrix.GetMatrix(); break;
                default: break;
            }
            switch (counter)
            {
                case 1: Console.SetCursorPosition(col, row);
                    break;
                case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                case 3: mazeLayout = Level4Matrix.GetMatrix(); break;
                default: break;
            }
            Console.WriteLine("");
            Console.SetCursorPosition(col, row);
            Console.Write("@");
            Console.SetCursorPosition(col, row);
            //Here it is the controls 
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.UpArrow && !mazeLayout[row - 2][col])
                {


                    Console.Write(" ");
                    Debug.Print("W");
                    row--;
                }
                if (info.Key == ConsoleKey.DownArrow && !mazeLayout[row][col])
                {


                    Console.Write(" ");
                    Debug.Print("S");
                    row++;
                }

                if (info.Key == ConsoleKey.LeftArrow && !mazeLayout[row - 1][col - 1])
                {

                    Console.Write(" ");
                    Debug.Print("A");
                    col--;
                }

                if (info.Key == ConsoleKey.RightArrow && !mazeLayout[row - 1][col + 1])
                {
                    Console.Write(" ");
                    Debug.Print("D");
                    col++;
                }

                Console.SetCursorPosition(col, row);
                Console.Write("@");
                Console.SetCursorPosition(col, row);
               

                //Print the Colected Gold
                if (!mazeLayout[row][col] && (col % 4 == 0))
                {
                    gold += 5;
                    Console.SetCursorPosition(38, 4);
                    Console.Write("You need at least  ");
                    Console.SetCursorPosition(38, 6);
                    Console.Write("400 gold to pass: ");
                    Console.SetCursorPosition(38, 8);
                    Console.Write("Gold: {0}", gold);
                    Console.SetCursorPosition(38, 10);
                    Console.SetCursorPosition(col, row);

                }
                //Check if gold is not colectet Exit = No no no
                if (gold < 400)
                {
                    switch (counter)
                    {
                        case 0: mazeLayout[26][32] = true;  break;
                        case 1: mazeLayout[8][31] = true; break;
                        case 2: mazeLayout[23][33] = true; break;
                        case 3: mazeLayout[32][28] = true; break;
                        default:break;
                            
                    }
                }
                    // Check if gold is colectet exit = yes yes yes
                else
                {
                    switch (counter)
                    {
                        case 0: mazeLayout[26][32] = false; break;
                        case 1: mazeLayout[8][31] = false; break;
                        case 2: mazeLayout[23][33] = false; break;
                        case 3: mazeLayout[32][28] = false; break;
                        default: break;
                           
                    }
                }
                
                //Check if the user found exit on first level
                if (col == 32 && row == 27 && counter == 0)
                {
                    timer.Close();
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on first level :)");
                    Console.WriteLine("Colected gold: {0}", gold);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    col = 1;
                    row = 3;
                    gold = 0;
                    Console.SetCursorPosition(col, row);

                    Console.Clear();
                    break;
                }
                //Check if the user found exit on second level
                if (col == 31 && row == 9 && counter == 1 )
                {
                    timer.Close();
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on second level :)");
                    Console.WriteLine("Colected gold: {0}", gold);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    col = 1;
                    row = 3;
                    gold = 0;
                    Console.SetCursorPosition(col, row);

                    Console.Clear();
                    break;
                }
                //Check if the user found exit on third level
                if (col == 33 && row == 24 && counter == 2)
                {
                    timer.Close();
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on third level :)");
                    Console.WriteLine("Colected gold: {0}", gold);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    col = 1;
                    row = 3;
                    gold = 0;
                    Console.SetCursorPosition(col, row);

                    Console.Clear();
                    break;
                }
                //Check if the user found exit on four level
                if (col == 32 && row == 29 && counter == 3)
                {
                    timer.Close();
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n\nYou Found the exit on four level :)");
                    Console.WriteLine("Colected gold: {0}", gold);
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();

                    col = 1;
                    row = 3;
                    gold = 0;
                    Console.SetCursorPosition(col, row);

                    Console.Clear();

                    break;
                }
                //If time is 0 return from begining 
                if (sec == 0)
                {
                    col = 1;
                    row = 3;
                    gold = 0;
                    Console.SetCursorPosition(col, row);
                    Console.Clear();

                    LabyrinthOfDoom.PrintArray();

                }

            }
        }

        //Make Timer for each level
        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sec--;
            if (sec > 0)
            {
                Console.SetCursorPosition(36, 15);
                Console.Write("Time to get out: {0} ", sec.ToString());
                Console.SetCursorPosition(col, row);
            }


            //What happend when time is zero
            if (sec == 0)
            {

                Console.Write("G A M E  O V E R");


                timer.Close();
            }


        }

    }
}


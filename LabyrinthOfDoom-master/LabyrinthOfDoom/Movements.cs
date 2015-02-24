
namespace LabyrinthOfDoom
{
    using System;
    using System.Diagnostics;
    using System.Timers;

    class Movements

    {
        private const char wallchar = '\u2588';
        private const char mazechar = '\u0020';
        //Time for each level
        static Timer timer = new Timer(1000);
        public static int sec = 30;
        public static int time = 0;
        //Where the cursor should be in begining in each level
        public static int col = 1;
        public static int row = 3;
        //variable for gold
        public static int gold = 0;
        public static void PrintArray(int level)
        {

                Console.Write("");
                bool[][] mazeLayout = Level1Matrix.GetMatrix();
                switch (level)
                {
                    case 1: mazeLayout = Level2Matrix.GetMatrix(); break;
                    case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                    case 3: mazeLayout = Level4Matrix.GetMatrix(); break;
                    case 4: mazeLayout = Level5Matrix.GetMatrix(); break;
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

            }




        public static void Move()
        {
            for (int level = 0; level <= 4; level++)
            {

                PrintArray(level);
                int counter = level;
                if (time == 0)
                {
                    timer.Elapsed += timer_Elapsed;
                    time++;
                }
                sec = 30;
                timer.Start();

                //Make mask for movements
                bool[][] mazeLayout = Level1Matrix.GetMatrix();
                switch (counter)
                {
                    case 1: mazeLayout = Level2Matrix.GetMatrix();
                        break;
                    case 2: mazeLayout = Level3Matrix.GetMatrix(); break;
                    case 3: mazeLayout = Level4Matrix.GetMatrix(); break;
                    case 4: mazeLayout = Level5Matrix.GetMatrix(); break;
                    default: break;
                }

                Console.WriteLine("");
                Console.SetCursorPosition(col, row);
                Console.Write("@");
                Console.SetCursorPosition(col, row);

                #region Controls
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

                #endregion


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


                    #region If 400 gold is colected make exit avalible
                    //Check if gold is not colectet, Exit = No no no
                    if (gold < 400)
                    {
                        switch (counter)
                        {
                            case 0: mazeLayout[26][32] = true; break;
                            case 1: mazeLayout[8][31] = true; break;
                            case 2: mazeLayout[23][33] = true; break;
                            case 3: mazeLayout[32][28] = true; break;
                            case 5: mazeLayout[8][31] = true; break;
                            default: break;

                        }
                    }
                    // Check if gold is colectet, exit = yes yes yes
                    else
                    {
                        switch (counter)
                        {
                            case 0: mazeLayout[26][32] = false; break;
                            case 1: mazeLayout[8][31] = false; break;
                            case 2: mazeLayout[23][33] = false; break;
                            case 3: mazeLayout[32][28] = false; break;
                            case 5: mazeLayout[8][31] = true; break;
                            default: break;

                        }
                    }
                    #endregion



                    #region If user found exit
                    //Check if the user found exit on first level
                    if (col == 32 && row == 27 && counter == 0)
                    {
                        timer.Close();
                        Console.Clear();
                        FoundExit();
                        Console.ReadLine();

                        col = 1;
                        row = 3;
                        gold = 0;
                        Console.SetCursorPosition(col, row);

                        Console.Clear();
                        break;
                    }
                    //Check if the user found exit on second level
                    if (col == 31 && row == 9 && counter == 1)
                    {
                        timer.Close();
                        Console.Clear();
                        FoundExit();
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
                        FoundExit();
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
                        FoundExit();
                        Console.ReadLine();

                        col = 1;
                        row = 3;
                        gold = 0;
                        Console.SetCursorPosition(col, row);

                        Console.Clear();

                        break;
                    }
                    //Check if the user found exit on five level
                    if (col == 31 && row == 9 && counter == 4)
                    {
                        timer.Close();
                        Console.Clear();
                        FoundExit();
                        Console.ReadLine();

                        col = 1;
                        row = 3;
                        gold = 0;
                        Console.SetCursorPosition(col, row);

                        Console.Clear();
                        break;
                    }
                    #endregion


                    //If time is 0 return from begining 
                    if (sec == 0)
                    {
                        

                    }

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
                col = 1;
                row = 3;
                gold = 0;
                Console.SetCursorPosition(col, row);
                Console.Clear();

                Move();

                timer.Close();
            }

        
        }
    
        private static void FoundExit()
        {
            #region Print the picture between level
            Console.WriteLine(@"
    
   ....................................................
   .   o   \ o /  _ o        __|      o _  \ o /   o   .
   .  /|\    |     /\   __\o   \o     /\     |    /|\  .
   .  / \   / \   | \  /) |    ( \    / |   / \   / \  .
   .       .....................................       .
   . \ o / .                                   . \ o / .
   .   |   .                                   .   |   .
   .  / \  .                                   .  / \  .
   .       .                                   .       .
   .       .                                   .       .
   .  __\o .        You Found the EXIT         .  __\o .
   . /) |  .                                   . /) |  .
   .       .                                   .       .
   . __|   .                                   . __|   .
   .   \o  .        Colected Gold: {0}         .    \o .
   .   ( \ .                                   .   ( \ .
   .       .        Enter to Continue          .       .
   .  \ /  .                                   .  \ /  .
   .   |   .                                   .   |   .
   .  /o\  .                                   .  /o\  .
   .       .                                   .       .
   .  o _  .                                   .  o _  .
   .  /\   .                                   .  /\   .
   .  / |  .                                   .  / |  .
   .       .                                   .       .
   . \ o / .                                   . \ o / .
   .   |   .                                   .   |   .
   .  / \  .                                   .  / \  .
   .       .....................................       .
   .   o   \ o /  _ o        __|      o _  \ o /   o   .
   .  /|\    |     /\   __\o   \o     /\     |    /|\  .
   .  / \   / \   | \  /) |    ( \    / |   / \   / \  .
   .....................................................

            ", gold);
            #endregion
            Console.SetCursorPosition(28,20);
        }

        public static void PrintBeatTheGame()
        {
            #region Print The picture for the end of the game

            Console.WriteLine(@"
  
 ____                 ___.--''''`--(  )
    \______ __.------'              ||
                                    ||
                                    ||
       YOU BEAT THE GAME            ||
                                    ||
            +   +                   ||
                            ,,,,,   ||
          '-_____-'        ;;;;;\   ||
_                    /--'-'';;C '\-\|-:
 `----.____.--`\----'       );  _)  / :
                          .'=. (   / /
                          |   )`-\/ /|
                          \   \ /  /||
                           ;.  '  / ||
                           | `._,'|-||:
                           \      )-||:
                            )=====] ||
                           /       \||
                           \_      )||
                           \\      |
                            \\     |
                            ||     /
                            ||    |
                            \\    |
                            ||    |
                            ||    |
                            ||    |
                            ||    |
                            |[___/
                            ))  '`--.
                        ``='===='

");
            #endregion
        }
    }
}


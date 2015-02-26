using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

// Struct for printing each symbol
struct Symbol
{
    public int x;
    public int y;
    public string str;
    public ConsoleColor color;
}
class Spider
{
    // Printing rain, spider and flies
    static void PrintOnPosition(int x, int y, string str,
        ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }
    // Method for printing the text
    static void PrintOnPositionInfo(int x, int y, string str, ConsoleColor color = ConsoleColor.DarkGray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.WriteLine(str);
    }

    static void Main()
    {
        // you need to copy the filepath here for your own pc else it won't work
        SoundPlayer player = new SoundPlayer(@"..\..\..\storm.wav");
        player.PlayLooping();

        // Parameters of the playfield
        Console.BufferHeight = Console.WindowHeight = 40;
        Console.BufferWidth = Console.WindowWidth = 70;

        // Defining user spider
        Symbol bug = new Symbol();
        bug.x = Console.WindowWidth / 2;
        bug.y = Console.WindowHeight - 3;

        // LifeCounter
        int lifesCount = 3;

        // Creating list which will contain all falling drops
        List<Symbol> rain = new List<Symbol>();

        // Creating list which will contain all flies
        List<Symbol> flies = new List<Symbol>();

        //Creating list which will count flies/store score
        List<Symbol> score = new List<Symbol>();

        // RandomGenerator of all random needs
        Random randomGenerator = new Random();

        // While cycle for the game itself
        while (true)
        {
            // Clearing console
            Console.Clear();

            // Blue sky
            Console.SetCursorPosition(0, 1);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            string blue = new string(' ', Console.WindowWidth);
            Console.Write(blue);
            Console.ResetColor();

            // Clouds
            StreamReader readerClouds = new StreamReader(@"..\..\..\Clouds.txt");
            string contentClouds = readerClouds.ReadToEnd();
            Console.SetCursorPosition(1, 2);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(contentClouds);
            Console.ResetColor();

            // Score and Lives
            PrintOnPositionInfo(3, 3, "Score:" + score.Count, ConsoleColor.Blue);
            PrintOnPositionInfo(3, 4, "Lives:" + lifesCount, ConsoleColor.Green);

            // GreenGrass
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            string green = new string(' ', 0);
            Console.WriteLine(green);
            Console.ResetColor();

            // Decorative web
            // you need to copy the filepath here for your own pc else it won't work
            Console.SetCursorPosition(0, Console.WindowHeight - 15);
            Console.ForegroundColor = ConsoleColor.White;
            StreamReader readerWeb = new StreamReader(@"..\..\..\spiderWeb.txt");
            string contentWeb = readerWeb.ReadToEnd();
            Console.Write(contentWeb);

            // Defyining fly
            Symbol newFly = new Symbol();
            // Generating flies
            int chance = randomGenerator.Next(0, 100);

            if (chance < 10)
            {
                newFly.x = 1;
                newFly.y = randomGenerator.Next(0, Console.WindowHeight);
                newFly.color = ConsoleColor.Yellow;
                newFly.str = ">:<";
                flies.Add(newFly);
            }
            else
            {
                // Specifying top drops
                Symbol newDropTop = new Symbol();
                newDropTop.color = ConsoleColor.Cyan;
                newDropTop.x = randomGenerator.Next(0, Console.WindowWidth);
                newDropTop.y = -1;
                newDropTop.str = "/";
                rain.Add(newDropTop);

                // Specyfying right drops
                Symbol newDropRight = new Symbol();
                newDropRight.color = ConsoleColor.Cyan;
                newDropRight.x = Console.WindowWidth - 1;
                newDropRight.y = randomGenerator.Next(0, Console.WindowHeight);
                newDropRight.str = "/";
                rain.Add(newDropRight);
            }

            // Fly list
            List<Symbol> newListFlies = new List<Symbol>();
            Symbol movingFly = new Symbol();

            for (int i = 0; i < flies.Count; i++)
            {
                // Moving flies
                if (flies[i].x < Console.WindowWidth && flies[i].y + 1 < Console.WindowHeight - 1)
                {
                    movingFly.x = flies[i].x + 1;
                    movingFly.y = flies[i].y + 1;
                    movingFly.str = flies[i].str;
                    movingFly.color = flies[i].color;
                    newListFlies.Add(movingFly);
                }

                // Check for collision - if we eat fly
                if (movingFly.x >= bug.x
                    && movingFly.x <= bug.x + 5
                    && movingFly.y >= bug.y
                    && movingFly.y <= bug.y + 1)
                {
                    score.Add(movingFly);
                    newListFlies.Remove(movingFly);
                }
            }
            flies = newListFlies;

            // Drop list
            List<Symbol> newListDrops = new List<Symbol>();
            Symbol movingDrop = new Symbol();

            for (int i = 0; i < rain.Count; i++)
            {
                // Moving drops
                if (rain[i].x - 1 >= 1 && rain[i].y + 1 < Console.WindowHeight - 1)
                {
                    movingDrop.x = rain[i].x - 1;
                    movingDrop.y = rain[i].y + 1;
                    movingDrop.str = rain[i].str;
                    movingDrop.color = rain[i].color;
                    newListDrops.Add(movingDrop);
                }

                // Check for collision - if drop has hit us
                if (movingDrop.x >= bug.x + 1
                    && movingDrop.x <= bug.x + 3
                    && movingDrop.y >= bug.y
                    && movingDrop.y <= bug.y + 1)
                {
                    lifesCount--;

                    player.Stop();
                    SoundPlayer playerDied = new SoundPlayer(@"..\..\..\died.wav");
                    playerDied.Play();

                    PrintOnPosition(bug.x, bug.y - 1, bug.str = "DIED!", bug.color = ConsoleColor.Blue);
                    PrintOnPosition(bug.x, bug.y, bug.str = string.Format("  {0}", lifesCount), bug.color = ConsoleColor.Blue);
                    PrintOnPosition(bug.x, bug.y + 1, bug.str = "lifes", bug.color = ConsoleColor.Blue);
                    PrintOnPosition(bug.x, bug.y + 2, bug.str = "left!", bug.color = ConsoleColor.Blue);

                    Thread.Sleep(2000);
                    Console.Clear();
                    newListDrops.Clear();
                    newListFlies.Clear();
                    rain.Clear();
                    player.Play();

                    if (lifesCount <= 0)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 2);
                        Console.WriteLine("GAME OVER");
                        Console.ReadLine();
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2);
                        return;
                    }
                }
            }
            rain = newListDrops;

            // Printing drops
            foreach (Symbol drop in rain)
            {
                if (drop.y == 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                PrintOnPosition(drop.x, drop.y, drop.str, drop.color);
                Console.ResetColor();
            }

            // Printing flies
            foreach (Symbol fly in flies)
            {
                PrintOnPosition(fly.x, fly.y, fly.str, fly.color);
                Console.ResetColor();
            }

            for (int i = 0; i < 10; i++)
            {
                // Printing the spider in black which fix the moving bug at printing
                PrintOnPosition(bug.x, bug.y, bug.str = @"_\o/_", bug.color = ConsoleColor.Black);
                PrintOnPosition(bug.x, bug.y + 1, bug.str = @"/(_)\", bug.color = ConsoleColor.Black);

                // Catching movement of the arrows and spider movement
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    if (pressedKey.Key == ConsoleKey.LeftArrow
                        && bug.x >= 1)
                    {
                        bug.x--;
                    }
                    else if (pressedKey.Key == ConsoleKey.RightArrow
                        && bug.x < Console.WindowWidth - 5)
                    {
                        bug.x++;
                    }
                    else if (pressedKey.Key == ConsoleKey.UpArrow
                        && bug.y >= Console.WindowHeight - 12)
                    {
                        bug.y--;
                    }
                    else if (pressedKey.Key == ConsoleKey.DownArrow
                        && bug.y < Console.WindowHeight - 3)
                    {
                        bug.y++;
                    }
                }

                // Printing red spider as it should be
                PrintOnPosition(bug.x, bug.y, bug.str = @"_\o/_", bug.color = ConsoleColor.Red);
                PrintOnPosition(bug.x, bug.y + 1, bug.str = @"/(_)\", bug.color = ConsoleColor.Red);

                // Slowing down the cycle
                Thread.Sleep(15);
            }
        }
    }
}
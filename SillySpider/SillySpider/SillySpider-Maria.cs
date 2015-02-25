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
class Game
{
    // Printing rain, spider and flies
    static void PrintOnPosition(int x, int y, string str,
        ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }
    /*Method for printing the text*/
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

        Symbol newFly = new Symbol();
        //LifeCounter------------------------------------>>>>>Maria<<<<<<<<<<<<<<-------------------------------------
        int lifesCount = 5;

        // Creating list which will contain all falling drops
        List<Symbol> rain = new List<Symbol>();
        //Creating list which will count flies/ store score------------------------------------------------>>>Maria<<<<<<<<<<<<<<-------------------------------------
        List<Symbol> score = new List<Symbol>();
        // RandomGenerator of all random needs
        Random randomGenerator = new Random();

        // While cycle for the game itself
        while (true)
        {
            bool hit = false;//----------------->Maria<<<<<<<<<<<<<<-------------------------------------
            // Clearing Console
            Console.Clear();

            // Blue sky
            Console.SetCursorPosition(0, 1);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            string blue = new string(' ', Console.WindowWidth);
            Console.Write(blue);
            Console.ResetColor();

            // you need to copy the filepath here for your own pc else it won't work
            StreamReader readerClouds = new StreamReader(@"..\..\..\Clouds.txt");
            string contentClouds = readerClouds.ReadToEnd();
            Console.SetCursorPosition(1, 2);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(contentClouds);
            Console.ResetColor();

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

            int chance = randomGenerator.Next(0, 100);//---------------->Generating flies<<<<<<<<<<<<<<---------------------------Maria-----
            if (chance < 10)
            {
                //Defyining fly------------------------------------>>>>>Maria<<<<<<<<<<<<<<-------------------------------------

                newFly.x = Console.WindowWidth / 2;
                newFly.y = Console.WindowHeight - 3;
                newFly.color = ConsoleColor.Yellow;
                newFly.str = ">:<";
                newFly.x = randomGenerator.Next(0, Console.WindowWidth);
                newFly.y = -1;
                rain.Add(newFly);
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

                // Specyfyinf Right drops
                Symbol newDropRight = new Symbol();
                newDropRight.color = ConsoleColor.Cyan;
                newDropRight.x = Console.WindowWidth - 1;
                newDropRight.y = randomGenerator.Next(0, Console.WindowHeight);
                newDropRight.str = "/";
                rain.Add(newDropRight);
            }
            // Drop list
            List<Symbol> newList = new List<Symbol>();
            Symbol movingDrop = new Symbol();
            for (int i = 0; i < rain.Count; i++)
            {
                // Moving drops
                if (rain[i].x - 1 >= 1 && rain[i].y + 1 < Console.WindowHeight)
                {
                    movingDrop.x = rain[i].x - 1;
                    movingDrop.y = rain[i].y + 1;
                    movingDrop.str = rain[i].str;
                    movingDrop.color = rain[i].color;
                    newList.Add(movingDrop);

                }
                if (movingDrop.x >= bug.x + 1
                 && movingDrop.x <= bug.x + 3
                 && movingDrop.y >= bug.y
                 && movingDrop.y <= bug.y + 1 && newFly.str == ">:<")
                {
                    score.Add(newFly);//----------------------------------------------------->Maria
                }
                // Check for collision - if drop has hit us
                if (movingDrop.x >= bug.x + 1
                    && movingDrop.x <= bug.x + 3
                    && movingDrop.y >= bug.y
                    && movingDrop.y <= bug.y + 1 )
                {
                    lifesCount--;//Taking life------------------->>>Maria<<<<<<<<<<<<<<-------------------------------------
                    hit = true;
                    // you need to copy the filepath here for your own pc else it won't work
                    if (lifesCount <= 0)
                    {
                        player.Stop();
                        SoundPlayer playerDied = new SoundPlayer(@"..\..\..\died.wav");
                        playerDied.Play();

                        PrintOnPosition(bug.x, bug.y - 1, bug.str = "~ ~ ~~~ ~ ~", bug.color = ConsoleColor.Blue);
                        PrintOnPosition(bug.x, bug.y, bug.str = " YOU DIED ", bug.color = ConsoleColor.Blue);
                        PrintOnPosition(bug.x, bug.y + 1, bug.str = "~ ~ ~~~ ~ ~", bug.color = ConsoleColor.Blue);
                        Console.ReadLine();
                        return;//exit from the void Main = end of program, you can also use Environment.ExitCode;------>Maria
                    }

                    Thread.Sleep(2050);
                    Console.Clear();
                    newList.Clear();
                    rain.Clear();
                    player.Play();
                }
            }
            rain = newList;

            // Printing drops
            foreach (Symbol drop in rain)
            {
                if (drop.y == Console.WindowHeight - 1)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                }
                if (drop.y == 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                PrintOnPosition(drop.x, drop.y, drop.str, drop.color);
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
                PrintOnPositionInfo(10, 8, "Score:" + score.Count, ConsoleColor.Blue);
                PrintOnPositionInfo(10, 4, "Lives:" + lifesCount, ConsoleColor.Green);
                // Slowing down the cycle
                Thread.Sleep(15);
            }
        }
    }
}
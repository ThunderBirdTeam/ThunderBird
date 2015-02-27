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
    //1 Method Printing rain, spider and flies
    static void PrintOnPosition(int x, int y, string str,
        ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }
    //2 Method for printing the text
    static void PrintOnPositionInfo(int x, int y, string str, ConsoleColor color = ConsoleColor.DarkGray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.WriteLine(str);
    }
    //3 Method for printing playfield static elements:the sky,some clouds, grass decorative web
    static void PrintingStaticElements()
    {
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

        //green Grass
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        string green = new string(' ', 0);
        Console.WriteLine(green);
        Console.ResetColor();


        // Decorative web
        Console.SetCursorPosition(0, Console.WindowHeight - 15);
        Console.ForegroundColor = ConsoleColor.White;
        StreamReader readerWeb = new StreamReader(@"..\..\..\spiderWeb.txt");
        string contentWeb = readerWeb.ReadToEnd();
        Console.Write(contentWeb);

    }
    //4 method creating flies

    private static Symbol SpecifyingFlies(List<Symbol> flies, Random randomGenerator, Symbol newFly)
    {
        newFly.x = 1;
        newFly.y = randomGenerator.Next(0, Console.WindowHeight);
        newFly.color = ConsoleColor.Yellow;
        newFly.str = ">:<";
        flies.Add(newFly);
        return newFly;
    }

    //5 Method moving flies
    private static Symbol MovingFlies(List<Symbol> flies, List<Symbol> newListFlies, Symbol movingFly, int i)
    {
        movingFly.x = flies[i].x + 1;
        movingFly.y = flies[i].y + 1;
        movingFly.str = flies[i].str;
        movingFly.color = flies[i].color;
        newListFlies.Add(movingFly);
        return movingFly;
    }
    //6 method creating drops
    private static void SpecifyingDrops(List<Symbol> rain, Random randomGenerator)
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
    //7 method moving drops
    private static Symbol MovingDrops(List<Symbol> rain, List<Symbol> newListDrops, Symbol movingDrop, int i)
    {
        movingDrop.x = rain[i].x - 1;
        movingDrop.y = rain[i].y + 1;
        movingDrop.str = rain[i].str;
        movingDrop.color = rain[i].color;
        newListDrops.Add(movingDrop);
        return movingDrop;
    }
    // 8 method Printing flies
    private static void PrintFlies(List<Symbol> flies)
    {
        foreach (Symbol fly in flies)
        {
            PrintOnPosition(fly.x, fly.y, fly.str, fly.color);
            Console.ResetColor();
        }
    }
    // 9method Printing drops
    private static void PrintDrops(List<Symbol> rain)
    {
        foreach (Symbol drop in rain)
        {
            if (drop.y == 0)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            PrintOnPosition(drop.x, drop.y, drop.str, drop.color);
            Console.ResetColor();
        }
    }
    //10 method MovingSpider
      private static Symbol MovingSpider(Symbol spider)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            while (Console.KeyAvailable) Console.ReadKey(true);
            if (pressedKey.Key == ConsoleKey.LeftArrow
                && spider.x >= 1)
            {
                spider.x--;
            }
            else if (pressedKey.Key == ConsoleKey.RightArrow
                && spider.x < Console.WindowWidth - 5)
            {
                spider.x++;
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow
                && spider.y >= Console.WindowHeight - 12)
            {
                spider.y--;
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow
                && spider.y < Console.WindowHeight - 3)
            {
                spider.y++;
            }
        }
        return spider;
    }
    static void Main()
    {
        SoundPlayer player = new SoundPlayer(@"..\..\..\storm.wav");
        player.PlayLooping();

        // Parameters of the playfield
        Console.BufferHeight = Console.WindowHeight = 40;
        Console.BufferWidth = Console.WindowWidth = 70;

        // Defining user spider
        Symbol spider = new Symbol();
        spider.x = Console.WindowWidth / 2;
        spider.y = Console.WindowHeight - 3;

        // LifeCounter
        int lifes = 3;
        // Counts flies/store score
        int score = 0;

        // Creating list which will contain all falling drops
        List<Symbol> rain = new List<Symbol>();

        // Creating list which will contain all flies
        List<Symbol> flies = new List<Symbol>();

        // RandomGenerator of all random needs
        Random randomGenerator = new Random();

        // While cycle for the game itself
        while (true)
        {
            // Clearing console
            Console.Clear();

            //Printing the sky and some clouds
            PrintingStaticElements();

            // Score and Lives
            PrintOnPositionInfo(3, 3, "Score: " + score, ConsoleColor.Blue);
            PrintOnPositionInfo(3, 4, "Lives: " + lifes, ConsoleColor.Green);

            // Defining fly
            Symbol newFly = new Symbol();
            // Generating chance of flies and drops appearing
            int chance = randomGenerator.Next(0, 100);

            if (chance < 10)
            {
                newFly = SpecifyingFlies(flies, randomGenerator, newFly);
            }
            else
            {
                SpecifyingDrops(rain, randomGenerator);
            }

            // Fly list
            List<Symbol> newListFlies = new List<Symbol>();
            Symbol movingFly = new Symbol();

            for (int i = 0; i < flies.Count; i++)
            {
                // Moving flies
                if (flies[i].y + 1 < Console.WindowHeight - 1)
                {
                    movingFly = MovingFlies(flies, newListFlies, movingFly, i);
                }

                // Check for collision - if we eat fly
                if (movingFly.x + 2 >= spider.x
                    && movingFly.x <= spider.x + 4
                    && movingFly.y >= spider.y
                    && movingFly.y <= spider.y + 2)
                {
                    score++;
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
                    movingDrop = MovingDrops(rain, newListDrops, movingDrop, i);
                }

                // Check for collision - if drop has hit us
                if (movingDrop.x >= spider.x + 1
                    && movingDrop.x <= spider.x + 3
                    && movingDrop.y >= spider.y
                    && movingDrop.y <= spider.y + 1)
                {
                    lifes--;

                    player.Stop();
                    SoundPlayer lifeLost = new SoundPlayer(@"..\..\..\lifeLost.wav");
                    lifeLost.Play();

                    if (lifes >= 0)
                    {
                        PrintOnPosition(spider.x, spider.y - 2, spider.str = "DIED!", spider.color = ConsoleColor.Blue);
                        PrintOnPosition(spider.x, spider.y - 1, spider.str = string.Format("  {0}", lifes), spider.color = ConsoleColor.Blue);
                        PrintOnPosition(spider.x, spider.y, spider.str = "lives", spider.color = ConsoleColor.Blue);
                        PrintOnPosition(spider.x, spider.y + 1, spider.str = "left!", spider.color = ConsoleColor.Blue);

                        Thread.Sleep(2000);
                        Console.Clear();
                        newListDrops.Clear();
                        newListFlies.Clear();
                        rain.Clear();
                        player.Play();
                    }
                    else
                    {
                        SoundPlayer died = new SoundPlayer(@"..\..\..\died.wav");
                        died.Play();

                        Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 2);
                        Console.WriteLine("GAME OVER");
                        Console.ReadLine();
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2);
                        return;
                    }
                }
            }
            rain = newListDrops;

            PrintDrops(rain);

            PrintFlies(flies);

            for (int i = 0; i < 10; i++)
            {
                // Printing the spider in black which fix the moving spider at printing
                PrintOnPosition(spider.x, spider.y, spider.str = @"_\o/_", spider.color = ConsoleColor.Black);
                PrintOnPosition(spider.x, spider.y + 1, spider.str = @"/(_)\", spider.color = ConsoleColor.Black);

                // Catching movement of the arrows and spider movement
                spider = MovingSpider(spider);

                // Printing red spider as it should be
                PrintOnPosition(spider.x, spider.y, spider.str = @"_\o/_", spider.color = ConsoleColor.Red);
                PrintOnPosition(spider.x, spider.y + 1, spider.str = @"/(_)\", spider.color = ConsoleColor.Red);

                // Slowing down the cycle
                Thread.Sleep(15);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Security;
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
    private static string playerName;
    // Printing the rain, spider, flies and info
    static void PrintOnPosition(int x, int y, string str, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }

    // Checks if text file is readed successfully
    private static void PrintASCIIBackground(int x, int y, ConsoleColor color, string txt)
    {
        try
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            StreamReader content = new StreamReader(txt);
            Console.Write(content.ReadToEnd());
        }
        catch (ArgumentNullException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (PathTooLongException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (FileNotFoundException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (IOException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (NotSupportedException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (SecurityException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
    }

    // Printing static elements of the playfield - the sky, grass, static web and some clouds
    public static void PrintStaticElementsPlayfield()
    {
        // Blue sky
        Console.SetCursorPosition(0, 1);
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        string blue = new string(' ', Console.WindowWidth);
        Console.Write(blue);
        Console.ResetColor();

        // Clouds
        PrintASCIIBackground(0, 2, ConsoleColor.Blue, @"..\..\..\Clouds.txt");

        // Decorative web
        PrintASCIIBackground(0, Console.WindowHeight - 14, ConsoleColor.White, @"..\..\..\SpiderWeb.txt");

        // Green grass
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        string green = new string(' ', 0);
        Console.WriteLine(green);
        Console.ResetColor();
    }

    // Defining user spider
    private static Symbol DefiningSpider()
    {
        Symbol spider = new Symbol();
        spider.x = Console.WindowWidth / 2;
        spider.y = Console.WindowHeight - 3;
        return spider;
    }

    // Generating chance of falling objects - rain and flies
    private static void RandomChanceFliesAndDrops(List<Symbol> rain, List<Symbol> flies, Random randomGenerator)
    {
        // Randomly generating flies
        int chance = randomGenerator.Next(0, 100);

        if (chance < 10)
        {
            // Defining fly
            Symbol newFly = new Symbol();
            newFly.color = ConsoleColor.Yellow;
            newFly.x = 1;
            newFly.y = randomGenerator.Next(0, Console.WindowHeight);
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
    }

    // Moving flies
    private static List<Symbol> MovingFlies(ref Symbol spider, ref int score, List<Symbol> flies)
    {
        // Fly list
        List<Symbol> newListFlies = new List<Symbol>();
        Symbol movingFly = new Symbol();

        for (int i = 0; i < flies.Count; i++)
        {
            // Moving flies
            if (flies[i].y + 1 < Console.WindowHeight - 1)
            {
                movingFly.x = flies[i].x + 1;
                movingFly.y = flies[i].y + 1;
                movingFly.str = flies[i].str;
                movingFly.color = flies[i].color;
                newListFlies.Add(movingFly);
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
        return newListFlies;
    }

    // Moving Drops
    private static List<Symbol> MovingDrops(SoundPlayer player, ref Symbol spider, ref int lifes, List<Symbol> rain, List<Symbol> newListFlies)
    {
        // Drops list
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
            if (movingDrop.x >= spider.x + 1
                && movingDrop.x <= spider.x + 3
                && movingDrop.y >= spider.y
                && movingDrop.y <= spider.y + 1)
            {
                lifes--;

                player.Stop();
                SoundPlayer lifeLost = new SoundPlayer();
                PlaySound(@"..\..\..\LifeLost.wav", lifeLost);
                lifeLost.Play();

                // Message that shows lives left
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

                // Game over
                else
                {
                    SoundPlayer died = new SoundPlayer();
                    PlaySound(@"..\..\..\Died.wav", died);
                    died.Play();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 2);
                    Console.WriteLine("GAME OVER");
                    Console.ReadLine();
                    Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight / 2);

                    Environment.Exit(0);
                }
            }
        }
        return newListDrops;
    }

    // Printing drops
    private static void PrintingDrops(List<Symbol> rain)
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

    // Printing flies
    private static void PrintingFlies(List<Symbol> flies)
    {
        foreach (Symbol fly in flies)
        {
            PrintOnPosition(fly.x, fly.y, fly.str, fly.color);
            Console.ResetColor();
        }
    }

    // Printing moving spider
    private static Symbol MovingAndPrintingSpider(Symbol spider)
    {
        for (int i = 0; i < 10; i++)
        {
            // Printing the spider in black which fix the moving spider bug at printing
            PrintOnPosition(spider.x, spider.y, spider.str = @"_\o/_", spider.color = ConsoleColor.Black);
            PrintOnPosition(spider.x, spider.y + 1, spider.str = @"/(_)\", spider.color = ConsoleColor.Black);

            // Catching movement of the arrows and spider movement
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

            // Printing red spider as it should be
            PrintOnPosition(spider.x, spider.y, spider.str = @"_\o/_", spider.color = ConsoleColor.Red);
            PrintOnPosition(spider.x, spider.y + 1, spider.str = @"/(_)\", spider.color = ConsoleColor.Red);

            // Slowing down the cycle
            Thread.Sleep(15);
        }
        return spider;
    }

    // Checks if wav file is loaded successfully
    private static SoundPlayer PlaySound(string wav, SoundPlayer player)
    {
        try
        {
            player.SoundLocation = wav;
        }
        catch (ArgumentNullException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (PathTooLongException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (FileNotFoundException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (IOException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (NotSupportedException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (SecurityException e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error: " + e.Message);
        }
        return player;
    }

    // While cycle of the game
    private static void GameCycle(SoundPlayer player, ref Symbol spider, ref int lifes, ref int score, ref List<Symbol> rain, ref List<Symbol> flies, Random randomGenerator)
    {
        while (true)
        {
            Console.Clear();

            PrintStaticElementsPlayfield();

            PrintOnPosition(3, 3, "Score: " + score, ConsoleColor.Magenta);
            PrintOnPosition(3, 4, "Lives: " + lifes, ConsoleColor.Green);
            PrintOnPosition(3, 5, "Player: " + playerName, ConsoleColor.Yellow);//------------------------>>>>>>>>>>>>>Maria<<<<<<<<<<<<

            RandomChanceFliesAndDrops(rain, flies, randomGenerator);

            List<Symbol> newListFlies = MovingFlies(ref spider, ref score, flies);
            flies = newListFlies;

            List<Symbol> newListDrops = MovingDrops(player, ref spider, ref lifes, rain, newListFlies);
            rain = newListDrops;

            PrintingDrops(rain);

            PrintingFlies(flies);

            spider = MovingAndPrintingSpider(spider);
        }
    }

    private static void GameResults(ref int score, ref string playerName)//----------------------------->>>>>>Maria<<<<<<<<<<<>>
    {
        string totalScore = score.ToString();
        string[,] gameResult = new string[1, 10];
        gameResult[0, 0] = playerName;
        gameResult[0, 1] = totalScore;
      //Printing result
        for (int row = 0; row < gameResult.GetLength(0); row++)
        {
            for (int col = 0; col < gameResult.GetLength(1); col++)
            {
                Console.Write(" " + gameResult[row, col]);
            }
            Console.WriteLine();
        }
    }

    static void Main()
    {
        // Playing rain sound
        SoundPlayer player = new SoundPlayer();
        PlaySound(@"..\..\..\Storm.wav", player);
        player.PlayLooping();

        // Parameters of the playfield
        Console.BufferHeight = Console.WindowHeight = 40;
        Console.BufferWidth = Console.WindowWidth = 70;

        // Defininag user spider
        Symbol spider = DefiningSpider();

        // LifeCounter and score
        int lifes = 2;
        int score = 0;

        // Creating list which will contain all falling drops
        List<Symbol> rain = new List<Symbol>();

        // Creating list which will contain all flies
        List<Symbol> flies = new List<Symbol>();

        //Getting PlayerName------------------------------------------------------------------------->>>>>Maria<<<<<<<<<<<<<<<<<<<<<<<<<<
        PrintOnPosition(4, 4, "Please,Enter your name:", ConsoleColor.Blue);
        playerName = Console.ReadLine();
        // RandomGenerator of all random needs
        Random randomGenerator = new Random();

        // While cycle for the game itself
        GameCycle(player, ref spider, ref lifes, ref score, ref rain, ref flies, randomGenerator);
    }
}
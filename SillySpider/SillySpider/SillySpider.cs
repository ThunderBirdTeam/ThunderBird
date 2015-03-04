using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Security;
using System.Threading;
using System.Linq;

// Struct for printing each symbol
struct Symbol
{
    public int x;
    public int y;
    public string str;
    public ConsoleColor color;
}
//Exception to let the program know it has to start a new game
public class ContinuePlaying : Exception { }
//Exception to let the program know it has to end the game
public class StopPlaying : Exception { }
class Spider
{
    // A variable which tells the program whether to continue
    private static bool keepPlaying = true;
    // The player name
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
    private static List<Symbol> MovingDrops(SoundPlayer player, ref Symbol spider, ref int lifes, ref int score, List<Symbol> rain, List<Symbol> newListFlies)
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
                    PrintOnPosition(spider.x, spider.y - 2, spider.str = "DIED!", spider.color = ConsoleColor.Yellow);
                    PrintOnPosition(spider.x, spider.y - 1, spider.str = string.Format("  {0}", lifes), spider.color = ConsoleColor.Yellow);
                    PrintOnPosition(spider.x, spider.y, spider.str = "lives", spider.color = ConsoleColor.Yellow);
                    PrintOnPosition(spider.x, spider.y + 1, spider.str = "left!", spider.color = ConsoleColor.Yellow);

                    Thread.Sleep(2000);
                    Console.Clear();
                    newListDrops.Clear();
                    newListFlies.Clear();
                    rain.Clear();
                    player.Play();
                }

                // Game over keepPlaying is false -> exit the function
                else
                {
                    keepPlaying = false;
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

    //Method for storing results in file------------------>>Maria
    private static void Result(int score)
    {
        string totalScore = score.ToString();
        string[,] gameResult = new string[2, 1];
        gameResult[0, 0] = playerName;
        gameResult[1, 0] = totalScore;

        // Create a StreamWriter instance
        StreamWriter writer = new StreamWriter(@"..\..\currentPlayerScore.txt");
        //Printing result
        using (writer)
        {
            for (int row = 0; row < gameResult.GetLength(0); row++)
            {
                for (int col = 0; col < gameResult.GetLength(1); col++)
                {
                    writer.Write(gameResult[row, col] + " ");
                }
                Console.WriteLine();
            }
        }
    }
    //Compare current result file with old one and joins them in new text file--------------------->Maria
    private static void CompareResults()
    {
        System.Text.Encoding encodingCyr = System.Text.Encoding.GetEncoding(1251);

        try
        {
            string fileOne = @"..\..\currentPlayerScore.txt";
            StreamReader readOne = new StreamReader(fileOne, encodingCyr);


            string readFileOne = "";
            using (readOne)
            {
                readFileOne = readOne.ReadToEnd();
            }

            StreamWriter newFile = new StreamWriter(@"..\..\AllPlayersScore.txt", true, encodingCyr);

            using (newFile)
            {
                newFile.WriteLine(readFileOne);

            }

            Console.WriteLine("Done!");
        }
        catch (FileLoadException)
        {
            Console.WriteLine("Files not found.");
        }
    }

    //Method for printing the results ---------------------------------------->>>Maria<<<<<<<<<<<<<<<<<<<<<<<<<<<------------------
    private static void PrintingResults()
    {
        // Create an instance of StreamReader to read from a file
        StreamReader reader = new StreamReader(@"..\..\AllPlayersScore.txt");
        var scoreDict = new Dictionary<string, int>();
        int lineNumber = 0;

        // Read first line from the text file
        string line = reader.ReadLine();

        // Read the other lines from the text file
        while (line != null)
        {
            string[] lineSplit = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (scoreDict.ContainsKey(lineSplit[0]))
            {
                if (scoreDict[lineSplit[0]] < int.Parse(lineSplit[1]))
                {
                    scoreDict[lineSplit[0]] = int.Parse(lineSplit[1]);
                }
            }
            else
            {
                scoreDict.Add(lineSplit[0], int.Parse(lineSplit[1]));
            }

            line = reader.ReadLine();
        }
        // Creates a sorted dictionary by score, then by name from the last dictionary
        var sortedDict = scoreDict.OrderByDescending(w => w.Value)
                                  .ThenBy(w => w.Key)
                                  .ToDictionary(p => p.Key, p => p.Value);
        // Keeps only 30 values in the dictionary to prevent using more memory than needed
        while (sortedDict.Count > 30)
        {
            sortedDict.Remove(sortedDict.Keys.Last());
        }
        // Close the resource after you've finished using it
        reader.Close();
        // Erases the previous file and adds the elements of the dictionary to the file and saves it
        File.WriteAllText(@"..\..\AllPlayersScore.txt", String.Empty);
        StreamWriter newFile = new StreamWriter(@"..\..\AllPlayersScore.txt");
        using (newFile)
        {
            foreach (var v in sortedDict)
            {
                newFile.WriteLine("{0} {1}", v.Key, v.Value);
            }
        }
        // Finally prints the scoreboard element by element.
        Console.WriteLine("______________________________________________________________________");
        Console.SetCursorPosition(Console.WindowWidth / 2 - 6, Console.WindowHeight - 38 + lineNumber);
        Console.WriteLine("TOP SCORES");
        Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight - 36 + lineNumber);
        Console.WriteLine("     Nickname      Score");
        foreach (var t in sortedDict)
        {
            if (lineNumber < 30)
            {
                lineNumber++;
                Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight - 36 + lineNumber);
                Console.WriteLine("{0:00} - {1,-15} {2}", lineNumber, t.Key, t.Value);
            }
        }
        Console.SetCursorPosition(Console.WindowWidth / 2 - 28, Console.WindowHeight - 34 + lineNumber);
        Console.WriteLine("Press ENTER to start a new game or ESC to end the game");
        Console.WriteLine("______________________________________________________________________");

    }

    // While cycle of the game
    private static void GameCycle(SoundPlayer player, ref Symbol spider, ref int lifes, ref int score, ref List<Symbol> rain, ref List<Symbol> flies, Random randomGenerator)
    {
        while (keepPlaying)
        {
            Console.Clear();

            PrintStaticElementsPlayfield();

            RandomChanceFliesAndDrops(rain, flies, randomGenerator);

            List<Symbol> newListFlies = MovingFlies(ref spider, ref score, flies);
            flies = newListFlies;

            List<Symbol> newListDrops = MovingDrops(player, ref spider, ref lifes, ref score, rain, newListFlies);

            // keepPlaying is false, therefore the game ends
            if (keepPlaying == false)
            {
                SoundPlayer died = new SoundPlayer();
                PlaySound(@"..\..\..\Died.wav", died);
                died.Play();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2 - 2);
                Console.Write("GAME OVER");
                Thread.Sleep((int)3000);

                Result(score);//---------------------------->>>>Getting results to file
                CompareResults();
                Console.Clear();
                PrintingResults();//------------------------>>Printing all players score
                Thread.Sleep((int)1000);

                // After the scoreboard the player is given a choice between continuing and exiting
                // After his choice we exit the function
                ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
                do
                {
                    pressedKey = Console.ReadKey(true);
                    if (pressedKey.Key == ConsoleKey.Enter)
                    {
                        throw new ContinuePlaying();
                    }
                    else if (pressedKey.Key == ConsoleKey.Escape)
                    {
                        throw new StopPlaying();
                    }
                }
                while (pressedKey.Key != ConsoleKey.Enter && pressedKey.Key != ConsoleKey.Escape);
            }
            rain = newListDrops;

            PrintingDrops(rain);

            PrintingFlies(flies);

            PrintOnPosition(3, 2, "Player: " + playerName, ConsoleColor.Yellow);//------------------------>>>>>>>>>>>>>Maria<<<<<<<<<<<<
            PrintOnPosition(3, 3, "Score: " + score, ConsoleColor.Magenta);
            PrintOnPosition(3, 4, "Lives: " + lifes, ConsoleColor.Green);

            spider = MovingAndPrintingSpider(spider);
        }
    }
    // Loads up the Starting Screen and starts the game after ENTER is pressed
    private static void StartGame()
    {
        // Prints teh StartUp screen in 4 changing colors until the player presses ENTER to start the game
        Console.CursorVisible = false;
        do
        {
            int colorNumber = 0;
            while (!Console.KeyAvailable)
            {

                switch (colorNumber)
                {
                    case 0: Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case 1: Console.ForegroundColor = ConsoleColor.Green; break;
                    case 2: Console.ForegroundColor = ConsoleColor.Cyan; break;
                    case 3: Console.ForegroundColor = ConsoleColor.White; break;

                    default:
                        break;
                }
                StreamReader startUp = new StreamReader(@"..\..\..\StartUp.txt");
                string startScreen = startUp.ReadToEnd();
                Console.BufferHeight = Console.WindowHeight = 40;
                Console.BufferWidth = Console.WindowWidth = 70;
                Console.Clear();
                Console.Write(startScreen);
                colorNumber++;

                if (colorNumber == 4)
                {
                    colorNumber = 0;
                }
                Console.Write("                   Press ENTER to start playing!");
                Thread.Sleep((int)(750));
            }
        }
        while (Console.ReadKey(true).Key != ConsoleKey.Enter);

        // After ENTER is pressed, the player is promted to enter his name and start playing
        
        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            StreamReader startUp2 = new StreamReader(@"..\..\..\StartUp.txt");
            string startScreen2 = startUp2.ReadToEnd();
            Console.BufferHeight = Console.WindowHeight = 40;
            Console.BufferWidth = Console.WindowWidth = 70;
            Console.Write(startScreen2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("                   Enter player name: ");
            playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName) || playerName.Contains('\t') || playerName.Contains(' ') || playerName.Length > 15)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(16, Console.WindowHeight - 2);
                Console.Write("\r                   Enter player name: ");
                Thread.Sleep((int)500);
            }
        }
        // The player name cannot contain empty spaces or be more than 15 characters
        while (string.IsNullOrWhiteSpace(playerName) || playerName.Contains('\t') || playerName.Contains(' ') || playerName.Length > 15);
        Thread.Sleep((int)200);
        StartPlaying();
    }

    //EndGameScreen
    private static void EndScreen()
    {
        Console.Clear();
        StreamReader EndScreen = new StreamReader(@"..\..\..\EndScreen.txt");
        string end = EndScreen.ReadToEnd();
        Console.BufferHeight = Console.WindowHeight = 40;
        Console.BufferWidth = Console.WindowWidth = 70;
        Console.Write(end);
        Console.SetCursorPosition(Console.WindowWidth / 2 - 15, Console.WindowHeight - 1);
        return;
    }

    // Starts up the game
    private static void StartPlaying()
    {
        // Playing rain sound
        SoundPlayer player = new SoundPlayer(@"..\..\..\Storm.wav");
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

        // RandomGenerator of all random needs
        Random randomGenerator = new Random();

        // While cycle for the game itself
        try
        {
            GameCycle(player, ref spider, ref lifes, ref score, ref rain, ref flies, randomGenerator);
        }
        catch (ContinuePlaying)
        {
            Console.Clear();
            keepPlaying = true;
            StartGame();
        }
        catch (StopPlaying)
        {
            EndScreen();
        }
    }
    static void Main()
    {
        StartGame();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

// Структура за отпечатване на всеки един символ
struct Symbol
{
    public int x;
    public int y;
    public string str;
    public ConsoleColor color;
}
class Game
{
    // Отпечатване на капките дъжд и буболечката
    static void PrintOnPosition(int x, int y, string str,
        ConsoleColor color = ConsoleColor.Cyan)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(str);
    }

    static void Main()
    {
        // you need to copy the filepath here for your own pc else it won't work
        SoundPlayer player = new SoundPlayer(@"C:\\Users\Alexandrina\Desktop\storm.wav");
        player.PlayLooping();
       
        // Задаване на работното поле
        Console.BufferHeight = Console.WindowHeight = 40;
        Console.BufferWidth = Console.WindowWidth = 70;

        // Дефиниране на потребителската буболечка
        Symbol bug = new Symbol();
        bug.x = Console.WindowWidth / 2;
        bug.y = Console.WindowHeight - 3;

        // Дефиниране на лист, който ще съдържа всички капки дъжд
        List<Symbol> rain = new List<Symbol>();

        // Генератор на случайни числа за разположението на капките
        Random randomGenerator = new Random();

        // Безкраен цикъл за същинската игра
        while (true)
        {
            // Изчистване на конзолата
            Console.Clear();

            
            

            // Синьо небе
            Console.SetCursorPosition(0, 1);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            string blue = new string(' ', Console.WindowWidth);
            Console.WriteLine(blue);
            Console.ResetColor();

            // you need to copy the filepath here for your own pc else it won't work
            StreamReader readerClouds = new StreamReader(@"C:\Users\Alexandrina\Desktop\Clouds.txt");
            string contentClouds = readerClouds.ReadToEnd();
            Console.SetCursorPosition(1, 2);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(contentClouds);
            Console.ResetColor();

            // Зелена трева
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            string green = new string(' ', 1);
            Console.WriteLine(green);
            Console.ResetColor();

            // Декоративни паяжини
            // you need to copy the filepath here for your own pc else it won't work
            Console.SetCursorPosition(0, Console.WindowHeight - 16);
            Console.ForegroundColor = ConsoleColor.White;
            StreamReader readerWeb = new StreamReader(@"C:\Users\Alexandrina\Desktop\spiderWeb.txt");
            string contentWeb = readerWeb.ReadToEnd();
            Console.WriteLine(contentWeb);

            // Дефиниране на капка дъжд най-отгоре
            Symbol newDropTop = new Symbol();
            newDropTop.color = ConsoleColor.Cyan;
            newDropTop.x = randomGenerator.Next(0, Console.WindowWidth);
            newDropTop.y = -1;
            newDropTop.str = "/";
            rain.Add(newDropTop);

            // Дефиниране на капка дъжд най-отдясно
            Symbol newDropRight = new Symbol();
            newDropRight.color = ConsoleColor.Cyan;
            newDropRight.x = Console.WindowWidth - 1;
            newDropRight.y = randomGenerator.Next(0, Console.WindowHeight);
            newDropRight.str = "/";
            rain.Add(newDropRight);

            // Капки
            List<Symbol> newList = new List<Symbol>();
            Symbol movingDrop = new Symbol();
            for (int i = 0; i < rain.Count; i++)
            {
                // Преместване на капките
                if (rain[i].x - 1 >= 1 && rain[i].y + 1 < Console.WindowHeight)
                {
                    movingDrop.x = rain[i].x - 1;
                    movingDrop.y = rain[i].y + 1;
                    movingDrop.str = rain[i].str;
                    movingDrop.color = rain[i].color;
                    newList.Add(movingDrop);
                   
                }
                // Проверка дали някоя капка не ни е удавила
                if (movingDrop.x >= bug.x + 1
                    && movingDrop.x <= bug.x + 3
                    && movingDrop.y >= bug.y
                    && movingDrop.y <= bug.y + 1)
                {
                    // you need to copy the filepath here for your own pc else it won't work
                    player.Stop();
                    SoundPlayer playerDied = new SoundPlayer(@"C:\\Users\Alexandrina\Desktop\died.wav");
                    playerDied.Play();

                    PrintOnPosition(bug.x, bug.y - 1, bug.str = "~ ~ ~~~ ~ ~", bug.color = ConsoleColor.Blue);
                    PrintOnPosition(bug.x, bug.y, bug.str = " YOU DIED ", bug.color = ConsoleColor.Blue);
                    PrintOnPosition(bug.x, bug.y + 1, bug.str = "~ ~ ~~~ ~ ~", bug.color = ConsoleColor.Blue);
                    Thread.Sleep(2050);
                    Console.Clear();
                    newList.Clear();
                    rain.Clear();
                    player.Play();
                }
            }
            rain = newList;

            // Отпечатване на капките дъжд
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
                // Отпечатване на буболечката черна/невидима,
                // което оправя бъг с отпечатването при преместване
                PrintOnPosition(bug.x, bug.y, bug.str = @"_\o/_", bug.color = ConsoleColor.Black);
                PrintOnPosition(bug.x, bug.y + 1, bug.str = @"/(_)\", bug.color = ConsoleColor.Black);

                // Улавяне на натиснатите клавиши-стрелки
                // и преместване на буболечката
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

                // Отпечатване на буболечката червена, каквато трябва да бъде
                PrintOnPosition(bug.x, bug.y, bug.str = @"_\o/_", bug.color = ConsoleColor.Red);
                PrintOnPosition(bug.x, bug.y + 1, bug.str = @"/(_)\", bug.color = ConsoleColor.Red);

                // Изкуствено забавяне изпълнението на цикъла
                Thread.Sleep(15);
            }
        }
    }
}
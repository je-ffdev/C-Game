using System;

class Program
{
    static Random random = new Random();
    static bool shouldExit = false;
    static int height = Console.WindowHeight - 1;
    static int width = Console.WindowWidth - 5;
    static int playerX = 0;
    static int playerY = 0;
    static int foodX = 0;
    static int foodY = 0;
    static string[] states = { "('-')", "(^-^)", "(X_X)" };
    static string[] foods = { "@@@@@", "$$$$$", "#####" };
    static string player = states[0];
    static int food = 0;

    static void Main(string[] args)
    {
        InitializeGame();
        while (!shouldExit)
        {
            Move(enableTermination: true);
            if (PlayerConsumedFood())
            {
                ChangePlayer();
                ShowFood();
            }
        }
    }

    static bool TerminalResized()
    {
        return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
    }

    static void ShowFood()
    {
        food = random.Next(0, foods.Length);
        foodX = random.Next(0, width - player.Length);
        foodY = random.Next(0, height - 1);
        Console.SetCursorPosition(foodX, foodY);
        Console.Write(foods[food]);
    }

    static void ChangePlayer()
    {
        player = states[food];
        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }

    static void FreezePlayer()
    {
        System.Threading.Thread.Sleep(1000);
        player = states[0];
    }

    static void Move(bool enableTermination = false)
    {
        int lastX = playerX;
        int lastY = playerY;

        if (TerminalResized())
        {
            Console.Clear();
            Console.WriteLine("Console was resized. Program exiting.");
            shouldExit = true;
            return;
        }

        var keyInfo = Console.ReadKey(true);
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                playerY--;
                break;
            case ConsoleKey.DownArrow:
                playerY++;
                break;
            case ConsoleKey.LeftArrow:
                playerX--;
                break;
            case ConsoleKey.RightArrow:
                playerX++;
                break;
            case ConsoleKey.Escape:
                shouldExit = true;
                break;
            default:
                if (enableTermination)
                {
                    shouldExit = true;
                }
                break;
        }

        Console.SetCursorPosition(lastX, lastY);
        for (int i = 0; i < player.Length; i++)
        {
            Console.Write(" ");
        }

        playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
        playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }

    static void InitializeGame()
    {
        Console.CursorVisible = false;
        Console.Clear();
        ShowFood();
        Console.SetCursorPosition(0, 0);
        Console.Write(player);
    }

    static bool PlayerConsumedFood()
    {
        return playerX == foodX && playerY == foodY;
    }
}

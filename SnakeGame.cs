using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Program
    {
        static int width = 40;
        static int height = 20;
        static int score = 0;
        static bool gameOver = false;
        static Random rand = new Random();
        static (int, int) food;

        enum Direction { Up, Down, Left, Right }
        static Direction dir = Direction.Right;

        static List<(int, int)> snake = new List<(int, int)> { (20, 10) };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 2, height + 2);

            GenerateFood();

            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    ChangeDirection(key);
                }

                MoveSnake();
                CheckCollision();
                DrawGame();

                Thread.Sleep(100);
            }

            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine($"Game Over! Your score: {score}");
        }

        static void GenerateFood()
        {
            food = (rand.Next(0, width), rand.Next(0, height));
        }

        static void ChangeDirection(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (dir != Direction.Down) dir = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if (dir != Direction.Up) dir = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    if (dir != Direction.Right) dir = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (dir != Direction.Left) dir = Direction.Right;
                    break;
            }
        }

        static void MoveSnake()
        {
            (int, int) head = snake[0];
            (int, int) newHead = head;

            switch (dir)
            {
                case Direction.Up: newHead = (head.Item1, head.Item2 - 1); break;
                case Direction.Down: newHead = (head.Item1, head.Item2 + 1); break;
                case Direction.Left: newHead = (head.Item1 - 1, head.Item2); break;
                case Direction.Right: newHead = (head.Item1 + 1, head.Item2); break;
            }

            snake.Insert(0, newHead);

            if (newHead == food)
            {
                score++;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void CheckCollision()
        {
            (int, int) head = snake[0];

            if (head.Item1 < 0 || head.Item1 >= width || head.Item2 < 0 || head.Item2 >= height)
            {
                gameOver = true;
            }

            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[i] == head)
                {
                    gameOver = true;
                }
            }
        }

        static void DrawGame()
        {
            Console.Clear();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (snake.Contains((x, y)))
                    {
                        Console.Write("O");
                    }
                    else if ((x, y) == food)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }

            Console.SetCursorPosition(0, height);
            Console.Write($"Score: {score}");
        }
    }
}

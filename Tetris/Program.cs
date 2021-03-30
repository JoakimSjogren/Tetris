using System;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();

            game.Start();

            // game loop
            while (true)
            {
                // listen to key presses
                if (Console.KeyAvailable)
                {
                    var input = Console.ReadKey(true);

                    switch (input.Key)
                    {
                        // pause and resume the game with P
                        case ConsoleKey.P:

                            if (game.Paused)
                                game.Resume();
                            else
                                game.Pause();

                            break;

                        // send key presses to the game if it's not paused
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.RightArrow:
                        
                        if (!game.Paused)
                            game.Input(input.Key);

                        break;

                        // end the game with ESC
                        case ConsoleKey.Escape:

                            game.Stop();
                            return;
                    }
                }
            }
        }
    }
}

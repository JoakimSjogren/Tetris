using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Game
    {
        ScheduleTimer _timer;
        int tickDelay = 500;

        public static int width = 15;
        public static int height = 29;
        bool finished = true;
        bool gameOver = false;

        int score;

        List<int> boxes = new List<int>();

        Shape currentShape;
        public bool Paused { get; private set; }

        public void Start()
        {
            Console.CursorVisible = false;
            GenerateMap();
            RenderMap();
            NewShape();

            ScheduleNextTick();
        }

        public void Pause()
        {
            Paused = true;
            _timer.Pause();
        }

        public void Resume()
        {
            Paused = false;
            _timer.Resume();
        }

        public void Stop()
        {
            _timer.Pause();
            gameOver = true;
            Console.SetCursorPosition(width + width, Console.WindowHeight / 5);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine($"GAME OVER");
        }

        void ScheduleNextTick()
        {
            _timer = new ScheduleTimer(tickDelay, Tick);
        }

        void NewShape()
        {
            if (!gameOver)
            {
                currentShape = new Shape();

                //Checks for GameOver
                if (boxes[currentShape.rootPosition] != 0)
                {
                    DrawShape();
                    Stop();
                    return;
                }
                DrawShape();
            }
        }

        void UndrawShape()
        {
            int root = currentShape.rootPosition;
            var x = root % width;
            var y = (int)root / width;

            boxes[root] = 0;

            Console.SetCursorPosition(x+x, y);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("  ");

            for (int i = 0; i < currentShape.boxes.Count; i++)
            {
                var boxInt = root + currentShape.boxes[i];
                if (boxInt > boxes.Count)
                    break;

                var boxX = boxInt % width;
                var boxY = (int)boxInt / width;

                boxes[boxInt] = 0;

                Console.SetCursorPosition(boxX + boxX, boxY);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("  ");
            }
        }

        void DrawShape()
        {
            int root = currentShape.rootPosition;
            var x = root % width;
            var y = (int)root / width;

            boxes[root] = currentShape.colorInt +2;

            Console.SetCursorPosition(x+x, y);
            Console.BackgroundColor = currentShape.color;
            Console.Write("  ");

            for (int i = 0; i < currentShape.boxes.Count; i++)
            {
                var boxInt = root + currentShape.boxes[i];
                if (boxInt > boxes.Count)
                    break;

                var boxX = boxInt % width;
                var boxY = (int)boxInt / width;

                boxes[boxInt] = currentShape.colorInt + 2;

                Console.SetCursorPosition(boxX + boxX, boxY);
                Console.BackgroundColor = currentShape.color;
                Console.Write("  ");
            }
        }

        bool MoveShapeDown()
        {
            int root = currentShape.rootPosition;
            if (root + width > boxes.Count)
                return false;
            if (boxes[root + width] == 0)
            {
                for (int i = 0; i < currentShape.boxes.Count; i++)
                {
                    var boxInt = root + currentShape.boxes[i];
                    if (boxes[boxInt + width] != 0)
                    {
                        return false;
                    }
                }
                currentShape.rootPosition += width;
                return true;
            }
            return false;
        }

        bool MoveShapeRight()
        {
            if (finished)
            {
                finished = false;
                int root = currentShape.rootPosition;
                if (boxes[root + 1] == 0)
                {
                    for (int i = 0; i < currentShape.boxes.Count; i++)
                    {
                        var boxInt = root + currentShape.boxes[i];
                        if (boxes[boxInt + 1] != 0)
                        {
                            finished = true;
                            return false;
                        }
                    }
                    currentShape.rootPosition += 1;
                    finished = true;
                    return true;
                }
                finished = true;
                return false;
            }
            return false;
        }

        bool MoveShapeLeft()
        {
            if (finished)
            {
                finished = false;
                int root = currentShape.rootPosition;
                if (boxes[root - 1] == 0)
                {
                    for (int i = 0; i < currentShape.boxes.Count; i++)
                    {
                        var boxInt = root + currentShape.boxes[i];
                        if (boxes[boxInt - 1] != 0)
                        {
                            finished = true;
                            return false;
                        }
                    }

                    currentShape.rootPosition -= 1;
                    finished = true;
                    return true;
                }
                finished = true;
                return false;
            }
            return false;
        }

        void RotateShapeRight()
        {
            if (finished)
            {
                finished = false;
                int root = currentShape.rootPosition;


                //Check if shape can rotate else returns
                for (int i = 0; i < currentShape.boxes.Count; i++)
                {
                    var boxInt = root + currentShape.boxes[i];
                    int newBoxInt = RotateBox(boxInt, root);

                    bool boxIntIsInShape = false;
                    for (int j = 0; j < currentShape.boxes.Count; j++)
                    {

                        var box = root + currentShape.boxes[j];
                        if (root + newBoxInt == box)
                        {
                            boxIntIsInShape = true;
                        }
                    }
                    if (boxes[root + newBoxInt] != 0 && !boxIntIsInShape)
                    {
                        finished = true;
                        return;
                    }
                }

                UndrawShape();

                for (int i = 0; i < currentShape.boxes.Count; i++) //Loops through all boxes
                {
                    var boxInt = root + currentShape.boxes[i];

                    int newBoxInt = RotateBox(boxInt, root);
                    currentShape.boxes[i] = newBoxInt;

                }
                DrawShape();
                finished = true;
            }
        }

        static int RotateBox(int boxInt, int root)
        {
            if (boxInt == root - width)
            {
                return 1;
            }
            else if (boxInt == root + 1)
            {
                return width;
            }
            else if (boxInt == root + width)
            {
                return -1;
            }
            else if (boxInt == root - 1)
            {
                return -width;
            }
            else if (boxInt == root - width * 2)
            {
                return 2;
            }
            else if (boxInt == root + 2)
            {
                return width * 2;
            }
            else if (boxInt == root + width * 2)
            {
                return -2;
            }
            else if (boxInt == root - 2)
            {
                return -width * 2;
            }
            if (boxInt == root - width + 1)
            {
                return 1 + width;
            }
            else if (boxInt == root + 1 + width)
            {
                return width - 1;
            }
            else if (boxInt == root + width - 1)
            {
                return -1 - width;
            }
            else if (boxInt == root - 1 - width)
            {
                return -width + 1;
            }
            return 0;
        }
        void ManageShape()
        {
            if (finished)
            {
                finished = false;
                UndrawShape(); // Clears last iteration of the current shape.

                //Tries to move the shape down 1 step (root += width).
                bool canMoveDown = MoveShapeDown();

                DrawShape(); // Draws Shape

                if (!canMoveDown) //Checks if shape is at the bottom
                {
                    RowCheck(); //Checks for a full row
                    NewShape();
                }
                finished = true;
            }
        }

        void DrawBox(int boxInt, int boxColor)
        {
            var x = boxInt % width;
            var y = (int)boxInt / width;

            boxes[boxInt] = boxColor;
            Console.SetCursorPosition(x+x, y);
            Console.BackgroundColor = Shape.colorScheme[boxColor -2];
            Console.Write("  ");
        }
        void ClearBox(int boxInt)
        {
            var x = boxInt % width;
            var y = (int)boxInt / width;

            boxes[boxInt] = 0;

            Console.SetCursorPosition(x+x, y);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("  ");
        }

        void RightKeyPress()
        {
            UndrawShape(); // Clears last iteration of the current shape.

            //Tries to move the shape to the right 1 step (root += width).
            bool canMoveRight = MoveShapeRight();

            DrawShape(); // Draws Shape
        }

        void LeftKeyPress()
        {
            UndrawShape(); // Clears last iteration of the current shape.

            //Tries to move the shape to the left 1 step (root += width).
            bool canMoveLeft = MoveShapeLeft();

            DrawShape(); // Draws Shape
        }

        void RenderMap()
        {
            int boxInt = 0;
            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (boxes[boxInt] == 0)
                        Console.BackgroundColor = ConsoleColor.Blue;
                    else if (boxes[boxInt] == 1)
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    else if (boxes[boxInt] == 2)
                        Console.BackgroundColor = ConsoleColor.Red;

                    Console.SetCursorPosition(x+x, y);
                    Console.Write("  ");
                    boxInt++;
                }
                Console.WriteLine("");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        void RowCleared(int height) // Move all rows down
        {
            score++;
            for (int y = height; y > 0; y -= width)
            {
                
                for (int x = 1; x < width - 1; x++)
                {
                    int boxInt = y + x;

                    if (boxes[boxInt] != 0)
                    {
                        var colorInt = boxes[boxInt];
                        //Move box down
                        ClearBox(boxInt);
                        boxInt += width;
                        DrawBox(boxInt, colorInt);
                    }
                }
            }
        }

        void RowCheck()
        {
            for (int y = width; y < height * width -width; y += width)
            {
                bool fullRow = true;
                for (int x = 1; x < width -1; x++)
                {
                    int boxInt = y + x;

                    if (boxes[boxInt] == 0)
                    {
                        fullRow = false;
                    }
                }
                if (fullRow)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        int boxInt = y + x;
                        ClearBox(boxInt);
                    }
                    RowCleared(y);
                }
            }
        }
        void Iteration()
        {
            if (finished && !gameOver)
            {
                finished = false;
                Console.SetCursorPosition(width + width, Console.WindowHeight / 4);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"Score: {score}");
                finished = true;
            }
            ManageShape();
        }

        void GenerateMap() // Adds walls around the map
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == height-1)
                    {
                        boxes.Add(1);
                    }
                    else if (x == 0 || x == width-1)
                    {
                        boxes.Add(1);
                    }
                    else
                    {
                        boxes.Add(0);
                    }
                }
            }
        }

        void Tick()
        {
            Iteration();
            ScheduleNextTick();
        }

        public void Input(ConsoleKey key)
        {
            if (!gameOver)
            {
                if (key.ToString() == "RightArrow")
                {
                    RightKeyPress();
                }
                else if (key.ToString() == "LeftArrow")
                {
                    LeftKeyPress();
                }
                else if (key.ToString() == "DownArrow")
                {
                    ManageShape();
                }
                else if (key.ToString() == "UpArrow")
                {
                    RotateShapeRight();
                }
            }
        }
    }
}
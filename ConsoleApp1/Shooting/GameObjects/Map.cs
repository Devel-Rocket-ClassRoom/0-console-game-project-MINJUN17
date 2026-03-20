using System;
using Framework.Engine;

public class Map : GameObject
{
    public const int Left = 1;
    public const int Top = 3;
    public const int Right = 58;
    public const int Bottom = 27;
    public Map(Scene scene) : base(scene)
    {
        Name = "Map1";
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.FillRect(Left - 1, Top, Right - Left + 2, 2, bgColor:ConsoleColor.Red);
        buffer.FillRect(Left - 1, Bottom - Top + 2, Right - Left + 2, 2, bgColor: ConsoleColor.Red);
        buffer.FillRect(Left, Top, 4, Bottom - Top + 2, bgColor: ConsoleColor.Red);
        buffer.FillRect(Right - Left - 3, Top, 4, Bottom - Top + 2, bgColor: ConsoleColor.Red);
        buffer.DrawBox(Left - 1, Top - 1, Right - Left + 2, Bottom - Top + 3, ConsoleColor.White);
    }
    public static bool IsInBounds(int x, int y)
    {
        return x >= Left && x <= Right && y >= Top && y <= Bottom;
    }
    public bool IsInBounds((int X, int Y) position)
    {
        return IsInBounds(position.X, position.Y);
    }
}


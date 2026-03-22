using System;
using Framework.Engine;

public class Map : GameObject
{
    public const int Left = 1;
    public const int Top = 3;
    public const int Right = 58;
    public const int Bottom = 27;

    // 스폰존 두께
    private const int SpawnDepth = 3;

    public Map(Scene scene) : base(scene)
    {
        Name = "Map1";
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 외곽 테두리 (이중선)
        // 상단
        buffer.SetCell(Left - 1, Top - 1, '+', ConsoleColor.DarkCyan);
        buffer.SetCell(Right, Top - 1, '+', ConsoleColor.DarkCyan);
        buffer.SetCell(Left - 1, Bottom + 1, '+', ConsoleColor.DarkCyan);
        buffer.SetCell(Right, Bottom + 1, '+', ConsoleColor.DarkCyan);
        buffer.DrawHLine(Left, Top - 1, Right - Left, '=', ConsoleColor.DarkCyan);
        buffer.DrawHLine(Left, Bottom + 1, Right - Left, '=', ConsoleColor.DarkCyan);
        buffer.DrawVLine(Left - 1, Top, Bottom - Top + 1, '|', ConsoleColor.DarkCyan);
        buffer.DrawVLine(Right, Top, Bottom - Top + 1, '|', ConsoleColor.DarkCyan);

        // 스폰존 (경고 표시 - 어두운 배경 + 화살표)
        // 상단 스폰존
        for (int y = Top; y < Top + SpawnDepth; y++)
        {
            for (int x = Left; x < Right; x++)
            {
                buffer.SetCell(x, y, ' ', ConsoleColor.DarkGray, ConsoleColor.DarkRed);
            }
        }
        // 하단 스폰존
        for (int y = Bottom - SpawnDepth + 1; y <= Bottom; y++)
        {
            for (int x = Left; x < Right; x++)
            {
                buffer.SetCell(x, y, ' ', ConsoleColor.DarkGray, ConsoleColor.DarkRed);
            }
        }
        // 좌측 스폰존
        for (int y = Top; y <= Bottom; y++)
        {
            for (int x = Left; x < Left + SpawnDepth; x++)
            {
                buffer.SetCell(x, y, ' ', ConsoleColor.DarkGray, ConsoleColor.DarkRed);
            }
        }
        // 우측 스폰존
        for (int y = Top; y <= Bottom; y++)
        {
            for (int x = Right - SpawnDepth; x < Right; x++)
            {
                buffer.SetCell(x, y, ' ', ConsoleColor.DarkGray, ConsoleColor.DarkRed);
            }
        }

        // 스폰존 안쪽 경계선 (점선)
        int innerL = Left + SpawnDepth;
        int innerR = Right - SpawnDepth - 1;
        int innerT = Top + SpawnDepth;
        int innerB = Bottom - SpawnDepth;

        buffer.DrawHLine(innerL, innerT, innerR - innerL + 1, '.', ConsoleColor.DarkGray);
        buffer.DrawHLine(innerL, innerB, innerR - innerL + 1, '.', ConsoleColor.DarkGray);
        buffer.DrawVLine(innerL, innerT, innerB - innerT + 1, '.', ConsoleColor.DarkGray);
        buffer.DrawVLine(innerR, innerT, innerB - innerT + 1, '.', ConsoleColor.DarkGray);

        // 스폰존 코너 화살표 (몬스터 나오는 방향 표시)
        buffer.SetCell(Left + 1, Top, 'v', ConsoleColor.Red);
        buffer.SetCell(Right - 2, Top, 'v', ConsoleColor.Red);
        buffer.SetCell(Left + 1, Bottom, '^', ConsoleColor.Red);
        buffer.SetCell(Right - 2, Bottom, '^', ConsoleColor.Red);
        buffer.SetCell(Left, Top + 1, '>', ConsoleColor.Red);
        buffer.SetCell(Left, Bottom - 1, '>', ConsoleColor.Red);
        buffer.SetCell(Right - 1, Top + 1, '<', ConsoleColor.Red);
        buffer.SetCell(Right - 1, Bottom - 1, '<', ConsoleColor.Red);

        // 코너 장식
        buffer.SetCell(innerL, innerT, '+', ConsoleColor.DarkGray);
        buffer.SetCell(innerR, innerT, '+', ConsoleColor.DarkGray);
        buffer.SetCell(innerL, innerB, '+', ConsoleColor.DarkGray);
        buffer.SetCell(innerR, innerB, '+', ConsoleColor.DarkGray);
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
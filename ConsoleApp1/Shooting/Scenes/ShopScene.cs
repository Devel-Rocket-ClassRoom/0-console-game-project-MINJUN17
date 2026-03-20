using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class ShopScene() : Scene
{
    private int _left = 3;
    private int _right = 50;
    private int _topOffset = 2;
    public override void Load()
    {
    }

    public override void Unload()
    {
    }

    public override void Update(float deltaTime)
    {
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(_left + 3, 1, $"현재 골드:");
        buffer.WriteText(_left + 10, 1, $"{Player.Gold}G", ConsoleColor.DarkYellow);
        buffer.DrawBoxNoRightLine(_left, 1 + _topOffset, 30, 5, ConsoleColor.White);
        buffer.SetCell(27, 2 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(32, 3 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(24, 4 + _topOffset, '|', ConsoleColor.White);
        buffer.WriteText(_left + 3, 2 + _topOffset, "라이플(무기)");
        buffer.WriteText(_left + 12, 2 + _topOffset, "500G", ConsoleColor.DarkYellow);
        buffer.WriteText(_left + 3, 4 + _topOffset, "특징: 공격속도 빠름");

        buffer.DrawBoxNoRightLine(_left, 7 + _topOffset, 30, 5, ConsoleColor.White);
        buffer.WriteText(_left + 3, 8 + _topOffset, "샷건(무기)");
        buffer.WriteText(_left + 12, 8 + _topOffset, "800G", ConsoleColor.DarkYellow);
        buffer.WriteText(_left + 3, 10 + _topOffset, "특징: 세발씩 발사됨");
        buffer.SetCell(28, 8 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(32, 9 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(24, 10 + _topOffset, '|', ConsoleColor.White);

        buffer.DrawBoxNoRightLine(_left, 13 + _topOffset, 30, 5, ConsoleColor.White);
        buffer.WriteText(_left + 3, 14 + _topOffset, "달리기(패시브)");
        buffer.WriteText(_left + 12, 14 + _topOffset, "300G", ConsoleColor.DarkYellow);
        buffer.WriteText(_left +  3, 16 + _topOffset, "특징: 달리기 빨라짐");
        buffer.SetCell(26, 14 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(32, 15 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(24, 16 + _topOffset, '|', ConsoleColor.White);

        buffer.WriteText(3, 19 + _topOffset, "    /\\_/\\");
        buffer.WriteText(3, 20 + _topOffset, "   ( o o )");
        buffer.WriteText(3, 21 + _topOffset, "  (  =^= )");
        buffer.WriteText(15, 20 + _topOffset, "돈 줘");
    }
}


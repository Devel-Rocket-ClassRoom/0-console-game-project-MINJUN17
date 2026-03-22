using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class ShopScene : Scene
{
    public event GameAction NextStage;
    public event GameAction GoBoss;
    private int _productNumber;
    private Player _player;
    private bool _noMoney1;
    private bool _noMoney2;
    private bool _noMoney3;
    private bool _boughtThisVisit;

    private const int BoxX = 3;
    private const int BoxW = 30;
    private const int BoxH = 4;
    private const int Gap = 1;

    public ShopScene(Player player)
    {
        _player = player;
        _productNumber = 1;
        _noMoney1 = false;
        _noMoney2 = false;
        _noMoney3 = false;
        _boughtThisVisit = false;
    }

    public override void Load() { }
    public override void Unload() { }

    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.DownArrow))
        {
            _productNumber++;
            if (_productNumber > 3) _productNumber = 1;
        }
        if (Input.IsKeyDown(ConsoleKey.UpArrow))
        {
            _productNumber--;
            if (_productNumber < 1) _productNumber = 3;
        }
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            _noMoney1 = false;
            _noMoney2 = false;
            _noMoney3 = false;

            if (_productNumber == 1 && !_player.HasRifle)
            {
                if (_player.Gold >= 50)
                {
                    _player.SetWeapon(new Rifle());
                    _player.SpendGold(50);
                    _boughtThisVisit = true;
                }
                else { _noMoney1 = true; }
            }
            if (_productNumber == 2 && !_player.HasShotgun)
            {
                if (_player.Gold >= 50)
                {
                    _player.SetWeapon(new ShotGun());
                    _player.SpendGold(50);
                    _boughtThisVisit = true;
                }
                else { _noMoney2 = true; }
            }
            if (_productNumber == 3 && !_player.HasMoveFast)
            {
                if (_player.Gold >= 50)
                {
                    _player.MoveFast(2);
                    _player.SpendGold(50);
                    _boughtThisVisit = true;
                }
                else { _noMoney3 = true; }
            }
        }
        if (Input.IsKeyDown(ConsoleKey.S))
        {
            NextStage?.Invoke();
        }
        if (Input.IsKeyDown(ConsoleKey.Y))
        {
            GoBoss?.Invoke();
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 상단 골드
        buffer.WriteText(BoxX + 1, 1, "Gold:", ConsoleColor.Gray);
        buffer.WriteText(BoxX + 7, 1, $"{_player.Gold}G", ConsoleColor.DarkYellow);

        // 아이템 박스 3개
        int y1 = 3;
        int y2 = y1 + BoxH + Gap;
        int y3 = y2 + BoxH + Gap;

        DrawItemBox(buffer, BoxX, y1, "Rifle", "50G", "Fast fire rate", _player.HasRifle, _productNumber == 1);
        DrawItemBox(buffer, BoxX, y2, "Shotgun", "50G", "Fires 3 bullets", _player.HasShotgun, _productNumber == 2);
        DrawItemBox(buffer, BoxX, y3, "Sprint", "50G", "Move faster", _player.HasMoveFast, _productNumber == 3);

        // 고양이 상인
        int catY = 20;
        buffer.WriteText(3, catY, "    /\\_/\\", ConsoleColor.White);
        buffer.WriteText(3, catY + 1, "   ( o o )", ConsoleColor.White);
        buffer.WriteText(3, catY + 2, "  (  =^=  )", ConsoleColor.White);

        // 고양이 대사
        string catMsg = "Pay up!";
        if (_boughtThisVisit)
            catMsg = "Thanks!";
        if (_noMoney1 || _noMoney2 || _noMoney3)
            catMsg = "No money!";
        buffer.WriteText(16, catY + 1, catMsg, (_noMoney1 || _noMoney2 || _noMoney3) ? ConsoleColor.Red : ConsoleColor.White);

        // 조작법 (하단)
        int helpX = 25;
        int helpY = 24;
        buffer.WriteText(helpX, helpY, "ARROW  Select item", ConsoleColor.Yellow);
        buffer.WriteText(helpX, helpY + 1, "ENTER  Buy item", ConsoleColor.Green);
        buffer.WriteText(helpX, helpY + 2, "S      Back to battle", ConsoleColor.Cyan);
        buffer.WriteText(helpX, helpY + 3, "Y      Boss fight!", ConsoleColor.Red);
    }

    private void DrawItemBox(ScreenBuffer buffer, int x, int y, string name, string price, string desc, bool sold, bool selected)
    {
        ConsoleColor boxColor = selected ? ConsoleColor.White : ConsoleColor.DarkGray;
        ConsoleColor bgColor = selected ? ConsoleColor.DarkGray : ConsoleColor.Black;

        // DrawBox로 테두리
        buffer.DrawBox(x, y, BoxW, BoxH, boxColor);

        // 선택된 아이템이면 내부 배경 채우기
        if (selected)
        {
            buffer.FillRect(x + 1, y + 1, BoxW - 2, BoxH - 2, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
        }

        // 이름 + 가격
        buffer.WriteText(x + 2, y + 1, name, ConsoleColor.White, bgColor);
        buffer.WriteText(x + BoxW - price.Length - 2, y + 1, price, ConsoleColor.DarkYellow, bgColor);

        // 설명
        buffer.WriteText(x + 2, y + 2, desc, ConsoleColor.Gray, bgColor);

        // 구매 완료 표시
        if (sold)
        {
            buffer.WriteText(x + BoxW + 2, y + 1, "SOLD", ConsoleColor.Green);
        }
    }
}
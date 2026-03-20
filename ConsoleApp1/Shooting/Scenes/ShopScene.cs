using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class ShopScene : Scene
{
    public event GameAction NextStage;
    private int _productNumber;
    private int _left = 3;
    private int _right = 50;
    private int _topOffset = 2;
    private Player _player;
    private bool _noMoney1;
    private bool _noMoney2;
    private bool _noMoney3;

    public ShopScene(Player player)
    {
        _player = player;
        _productNumber = 1;
        _noMoney1 = false;
        _noMoney2 = false;
        _noMoney3 = false;
    }
    public override void Load()
    {
    }

    public override void Unload()
    {
    }

    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.DownArrow))
        {
            _productNumber++;
            if( _productNumber > 3)
            {
                _productNumber = 1;
            } 
        }
        if (Input.IsKeyDown(ConsoleKey.UpArrow))
        {
            _productNumber--;
            if (_productNumber < 1)
            {
                _productNumber = 3;
            }
        }
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            if(_productNumber == 1 && !_player.HasRifle)
            {
                if(_player.Gold >= 50)
                {
                    _player.SetWeapon(new Rifle());
                    _player.SpendGold(50);
                }
                else
                {
                    _noMoney1 = true;
                }
            }
            if (_productNumber == 2 && !_player.HasShotgun)
            {
                if (_player.Gold >= 50)
                {
                    _player.SetWeapon(new ShotGun());
                    _player.SpendGold(50);
                }
                else
                {
                    _noMoney2 = true;
                }
            }
            if (_productNumber == 3 && !_player.HasMoveFast)
            {
                if (_player.Gold >= 50)
                {
                    _player.MoveFast(2);
                    _player.SpendGold(50);
                }
                else
                {
                    _noMoney3 = true;
                }
            }
        }
        if (Input.IsKeyDown(ConsoleKey.Tab))
        {
            NextStage?.Invoke();
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(_left + 3, 1, $"현재 골드:");
        buffer.WriteText(_left + 10, 1, $"{_player.Gold}G", ConsoleColor.DarkYellow);
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
        buffer.WriteText(_left + 3, 16 + _topOffset, "특징: 달리기 빨라짐");
        buffer.SetCell(26, 14 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(32, 15 + _topOffset, '|', ConsoleColor.White);
        buffer.SetCell(24, 16 + _topOffset, '|', ConsoleColor.White);

        buffer.WriteText(3, 19 + _topOffset, "    /\\_/\\");
        buffer.WriteText(3, 20 + _topOffset, "   ( o o )");
        buffer.WriteText(3, 21 + _topOffset, "  (  =^= )");
        buffer.WriteText(15, 20 + _topOffset, "돈내놔");

        buffer.WriteText(26, 19 + _topOffset, "방향키: 구매아이템 변경",ConsoleColor.Yellow);
        buffer.WriteText(23, 20 + _topOffset, "Enter : 아이템 구매", ConsoleColor.Green);
        buffer.WriteText(26, 21 + _topOffset, "Tab   : 상점 나가기", ConsoleColor.Red);

        if (_player.HasRifle)
        {
            buffer.WriteText(40, 3 + _topOffset, "구입 완료");
            buffer.WriteText(15, 20 + _topOffset, "감사요");
        }
        if (_player.HasShotgun)
        {
            buffer.WriteText(40, 9 + _topOffset, "구입 완료");
            buffer.WriteText(15, 20 + _topOffset, "감사요");
        }
        if (_player.HasMoveFast)
        {
            buffer.WriteText(40, 15 + _topOffset, "구입 완료");
            buffer.WriteText(15, 20 + _topOffset, "감사요");
        }

        if (_noMoney1 || _noMoney2 || _noMoney3)
        {
            buffer.WriteText(15, 20 + _topOffset, "돈 벌어",ConsoleColor.Red);
        }


        if (_productNumber == 1)
        {
            buffer.FillRect(_left+1, 2 + _topOffset, 21, 3, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
            buffer.FillRect(25, 2 + _topOffset, 2, 1, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
            buffer.FillRect(25, 3 + _topOffset, 7, 1, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);


            
            buffer.SetCell(27, 2 + _topOffset, '|', ConsoleColor.White);
            buffer.SetCell(32, 3 + _topOffset, '|', ConsoleColor.White);
            buffer.SetCell(24, 4 + _topOffset, '|', ConsoleColor.White);
            buffer.WriteText(_left + 3, 2 + _topOffset, "라이플(무기)", bgColor: ConsoleColor.DarkGray);
            buffer.WriteText(_left + 12, 2 + _topOffset, "500G", ConsoleColor.DarkYellow, bgColor: ConsoleColor.DarkGray);
            buffer.WriteText(_left + 3, 4 + _topOffset, "특징: 공격속도 빠름", bgColor: ConsoleColor.DarkGray);
        }
        else if(_productNumber == 2)
        {
            buffer.FillRect(_left + 1, 8 + _topOffset, 21, 3, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
            buffer.FillRect(25, 8 + _topOffset, 3, 1, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
            buffer.FillRect(25, 9 + _topOffset, 7, 1, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);

            buffer.WriteText(_left + 3, 8 + _topOffset, "샷건(무기)", bgColor: ConsoleColor.DarkGray);
            buffer.WriteText(_left + 12, 8 + _topOffset, "800G", ConsoleColor.DarkYellow, bgColor: ConsoleColor.DarkGray);
            buffer.WriteText(_left + 3, 10 + _topOffset, "특징: 세발씩 발사됨", bgColor: ConsoleColor.DarkGray);
            buffer.SetCell(28, 8 + _topOffset, '|', ConsoleColor.White);
            buffer.SetCell(32, 9 + _topOffset, '|', ConsoleColor.White);
            buffer.SetCell(24, 10 + _topOffset, '|', ConsoleColor.White);
        }
        else if(_productNumber == 3)
        {
            buffer.FillRect(_left + 1, 14 + _topOffset, 21, 3, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
            buffer.FillRect(25, 14 + _topOffset, 2, 1, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);
            buffer.FillRect(25, 15 + _topOffset, 7, 1, ' ', ConsoleColor.Gray, ConsoleColor.DarkGray);

            buffer.WriteText(_left + 3, 14 + _topOffset, "달리기(패시브)", bgColor: ConsoleColor.DarkGray);
            buffer.WriteText(_left + 12, 14 + _topOffset, "300G", ConsoleColor.DarkYellow, bgColor: ConsoleColor.DarkGray);
            buffer.WriteText(_left + 3, 16 + _topOffset, "특징: 달리기 빨라짐", bgColor: ConsoleColor.DarkGray);
            buffer.SetCell(26, 14 + _topOffset, '|', ConsoleColor.White);
            buffer.SetCell(32, 15 + _topOffset, '|', ConsoleColor.White);
            buffer.SetCell(24, 16 + _topOffset, '|', ConsoleColor.White);
        }

    }
}


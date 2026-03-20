using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Player : GameObject
{
    private Position _playerPosition;
    public Position PlayerPosition => _playerPosition;
    public Direction CurrentDirection { get; private set; }

    private const float k_MoveInterval = 0.1f;
    private float _moveTimer;
    public Weapon Weapon { get; private set; }

    public Action<Position, Direction> OnFire;
    public static int Gold { get; private set; }

    public Player(Scene scene, Position startPosition) : base(scene)
    {
        Name = "Player";
        Gold = 0;
        Weapon = new Pistol();
        _playerPosition = startPosition;
        _moveTimer = 0;
        CurrentDirection = Direction.Up;
    }

    public override void Update(float deltaTime)
    {
        Weapon.Update(deltaTime);
        if (Input.IsKey(ConsoleKey.Spacebar))
        {
            Weapon.TryFire((PlayScene)Scene, PlayerPosition, CurrentDirection);
        }
        _moveTimer += deltaTime;
        if (_moveTimer > k_MoveInterval)
        {
            Move();
            _moveTimer = 0f;
        }

    }

    public override void Draw(ScreenBuffer buffer)
    {
        int x = _playerPosition.X;
        int y = _playerPosition.Y;
        if (CurrentDirection == Direction.Down || CurrentDirection == Direction.DownLeft)
        {
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┏', ConsoleColor.Yellow);
            buffer.SetCell(x, y + 1, '┼', ConsoleColor.DarkGray);

        }
        else if (CurrentDirection == Direction.Up || CurrentDirection == Direction.UpRight)
        {
            buffer.SetCell(x - 2, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┛', ConsoleColor.Yellow);
            buffer.SetCell(x, y - 1, '┼', ConsoleColor.DarkGray);
        }
        else if (CurrentDirection == Direction.Left || CurrentDirection == Direction.UpLeft)
        {
            buffer.SetCell(x, y + 1, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y + 1, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┓', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '━', ConsoleColor.Yellow);

            buffer.SetCell(x - 2, y, '┼', ConsoleColor.DarkGray);
            buffer.SetCell(x - 3, y, '─', ConsoleColor.DarkGray);
        }
        else if (CurrentDirection == Direction.Right || CurrentDirection == Direction.DownRight)
        {
            buffer.SetCell(x, y - 1, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y - 1, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┗', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '━', ConsoleColor.Yellow);

            buffer.SetCell(x + 3, y, '─', ConsoleColor.DarkGray);
            buffer.SetCell(x + 2, y, '┼', ConsoleColor.DarkGray);
        }
    }

    private void Move()
    {
        int dx = 0;
        int dy = 0;

        if (Input.IsKey(ConsoleKey.LeftArrow)) dx--;
        if (Input.IsKey(ConsoleKey.RightArrow)) dx++;
        if (Input.IsKey(ConsoleKey.UpArrow)) dy--;
        if (Input.IsKey(ConsoleKey.DownArrow)) dy++;

        if (dx == -1 && dy == -1) CurrentDirection = Direction.UpLeft;
        else if (dx == 1 && dy == -1) CurrentDirection = Direction.UpRight;
        else if (dx == -1 && dy == 1) CurrentDirection = Direction.DownLeft;
        else if (dx == 1 && dy == 1) CurrentDirection = Direction.DownRight;
        else if (dx == 0 && dy == -1) CurrentDirection = Direction.Up;
        else if (dx == 0 && dy == 1) CurrentDirection = Direction.Down;
        else if (dx == -1 && dy == 0) CurrentDirection = Direction.Left;
        else if (dx == 1 && dy == 0) CurrentDirection = Direction.Right;

        int nextX = _playerPosition.X + dx;
        int nextY = _playerPosition.Y + dy;
        Position nextPosition = new Position(nextX, nextY);
        if (!Map1.IsInBounds(nextX, nextY))
        {
            return;
        }
        _playerPosition = nextPosition;
    }
    public void SetWeapon(Weapon weapon)
    {
        Weapon = weapon;
    }
    public Rect PlayerRect(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
            case Direction.UpRight:
                return new Rect
                {
                    X = _playerPosition.X - 2,
                    Y = _playerPosition.Y,
                    Width = 3,  
                    Height = 1
                };

            case Direction.Down:
            case Direction.DownLeft:
                return new Rect
                {
                    X = _playerPosition.X,
                    Y = _playerPosition.Y,
                    Width = 3,  
                    Height = 1
                };

            case Direction.Left:
            case Direction.UpLeft:
                return new Rect
                {
                    X = _playerPosition.X - 1,
                    Y = _playerPosition.Y,
                    Width = 2,  
                    Height = 2  
                };

            case Direction.Right:
            case Direction.DownRight:
                return new Rect
                {
                    X = _playerPosition.X,
                    Y = _playerPosition.Y - 1,
                    Width = 2,  
                    Height = 2 
                };

            default:
                return new Rect();
        }
    }
    public void GetGold(int amount)
    {
        Gold += amount;
    }
    public void SpendGold(int amount)
    {
        if(Gold < amount)
        {
            return;
        }
        Gold -= amount;
    }
}

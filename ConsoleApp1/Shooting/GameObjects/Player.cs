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

    private float _basemoveInterval = 0.1f;
    private float _currentMoveInterval;
    private float _moveTimer;
    private float _speedBuffTimer;
    public Weapon Weapon { get; private set; }
    private Scene Scene;

    public Action<Position, Direction> OnFire;
    public int Gold { get; private set; }

    public bool HasRifle {  get; private set; }
    public bool HasShotgun {  get; private set; }
    public bool HasMoveFast { get; private set; }

    public Player(Scene scene, Position startPosition) : base(scene)
    {
        Name = "Player";
        _speedBuffTimer = 0;
        Gold = 0;
        HasRifle = false;
        HasShotgun = false;
        HasMoveFast = false;
        Weapon = new Pistol();
        _playerPosition = startPosition;
        _moveTimer = 0;
        _currentMoveInterval = _basemoveInterval;
        CurrentDirection = Direction.Up;
    }

    public override void Update(float deltaTime)
    {
        Weapon.Update(deltaTime);
        if (Input.IsKey(ConsoleKey.Spacebar))
        {
            Weapon.TryFire((PlayScene)Scene, PlayerPosition, CurrentDirection);
        }
        if (_speedBuffTimer > 0)
        {
            _speedBuffTimer -= deltaTime;

            if (_speedBuffTimer <= 0 && !HasMoveFast)
            {
                _currentMoveInterval = _basemoveInterval;
            }
        }
        _moveTimer += deltaTime;
        if (_moveTimer > _currentMoveInterval)
        {
            Move();
            _moveTimer = 0f;
        }

    }

    public override void Draw(ScreenBuffer buffer)
    {
        int x = _playerPosition.X;
        int y = _playerPosition.Y;
        if (CurrentDirection == Direction.Down)
        {
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┏', ConsoleColor.Yellow);
            buffer.SetCell(x, y + 1, '┼', ConsoleColor.DarkGray);

        }
        else if (CurrentDirection == Direction.Up)
        {
            buffer.SetCell(x - 2, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┛', ConsoleColor.Yellow);
            buffer.SetCell(x, y - 1, '┼', ConsoleColor.DarkGray);
        }
        else if (CurrentDirection == Direction.Left)
        {
            buffer.SetCell(x, y + 1, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y + 1, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y, '┓', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '━', ConsoleColor.Yellow);

            buffer.SetCell(x - 2, y, '┼', ConsoleColor.DarkGray);
            buffer.SetCell(x - 3, y, '─', ConsoleColor.DarkGray);
        }
        else if (CurrentDirection == Direction.Right)
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

        if (dx == 0 && dy == -1) CurrentDirection = Direction.Up;
        if (dx == 0 && dy == 1) CurrentDirection = Direction.Down;
        if (dx == -1 && dy == 0) CurrentDirection = Direction.Left;
        if (dx == 1 && dy == 0) CurrentDirection = Direction.Right;

        int nextX = _playerPosition.X + dx;
        int nextY = _playerPosition.Y + dy;
        Position nextPosition = new Position(nextX, nextY);
        if (!Map.IsInBounds(nextX, nextY))
        {
            return;
        }
        _playerPosition = nextPosition;
    }
    public void SetWeapon(Weapon weapon)
    {
        Weapon = weapon;
        if(weapon is Rifle)
        {
            HasRifle = true;
        }
        else if (weapon is ShotGun)
        {
            HasShotgun = true;
        }
    }
    public Rect PlayerRect(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Rect
                {
                    X = _playerPosition.X - 2,
                    Y = _playerPosition.Y,
                    Width = 3,  
                    Height = 1
                };

            case Direction.Down:
                return new Rect
                {
                    X = _playerPosition.X,
                    Y = _playerPosition.Y,
                    Width = 3,  
                    Height = 1
                };

            case Direction.Left:
                return new Rect
                {
                    X = _playerPosition.X - 1,
                    Y = _playerPosition.Y,
                    Width = 2,  
                    Height = 2  
                };

            case Direction.Right:
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
    public void SetScene(Scene scene)
    {
        this.Scene = scene;
    }
    public void Reset()
    {
        Gold = 0;
        _speedBuffTimer = 0;
        _basemoveInterval = 0.1f;
        Weapon = new Pistol();
        HasRifle = false;
        HasShotgun = false;
        HasMoveFast = false;
        _currentMoveInterval = _basemoveInterval;
    }
    public void SpeedReset()
    {
        _speedBuffTimer = 0;
    }
    public void MoveFast(float amount)
    {
        _currentMoveInterval = _basemoveInterval / amount;
        HasMoveFast = true;
    }
    public void MoveFast(float amount, float buffTime)
    {
        _currentMoveInterval = _basemoveInterval / amount;
        _speedBuffTimer = buffTime;
    }
    public void SetPosition(Position position)
    {
        _playerPosition = position;
    }
}

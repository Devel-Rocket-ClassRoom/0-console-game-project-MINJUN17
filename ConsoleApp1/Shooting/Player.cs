using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class Player : GameObject
{
    private const float k_MoveInterval = 0.1f;
    private (int X, int Y) _playerBody;
    private Scene _scene;
    public (int X, int Y) PlayerBody => _playerBody;
    private float _moveTimer;
    public Direction Direction;
    public Player(Scene scene, int startX, int startY) : base(scene)
    {
        Name = "Player";

        _playerBody = (startX, startY);
        _moveTimer = 0;
        Direction = Direction.Up;
    }
    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;
        if (_moveTimer > k_MoveInterval)
        {
            Move();
            _moveTimer = 0f;
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        int x = _playerBody.X;
        int y = _playerBody.Y;
        //┏ ┓ ┗ ┛
        if (Direction == Direction.Down)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '┏', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y + 1, '┼', ConsoleColor.Yellow);
        }
        else if (Direction == Direction.Up)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y, '┛', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y - 1, '┼', ConsoleColor.Yellow);
        }
        else if( Direction == Direction.Left)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x, y - 1, '┓', ConsoleColor.Yellow);
            buffer.SetCell(x -1, y - 1, '━', ConsoleColor.Yellow);
            buffer.SetCell(x - 2, y - 1, '┼', ConsoleColor.Yellow);
            buffer.SetCell(x - 3, y - 1, '─', ConsoleColor.Yellow);
        }
        else if (Direction == Direction.Right)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);

            buffer.SetCell(x, y + 1, '┗', ConsoleColor.Yellow);

            buffer.SetCell(x + 1, y + 1, '━', ConsoleColor.Yellow);

            buffer.SetCell(x + 3, y + 1, '─', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y + 1, '┼', ConsoleColor.Yellow);
        }
    }
    private void Move()
    {
        int nextX = _playerBody.X;
        int nextY = _playerBody.Y;
        if (Input.IsKey(ConsoleKey.UpArrow))
        {
            nextY -= 1;
            Direction = Direction.Up;
        }
        if (Input.IsKey(ConsoleKey.DownArrow))
        {
            nextY += 1;
            Direction = Direction.Down;
        }
        if (Input.IsKey(ConsoleKey.LeftArrow))
        {
            nextX -= 1;
            Direction = Direction.Left;
        }
        if (Input.IsKey(ConsoleKey.RightArrow))
        {
            nextX += 1;
            Direction = Direction.Right;
        }
        if(!Map1.IsInBounds(nextX, nextY))
        {
            return;
        }
        _playerBody.X = nextX;
        _playerBody.Y = nextY;
    }
}

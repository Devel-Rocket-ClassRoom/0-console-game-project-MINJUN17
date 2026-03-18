using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class Player : GameObject
{
    private const float k_MoveInterval = 0.1f;
    private (int X, int Y) _playerBody;
    private float _moveTimer;
    private Direction _direction;
    public Player(Scene scene, int startX, int startY) : base(scene)
    {
        Name = "Player";

        _playerBody = (startX, startY);
        _moveTimer = 0;
        _direction = Direction.Up;
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
        if (_direction == Direction.Down)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            //buffer.SetCell(x + 2, y, '┓', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '┏', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y + 1, '┼', ConsoleColor.Yellow);
        }
        else if (_direction == Direction.Up)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y, '┛', ConsoleColor.Yellow);
            //buffer.SetCell(x - 1, y, '┗', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y - 1, '┼', ConsoleColor.Yellow);
        }
        else if( _direction == Direction.Left)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x, y - 1, '┓', ConsoleColor.Yellow);
            //buffer.SetCell(x, y + 1, '┛', ConsoleColor.Yellow);
            buffer.SetCell(x -1, y - 1, '━', ConsoleColor.Yellow);
            //buffer.SetCell(x - 1, y + 1, '━', ConsoleColor.Yellow);
            buffer.SetCell(x - 2, y - 1, '┼', ConsoleColor.Yellow);
            buffer.SetCell(x - 3, y - 1, '─', ConsoleColor.Yellow);
        }
        else if (_direction == Direction.Right)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);

            //buffer.SetCell(x, y - 1, '┏', ConsoleColor.Yellow);
            buffer.SetCell(x, y + 1, '┗', ConsoleColor.Yellow);

            //buffer.SetCell(x + 1, y - 1, '━', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y + 1, '━', ConsoleColor.Yellow);

            buffer.SetCell(x + 3, y + 1, '─', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y + 1, '┼', ConsoleColor.Yellow);
        }
    }
    private void Move()
    {
        if (Input.IsKey(ConsoleKey.UpArrow))
        {
            _playerBody.Y -= 1;
            _direction = Direction.Up;
        }
        if (Input.IsKey(ConsoleKey.DownArrow))
        {
            _playerBody.Y += 1;
            _direction = Direction.Down;
        }
        if (Input.IsKey(ConsoleKey.LeftArrow))
        {
            _playerBody.X -= 1;
            _direction = Direction.Left;
        }
        if (Input.IsKey(ConsoleKey.RightArrow))
        {
            _playerBody.X += 1;
            _direction = Direction.Right;
        }

    }
}

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
        if (Direction == Direction.Down || Direction == Direction.DownLeft)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '┏', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y + 1, '┼', ConsoleColor.Yellow);
        }
        else if (Direction == Direction.Up || Direction == Direction.UpRight)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y, '┛', ConsoleColor.Yellow);
            buffer.SetCell(x + 2, y - 1, '┼', ConsoleColor.Yellow);
        }
        else if( Direction == Direction.Left || Direction == Direction.UpLeft)
        {
            buffer.SetCell(x, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x - 1, y, '█', ConsoleColor.Yellow);
            buffer.SetCell(x, y - 1, '┓', ConsoleColor.Yellow);
            buffer.SetCell(x -1, y - 1, '━', ConsoleColor.Yellow);
            buffer.SetCell(x - 2, y - 1, '┼', ConsoleColor.Yellow);
            buffer.SetCell(x - 3, y - 1, '─', ConsoleColor.Yellow);
        }
        else if (Direction == Direction.Right || Direction == Direction.DownRight)
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
        int dx = 0;
        int dy = 0;

        if (Input.IsKey(ConsoleKey.LeftArrow)) dx--;
        if (Input.IsKey(ConsoleKey.RightArrow)) dx++;
        if (Input.IsKey(ConsoleKey.UpArrow)) dy--;
        if (Input.IsKey(ConsoleKey.DownArrow)) dy++;

        // 방향 설정
        if (dx == -1 && dy == -1) Direction = Direction.UpLeft;
        else if (dx == 1 && dy == -1) Direction = Direction.UpRight;
        else if (dx == -1 && dy == 1) Direction = Direction.DownLeft;
        else if (dx == 1 && dy == 1) Direction = Direction.DownRight;
        else if (dx == 0 && dy == -1) Direction = Direction.Up;
        else if (dx == 0 && dy == 1) Direction = Direction.Down;
        else if (dx == -1 && dy == 0) Direction = Direction.Left;
        else if (dx == 1 && dy == 0) Direction = Direction.Right;

        int nextX = _playerBody.X + dx;
        int nextY = _playerBody.Y + dy;

        if (!Map1.IsInBounds(nextX, nextY))
            return;

        _playerBody = (nextX, nextY);
    }
}

using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class Player : GameObject
{
    private const float k_MoveInterval = 0.1f;
    private (int X, int Y) _playerBody;
    private float _moveTimer;
    public Player(Scene scene, int startX, int startY) : base(scene)
    {
        Name = "Player";

        _playerBody = (startX, startY);
        _moveTimer = 0;
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

        buffer.SetCell(x, y, 'O', ConsoleColor.Yellow);
        buffer.SetCell(x + 1, y, '-', ConsoleColor.Yellow);
        buffer.SetCell(x + 2, y, '>', ConsoleColor.Yellow);

        buffer.SetCell(x - 1, y + 1, '/', ConsoleColor.Yellow);
        buffer.SetCell(x, y + 1, '|', ConsoleColor.Yellow);

        buffer.SetCell(x - 1, y + 2, '/', ConsoleColor.Yellow);
        buffer.SetCell(x + 1, y + 2, '\\', ConsoleColor.Yellow);
    }
    private void Move()
    {
        if (Input.IsKey(ConsoleKey.UpArrow))
        {
            _playerBody.Y -= 1;
        }
        if (Input.IsKey(ConsoleKey.DownArrow))
        {
            _playerBody.Y += 1;
        }
        if (Input.IsKey(ConsoleKey.LeftArrow))
        {
            _playerBody.X -= 1;
        }
        if (Input.IsKey(ConsoleKey.RightArrow))
        {
            _playerBody.X += 1;
        }

    }
}

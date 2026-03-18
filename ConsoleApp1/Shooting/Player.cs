using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class Player : GameObject
{
    private const float k_MoveInterval = 0.15f;
    private (int X, int Y) _playerBody;
    public Player(Scene scene, int startX, int startY) : base(scene)
    {
        Name = "Player";

        _playerBody = (startX, startY);
    }
    public Player(Scene scene) : base(scene)
    {
    }
    public override void Update(float deltaTime)
    {
        Move();
    }
    public override void Draw(ScreenBuffer buffer)
    {

    }
    private void Move()
    {
        if (Input.IsKeyDown(ConsoleKey.UpArrow))
        {
            _playerBody.Y -= 1;
        }
        else if (Input.IsKeyDown(ConsoleKey.DownArrow))
        {
            _playerBody.Y += 1;
        }
        else if (Input.IsKeyDown(ConsoleKey.LeftArrow))
        {
            _playerBody.X -= 1;
        }
        else if (Input.IsKeyDown(ConsoleKey.RightArrow))
        {
            _playerBody.X += 1;
        }

    }
}

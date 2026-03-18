using Framework.Engine;
using System;
using static System.Net.Mime.MediaTypeNames;

public class Bullet : GameObject
{
    private Direction _direction;
    private const float k_MoveInterval = 0.05f;
    private float _moveTimer;
    private (int X, int Y) _bulletPosition;
    public Bullet(Scene scene, int positionX, int positionY, Direction direction) : base(scene)
    {
        _direction = direction;
        _bulletPosition = (positionX,  positionY);
    }


    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;
        if(_moveTimer > k_MoveInterval)
        {
            Move();
            _moveTimer = 0;
        }
    }
    public void Move()
    {
        int nextX = _bulletPosition.X;
        int nextY = _bulletPosition.Y;
        if (_direction == Direction.Left)
        {
            nextX -= 1;
        }
        else if(_direction == Direction.Right)
        {
            nextX += 1;
        }
        else if( _direction == Direction.Up)
        {
            nextY -= 1;
        }
        else if (_direction == Direction.Down)
        {
            nextY += 1;
        }
        if (!Map1.IsInBounds(nextX, nextY))
        {
            return;
        }
        _bulletPosition.X = nextX;
        _bulletPosition.Y = nextY;
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(_bulletPosition.X, _bulletPosition.Y, '•', ConsoleColor.Gray);
    }

}

using Framework.Engine;
using System;
using static System.Net.Mime.MediaTypeNames;

public class Bullet : GameObject
{
    private Direction _direction;
    private float _moveTimer;
    private Position _bulletPosition;
    public Position BulletPosition => _bulletPosition;
    public bool IsAlive { get; private set; }
    public Bullet(Scene scene, Position postion, Direction direction) : base(scene)
    {
        _bulletPosition = postion;
        _direction = direction;

        _moveTimer = 0;
        IsAlive = true;
    }


    public override void Update(float deltaTime)
    {
        Move();
        if (!IsAlive)
        {
            IsActive = false;
            Scene.RemoveGameObject(this);
        }
    }
    public void Move()
    {
        int dx = 0;
        int dy = 0;

        switch (_direction)
        {
            case Direction.Up: dx = 0; dy = -1; break;
            case Direction.Down: dx = 0; dy = 1; break;
            case Direction.Left: dx = -1; dy = 0; break;
            case Direction.Right: dx = 1; dy = 0; break;
        }
        _bulletPosition.X += dx;
        _bulletPosition.Y += dy;
        if (!Map.IsInBounds(_bulletPosition.X, _bulletPosition.Y))
        {
            IsAlive = false;
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(_bulletPosition.X, _bulletPosition.Y, '•', ConsoleColor.Gray);
    }
}
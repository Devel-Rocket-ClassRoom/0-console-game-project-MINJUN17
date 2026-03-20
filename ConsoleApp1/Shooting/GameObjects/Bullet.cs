using Framework.Engine;
using System;
using static System.Net.Mime.MediaTypeNames;

public class Bullet : GameObject
{
    private Direction _direction;
    private const float k_MoveInterval = 0.01f;
    private float _moveTimer;
    private Position _bulletPosition;
    public Position BulletPosition => _bulletPosition;
    public Direction Direction { get; }
    public bool IsAlive { get; private set; }
    public Bullet(Scene scene, Position postion, Direction direction) : base(scene)
    {
        _bulletPosition = postion;
        Direction = direction;

        _moveTimer = 0;
        IsAlive = true;
    }


    public override void Update(float deltaTime)
    {

        _moveTimer += deltaTime;
        if (_moveTimer > k_MoveInterval)
        {
            Move();
            _moveTimer = 0;
        }
        if (!IsAlive)
        {
            Scene.RemoveGameObject(this);
        }
    }
    public void Move()
    {
        int dx = 0;
        int dy = 0;

        switch (Direction)
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

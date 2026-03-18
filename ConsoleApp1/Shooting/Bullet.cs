using Framework.Engine;
using System;
using static System.Net.Mime.MediaTypeNames;

public class Bullet : GameObject
{
    private Direction _direction;
    private const float k_MoveInterval = 0.01f;
    private float _moveTimer;
    private (int X, int Y) _bulletPosition;
    private int _dx;
    private int _dy;
    public bool IsAlive { get; private set; }
    public Bullet(Scene scene, int x, int y, int dx, int dy) : base(scene)
    {
        _bulletPosition = (x, y);
        _dx = dx;
        _dy = dy;

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
        int nextX = _bulletPosition.X + _dx;
        int nextY = _bulletPosition.Y + _dy;

        _bulletPosition = (nextX, nextY);

        if (!Map1.IsInBounds(nextX, nextY))
        {
            IsAlive = false;
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(_bulletPosition.X, _bulletPosition.Y, '•', ConsoleColor.Gray);
    }

}

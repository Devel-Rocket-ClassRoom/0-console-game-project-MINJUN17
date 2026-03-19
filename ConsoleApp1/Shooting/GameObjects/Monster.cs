using Framework.Engine;
using System;
using System.Numerics;

public class Monster : GameObject
{
    private Position _monsterPostion;
    private Player _player;
    private int _monsterHeight = 2;
    private int _monsterWidth = 4;
    private const float k_MoveInterval = 0.5f;
    private float _moveTimer;
    public Position MonsterPosition => _monsterPostion;
    private Random _random = new Random();
    private List<Monster> _others;
    public Monster(Scene scene, Player player, List<Monster> others) : base(scene)
    {
        _others = others;
        _player = player;
        Name = "Monster";
        _moveTimer = 0;
    }
    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;
        if (_moveTimer > k_MoveInterval)
        {
            Move(_player.PlayerPosition);
            _moveTimer = 0;
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        for(int i = 0; i < _monsterHeight; i++)
        {
            for(int j = 0; j < _monsterWidth; j++)
            {
                buffer.SetCell(_monsterPostion.X + j, _monsterPostion.Y + i, '█', ConsoleColor.Green);
            }
        }
    }
    public void Move(Position position)
    {
        int dx = 0;
        int dy = 0;
        if (position.X - _monsterPostion.X > 0) dx++;
        if (position.X - _monsterPostion.X < 0) dx--;
        if (position.Y - _monsterPostion.Y > 0) dy++;
        if (position.Y - _monsterPostion.Y < 0) dy--;
        Position newPosition = new Position(_monsterPostion.X + dx, _monsterPostion.Y + dy);
        if(_others.Any(m => m != this && m.IsOverlap(newPosition)))
        {
            return;
        }
        if (Map1.IsInBounds(newPosition.X, newPosition.Y))
        {
            _monsterPostion = newPosition;
        }
    }
    public void Spawn(Position position)
    {
        do
        {
            int spawn = _random.Next(4);
            if (spawn < 1)
            {
                _monsterPostion.X = _random.Next(Map1.Left, Map1.Right + 1);
                _monsterPostion.Y = Map1.Top + 2;
            }
            else if(spawn < 2)
            {
                _monsterPostion.Y = _random.Next(Map1.Top, Map1.Bottom + 1);
                _monsterPostion.X = Map1.Left + 2;
            }
            else if (spawn < 3)
            {
                _monsterPostion.X = _random.Next(Map1.Left, Map1.Right + 1);
                _monsterPostion.Y = Map1.Bottom - 2;
            }
            else
            {
                _monsterPostion.Y = _random.Next(Map1.Top, Map1.Bottom + 1);
                _monsterPostion.X = Map1.Right - 4;
            }
        }
        while(IsOverlap(position));
    }
    public bool IsOverlap(Position position)
    {
        return position.X >= _monsterPostion.X && position.X < _monsterPostion.X + _monsterWidth &&
           position.Y >= _monsterPostion.Y && position.Y < _monsterPostion.Y + _monsterHeight;
    }
    public bool IsHit(Position position)
    {
        return position.X >= _monsterPostion.X
            && position.X < _monsterPostion.X + _monsterWidth
            && position.Y >= _monsterPostion.Y
            && position.Y < _monsterPostion.Y + _monsterHeight;
    }
}

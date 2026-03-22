using Framework.Engine;
using System;
using System.Collections.Generic;

public class Boss : GameObject
{
    private Position _position;
    private Player _player;
    private int _width;
    private int _height;
    private int _maxHp;
    private int _hp;
    private float _moveTimer;
    private float _moveInterval;
    private Random _random = new Random();
    private bool _isBig; // true: 1차전 큰 슬라임, false: 2차전 분열 슬라임
    private List<Boss> _others; // 분열체끼리 겹침 방지

    public Position BossPosition => _position;
    public int Hp => _hp;
    public int MaxHp => _maxHp;
    public bool IsDead => _hp <= 0;
    public bool IsBig => _isBig;

    // 1차전 큰 보스 생성
    public Boss(Scene scene, Player player) : base(scene)
    {
        Name = "Boss";
        _player = player;
        _isBig = true;
        _width = 10;
        _height = 6;
        _maxHp = 30;
        _hp = _maxHp;
        _moveInterval = 0.6f;
        _moveTimer = 0;
        _position = new Position(Map.Left + (Map.Right - Map.Left) / 2 - _width / 2, Map.Top + 2);
        _others = new List<Boss>();
    }

    // 2차전 분열 슬라임 생성
    public Boss(Scene scene, Player player, Position spawnPos, List<Boss> others) : base(scene)
    {
        Name = "MiniBoss";
        _player = player;
        _isBig = false;
        _width = 5;
        _height = 3;
        _maxHp = 12;
        _hp = _maxHp;
        _moveInterval = 0.35f;
        _moveTimer = 0;
        _position = spawnPos;
        _others = others;
    }

    public override void Update(float deltaTime)
    {
        if (IsDead) return;

        _moveTimer += deltaTime;
        if (_moveTimer >= _moveInterval)
        {
            MoveTowardPlayer();
            _moveTimer = 0;
        }
    }

    private void MoveTowardPlayer()
    {
        int dx = 0;
        int dy = 0;
        Position target = _player.PlayerPosition;

        if (target.X > _position.X + _width / 2) dx = 1;
        else if (target.X < _position.X + _width / 2) dx = -1;

        if (target.Y > _position.Y + _height / 2) dy = 1;
        else if (target.Y < _position.Y + _height / 2) dy = -1;

        // 가끔 랜덤하게 한 방향만 이동 (움직임에 변화)
        if (_random.Next(3) == 0)
        {
            if (_random.Next(2) == 0) dx = 0;
            else dy = 0;
        }

        Position next = new Position(_position.X + dx, _position.Y + dy);

        // 맵 경계 체크
        if (next.X < Map.Left) next.X = Map.Left;
        if (next.X + _width - 1 > Map.Right) next.X = Map.Right - _width + 1;
        if (next.Y < Map.Top) next.Y = Map.Top;
        if (next.Y + _height - 1 > Map.Bottom) next.Y = Map.Bottom - _height + 1;

        // 분열체끼리 겹침 방지
        Rect nextRect = new Rect { X = next.X, Y = next.Y, Width = _width, Height = _height };
        foreach (var other in _others)
        {
            if (other != this && !other.IsDead && Overlap.IsOverlap(nextRect, other.BossRect()))
            {
                return;
            }
        }

        _position = next;
    }

    public override void Draw(ScreenBuffer buffer)
    {
        if (IsDead) return;

        ConsoleColor bodyColor = _isBig ? ConsoleColor.DarkGreen : ConsoleColor.Green;
        ConsoleColor eyeColor = ConsoleColor.White;

        if (_isBig)
        {
            DrawBigSlime(buffer, bodyColor, eyeColor);
        }
        else
        {
            DrawMiniSlime(buffer, bodyColor, eyeColor);
        }
    }

    private void DrawBigSlime(ScreenBuffer buffer, ConsoleColor body, ConsoleColor eye)
    {
        int x = _position.X;
        int y = _position.Y;

        // row 0: 상단 둥글게
        for (int i = 1; i < _width - 1; i++)
            buffer.SetCell(x + i, y, '█', body);

        // row 1: 꽉 채움
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y + 1, '█', body);

        // row 2: 눈
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y + 2, '█', body);
        buffer.SetCell(x + 2, y + 2, '●', eye);
        buffer.SetCell(x + 7, y + 2, '●', eye);

        // row 3: 몸통
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y + 3, '█', body);

        // row 4: 입
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y + 4, '█', body);
        buffer.SetCell(x + 4, y + 4, '▽', eye);
        buffer.SetCell(x + 5, y + 4, '▽', eye);

        // row 5: 하단 발
        buffer.SetCell(x + 1, y + 5, '█', body);
        buffer.SetCell(x + 2, y + 5, '█', body);
        buffer.SetCell(x + 4, y + 5, '█', body);
        buffer.SetCell(x + 5, y + 5, '█', body);
        buffer.SetCell(x + 7, y + 5, '█', body);
        buffer.SetCell(x + 8, y + 5, '█', body);
    }

    private void DrawMiniSlime(ScreenBuffer buffer, ConsoleColor body, ConsoleColor eye)
    {
        int x = _position.X;
        int y = _position.Y;

        // row 0: 머리
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y, '█', body);

        // row 1: 눈
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y + 1, '█', body);
        buffer.SetCell(x + 1, y + 1, '●', eye);
        buffer.SetCell(x + 3, y + 1, '●', eye);

        // row 2: 입
        for (int i = 0; i < _width; i++)
            buffer.SetCell(x + i, y + 2, '█', body);
        buffer.SetCell(x + 2, y + 2, '▽', eye);
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp < 0) _hp = 0;
    }

    public Rect BossRect()
    {
        return new Rect
        {
            X = _position.X,
            Y = _position.Y,
            Width = _width,
            Height = _height
        };
    }

    /// <summary>
    /// 큰 슬라임이 죽었을 때 분열 위치 2개 반환
    /// </summary>
    public (Position left, Position right) GetSplitPositions()
    {
        int centerX = _position.X + _width / 2;
        int centerY = _position.Y + _height / 2;

        Position left = new Position(centerX - 7, centerY - 1);
        Position right = new Position(centerX + 2, centerY - 1);

        // 경계 보정
        if (left.X < Map.Left) left.X = Map.Left;
        if (right.X + 5 > Map.Right) right.X = Map.Right - 5;
        if (left.Y < Map.Top) left.Y = Map.Top;
        if (right.Y < Map.Top) right.Y = Map.Top;
        if (left.Y + 3 > Map.Bottom) left.Y = Map.Bottom - 3;
        if (right.Y + 3 > Map.Bottom) right.Y = Map.Bottom - 3;

        return (left, right);
    }
}
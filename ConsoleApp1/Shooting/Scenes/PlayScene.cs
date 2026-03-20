using Framework.Engine;
using System;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

public class PlayScene : Scene
{
    private Map map1;
    private Player _player;
    private RifleItem rifleItem;
    private List<Monster> monsters = new List<Monster>();
    private List<Bullet> bullets = new List<Bullet>();
    private List<Item> items = new List<Item>();

    private bool isGameOver;
    private bool isGameClear;
    private float _gameTime;
    private int WeaponNumber;
    private readonly float _maxTime = 15f;
    private Random _random = new Random();
    public event GameAction PlayAgainRequested;
    public event GameAction GoShop;

    public PlayScene(Player player)
    {
        _player = player;
    }
    public override void Load()
    {
        
        _gameTime = 0f;
        WeaponNumber = 1;
        isGameClear = false;

        map1 = new Map(this);
        AddGameObject(map1);


        _player.SetScene(this);
        if (isGameOver)
        {
            _player.Reset();
            isGameOver = false;
        }


        Position startPosition = new Position(30, 15);
        _player.SetPosition(startPosition);
        AddGameObject(_player);

        
    }

    public override void Unload()
    {
        ClearGameObjects();
    }

    public override void Update(float deltaTime)
    {
        if (isGameOver)
        {
            if (Input.IsKeyDown(ConsoleKey.Enter))
            {
                PlayAgainRequested?.Invoke();
            }
            return;
        }
        _gameTime += deltaTime;
        if(_gameTime >= _maxTime)
        {
            isGameClear = true;
            if (Input.IsKeyDown(ConsoleKey.Enter))
            {
                GoShop?.Invoke();
            }
            return;
        }
        UpdateGameObjects(deltaTime);
        if (Input.IsKeyDown(ConsoleKey.Tab))
        {
            if (_player.HasRifle && _player.HasShotgun)
            {
                if(_player.Weapon is Rifle)
                {
                    _player.SetWeapon(new ShotGun());
                }
                else if(_player.Weapon is ShotGun)
                {
                    _player.SetWeapon(new Pistol());
                }
                else
                {
                    _player.SetWeapon(new Rifle());
                }
            }
            else if (_player.HasRifle)
            {
                if (_player.Weapon is Rifle)
                {
                    _player.SetWeapon(new Pistol());
                }
                else
                {
                    _player.SetWeapon(new Rifle());
                }
            }
            else if (_player.HasShotgun)
            {
                if (_player.Weapon is ShotGun)
                {
                    _player.SetWeapon(new Pistol());
                }
                else
                {
                    _player.SetWeapon(new ShotGun());
                }
            }
        }
        int activeItemCount = items.Count(i => i.IsActive);
        if (activeItemCount == 0)
        {
            var item = new SpeedUpItem(this);
            item.Spawn(_player.PlayerRect(_player.CurrentDirection));
            items.Add(item);
            AddGameObject(item);
        }

        int activeMonsterCount = monsters.Count(m => m.IsActive);
        if (_random.Next(100) < 5 && activeMonsterCount < 7)
        {
            var monster = new Monster(this, _player, monsters);
            monster.Spawn(_player.PlayerRect(_player.CurrentDirection));
            monsters.Add(monster);
            AddGameObject(monster);
        }
        foreach (var item in items)
        {
            if (Overlap.IsOverlap(_player.PlayerRect(_player.CurrentDirection), item.itemRect))
            {
                item.PickUpEffect(_player);
                item.IsActive = false;
                RemoveGameObject(item);
            }
        }
            foreach (var monster in monsters)
        {
            if(Overlap.IsOverlap(_player.PlayerRect(_player.CurrentDirection), monster.MonsterRect()))
            {
                isGameOver = true;
                return;
            }
            foreach (var bullet in bullets)
            {
                if (Overlap.IsOverlap(monster.MonsterRect(), bullet.BulletPosition))
                {
                    monster.IsActive = false;
                    RemoveGameObject(monster);
                    bullet.IsActive = false;
                    RemoveGameObject(bullet);
                    _player.GetGold(10);
                }
            }
        }
        monsters.RemoveAll(m => !m.IsActive);
        bullets.RemoveAll(m => !m.IsActive);
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(0, 0, $"Gold: {(_player.Gold)}G", ConsoleColor.Yellow);
        buffer.WriteTextCentered(0, $"현재무기 : {_player.Weapon}");
        buffer.WriteText(0, 1, TimeBar(), ConsoleColor.DarkGreen);
        DrawGameObjects(buffer);
        if (isGameOver)
        {
            buffer.WriteTextCentered(13, $"GAME OVER", ConsoleColor.Red);
            buffer.WriteTextCentered(15, "Press ENTER to Retry", ConsoleColor.White);
        }
        if (isGameClear)
        {
            buffer.WriteTextCentered(13, $"STAGE CLEAR", ConsoleColor.Green);
            buffer.WriteTextCentered(15, "Press ENTER to continue to the shop", ConsoleColor.White);
        }
    }
    public void CreateBullet(Position body, Direction dir)
    {
        var bullet = new Bullet(this, body, dir);
        bullets.Add(bullet);
        AddGameObject(bullet);
    }
    public string TimeBar()
    {
        int maxBar = 60;
        float remainTime = _maxTime - _gameTime;
        float ratio = remainTime / _maxTime;
        int filled = (int)(ratio * maxBar);
        StringBuilder sb = new StringBuilder();
        int bar = (int)_gameTime / 3;
        for (int i = 0; i < filled; i++)
        {
            sb.Append("█");
        }

        for (int i = filled; i < maxBar; i++)
        {
            sb.Append("░");
        }
        return sb.ToString();
    }
}

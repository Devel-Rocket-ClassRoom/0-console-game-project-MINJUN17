using Framework.Engine;
using System;
using System.Numerics;

public class PlayScene : Scene
{
    private Map1 map1;
    public Player player { get; private set; }
    private RifleItem rifleItem;
    private List<Monster> monsters = new List<Monster>();
    private List<Bullet> bullets = new List<Bullet>();

    private bool isGameOver;
    private Random _random = new Random();
    private int _Gold;
    public event GameAction PlayAgainRequested;
    public override void Load()
    {
        isGameOver = false;
        _Gold = 0;

        map1 = new Map1(this);
        AddGameObject(map1);

        Position startPosition = new Position(20, 10);
        player = new Player(this, startPosition);
        AddGameObject(player);

        rifleItem = new RifleItem(this);
        rifleItem.Spawn(player);
        AddGameObject(rifleItem);
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
        UpdateGameObjects(deltaTime);
        if(player.PlayerPosition == rifleItem.RiflePosition)
        {
            player.SetWeapon(new Rifle());
            RemoveGameObject(rifleItem);
        }
        int activeMonsterCount = monsters.Count(m => m.IsActive);
        if (_random.Next(100) < 3 && activeMonsterCount < 7)
        {
            var monster = new Monster(this, player, monsters);
            monster.Spawn(player.PlayerPosition);
            monsters.Add(monster);
            AddGameObject(monster);
        }
        foreach (var monster in monsters)
        {
            if(monster.MonsterPosition == player.PlayerPosition)
            {
                isGameOver = true;
                return;
            }
            foreach (var bullet in bullets)
            {
                if (monster.IsHit(bullet.BulletPosition)) 
                {
                    monster.IsActive = false;
                    RemoveGameObject(monster);
                    bullet.IsActive = false;
                    RemoveGameObject(bullet);
                    _Gold += 10;
                }
            }
        }
        monsters.RemoveAll(m => !m.IsActive);
        bullets.RemoveAll(m => !m.IsActive);
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(0, 0, $"Gold: {_Gold}G", ConsoleColor.Yellow);
        DrawGameObjects(buffer);
        if (isGameOver)
        {
            buffer.WriteTextCentered(13, $"GAME OVER", ConsoleColor.Red);
            buffer.WriteTextCentered(15, "Press ENTER to Retry", ConsoleColor.White);
        }
    }
    public void CreateBullet(Position body, Direction dir)
    {
        var bullet = new Bullet(this, body, dir);
        bullets.Add(bullet);
        AddGameObject(bullet);
    }
}

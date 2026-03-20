using Framework.Engine;
using System;
using System.Numerics;
using System.Text;

public class PlayScene : Scene
{
    private Map1 map1;
    public Player player { get; private set; }
    private RifleItem rifleItem;
    private List<Monster> monsters = new List<Monster>();
    private List<Bullet> bullets = new List<Bullet>();

    private bool isGameOver;
    private float _gameTime;
    private readonly float _maxTime = 90f;
    private Random _random = new Random();
    public event GameAction PlayAgainRequested;
    public event GameAction GoToNextStage;
    public override void Load()
    {
        _gameTime = 0f;
        isGameOver = false;

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
        _gameTime += deltaTime;
        if( _gameTime >= _maxTime)
        {
            GameClear();
        }
        UpdateGameObjects(deltaTime);
        if(Overlap.IsOverlap(player.PlayerRect(player.CurrentDirection), rifleItem.RiflePosition))
        {
            player.SetWeapon(new Rifle());
            RemoveGameObject(rifleItem);
        }
        int activeMonsterCount = monsters.Count(m => m.IsActive);
        if (_random.Next(100) < 3 && activeMonsterCount < 7)
        {
            var monster = new Monster(this, player, monsters);
            monster.Spawn(player.PlayerRect(player.CurrentDirection));
            monsters.Add(monster);
            AddGameObject(monster);
        }
        foreach (var monster in monsters)
        {
            if(Overlap.IsOverlap(player.PlayerRect(player.CurrentDirection), monster.MonsterRect()))
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
                    player.GetGold(10);
                }
            }
        }
        monsters.RemoveAll(m => !m.IsActive);
        bullets.RemoveAll(m => !m.IsActive);
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(0, 0, $"Gold: {Player.Gold}G", ConsoleColor.Yellow);
        buffer.WriteTextCentered(0, $"현재무기 : {player.Weapon}");
        buffer.WriteText(0, 1, TimeBar(), ConsoleColor.DarkGreen);
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
    public void GameClear()
    {

    }
}

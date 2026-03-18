using System;
using Framework.Engine;

public class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;
    private Map1 map1;
    private Player player;
    private const float k_shootInterval = 0.2f;
    private float _shootTimer;
    public override void Load()
    {
        map1 = new Map1(this);
        AddGameObject(map1);

        player = new Player(this, 20, 10);
        AddGameObject(player);

        _shootTimer = 0;
    }

    public override void Unload()
    {
        ClearGameObjects();
    }

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);
        _shootTimer += deltaTime;
        if (_shootTimer > k_shootInterval)
        {
            BulletFire();
            _shootTimer = 0;
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);
    }
    public void BulletFire()
    {
        if (Input.IsKey(ConsoleKey.Spacebar))
        {
            int x = player.PlayerBody.X;
            int y = player.PlayerBody.Y;

            int dx = 0;
            int dy = 0;

            switch (player.Direction)
            {
                case Direction.Up:
                    dx = 0; dy = -1;
                    x += 1; y -= 1;
                    break;

                case Direction.Down:
                    dx = 0; dy = 1;
                    x -= 1; y += 1;
                    break;

                case Direction.Left:
                    dx = -1; dy = 0;
                    x -= 2; y -= 1;
                    break;

                case Direction.Right:
                    dx = 1; dy = 0;
                    x += 2; y += 1;
                    break;
                case Direction.UpLeft:
                    dx = -1; dy = -1;
                    x -= 2; y -= 1;
                    break;
                case Direction.UpRight:
                    dx = 1; dy = -1;
                    x += 1; y -= 1;
                    break;
                case Direction.DownRight:
                    dx = 1; dy = 1;
                    x += 2; y += 1;
                    break;
                case Direction.DownLeft:
                    dx = -1; dy = 1;
                    x -= 1; y += 1;
                    break;
            }

            var bullet = new Bullet(this, x, y, dx, dy);
            AddGameObject(bullet);
        }
    }
}

using System;
using Framework.Engine;

public class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;
    private Map1 map1;
    private Bullet bullet;
    private Player player;
    public override void Load()
    {
        map1 = new Map1(this);
        AddGameObject(map1);

        player = new Player(this, 20, 10);
        AddGameObject(player);

    }

    public override void Unload()
    {
        ClearGameObjects();
    }

    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);
        BulletFire();
    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);
    }
    public void BulletFire()
    {
        if (Input.IsKeyDown(ConsoleKey.Spacebar))
        {
            int x = player.PlayerBody.X;
            int y = player.PlayerBody.Y;

            switch (player.Direction) // 캐릭터 방향, 총구 위치에 맞게 조정
            {
                case Direction.Down:
                    x = x - 1;
                    y = y + 1;
                    break;

                case Direction.Up:
                    x = x + 2;
                    y = y - 1;
                    break;

                case Direction.Left:
                    x = x - 2;
                    y = y - 1;
                    break;

                case Direction.Right:
                    x = x + 2;
                    y = y + 1;
                    break;
            }

            Bullet bullet = new Bullet(this, x, y, player.Direction);
            AddGameObject(bullet);
        }
    }
}

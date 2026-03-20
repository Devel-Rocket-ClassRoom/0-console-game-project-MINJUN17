using System;
using Framework.Engine;

class ShootingGame : GameApp
{
    private readonly SceneManager<Scene> _scenes = new SceneManager<Scene>();
    public ShootingGame() : base(60, 30)
    {

    }
    public ShootingGame(int width, int height) : base(width, height)
    {

    }
    protected override void Initialize()
    {
        ChaneToShop();
    }
    protected override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Escape))
        {
            Quit();
            return;
        }
        _scenes.CurrentScene?.Update(deltaTime);
    }
    protected override void Draw()
    {
        _scenes.CurrentScene?.Draw(Buffer);
    }

    private void ChaneToShop()
    {
        var shop = new ShopScene();
        _scenes.ChangeScene(shop);
    }
    private void ChangeToTitle()
    {
        var title = new TitleScene();
        title.StartRequested += ChangeToPlay;
        _scenes.ChangeScene(title);
    }

    private void ChangeToPlay()
    {
        var play = new PlayScene();
        play.PlayAgainRequested += ChangeToTitle;
        play.GameClear
        _scenes.ChangeScene(play);
    }
}

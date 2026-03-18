using System;
using Framework.Engine;

public class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;
    private Map1 map1;
    public override void Load()
    {
        map1 = new Map1(this);
        AddGameObject(map1);
    }

    public override void Unload()
    {
        ClearGameObjects();
    }

    public override void Update(float deltaTime)
    {
    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);
    }
}

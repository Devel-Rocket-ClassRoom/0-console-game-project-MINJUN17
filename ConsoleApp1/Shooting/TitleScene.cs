using System;
using Framework.Engine;

public class TitleScene : Scene
{
    public event GameAction StartRequested;
    public override void Load()
    {
    }

    public override void Unload()
    {
    }

    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            StartRequested?.Invoke();
        }
    }
    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteTextCentered(6, "G A M E N A M E", ConsoleColor.Yellow);
        buffer.WriteTextCentered(10, "Arrow Keys: Move");
        buffer.WriteTextCentered(12, "ESC: Quit");
        buffer.WriteTextCentered(15, "Press ENTER to Start", ConsoleColor.Green);
    }
}

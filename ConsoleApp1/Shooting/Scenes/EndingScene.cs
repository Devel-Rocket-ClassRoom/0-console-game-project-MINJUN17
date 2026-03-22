using Framework.Engine;
using System;

public class EndingScene : Scene
{
    private Player _player;
    private float _timer;

    public event GameAction BackToTitle;

    public EndingScene(Player player)
    {
        _player = player;
    }

    public override void Load()
    {
        _timer = 0;
    }

    public override void Unload()
    {
    }

    public override void Update(float deltaTime)
    {
        _timer += deltaTime;

        // 3초 뒤부터 Enter 입력 받음
        if (_timer > 3.0f && Input.IsKeyDown(ConsoleKey.Enter))
        {
            BackToTitle?.Invoke();
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        int cy = 5;

        buffer.WriteTextCentered(cy, "========================================", ConsoleColor.DarkYellow);
        buffer.WriteTextCentered(cy + 2, "  ★  C O N G R A T U L A T I O N S  ★  ", ConsoleColor.Yellow);
        buffer.WriteTextCentered(cy + 4, "========================================", ConsoleColor.DarkYellow);

        buffer.WriteTextCentered(cy + 7, "You defeated the King Slime and saved the world!", ConsoleColor.White);

        buffer.WriteTextCentered(cy + 10, $"Final Gold: {_player.Gold}G", ConsoleColor.Yellow);

        // 슬라임 묘비
        buffer.WriteTextCentered(cy + 13, "    ___", ConsoleColor.DarkGreen);
        buffer.WriteTextCentered(cy + 14, "   | R |", ConsoleColor.DarkGreen);
        buffer.WriteTextCentered(cy + 15, "   | I |", ConsoleColor.DarkGreen);
        buffer.WriteTextCentered(cy + 16, "   | P |", ConsoleColor.DarkGreen);
        buffer.WriteTextCentered(cy + 17, "  _|___|_", ConsoleColor.DarkGray);
        buffer.WriteTextCentered(cy + 18, " /       \\", ConsoleColor.DarkGray);

        if (_timer > 3.0f)
        {
            // 깜빡임
            if ((int)(_timer * 3) % 2 == 0)
            {
                buffer.WriteTextCentered(cy + 21, "Press ENTER to return to title", ConsoleColor.Gray);
            }
        }
        else
        {
            buffer.WriteTextCentered(cy + 21, "...", ConsoleColor.DarkGray);
        }
    }
}
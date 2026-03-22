using System;
using Framework.Engine;

public class TitleScene : Scene
{
    public event GameAction StartRequested;
    private float _timer;

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

        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            StartRequested?.Invoke();
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        // 타이틀 로고
        buffer.WriteTextCentered(2, "╔═╗╦  ╦╔╦╗╔═╗  ╦╔═╦╦  ╦  ╔═╗╦═╗", ConsoleColor.Green);
        buffer.WriteTextCentered(3, "╚═╗║  ║║║║║╣   ╠╩╗║║  ║  ║╣ ╠╦╝", ConsoleColor.Green);
        buffer.WriteTextCentered(4, "╚═╝╩═╝╩╩ ╩╚═╝  ╩ ╩╩╩═╝╩═╝╚═╝╩╚═", ConsoleColor.DarkGreen);

        // 슬라임 (오른쪽)
        buffer.WriteText(33, 8, "  ████████", ConsoleColor.Green);
        buffer.WriteText(33, 9, "████████████", ConsoleColor.Green);
        buffer.WriteText(33, 10, "██ ", ConsoleColor.Green);
        buffer.SetCell(36, 10, '*', ConsoleColor.White);
        buffer.WriteText(37, 10, " ██ ", ConsoleColor.Green);
        buffer.SetCell(41, 10, '*', ConsoleColor.White);
        buffer.WriteText(42, 10, " ██", ConsoleColor.Green);
        buffer.WriteText(33, 11, "████████████", ConsoleColor.Green);
        buffer.WriteText(33, 12, "████████████", ConsoleColor.Green);
        buffer.WriteText(33, 13, "  ████████", ConsoleColor.DarkGreen);

        // 플레이어 (왼쪽) + 총
        buffer.SetCell(16, 10, ' ', ConsoleColor.Yellow);
        buffer.SetCell(17, 10, ' ', ConsoleColor.Yellow);

        buffer.SetCell(15, 9, ' ', ConsoleColor.Yellow);
        buffer.SetCell(16, 9, ' ', ConsoleColor.Yellow);
        buffer.SetCell(17, 9, ' ', ConsoleColor.Yellow);

        buffer.SetCell(15, 10, ' ', ConsoleColor.Yellow);
        buffer.SetCell(16, 10, ' ', ConsoleColor.Yellow);
        buffer.SetCell(17, 10, ' ', ConsoleColor.Yellow);

        buffer.SetCell(15, 8, '*', ConsoleColor.Yellow);
        buffer.SetCell(17, 8, '*', ConsoleColor.Yellow);
        buffer.SetCell(16, 8, 'v', ConsoleColor.DarkYellow);

        buffer.SetCell(15, 9, '#', ConsoleColor.Yellow);
        buffer.SetCell(16, 9, '#', ConsoleColor.Yellow);
        buffer.SetCell(17, 9, '#', ConsoleColor.Yellow);

        buffer.SetCell(15, 10, '|', ConsoleColor.DarkYellow);
        buffer.SetCell(17, 10, '|', ConsoleColor.DarkYellow);

        // 총
        buffer.SetCell(18, 9, '+', ConsoleColor.Gray);
        buffer.SetCell(19, 9, '=', ConsoleColor.Gray);
        buffer.SetCell(20, 9, '=', ConsoleColor.DarkGray);
        buffer.SetCell(21, 9, '-', ConsoleColor.DarkGray);

        // 총알들
        buffer.SetCell(24, 9, '.', ConsoleColor.White);
        buffer.SetCell(27, 9, '.', ConsoleColor.White);
        buffer.SetCell(30, 9, '.', ConsoleColor.White);

        // VS
        buffer.WriteText(23, 12, "V S", ConsoleColor.Red);

        // 구분선
        buffer.WriteTextCentered(15, "------------------------------------", ConsoleColor.DarkGray);

        // 조작법 (한글 없이 영어로 통일)
        buffer.WriteText(14, 17, "ARROW", ConsoleColor.Cyan);
        buffer.WriteText(24, 17, "Move", ConsoleColor.Gray);

        buffer.WriteText(14, 18, "SPACE", ConsoleColor.Cyan);
        buffer.WriteText(24, 18, "Fire", ConsoleColor.Gray);

        buffer.WriteText(14, 19, "TAB", ConsoleColor.Cyan);
        buffer.WriteText(24, 19, "Switch weapon", ConsoleColor.Gray);

        buffer.WriteText(14, 20, "ESC", ConsoleColor.Cyan);
        buffer.WriteText(24, 20, "Quit", ConsoleColor.Gray);

        buffer.WriteTextCentered(22, "------------------------------------", ConsoleColor.DarkGray);

        // 깜빡이는 시작 안내
        if ((int)(_timer * 2.5f) % 2 == 0)
        {
            buffer.WriteTextCentered(25, ">> Press ENTER to Start <<", ConsoleColor.Green);
        }

        buffer.WriteTextCentered(28, "- SLIME KILLER 2026 -", ConsoleColor.DarkGray);
    }
}
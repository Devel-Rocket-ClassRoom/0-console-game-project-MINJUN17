using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class SpeedUpItem : Item
{
    private float _speedMultiply = 3;
    public SpeedUpItem(Scene scene) : base(scene)
    {
        _buffTime = 5f;
    }
    public override void Draw(ScreenBuffer buffer)
    {
        int x = itemRect.X;
        int y = itemRect.Y;
        int w = itemRect.Width;
        int h = itemRect.Height;

        // 테두리 그리기
        for (int i = 0; i < w; i++)
        {
            buffer.SetCell(x + i, y, '─');             // top
            buffer.SetCell(x + i, y + h - 1, '─');     // bottom
        }

        for (int i = 0; i < h; i++)
        {
            buffer.SetCell(x, y + i, '│');             // left
            buffer.SetCell(x + w - 1, y + i, '│');     // right
        }

        // 모서리
        buffer.SetCell(x, y, '┌');
        buffer.SetCell(x + w - 1, y, '┐');
        buffer.SetCell(x, y + h - 1, '└');
        buffer.SetCell(x + w - 1, y + h - 1, '┘');

        // 내부 아이콘 (>>)
        buffer.SetCell(x + 2, y + 1, '>');
        buffer.SetCell(x + 3, y + 1, '>');
    }

    public override void PickUpEffect(Player player)
    {
        player.MoveFast(_speedMultiply, _buffTime);
        Scene.RemoveGameObject(this);
    }
}

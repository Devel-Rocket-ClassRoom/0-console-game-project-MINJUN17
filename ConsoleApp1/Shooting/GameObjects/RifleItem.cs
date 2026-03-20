using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

public class RifleItem : GameObject
{
    private readonly Random _random = new Random();
    private Position _riflePosition;
    public Position RiflePosition => _riflePosition;
    public RifleItem(Scene scene) : base(scene)
    {
    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(_riflePosition.X, _riflePosition.Y, '*', ConsoleColor.Red);
    }

    public override void Update(float deltaTime)
    {
    }
    public void Spawn(Player player)
    {
        do
        {
            _riflePosition.X = _random.Next(Map.Left, Map.Right + 1);
            _riflePosition.Y = _random.Next(Map.Top, Map.Bottom + 1);
        }
        while (player.PlayerPosition == _riflePosition);
    }
}

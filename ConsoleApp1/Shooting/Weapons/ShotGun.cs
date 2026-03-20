using System;
using System.Collections.Generic;
using System.Text;


public class ShotGun : Weapon
{
    private Position position1;
    private Position position2;
    public ShotGun()
    {
        _cooldown = 1.0f;
    }
    public override void Fire(PlayScene scene, Position pos, Direction dir)
    {
        if(dir == Direction.Up || dir == Direction.Down)
        {
            scene.CreateBullet(pos, dir);
            position1 = new Position() { X = pos.X - 2, Y = pos.Y};
            scene.CreateBullet(position1, dir);
            position2 = new Position() { X = pos.X + 2, Y = pos.Y };
            scene.CreateBullet(position2, dir);
        }
        else if(dir == Direction.Left || dir == Direction.Right)
        {
            scene.CreateBullet(pos, dir);
            position1 = new Position() { X = pos.X, Y = pos.Y + 1 };
            scene.CreateBullet(position1, dir);
            position2 = new Position() { X = pos.X, Y = pos.Y - 1 };
            scene.CreateBullet(position2, dir);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;


public class ShotGun : Weapon
{
    public ShotGun()
    {
        _cooldown = 1.0f;
    }
    public override void Fire(PlayScene scene, Position pos, Direction dir)
    {
        if(dir == Direction.Up)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir + 2);
            scene.CreateBullet(pos, dir + 3);
        }
        if(dir == Direction.Down)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir + 3);
            scene.CreateBullet(pos, dir + 4);
        }
        if (dir == Direction.Left)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir + 4);
            scene.CreateBullet(pos, dir + 6);
        }
        if (dir == Direction.Right)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir + 4);
            scene.CreateBullet(pos, dir + 6);
        }
        if (dir == Direction.DownLeft)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir - 6);
            scene.CreateBullet(pos, dir - 3);
        }
        if (dir == Direction.DownRight)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir - 4);
            scene.CreateBullet(pos, dir - 6);
        }
        if (dir == Direction.UpRight)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir - 3);
            scene.CreateBullet(pos, dir - 4);
        }
        if (dir == Direction.UpLeft)
        {
            scene.CreateBullet(pos, dir);
            scene.CreateBullet(pos, dir - 2);
            scene.CreateBullet(pos, dir - 4);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

public class Rifle : Weapon
{
    public Rifle()
    {
        _cooldown = 0.3f;
    }
    public override void Fire(IBulletCreator creator, Position pos, Direction dir)
    {
        creator.CreateBullet(pos, dir);
    }
}
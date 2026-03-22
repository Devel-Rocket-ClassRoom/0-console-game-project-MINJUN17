using System;
using System.Collections.Generic;
using System.Text;

public class Pistol : Weapon
{
    public Pistol()
    {
        _cooldown = 2.0f;
    }
    public override void Fire(IBulletCreator creator, Position pos, Direction dir)
    {
        if (creator == null)
        {
            throw new Exception("scene null임");
        }
        creator.CreateBullet(pos, dir);
    }
}
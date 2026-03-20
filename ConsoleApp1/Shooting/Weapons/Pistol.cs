using System;
using System.Collections.Generic;
using System.Text;

public class Pistol : Weapon
{
    public Pistol()
    {
        _cooldown = 2.0f;
    }
    public override void Fire(PlayScene scene, Position pos, Direction dir)
    {
        if (scene == null)
        {
            throw new Exception("scene null임");
        }
        scene.CreateBullet(pos, dir);
    }
}

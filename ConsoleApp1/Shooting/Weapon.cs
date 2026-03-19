using System;
using System.Collections.Generic;
using System.Text;

public abstract class Weapon
{
    protected float _cooldown;
    protected float _timer;

    public void Update(float dt)
    {
        _timer += dt;
    }

    public bool CanFire()
    {
        return _timer >= _cooldown;
    }

    public void ResetTimer()
    {
        _timer = 0;
    }

    public abstract void Fire(PlayScene scene, Position pos, Direction dir);
}

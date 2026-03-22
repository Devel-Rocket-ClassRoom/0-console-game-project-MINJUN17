using Framework.Engine;
using System;
using System.Linq;


public abstract class Item : GameObject
{
    public Rect itemRect { get; set; }
    private Random _random = new Random();
    private List<Item> _others;
    protected float _buffTime;

    public Item(Scene scene, List<Item> items) : base(scene)
    {
        _others = items;
    }

    public Item(Scene scene) : base(scene)
    {
    }

    public override void Update(float deltaTime)
    {
    }

    public override void Draw(ScreenBuffer buffer)
    {

    }
    public void Spawn(Rect rect)
    {
        // 중앙 영역에만 스폰 (빨간 스폰존 안쪽으로)
        int innerLeft = Map.Left + 8;
        int innerRight = Map.Right - 8;
        int innerTop = Map.Top + 4;
        int innerBottom = Map.Bottom - 4;

        do
        {
            itemRect = new Rect()
            {
                Width = 6,
                Height = 3,
                X = _random.Next(innerLeft, innerRight - 5),
                Y = _random.Next(innerTop, innerBottom - 2)
            };
        }
        while (
    Overlap.IsOverlap(itemRect, rect) ||
    (_others != null && _others.Any(m => m != this && Overlap.IsOverlap(itemRect, m.itemRect)))
);
    }
    public abstract void PickUpEffect(Player player);
}
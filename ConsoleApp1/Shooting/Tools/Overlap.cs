using System;
using System.Collections.Generic;
using System.Text;

public static class Overlap
{
    public static bool IsOverlap(Rect a, Rect b)
    {
        return a.X < b.X + b.Width &&
               a.X + a.Width > b.X &&
               a.Y < b.Y + b.Height &&
               a.Y + a.Height > b.Y;
    }
    public static bool IsOverlap(Rect a, Position b)
    {
        return b.X >= a.X &&
               b.X < a.X + a.Width &&
               b.Y >= a.Y &&
               b.Y < a.Y + a.Height;
    }
}


//a struct for positions on the grid
//it's nice because while Vector2 seems good, its variables are floats, and I want precise coordination, where ints are better
//Also help code stay organizes
public struct GridPos
{
    public int x;
    public int y;

    public GridPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static GridPos operator +(GridPos lhs, GridPos rhs)
    {
        return new GridPos(lhs.x + rhs.x, lhs.y + rhs.y);
    }

    public static GridPos operator -(GridPos lhs, GridPos rhs)
    {
        return new GridPos(lhs.x - rhs.x, lhs.y - rhs.y);
    }

    public static GridPos operator -(GridPos v)
    {
        return new GridPos(-v.x, -v.y);
    }
}

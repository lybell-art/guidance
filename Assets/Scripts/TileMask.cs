public static class TileMask
{
    // constant
    public static readonly int platform = 1<<0;
    public static readonly int trap = 1<<1;
    public static readonly int banned = 1<<2;
    public static readonly int cantGo = platform;
    public static readonly int cantPlace = platform | trap | banned;

    public static bool IsPlatform(int mask)
    {
        return (mask & platform) != 0;
    }
    public static bool IsTrap(int mask)
    {
        return (mask & trap) != 0;
    }
    public static bool IsEmpty(int mask)
    {
        return (mask & cantGo) == 0;
    }
    public static bool IsPlaceable(int mask)
    {
        return (mask & cantPlace) == 0;
    }
}

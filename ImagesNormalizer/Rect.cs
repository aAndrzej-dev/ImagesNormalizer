namespace ImagesNormalizer;
internal struct Rect : IEquatable<Rect>
{
    public readonly bool IsZero => left == 0 && top == 0 && right == 0 && bottom == 0;

    public int left;
    public int top;
    public int right;
    public int bottom;
    public Rect(int left, int top, int right, int bottom)
    {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
    }
    public Rect(int all)
    {
        left = all;
        top = all;
        right = all;
        bottom = all;
    }
    public override string ToString()
    {
        return $"Left: {left}, Top: {top}, Right: {right}, Bottom: {bottom}";
    }

    public override readonly bool Equals(object? obj) => obj is Rect rect && Equals(rect);
    public readonly bool Equals(Rect other) => left == other.left && top == other.top && right == other.right && bottom == other.bottom;
    public override readonly int GetHashCode() => HashCode.Combine(left, top, right, bottom);

    public static bool operator ==(Rect left, Rect right) => left.Equals(right);
    public static bool operator !=(Rect left, Rect right) => !(left == right);
}
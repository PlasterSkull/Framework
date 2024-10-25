namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public readonly partial struct CodeExampleKey : IComparable<CodeExampleKey>, IEquatable<CodeExampleKey>
{
    public string Value
    {
        get;
    }

    public CodeExampleKey(string value)
    {
        Value = value;
    }

    public static readonly CodeExampleKey Empty = new CodeExampleKey(string.Empty);
    public bool Equals(CodeExampleKey other) => this.Value.Equals(other.Value);
    public int CompareTo(CodeExampleKey other) => Value.CompareTo(other.Value);
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        return obj is CodeExampleKey other && Equals(other);
    }

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(CodeExampleKey a, CodeExampleKey b) => a.CompareTo(b) == 0;
    public static bool operator !=(CodeExampleKey a, CodeExampleKey b) => !(a == b);
}
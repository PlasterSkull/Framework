namespace PlasterSkull.Framework.Blazor;

public readonly struct TagId
{
    public string Tag { get; private init; }
    public Guid Value { get; private init; }

    private TagId(string tag)
    {
        Tag = tag;
        Value = Guid.NewGuid();
    }

    public override string ToString() => this;

    public static implicit operator string(TagId id) =>
        $"{id.Tag}:{id.Value}";

    public static implicit operator Guid(TagId id) =>
        id.Value;

    public static TagId New(string tag) => new(tag);  
    public static TagId NewWithTagFormatting(string tag) => new(tag.ToKebabCase());
}

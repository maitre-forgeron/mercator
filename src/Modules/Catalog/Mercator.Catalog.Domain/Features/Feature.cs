namespace Mercator.Catalog.Domain.Features;

/// <summary>
/// Just for example. Will be deleted later.
/// </summary>
public sealed class Feature
{
    public Guid Id { get; }
    public string Name { get; private set; }

    public Feature(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

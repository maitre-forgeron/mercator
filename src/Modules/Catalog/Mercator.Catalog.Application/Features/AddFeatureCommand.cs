using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.Catalog.Domain.Features;

namespace Mercator.Catalog.Application.Features;

public sealed record AddFeatureCommand(string Name) : ICommand<Guid>;

public sealed class AddFeatureHandler() : ICommandHandler<AddFeatureCommand, Guid>
{
    public async Task<Guid> Handle(AddFeatureCommand command, CancellationToken ct = default)
    {
        var id = Guid.NewGuid();
        var feature = new Feature(id, command.Name.Trim());

        //save into db
        return id;
    }
}
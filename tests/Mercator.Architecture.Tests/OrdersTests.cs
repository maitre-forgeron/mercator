using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.BuildingBlocks.Application.Abstractions.Queries;
using NetArchTest.Rules;
using System.Reflection;

namespace Mercator.Architecture.Tests;

public class OrdersTests
{
    [Fact]
    public void Domain_Should_Not_Have_Dependency()
    {
        var domainAssembly = Assembly.Load("Mercator.Orders.Domain");

        var result = Types
            .InAssembly(domainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("Mercator.Orders.Application", "Mercator.Orders.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var applicationAssembly = Assembly.Load("Mercator.Orders.Application");

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn("Mercator.Orders.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Must_Depend_On_Domain()
    {
        var infrastructureAssembly = Assembly.Load("Mercator.Orders.Infrastructure");

        var result = Types
            .InAssembly(infrastructureAssembly)
            .Should()
            .NotHaveDependencyOn("Mercator.Orders.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Command_Handlers_Must_End_With_CommandHandler()
    {
        var applicationAssembly = Assembly.Load("Mercator.Orders.Application");

        var result = Types
            .InAssembly(applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Query_Handlers_Must_End_With_QueryHandler()
    {
        var applicationAssembly = Assembly.Load("Mercator.Orders.Application");

        var result = Types
            .InAssembly(applicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}

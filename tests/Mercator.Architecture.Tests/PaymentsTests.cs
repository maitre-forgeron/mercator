using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.BuildingBlocks.Application.Abstractions.Queries;
using NetArchTest.Rules;
using System.Reflection;

namespace Mercator.Architecture.Tests;

public class PaymentsTests
{
    [Fact]
    public void Domain_Should_Not_Have_Dependency()
    {
        var domainAssembly = Assembly.Load("Mercator.Payments.Domain");

        var result = Types
            .InAssembly(domainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("Mercator.Payments.Application", "Mercator.Payments.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var applicationAssembly = Assembly.Load("Mercator.Payments.Application");

        var result = Types
            .InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn("Mercator.Payments.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Infrastructure_Must_Depend_On_Domain()
    {
        var infrastructureAssembly = Assembly.Load("Mercator.Payments.Infrastructure");

        var result = Types
            .InAssembly(infrastructureAssembly)
            .Should()
            .NotHaveDependencyOn("Mercator.Payments.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Command_Handlers_Must_End_With_CommandHandler()
    {
        var applicationAssembly = Assembly.Load("Mercator.Payments.Application");

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
        var applicationAssembly = Assembly.Load("Mercator.Payments.Application");

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

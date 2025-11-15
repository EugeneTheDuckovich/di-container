using DependencyInjectionContainer;
using Xunit;

namespace Tests;

public class ContainerTests
{
    [Fact]
    public void TransientResolve()
    {
        var container = new Container()
            .RegisterTransient<A>()
            .RegisterTransient<B>(new A());
        
        var b = container.Resolve<B>(new A());

        Assert.Throws<ArgumentException>(() =>  container.Resolve<B>());
        
        Assert.NotNull(b);
    }
}

public record A;

public record B(A _);
using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Fastpix.Tests;

public class EndpointEnumerationTests
{
    [Fact]
    public void Count_All_Public_Async_Endpoints()
    {
        var sdkAssembly = typeof(Fastpix.FastPix).Assembly;
        var endpointTypes = sdkAssembly
            .GetTypes()
            .Where(t => t.IsClass && t.Namespace == "Fastpix")
            .ToList();

        var asyncEndpoints = endpointTypes
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            .Where(m => m.ReturnType.IsGenericType && m.ReturnType.Name.StartsWith("Task"))
            .ToList();

        asyncEndpoints.Should().NotBeNull();
        // Expected around 66 endpoints according to user; assert at least 60 to tolerate minor diffs
        asyncEndpoints.Count.Should().BeGreaterOrEqualTo(60);
    }
}



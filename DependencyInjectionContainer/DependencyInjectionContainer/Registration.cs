using System;

namespace DependencyInjectionContainer;

public record struct Registration(Type InjectionType, Type ValueType,
    ReusePolicy ReusePolicy, object? Key = null);
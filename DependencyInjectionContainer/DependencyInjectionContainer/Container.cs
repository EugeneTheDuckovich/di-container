using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyInjectionContainer;

public class Container
{
    private readonly Dictionary<(Type injectionType, object? key), Registration> _registrations = [];

    public Container RegisterTransient<TInject, TValue>(object? key = null)
        where TValue : TInject
    {
        var registration = new Registration(typeof(TInject), typeof(TValue), ReusePolicy.Transient, key);
        
        _registrations.Add((registration.InjectionType, key), registration);

        return this;
    }

    public Container RegisterTransient<T>(object? key = null) => RegisterTransient<T, T>(key);

    public T Resolve<T>(object? key = null) => (T)Resolve(typeof(T), key);
    
    private object Resolve(Type type, object? key = null)
    {
        if(!_registrations.TryGetValue((type, key), out  var registration))
            throw new ArgumentException($"type {type} is not registered!");
        
        var constructor = registration.ValueType.GetConstructors()[0];

        var parameters = constructor.GetParameters()
            .Select(p => Resolve(p.ParameterType))
            .ToArray();

        return registration.ReusePolicy switch
        {
            ReusePolicy.Transient => constructor.Invoke(parameters),
            _ => throw new NotImplementedException("only transient reuse works for now!"),
        };
    }
}
public class World
{
    private readonly List<object> _components = [];
    public delegate void SystemDelegate();
    private readonly List<SystemDelegate> _systems = new();
    public void Spawn<T>(T component) where T : class
    {
        _components.Add(component);
    }

    public void RegisterSystem<T1>(Action<T1> system)
    {
        var parameters = system.Method.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var paramType = parameters[i].ParameterType;

            if (paramType.IsGenericType && paramType.GetGenericTypeDefinition() == typeof(Query<>))
            {
                var componentType = paramType.GetGenericArguments()[0];
                var genericQuery = typeof(Query<>).MakeGenericType(componentType);
                var queryInstance = Activator.CreateInstance(genericQuery, [_components]);
                args[i] = queryInstance ?? throw new InvalidOperationException($"Failed to create instance of {paramType}");
            }
            else
            {
                throw new InvalidOperationException($"Unsupported parameter type: {paramType}");
            }
        }

        // Create a SystemDelegate that invokes the system with the resolved arguments
        _systems.Add(() => system.DynamicInvoke(args));
    }


    public void Update()
    {
        foreach (var system in _systems)
        {
            system.Invoke();
        }
    }
}

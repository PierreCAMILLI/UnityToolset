namespace Toolset
{

    public interface ILocator
    {
        public bool IsRegistered { get; }
        public void Dispose();
    }

    public interface ILocator<Class> : ILocator
    {
        public void Register(Class implementation);
    }

    /// <summary>
    /// Provides access to a service by finding an appropriate provider
    /// </summary>
    /// <typeparam name="Class">Base class that implements the service</typeparam>
    /// <typeparam name="Null">Null implementation of the service</typeparam>
    public class Locator<Class, Null> : ILocator<Class>
        where Class : class
        where Null : Class, new()
    {
        protected Class _implementation;
        protected static Null _nullImplementation;

        /// <summary>
        /// Indicates if our locator has an implementation registered
        /// </summary>
        public bool IsRegistered => _implementation != null;

        /// <summary>
        /// Provides the located service
        /// </summary>
        public virtual Class Provide => _implementation == null ? _nullImplementation : _implementation;

        public Locator()
        {
            _implementation = null;
            if (_nullImplementation == null)
            {
                _nullImplementation = new Null();
            }
        }

        /// <summary>
        /// Register a new implementation for the service
        /// </summary>
        /// <param name="implementation"></param>
        public virtual void Register(Class implementation)
        {
            _implementation = implementation;
        }

        /// <summary>
        /// Dispose the current implementation of the service
        /// </summary>
        public virtual void Dispose()
        {
            _implementation = null;
        }
    }
}

/*
public interface ISomeService
{
    public void Foo();
}

public class NullService : ISomeService
{
    public void Foo()
    {
        Debug.Log("Nothing");
    }
}

public class MyService : ISomeService
{
    public void Foo()
    {
        Debug.Log("Do something");
    }
}

public class SomeManager
{
    private Locator<ISomeService, NullService> _service;
    public Locator<ISomeService, NullService> Service => _service;

    public SomeManager()
    {
        _service = new Locator<ISomeService, NullService>();
        _service.Provide.Foo();
        // Print: Nothing

        _service.Register(new MyService());
        _service.Provide.Foo();
        // Print: Do something
    }
}
*/

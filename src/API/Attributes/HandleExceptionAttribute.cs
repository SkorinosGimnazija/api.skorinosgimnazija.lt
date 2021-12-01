namespace SkorinosGimnazija.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class HandleExceptionAttribute : Attribute
{
    public HandleExceptionAttribute(Type exception)
    {
        Exception = exception;
    }

    public Type Exception { get; }
}
namespace GeoCore.Shared.Exceptions;

public sealed class GeometryValidationException : GeoCoreException
{
    public GeometryValidationException(string message) : base(message)
    {
    }
}

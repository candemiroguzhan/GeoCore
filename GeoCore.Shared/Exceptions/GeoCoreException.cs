namespace GeoCore.Shared.Exceptions;

public class GeoCoreException : Exception
{
    public GeoCoreException(string message) : base(message)
    {
    }

    public GeoCoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using GeoCore.Application.Caching;

namespace GeoCore.Infrastructure.Caching;

public sealed class GeoCacheKeyBuilder : IGeoCacheKeyBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public string ForCoordinateTransform(int sourceSrid, int targetSrid, string geometryHash)
    {
        return $"geocore:spatial:transform:{sourceSrid}:{targetSrid}:{Normalize(geometryHash)}";
    }

    public string ForRasterMetadata(string fileHash)
    {
        return $"geocore:raster:metadata:{Normalize(fileHash)}";
    }

    public string ForPostGisQuery(string queryHash)
    {
        return $"geocore:postgis:query:{Normalize(queryHash)}";
    }

    public string ForOtbResult(string applicationName, string parametersHash)
    {
        return $"geocore:otb:result:{Normalize(applicationName)}:{Normalize(parametersHash)}";
    }

    public string ForSpatialQuery(string queryHash)
    {
        return $"geocore:spatial:query:{Normalize(queryHash)}";
    }

    public string ForGeometryConversion(string conversionType, string geometryHash)
    {
        return $"geocore:geometry:conversion:{Normalize(conversionType)}:{Normalize(geometryHash)}";
    }

    public string ForTileMetadata(string tileHash)
    {
        return $"geocore:tile:metadata:{Normalize(tileHash)}";
    }

    public string ForRemoteSensingOutput(string processName, string parametersHash)
    {
        return $"geocore:remote-sensing:output:{Normalize(processName)}:{Normalize(parametersHash)}";
    }

    public string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public string HashObject<T>(T value)
    {
        return Hash(JsonSerializer.Serialize(value, JsonOptions));
    }

    private static string Normalize(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}

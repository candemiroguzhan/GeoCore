namespace GeoCore.Application.Caching;

public interface IGeoCacheKeyBuilder
{
    string ForCoordinateTransform(int sourceSrid, int targetSrid, string geometryHash);

    string ForRasterMetadata(string fileHash);

    string ForPostGisQuery(string queryHash);

    string ForOtbResult(string applicationName, string parametersHash);

    string ForSpatialQuery(string queryHash);

    string ForGeometryConversion(string conversionType, string geometryHash);

    string ForTileMetadata(string tileHash);

    string ForRemoteSensingOutput(string processName, string parametersHash);

    string Hash(string value);

    string HashObject<T>(T value);
}

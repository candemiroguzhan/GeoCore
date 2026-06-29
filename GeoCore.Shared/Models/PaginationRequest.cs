using GeoCore.Shared.Constants;

namespace GeoCore.Shared.Models;

public sealed record PaginationRequest(int PageNumber = GeoCoreConstants.DefaultPageNumber, int PageSize = GeoCoreConstants.DefaultPageSize)
{
    public int Skip => (NormalizedPageNumber - 1) * NormalizedPageSize;

    public int NormalizedPageNumber => Math.Max(1, PageNumber);

    public int NormalizedPageSize => Math.Clamp(PageSize, 1, GeoCoreConstants.MaxPageSize);
}

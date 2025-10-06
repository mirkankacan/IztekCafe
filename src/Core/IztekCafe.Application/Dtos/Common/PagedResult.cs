namespace IztekCafe.Application.Dtos.Common
{
    public class PagedResult<T>(IEnumerable<T> Data, int TotalCount, int PageNumber, int PageSize)
    {
        public IEnumerable<T> Data { get; set; } = Data;
        public int TotalCount { get; set; } = TotalCount;
        public int PageNumber { get; set; } = PageNumber;
        public int PageSize { get; set; } = PageSize;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => PageNumber < TotalPages;
        public bool HasPrevious => PageNumber > 1;
    }
}
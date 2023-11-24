namespace ApiWebService.Contracts
{
    public interface IETagService
    {
        string GenerateETag<T>(T value) where T : class;
    }
}

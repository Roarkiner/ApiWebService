namespace ApiWebService.Models.RequestModels
{
    public class GetPersonsRequestModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
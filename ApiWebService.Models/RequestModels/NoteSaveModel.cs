namespace ApiWebService.Models.RequestModels
{
    public class NoteSaveModel
    {
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public Guid PersonId { get; set; }
    }
}

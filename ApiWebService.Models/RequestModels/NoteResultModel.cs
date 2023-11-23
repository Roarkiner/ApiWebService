namespace ApiWebService.Models.RequestModels
{
    public class NoteResultModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid PersonId { get; set; }
    }
}
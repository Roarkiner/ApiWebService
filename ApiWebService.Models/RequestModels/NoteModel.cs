namespace ApiWebService.Models.RequestModels
{
    public class NoteModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid PersonId { get; set; }
    }
}

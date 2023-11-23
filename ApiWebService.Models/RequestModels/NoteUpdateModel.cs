namespace ApiWebService.Models.RequestModels
{
    public class NoteUpdateModel
    {
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
    }
}

namespace GeorgiaTechLibraryProject.Models.Requests
{
    public class CreateProductRequest
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}

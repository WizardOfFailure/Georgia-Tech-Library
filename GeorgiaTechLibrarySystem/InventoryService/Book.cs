namespace InventoryService
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
    }
}

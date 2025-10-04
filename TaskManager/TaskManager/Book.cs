public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Author { get; set; } = null!;

    public decimal Price { get; set; }

    public double Rating { get; set; }

    public async Task UpdateTitleAsync(string newTitle, Book book)
    {

    }
}

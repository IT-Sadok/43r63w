using System.Data;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class Program
    {
        private static readonly object _lock = new object();

        private static readonly Book _book = new Book
        {
            Id = 1,
            Title = "Sample Book",
            Description = "This is a sample book description.",
            Author = "John Doe",
            Price = 19.99m,
            Rating = 4.5
        };


        static async Task Main(string[] args)
        {
            var tasks = new Task[100];
            for (int i = 0; i < tasks.Length; i++)
            {
                var taskNumber = i;
                tasks[i] = Task.Run(() => UpdateBookTitle(taskNumber.ToString()));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine($"Final Book Title: {_book.Title}");
        }

        static void UpdateBookTitle(string titleName)
        {
            lock (_lock)
            {
                _book.Title = $"Sample Book {titleName}";
            }
        }

    }
}

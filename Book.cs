namespace ParallelProgramming_lab2
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int NumberOnShelf { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} Книга: {Title} Автор: {Author} Доступно: {NumberOnShelf}";
        }
    }
}
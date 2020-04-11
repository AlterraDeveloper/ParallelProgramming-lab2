using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParallelProgramming_lab2
{
    public class Library
    {
        private readonly List<Book> _booksInLibrary;

        private int _readers;
        private int _waitingReaders;
        private int _waitingWriters;

        //0 = false, 1=true
        private int _isWriting;

        private readonly object _canRead = new object();
        private readonly object _canWrite = new object();

        public Library()
        {
            _booksInLibrary = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Author = "Лев Толстой",
                    Title = "Война и мир том 1",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 2,
                    Author = "Лев Толстой",
                    Title = "Война и мир том 2",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 3,
                    Author = "Тургенев",
                    Title = "Отцы и дети",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 4,
                    Author = "Генри Дэвид Торо",
                    Title = "Уолден",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 5,
                    Author = "Айтматов",
                    Title = "Плаха",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 6,
                    Author = "Марк Твен",
                    Title = "Приключения Тома Сойера",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 7,
                    Author = "Пушкин",
                    Title = "Дубровский",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 8,
                    Author = "Лермонтов",
                    Title = "Герой нашего времени",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 9,
                    Author = "Жюль Верн",
                    Title = "Дети капитана Гранта",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 10,
                    Author = "Артур Конан Дойл",
                    Title = "Записки о Шерлоке Холмсе",
                    NumberOnShelf = 10
                },
                new Book
                {
                    Id = 11,
                    Author = "Гоголь",
                    Title = "Мертвые души",
                    NumberOnShelf = 10
                },
            };
        }

        private void StartReading()
        {
            if (Interlocked.Add(ref _isWriting, 0) == 1 || Interlocked.Add(ref _waitingWriters, 0) > 0)
            {
                Interlocked.Increment(ref _waitingReaders);
                lock (_canRead) Monitor.Wait(_canRead);
                Interlocked.Decrement(ref _waitingReaders);
            }

            Interlocked.Increment(ref _readers);
            lock (_canRead) Monitor.Pulse(_canRead);
        }

        private void EndReading()
        {
            if (Interlocked.Decrement(ref _readers) == 0)
            {
                lock (_canWrite) Monitor.Pulse(_canWrite);
            }
        }

        private void StartWriting()
        {
            if (Interlocked.Add(ref _readers, 0) > 0 || Interlocked.Add(ref _isWriting, 0) == 1)
            {
                Interlocked.Increment(ref _waitingWriters);
                lock (_canWrite) Monitor.Wait(_canWrite);
                Interlocked.Decrement(ref _waitingWriters);
            }

            Interlocked.CompareExchange(ref _isWriting, 1, 0);
        }

        private void EndWriting()
        {
            Interlocked.CompareExchange(ref _isWriting, 0, 1);
            if(Interlocked.Add(ref _waitingReaders, 0) > 0) lock(_canRead) Monitor.Pulse(_canRead);
            else lock(_canWrite) Monitor.Pulse(_canWrite);
        }

        public bool AskForBook(int id)
        {
            StartReading();

            var desiredBook = _booksInLibrary.FirstOrDefault(book => book.Id == id);

            var ok = desiredBook != null;

            EndReading();

            return ok;
        }

        public string GetBook(int id)
        {
            StartWriting();

            var desiredBook = _booksInLibrary.FirstOrDefault(book => book.Id == id && book.NumberOnShelf > 0);

            if (desiredBook != null)
            {
                var index = _booksInLibrary.FindIndex(book => book == desiredBook);
                desiredBook.NumberOnShelf -= 1;
                _booksInLibrary[index] = desiredBook;
            }

            EndWriting();

            return desiredBook?.ToString();
        }
    }
}
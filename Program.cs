using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgramming_lab2
{
    internal class Program
    {
        private static readonly Library _library = new Library();
        private static Random _random = new Random();

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var thread1 = new Thread(() => { ActionsInLibrary("Вася"); });
            thread1.Start();

            var thread2 = new Thread(() => { ActionsInLibrary("Петя"); });
            thread2.Start();

            var thread3 = new Thread(() => { ActionsInLibrary("Саша"); });
            thread3.Start();

            var thread4 = new Thread(() => { ActionsInLibrary("Коля"); });
            thread4.Start();

            var thread5 = new Thread(() => { ActionsInLibrary("Миша"); });
            thread5.Start();

            Console.ReadKey();
        }

        private static void ActionsInLibrary(string threadName)
        {
            for (int i = 0; i < 50; i++)
            {
                var bookId = _random.Next(1, 20);
                if (_library.AskForBook(bookId))
                {
                    var book = _library.GetBook(bookId);

                    if (string.IsNullOrEmpty(book))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(threadName + " : книги с id " + bookId + " нет в наличии");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(threadName + " : есть в наличии " + book);
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(threadName + " : книга с id " + bookId + " не найдена");
                    Console.ResetColor();
                }
                
            }
        }
    }
}

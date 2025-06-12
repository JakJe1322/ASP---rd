using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReaderDiary.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReaderDiary.Controllers
{
    [Authorize] // vyžaduje přihlášení pro všechny akce
    public class BooksController : Controller
    {
        private static List<Book> books = new List<Book>
        {
            new Book { Id = 1, Title = "1984", Author = "George Orwell", PublishedDate = new DateTime(1949, 6, 8), Description = "Dystopian novel" },
            new Book { Id = 2, Title = "Brave New World", Author = "Aldous Huxley", PublishedDate = new DateTime(1932, 8, 30), Description = "Science fiction" }
        };

        public IActionResult Index()
        {
            return View(books);
        }

        public IActionResult Details(int id)
        {
            var book = books.Find(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // Jen Admin může přidávat
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = books.Count + 1;
                books.Add(book);
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // Jen Admin může upravovat
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var book = books.Find(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Book updatedBook)
        {
            var book = books.Find(b => b.Id == id);
            if (book == null) return NotFound();

            if (ModelState.IsValid)
            {
                book.Description = Regex.Replace(updatedBook.Description ?? "", "<.*?>", "");
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.PublishedDate = updatedBook.PublishedDate;
                return RedirectToAction("Index");
            }
            return View(updatedBook);
        }

        // Jen Admin může mazat
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var book = books.Find(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = books.Find(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
            }
            return RedirectToAction("Index");
        }
    }
}

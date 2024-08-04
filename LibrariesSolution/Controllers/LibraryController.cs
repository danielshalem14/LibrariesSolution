using LibrariesSolution.DAL;
using LibrariesSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibrariesSolution.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index()
        {
            List<Library> libs = Data.Get.Libraries
                .Include(lib => lib.ShelfList)
                .ToList();

            return View(libs);
        }

        // GET Library/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST Library/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Library library)
        {
            if (library == null)
            {
                return BadRequest();
            }
            Data.Get.Libraries.Add(library);
            Data.Get.SaveChanges();
            return RedirectToAction(nameof(Index)); // "Index"
        }


        // GET Library/AddShelf?libraryId=
        public IActionResult AddShelf(int libraryId)
        {
            Library? library = Data.Get.Libraries.FirstOrDefault(lib => lib.Id == libraryId);

            if (library == null)
            {
                return NotFound();
            }

            //Shelf shelf = new Shelf { Library = library };

            return View(new Shelf { Library = library });
        }

        // POST Library/AddShelf
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddShelf(Shelf shelf)
        {
            Library? library = Data.Get.Libraries.FirstOrDefault(lib => lib.Id == shelf.Library.Id);

            if (library == null)
            {
                return NotFound();
            }

            shelf.Library = library;
            Data.Get.Shelves.Add(shelf);
            Data.Get.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = shelf.Library.Id });
        }

        // GET Library/Details/:id
        public IActionResult Details(int id)
        {
            Library? library = Data.Get.Libraries
                .Include(lib => lib.ShelfList)
                .ThenInclude(shelf => shelf.BookList)
                .FirstOrDefault(lib => lib.Id == id);

            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        // GET Library/AddBook?shelfId
        public IActionResult AddBook(int shelfId)
        {
            Shelf? shelf = Data.Get.Shelves
                .Include(s => s.Library)
                .FirstOrDefault(slf => slf.Id == shelfId);

            if (shelf == null)
            {
                return NotFound();
            }

            Book book = new Book { Shelf = shelf };
            return View(book);
        }

        // POST Library/AddBook
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddBook(Book book)
        {
            if (book != null && book.Shelf != null)
            {
                Shelf? shelf = Data.Get.Shelves
                    .Include(slf => slf.BookList)
                    .FirstOrDefault(slf => slf.Id == book.Shelf.Id);

                if (shelf == null)
                {
                    return NotFound();
                }

                if (book.Height > shelf.Height)
                {
                    ModelState.AddModelError("", "Book is too high for this shlef!");
                    return View(book);
                }

                int totalBookWidth = shelf.BookList.Sum(b => b.Width) + book.Width;
                if (totalBookWidth > shelf.Width)
                {
                    ModelState.AddModelError("", "Book cannot be inserted!");
                    return View(book);
                }

                shelf.BookList.Add(book);
                book.Shelf = shelf;
                Data.Get.Books.Add(book);
                Data.Get.SaveChanges();

                return RedirectToAction(nameof(Details), new { id = shelf.Library.Id });
            }
            return View(book);
        }
    }
}

using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace LibraryApi.Controllers
{
    public class BooksController : ControllerBase
    {
        LibraryDataContext _context;
        IMapper _mapper;
        MapperConfiguration _config;

        public BooksController(LibraryDataContext context, IMapper mapper, MapperConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }


        //GET /books - returns a collection of all our books.
        //           - can filter on genre
        [HttpGet("books")]
        public async Task<ActionResult> GetAllBooks([FromQuery] string genre = "All")
        {
            var response = new GetBooksResponse();
            var results =  _context.Books
                .Where(b => b.RemovedFromInventory == false);

            if (genre != "All")
            {
                results = results.Where(b => b.Genre == genre);
            }
             
            response.Data = await results.ProjectTo<GetBooksResponseItem>(_config)
                .ToListAsync();
            
            response.Genre = genre;
            
            return Ok(response); 
        }

        //GET /books/{id} - get a single book or a 404

        [HttpGet("books/{bookId:int}")]
        public async Task<ActionResult> GetBookById(int bookId)
        {
            var response = await _context.Books
                .Where(b => b.RemovedFromInventory == false && b.Id == bookId)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();
            if(response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        // DELETE /books/{id} - remove a book from inventory
        [HttpDelete("books/{bookId:int}")]
        public async Task<ActionResult> RemoveBookFromInventory(int bookId)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId && b.RemovedFromInventory == false);
            if(book != null) 
            {
                book.RemovedFromInventory = true;
                await _context.SaveChangesAsync();
            }
            return NoContent(); // "Fine.... whatever"
        }
    }
}

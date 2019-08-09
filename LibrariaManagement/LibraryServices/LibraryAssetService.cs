using System;
using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _context;

        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        public LibraryAsset GetById(int id)
        {
            return 
                GetAll()
                .FirstOrDefault(asset => asset.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books
                    .First(book => book.Id == id)
                    .DeweyIndex;
            }
            else return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(a => a.Id == id))
            {
                return _context.Books
                    .First(book => book.Id == id)
                    .Isbn;
            }
            else return "";
        }

        public string GetTitle(int id)
        {
            return _context.LibraryAssets
                .First(a => a.Id == id)
                .Title;
        }

        public string GetType(int id)
        {
            var book = _context
                .LibraryAssets
                .OfType<Book>().Any(b => b.Id == id);

            return book ? "Book" : "Video";
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>()
                .Any(asset => asset.Id == id);

            var isVideo = _context.LibraryAssets.OfType<Video>()
                .Any(asset => asset.Id == id);

            return isBook
                ? _context.Books.First(book => book.Id == id).Author
                : _context.Videos.First(video => video.Id == id).Director
                ?? "Unknown";
        }
    }
}

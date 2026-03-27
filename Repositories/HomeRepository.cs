

namespace Ecommerce.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Genre>> GetGenresAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Genres
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        /// <summary>
        /// Returns a list of books optionally filtered by a search term and/or genre.
        /// The search is normalized to lower-case for database translation.
        /// </summary>
        public async Task<IReadOnlyList<Book>> GetBooksAsync(
            string searchTerm = "",
            int genreId = 0,
            CancellationToken cancellationToken = default)
        {
            // normalize search once
            var normalized = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim().ToLower();

            // left-join books to genres and use AsNoTracking for read-only queries
            var query = from book in _context.Books.AsNoTracking()
                        join genre in _context.Genres.AsNoTracking()
                            on book.GenreId equals genre.Id into gj
                        from genre in gj.DefaultIfEmpty()
                        select new { Book = book, Genre = genre };

            if (normalized is not null)
            {
                // using ToLower on the column side so EF can translate to LOWER(...)
                query = query.Where(x =>
                    x.Book.BookName.ToLower().Contains(normalized) ||
                    x.Book.AuthorName.ToLower().Contains(normalized));
            }

            if (genreId > 0)
            {
                query = query.Where(x => x.Book.GenreId == genreId);
            }

            var result = await query
                .Select(x => new Book
                {
                    Id = x.Book.Id,
                    BookName = x.Book.BookName,
                    AuthorName = x.Book.AuthorName,
                    ImageUrl = x.Book.ImageUrl,
                    Price = x.Book.Price,
                    GenreId = x.Book.GenreId,
                    GenreName = x.Genre != null ? x.Genre.GenreName : null
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}

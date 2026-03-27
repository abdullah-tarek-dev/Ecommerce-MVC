namespace Ecommerce
{
    public interface IHomeRepository
    {
        /// <summary>
        /// Returns a list of books optionally filtered by a search term and/or genre.
        /// The search is normalized to lower-case for database translation.
        /// </summary>
        Task<IReadOnlyList<Book>> GetBooksAsync(
            string searchTerm = "",
            int genreId = 0,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Genre>> GetGenresAsync(
            CancellationToken cancellationToken = default);
    }
}
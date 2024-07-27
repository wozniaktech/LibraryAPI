namespace LibraryAPI.Domain
{
    public enum BookStatus
    {
        OnShelf,       // Na półce
        Borrowed,      // Wypożyczona
        Returned,      // Zwrócona
        Damaged        // Uszkodzona
    }

    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public BookStatus Status { get; set; }

        public void SetStatus(BookStatus newStatus)
        {
            if ((Status == BookStatus.OnShelf && (newStatus == BookStatus.Returned || newStatus == BookStatus.Damaged || newStatus == BookStatus.Borrowed)) ||
                (Status == BookStatus.Borrowed && newStatus == BookStatus.Returned) ||
                (Status == BookStatus.Returned && (newStatus == BookStatus.OnShelf || newStatus == BookStatus.Damaged)) ||
                (Status== BookStatus.Damaged && (newStatus==BookStatus.OnShelf ||newStatus == BookStatus.Returned)))
            {
                Status = newStatus;
            }
            else
            {
                throw new InvalidOperationException("Invalid status transition");
            }
        }
    }
}
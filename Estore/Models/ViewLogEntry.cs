namespace Estore.Models
{
    /// <summary>
    /// Represents a record of a user having viewed a specific item.
    /// </summary>
    public class ViewLogEntry
    {
        public int Id { get; set; }
        public User Viewer { get; set; }
        public Item Item { get; set; }
    }
}

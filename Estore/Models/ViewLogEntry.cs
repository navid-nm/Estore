namespace Estore.Models
{
    public class ViewLogEntry
    {
        public int Id { get; set; }
        public User Viewer { get; set; }
        public Item Item { get; set; }
    }
}

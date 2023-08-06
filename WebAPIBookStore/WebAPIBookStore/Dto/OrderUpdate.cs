namespace WebAPIBookStore.Dto
{
    public class OrderUpdate
    {
        public int OrderId {  get; set; }
        public string Status { get; set; } = null!;
        public int ManageId { get; set; }
    }
}

namespace WebAPIBookStore.Dto
{
    public class OrderUpdate
    {
        public int orderId {  get; set; }
        public string status { get; set; } = null!;
        public int manageId { get; set; }
    }
}

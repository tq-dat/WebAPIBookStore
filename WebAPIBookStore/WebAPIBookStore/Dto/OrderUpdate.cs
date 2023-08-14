using WebAPIBookStore.Enum;

namespace WebAPIBookStore.Dto
{
    public class OrderUpdate
    {
        public int OrderId {  get; set; }
        public OrderStatus Status { get; set; }
        public int ManageId { get; set; }
    }
}

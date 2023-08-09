using WebAPIBookStore.Models;

namespace WebAPIBookStore.Dto
{
    public class Output
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Message { get; set; }
        public Object? Data { get; set; }
    }
}

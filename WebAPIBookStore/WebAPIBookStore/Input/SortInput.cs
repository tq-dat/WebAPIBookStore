using WebAPIBookStore.Result;

namespace WebAPIBookStore.Input
{
    public class SortInput
    {
        public int ByValue { get; set; }

        public bool Descending { get; set; }

        public ICollection<ProductOutput> ProductOutputs { get; set; } = null!;
    }
}

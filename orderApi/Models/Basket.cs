using orderApi.Models.Shared;

namespace orderApi.Models
{
    public class Basket : EntityBase
    {
        public int customerid { get; set; }
        public List<int> ProductId { get; set; } = new List<int>();
       
    }
}

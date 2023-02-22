using Mediator;

namespace MinimalApi.Core.Model;

public class OrderDto : IRequest<bool>
{
    public int OrderId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }


    public override string ToString()
    {
        return $"{Name} - {Description} R{Price:##.##}";
    }
}
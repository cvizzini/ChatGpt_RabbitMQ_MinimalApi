using Mediator;

namespace MinimalApi.Core.Model;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }


    public override string ToString()
    {
        return $"{Name} - {Description} R{Price:##.##}";
    }
}
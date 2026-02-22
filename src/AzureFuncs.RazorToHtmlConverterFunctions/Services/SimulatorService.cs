using Bogus;
namespace AzureFuncs.RazorToHtmlConverterFunctions.Services;
public interface ISimulatorService
{
    public List<Products> GetProductsSale(int totalLineItem);
}
public class SimulatorService : ISimulatorService
{
    public List<Products> GetProductsSale(int totalLineItem)
    {
        var lineItem = new Faker<Products>()
           .RuleFor(i => i.Name, (fake) => fake.Commerce.ProductName())
           .RuleFor(i => i.Price, (fake) => fake.Finance.Amount())
           .RuleFor(i => i.Quantity, (fake) => fake.Random.Number(1, 10))
           .Generate(totalLineItem);

        return lineItem;

    }
}

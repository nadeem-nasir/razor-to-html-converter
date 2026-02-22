namespace AzureFuncs.RazorToHtmlConverterFunctions.Models;
public class Products
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
}

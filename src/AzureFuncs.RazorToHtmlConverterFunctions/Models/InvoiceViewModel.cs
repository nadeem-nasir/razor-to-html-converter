namespace AzureFuncs.RazorToHtmlConverterFunctions.Models;
public enum InvoiceStatus
{
    Saved = 1,
    Send = 3,
    Customer = 4,
    Paid = 5,
}

public enum InvoiceUnit
{
    Hours,
    Pieces
}
public abstract class BaseViewModel
{
    public DateTimeOffset DateCreated { get; set; } = default;
}

public class InvoiceViewModel : BaseViewModel
{
    public Guid InvoiceId { get; set; }
    public int UserId { get; set; }
    public long ClientId { get; set; }
    public string? CurrencyCode { get; set; }
    public DateTime? SendTimeStamp { get; set; }
    public int InvoiceExpireTermValue { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Saved;
    public List<LineItemViewModel>? LineItem { get; set; } = [];
    public ClientViewModel? Client { get; set; }
    public UserViewModel? User { get; set; }
    public DateTime InvoiceDueDate
    {
        get
        {
            return DateCreated.AddDays(InvoiceExpireTermValue).UtcDateTime;
        }
    }
    public decimal InvoiceLineItemTotalAmount
    {
        get
        {
            return LineItem?.Sum(s => s.GetTotalAmount) ?? 0m;
        }
    }

}

public sealed class ClientViewModel : BaseViewModel
{
    public long ClientId { get; set; }
    public int UserId { get; set; }
    public string? OrganisationNumber { get; set; }
    public string? CompanyName { get; set; }
    public string? Reference { get; set; }
    public string? PhoneNumber { get; set; }
    public string? BillingEmail { get; set; }
}

public sealed class UserViewModel : BaseViewModel
{
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public sealed class LineItemViewModel : BaseViewModel
{
    public long LineItemId { get; set; }
    public Guid InvoiceId { get; set; }
    public InvoiceUnit Unit { get; set; }
    public int? Quantity { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public decimal GetTotalAmount
    {
        get
        {
            return Unit switch {
                InvoiceUnit.Hours => Quantity.GetValueOrDefault() * Amount.GetValueOrDefault(),
                InvoiceUnit.Pieces => Quantity.GetValueOrDefault() * Amount.GetValueOrDefault(),
                _ => 0m
            };

        }
    }
}


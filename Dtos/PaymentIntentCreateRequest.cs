namespace TallerMecanico.Dtos;

public class PaymentIntentCreateRequest
{
    public long Amount { get; set; }
    public string Currency { get; set; }
}
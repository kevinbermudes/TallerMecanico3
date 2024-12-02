namespace TallerMecanico.Dtos;

public class ConfirmarPagoRequest
{
    public int ClienteId { get; set; }
    public string PaymentIntentId { get; set; }
}
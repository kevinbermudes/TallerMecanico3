namespace TallerMecanico.Dtos;

public class ConfirmarPagoRequest
{
    public int ClienteId { get; set; }
    public string PaymentIntentId { get; set; }
    public int? FacturaId { get; set; }
    public List<int>? FacturaIds { get; set; }
}

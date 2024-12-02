using Stripe;

namespace TallerMecanico.Services;

public class PaymentService
{
    public async Task<PaymentIntent> CreatePaymentIntentAsync(long amount, string currency)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = new List<string> { "card" },
            };
            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error al crear el intento de pago: " + ex.Message, ex);
        }
    }
    public async Task<PaymentIntent> VerificarIntentoPagoAsync(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        return await service.GetAsync(paymentIntentId);
    }
}
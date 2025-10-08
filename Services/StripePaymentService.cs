using Stripe;
using Stripe.Checkout;
using LocOn.Models;

namespace LocOn.Services
{
    public class StripePaymentService
    {
        public Session CreateCheckoutSession(Plano plano, int userId)
        {
            // O domínio do seu projeto (Ex: http://localhost:5000)
            var YOUR_DOMAIN = "http://localhost:5000"; 

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl", // Moeda
                            UnitAmount = (long)(plano.PrecoMensal * 100), // O Stripe usa o valor em centavos
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = plano.Nome,
                                Description = plano.Descricao,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment", // Modo de pagamento único. Use "subscription" para planos
                
                // URLs para onde o usuário será redirecionado
                SuccessUrl = YOUR_DOMAIN + "/api/pagamentos/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = YOUR_DOMAIN + "/cancel",
                
                // Metadata para rastrear quem está pagando
                Metadata = new Dictionary<string, string>
                {
                    { "user_id", userId.ToString() },
                    { "plano_id", plano.Id.ToString() }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        // Método para ser usado no Webhook ou na Success URL para verificar o pagamento
        public Session RetrieveSession(string sessionId)
        {
            var service = new SessionService();
            return service.Get(sessionId);
        }
    }
}
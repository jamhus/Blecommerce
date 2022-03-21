using Stripe;
using Stripe.Checkout;

namespace Blecommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "whsec_0eb72b71bcd6c7310cbf478fc80533f8cae88a282cabb8b91ea622aaa217d993";

        public PaymentService(
            ICartService cartService,
            IAuthService authService,
            IOrderService orderService
            )
        {
            StripeConfiguration.ApiKey = "sk_test_51KZAb0Jv7qUlgLeRW7oeFwP5v6bdRGPfGyFjfEwH7QdZZaSOp8t30dPhtFit1UV1yf0KxcDw0lgXj1gpoV8oCpTb00gAZdDbXt";
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }
        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItem = new List<SessionLineItemOptions>();
            foreach (var product in products!)
            {
                lineItem.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = product.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Title,
                            Images = new List<string> { product.ImageUrl },

                        }
                    },
                    Quantity = product.Quantity,
                });

            }
            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "SE" }
                },
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItem,
                Mode = "payment",
                SuccessUrl = "https://localhost:7225/order-success",
                CancelUrl = "https://localhost:7225/cart"
            };

            var service = new SessionService();
            var session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    request.Headers["Stripe-Signature"],
                    secret
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.AddOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = e.Message
                };
            }
        }
    }
}

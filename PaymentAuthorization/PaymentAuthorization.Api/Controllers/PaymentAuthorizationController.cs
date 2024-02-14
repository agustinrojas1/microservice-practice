using Microsoft.AspNetCore.Mvc;
using PaymentAuthorization.Api.AsyncDataServices;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Services;

namespace PaymentAuthorization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentAuthorizationController : ControllerBase
    {

        private readonly IPaymentAuthorizationService _authorizationService;
        private readonly IPaymentAuthorizationRepository _authorizationRepo;

        public PaymentAuthorizationController(IPaymentAuthorizationService authorizationService, 
                                              IPaymentAuthorizationRepository authorizationRepo)
        {
            _authorizationService = authorizationService;
            _authorizationRepo = authorizationRepo;
        }

        [HttpPost]
        public async Task<IActionResult> AuthorizePayment([FromBody] PaymentAuthorizationRequest paymentRequest)
        {
            //guardo la peticion en la DB
            //repository.saveAuthorization();

            var response = await _authorizationService.AuthorizePaymentAsync(paymentRequest);
            

            return Ok(response);
        }
    }
}

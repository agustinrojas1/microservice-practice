using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentAuthorization.Api.AsyncDataServices;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Dtos;
using PaymentAuthorization.Api.Models.Enums;
using PaymentAuthorization.Api.Services;

namespace PaymentAuthorization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentAuthorizationController : ControllerBase
    {
        private readonly IPaymentAuthorizationService _authorizationService;
        private readonly IPaymentAuthorizationRepository _authorizationRepo;
        private readonly ILogger<PaymentAuthorizationController> _logger;
        private readonly IMapper _mapper;

        public PaymentAuthorizationController(IPaymentAuthorizationService authorizationService,
                                              IPaymentAuthorizationRepository authorizationRepo, 
                                              ILogger<PaymentAuthorizationController> logger,
                                              IMapper mapper)
        {
            _authorizationService = authorizationService;
            _authorizationRepo = authorizationRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentAuthorizations()
        {
            try
            {
                var authorizations = await _authorizationRepo.GetAllAuthorizations();

                if(authorizations != null)
                    return Ok(authorizations);
                else 
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during GetAllPaymentAuthorizations: " + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentAuthorizationById(int id) 
        {
            try
            {
                var authorization = await _authorizationRepo.GetAuthorizationById(id);

                if (authorization != null)
                    return Ok(authorization);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during GetPaymentAuthorizationById: " + ex.Message);
                return Problem(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AuthorizePayment([FromBody] PaymentAuthorizationRequestDto paymentRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var paymentRequest = _mapper.Map<PaymentAuthorizationRequest>(paymentRequestDto);

                await _authorizationRepo.SaveAuthorization(paymentRequest);

                var response = await _authorizationService.AuthorizePaymentAsync(paymentRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during AuthorizePayment: " + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentAuthorization(int id)
        {
            try
            {
                await _authorizationRepo.DeleteAuthorization(id);
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occurred during DeletePaymentAuthorization: " + ex.Message);
                return Problem(ex.Message);
            }
        }

    }
}

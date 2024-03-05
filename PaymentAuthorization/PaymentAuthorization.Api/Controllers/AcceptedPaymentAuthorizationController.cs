using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PaymentAuthorization.Api.Data.Repository;
using PaymentAuthorization.Api.Services;

namespace PaymentAuthorization.Api.Controllers
{
    [ApiController]
    [Route("api/accepted/[controller]")]
    public class AcceptedPaymentAuthorizationController : Controller
    {
        private readonly IAcceptedAuthorizationRepository _acceptedAuthorizationRepo;
        private readonly ILogger<AcceptedPaymentAuthorizationController> _logger;

        public AcceptedPaymentAuthorizationController(IAcceptedAuthorizationRepository acceptedAuthorizationRepo, ILogger<AcceptedPaymentAuthorizationController> logger)
        {
            _acceptedAuthorizationRepo = acceptedAuthorizationRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAcceptedPaymentAuthorizations() 
        {
            try
            {
                var acceptedAuths = await _acceptedAuthorizationRepo.GetAllAcceptedAuthorizations();

                if (acceptedAuths != null)
                    return Ok(acceptedAuths);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during GetAllAcceptedPaymentAuthorizations: " + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAcceptedAuthorizationById(int id)
        {
            try
            {
                var acceptedAuth = await _acceptedAuthorizationRepo.GetAcceptedAuthorizationById(id);

                if (acceptedAuth != null)
                    return Ok(acceptedAuth);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during GetAcceptedAuthorizationById: " + ex.Message);
                return Problem(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAcceptedPaymentAuthorization(int id)
        {
            try
            {
                await _acceptedAuthorizationRepo.DeleteAcceptedAuthorization(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during DeleteAcceptedPaymentAuthorization: " + ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}

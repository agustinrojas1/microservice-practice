using AutoMapper;
using PaymentAuthorization.Api.Models;
using PaymentAuthorization.Api.Models.Dtos;

namespace PaymentAuthorization.Api.Profiles
{
    public class PaymentAuthorizationRequestsProfile : Profile
    {
        public PaymentAuthorizationRequestsProfile() {

            CreateMap<PaymentAuthorizationRequestDto, PaymentAuthorizationRequest>();
        }
    }
}

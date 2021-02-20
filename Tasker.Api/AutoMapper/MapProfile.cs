using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Tasker.Api.AutoMapper
{
    public class MapProfile : Profile
    {
        private readonly IConfiguration _configuration;

        public MapProfile(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}

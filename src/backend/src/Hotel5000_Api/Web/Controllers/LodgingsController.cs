using AutoMapper;
using Core.Interfaces.LodgingDomain;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/lodgings")]
    [ApiController]
    public class LodgingsController : ControllerBase
    {
        private readonly ILodgingManagementService _lodgingManagementService;
        private readonly IMapper _mapper;
        public LodgingsController(ILodgingManagementService lodgingManagementService,
            IMapper mapper)
        {
            _lodgingManagementService = lodgingManagementService;
            _mapper = mapper;
        }
    }
}
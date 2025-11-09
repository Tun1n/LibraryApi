using AutoMapper;
using LibraryApi.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    public class GenreController : ControllerBase
    {
        private readonly ILogger<GenreController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GenreController(ILogger<GenreController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}

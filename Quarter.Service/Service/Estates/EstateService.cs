using AutoMapper;
using Quarter.Core;
using Quarter.Core.Dto;
using Quarter.Core.Entites;
using Quarter.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Service.Service.Estates
{
    public class EstateService : IProductService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public EstateService(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitofWork = unitOfWork;

            _mapper = mapper;
        }
        public async Task<IEnumerable<EstateDto>> GetAllEstatesAsync()
        {
            var Estates = await _unitofWork.Repository<Estate, int>().GetAllAsync();
            var mappedEstates = _mapper.Map<IEnumerable<EstateDto>>(Estates);
            return mappedEstates;
        }

        public async Task<IEnumerable<EstateLocationDto>> GetAllloctionAsync()
        {
            return _mapper.Map<IEnumerable<EstateLocationDto>>(await _unitofWork.Repository<EstateLocation, int>().GetAllAsync());
        }

        public async Task<IEnumerable<EstateTypeDto>> GetAllTypeAsync()
        {
            return _mapper.Map<IEnumerable<EstateTypeDto>>(await _unitofWork.Repository<EstateType, int>().GetAllAsync());
        }

        public async Task<EstateDto> GetEstateById(int id)
        {
            var Estate = await _unitofWork.Repository<Estate, int>().GetAsync(id);
            var mappedEstates = _mapper.Map<EstateDto>(Estate);
            return mappedEstates;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class GpusManagementService
    {
        public List<GPUDTO> Get()
        {
            var gpuDtos = new List<GPUDTO>();

            using var unitOfWork = new UnitOfWork();
            gpuDtos.AddRange(unitOfWork.GpusRepository.Get().Select(item => new GPUDTO
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return gpuDtos;
        }

        public GPUDTO GetById(int id)
        {
            var gpuDto = new GPUDTO();

            using var unitOfWork = new UnitOfWork();
            var gpu = unitOfWork.GpusRepository.GetByID(id);

            if (gpu != null)
            {
                gpuDto.Id = gpu.Id;
                gpuDto.Name = gpu.Name;
                gpuDto.Price = gpu.Price;
                gpuDto.IssuedOn = gpu.IssuedOn;
                gpuDto.Description = gpu.Description;
                gpuDto.Warranty = gpu.Warranty;
            }

            return gpuDto;
        }

        public bool Save(GPUDTO gpuDto)
        {
            if (!gpuDto.Validate()) return false;
            
            var gpu = new GPU()
            {
                Name = gpuDto.Name,
                Price = gpuDto.Price,
                IssuedOn = gpuDto.IssuedOn,
                Description = gpuDto.Description,
                Warranty = gpuDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (gpuDto.Id < 1)
                {
                    unitOfWork.GpusRepository.Insert(gpu);
                }
                else
                {
                    gpu.Id = gpuDto.Id;
                    unitOfWork.GpusRepository.Update(gpu);
                }
                unitOfWork.Save();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using var unitOfWork = new UnitOfWork();
                var gpu = unitOfWork.GpusRepository.GetByID(id);
                unitOfWork.GpusRepository.Delete(gpu);
                unitOfWork.Save();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
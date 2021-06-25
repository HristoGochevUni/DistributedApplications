using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class CoolersManagementService
    {
        public List<CoolerDTO> Get()
        {
            var coolerDtos = new List<CoolerDTO>();

            using var unitOfWork = new UnitOfWork();
            coolerDtos.AddRange(unitOfWork.CoolersRepository.Get().Select(item => new CoolerDTO
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return coolerDtos;
        }

        public CoolerDTO GetById(int id)
        {
            var coolerDto = new CoolerDTO();

            using var unitOfWork = new UnitOfWork();
            var cooler = unitOfWork.CoolersRepository.GetByID(id);

            if (cooler != null)
            {
                coolerDto.Id = cooler.Id;
                coolerDto.Name = cooler.Name;
                coolerDto.Price = cooler.Price;
                coolerDto.IssuedOn = cooler.IssuedOn;
                coolerDto.Description = cooler.Description;
                coolerDto.Warranty = cooler.Warranty;
            }

            return coolerDto;
        }

        public bool Save(CoolerDTO coolerDto)
        {
            if (!coolerDto.Validate()) return false;

            var cooler = new Cooler
            {
                Name = coolerDto.Name,
                Price = coolerDto.Price,
                IssuedOn = coolerDto.IssuedOn,
                Description = coolerDto.Description,
                Warranty = coolerDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (coolerDto.Id < 1)
                {
                    unitOfWork.CoolersRepository.Insert(cooler);
                }
                else
                {
                    cooler.Id = coolerDto.Id;
                    unitOfWork.CoolersRepository.Update(cooler);
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
                using UnitOfWork unitOfWork = new UnitOfWork();
                var cooler = unitOfWork.CoolersRepository.GetByID(id);
                unitOfWork.CoolersRepository.Delete(cooler);
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
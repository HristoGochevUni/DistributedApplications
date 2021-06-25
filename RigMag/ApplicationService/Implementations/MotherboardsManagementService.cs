using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class MotherboardsManagementService
    {
        public List<MotherboardDTO> Get()
        {
            var motherboardDtos = new List<MotherboardDTO>();

            using var unitOfWork = new UnitOfWork();
            motherboardDtos.AddRange(unitOfWork.MotherboardsRepository.Get().Select(item => new MotherboardDTO   {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return motherboardDtos;
        }

        public MotherboardDTO GetById(int id)
        {
            var motherboardDto = new MotherboardDTO();

            using var unitOfWork = new UnitOfWork();
            var motherboard = unitOfWork.MotherboardsRepository.GetByID(id);

            if (motherboard != null)
            {
                motherboardDto.Id = motherboard.Id;
                motherboardDto.Name = motherboard.Name;
                motherboardDto.Price = motherboard.Price;
                motherboardDto.IssuedOn = motherboard.IssuedOn;
                motherboardDto.Description = motherboard.Description;
                motherboardDto.Warranty = motherboard.Warranty;
            }

            return motherboardDto;
        }

        public bool Save(MotherboardDTO motherboardDto)
        {
            if (!motherboardDto.Validate()) return false;
            
            var motherboard = new Motherboard()
            {
                Name = motherboardDto.Name,
                Price = motherboardDto.Price,
                IssuedOn = motherboardDto.IssuedOn,
                Description = motherboardDto.Description,
                Warranty = motherboardDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (motherboardDto.Id < 1)
                {
                    unitOfWork.MotherboardsRepository.Insert(motherboard);
                }
                else
                {
                    motherboard.Id = motherboardDto.Id;
                    unitOfWork.MotherboardsRepository.Update(motherboard);
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
                var motherboard = unitOfWork.MotherboardsRepository.GetByID(id);
                unitOfWork.MotherboardsRepository.Delete(motherboard);
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
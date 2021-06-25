using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class RamsManagementService
    {
        public List<RAMDTO> Get()
        {
            var ramDtos = new List<RAMDTO>();

            using var unitOfWork = new UnitOfWork();
            ramDtos.AddRange(unitOfWork.RamsRepository.Get().Select(item => new RAMDTO()
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return ramDtos;
        }

        public RAMDTO GetById(int id)
        {
            var ramDto = new RAMDTO();

            using var unitOfWork = new UnitOfWork();
            var ram = unitOfWork.RamsRepository.GetByID(id);

            if (ram != null)
            {
                ramDto.Id = ram.Id;
                ramDto.Name = ram.Name;
                ramDto.Price = ram.Price;
                ramDto.IssuedOn = ram.IssuedOn;
                ramDto.Description = ram.Description;
                ramDto.Warranty = ram.Warranty;
            }

            return ramDto;
        }

        public bool Save(RAMDTO ramDto)
        {
            if (!ramDto.Validate()) return false;
            
            var ram = new RAM()
            {
                Name = ramDto.Name,
                Price = ramDto.Price,
                IssuedOn = ramDto.IssuedOn,
                Description = ramDto.Description,
                Warranty = ramDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (ramDto.Id < 1)
                {
                    unitOfWork.RamsRepository.Insert(ram);
                }
                else
                {
                    ram.Id = ramDto.Id;
                    unitOfWork.RamsRepository.Update(ram);
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
                var ram = unitOfWork.RamsRepository.GetByID(id);
                unitOfWork.RamsRepository.Delete(ram);
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
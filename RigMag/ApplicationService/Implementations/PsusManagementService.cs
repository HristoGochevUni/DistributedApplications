using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class PsusManagementService
    {
        public List<PSUDTO> Get()
        {
            var psuDtos = new List<PSUDTO>();

            using var unitOfWork = new UnitOfWork();
            psuDtos.AddRange(unitOfWork.PsusRepository.Get().Select(item => new PSUDTO
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return psuDtos;
        }

        public PSUDTO GetById(int id)
        {
            var psuDto = new PSUDTO();

            using var unitOfWork = new UnitOfWork();
            var psu = unitOfWork.PsusRepository.GetByID(id);

            if (psu != null)
            {
                psuDto.Id = psu.Id;
                psuDto.Name = psu.Name;
                psuDto.Price = psu.Price;
                psuDto.IssuedOn =psu.IssuedOn;
                psuDto.Description = psu.Description;
                psuDto.Warranty = psu.Warranty;
            }

            return  psuDto;
        }

        public bool Save(PSUDTO psuDto)
        {
            if (!psuDto.Validate()) return false;
            
            var psu = new PSU()
            {
                Name = psuDto.Name,
                Price = psuDto.Price,
                IssuedOn = psuDto.IssuedOn,
                Description = psuDto.Description,
                Warranty = psuDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (psuDto.Id < 1)
                {
                    unitOfWork.PsusRepository.Insert(psu);
                }
                else
                {
                    psu.Id = psuDto.Id;
                    unitOfWork.PsusRepository.Update(psu);
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
                var psu = unitOfWork.PsusRepository.GetByID(id);
                unitOfWork.PsusRepository.Delete(psu);
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
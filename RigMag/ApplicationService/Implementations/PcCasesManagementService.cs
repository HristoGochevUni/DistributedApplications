using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class PcCasesManagementService
    {
        public List<PCCaseDTO> Get()
        {
            var pcCaseDtos = new List<PCCaseDTO>();

            using var unitOfWork = new UnitOfWork();
            pcCaseDtos.AddRange(unitOfWork.PcCasesRepository.Get().Select(item => new PCCaseDTO()
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return pcCaseDtos;
        }

        public PCCaseDTO GetById(int id)
        {
            var pcCaseDto = new PCCaseDTO();

            using var unitOfWork = new UnitOfWork();
            var pcCase = unitOfWork.PcCasesRepository.GetByID(id);

            if (pcCase != null)
            {
                pcCaseDto.Id = pcCase.Id;
                pcCaseDto.Name = pcCase.Name;
                pcCaseDto.Price = pcCase.Price;
                pcCaseDto.IssuedOn = pcCase.IssuedOn;
                pcCaseDto.Description = pcCase.Description;
                pcCaseDto.Warranty = pcCase.Warranty;
            }

            return pcCaseDto;
        }

        public bool Save(PCCaseDTO pcCaseDto)
        {
            if (!pcCaseDto.Validate()) return false;
            
            var pcCase = new PCCase()
            {
                Name = pcCaseDto.Name,
                Price = pcCaseDto.Price,
                IssuedOn = pcCaseDto.IssuedOn,
                Description = pcCaseDto.Description,
                Warranty = pcCaseDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (pcCaseDto.Id < 1)
                {
                    unitOfWork.PcCasesRepository.Insert(pcCase);
                }
                else
                {
                    pcCase.Id = pcCaseDto.Id;
                    unitOfWork.PcCasesRepository.Update(pcCase);
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
                var pcCase = unitOfWork.PcCasesRepository.GetByID(id);
                unitOfWork.PcCasesRepository.Delete(pcCase);
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
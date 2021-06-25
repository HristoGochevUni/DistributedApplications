using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class CpusManagementService
    {
        public List<CPUDTO> Get()
        {
            var cpuDtos = new List<CPUDTO>();
         
            using var unitOfWork = new UnitOfWork();
            cpuDtos.AddRange(unitOfWork.CpusRepository.Get().Select(item => new CPUDTO
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return cpuDtos;
        }

        public CPUDTO GetById(int id)
        {
            var cpuDto = new CPUDTO();

            using var unitOfWork = new UnitOfWork();
            var cpu = unitOfWork.CpusRepository.GetByID(id);

            if (cpu != null)
            {
                cpuDto.Id = cpu.Id;
                cpuDto.Name = cpu.Name;
                cpuDto.Price = cpu.Price;
                cpuDto.IssuedOn = cpu.IssuedOn;
                cpuDto.Description = cpu.Description;
                cpuDto.Warranty = cpu.Warranty;
            }

            return cpuDto;
        }

        public bool Save(CPUDTO cpuDto)
        { 
            if (!cpuDto.Validate()) return false;
            
            var cpu = new CPU
            {
                Name = cpuDto.Name,
                Price = cpuDto.Price,
                IssuedOn = cpuDto.IssuedOn,
                Description = cpuDto.Description,
                Warranty = cpuDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (cpuDto.Id < 1)
                {
                    unitOfWork.CpusRepository.Insert(cpu);
                }
                else
                {
                    cpu.Id = cpuDto.Id;
                    unitOfWork.CpusRepository.Update(cpu);
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
                var cpu = unitOfWork.CpusRepository.GetByID(id);
                unitOfWork.CpusRepository.Delete(cpu);
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
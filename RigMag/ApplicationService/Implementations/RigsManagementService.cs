using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class RigsManagementService
    {
        private CoolersManagementService CoolersManagementService { get; }
        private CpusManagementService CpusManagementService { get; }
        private DrivesManagementService DrivesManagementService { get; }
        private GpusManagementService GpusManagementService { get; }
        private MotherboardsManagementService MotherboardsManagementService { get; }
        private PcCasesManagementService PcCasesManagementService { get; }
        private PsusManagementService PsusManagementService { get; }
        private RamsManagementService RamsManagementService { get; }

        public RigsManagementService()
        {
            CoolersManagementService = new CoolersManagementService();
            CpusManagementService = new CpusManagementService();
            DrivesManagementService = new DrivesManagementService();
            GpusManagementService = new GpusManagementService();
            MotherboardsManagementService = new MotherboardsManagementService();
            PcCasesManagementService = new PcCasesManagementService();
            PsusManagementService = new PsusManagementService();
            RamsManagementService = new RamsManagementService();
        }

        public List<RIGDTO> Get()
        {
            var rigDtos = new List<RIGDTO>();

            using var unitOfWork = new UnitOfWork();
            var collection = unitOfWork.RigsRepository.Get();
            rigDtos.AddRange(collection.Select(item => new RIGDTO
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                IssuedOn = item.IssuedOn,
                Description = item.Description,
                Warranty = item.Warranty,
                CpuId = item.CPU_Id,
                CoolerId = item.Cooler_Id, 
                DriveId = item.Drive_Id, 
                GpuId = item.GPU_Id, 
                MotherboardId = item.Motherboard_Id,
                PcCaseId = item.PCCase_Id,
                PsuId = item.PSU_Id,
                RamId = item.RAM_Id
            }));
            return rigDtos;
        }

        public RIGDTO GetById(int id)
        {
            using var unitOfWork = new UnitOfWork();
            var rig = unitOfWork.RigsRepository.GetByID(id);

            if (rig == null) return new RIGDTO();

            var rigDto = new RIGDTO
            {
                Id = rig.Id,
                Name = rig.Name,
                Price = rig.Price,
                IssuedOn = rig.IssuedOn,
                Description = rig.Description,
                Warranty = rig.Warranty,
                CpuId = rig.CPU_Id,
                CoolerId = rig.Cooler_Id,
                DriveId = rig.Drive_Id,
                GpuId = rig.GPU_Id,
                MotherboardId = rig.Motherboard_Id,
                PcCaseId = rig.PCCase_Id,
                PsuId = rig.PSU_Id,
                RamId = rig.RAM_Id
            };

            return rigDto;
        }

        public bool Save(RIGDTO rigDto)
        {
            if (!rigDto.Validate()) return false;

            var rig = new Rig
            {
                Name = rigDto.Name,
                Price = rigDto.Price,
                IssuedOn = rigDto.IssuedOn,
                Description = rigDto.Description,
                Warranty = rigDto.Warranty,
                CPU_Id = rigDto.CpuId,
                Cooler_Id = rigDto.CoolerId,
                Drive_Id = rigDto.DriveId,
                GPU_Id = rigDto.GpuId,
                Motherboard_Id = rigDto.MotherboardId,
                PCCase_Id = rigDto.PcCaseId,
                PSU_Id = rigDto.PsuId,
                RAM_Id = rigDto.RamId
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (rigDto.Id < 1)
                {
                    unitOfWork.RigsRepository.Insert(rig);
                }
                else
                {
                    rig.Id = rigDto.Id;
                    unitOfWork.RigsRepository.Update(rig);
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
                var rig = unitOfWork.RigsRepository.GetByID(id);
                unitOfWork.RigsRepository.Delete(rig);
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
using System.Collections.Generic;
using System.Linq;
using ApplicationService.DTOs;
using Data.Entities;
using Repository.Implementations;

namespace ApplicationService.Implementations
{
    public class DrivesManagementService
    {
        public List<DriveDTO> Get()
        {
            var driveDtos = new List<DriveDTO>();

            using var unitOfWork = new UnitOfWork();
            driveDtos.AddRange(unitOfWork.DrivesRepository.Get().Select(item => new DriveDTO
            {
                Id = item.Id, Name = item.Name, Price = item.Price, IssuedOn = item.IssuedOn,
                Description = item.Description, Warranty = item.Warranty
            }));

            return driveDtos;
        }

        public DriveDTO GetById(int id)
        {
            var driveDto = new DriveDTO();

            using var unitOfWork = new UnitOfWork();
            var drive = unitOfWork.DrivesRepository.GetByID(id);

            if (drive != null)
            {
                driveDto.Id = drive.Id;
                driveDto.Name = drive.Name;
                driveDto.Price = drive.Price;
                driveDto.IssuedOn = drive.IssuedOn;
                driveDto.Description = drive.Description;
                driveDto.Warranty = drive.Warranty;
            }

            return driveDto;
        }

        public bool Save(DriveDTO driveDto)
        {
            if (!driveDto.Validate()) return false;

            var drive = new Drive
            {
                Name = driveDto.Name,
                Price = driveDto.Price,
                IssuedOn = driveDto.IssuedOn,
                Description = driveDto.Description,
                Warranty = driveDto.Warranty
            };

            try
            {
                using var unitOfWork = new UnitOfWork();
                if (driveDto.Id < 1)
                {
                    unitOfWork.DrivesRepository.Insert(drive);
                }
                else
                {
                    drive.Id = driveDto.Id;
                    unitOfWork.DrivesRepository.Update(drive);
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
                var drive = unitOfWork.DrivesRepository.GetByID(id);
                unitOfWork.DrivesRepository.Delete(drive);
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
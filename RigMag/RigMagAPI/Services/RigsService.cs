using System;
using System.Threading.Tasks;
using ApplicationService.DTOs;
using ApplicationService.Implementations;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace RigMagAPI.Services
{
    public class RigsService : Rig.RigBase
    {
        private readonly RigsManagementService _service = new();

        private readonly ILogger<RigsService> _logger;

        public RigsService(ILogger<RigsService> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public override Task<RigGetResponse> RigGet(RigGetRequest request, ServerCallContext context)
        {
            var response = new RigGetResponse();

            var dto = _service.GetById(request.Id);

            if (dto.Id < 1) return Task.FromResult(response);

            response.Id = dto.Id;
            response.Name = dto.Name;
            response.Price = dto.Price;
            if (dto.IssuedOn != null) response.IssuedOn = dto.IssuedOn.ToString();
            if (dto.Description != null) response.Description = dto.Description;
            if (dto.Warranty != null) response.Warranty = (float) dto.Warranty;
            response.CpuId = dto.CpuId;
            response.CoolerId = dto.CoolerId;
            response.DriveId = dto.DriveId;
            response.GpuId = dto.GpuId;
            response.MotherboardId = dto.MotherboardId;
            response.PcCaseId = dto.PcCaseId;
            response.PsuId = dto.PsuId;
            response.RamId = dto.RamId;

            return Task.FromResult(response);
        }

        [Authorize]
        public override async Task RigGetAllByName(RigGetAllByNameRequest request,
            IServerStreamWriter<RigGetResponse> responseStream, ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                if (!dto.Name.Contains(request.Name)) continue;
                var response = new RigGetResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    CpuId = dto.CpuId,
                    CoolerId = dto.CoolerId,
                    DriveId = dto.DriveId,
                    GpuId = dto.GpuId,
                    MotherboardId = dto.MotherboardId,
                    PcCaseId = dto.PcCaseId,
                    PsuId = dto.PsuId,
                    RamId = dto.RamId
                };
                if (dto.IssuedOn != null) response.IssuedOn = dto.IssuedOn.ToString();
                if (dto.Description != null) response.Description = dto.Description;
                if (dto.Warranty != null) response.Warranty = (float) dto.Warranty;

                await responseStream.WriteAsync(response);
            }
        }

        [Authorize]
        public override async Task RigGetAll(RigGetAllRequest request,
            IServerStreamWriter<RigGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                var response = new RigGetResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    CpuId = dto.CpuId,
                    CoolerId = dto.CoolerId,
                    DriveId = dto.DriveId,
                    GpuId = dto.GpuId,
                    MotherboardId = dto.MotherboardId,
                    PcCaseId = dto.PcCaseId,
                    PsuId = dto.PsuId,
                    RamId = dto.RamId
                };
                if (dto.IssuedOn != null) response.IssuedOn = dto.IssuedOn.ToString();
                if (dto.Description != null) response.Description = dto.Description;
                if (dto.Warranty != null) response.Warranty = (float) dto.Warranty;

                await responseStream.WriteAsync(response);
            }
        }

        [Authorize]
        public override Task<RigSaveResponse> RigSave(RigSaveRequest request, ServerCallContext context)
        {
            var dto = new RIGDTO()
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                CpuId = request.CpuId,
                CoolerId = request.CoolerId,
                DriveId = request.DriveId,
                GpuId = request.GpuId,
                MotherboardId = request.MotherboardId,
                PcCaseId = request.PcCaseId,
                PsuId = request.PsuId,
                RamId = request.RamId
            };

            if (request.HasIssuedOn)
            {
                try
                {
                    dto.IssuedOn = DateTime.Parse(request.IssuedOn);
                }
                catch
                {
                    // ignored
                }
            }

            if (request.HasDescription) dto.Description = request.Description;
            if (request.HasWarranty) dto.Warranty = request.Warranty;

            bool saved = _service.Save(dto);

            var response = new RigSaveResponse {Success = saved};

            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<RigDeleteResponse> RigDelete(RigDeleteRequest request,
            ServerCallContext context)
        {
            bool deleted = _service.Delete(request.Id);

            var response = new RigDeleteResponse {Success = deleted};

            return Task.FromResult(response);
        }
    }
}
using System;
using System.Threading.Tasks;
using ApplicationService.DTOs;
using ApplicationService.Implementations;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace RigMagAPI.Services
{
    public class MotherboardsService : Motherboard.MotherboardBase
    {
        private readonly MotherboardsManagementService _service = new();

        private readonly ILogger<MotherboardsService> _logger;

        public MotherboardsService(ILogger<MotherboardsService> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public override Task<MotherboardGetResponse> MotherboardGet(MotherboardGetRequest request,
            ServerCallContext context)
        {
            var response = new MotherboardGetResponse();

            var dto = _service.GetById(request.Id);

            if (dto.Id < 1) return Task.FromResult(response);

            response.Id = dto.Id;
            response.Name = dto.Name;
            response.Price = dto.Price;
            if (dto.IssuedOn != null) response.IssuedOn = dto.IssuedOn.ToString();
            if (dto.Description != null) response.Description = dto.Description;
            if (dto.Warranty != null) response.Warranty = (float) dto.Warranty;

            return Task.FromResult(response);
        }

        [Authorize]
        public override async Task MotherboardGetAllByName(MotherboardGetAllByNameRequest request, IServerStreamWriter<MotherboardGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                if (!dto.Name.Contains(request.Name)) continue;
                var response = new MotherboardGetResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price
                };
                if (dto.IssuedOn != null) response.IssuedOn = dto.IssuedOn.ToString();
                if (dto.Description != null) response.Description = dto.Description;
                if (dto.Warranty != null) response.Warranty = (float) dto.Warranty;

                await responseStream.WriteAsync(response);
            }
        }

        [Authorize]
        public override async Task MotherboardGetAll(MotherboardGetAllRequest request,
            IServerStreamWriter<MotherboardGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                var response = new MotherboardGetResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price
                };
                if (dto.IssuedOn != null) response.IssuedOn = dto.IssuedOn.ToString();
                if (dto.Description != null) response.Description = dto.Description;
                if (dto.Warranty != null) response.Warranty = (float) dto.Warranty;

                await responseStream.WriteAsync(response);
            }
        }

        [Authorize]
        public override Task<MotherboardSaveResponse> MotherboardSave(MotherboardSaveRequest request,
            ServerCallContext context)
        {
            var dto = new MotherboardDTO
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price
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

            var response = new MotherboardSaveResponse {Success = saved};

            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<MotherboardDeleteResponse> MotherboardDelete(MotherboardDeleteRequest request,
            ServerCallContext context)
        {
            bool deleted = _service.Delete(request.Id);

            var response = new MotherboardDeleteResponse {Success = deleted};

            return Task.FromResult(response);
        }
    }
}
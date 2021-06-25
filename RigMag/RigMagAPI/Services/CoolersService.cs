using System;
using System.Threading.Tasks;
using ApplicationService.DTOs;
using ApplicationService.Implementations;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;


namespace RigMagAPI.Services
{
    public class CoolersService : Cooler.CoolerBase
    {
        private readonly CoolersManagementService _service = new();

        private readonly ILogger<CoolersService> _logger;

        public CoolersService(ILogger<CoolersService> logger)
        {
            _logger = logger;
        }


        [Authorize]
        public override Task<CoolerGetResponse> CoolerGet(CoolerGetRequest request, ServerCallContext context)
        {
            var response = new CoolerGetResponse();

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
        public override async Task CoolerGetAllByName(CoolerGetAllByNameRequest request,
            IServerStreamWriter<CoolerGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                if (!dto.Name.Contains(request.Name)) continue;
                var response = new CoolerGetResponse()
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
        public override async Task CoolerGetAll(CoolerGetAllRequest request,
            IServerStreamWriter<CoolerGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                var response = new CoolerGetResponse()
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
        public override Task<CoolerSaveResponse> CoolerSave(CoolerSaveRequest request, ServerCallContext context)
        {
            var dto = new CoolerDTO()
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

            var saved = _service.Save(dto);

            var response = new CoolerSaveResponse {Success = saved};

            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<CoolerDeleteResponse> CoolerDelete(CoolerDeleteRequest request,
            ServerCallContext context)
        {
            bool deleted = _service.Delete(request.Id);

            var response = new CoolerDeleteResponse {Success = deleted};

            return Task.FromResult(response);
        }
    }
}
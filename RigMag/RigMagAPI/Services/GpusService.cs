using System;
using System.Threading.Tasks;
using ApplicationService.DTOs;
using ApplicationService.Implementations;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace RigMagAPI.Services
{
    public class GpusService : GPU.GPUBase
    {
        private readonly GpusManagementService _service = new();

        private readonly ILogger<GpusService> _logger;

        public GpusService(ILogger<GpusService> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public override Task<GPUGetResponse> GPUGet(GPUGetRequest request, ServerCallContext context)
        {
            var response = new GPUGetResponse();

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
        public override async Task GPUGetAllByName(GPUGetAllByNameRequest request,
            IServerStreamWriter<GPUGetResponse> responseStream, ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                if (!dto.Name.Contains(request.Name)) continue;
                var response = new GPUGetResponse()
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
        public override async Task GPUGetAll(GPUGetAllRequest request,
            IServerStreamWriter<GPUGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                var response = new GPUGetResponse()
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
        public override Task<GPUSaveResponse> GPUSave(GPUSaveRequest request, ServerCallContext context)
        {
            var dto = new GPUDTO
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

            var response = new GPUSaveResponse {Success = saved};

            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<GPUDeleteResponse> GPUDelete(GPUDeleteRequest request,
            ServerCallContext context)
        {
            bool deleted = _service.Delete(request.Id);

            var response = new GPUDeleteResponse {Success = deleted};

            return Task.FromResult(response);
        }
    }
}
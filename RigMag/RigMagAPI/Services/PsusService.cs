using System;
using System.Threading.Tasks;
using ApplicationService.DTOs;
using ApplicationService.Implementations;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace RigMagAPI.Services
{
    public class PsusService : PSU.PSUBase
    {
        private readonly PsusManagementService _service = new();

        private readonly ILogger<PsusService> _logger;

        public PsusService(ILogger<PsusService> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public override Task<PSUGetResponse> PSUGet(PSUGetRequest request, ServerCallContext context)
        {
            var response = new PSUGetResponse();

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
        public override async Task PSUGetAllByName(PSUGetAllByNameRequest request,
            IServerStreamWriter<PSUGetResponse> responseStream, ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                if (!dto.Name.Contains(request.Name)) continue;
                var response = new PSUGetResponse()
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
        public override async Task PSUGetAll(PSUGetAllRequest request,
            IServerStreamWriter<PSUGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                var response = new PSUGetResponse()
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
        public override Task<PSUSaveResponse> PSUSave(PSUSaveRequest request, ServerCallContext context)
        {
            var dto = new PSUDTO
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

            var response = new PSUSaveResponse {Success = saved};

            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<PSUDeleteResponse> PSUDelete(PSUDeleteRequest request,
            ServerCallContext context)
        {
            bool deleted = _service.Delete(request.Id);

            var response = new PSUDeleteResponse {Success = deleted};

            return Task.FromResult(response);
        }
    }
}
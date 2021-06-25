using System;
using System.Threading.Tasks;
using ApplicationService.DTOs;
using ApplicationService.Implementations;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace RigMagAPI.Services
{
    public class RamsService : RAM.RAMBase
    {
        private readonly RamsManagementService _service = new();

        private readonly ILogger<RamsService> _logger;

        public RamsService(ILogger<RamsService> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public override Task<RAMGetResponse> RAMGet(RAMGetRequest request, ServerCallContext context)
        {
            var response = new RAMGetResponse();

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
        public override async Task RAMGetAllByName(RAMGetAllByNameRequest request,
            IServerStreamWriter<RAMGetResponse> responseStream, ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                if (!dto.Name.Contains(request.Name)) continue;
                var response = new RAMGetResponse()
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
        public override async Task RAMGetAll(RAMGetAllRequest request,
            IServerStreamWriter<RAMGetResponse> responseStream,
            ServerCallContext context)
        {
            foreach (var dto in _service.Get())
            {
                if (context.CancellationToken.IsCancellationRequested) break;
                var response = new RAMGetResponse()
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
        public override Task<RAMSaveResponse> RAMSave(RAMSaveRequest request, ServerCallContext context)
        {
            var dto = new RAMDTO
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

            var response = new RAMSaveResponse {Success = saved};

            return Task.FromResult(response);
        }

        [Authorize]
        public override Task<RAMDeleteResponse> RAMDelete(RAMDeleteRequest request,
            ServerCallContext context)
        {
            bool deleted = _service.Delete(request.Id);

            var response = new RAMDeleteResponse {Success = deleted};

            return Task.FromResult(response);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class GpusManagementService
    {
        private readonly GPU.GPUClient _client;
        private readonly string _accessToken;

        public GpusManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new GPU.GPUClient(channel);
        }

        public async Task<GPUDTO> Get(int gpuId)
        {
            var request = new GPUGetRequest {Id = gpuId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            GPUGetResponse response;
            try
            {
                response = await _client.GPUGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new GPUDTO
            {
                Id = response.Id,
                Name = response.Name,
                Price = response.Price
            };
            if (response.HasIssuedOn)
            {
                try
                {
                    output.IssuedOn = DateTime.Parse(response.IssuedOn);
                }
                catch
                {
                    // ignored
                }
            }

            if (response.HasDescription) output.Description = response.Description;
            if (response.HasWarranty) output.Warranty = response.Warranty;

            return output;
        }

        public async Task<List<GPUDTO>> GetAllByName(string name)
        {
            var output = new List<GPUDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new GPUGetAllByNameRequest() {Name = name};
                using var call = _client.GPUGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var gpuDto = new GPUDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            gpuDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) gpuDto.Description = current.Description;
                    if (current.HasWarranty) gpuDto.Warranty = current.Warranty;

                    output.Add(gpuDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<GPUDTO>> GetAll()
        {
            var output = new List<GPUDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.GPUGetAll(new GPUGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var gpuDto = new GPUDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            gpuDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) gpuDto.Description = current.Description;
                    if (current.HasWarranty) gpuDto.Warranty = current.Warranty;

                    output.Add(gpuDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(GPUDTO gpuDto)
        {
            var request = new GPUSaveRequest()
            {
                Id = gpuDto.Id,
                Name = gpuDto.Name,
                Price = gpuDto.Price
            };
            if (gpuDto.IssuedOn != null) request.IssuedOn = gpuDto.IssuedOn.ToString();
            if (gpuDto.Description != null) request.Description = gpuDto.Description;
            if (gpuDto.Warranty != null) request.Warranty = (float) gpuDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.GPUSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int gpuId)
        {
            var request = new GPUDeleteRequest {Id = gpuId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.GPUDeleteAsync(request, headers);

                return response.Success;
            }

            catch
            {
                return false;
            }
        }
    }
}
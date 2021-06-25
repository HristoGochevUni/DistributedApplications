using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class CpusManagementService
    {
        private readonly CPU.CPUClient _client;
        private readonly string _accessToken;

        public CpusManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new CPU.CPUClient(channel);
        }

        public async Task<CPUDTO> Get(int cpuId)
        {
            var request = new CPUGetRequest {Id = cpuId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            CPUGetResponse response;
            try
            {
                response = await _client.CPUGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new CPUDTO
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

        public async Task<List<CPUDTO>> GetAllByName(string name)
        {
            var output = new List<CPUDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new CPUGetAllByNameRequest() {Name = name};
                using var call = _client.CPUGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var cpuDto = new CPUDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            cpuDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) cpuDto.Description = current.Description;
                    if (current.HasWarranty) cpuDto.Warranty = current.Warranty;

                    output.Add(cpuDto);
                }
            }
            catch
            {
                // ignored
            }


            return output;
        }

        public async Task<List<CPUDTO>> GetAll()
        {
            var output = new List<CPUDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.CPUGetAll(new CPUGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var cpuDto = new CPUDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            cpuDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) cpuDto.Description = current.Description;
                    if (current.HasWarranty) cpuDto.Warranty = current.Warranty;

                    output.Add(cpuDto);
                }
            }
            catch
            {
                // ignored
            }


            return output;
        }

        public async Task<bool> Save(CPUDTO cpuDto)
        {
            var request = new CPUSaveRequest()
            {
                Id = cpuDto.Id,
                Name = cpuDto.Name,
                Price = cpuDto.Price
            };
            if (cpuDto.IssuedOn != null) request.IssuedOn = cpuDto.IssuedOn.ToString();
            if (cpuDto.Description != null) request.Description = cpuDto.Description;
            if (cpuDto.Warranty != null) request.Warranty = (float) cpuDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.CPUSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int cpuId)
        {
            var request = new CPUDeleteRequest {Id = cpuId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.CPUDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
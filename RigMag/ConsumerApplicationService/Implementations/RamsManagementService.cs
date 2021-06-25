using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class RamsManagementService
    {
        private readonly RAM.RAMClient _client;
        private readonly string _accessToken;

        public RamsManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new RAM.RAMClient(channel);
        }

        public async Task<RAMDTO> Get(int ramId)
        {
            var request = new RAMGetRequest {Id = ramId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            RAMGetResponse response;
            try
            {
                response = await _client.RAMGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new RAMDTO
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

        public async Task<List<RAMDTO>> GetAllByName(string name)
        {
            var output = new List<RAMDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new RAMGetAllByNameRequest() {Name = name};
                using var call = _client.RAMGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var ramDto = new RAMDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            ramDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) ramDto.Description = current.Description;
                    if (current.HasWarranty) ramDto.Warranty = current.Warranty;

                    output.Add(ramDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<RAMDTO>> GetAll()
        {
            var output = new List<RAMDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.RAMGetAll(new RAMGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var ramDto = new RAMDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            ramDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) ramDto.Description = current.Description;
                    if (current.HasWarranty) ramDto.Warranty = current.Warranty;

                    output.Add(ramDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(RAMDTO ramDto)
        {
            var request = new RAMSaveRequest()
            {
                Id = ramDto.Id,
                Name = ramDto.Name,
                Price = ramDto.Price
            };
            if (ramDto.IssuedOn != null) request.IssuedOn = ramDto.IssuedOn.ToString();
            if (ramDto.Description != null) request.Description = ramDto.Description;
            if (ramDto.Warranty != null) request.Warranty = (float) ramDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.RAMSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int ramId)
        {
            var request = new RAMDeleteRequest {Id = ramId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.RAMDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
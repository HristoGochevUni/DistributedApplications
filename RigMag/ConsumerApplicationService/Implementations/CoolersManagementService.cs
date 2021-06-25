using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class CoolersManagementService
    {
        private readonly Cooler.CoolerClient _client;
        private readonly string _accessToken;

        public CoolersManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new Cooler.CoolerClient(channel);
        }

        public async Task<CoolerDTO> Get(int coolerId)
        {
            var request = new CoolerGetRequest {Id = coolerId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            CoolerGetResponse response;
            try
            {
                response = await _client.CoolerGetAsync(request, headers);
            }
            catch
            {
                return null;
            }


            if (response.Id < 1) return null;

            var output = new CoolerDTO
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

        public async Task<List<CoolerDTO>> GetAllByName(string name)
        {
            var output = new List<CoolerDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            try
            {
                var request = new CoolerGetAllByNameRequest() {Name = name};
                using var call = _client.CoolerGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var coolerDto = new CoolerDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            coolerDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) coolerDto.Description = current.Description;
                    if (current.HasWarranty) coolerDto.Warranty = current.Warranty;

                    output.Add(coolerDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<CoolerDTO>> GetAll()
        {
            var output = new List<CoolerDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            try
            {
                using var call = _client.CoolerGetAll(new CoolerGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var coolerDto = new CoolerDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            coolerDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) coolerDto.Description = current.Description;
                    if (current.HasWarranty) coolerDto.Warranty = current.Warranty;

                    output.Add(coolerDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(CoolerDTO coolerDto)
        {
            var request = new CoolerSaveRequest()
            {
                Id = coolerDto.Id,
                Name = coolerDto.Name,
                Price = coolerDto.Price
            };
            if (coolerDto.IssuedOn != null) request.IssuedOn = coolerDto.IssuedOn.ToString();
            if (coolerDto.Description != null) request.Description = coolerDto.Description;
            if (coolerDto.Warranty != null) request.Warranty = (float) coolerDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.CoolerSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int coolerId)
        {
            var request = new CoolerDeleteRequest {Id = coolerId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.CoolerDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
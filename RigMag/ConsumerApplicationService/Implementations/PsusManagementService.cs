using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class PsusManagementService
    {
        private readonly PSU.PSUClient _client;
        private readonly string _accessToken;

        public PsusManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new PSU.PSUClient(channel);
        }

        public async Task<PSUDTO> Get(int psuId)
        {
            var request = new PSUGetRequest {Id = psuId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            PSUGetResponse response;
            try
            {
                response = await _client.PSUGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new PSUDTO
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


        public async Task<List<PSUDTO>> GetAllByName(string name)
        {
            var output = new List<PSUDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new PSUGetAllByNameRequest() {Name = name};
                using var call = _client.PSUGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var psuDto = new PSUDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            psuDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) psuDto.Description = current.Description;
                    if (current.HasWarranty) psuDto.Warranty = current.Warranty;

                    output.Add(psuDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<PSUDTO>> GetAll()
        {
            var output = new List<PSUDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.PSUGetAll(new PSUGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var psuDto = new PSUDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            psuDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) psuDto.Description = current.Description;
                    if (current.HasWarranty) psuDto.Warranty = current.Warranty;

                    output.Add(psuDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(PSUDTO psuDto)
        {
            var request = new PSUSaveRequest()
            {
                Id = psuDto.Id,
                Name = psuDto.Name,
                Price = psuDto.Price
            };
            if (psuDto.IssuedOn != null) request.IssuedOn = psuDto.IssuedOn.ToString();
            if (psuDto.Description != null) request.Description = psuDto.Description;
            if (psuDto.Warranty != null) request.Warranty = (float) psuDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.PSUSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int psuId)
        {
            var request = new PSUDeleteRequest {Id = psuId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.PSUDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
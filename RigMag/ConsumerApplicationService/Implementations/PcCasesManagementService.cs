using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class PcCasesManagementService
    {
        private readonly PCCase.PCCaseClient _client;
        private readonly string _accessToken;

        public PcCasesManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new PCCase.PCCaseClient(channel);
        }

        public async Task<PCCaseDTO> Get(int pcCaseId)
        {
            var request = new PCCaseGetRequest {Id = pcCaseId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            PCCaseGetResponse response;
            try
            {
                response = await _client.PCCaseGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new PCCaseDTO
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

        public async Task<List<PCCaseDTO>> GetAllByName(string name)
        {
            var output = new List<PCCaseDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new PCCaseGetAllByNameRequest() {Name = name};
                using var call = _client.PCCaseGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var pcCaseDto = new PCCaseDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            pcCaseDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) pcCaseDto.Description = current.Description;
                    if (current.HasWarranty) pcCaseDto.Warranty = current.Warranty;

                    output.Add(pcCaseDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<PCCaseDTO>> GetAll()
        {
            var output = new List<PCCaseDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.PCCaseGetAll(new PCCaseGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var pcCaseDto = new PCCaseDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            pcCaseDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) pcCaseDto.Description = current.Description;
                    if (current.HasWarranty) pcCaseDto.Warranty = current.Warranty;

                    output.Add(pcCaseDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(PCCaseDTO pcCaseDto)
        {
            var request = new PCCaseSaveRequest()
            {
                Id = pcCaseDto.Id,
                Name = pcCaseDto.Name,
                Price = pcCaseDto.Price
            };
            if (pcCaseDto.IssuedOn != null) request.IssuedOn = pcCaseDto.IssuedOn.ToString();
            if (pcCaseDto.Description != null) request.Description = pcCaseDto.Description;
            if (pcCaseDto.Warranty != null) request.Warranty = (float) pcCaseDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.PCCaseSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int pcCaseId)
        {
            var request = new PCCaseDeleteRequest {Id = pcCaseId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.PCCaseDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
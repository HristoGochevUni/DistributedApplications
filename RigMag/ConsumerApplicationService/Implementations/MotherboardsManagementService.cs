using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class MotherboardsManagementService
    {
        private readonly Motherboard.MotherboardClient _client;
        private readonly string _accessToken;

        public MotherboardsManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new Motherboard.MotherboardClient(channel);
        }

        public async Task<MotherboardDTO> Get(int motherboardId)
        {
            var request = new MotherboardGetRequest {Id = motherboardId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};

            MotherboardGetResponse response;
            try
            {
                response = await _client.MotherboardGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new MotherboardDTO
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

        public async Task<List<MotherboardDTO>> GetAllByName(string name)
        {
            var output = new List<MotherboardDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new MotherboardGetAllByNameRequest() {Name = name};
                using var call = _client.MotherboardGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var motherboardDto = new MotherboardDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            motherboardDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) motherboardDto.Description = current.Description;
                    if (current.HasWarranty) motherboardDto.Warranty = current.Warranty;

                    output.Add(motherboardDto);
                }
            }
            catch
            {
                // ignored
            }


            return output;
        }

        public async Task<List<MotherboardDTO>> GetAll()
        {
            var output = new List<MotherboardDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.MotherboardGetAll(new MotherboardGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var motherboardDto = new MotherboardDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            motherboardDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) motherboardDto.Description = current.Description;
                    if (current.HasWarranty) motherboardDto.Warranty = current.Warranty;

                    output.Add(motherboardDto);
                }
            }
            catch
            {
                // ignored
            }


            return output;
        }

        public async Task<bool> Save(MotherboardDTO motherboardDto)
        {
            var request = new MotherboardSaveRequest()
            {
                Id = motherboardDto.Id,
                Name = motherboardDto.Name,
                Price = motherboardDto.Price
            };
            if (motherboardDto.IssuedOn != null) request.IssuedOn = motherboardDto.IssuedOn.ToString();
            if (motherboardDto.Description != null) request.Description = motherboardDto.Description;
            if (motherboardDto.Warranty != null) request.Warranty = (float) motherboardDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.MotherboardSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int motherboardId)
        {
            var request = new MotherboardDeleteRequest {Id = motherboardId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.MotherboardDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
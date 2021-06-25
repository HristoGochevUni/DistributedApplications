using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class DrivesManagementService
    {
        private readonly Drive.DriveClient _client;
        private readonly string _accessToken;

        public DrivesManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new Drive.DriveClient(channel);
        }

        public async Task<DriveDTO> Get(int driveId)
        {
            var request = new DriveGetRequest {Id = driveId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            DriveGetResponse response;
            try
            {
                response = await _client.DriveGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;
            var output = new DriveDTO
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

        public async Task<List<DriveDTO>> GetAllByName(string name)
        {
            var output = new List<DriveDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new DriveGetAllByNameRequest() {Name = name};
                using var call = _client.DriveGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var driveDto = new DriveDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            driveDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) driveDto.Description = current.Description;
                    if (current.HasWarranty) driveDto.Warranty = current.Warranty;

                    output.Add(driveDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<DriveDTO>> GetAll()
        {
            var output = new List<DriveDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.DriveGetAll(new DriveGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var driveDto = new DriveDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price
                    };
                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            driveDto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) driveDto.Description = current.Description;
                    if (current.HasWarranty) driveDto.Warranty = current.Warranty;

                    output.Add(driveDto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(DriveDTO driveDto)
        {
            var request = new DriveSaveRequest()
            {
                Id = driveDto.Id,
                Name = driveDto.Name,
                Price = driveDto.Price
            };
            if (driveDto.IssuedOn != null) request.IssuedOn = driveDto.IssuedOn.ToString();
            if (driveDto.Description != null) request.Description = driveDto.Description;
            if (driveDto.Warranty != null) request.Warranty = (float) driveDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.DriveSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int driveId)
        {
            var request = new DriveDeleteRequest {Id = driveId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.DriveDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
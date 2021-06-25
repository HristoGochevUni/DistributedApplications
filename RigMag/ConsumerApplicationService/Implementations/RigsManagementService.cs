using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerApplicationService.DTOs;
using Grpc.Core;
using Grpc.Net.Client;
using RigMagAPI;

namespace ConsumerApplicationService.Implementations
{
    public class RigsManagementService
    {
        private readonly Rig.RigClient _client;
        private readonly string _accessToken;

        private readonly CoolersManagementService _coolersManagementService;
        private readonly CpusManagementService _cpusManagementService;
        private readonly DrivesManagementService _drivesManagementService;
        private readonly GpusManagementService _gpusManagementService;
        private readonly MotherboardsManagementService _motherboardsManagementService;
        private readonly PcCasesManagementService _pccasesManagementService;
        private readonly PsusManagementService _psusManagementService;
        private readonly RamsManagementService _ramsManagementService;

        public RigsManagementService(string apiAddress, string accessToken)
        {
            _accessToken = accessToken;
            var channel = GrpcChannel.ForAddress(apiAddress);
            _client = new Rig.RigClient(channel);
            _coolersManagementService = new CoolersManagementService(apiAddress, accessToken);
            _cpusManagementService = new CpusManagementService(apiAddress, accessToken);
            _drivesManagementService = new DrivesManagementService(apiAddress, accessToken);
            _gpusManagementService = new GpusManagementService(apiAddress, accessToken);
            _motherboardsManagementService = new MotherboardsManagementService(apiAddress, accessToken);
            _pccasesManagementService = new PcCasesManagementService(apiAddress, accessToken);
            _psusManagementService = new PsusManagementService(apiAddress, accessToken);
            _ramsManagementService = new RamsManagementService(apiAddress, accessToken);
        }

        public async Task<RigDTO> Get(int rigId)
        {
            var request = new RigGetRequest {Id = rigId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            RigGetResponse response;
            try
            {
                response = await _client.RigGetAsync(request, headers);
            }
            catch
            {
                return null;
            }

            if (response.Id < 1) return null;

            var output = new RigDTO
            {
                Id = response.Id,
                Name = response.Name,
                Price = response.Price,
                CoolerId = response.CoolerId,
                CpuId = response.CpuId,
                DriveId = response.DriveId,
                GpuId = response.GpuId,
                MotherboardId = response.MotherboardId,
                PcCaseId = response.PcCaseId,
                PsuId = response.PsuId,
                RamId = response.RamId
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

            if (!output.Validate()) return null;

            output.Cooler = await _coolersManagementService.Get(response.CoolerId);
            output.Cpu = await _cpusManagementService.Get(response.CpuId);
            output.Drive = await _drivesManagementService.Get(response.DriveId);
            output.Gpu = await _gpusManagementService.Get(response.GpuId);
            output.Motherboard = await _motherboardsManagementService.Get(response.MotherboardId);
            output.PcCase = await _pccasesManagementService.Get(response.PcCaseId);
            output.Psu = await _psusManagementService.Get(response.PsuId);
            output.Ram = await _ramsManagementService.Get(response.RamId);

            return output;
        }

        public async Task<List<RigDTO>> GetAllByName(string name)
        {
            var output = new List<RigDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var request = new RigGetAllByNameRequest() {Name = name};
                using var call = _client.RigGetAllByName(request, headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var dto = new RigDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price,
                        CoolerId = current.CoolerId,
                        CpuId = current.CpuId,
                        DriveId = current.DriveId,
                        GpuId = current.GpuId,
                        MotherboardId = current.MotherboardId,
                        PcCaseId = current.PcCaseId,
                        PsuId = current.PsuId,
                        RamId = current.RamId
                    };

                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            dto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) dto.Description = current.Description;
                    if (current.HasWarranty) dto.Warranty = current.Warranty;

                    if (!dto.Validate()) continue;

                    dto.Cooler = await _coolersManagementService.Get(current.CoolerId);
                    dto.Cpu = await _cpusManagementService.Get(current.CpuId);
                    dto.Drive = await _drivesManagementService.Get(current.DriveId);
                    dto.Gpu = await _gpusManagementService.Get(current.GpuId);
                    dto.Motherboard = await _motherboardsManagementService.Get(current.MotherboardId);
                    dto.PcCase = await _pccasesManagementService.Get(current.PcCaseId);
                    dto.Psu = await _psusManagementService.Get(current.PsuId);
                    dto.Ram = await _ramsManagementService.Get(current.RamId);

                    output.Add(dto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<List<RigDTO>> GetAll()
        {
            var output = new List<RigDTO>();
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                using var call = _client.RigGetAll(new RigGetAllRequest(), headers);
                while (await call.ResponseStream.MoveNext())
                {
                    var current = call.ResponseStream.Current;

                    var dto = new RigDTO
                    {
                        Id = current.Id,
                        Name = current.Name,
                        Price = current.Price,
                        CoolerId = current.CoolerId,
                        CpuId = current.CpuId,
                        DriveId = current.DriveId,
                        GpuId = current.GpuId,
                        MotherboardId = current.MotherboardId,
                        PcCaseId = current.PcCaseId,
                        PsuId = current.PsuId,
                        RamId = current.RamId
                    };

                    if (current.HasIssuedOn)
                    {
                        try
                        {
                            dto.IssuedOn = DateTime.Parse(current.IssuedOn);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    if (current.HasDescription) dto.Description = current.Description;
                    if (current.HasWarranty) dto.Warranty = current.Warranty;

                    if (!dto.Validate()) continue;

                    dto.Cooler = await _coolersManagementService.Get(current.CoolerId);
                    dto.Cpu = await _cpusManagementService.Get(current.CpuId);
                    dto.Drive = await _drivesManagementService.Get(current.DriveId);
                    dto.Gpu = await _gpusManagementService.Get(current.GpuId);
                    dto.Motherboard = await _motherboardsManagementService.Get(current.MotherboardId);
                    dto.PcCase = await _pccasesManagementService.Get(current.PcCaseId);
                    dto.Psu = await _psusManagementService.Get(current.PsuId);
                    dto.Ram = await _ramsManagementService.Get(current.RamId);

                    output.Add(dto);
                }
            }
            catch
            {
                // ignored
            }

            return output;
        }

        public async Task<bool> Save(RigDTO rigDto)
        {
            var request = new RigSaveRequest()
            {
                Id = rigDto.Id,
                Name = rigDto.Name,
                Price = rigDto.Price,
                CpuId = rigDto.CpuId,
                CoolerId = rigDto.CoolerId,
                DriveId = rigDto.DriveId,
                GpuId = rigDto.GpuId,
                MotherboardId = rigDto.MotherboardId,
                PcCaseId = rigDto.PcCaseId,
                PsuId = rigDto.PsuId,
                RamId = rigDto.RamId
            };
            if (rigDto.IssuedOn != null) request.IssuedOn = rigDto.IssuedOn.ToString();
            if (rigDto.Description != null) request.Description = rigDto.Description;
            if (rigDto.Warranty != null) request.Warranty = (float) rigDto.Warranty;
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.RigSaveAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int rigId)
        {
            var request = new RigDeleteRequest {Id = rigId};
            var headers = new Metadata {{"Authorization", $"Bearer {_accessToken}"}};
            try
            {
                var response = await _client.RigDeleteAsync(request, headers);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ConsumerApplicationService.DTOs;
using ConsumerApplicationService.Implementations;
using Microsoft.AspNetCore.Mvc;
using RigMag.ViewModels;

namespace RigMag.Controllers
{
    public class RigPartsController : Controller
    {
        private CoolersManagementService CoolersManagementService { get; }
        private CpusManagementService CpusManagementService { get; }
        private DrivesManagementService DrivesManagementService { get; }
        private GpusManagementService GpusManagementService { get; }
        private MotherboardsManagementService MotherboardsManagementService { get; }
        private PcCasesManagementService PcCasesManagementService { get; }
        private PsusManagementService PsusManagementService { get; }
        private RamsManagementService RamsManagementService { get; }

        public RigPartsController(string apiInfo)
        {
            var info = apiInfo.Split("$");
            var apiAddress = info[0];
            var apiAccessToken = info[1];
            CoolersManagementService = new CoolersManagementService(apiAddress, apiAccessToken);
            CpusManagementService = new CpusManagementService(apiAddress, apiAccessToken);
            DrivesManagementService = new DrivesManagementService(apiAddress, apiAccessToken);
            GpusManagementService = new GpusManagementService(apiAddress, apiAccessToken);
            MotherboardsManagementService = new MotherboardsManagementService(apiAddress, apiAccessToken);
            PcCasesManagementService = new PcCasesManagementService(apiAddress, apiAccessToken);
            PsusManagementService = new PsusManagementService(apiAddress, apiAccessToken);
            RamsManagementService = new RamsManagementService(apiAddress, apiAccessToken);
        }

        [HttpGet]
        public ActionResult Browse(string type, string name)
        {
            var parsed = Enum.TryParse(type, out RigPartType rigPartType);
            if (!parsed) return RedirectToAction("Index", "Home");
            if (string.IsNullOrEmpty(name)) name = "";
            ViewData["name"] = name;
            switch (rigPartType)
            {
                case RigPartType.Cooler:
                {
                    var outputParams = CoolersManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/cooler.png";
                    var output = new RigPartsVM(rigPartType, outputParams);

                    return View(output);
                }
                case RigPartType.CPU:
                {
                    var outputParams = CpusManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/cpu.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                case RigPartType.Drive:
                {
                    var outputParams = DrivesManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/drive.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                case RigPartType.GPU:
                {
                    var outputParams = GpusManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/gpu.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                case RigPartType.Motherboard:
                {
                    var outputParams = MotherboardsManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/motherboard.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                case RigPartType.PCCase:
                {
                    var outputParams = PcCasesManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/pccase.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                case RigPartType.PSU:
                {
                    var outputParams = PsusManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/psu.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                case RigPartType.RAM:
                {
                    var outputParams = RamsManagementService.GetAllByName(name).Result
                        .Select(dto => new RigPartVM(dto, rigPartType))
                        .ToList();
                    foreach (var vm in outputParams) vm.ImagePath = "/images/ram.png";
                    var output = new RigPartsVM(rigPartType, outputParams);
                    return View(output);
                }
                default:
                    return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult Details(string type, int id)
        {
            var parsed = Enum.TryParse(type, out RigPartType rigPartType);
            if (!parsed || id < 1) return RedirectToAction("Index", "Home");
            switch (rigPartType)
            {
                case RigPartType.Cooler:
                {
                    var dto = CoolersManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/cooler.png"
                    };
                    return View(output);
                }
                case RigPartType.CPU:
                {
                    var dto = CpusManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/cpu.png"
                    };
                    return View(output);
                }
                case RigPartType.Drive:
                {
                    var dto = DrivesManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/drive.png"
                    };
                    return View(output);
                }
                case RigPartType.GPU:
                {
                    var dto = GpusManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/gpu.png"
                    };
                    return View(output);
                }
                case RigPartType.Motherboard:
                {
                    var dto = MotherboardsManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/motherboard.png"
                    };
                    return View(output);
                }
                case RigPartType.PCCase:
                {
                    var dto = PcCasesManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/pccase.png"
                    };
                    return View(output);
                }
                case RigPartType.PSU:
                {
                    var dto = PsusManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/psu.png"
                    };
                    return View(output);
                }
                case RigPartType.RAM:
                {
                    var dto = RamsManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var output = new RigPartVM(dto, rigPartType)
                    {
                        ImagePath = "/images/ram.png"
                    };
                    return View(output);
                }
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Delete(string type, int id)
        {
            var parsed = Enum.TryParse(type, out RigPartType rigPartType);
            if (!parsed || id < 1) return RedirectToAction("Index", "Home");
            var result = rigPartType switch
            {
                RigPartType.Cooler => CoolersManagementService.Delete(id).Result,
                RigPartType.CPU => CpusManagementService.Delete(id).Result,
                RigPartType.Drive => DrivesManagementService.Delete(id).Result,
                RigPartType.GPU => GpusManagementService.Delete(id).Result,
                RigPartType.Motherboard => MotherboardsManagementService.Delete(id).Result,
                RigPartType.PCCase => PcCasesManagementService.Delete(id).Result,
                RigPartType.PSU => PsusManagementService.Delete(id).Result,
                RigPartType.RAM => RamsManagementService.Delete(id).Result,
                _ => false
            };
            return RedirectToAction("Browse", new {type = rigPartType});
        }

        [HttpGet]
        public ActionResult Update(string type, int id)
        {
            var parsed = Enum.TryParse(type, out RigPartType rigPartType);
            if (!parsed || id < 1) return RedirectToAction("Index", "Home");
            switch (rigPartType)
            {
                case RigPartType.Cooler:
                {
                    var dto = CoolersManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.CPU:
                {
                    var dto = CpusManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.Drive:
                {
                    var dto = DrivesManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.GPU:
                {
                    var dto = GpusManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.Motherboard:
                {
                    var dto = MotherboardsManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.PCCase:
                {
                    var dto = PcCasesManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.PSU:
                {
                    var dto = PsusManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                case RigPartType.RAM:
                {
                    var dto = RamsManagementService.Get(id).Result;
                    if (dto == null) return RedirectToAction("Browse", new {type = rigPartType});
                    var vm = new RigPartVM(dto, rigPartType);
                    return View(vm);
                }
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Update(RigPartVM vm)
        {
            if (vm == null || vm.Id < 1) return RedirectToAction("Index", "Home");
            var result = false;
            switch (vm.Type)
            {
                case RigPartType.Cooler:
                    var coolerDto = new CoolerDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = CoolersManagementService.Save(coolerDto).Result;
                    break;
                case RigPartType.CPU:
                    var cpuDto = new CPUDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = CpusManagementService.Save(cpuDto).Result;
                    break;
                case RigPartType.Drive:
                    var driveDto = new DriveDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = DrivesManagementService.Save(driveDto).Result;
                    break;
                case RigPartType.GPU:
                    var gpuDto = new GPUDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = GpusManagementService.Save(gpuDto).Result;
                    break;
                case RigPartType.Motherboard:
                    var motherboardDto = new MotherboardDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = MotherboardsManagementService.Save(motherboardDto).Result;
                    break;
                case RigPartType.PCCase:
                    var pcCaseDto = new PCCaseDTO()
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = PcCasesManagementService.Save(pcCaseDto).Result;
                    break;
                case RigPartType.PSU:
                    var psuDto = new PSUDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = PsusManagementService.Save(psuDto).Result;
                    break;
                case RigPartType.RAM:
                    var ramDto = new RAMDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = RamsManagementService.Save(ramDto).Result;
                    break;
                default:
                    break;
            }

            return result ? RedirectToAction("Details", new {vm.Type, vm.Id}) : RedirectToAction("Browse", vm.Type);
        }

        [HttpGet]
        public ActionResult Create(string type)
        {
            var parsed = Enum.TryParse(type, out RigPartType rigPartType);
            if (!parsed) return RedirectToAction("Index", "Home");
            var vm = new RigPartVM
            {
                Id = 0,
                Type = rigPartType
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(RigPartVM vm)
        {
            if (vm == null) return RedirectToAction("Index", "Home");
            var result = false;
            vm.Id = 0;
            switch (vm.Type)
            {
                case RigPartType.Cooler:
                    var coolerDto = new CoolerDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = CoolersManagementService.Save(coolerDto).Result;
                    break;
                case RigPartType.CPU:
                    var cpuDto = new CPUDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = CpusManagementService.Save(cpuDto).Result;
                    break;
                case RigPartType.Drive:
                    var driveDto = new DriveDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = DrivesManagementService.Save(driveDto).Result;
                    break;
                case RigPartType.GPU:
                    var gpuDto = new GPUDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = GpusManagementService.Save(gpuDto).Result;
                    break;
                case RigPartType.Motherboard:
                    var motherboardDto = new MotherboardDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = MotherboardsManagementService.Save(motherboardDto).Result;
                    break;
                case RigPartType.PCCase:
                    var pcCaseDto = new PCCaseDTO()
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = PcCasesManagementService.Save(pcCaseDto).Result;
                    break;
                case RigPartType.PSU:
                    var psuDto = new PSUDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = PsusManagementService.Save(psuDto).Result;
                    break;
                case RigPartType.RAM:
                    var ramDto = new RAMDTO
                    {
                        Id = vm.Id,
                        Name = vm.Name,
                        Price = vm.Price,
                        IssuedOn = vm.IssuedOn,
                        Warranty = vm.Warranty,
                        Description = vm.Description
                    };
                    result = RamsManagementService.Save(ramDto).Result;
                    break;
                default:
                    break;
            }


            return result ? RedirectToAction("Browse", new {vm.Type}) : RedirectToAction("Index", "Home");
        }
    }
}
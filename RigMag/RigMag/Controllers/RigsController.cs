using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ConsumerApplicationService.DTOs;
using ConsumerApplicationService.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RigMag.ViewModels;

namespace RigMag.Controllers
{
    public class RigsController : Controller
    {
        private CoolersManagementService CoolersManagementService { get; }
        private CpusManagementService CpusManagementService { get; }
        private DrivesManagementService DrivesManagementService { get; }
        private GpusManagementService GpusManagementService { get; }
        private MotherboardsManagementService MotherboardsManagementService { get; }
        private PcCasesManagementService PcCasesManagementService { get; }
        private PsusManagementService PsusManagementService { get; }
        private RamsManagementService RamsManagementService { get; }

        private RigsManagementService RigsManagementService { get; }

        public RigsController(string apiInfo)
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
            RigsManagementService = new RigsManagementService(apiAddress, apiAccessToken);
        }

        [HttpGet]
        public ActionResult Browse(string name)
        {
            if (string.IsNullOrEmpty(name)) name = "";
            ViewData["name"] = name;
            var outputParams = RigsManagementService.GetAllByName(name).Result
                .Select(dto => new RigVM(dto))
                .ToList();
            foreach (var vm in outputParams) vm.ImagePath = "/images/rig.png";
            return View(outputParams);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id < 1) return RedirectToAction("Browse");

            var dto = RigsManagementService.Get(id).Result;

            if (dto == null) return RedirectToAction("Browse");

            var output = new RigVM(dto)
            {
                ImagePath = "/images/rig.png"
            };
            return View(output);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            RigsManagementService.Delete(id);
            return RedirectToAction("Browse");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (id < 1) return RedirectToAction("Browse");

            var rigDto = RigsManagementService.Get(id).Result;


            if (rigDto == null) return RedirectToAction("Browse");


            var vm = new RigVM(rigDto);

            var coolers = CoolersManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.Cooler))
                .ToList();
            var cpus = CpusManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.CPU))
                .ToList();
            var drives = DrivesManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.Drive))
                .ToList();
            var gpus = GpusManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.GPU))
                .ToList();
            var motherboards = MotherboardsManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.Motherboard))
                .ToList();
            var pcCases = PcCasesManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.PCCase))
                .ToList();
            var psus = PsusManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.PSU))
                .ToList();
            var rams = RamsManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.RAM))
                .ToList();

            ViewBag.Coolers = new SelectList(coolers, "Id", "Name", vm.CoolerVm);
            ViewBag.Cpus = new SelectList(cpus, "Id", "Name", vm.CpuVm);
            ViewBag.Drives = new SelectList(drives, "Id", "Name", vm.DriveVm);
            ViewBag.Gpus = new SelectList(gpus, "Id", "Name", vm.GpuVm);
            ViewBag.Motherboards = new SelectList(motherboards, "Id", "Name", vm.MotherboardVm);
            ViewBag.PcCases = new SelectList(pcCases, "Id", "Name", vm.PcCaseVm);
            ViewBag.Psus = new SelectList(psus, "Id", "Name", vm.PsuVm);
            ViewBag.Rams = new SelectList(rams, "Id", "Name", vm.RamVm);


            return View(vm);
        }

        [HttpPost]
        public ActionResult Update(RigVM vm)
        {
            if (vm == null || vm.Id < 1) return RedirectToAction("Browse");

            var dto = new RigDTO
            {
                Id = vm.Id,
                Name = vm.Name,
                Price = vm.Price,
                IssuedOn = vm.IssuedOn,
                Warranty = vm.Warranty,
                Description = vm.Description,
                CoolerId = vm.CoolerId,
                CpuId = vm.CpuId,
                DriveId = vm.DriveId,
                GpuId = vm.GpuId,
                MotherboardId = vm.MotherboardId,
                PcCaseId = vm.PcCaseId,
                PsuId = vm.PsuId,
                RamId = vm.RamId,
            };

            var result = RigsManagementService.Save(dto).Result;
            return result ? RedirectToAction("Details", new {vm.Id}) : RedirectToAction("Browse");
        }

        public ActionResult Create()
        {
            var vm = new RigVM
            {
                Id = 0,
            };
            var coolers = CoolersManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.Cooler))
                .ToList();
            var cpus = CpusManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.CPU))
                .ToList();
            var drives = DrivesManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.Drive))
                .ToList();
            var gpus = GpusManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.GPU))
                .ToList();
            var motherboards = MotherboardsManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.Motherboard))
                .ToList();
            var pcCases = PcCasesManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.PCCase))
                .ToList();
            var psus = PsusManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.PSU))
                .ToList();
            var rams = RamsManagementService.GetAll().Result
                .Select(dto => new RigPartVM(dto, RigPartType.RAM))
                .ToList();

            ViewBag.Coolers = new SelectList(coolers, "Id", "Name");
            ViewBag.Cpus = new SelectList(cpus, "Id", "Name");
            ViewBag.Drives = new SelectList(drives, "Id", "Name");
            ViewBag.Gpus = new SelectList(gpus, "Id", "Name");
            ViewBag.Motherboards = new SelectList(motherboards, "Id", "Name");
            ViewBag.PcCases = new SelectList(pcCases, "Id", "Name");
            ViewBag.Psus = new SelectList(psus, "Id", "Name");
            ViewBag.Rams = new SelectList(rams, "Id", "Name");

            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(RigVM vm)
        {
            if (vm == null) return RedirectToAction("Browse");
            vm.Id = 0;
            var dto = new RigDTO
            {
                Id = vm.Id,
                Name = vm.Name,
                Price = vm.Price,
                IssuedOn = vm.IssuedOn,
                Warranty = vm.Warranty,
                Description = vm.Description,
                CoolerId = vm.CoolerId,
                CpuId = vm.CpuId,
                DriveId = vm.DriveId,
                GpuId = vm.GpuId,
                MotherboardId = vm.MotherboardId,
                PcCaseId = vm.PcCaseId,
                PsuId = vm.PsuId,
                RamId = vm.RamId,
            };

            var result = RigsManagementService.Save(dto).Result;
            Console.WriteLine(result);
            return result ? RedirectToAction("Details", new {vm.Id}) : RedirectToAction("Browse");
        }
    }
}
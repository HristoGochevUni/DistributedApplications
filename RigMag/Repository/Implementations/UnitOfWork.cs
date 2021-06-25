using System;
using Data.Context;
using Data.Entities;

namespace Repository.Implementations
{
    public class UnitOfWork : IDisposable
    {
        private readonly RigsSystemDbContext _context = new();
        private GenericRepository<Cooler> _coolersRepository;
        private GenericRepository<CPU> _cpusRepository;
        private GenericRepository<Drive> _drivesRepository;
        private GenericRepository<GPU> _gpusRepository;
        private GenericRepository<Motherboard> _motherboardsRepository;
        private GenericRepository<PCCase> _pccasesRepository;
        private GenericRepository<PSU> _psusRepository;
        private GenericRepository<RAM> _ramsRepository;
        private GenericRepository<Rig> _rigsRepository;


        public GenericRepository<Cooler> CoolersRepository =>
            _coolersRepository ??= new GenericRepository<Cooler>(_context);

        public GenericRepository<CPU> CpusRepository =>
            _cpusRepository ??= new GenericRepository<CPU>(_context);

        public GenericRepository<Drive> DrivesRepository =>
            _drivesRepository ??= new GenericRepository<Drive>(_context);

        public GenericRepository<GPU> GpusRepository =>
            _gpusRepository ??= new GenericRepository<GPU>(_context);

        public GenericRepository<Motherboard> MotherboardsRepository =>
            _motherboardsRepository ??= new GenericRepository<Motherboard>(_context);

        public GenericRepository<PCCase> PcCasesRepository =>
            _pccasesRepository ??= new GenericRepository<PCCase>(_context);

        public GenericRepository<PSU> PsusRepository =>
            _psusRepository ??= new GenericRepository<PSU>(_context);

        public GenericRepository<RAM> RamsRepository =>
            _ramsRepository ??= new GenericRepository<RAM>(_context);

        public GenericRepository<Rig> RigsRepository =>
            _rigsRepository ??= new GenericRepository<Rig>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
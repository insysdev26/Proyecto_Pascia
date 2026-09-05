using Microsoft.EntityFrameworkCore;
using Pegasus.Core.Entities;
using Pegasus.Core.Interfaces;
using Pegasus.Infrastructure.Data;

namespace Pegasus.Infrastructure.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ApplicationDbContext _context;

        public VehiculoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vehiculo>> GetAllAsync() =>
            await _context.Vehiculos.AsNoTracking().ToListAsync();

        public async Task<Vehiculo?> GetByIdAsync(int id) =>
            await _context.Vehiculos.FindAsync(id);

        public async Task AddAsync(Vehiculo vehiculo)
        {
            await _context.Vehiculos.AddAsync(vehiculo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehiculo vehiculo)
        {
            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
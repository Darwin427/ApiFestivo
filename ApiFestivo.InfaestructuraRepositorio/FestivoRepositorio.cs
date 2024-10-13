using ApiFestivo.Core.Interfaces;
using ApiFestivo.Dominio;
using ApiFestivo.InfraestructuraPersistencia.Contexto;
using Microsoft.EntityFrameworkCore;

namespace ApiFestivo.InfaestructuraRepositorio
{
    public class FestivoRepositorio : IFestivoRepositorio
    {
        private readonly APIFestivosContext context;
        public FestivoRepositorio(APIFestivosContext context)
        {
            this.context = context;
        }

        public async Task<Festivo> ActualizarFestivo(Festivo fechaFestivo)
        {
            var FestivoExistente = await context.Festivos.FindAsync(fechaFestivo.id);
            if (FestivoExistente == null)
            {
                return null;
            }
            context.Entry(FestivoExistente).CurrentValues.SetValues(fechaFestivo);
            await context.SaveChangesAsync();
            return await context.Festivos.FindAsync(fechaFestivo.id);
        }

        public async Task<Festivo> AgregarFestivo(Festivo fechaFestivo)
        {
            context.Festivos.Add(fechaFestivo);
            await context.SaveChangesAsync();
            return fechaFestivo;
        }

        public async Task<bool> EliminarFestivo(Festivo fechaFestivo)
        {
            var FestivoExistente = await context.Festivos.FindAsync(fechaFestivo.id);
            if (FestivoExistente == null)
            {
                return false;
            }
            try
            {
                context.Festivos.Remove(FestivoExistente);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> EsFestivo(Festivo fechaFestivo)
        {
            var festivoExistente = await context.Festivos
        .FirstOrDefaultAsync(f => f.Dia == fechaFestivo.Dia && f.Mes == fechaFestivo.Mes);
            return festivoExistente != null;
        }

        public async Task<Festivo> Obtener(int Id)
        {
            return await context.Festivos.FindAsync(Id);
        }

        public async Task<IEnumerable<Festivo>> ObtenerTodosLosFestivos()
        {
            return await context.Festivos.ToArrayAsync();
        }
    }
}

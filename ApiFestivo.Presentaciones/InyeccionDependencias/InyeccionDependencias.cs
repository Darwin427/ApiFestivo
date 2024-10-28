using Microsoft.EntityFrameworkCore;
using ApiFestivo.InfraestructuraPersistencia.Contexto;
using ApiFestivo.Core.Interfaces;
using ApiFestivo.InfaestructuraRepositorio;
using ApiFestivo.Core.Servicios;
using ApiFestivo.Aplicacion;

namespace ApiFestivo.Presentaciones.InyeccionDependencias
{
    static class InyeccionDependecias
    {
        public static IServiceCollection AgregarDependecias(this IServiceCollection servicios, IConfiguration configuration)
        {
            servicios.AddDbContext<APIFestivosContext>(Opciones =>
            {
                Opciones.UseSqlServer(configuration.GetConnectionString("Festivos"));
                Opciones.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            //Agregar los repositorios
            servicios.AddTransient<IFestivoRepositorio, FestivoRepositorio>();

            //Agregar los servicios
            servicios.AddTransient<IFestivoServicio, FestivoServicio>();


            return servicios;
        }
    }
}

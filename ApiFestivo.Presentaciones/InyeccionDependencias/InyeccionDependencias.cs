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
            //Agregar el DbContext
            servicios.AddDbContext<APIFestivosContext>(Opciones =>
            {
                Opciones.UseSqlServer(configuration.GetConnectionString("Festivos"));
            });

            //Agregar los repositorios
            servicios.AddTransient<IFestivoRepositorio, FestivoRepositorio>();

            //Agregar los servicios
            servicios.AddTransient<IFestivoServicio, FestivoServicio>();


            return servicios;
        }
    }
}

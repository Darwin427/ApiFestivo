using ApiFestivo.Core.Servicios;
using ApiFestivo.Dominio;

namespace ApiFestivo.Aplicacion
{
    public class FestivoServicio : IFestivoServicio
    {

        private readonly IFestivoServicio repositorio;

        public FestivoServicio(IFestivoServicio repositorio)
        {
            this.repositorio = repositorio;
        }


        public Task<IEnumerable<DateTime>> ObtenerTodosLosFestivos()
        {
            return repositorio.ObtenerTodosLosFestivos();
        }

        public Task<DateTime> AgregarFestivo(DateTime fechaFestivo)
        {
            return repositorio.AgregarFestivo(fechaFestivo);
        }

        public Task<DateTime> ActualizarFestivo(DateTime fechaFestivo)
        {
            return repositorio.ActualizarFestivo(fechaFestivo);
        }

        public Task<Festivo> Obtener(int Id)
        {
            return repositorio.Obtener(Id);
        }

        public Task<IEnumerable<Festivo>> EsFestivo(int indiceDato, DateTime fecha)
        {
            return repositorio.EsFestivo(indiceDato, fecha);
        }

        public Task<bool> EliminarFestivo(DateTime fechaFestivo)
        {
            return repositorio.EliminarFestivo(fechaFestivo);
        }
    }
}

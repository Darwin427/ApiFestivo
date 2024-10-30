using ApiFestivo.Core.Interfaces;
using ApiFestivo.Core.Servicios;

namespace ApiFestivo.Aplicacion
{
    public class FestivoServicio : IFestivoServicio
    {

        private readonly IFestivoRepositorio repositorio;

        public FestivoServicio(IFestivoRepositorio repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<bool> EsFestivo(DateTime fecha)
        {
            return repositorio.EsFestivo(fecha);
        }

        public Task<DateTime?> FestivoRelativo(DateTime fecha)
        {
            return repositorio.FestivoRelativo(fecha);
        }

        public Task<IEnumerable<DateTime>> FestivosFijos()
        {
            return repositorio.FestivosFijos();
        }

        public Task<DateTime?> FestivosPuente(DateTime fecha)
        {
            return repositorio.FestivosPuente(fecha);
        }

        public Task<DateTime?> FestivosPuenteRelativos(DateTime fecha)
        {
            return repositorio.FestivosPuenteRelativos(fecha);
        }
    }
}

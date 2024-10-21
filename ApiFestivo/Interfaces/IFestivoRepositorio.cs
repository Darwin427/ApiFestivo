using ApiFestivo.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFestivo.Core.Interfaces
{
    public interface IFestivoRepositorio
    {
        Task<bool> EsFestivo(DateTime fecha);

        Task<DateTime?> FestivoRelativo(DateTime fecha);

        Task<IEnumerable<DateTime>> FestivosFijos();

        Task<DateTime?> FestivosPuente(DateTime fecha);

        Task<DateTime?> FestivosPuenteRelativos(DateTime fecha);
    }
}

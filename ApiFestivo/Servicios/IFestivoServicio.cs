using ApiFestivo.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiFestivo.Core.Servicios
{
    public interface IFestivoServicio
    {
        Task<IEnumerable<DateTime>> ObtenerTodosLosFestivos();

        Task<Festivo> Obtener(int Id);

        Task<IEnumerable<Festivo>> EsFestivo(int indiceDato, DateTime fecha);

        Task<DateTime> AgregarFestivo(DateTime fechaFestivo, int Dias);

        Task<bool> EliminarFestivo(DateTime fechaFestivo);

        Task<DateTime> ActualizarFestivo(DateTime fechaFestivo);
    }
}

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

        //Logica de los festivos

        //Logica para obtener el inico de la semana santa
        public static DateTime ObtenerInicioSemanaSanta(int año)
        {
            int a = año % 19;
            int b = año % 4;
            int c = año % 7;
            int d = (19 * a + 24) % 30;

            int días = d + (2 * b + 4 * c + 6 * d + 5) % 7;

            int dia = 15 + días;
            int mes = 3;
            if (dia > 31)
            {
                dia = dia - 31;
                mes = 4;
            }
            return new DateTime(año, mes, dia);
        }




        public Task<IEnumerable<DateTime>> ObtenerTodosLosFestivos()
        {
            return repositorio.ObtenerTodosLosFestivos();
        }


        //Se le hizo un remiendo, pendiente de si funciona, pero asi funciona 
        public Task<DateTime> AgregarFestivo(DateTime fechaFestivo, int dias)
        {
            DateTime nuevaFechaFestivo = AgregarDias(fechaFestivo, dias);
            return Task.FromResult(nuevaFechaFestivo);
        }

        public static DateTime AgregarDias(DateTime fecha, int dias)
        {
            return fecha.AddDays(dias);
        }
        //Hasta aqui 


        public Task<DateTime> ActualizarFestivo(DateTime fechaFestivo)
        {
            return repositorio.ActualizarFestivo(fechaFestivo);
        }

        public Task<Festivo> Obtener(int Id)
        {
            return repositorio.Obtener(Id);
        }


        //Beta de EsFestivo, para que se aproxime al siguiente lunes si la fecha coincide con el festivo
        public Task<IEnumerable<Festivo>> EsFestivo(int indiceDato, DateTime fecha)
        {
            DateTime siguienteLunesFecha = siguienteLunes(fecha);
            return repositorio.EsFestivo(indiceDato, siguienteLunesFecha);
        }
        public static DateTime siguienteLunes(DateTime fecha)
        {
            DayOfWeek diaSemana = fecha.DayOfWeek;
            if (diaSemana != DayOfWeek.Monday)
            {
                if (diaSemana > DayOfWeek.Sunday)
                {
                    fecha = AgregarDias(fecha, 8 - (int)diaSemana);
                }
            }
            else
            {
                fecha = AgregarDias(fecha, 1);
            }
            return fecha;
        }
        //Hasta aqui

        public Task<bool> EliminarFestivo(DateTime fechaFestivo)
        {
            return repositorio.EliminarFestivo(fechaFestivo);
        }
    }
}

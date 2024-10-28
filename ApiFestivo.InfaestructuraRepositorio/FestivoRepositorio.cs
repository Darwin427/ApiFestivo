using ApiFestivo.Core.Interfaces;
using ApiFestivo.Dominio;
using ApiFestivo.InfraestructuraPersistencia.Contexto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ApiFestivo.InfaestructuraRepositorio
{
    public class FestivoRepositorio : IFestivoRepositorio
    {

        private readonly APIFestivosContext dbConnector;

        public FestivoRepositorio(APIFestivosContext context)
        {
            this.dbConnector = context;
        }
        public async Task<bool> EsFestivo(DateTime fecha)
        {
            if (fecha < DateTime.Now) return false; // Fecha inválida

            DateTime domingoPascua = ObtenerDomingoPascua(fecha.Year);

            // Verificar festivos fijos
            if (await EsFestivoFijo(fecha)) return true;

            // Verificar festivos puente
            if (await EsFestivoPuente(fecha)) return true;

            // Verificar festivos relativos
            if (await EsFestivoRelativo(fecha, domingoPascua)) return true;

            // Verificar festivos puente relativos
            if (await EsFestivoPuenteRelativo(fecha, domingoPascua)) return true;


            else
            {
                return false;
            }
        }

        public async Task<DateTime?> FestivoRelativo(DateTime fecha)
        {
            DateTime domingoPascua = ObtenerDomingoPascua(fecha.Year);

            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT DiasPascua FROM Festivo WHERE IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'relativo')", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int diasRelativos = reader.GetInt32(0);
                    DateTime fechaFestivo = domingoPascua.AddDays(diasRelativos);
                    if (fecha.Date == fechaFestivo.Date)
                    {
                        return fechaFestivo;
                    }
                }
            }
            return null;
        }

        public async Task<IEnumerable<DateTime>> FestivosFijos()
        {
            List<DateTime> festivos = new List<DateTime>();
            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT Dia, Mes FROM Festivo WHERE IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'fijo')", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int dia = reader.GetInt32(0);
                    int mes = reader.GetInt32(1);
                    festivos.Add(new DateTime(DateTime.Now.Year, mes, dia));
                }
            }
            return festivos;
        }

        public async Task<DateTime?> FestivosPuente(DateTime fecha)
        {
            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT Dia, Mes FROM Festivo WHERE IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'puente')", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int dia = reader.GetInt32(0);
                    int mes = reader.GetInt32(1);
                    DateTime fechaFestivo = new DateTime(fecha.Year, mes, dia);
                    if (fechaFestivo.DayOfWeek != DayOfWeek.Monday)
                    {
                        fechaFestivo = SiguienteLunes(fechaFestivo);
                    }
                    if (fecha.Date == fechaFestivo.Date)
                    {
                        return fechaFestivo;
                    }
                }
            }
            return null;
        }

        public async Task<DateTime?> FestivosPuenteRelativos(DateTime fecha)
        {
            DateTime domingoPascua = ObtenerDomingoPascua(fecha.Year);

            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT DiasPascua FROM Festivo WHERE IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'puente_relativo')", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int diasRelativos = reader.GetInt32(0);
                    DateTime fechaFestivo = domingoPascua.AddDays(diasRelativos);
                    if (fechaFestivo.DayOfWeek != DayOfWeek.Monday)
                    {
                        fechaFestivo = SiguienteLunes(fechaFestivo);
                    }
                    if (fecha.Date == fechaFestivo.Date)
                    {
                        return fechaFestivo;
                    }
                }
            }
            return null;
        }

        private DateTime ObtenerDomingoPascua(int año)
        {
            // Lógica para calcular el domingo de Pascua
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

        private DateTime SiguienteLunes(DateTime fecha)
        {
            while (fecha.DayOfWeek != DayOfWeek.Monday)
            {
                fecha = fecha.AddDays(1);
            }
            return fecha;
        }

        private async Task<bool> EsFestivoFijo(DateTime fecha)
        {
            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Festivo WHERE Dia = @dia AND Mes = @mes AND IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'fijo')", conn);
                cmd.Parameters.AddWithValue("@dia", fecha.Day);
                cmd.Parameters.AddWithValue("@mes", fecha.Month);
                int count = (int)await cmd.ExecuteScalarAsync();
                return count > 0;
            }
        }

        private async Task<bool> EsFestivoPuente(DateTime fecha)
        {
            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Festivo WHERE Dia = @Dia AND Mes = @Mes AND IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'puente')", conn);
                cmd.Parameters.AddWithValue("@Dia", fecha.Day);
                cmd.Parameters.AddWithValue("@Mes", fecha.Month);
                int count = (int)await cmd.ExecuteScalarAsync();
                return count > 0;
            }
        }

        private async Task<bool> EsFestivoRelativo(DateTime fecha, DateTime domingoPascua)
        {
            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT DiasPascua FROM Festivo WHERE IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'relativo')", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int diasRelativos = reader.GetInt32(0);
                    DateTime fechaFestivo = domingoPascua.AddDays(diasRelativos);
                    if (fecha.Date == fechaFestivo.Date) return true;
                }
            }
            return false;
        }

        private async Task<bool> EsFestivoPuenteRelativo(DateTime fecha, DateTime domingoPascua)
        {
            using (SqlConnection conn = dbConnector.GetConnection())
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT DiasPascua FROM Festivo WHERE IdTipo = (SELECT Id FROM Tipo WHERE Tipo = 'puente_relativo')", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int diasRelativos = reader.GetInt32(0);
                    DateTime fechaFestivo = domingoPascua.AddDays(diasRelativos);
                    if (fechaFestivo.DayOfWeek != DayOfWeek.Monday)
                    {
                        fechaFestivo = SiguienteLunes(fechaFestivo);
                    }
                    if (fecha.Date == fechaFestivo.Date) return true;
                }
            }
            return false;
        }
    }
}

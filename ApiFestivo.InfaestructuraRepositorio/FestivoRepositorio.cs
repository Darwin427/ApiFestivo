using ApiFestivo.Core.Interfaces;
using ApiFestivo.InfraestructuraPersistencia.Contexto;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


public class FestivoRepositorio : IFestivoRepositorio
{
    private readonly APIFestivosContext _context;

    public FestivoRepositorio(APIFestivosContext context)
    {
        _context = context;
    }

    public async Task<bool> EsFestivo(DateTime fecha)
    {
        // Fijo
        var fijo = await _context.Festivos
            .AnyAsync(f => f.IdTipo == 1 && f.Dia == fecha.Day && f.Mes == fecha.Month);

        if (fijo) return true;

        // Trasladado al lunes
        var trasladado = await FestivosPuente(fecha);
        if (trasladado.HasValue && trasladado.Value.Date == fecha.Date) return true;

        // Relativo al domingo de Pascua
        var relativo = await FestivoRelativo(fecha);
        if (relativo.HasValue && relativo.Value.Date == fecha.Date) return true;

        // Relativo trasladado al lunes
        var relativoPuente = await FestivosPuenteRelativos(fecha);
        if (relativoPuente.HasValue && relativoPuente.Value.Date == fecha.Date) return true;

        return false;
    }

    public async Task<DateTime?> FestivoRelativo(DateTime fecha)
    {
        var domingoPascua = ObtenerDomingoDePascua(fecha.Year);

        var festivo = await _context.Festivos
            .Where(f => f.IdTipo == 3)
            .Select(f => domingoPascua.AddDays(f.DiasPascua))
            .FirstOrDefaultAsync();

        return festivo;
    }

    public async Task<IEnumerable<DateTime>> FestivosFijos()
    {
        return await _context.Festivos
            .Where(f => f.IdTipo == 1)
            .Select(f => new DateTime(DateTime.Now.Year, f.Mes, f.Dia))
            .ToListAsync();
    }

    public async Task<DateTime?> FestivosPuente(DateTime fecha)
    {
        var festivo = await _context.Festivos
            .Where(f => f.IdTipo == 2 && f.Mes == fecha.Month && f.Dia == fecha.Day)
            .Select(f => new DateTime(fecha.Year, f.Mes, f.Dia))
            .SingleOrDefaultAsync();

        return festivo == default(DateTime) ? null : MoverAlLunes(festivo);
    }


    public async Task<DateTime?> FestivosPuenteRelativos(DateTime fecha)
    {
        var relativo = await FestivoRelativo(fecha);

        return relativo.HasValue ? MoverAlLunes(relativo.Value) : null;
    }

    private DateTime ObtenerDomingoDePascua(int year)
    {
        // Algoritmo de Meeus para calcular Pascua
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateTime(year, month, day);
    }

    private DateTime MoverAlLunes(DateTime fecha)
    {
        return fecha.DayOfWeek == DayOfWeek.Monday ? fecha : fecha.AddDays((int)DayOfWeek.Monday - (int)fecha.DayOfWeek);
    }
}
namespace Blazelounge_v2.Data.Models;

public partial class GamesStat
{
    public string Game { get; set; } = null!;

    public long TimesPlayed { get; set; }

    public long Wins { get; set; }

    public long Loses { get; set; }

    public double CurrencyGained { get; set; }

    public long IdGameStat { get; set; }

    public long FkStatisticsidStatistics { get; set; }

}

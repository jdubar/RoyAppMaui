using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface IDataService
{
    byte[] GetExportData(IEnumerable<Sleep> sleeps);
}

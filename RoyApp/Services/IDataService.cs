using RoyApp.Models;

namespace RoyApp.Services;
public interface IDataService
{
    byte[] GetExportData(IEnumerable<Sleep> sleeps);
}

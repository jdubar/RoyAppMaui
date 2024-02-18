using RoyAppMaui.Models;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Interfaces;
public interface IDataService
{
    decimal GetAverageOfBedtimes(ObservableCollection<Sleep> sleeps);
    decimal GetAverageOfWaketimes(ObservableCollection<Sleep> sleeps);
    string GetExportData(ObservableCollection<Sleep> sleeps);
}

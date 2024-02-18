using RoyAppMaui.Models;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Interfaces;
public interface IDataService
{
    decimal GetAverageOfBedtime(ObservableCollection<Sleep> sleeps);
    decimal GetAverageOfWaketime(ObservableCollection<Sleep> sleeps);
    string GetExportData(ObservableCollection<Sleep> sleeps);
}

using RoyAppMaui.Models;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Interfaces;
public interface IDataService
{
    string GetListDataAsString(ObservableCollection<Sleep> sleeps);
}

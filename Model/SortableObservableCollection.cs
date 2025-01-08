using System.Collections.ObjectModel;

namespace FWAPPA.Model;

public class SortableObservableCollection<T> : ObservableCollection<T>
{
    public void Sort(Func<T, object> keySelector)
    {
        var sorted = this.OrderBy(keySelector).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(this[i], sorted[i]))
            {
                this.Move(this.IndexOf(sorted[i]), i);
            }
        }
    }
}
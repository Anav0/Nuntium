namespace Nuntium.Core
{
    /// <summary>
    /// Class crudely representing sorting option used in combobox
    /// </summary>
    public class SortingOption
    {
        public int Id { get; set; }

        public string Option { get; set; }

        public SortedBy SortedBy { get; set; }
    }
}

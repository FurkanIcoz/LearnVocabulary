using LearnVocabulary.Models;

namespace LearnVocabulary.ViewModels
{
    public class WordWithTurkishSorted
    {
        public List<UnknownWord> OriginalList { get; set; }
        public List<UnknownWord> SortedByLevelAscending { get; set; }
        public List<UnknownWord> SortedByLevelDescending { get; set; }
        public List<UnknownWord> SortedByDateAscending { get; set; }
        public List<UnknownWord> SortedByDateDescending { get; set; }
    }
}

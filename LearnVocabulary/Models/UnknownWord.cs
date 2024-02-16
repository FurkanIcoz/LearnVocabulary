using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnVocabulary.Models
{
    public class UnknownWord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EnglistText { get; set; }
        public string TurkishText { get; set; }
        public int Level { get; set; }
        public List<WordsSentence> WordsSentences { get; set; }
        public DateTime WordDate { get; set; }
    }
}

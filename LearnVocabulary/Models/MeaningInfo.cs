using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnVocabulary.Models
{
    public class MeaningInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int WordId { get; set; } // Foreign Key
        public Word Word { get; set; } // Eklenen satır

        public string PartOfSpeech { get; set; }
        public List<DefinitionInfo> Definitions { get; set; }
    }
}

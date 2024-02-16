using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnVocabulary.Models
{
    public class WordsSentence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string Sentence { get; set; }
        public int UnknownWordId { get; set; }
        public UnknownWord UnknownWord { get; set; }

    }
}

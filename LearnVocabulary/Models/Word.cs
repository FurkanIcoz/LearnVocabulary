using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnVocabulary.Models
{
    public class Word
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string WordText { get; set; }
        public string Phonetic { get; set; }
        public List<PhoneticInfo> Phonetics { get; set; }
        public List<MeaningInfo> Meanings { get; set; }
        public LicanseInfo License { get; set; }

        public List<string> SourceUrls { get; set; }
    }

  
   
}

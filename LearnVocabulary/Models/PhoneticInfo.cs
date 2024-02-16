using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnVocabulary.Models
{
    public class PhoneticInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int WordId { get; set; } // Foreign Key
        public Word Word { get; set; } // Eklenen satır

        public string Text { get; set; }
        public string Audio { get; set; }
    }



}

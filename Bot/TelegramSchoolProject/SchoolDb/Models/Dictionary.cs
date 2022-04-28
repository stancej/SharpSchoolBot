using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TelegramSchoolProject.SchoolDb.Models
{
    public partial class Dictionary
    {
        [Key]
        public long Id { get; set; }

        [StringLength(20)]
        public string Paragraph { get; set; }

        [StringLength(2000)]
        public string WordsEn { get; set; }

        [StringLength(2000)]
        public string WordsRu { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TelegramSchoolProject.SchoolDb.Models
{
    public partial class User
    {

        [Key]
        public long Id { get; set; }

        public long ChatId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Stage { get; set; }

        [StringLength(4000)]
        public string r_WordsEn { get; set; }

        [StringLength(4000)]
        public string r_WordsRu { get; set; }

        [StringLength(100)]
        public string c_WordEn { get; set; }
    
        [StringLength(100)]
        public string c_WordRu { get; set; }
    }
}

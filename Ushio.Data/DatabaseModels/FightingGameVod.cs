using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ushio.Data.DatabaseModels
{
    public class FightingGameVod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OriginalTitle { get; set; }
        public string VideoId { get; set; }
        public FightingGameName GameName { get; set; }
        public string Source { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string CharacterP1 { get; set; }
        public string CharacterP2 { get; set; }
        public DateTimeOffset DateUploaded { get; set; }
        public DateTimeOffset DateAddedToRepo { get; set; }
    }
}

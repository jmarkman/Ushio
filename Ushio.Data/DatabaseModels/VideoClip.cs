using System;
using System.ComponentModel.DataAnnotations;

namespace Ushio.Data.DatabaseModels
{
    public class VideoClip
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}

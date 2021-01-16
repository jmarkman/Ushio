using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ushio.Data;

namespace Ushio.Infrastructure.Database.Repositories
{
    public class VideoClipRepository : BaseRepository<VideoClip>
    {
        public VideoClipRepository(UshioDbContext context) : base(context)
        {

        }
    }
}

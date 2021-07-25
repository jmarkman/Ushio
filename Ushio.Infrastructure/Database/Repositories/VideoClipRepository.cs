using Ushio.Data.DatabaseModels;

namespace Ushio.Infrastructure.Database.Repositories
{
    public class VideoClipRepository : BaseRepository<VideoClip>
    {
        public VideoClipRepository(UshioDbContext context) : base(context)
        {

        }
    }
}

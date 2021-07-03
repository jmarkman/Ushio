using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ushio.Data.DatabaseModels;

namespace Ushio.Infrastructure.Database.Repositories
{
    public class FightingGameVodRepository : BaseRepository<FightingGameVod>
    {
        public FightingGameVodRepository(UshioDbContext context): base(context)
        {

        }
    }
}

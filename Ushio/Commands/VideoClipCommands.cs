using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ushio.Infrastructure.Database.Repositories;

namespace Ushio.Commands
{
    [Name("VideoClips")]
    public class VideoClipCommands : ModuleBase<SocketCommandContext>
    {
        private readonly VideoClipRepository _clipRepository;

        public VideoClipCommands(VideoClipRepository repo)
        {
            _clipRepository = repo;
        }

        [Command("testclip")]
        public async Task GetTestClip()
        {
            var clipData = await _clipRepository.Get(1);

            await ReplyAsync($"{clipData.Title} -- {clipData.Link}");
        }

        [Command("clip")]
        public async Task GetClipByTitle(string clipTitle)
        {
            await ReplyAsync($"Clip by title: '{clipTitle}'");
        }

        [Command("clip")]
        public async Task GetClipById(int clipId)
        {
            await ReplyAsync($"Clip by ID: '{clipId}'");
        }

        [Command("addclip")]
        public async Task AddNewClip(string link, string title)
        {
            throw new NotImplementedException();
        }
    }
}

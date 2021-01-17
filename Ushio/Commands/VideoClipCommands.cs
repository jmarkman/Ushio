using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ushio.Data;
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

        /// <summary>
        /// Locates a clip with a matching title and replies to the source channel
        /// with the clip URL and its title, leveraging Discord's auto-embed for
        /// third-party media links like YouTube, Twitch, etc.
        /// </summary>
        /// <param name="clipTitle">The title associated with the clip when it was first added</param>
        [Command("clip")]
        public async Task GetClipByMatchingTitle([Remainder]string clipTitle)
        {
            var requestedClip = (await _clipRepository.FindAsync(c => c.Title.ToLower() == $"{clipTitle.ToLower()}")).FirstOrDefault();

            if (requestedClip != null)
            {
                await ReplyAsync($"{requestedClip.Link} -- {requestedClip.Title}");
            }
            else
            {
                await ReplyAsync($"Could not find video clip '{clipTitle}'. The '!clip' command performs exact title searches.");
            }
        }

        /// <summary>
        /// Locates a clip by its numeric ID and replies to the source channel
        /// with the clip URL and its title, leveraging Discord's auto-embed for
        /// third-party media links like YouTube, Twitch, etc.
        /// </summary>
        /// <param name="clipId">The numeric ID associated with the clip</param>
        [Command("clip")]
        public async Task GetClipById(int clipId)
        {
            var requestedClip = (await _clipRepository.FindAsync(c => c.Id == clipId)).FirstOrDefault();

            if (requestedClip != null)
            {
                await ReplyAsync($"{requestedClip.Link} -- {requestedClip.Title}");
            }
            else
            {
                await ReplyAsync($"Could not find video clip ID: '{clipId}'.");
            }
        }

        /// <summary>
        /// Adds a new clip to the database
        /// </summary>
        /// <param name="link">The hyperlink to the video</param>
        /// <param name="title"></param>
        /// <returns>If successfully added, the clip's data as it exists in the database,
        /// which is sent as a message to the source channel</returns>
        [Command("addclip")]
        public async Task AddNewClip(string link, [Remainder]string title)
        {
            var possibleMatchingClips = await _clipRepository.FindAsync(c => c.Link.ToLower() == link.ToLower());

            if (possibleMatchingClips.Any())
            {
                await ReplyAsync($"A clip with the specified hyperlink already exists, clip ID: {possibleMatchingClips.FirstOrDefault().Id}");
                return;
            }

            VideoClip newClip = new VideoClip()
            {
                Title = title,
                Link = link,
                AddedBy = Context.Message.Author.Username,
                AddedOn = DateTime.Now
            };

            var addedClip = await _clipRepository.AddAsync(newClip);
            await _clipRepository.SaveChangesAsync();

            if (addedClip != null)
            {
                await ReplyAsync($"{addedClip.AddedBy} added clip '{addedClip.Title}', ID: '{addedClip.Id}'");
            }
            else
            {
                await ReplyAsync($"Failed to add clip!");
            }
        }
    }
}

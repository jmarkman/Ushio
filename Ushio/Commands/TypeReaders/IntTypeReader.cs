using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Ushio.Commands.TypeReaders
{
    public class IntTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (int.TryParse(input, out int result))
            {
                return Task.FromResult(TypeReaderResult.FromSuccess(result));
            }

            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as an integer"));
        }
    }
}

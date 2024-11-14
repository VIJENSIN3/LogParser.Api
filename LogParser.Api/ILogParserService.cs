using LogParser.Api.Models;

namespace LogParser.Api
{
    public interface ILogParserService
    {
        IEnumerable<LogEntry> ParseLogEntries(string[] logLines);
        IEnumerable<HostAccessCount> GetHostAccessCounts(IEnumerable<LogEntry> entries);
        IEnumerable<ResourceAccessCount> GetSuccessfulResourceAccessCounts(IEnumerable<LogEntry> entries);
    }
}
using LogParser.Api.Models;

namespace LogParser.Api
{
    // Services/LogParserService.cs
    public class LogParserService : ILogParserService
    {
        public IEnumerable<LogEntry> ParseLogEntries(string[] logLines)
        {
            foreach (var line in logLines)
            {
                var columns = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Ensure we have enough columns to parse, with at least 6 items (0-5 for StatusCode access)
                if (columns.Length < 6) continue;

                yield return new LogEntry
                {
                    Host = columns[0],
                    Timestamp = columns[1],
                    Request = string.Join(" ", columns.Skip(2).Take(3)), // Capture the full request

                    // Safely parse StatusCode and ResponseSize if they exist
                    StatusCode = (int)(int.TryParse(columns.ElementAtOrDefault(5), out var statusCode) ? statusCode : (int?)null),
                    ResponseSize = int.TryParse(columns.ElementAtOrDefault(6), out var size) ? size : (int?)null
                };
            }
        }


        public IEnumerable<HostAccessCount> GetHostAccessCounts(IEnumerable<LogEntry> entries)
        {
            return entries
                .Where(e => !string.IsNullOrEmpty(e.Host)) // Ensure the host is not null or empty
                .GroupBy(e => e.Host) // Group by Host
                .Select(g => new HostAccessCount { Host = g.Key, AccessCount = g.Count() })
                .OrderByDescending(h => h.AccessCount);
        }

        public IEnumerable<ResourceAccessCount> GetSuccessfulResourceAccessCounts(IEnumerable<LogEntry> entries)
        {
            return entries
                .Where(e => e.StatusCode == 200 && e.Request.Contains("GET"))
                .GroupBy(e => GetUriFromRequest(e.Request)) // Extract URI from the request correctly
                .Select(g => new ResourceAccessCount { Uri = g.Key, AccessCount = g.Count() })
                .OrderByDescending(r => r.AccessCount);
        }

        private string GetUriFromRequest(string request)
        {
            var parts = request.Split(' ');

            // Check if the request is in the correct format ("GET /path HTTP/1.0")
            if (parts.Length > 1)
            {
                return parts[1]; // Return the URI (the second element in the request)
            }

            return string.Empty; // Return an empty string if the format is invalid
        }

    }

}

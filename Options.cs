using CommandLine;
using JetBrains.Annotations;

[PublicAPI]
class Options
{
	[Option('l', "limit",
		HelpText = "Limits the number of results per page (min 1, max 25). Can be used for performance tuning.",
		Default = 10)]
	public int Limit { get; set; }


	[Option('s', "stream", HelpText = "Filters by Unity Release stream. [LTS, BETA, ALPHA, TECH]")]
	public IEnumerable<Stream> Stream { get; set; }


	[Option('p', "platform", HelpText = "Filters by Unity release download platform. [MAC_OS, LINUX, WINDOWS]")]
	public IEnumerable<Platform> Platform { get; set; }


	[Option('a', "architecture", HelpText = "Filters by Unity release download architecture. [X86_64, ARM64]")]
	public IEnumerable<Architecture> Architecture { get; set; }


	[Option('v', HelpText = "Filters by a full text search on the version string.")]
	public string Version { get; set; }


	[Option('d', "since-date",
		HelpText = "Filters by a minimum release date (yyyy-mm-dd). Accepts formats supported by .NET DateTime.Parse.")]
	public DateTime? SinceReleaseDate { get; set; }

	public static string BuildQuery<T>(string key, IEnumerable<T> values) where T : Enum
	{
		string s = string.Empty;
		foreach (string name in values.Select(x => x.ToString().ToUpper()))
			s += $"&{key}={name}";
		return s;
	}

	public static string BuildQuery(string key, string value)
	{
		if (value != null)
			return $"&{key}={value}";

		return string.Empty;
	}
}

// ReSharper disable InconsistentNaming

[PublicAPI]
enum Stream
{
	LTS,
	BETA,
	ALPHA,
	TECH,
}

[PublicAPI]
enum Platform
{
	MAC_OS,
	LINUX,
	WINDOWS,
}

[PublicAPI]
enum Architecture
{
	X86_64,
	ARM64,
}
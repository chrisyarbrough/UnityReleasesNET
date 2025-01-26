using System.Globalization;
using Newtonsoft.Json.Linq;

if (!CommandLineUtility.TryGetOptions(args, out Options options))
	return;

using HttpClient client = new HttpClient();

// Pagination
int offset = 0;
int total;

do
{
	retry:

	HttpResponseMessage response = await client.GetAsync(
		"https://services.api.unity.com/unity/editor/release/v1/releases" +
		"?limit=" + options.Limit +
		"&offset=" + offset +
		"&order=RELEASE_DATE_DESC" +
		Options.BuildQuery("stream", options.Stream) +
		Options.BuildQuery("platform", options.Platform) +
		Options.BuildQuery("architecture", options.Architecture) +
		Options.BuildQuery("version", options.Version)
	);

	if (!response.IsSuccessStatusCode)
	{
		string responseContent = await response.Content.ReadAsStringAsync();
		string title = response.StatusCode + $" ({response.ReasonPhrase})";
		Console.Error.WriteLine($"Response:{title}\n{responseContent}");

		if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
		{
			int seconds = int.Parse(retryAfterValues.First().Split(' ').Last());
			Console.Error.WriteLine($"Retrying int {seconds} seconds...");
			Thread.Sleep(TimeSpan.FromSeconds(seconds));
			goto retry;
		}

		return;
	}

	string json = await response.Content.ReadAsStringAsync();
	dynamic responseObject = JObject.Parse(json);
	total = responseObject.Value<int>("total");

	foreach (dynamic release in responseObject.results)
	{
		DateTime releaseDate = DateTime.Parse((string)release.releaseDate, CultureInfo.InvariantCulture);
		if (releaseDate < options.SinceReleaseDate)
			return;

		string version = release.version;
		string revision = release.shortRevision;
		Console.WriteLine(version + " " + revision);
	}

	offset += options.Limit;
} while (offset < total);
using CommandLine;
using CommandLine.Text;

static class CommandLineUtility
{
	public static bool TryGetOptions(IEnumerable<string> args, out Options options)
	{
		var parser = new Parser(config => config.HelpWriter = null);
		var parserResult = parser.ParseArguments<Options>(args);

		parserResult
			.WithNotParsed(errors =>
			{
				HelpText helpText;
				if (errors.IsVersion())
				{
					helpText = HelpText.AutoBuild(parserResult);
				}
				else
				{
					helpText = HelpText.AutoBuild(parserResult, h =>
					{
						h.Copyright = string.Empty;
						h.Heading = string.Empty;
						return HelpText.DefaultParsingErrorsHandler(parserResult, h);
					}, e => e);
				}

				Console.WriteLine(helpText.ToString().Trim());
			});

		options = parserResult.Value;
		return !parserResult.Errors.Any();
	}
}
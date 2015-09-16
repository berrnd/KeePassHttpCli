using CommandLine;
using CommandLine.Text;

namespace KeePassHttpCli
{
    internal class Options
    {
        [Option('a', "action", HelpText = "Action, have to be one of the following strings (see explanation below): associate, get-single-password")]
        public string Action { get; set; }

        [Option('f', "search-field", HelpText = "Search field, have to be one the following strings: url, any")]
        public string SearchField { get; set; }

        [Option('s', "search-string", HelpText = "Search string")]
        public string SearchString { get; set; }

        [Option('o', "stay-open", HelpText = "Keeps the console window open")]
        public bool StayOpen { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText helpText = HelpText.AutoBuild(this);
            helpText.AddPreOptionsLine("https://github.com/berrnd/KeePassHttpCli");
            helpText.AddPostOptionsLine("Actions:\n\n\associate: Associate a new KeePass database, connection info is stored encrypted (can only be decrypted by the current logged in user) in %localappdata%\\KeePassHttpCli\n\nget-single-password: Get a single password in plain text (StdOut), if more than one entry is received, the first one is taken");
            return helpText;
        }
    }
}

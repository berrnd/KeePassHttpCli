using CommandLine;
using CommandLine.Text;

namespace KeePassHttpCli
{
    internal class Options
    {
        [Option('a', "associate", MutuallyExclusiveSet = "action", HelpText = "Associate to a KeePass database")]
        public bool Associate { get; set; }

        [Option('p', "get-single-password", MutuallyExclusiveSet = "action", HelpText = "Retrieve a single password in plain text (StdOut), if more than one entry is received, the password from the first one is taken")]
        public bool GetSinglePassword { get; set; }

        [Option('u', "url", HelpText = "Search string")]
        public string Url { get; set; }

        [Option('s', "stay-open", HelpText = "Keeps the console window open")]
        public bool StayOpen { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this);
        }

        public bool AnyActionRequested
        {
            get
            {
                return this.Associate || this.GetSinglePassword;
            }
        }
    }
}

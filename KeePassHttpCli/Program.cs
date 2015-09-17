using KeePassHttpClient;
using System;
using System.Reflection;

namespace KeePassHttpCli
{
    internal class Program
    {
        private readonly static ConsoleColor DefaultConsoleForegroundColor = Console.ForegroundColor;
        private static int ExitCode = 0;
        private const string SETTINGS_FILE_NAME = "UserSettings.dat";

        private static void Main(string[] args)
        {
            Console.Title = Assembly.GetExecutingAssembly().GetName().Name;

            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                KeePassHttpCliSettings userSettings = KeePassHttpCliSettings.Load<KeePassHttpCliSettings>(SETTINGS_FILE_NAME);

                try
                {
                    KeePassHttpConnection connection;
                    if (userSettings.ConnectionInfo != null)
                        connection = KeePassHttpConnection.FromConnectionInfo(userSettings.ConnectionInfo);
                    else
                        connection = new KeePassHttpConnection();

                    if (!connection.Connect() && !options.Action.Equals("associate") && !String.IsNullOrEmpty(options.Action))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("No KeePass database associated and/or open, you have to do this first");
                        CloseManually();
                    }

                    if (!String.IsNullOrEmpty(options.Action) && options.Action.Equals("associate"))
                    {
                        connection.Associate();
                        userSettings.ConnectionInfo = connection.GetConnectionInfo();
                        userSettings.Save(SETTINGS_FILE_NAME);
                    }
                    else if (!String.IsNullOrEmpty(options.Action) && (options.Action.Equals("get-single-password") || options.Action.Equals("get-single-field")))
                    {
                        KeePassCredential[] credentials = null;

                        if (!String.IsNullOrEmpty(options.SearchField) && options.SearchField.Equals("url"))
                            credentials = connection.RetrieveCredentialsByUrl(options.SearchString);
                        else if (!String.IsNullOrEmpty(options.SearchField) && options.SearchField.Equals("any"))
                            credentials = connection.RetrieveCredentialsByCustomSearchString(options.SearchString);

                        if (credentials != null && credentials.Length > 0)
                        {
                            if (options.Action.Equals("get-single-password"))
                                Console.Write(credentials[0].Password);
                            else if (options.Action.Equals("get-single-field"))
                            {
                                KeePassHttpResponseStringField stringField = credentials[0].StringFields.Find(p => p.Key == options.ReturnField);
                                if (stringField != null)
                                    Console.Write(stringField.Value);
                            }
                        }
                        else
                            ExitCode = 1;
                    }
                    else
                    {
                        Console.Write(options.GetUsage());
                        CloseManually();
                    }

                    connection.Disconnect();
                }
                catch (Exception ex)
                {
                    ExitCode = -1;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.GetType().Name);
                    Console.WriteLine(ex.StackTrace);
                }

                if (options.StayOpen)
                    CloseManually();
            }
        }

        private static void CloseManually()
        {
            Console.ForegroundColor = DefaultConsoleForegroundColor;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
            Close();
        }

        private static void Close()
        {
            Environment.Exit(ExitCode);
        }
    }
}

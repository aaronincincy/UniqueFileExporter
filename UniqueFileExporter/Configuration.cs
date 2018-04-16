using Microsoft.Extensions.CommandLineUtils;
using System;
using System.IO;

namespace UniqueFileExporter
{
    public class Configuration
    {
        public string Source { get; private set; }
        public string Destination { get; private set; }
        public bool Recursive { get; private set; }

        public bool IsValid { get; private set; }

        private Configuration() { }

        public static Configuration FromArguments(string[] args)
        {
            var config = new Configuration();
            var cla = new CommandLineApplication(throwOnUnexpectedArg: false);
            var recursive = cla.Option("-r|--recursive", "Process the source folder recursively.", CommandOptionType.NoValue);
            var destOption = cla.Option("-d|--dest", "Set the destination folder", CommandOptionType.SingleValue);
            var srcOption = cla.Option("-s|--source", "Set the source folder", CommandOptionType.SingleValue);
            cla.HelpOption("-?|-h|--help");
            cla.OnExecute(() =>
            {
                config.Source = srcOption.Value();
                config.Destination = destOption.Value();
                config.Recursive = recursive.HasValue();

                if (srcOption.HasValue() && !ValidateSource(config)) return 1;
                if (destOption.HasValue() && !ValidateDestination(config)) return 1;
                if (!srcOption.HasValue() || !destOption.HasValue())
                {
                    Console.WriteLine("Supply values for the following parameters:");
                    if (!srcOption.HasValue())
                    {
                        Console.Write("Source: ");
                        config.Source = Console.ReadLine();
                        if (!ValidateSource(config)) return 1;
                    }
                    if (!destOption.HasValue())
                    {
                        Console.Write("Destination: ");
                        config.Destination = Console.ReadLine();
                        if (!ValidateDestination(config)) return 1;
                    }
                }
                return 0;
            });
            var success = cla.Execute(args);
            config.IsValid = success == 0;
            return config;
        }


        private static bool ValidateSource(Configuration config)
        {
            if (!Directory.Exists(config.Source))
            {
                Console.WriteLine($"Directory not found: {config.Source}");
                return false;
            }
            return true;
        }

        private static bool ValidateDestination(Configuration config)
        {
            //how to validate that dest is a valid folder string/name?
            if (string.IsNullOrWhiteSpace(config.Destination)) return false;
            return true;
        }
    }
}

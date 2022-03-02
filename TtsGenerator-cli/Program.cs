using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TtsGenerator;

namespace TtsGenerator_cli
{
    public class Options
    {
        [Option('t', "text", Required = true, HelpText = "Sentence or paragraph")]
        public string Text { get; set; }

        [Option('l', "lang", Required = true, HelpText = "Language code (2 chars)")]
        public string Language { get; set; }

        [Option('o', "output", Required = true, HelpText = "Path to output mp3")]
        public string Output { get; set; }
    }
    class Program
    {
        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
            .MapResult((opts) => DownloadTextToSpeechAudio(opts), errs => PrintHelp(errs));
        }

        static int DownloadTextToSpeechAudio(Options o)
        {
            var generator = new Generator();
            string url = generator.GenerateUrl(o.Text, o.Language);
            Console.WriteLine(url);
            Console.WriteLine("Download started");
            var wb = new WebClient();
            wb.DownloadFile(url, o.Output);
            wb.Dispose();
            return 0;
        }

        static int PrintHelp(IEnumerable<Error> errs)
        {
            var result = 0;
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;
            return result;
        }
    }
}

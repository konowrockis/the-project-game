using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TheProjectGame.Settings.FromCommandLine
{
    public abstract class BaseParameters
    {
        public int Year { get; set; }

        public string Language { get; set; }

        public string GroupIdentifier { get; set; }

        protected BaseParameters(string[] args)
        {
            string[] code = args[0].Split('-');
            Year = int.Parse(code[0]);
            Language = code[1];
            GroupIdentifier = code[2];
            Parser.Default.ParseArguments(args, this);
        }
    }
}

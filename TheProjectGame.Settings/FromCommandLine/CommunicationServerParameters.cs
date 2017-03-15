using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TheProjectGame.Settings.FromCommandLine
{
    public class CommunicationServerParameters : BaseParameters
    {
        [Option('p', "port")]
        public int Port { get; set; }

        [Option('c', "conf")]
        public string ConfigurationPath { get; set; }

        public CommunicationServerParameters(string[] args) : base(args)
        {
        }
    }
}

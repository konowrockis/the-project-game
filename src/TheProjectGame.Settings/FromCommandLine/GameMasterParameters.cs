using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace TheProjectGame.Settings.FromCommandLine
{
    public class GameMasterParameters : BaseParameters
    {       
        [Option('a',"address")]
        public string Address { get; set; }

        [Option('p',"port")]
        public int Port { get; set; }

        public GameMasterParameters(string[] args) : base(args)
        {
        }

    }
}

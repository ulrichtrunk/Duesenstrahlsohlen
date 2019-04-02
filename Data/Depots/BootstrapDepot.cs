using NPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Depots
{
    public class BootstrapDepot : BaseDepot
    {
        public BootstrapDepot(IDatabase database)
            : base(database)
        {

        }

        public void Run(string command)
        {
            Database.Execute(command);
        }
    }
}

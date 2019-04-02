using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    [TableName("Users")]
    public class User : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsLocal { get; set; }
    }
}

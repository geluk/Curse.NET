using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curse.NET.Model
{
    public class CreateInviteRequest : RequestObject
    {
        public string ChannelId { get; set; }
        public bool AutoRemoveMembers { get; set; }
        public int LifespanMinutes { get; set; }
        public int MaxUses { get; set; }
        public string AdminDescription { get; set; }
        public int ReadableWordDescription { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ProcCore.JqueryHelp.DeferredHelp
{
    public enum funcDeferred
    {
        done, fail, always, then
    }

    public class Deferred : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public Deferred()
        {
            this.DeferredCollect = new List<DeferredLive>();
        }
        public List<DeferredLive> DeferredCollect { get; set; }
        public String ToScriptString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var f in this.DeferredCollect)
            {
                sb.AppendLine("." + f.deferredName + "(" + f.deferredFunc.ToScriptString() + ")");
            }
            return sb.ToString();
        }
    }

    public class DeferredLive
    {
        public DeferredLive()
        {
            this.deferredName = funcDeferred.done;
            this.deferredFunc = new funcBase();
            this.deferredFunc.funcParam.AddRange(new String[] { "data", "textStatus", "jqXHR" });
        }
        public funcDeferred deferredName { get; set; }
        public funcBase deferredFunc { get; set; }
    }
}

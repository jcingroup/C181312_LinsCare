using System;
using ProcCore.NetExtension;

namespace ProcCore.JqueryHelp.SelectRelationHandle
{
    /// <summary>
    /// 地址的Ajax Jquery Script 產生器
    /// </summary>
    public class SelectRelation : BaseJqueryScriptHelp, ijQueryUIScript
    {
        public SelectRelation(String elementId, String relation_element_Id)
        {
            this.Id = elementId;
            this.options = new SelectOption(relation_element_Id);
        }

        public String Id { get; set; }
        public SelectOption options { get; set; }

        public String ToScriptString()
        {
            return "$('#" + this.Id + "').selectajax({" + options.ToScriptString() + "});";
        }
    }

    public class SelectOption:ijQueryUIScript
    {
        public SelectOption(String relation_element_Id)
        {
            this.relation_element = "$('#" + relation_element_Id + "')";
        }

        public String relation_element { get; set; }
        public String master_value { get; set; }
        public String relation_value { get; set; }
        public String data_url { get; set; }
        public String ToScriptString()
        {
            MakeJqScript jqScript = new MakeJqScript() { GetObject = this};
            jqScript.ItemNotMake.Add("relation_element");
            return jqScript.MakeScript();
        }
    }
}

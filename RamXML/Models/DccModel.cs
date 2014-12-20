using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RamXML.Models
{
    public class DccModel
    {
        doc doc;
        public DccModel(doc doc)
        {
            this.doc = doc;
        }

        public string LoadTree(int Page = 0,int Count = 0)
        {
            var str = HttpContext.Current.Session["page"];
            
            Func<concepts, string> f = null;
            f = delegate(concepts c)
            {
                string s = "<li data-jstree='{\"type\" : \"second\" }' rel='" + c.nodes.First().id_concept + "'>";
                s += c.nodes.First().value;
                s += "<ul>";

                foreach (var node in c.nodes)
                {
                    
                        if (node.nodeName != "DESCRIPTOR")
                        {
                            s+="<li  rel='"+node.id+"'>"+node.value+"</li>";
                            
                        }
                        
                }

                foreach (var i in doc.concepts.Where(j => j.parent.HasValue && j.parent.Value == c.id))
                {
                    s += f(i);
                }
                s += "</ul>";
                s+="</li>";
                return s;
            };
            string o ="";

            if(Count != 0)
            {
                int Skip = Count * Page;
                foreach (var i in doc.concepts.Where(c => !c.parent.HasValue).Skip(Skip).Take(Count))
                {
                    o += f(i);
                }
            }
            else
            {
                foreach (var i in doc.concepts)
                {
                    o += f(i);
                }
            }

			

            return o;
        }

    }
}
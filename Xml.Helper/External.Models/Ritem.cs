using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;  
using System.Xml.Serialization; 


namespace Xml.Helper.External.Models
{
    [Serializable]
    [XmlRoot("CONCEPT"), XmlType("Ritem")]  
    public class Ritem
    {

       // public string[] nodes { get; set; } 
           public string Descriptor { get; set; }  
           public string DF { get; set; }  
           public string DS { get; set; }
           public string ES { get; set; }  
    }
}

  
    
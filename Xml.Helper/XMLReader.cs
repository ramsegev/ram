using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using Xml.Helper.External.Models;

namespace Xml.Helper
{
    public class XMLReader
    {
        
        public List<Ritem> RetrunListOfRitem()  
         {  
               string xmlData = HttpContext.Current.Server.MapPath("~/Doc/1.xml");//Path of the xml script  
               DataSet ds = new DataSet();//Using dataset to read xml file  
               ds.ReadXml(xmlData);
               
               var items = new List<Ritem>();  
               items = (from rows in ds.Tables[0].AsEnumerable() 
               select new Ritem 
               {     
                   
              
                    
                     Descriptor = rows[0].ToString(), 
                     DF = rows[1].ToString(),  
                     DS = rows[2].ToString(),  
                     ES = rows[3].ToString(),

               }).ToList();  
               return items;  
         }  
      }  
 }

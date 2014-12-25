using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Web.Helpers;
using Xml.Helper;
using System.Data.Entity;
using System.Net;
using RamXML.Models;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace RamXML.Controllers
{

    public class HomeController : Controller
    {
        private ramxmlEntities db = new ramxmlEntities();
        private static IDictionary<Guid, int> tasks = new Dictionary<Guid, int>();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (Request.Files.Count > 0)
            {
                file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var path = "";
                    var fileName = Path.GetFileName(file.FileName);
                    ViewBag.fileName = fileName;
                    path = Path.Combine(Server.MapPath("~/Doc/"), fileName);
                    file.SaveAs(path);
                }
            }

            return View();
        }



        int count = 0;

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Upload(string fileName)
        {

            var taskId = Guid.NewGuid();
            tasks.Add(taskId, 0);

            Task.Factory.StartNew(() =>
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                
                var path = "";
                path = Path.Combine(Server.MapPath("~/Doc/"), fileName);
                XmlDocument docX = new XmlDocument();
                docX.Load(path);
                doc documentDB = new doc();
                documentDB.name = fileName;
                db.Entry(documentDB).State = EntityState.Added;
                db.SaveChanges();

                XmlNodeList concept = docX.GetElementsByTagName("CONCEPT");

                //count = concept.Count;
                int i = 0;


                foreach (XmlNode item in concept)
                {
                   
                        i=i+1;
                        if (i == 1000)
                        {
                            break;
                        }
                        concepts conceptDB = new concepts();
                        conceptDB.id_doc = documentDB.id;
                        db.Entry(conceptDB).State = EntityState.Added;

                        db.SaveChanges();
                        documentDB.concepts.Add(conceptDB);
                        //conceptDB.id_doc = idDoc;
                        
                        
                        //db.SaveChanges();
                        foreach (XmlNode child in item)
                        {
                            
                            nodes node = new nodes();
                            node.id_concept = conceptDB.id;
                            node.nodeName = child.Name;
                            node.value = child.InnerText;
                            if (child.Name == "DESCRIPTOR")
                            {
                                conceptDB.value = child.InnerText;
                            }
                            conceptDB.nodes.Add(node);
                            db.Entry(node).State = EntityState.Added;
                            db.Entry(conceptDB).State = EntityState.Modified;
                            
                            
                        }


                        //db.SaveChanges();
                        //db.Entry(conceptDB).State = EntityState.Modified;
                        //db.SaveChanges();
                        tasks[taskId] = i; // update task progress

                   

                }

                db.Entry(documentDB).State = EntityState.Modified;
                db.SaveChanges();
                tasks.Remove(taskId);







            });


            return Json(taskId, JsonRequestBehavior.AllowGet);





        }

        public ActionResult CreateTree()
        {
            db.Configuration.AutoDetectChangesEnabled = false;
            doc docTable = db.doc.OrderByDescending(d => d.id).FirstOrDefault();
            int i = 0;
            foreach (concepts item in db.concepts)
            {
                i = i + 1;
                foreach (nodes node in item.nodes )
                {
                    if (node.nodeName == "BT")
                    {
                        try
                        {
                            concepts parent = db.concepts.SingleOrDefault(c => c.value == node.value);
                            if(parent !=null)
                            {
                                node.concepts.parent = parent.id;
                                db.Entry(node).State = EntityState.Modified;
                            }
                            else
                            {
                                
                                concepts newConcepts = new concepts();
                                newConcepts.id_doc = docTable.id;
                                newConcepts.value = node.value;
                                db.Entry(newConcepts).State = EntityState.Added;
                                db.SaveChanges();

                                nodes newNode = new nodes();
                                newNode.id_concept = newConcepts.id;
                                newNode.nodeName = "DESCRIPTOR";
                                newNode.value = node.value;
                                
                                db.Entry(newNode).State = EntityState.Added;
                                newConcepts.nodes.Add(newNode);
                                docTable.concepts.Add(newConcepts);

                                node.concepts.parent = newConcepts.id;
                                db.Entry(node).State = EntityState.Modified;
                                
                                

                            }
                            
                            
                        }
                        catch (Exception)
                        {
                            
                            
                        }
                        
                    }
                    if(node.nodeName == "NT")
                    {
                        try
                        {

                            concepts parent = db.concepts.SingleOrDefault(c => c.value == node.value);
                            if(parent !=null)
                            {
                                parent.parent = node.concepts.id;
                                db.Entry(parent).State = EntityState.Modified;
                            }
                            else
                            {
                                concepts newConcepts = new concepts();
                                newConcepts.id_doc = docTable.id;
                                newConcepts.value = node.value;
                                newConcepts.parent = node.concepts.id;
                                db.Entry(newConcepts).State = EntityState.Added;
                                db.SaveChanges();

                                nodes newNode = new nodes();
                                newNode.id_concept = newConcepts.id;
                                newNode.nodeName = "DESCRIPTOR";
                                newNode.value = node.value;

                                db.Entry(newNode).State = EntityState.Added;
                                newConcepts.nodes.Add(newNode);
                                docTable.concepts.Add(newConcepts);
                            }
                            
                        }
                        catch (Exception)
                        {

                            

                            
                        }
                        
                    }
                    
                }
                System.Diagnostics.Debug.WriteLine(i.ToString());
            }
            db.SaveChanges();
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string Query)
        {
            
            doc docTable = db.doc.OrderByDescending(i => i.id).FirstOrDefault();
            doc SDocTable = new doc();
            SDocTable.name = docTable.name;
            List<nodes> nodes1 = db.nodes.Where(s => s.value.Contains(Query)).ToList();
            foreach (nodes item in nodes1)
            {
                SDocTable.concepts.Add(item.concepts);
            }
            
             
            IEnumerable<ontology> oTable = db.ontology.Where(o => o.id_doc == docTable.id);
            ViewBag.ontology = oTable;
            //IEnumerable<nodes> nodes = db.nodes.Where(nod => nod.concepts.doc.id == docTable.id);
            //ViewBag.nodes = nodes;
            ViewBag.DocId = docTable.id;
            ViewBag.docName = docTable.name;
            ViewBag.PagesCount = Math.Ceiling((double)SDocTable.concepts.Count / 20);
            ViewBag.Page = 0;
            ViewBag.Count = 20;
            return PartialView("Tree", new DccModel(SDocTable));

        }

        public ActionResult Progress(Guid id)
        {
            return Json(tasks.Keys.Contains(id) ? tasks[id] : 5000000);
        }



        public ActionResult Drag(string oldId, string newId, string nodeType)
        {
            if (nodeType == "default")
            {
                int nId = Convert.ToInt16(newId);
                int oId = Convert.ToInt16(oldId);
                nodes node = db.nodes.Single(n => n.id == oId);
                node.id_concept = nId;
                db.Entry(node).State = EntityState.Modified;
                db.SaveChanges();
            }
            else if (nodeType == "second")
            {
                 if(newId == null)
                {
                    int oId = Convert.ToInt32(oldId);
                    concepts concept = db.concepts.Single(n => n.id == oId);
                    concept.parent = null;
                    db.Entry(concept).State = EntityState.Modified;
                    db.SaveChanges();
                }
                 else if (newId != "#")
                {
                    int nId = Convert.ToInt32(newId);
                    int oId = Convert.ToInt32(oldId);
                    concepts concept = db.concepts.Single(n => n.id == oId);
                    concept.parent = nId;
                    db.Entry(concept).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                else 
                {
                    int oId = Convert.ToInt32(oldId);
                    concepts concept = db.concepts.Single(n => n.id == oId);
                    concept.parent = null;
                    db.Entry(concept).State = EntityState.Modified;
                    db.SaveChanges();
                }



            }

            return Json("True", JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveConcept(string IdRemove)
        {
            int Id = Convert.ToInt32(IdRemove);
            concepts conD = db.concepts.Single(n => n.id == Id);
            doc d = db.doc.Where(d1 => d1.id == conD.doc.id).SingleOrDefault();

            foreach (nodes n in conD.nodes)
            {
                if(n.nodeName != "DESCRIPTOR")
                {
                    string str = n.value;
                    nodes nod = new nodes();
                    nod.nodeName = "DESCRIPTOR";
                    nod.value = str;
                    concepts con = new concepts();
                    con.nodes.Add(nod);
                    d.concepts.Add(con);
                }
               

            }

            db.concepts.Remove(conD);
            db.Entry(d).State = EntityState.Modified;
            db.SaveChanges();
            return View(new DccModel(d));
        }

       
        public ActionResult XMLShow(string fileName, string searchString, int? page)
        {
            

                doc docTable = db.doc.OrderByDescending(i => i.id).FirstOrDefault();
                IEnumerable<ontology> oTable = db.ontology.Where(o => o.id_doc == docTable.id);
                ViewBag.ontology = oTable;
                IEnumerable<nodes> nodes = db.nodes.Where(nod => nod.concepts.doc.id == docTable.id);
                ViewBag.nodes = nodes;
                ViewBag.DocId = docTable.id;
                ViewBag.docName = docTable.name;
                ViewBag.PagesCount = Math.Ceiling((double)docTable.concepts.Count / 20);
                ViewBag.Page = 0;
                ViewBag.Count = 20;
                return View(new DccModel(docTable));
            



        }

        public ActionResult Pager(int Page, int Count)
        {
            doc docTable = db.doc.OrderByDescending(i => i.id).FirstOrDefault();
            ViewBag.Page = Page;
            ViewBag.Count = Count;
            return PartialView("Tree", new DccModel(docTable));
        }
        public ActionResult DeleteItem(string id)
        {
            try
            {
                //int idI = Convert.ToInt16(id);
                ////concept concept = db.concept.First(item => item.id == idI);
                //db.Entry(concept).State = EntityState.Deleted;
                //db.SaveChanges();
                //var result = new { Success = "True", Message = "True" };
                //return Json(result, JsonRequestBehavior.AllowGet);
                var result = new { Error = "True", Message = "False" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var result = new { Error = "True", Message = "False" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult GetDropDownItems()
        {
            List<dropdown> items = db.dropdown.ToList();
            var result = new { items = items };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddDropDownItems(string id)
        {
            dropdown dropdown = new dropdown();
            dropdown.value = id;
            db.Entry(dropdown).State = EntityState.Added;
            db.SaveChanges();
            var result = new { Success = "True", Message = "True" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOntology(string item_id, string dropdown1, string dropdown2, string dropdown3)
        {
            int id_concept = Convert.ToInt32(item_id);
            try
            {
                ontology on = db.ontology.Single(o => o.id_concept == id_concept);
                concepts con = db.concepts.Single(c => c.id == id_concept);
                on.id_doc = con.id_doc;
                on.id_concept = id_concept;
                on.dropdown1 = dropdown1;
                on.dropdown2 = dropdown2;
                on.dropdown3 = dropdown3;
                db.Entry(on).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { dropdown1 = on.dropdown1, dropdown2 = on.dropdown2, dropdown3 = on.dropdown3 });

            }
            catch (Exception)
            {
                ontology ontology = new ontology();
                concepts con = db.concepts.Single(c => c.id == id_concept);
                ontology.id_doc = con.id_doc;
                ontology.id_concept = id_concept;
                ontology.dropdown1 = dropdown1;
                ontology.dropdown2 = dropdown2;
                ontology.dropdown3 = dropdown3;
                db.Entry(ontology).State = EntityState.Added;
                db.SaveChanges();
                return Json(new { dropdown1 = ontology.dropdown1, dropdown2 = ontology.dropdown2, dropdown3 = ontology.dropdown3 });

            }

        }

        public ActionResult GetOntology(string id)
        {
            try
            {
                int iid = Convert.ToInt32(id);
                ontology on = db.ontology.Single(o => o.id_concept == iid);
                return Json(new { dropdown1 = on.dropdown1, dropdown2 = on.dropdown2, dropdown3 = on.dropdown3 });
            }
            catch (Exception)
            {
                return Json("false");
            }



        }

        public ActionResult AddItemTree(string name)
        {
            doc docTable = db.doc.OrderByDescending(i => i.id).FirstOrDefault();
            concepts concept = new concepts();
            nodes node = new nodes();
            node.nodeName = "DESCRIPTOR";
            node.value = name;
            concept.nodes.Add(node);
            docTable.concepts.Add(concept);
            db.Entry(docTable).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewItemTree(string name,string[] syn,string BT,string DF)
        {
            
            doc docTable = db.doc.OrderByDescending(i => i.id).FirstOrDefault();
            concepts concept = new concepts();
            nodes node = new nodes();
            node.nodeName = "DESCRIPTOR";
            node.value = name;
            concept.nodes.Add(node);

            foreach (var item in syn)
            {
                nodes nodeS = new nodes();
                nodeS.nodeName = "UF";
                nodeS.value = item;
                concept.nodes.Add(nodeS);
            }
            nodes nodeB = new nodes();
            nodeB.nodeName = "BT";
            nodeB.value = BT;
            concept.nodes.Add(nodeB);

            nodes nodeD = new nodes();
            nodeD.nodeName = "DF";
            nodeD.value = DF;
            concept.nodes.Add(nodeD);
            docTable.concepts.Add(concept);
            db.Entry(docTable).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddItemSyn(string name, string type, string select_id)
        {
            int idC=Convert.ToInt32(select_id);
            concepts concept = db.concepts.SingleOrDefault(c => c.id == idC);

            nodes node = new nodes();
            node.nodeName = type;
            node.value = name;
            concept.nodes.Add(node);
            db.Entry(concept).State = EntityState.Modified;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchOntology(string searchTterm)
        {
            
            var Result = db.nodes.Where(w => w.value.Contains(searchTterm)).Select(s=>s.value).Take(100).ToArray();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchPrefetchOntology()
        {
            var Result = db.nodes;
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult GetOntologyTable(string id)
        {
            int iId = Convert.ToInt16(id);
            IEnumerable<ontology> ontologys = db.ontology.Where(o => o.id_doc == iId);
            return Json(ontologys.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDropDownItems(string id)
        {
            try
            {
                int idI = Convert.ToInt16(id);
                dropdown dropdown = db.dropdown.First(item => item.Id == idI);
                db.Entry(dropdown).State = EntityState.Deleted;
                db.SaveChanges();
                var result = new { Success = "True", Message = "True" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                var result = new { Error = "True", Message = "False" };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }

    }

}
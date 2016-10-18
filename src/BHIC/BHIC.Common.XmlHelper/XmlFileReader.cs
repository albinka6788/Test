using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BHIC.Common.XmlHelper
{
    public class XmlFileReader
    {
        #region Comment : Here variables/properties declaration

        public static string FilePath { get; set; }

        #endregion
        
        #region Comment : Here class methods

        #region Comment : Here public methods

        /// <summary>
        /// Get XLinq or Linq-to-XML object for specified file path
        /// </summary>
        /// <returns></returns>
        public XDocument GetXDocument()
        {
            return this.LoadXmlDocument(FilePath);
        }

        /// <summary>
        /// Get XLinq or Linq-to-XML object for specified file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public XDocument GetXDocument(string filePath)
        {
            return this.LoadXmlDocument(filePath);
        }

        /// <summary>
        /// Get XLinq or Linq-to-XML object for specified file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public XmlDocument GetXmlDocument(string filePath)
        {
            return LoadXMLFile(filePath);
        }

        /// <summary>
        /// Return the string data from specified xml file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public String GetXmlDocumentString(string filePath)
        {
            IsValidFile(filePath);

            XmlDocument document = LoadXMLFile(filePath);
            return document.InnerXml;
        }

        /// <summary>
        /// Get first occurance of specific node element from spefied xml file path 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public XmlNode GetSpecificXmlNode(string filePath, string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName.Trim()))
            {
                throw new ApplicationException("Specified empty Node name");
            }

            XmlDocument document = LoadXMLFile(filePath);

            return document.SelectSingleNode(nodeName);
        }

        /// <summary>
        /// Get all node items list from specified xml file path 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public XmlNodeList GetSpecificNodeList(string filePath, string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName.Trim()))
            {
                throw new ApplicationException("Specified empty Node name");
            }

            XmlDocument document = LoadXMLFile(filePath);

            return document.SelectNodes(nodeName); 
        }

        /// <summary>
        /// Get all node items list from specified XmlDocument 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public XmlNodeList GetSpecificNodeList(XmlDocument document, string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName.Trim()))
            {
                throw new ApplicationException("Specified empty Node name");
            }

            return document.SelectNodes(nodeName);
        }

        /// <summary>
        /// Get root XmlElement of
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public XmlElement GetRootXmlElement(string filePath)
        {
            XmlDocument document = LoadXMLFile(filePath);

            return document.DocumentElement;
        }

        /// <summary>
        /// Get XmlTextReader object for XmlDocument traversal
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public XmlTextReader GetXmlTextReader(string filePath)
        {
            using (XmlTextReader reader = new XmlTextReader(filePath))
            {
                return reader;
            }
        }

        /// <summary>
        /// Get XmlReader object for XmlDocument traversal
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public XmlReader GetXmlReader(string filePath)
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                return reader;
            }
        }

        /// <summary>
        /// Traverse XmlDocument to get all node type, thier attrbutes and types information
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public void DoXmlTraversal(string filePath)
        {
            using(XmlTextReader reader = new XmlTextReader(filePath))
	        {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            Console.Write("<" + reader.Name);

                            //Comment : Here Element node types can include a list of attribute nodes 
                            //that are associated with them by sequnetial iteration
                            while (reader.MoveToNextAttribute()) // Read the attributes.
                                Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                            Console.Write(">");
                            Console.WriteLine(">");
                            break;

                        case XmlNodeType.Text: //Display the text in each element.
                            Console.WriteLine(reader.Value);
                            break;

                        case XmlNodeType.EndElement: //Display the end of the element.
                            Console.Write("</" + reader.Name);
                            Console.WriteLine(">");
                            break;
                    }
                }
	        }
        }

        /// <summary>
        /// Method return specific tag and specific attribute value
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="tagName"></param>
        /// <param name="attrName"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        public string GetSpecificTagAttributeValue(XmlDocument xDoc,string tagName, string attrName, int nodeIndex = 0)
        {
            if (string.IsNullOrEmpty(attrName.Trim()))
            {
                throw new ApplicationException("Specified empty Attribute name");
            }

            string retvalue = null;

            //Get the record at the specified node index
            var currentRecord = this.GetCurrentXmlElement(ref xDoc, tagName, nodeIndex);

            //Get the record attribute information
            if (currentRecord != null)
            {
                //Comment : Here finally get attribute value
                retvalue = currentRecord.Attributes[attrName].Value;
            }

            return retvalue;
        }

        /// <summary>
        /// Get specific attribute value for supplied XmlNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public string GetSpecificAttributeValue(XmlNode xmlNode, string attrName)
        {
            if (string.IsNullOrEmpty(attrName.Trim()))
            {
                throw new ApplicationException("Specified empty Attribute name");
            }
            else if (xmlNode == null)
            {
                throw new ApplicationException("Specified empty XmlNode");
            }

            return xmlNode.Attributes[attrName].Value;
        }

        /// <summary>
        /// Method return specific child tag inner text or value
        /// </summary>
        /// <param name="xDoc"></param>
        /// <param name="rooTagName"></param>
        /// <param name="tagName"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        public string GetSpecificChildTagValue(XmlDocument xDoc, string rooTagName, string tagName, int nodeIndex = 0)
        {
            if (string.IsNullOrEmpty(tagName.Trim()))
            {
                throw new ApplicationException("Specified empty XmlTag name");
            }

            string retvalue = null;

            //Get the record at the specified node index
            var currentRecord = this.GetCurrentXmlElement(ref xDoc, rooTagName, nodeIndex);

            //Get the record tag information
            if (currentRecord !=null)
            {
                //Comment : Here finally get XmlElement tag value
                var XmlNode = currentRecord.GetElementsByTagName(tagName)[0];

                //Comment : Here based on XmlElemeny type return resultant string
                if (!XmlNode.HasChildNodes)
                {
                    retvalue = XmlNode.InnerText;
                }
                else if (XmlNode.HasChildNodes)
                {
                    retvalue = XmlNode.InnerXml;
                }
                else
                {
                    retvalue = XmlNode.Value;
                }
            }

            return retvalue;
        }

        /// <summary>
        /// Generic function to read node value from xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <param name="sectionName"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static T ReadXml<T>(string filePath, string sectionName, string nodeName)
        {
            string defaultSection = "/" + sectionName + "/";
            string ret = string.Empty;

            T retT = default(T);

            try
            {
                XmlDocument xdXml = LoadXMLFile(filePath);

                //Make a nodelist 
                XmlNodeList xnNodes = xdXml.SelectNodes(defaultSection + nodeName);

                //Walk through the list 
                foreach (XmlNode node in xnNodes)
                {
                    XmlNodeList childNodes = node.ChildNodes;
                    foreach (XmlNode child in childNodes)
                    {
                        ret = child.InnerText.Trim();
                        // casting the value to generic 
                        retT = ((T)(Convert.ChangeType(ret, typeof(T))));
                    }
                }
                xdXml = null;
                xnNodes = null;
            }
            catch { }

            return retT;
        }

        /// <summary>
        /// Return parsed XDocument from xml string data
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public XDocument GetXmlDcoumentFromXmlString(string xmlString)
        {
            return XDocument.Parse(xmlString);
        }

        /// <summary>
        /// Return DataSet from specified file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string filePath)
        {
            DataSet retDS = new DataSet();
            retDS.Reset();
            retDS.ReadXml(filePath);

            return retDS;
        }

        #region Comment : Here Methods to get Web.Config details 

        /// <summary>
        /// Get collection of key-value pair of "app settings" in supplied application Web.config file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAppSettingsCollection(string filePath)
        {            
            //doc.Load(HttpContext.Current.Server.MapPath("~/web.config"));
            XmlDocument document = LoadXMLFile(filePath);

            XmlNodeList list = document.SelectNodes("//configuration/appSettings");

            Dictionary<string, string> retDictionary = new Dictionary<string, string>();

            foreach (XmlNode node in list[0].ChildNodes) 
            {
                //Comment : Here Must check for attributes existance in this node
                if (node.Attributes != null)
                {
                    retDictionary.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            };            

            return retDictionary;
        }

        /// <summary>
        /// Get collection of key-value pair of "app settings" in supplied application Web.config file XmlDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAppSettingsCollection(XmlDocument document)
        {
            if (document == null && !document.HasChildNodes)
            {
                throw new ApplicationException("Specified empty XmlDocument");
            }            

            XmlNodeList list = document.SelectNodes("//configuration/appSettings");

            Dictionary<string, string> retDictionary = new Dictionary<string, string>();

            foreach (XmlNode node in list[0].ChildNodes)
            {
                //Comment : Here Must check for attributes existance in this node
                if (node.Attributes != null)
                {
                    retDictionary.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                }
            };

            return retDictionary;
        }

        /// <summary>
        /// Get collection of key-value pair of "connection string" in supplied application Web.config file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetConnectionStringsCollection(string filePath)
        {
            //doc.Load(HttpContext.Current.Server.MapPath("~/web.config"));
            XmlDocument document = LoadXMLFile(filePath);

            XmlNodeList list = document.SelectNodes("//configuration/connectionStrings");

            Dictionary<string, string> retDictionary = new Dictionary<string, string>();

            foreach (XmlNode node in list[0].ChildNodes)
            {
                //Comment : Here Must check for attributes existance in this node
                if (node.Attributes != null)
                {
                    retDictionary.Add(node.Attributes["name"].Value, node.Attributes["connectionString"].Value);
                }
            };

            return retDictionary;
        }

        /// <summary>
        /// Get collection of key-value pair of "connection string" in supplied application Web.config file XmlDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetConnectionStringsCollection(XmlDocument document)
        {
            if (document == null && !document.HasChildNodes)
            {
                throw new ApplicationException("Specified empty XmlDocument");
            }

            XmlNodeList list = document.SelectNodes("//configuration/connectionStrings");

            Dictionary<string, string> retDictionary = new Dictionary<string, string>();

            foreach (XmlNode node in list[0].ChildNodes)
            {
                //Comment : Here Must check for attributes existance in this node
                if (node.Attributes != null)
                {
                    retDictionary.Add(node.Attributes["name"].Value, node.Attributes["connectionString"].Value);
                }
            };

            return retDictionary;
        }

        /// <summary>
        /// Get list of XmlNode of specific section in web.config
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public List<XmlNode> GetListOfWebConfigSectionNodes(string filePath, string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ApplicationException("Specified empty SectionName");
            }

            XmlDocument document = LoadXMLFile(filePath);

            XmlNodeList nodesList = document.SelectNodes("//configuration/" + sectionName);

            List<XmlNode> retList = new List<XmlNode>();

            foreach (XmlNode node in nodesList[0].ChildNodes)
            {
                //Comment : Here Must check for attributes existance in this node
                if (node.Attributes != null)
                {
                    retList.Add(node);
                }
            };

            return retList;
        }

        /// <summary>
        /// Get all attribute key-value collection for supplied XmlNode
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetNodeAllAttributeKeyValue(XmlNode xmlNode)
        {
            if (xmlNode == null)
            {
                throw new ApplicationException("Specified empty XmlNode");
            }

            Dictionary<string, string> retDictionary = new Dictionary<string, string>();

            //Comment : Here Must check for attributes existance in this node
            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute atrribute in xmlNode.Attributes)
                {
                    retDictionary.Add(atrribute.Name, atrribute.Value);
                }
            }

            return retDictionary;
        }

        #endregion

        #region Comment : Here Get value from Dictionary based on key

        /// <summary>
        /// Get specific value of supplied key from collection
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetKeysValue(Dictionary<string, string> nameValueCollection,string keyName)
        {
            if (nameValueCollection == null && nameValueCollection.Count <= 0)
            {
                throw new ApplicationException("Specified empty Collection");
            }

            return
            (
                from nvc in nameValueCollection
                where nvc.Key.Contains(keyName)
                select nvc.Value
            ).FirstOrDefault();
        }
        
        #endregion

        #endregion

        #region Comment : Here private methods

        private static void IsValidFile(string fileFullName)
        {
            if (!string.IsNullOrEmpty(fileFullName.Trim()) && File.Exists(fileFullName))
            {
                return;
            }
            else
            {
                throw new ApplicationException((string.IsNullOrEmpty(fileFullName.Trim()) ? "File name not specified" : 
                    string.Format("Specified file i.e. {0}, does not exist", fileFullName)));
            }
        }

        private static XmlDocument LoadXMLFile(string fileFullName)
        {
            IsValidFile(fileFullName);

            XmlDocument document = new XmlDocument();
            document.Load(fileFullName);

            return document;
        }

        private XDocument LoadXmlDocument(string fileFullName)
        {
            IsValidFile(fileFullName);

            return XDocument.Load(fileFullName);
        }

        private XmlElement GetCurrentXmlElement(ref XmlDocument xDoc, string rooTagName, int nodeIndex = 0)
        {
            XmlElement retXElement = null;

            if (xDoc != null && xDoc.HasChildNodes)
            {
                //Get root element
                var root = xDoc.DocumentElement;

                //Determine maximum possible index
                var max = root.GetElementsByTagName(rooTagName).Count - 1;

                //Get the record at the nth index
                if (max > 0 && nodeIndex < max)
                {
                    //Get the record at the specified node index
                    var currentRecord = (XmlElement)root.ChildNodes[nodeIndex];
                    retXElement = currentRecord;
                }
            }

            return retXElement;
        }

        #endregion

        #endregion
    }
}

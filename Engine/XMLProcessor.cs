using Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine
{
    public class XMLProcessor
    {
        public bool isDistinct(XmlDocument xmlDoc)
        {
            bool IsDistinct = false;

            foreach (XmlNode fetchNode in xmlDoc.GetElementsByTagName("fetch"))
            {
                if (fetchNode.Attributes["distinct"] != null)
                {
                    IsDistinct = Convert.ToBoolean(fetchNode.Attributes["distinct"].InnerText);
                }
            }

            return IsDistinct;
        }
        public string GetEntityName(XmlDocument xmlDoc)
        {
            string entityName = "";
            //Get the main entity from fetchXML
            foreach (XmlNode entityNode in xmlDoc.GetElementsByTagName("entity"))
            {
                entityName = entityNode.Attributes["name"].InnerText;
            }

            if (string.IsNullOrEmpty(entityName))
            {
                throw new Exception("Couldn't find entity name.");
            }

            return entityName;
        }

        public List<string> GetEntityAttributes(XmlDocument xmlDoc)
        {
            List<string> attributeList = new List<string>();

            //get all the attributes for the main entity and put them in a list.
            foreach (XmlNode entityAttributeNode in xmlDoc.DocumentElement.SelectNodes("/fetch/entity/attribute"))
            {
                attributeList.Add(entityAttributeNode.Attributes["name"].InnerText);
            }

            if(attributeList.Count == 0)
            {
                throw new Exception("Couldn't find any entity attribute.");
            }

            return attributeList;
        }

        public List<linkEntitiesFromTo> GetLinkEntityList(XmlDocument xmlDoc, string parentEntity, Dictionary<string, string> operatorReplacement)
        {
            List<linkEntitiesFromTo> linkEntitiesFromToList = new List<linkEntitiesFromTo>();

            string node = @"/fetch/entity/link-entity";
            bool isContinue = true;

            while (isContinue)
            {
                string nodeEntity = "";
                XmlNodeList linkNodes = xmlDoc.DocumentElement.SelectNodes(node);
                isContinue = (linkNodes.Count != 0);
                
                foreach (XmlNode linkEntity in linkNodes)
                {
                    //get from and to fields for the joins
                    isContinue = linkEntity.HasChildNodes;
                    if (linkEntity.Attributes["from"] != null && linkEntity.Attributes["to"] != null)
                    {
                        string getLinkType;
                        if (linkEntity.Attributes["link-type"] != null)
                        {
                            getLinkType = linkEntity.Attributes["link-type"].InnerText;
                            getLinkType = "LEFT " + getLinkType.ToUpper();
                        }
                        else
                        {
                            getLinkType = "INNER";
                        }

                        List<linkEntitiesFilters> filterList = GetLinkEntityFilters(xmlDoc, node, operatorReplacement);

                        linkEntitiesFromTo linkEntityFromTo = new linkEntitiesFromTo
                        {
                            linkEntitiesFiltersList = filterList,
                            linkEntityAlias = linkEntity.Attributes["alias"].InnerText,
                            linkEntityFrom = linkEntity.Attributes["from"].InnerText,
                            linkEntityFromToName = linkEntity.Attributes["name"].InnerText,
                            linkEntityJoinType = getLinkType,
                            linkEntityName = parentEntity,
                            linkEntityTo = linkEntity.Attributes["to"].InnerText
                        };

                        linkEntitiesFromToList.Add(linkEntityFromTo);

                        nodeEntity = linkEntity.Attributes["name"].InnerText;
                    }
                }
                parentEntity = nodeEntity;
                node = node + @"/link-entity";
            }
            return linkEntitiesFromToList;
        }

        private List<linkEntitiesFilters> GetLinkEntityFilters(XmlDocument xmlDoc, string node, Dictionary<string, string> operatorReplacement)
        {
            List<linkEntitiesFilters> linkEntitiesFiltersList = new List<linkEntitiesFilters>();

            List<string> inStatements = Operator.GetInStatements();
            List<string> xMinusStatements = Operator.GetxMinusStatements();
            List<string> inFiscalYearStatements = Operator.GetInFiscalStatements();
            List<string> xPlusStatements = Operator.GetxPlusStatements();

            string convertOperator = "";
            node += @"/filter";

            //get all link entities and their filters
            foreach (XmlNode linkEntityFilter in xmlDoc.DocumentElement.SelectNodes(node))
            {
                if (linkEntityFilter.HasChildNodes)
                {
                    for (int i = 0; i < linkEntityFilter.ChildNodes.Count; i++)
                    {
                        ////check if there are multiple filters
                        //if (linkEntityFilter.ChildNodes[i].Name == "filter")
                        //{
                        //    MessageBox.Show("Filters within filters are currently not supported!", "Please Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}

                        if (linkEntityFilter.ChildNodes[i].Attributes["operator"] != null)
                        {
                            operatorReplacement.TryGetValue(linkEntityFilter.ChildNodes[i].Attributes["operator"].InnerText, out convertOperator);
                            if(string.IsNullOrEmpty(convertOperator))
                            {
                                throw new Exception(string.Format("Couldn't find operator: {0}", linkEntityFilter.ChildNodes[i].Attributes["operator"].InnerText));
                            }
                        }

                        if (linkEntityFilter.ChildNodes[i].Attributes["value"] != null)
                        {
                            linkEntitiesFilters filter = new linkEntitiesFilters();
                            if (xMinusStatements.Exists(x => x.Equals(linkEntityFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                filter = new linkEntitiesFilters
                                {
                                    linkEntitiesFiltersFilterType = linkEntityFilter.Attributes["type"].InnerText.ToUpper(),
                                    linkEntitiesFiltersLinkEntityAttribute = linkEntityFilter.ChildNodes[i].Attributes["attribute"].InnerText,
                                    linkEntitiesFiltersLinkEntityName = linkEntityFilter.ParentNode.Attributes["name"].InnerText,
                                    linkEntitiesFiltersLinkEntityOperator = string.Format(convertOperator, "-" + linkEntityFilter.ChildNodes[i].Attributes["value"].InnerText),
                                    linkEntitiesFiltersLinkEntityValue = ""
                                };
                            }
                            else if (xPlusStatements.Exists(x => x.Equals(linkEntityFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                filter = new linkEntitiesFilters
                                {
                                    linkEntitiesFiltersFilterType = linkEntityFilter.Attributes["type"].InnerText.ToUpper(),
                                    linkEntitiesFiltersLinkEntityAttribute = linkEntityFilter.ChildNodes[i].Attributes["attribute"].InnerText,
                                    linkEntitiesFiltersLinkEntityName = linkEntityFilter.ParentNode.Attributes["name"].InnerText,
                                    linkEntitiesFiltersLinkEntityOperator = string.Format(convertOperator, "+" + linkEntityFilter.ChildNodes[i].Attributes["value"].InnerText),
                                    linkEntitiesFiltersLinkEntityValue = ""
                                };
                            }
                            else if (inFiscalYearStatements.Exists(x => x.Equals(linkEntityFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                filter = new linkEntitiesFilters
                                {
                                    linkEntitiesFiltersFilterType = linkEntityFilter.Attributes["type"].InnerText.ToUpper(),
                                    linkEntitiesFiltersLinkEntityAttribute = linkEntityFilter.ChildNodes[i].Attributes["attribute"].InnerText,
                                    linkEntitiesFiltersLinkEntityName = linkEntityFilter.ParentNode.Attributes["name"].InnerText,
                                    linkEntitiesFiltersLinkEntityOperator = string.Format(convertOperator, linkEntityFilter.ChildNodes[i].Attributes["value"].InnerText),
                                    linkEntitiesFiltersLinkEntityValue = ""
                                };
                            }
                            else
                            {
                                filter = new linkEntitiesFilters
                                {
                                    linkEntitiesFiltersFilterType = linkEntityFilter.Attributes["type"].InnerText.ToUpper(),
                                    linkEntitiesFiltersLinkEntityAttribute = linkEntityFilter.ChildNodes[i].Attributes["attribute"].InnerText,
                                    linkEntitiesFiltersLinkEntityName = linkEntityFilter.ParentNode.Attributes["name"].InnerText,
                                    linkEntitiesFiltersLinkEntityOperator = convertOperator,
                                    linkEntitiesFiltersLinkEntityValue = linkEntityFilter.ChildNodes[i].Attributes["value"].InnerText
                                };
                            }
                            
                            linkEntitiesFiltersList.Add(filter);
                        }
                        else
                        {
                            if (inStatements.Exists(x => x.Equals(convertOperator)))
                            {
                                if (linkEntityFilter.ChildNodes[i].HasChildNodes)
                                {
                                    string inValue = FindInStatementValue(linkEntityFilter, i);
                                    
                                    linkEntitiesFilters filter = new linkEntitiesFilters
                                    {
                                        linkEntitiesFiltersFilterType = linkEntityFilter.Attributes["type"].InnerText.ToUpper(),
                                        linkEntitiesFiltersLinkEntityAttribute = linkEntityFilter.ChildNodes[i].Attributes["attribute"].InnerText,
                                        linkEntitiesFiltersLinkEntityName = linkEntityFilter.ParentNode.Attributes["name"].InnerText,
                                        linkEntitiesFiltersLinkEntityOperator = convertOperator,
                                        linkEntitiesFiltersLinkEntityValue = inValue
                                    };
                                    linkEntitiesFiltersList.Add(filter);

                                }
                            }
                            else
                            {
                                linkEntitiesFilters filter = new linkEntitiesFilters
                                {
                                    linkEntitiesFiltersFilterType = linkEntityFilter.Attributes["type"].InnerText.ToUpper(),
                                    linkEntitiesFiltersLinkEntityAttribute = linkEntityFilter.ChildNodes[i].Attributes["attribute"].InnerText,
                                    linkEntitiesFiltersLinkEntityName = linkEntityFilter.ParentNode.Attributes["name"].InnerText,
                                    linkEntitiesFiltersLinkEntityOperator = convertOperator,
                                    linkEntitiesFiltersLinkEntityValue = ""
                                };
                                linkEntitiesFiltersList.Add(filter);
                            }
                            
                        }
                    }
                }
            }
            return linkEntitiesFiltersList;
        }

        public List<mainFilters> GetMainFilterList(XmlDocument xmlDoc, Dictionary<string, string> operatorReplacement)
        {
            List<mainFilters> mainFiltersList = new List<mainFilters>();

            List<string> xMinusStatements = Operator.GetxMinusStatements();
            List<string> inStatements = Operator.GetInStatements();
            List<string> inFiscalYearStatements = Operator.GetInFiscalStatements();
            List<string> xPlusStatements = Operator.GetxPlusStatements();
            //get all filters for main entity
            var allMainFilterNodes = xmlDoc.DocumentElement.SelectNodes("/fetch/entity/filter");
            foreach (XmlNode mainFilter in allMainFilterNodes)
            {
                string convertOperator = "";
                if (mainFilter.HasChildNodes)
                {
                    for (int i = 0; i < mainFilter.ChildNodes.Count; i++)
                    {
                        if (mainFilter.ChildNodes[i].Attributes["operator"] != null)
                        {
                            operatorReplacement.TryGetValue(mainFilter.ChildNodes[i].Attributes["operator"].InnerText, out convertOperator);
                        }

                        if (mainFilter.ChildNodes[i].Attributes["value"] != null)
                        {
                            if (xMinusStatements.Exists(x => x.Equals(mainFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                mainFiltersList.Add(new mainFilters(mainFilter.ParentNode.Attributes["name"].InnerText, mainFilter.Attributes["type"].InnerText.ToUpper(), mainFilter.ChildNodes[i].Attributes["attribute"].InnerText, string.Format(convertOperator, "-" + mainFilter.ChildNodes[i].Attributes["value"].InnerText), ""));
                            }
                            else if (xPlusStatements.Exists(x => x.Equals(mainFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                mainFiltersList.Add(new mainFilters(mainFilter.ParentNode.Attributes["name"].InnerText, mainFilter.Attributes["type"].InnerText.ToUpper(), mainFilter.ChildNodes[i].Attributes["attribute"].InnerText, string.Format(convertOperator, "+" + mainFilter.ChildNodes[i].Attributes["value"].InnerText), ""));
                            }
                            else if (inFiscalYearStatements.Exists(x => x.Equals(mainFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                mainFiltersList.Add(new mainFilters(mainFilter.ParentNode.Attributes["name"].InnerText, mainFilter.Attributes["type"].InnerText.ToUpper(), mainFilter.ChildNodes[i].Attributes["attribute"].InnerText, string.Format(convertOperator, mainFilter.ChildNodes[i].Attributes["value"].InnerText), ""));
                            }
                            else
                            {
                                mainFiltersList.Add(new mainFilters(mainFilter.ParentNode.Attributes["name"].InnerText, mainFilter.Attributes["type"].InnerText.ToUpper(), mainFilter.ChildNodes[i].Attributes["attribute"].InnerText, convertOperator, mainFilter.ChildNodes[i].Attributes["value"].InnerText));
                            }
                        }
                        else
                        {
                            if (inStatements.Exists(x => x.Equals(mainFilter.ChildNodes[i].Attributes["operator"].InnerText)))
                            {
                                if (mainFilter.ChildNodes[i].HasChildNodes)
                                {
                                    string inValue = "(";
                                    List<object> inValues = new List<object>();
                                    for (int j = 0; j < mainFilter.ChildNodes[i].ChildNodes.Count; j++)
                                    {
                                        if (mainFilter.ChildNodes[i].ChildNodes[j].LocalName == "value")
                                        {
                                            inValues.Add(mainFilter.ChildNodes[i].ChildNodes[j].InnerText);
                                        }
                                    }
                                    int valueCounter = 1;
                                    foreach (var v in inValues)
                                    {
                                        inValue += v;
                                        if (valueCounter < inValues.Count)
                                            inValue += ",";

                                        valueCounter++;
                                    }
                                    inValue += ")";
                                    mainFiltersList.Add(new mainFilters(mainFilter.ParentNode.Attributes["name"].InnerText, mainFilter.Attributes["type"].InnerText.ToUpper(), mainFilter.ChildNodes[i].Attributes["attribute"].InnerText, convertOperator, inValue));
                                }
                            }
                            else
                            {
                                mainFiltersList.Add(new mainFilters(mainFilter.ParentNode.Attributes["name"].InnerText, mainFilter.Attributes["type"].InnerText.ToUpper(), mainFilter.ChildNodes[i].Attributes["attribute"].InnerText, convertOperator, ""));
                            }
                        }
                    }
                }
            }

            return mainFiltersList;
        }

        private string FindInStatementValue(XmlNode filterNode, int i)
        {
            string inValue = "(";
            List<object> inValues = new List<object>();
            for (int j = 0; j < filterNode.ChildNodes[i].ChildNodes.Count; j++)
            {
                if (filterNode.ChildNodes[i].ChildNodes[j].LocalName == "value")
                {
                    string value = filterNode.ChildNodes[i].ChildNodes[j].InnerText;
                    if(!string.IsNullOrEmpty(value))
                    {
                        var ret = value.All(c => char.IsDigit(c));
                        if(ret)
                        {
                            inValues.Add(value);
                        }
                        else
                        {
                            inValues.Add("'" + value.Replace("{","").Replace("}", "") + "'");
                        }
                    }
                }
            }

            if (inValues.Count == 0)
            {
                throw new Exception("Couldn't find Filter In Statement Values");
            }

            int valueCounter = 1;
            foreach (var v in inValues)
            {
                inValue += v;
                if (valueCounter < inValues.Count)
                    inValue += ",";

                valueCounter++;
            }
            inValue += ")";

            return inValue;
        }
    }
}

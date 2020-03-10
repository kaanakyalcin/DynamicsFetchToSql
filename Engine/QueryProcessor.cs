using Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class QueryProcessor
    {
        public string mainEntity;

        public bool isDistinct;

        public List<string> entityAttributes = new List<string>();

        public List<linkEntitiesAndAttributes> linkEntitiesAndAttributesList = new List<linkEntitiesAndAttributes>();

        public List<linkEntitiesFromTo> linkEntitiesFromToList = new List<linkEntitiesFromTo>();

        public List<linkEntitiesFilters> linkEntitiesFiltersList = new List<linkEntitiesFilters>();

        public List<mainFilters> mainFiltersList = new List<mainFilters>();

        public QueryProcessor(string _mainEntity, List<string> _entityAttributes, List<linkEntitiesFromTo> _linkEntitiesFromToList, List<mainFilters> _mainFiltersList, bool _isDistinct = false)
        {
            mainEntity = _mainEntity;
            entityAttributes = _entityAttributes;
            linkEntitiesFromToList = _linkEntitiesFromToList;
            mainFiltersList = _mainFiltersList;
            isDistinct = _isDistinct;
        }

        public string CreateSqlQuery()
        {
            string query = SetSelectStatement(mainEntity, entityAttributes).ToString();
            query += SetFromStatement(mainEntity);
            query += SetJoinStatement(linkEntitiesFromToList);
            query += SetWhereStatement(mainFiltersList);

            return query;
        }

        public string SetSelectStatement(string mainEntity, List<string> entityAttributes)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");

            if(isDistinct)
            {
                sb.Append("DISTINCT ");
            }

            foreach (string attributes in entityAttributes)
            {
                string tempString = "";
                
                tempString = mainEntity + "." + attributes + ", ";
                
                var lastItem = entityAttributes.Last();

                if (attributes == lastItem)
                {
                    tempString = mainEntity + "." + attributes;
                }

                sb.Append(tempString + " ");
            }

            return sb.ToString();
        }

        public string SetFromStatement(string mainEntity)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FROM ");
            sb.Append(mainEntity);
            sb.Append(" ");

            return sb.ToString();
        }

        public string SetJoinStatement(List<linkEntitiesFromTo> linkEntitiesFromToList)
        {
            List<string> inStatements = Operator.GetInStatements();

            StringBuilder sb = new StringBuilder();
            foreach (linkEntitiesFromTo myStruct in linkEntitiesFromToList)
            {
                sb.Append(myStruct.linkEntityJoinType + " JOIN ");
                sb.Append(myStruct.linkEntityFromToName);
                sb.Append(" ON ");

                string fromString = myStruct.linkEntityName + "." + myStruct.linkEntityTo;

                string toString = myStruct.linkEntityFromToName + "." + myStruct.linkEntityFrom;

                string tempString = fromString + " = " + toString;

                sb.Append(tempString);

                bool writtenANDOnce;

                writtenANDOnce = false;


                foreach (linkEntitiesFilters myFilterstruct in myStruct.linkEntitiesFiltersList)
                {

                    if (myStruct.linkEntityFromToName == myFilterstruct.linkEntitiesFiltersLinkEntityName)
                    {
                        if (writtenANDOnce == false)
                        {
                            sb.Append(" AND ");
                            writtenANDOnce = true;
                        }

                        if (string.IsNullOrEmpty(myFilterstruct.linkEntitiesFiltersLinkEntityValue))
                        {
                            sb.Append(myFilterstruct.linkEntitiesFiltersLinkEntityName + "." + myFilterstruct.linkEntitiesFiltersLinkEntityAttribute + " " + myFilterstruct.linkEntitiesFiltersLinkEntityOperator);
                        }                           
                        else
                        {
                            if (inStatements.Exists(x => x.Equals(myFilterstruct.linkEntitiesFiltersLinkEntityOperator)))
                            {
                                sb.Append(myFilterstruct.linkEntitiesFiltersLinkEntityName + "." + myFilterstruct.linkEntitiesFiltersLinkEntityAttribute + " " + myFilterstruct.linkEntitiesFiltersLinkEntityOperator + myFilterstruct.linkEntitiesFiltersLinkEntityValue);
                            }
                            else
                            {
                                sb.Append(myFilterstruct.linkEntitiesFiltersLinkEntityName + "." + myFilterstruct.linkEntitiesFiltersLinkEntityAttribute + " " + myFilterstruct.linkEntitiesFiltersLinkEntityOperator + " '" + myFilterstruct.linkEntitiesFiltersLinkEntityValue + "' ");
                            }
                        }

                        var lastItem = myStruct.linkEntitiesFiltersList.Where(x => x.linkEntitiesFiltersLinkEntityName.Equals(myStruct.linkEntityFromToName)).Last();

                        if (writtenANDOnce == true && (myFilterstruct.linkEntitiesFiltersLinkEntityValue != lastItem.linkEntitiesFiltersLinkEntityValue || myFilterstruct.linkEntitiesFiltersLinkEntityAttribute != lastItem.linkEntitiesFiltersLinkEntityAttribute || myFilterstruct.linkEntitiesFiltersLinkEntityName != lastItem.linkEntitiesFiltersLinkEntityName))
                        {
                            sb.Append(" " + myFilterstruct.linkEntitiesFiltersFilterType + " ");
                        }
                    }
                }

            }
            return sb.ToString();
        }

        public string SetWhereStatement(List<mainFilters> mainFiltersList)
        {
            StringBuilder sb = new StringBuilder();

            if(mainFiltersList.Count > 0)
            {
                sb.Append(" WHERE ( ");
            }

            List<string> inStatements = Operator.GetInStatements();

            foreach (mainFilters myMainFilter in mainFiltersList)
            {
                var lastItem = mainFiltersList.Last();

                if (myMainFilter.mainFiltersEntityValue == "")
                {
                    sb.Append(myMainFilter.mainFiltersEntityName + "." + myMainFilter.mainFiltersEntityAttribute + " " + myMainFilter.mainFiltersEntityOperator);
                }
                else
                {
                    if (inStatements.Exists(x => x.Equals(myMainFilter.mainFiltersEntityOperator)))
                    {
                        sb.Append(myMainFilter.mainFiltersEntityName + "." + myMainFilter.mainFiltersEntityAttribute + " " + myMainFilter.mainFiltersEntityOperator + " " + myMainFilter.mainFiltersEntityValue);
                    }
                    else
                    {
                        sb.Append(myMainFilter.mainFiltersEntityName + "." + myMainFilter.mainFiltersEntityAttribute + " " + myMainFilter.mainFiltersEntityOperator + " '" + myMainFilter.mainFiltersEntityValue + "' ");
                    }
                }

                if (myMainFilter.mainFiltersEntityAttribute != lastItem.mainFiltersEntityAttribute || myMainFilter.mainFiltersEntityOperator != lastItem.mainFiltersEntityOperator || myMainFilter.mainFiltersEntityValue != lastItem.mainFiltersEntityValue)
                {
                    sb.Append(" " + myMainFilter.mainFiltersFilterType + " ");
                }
            }

            if (mainFiltersList.Count > 0)
            {
                sb.Append(" ) ");
            }

            return sb.ToString();

        }

        public string SetSpecialStatement(bool isSpecial)
        {
            StringBuilder sb = new StringBuilder();

            if (isSpecial)
            {
                string specailAttribute = mainEntity + "." + mainEntity + "id";
                specailAttribute += " = @CustomerId";

                if (mainFiltersList.Count > 0)
                {
                    sb.Append(" and " + specailAttribute);
                }
                else
                {
                    sb.Append(" WHERE " + specailAttribute);
                }
            }

            return sb.ToString();
        }
    }
}

using Engine;
using Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace Api.Controllers
{
    public class FetchController : ApiController
    {
        [HttpGet, ActionName("GetSqlQuery")]
        public ReturnObject GetSqlQuery(string fetchXML)
        {
            ReturnObject result = new ReturnObject();
            string sqlQuery = "";

            try
            {
                Validation validation = new Validation();

                if (validation.CheckFetchText(fetchXML))
                {
                    XmlDocument xmlDoc = validation.CreateFetchXml(fetchXML);

                    XMLProcessor processor = new XMLProcessor();

                    Dictionary<string, string> operatorReplacement = Operator.GetList();

                    string entityName = processor.GetEntityName(xmlDoc);
                    bool isDistinct = processor.isDistinct(xmlDoc);
                    List<string> entityAttributeList = processor.GetEntityAttributes(xmlDoc);
                    List<linkEntitiesFromTo> linkEntitiesList = processor.GetLinkEntityList(xmlDoc, entityName, operatorReplacement);
                    List<mainFilters> mainFilterList = processor.GetMainFilterList(xmlDoc, operatorReplacement);

                    QueryProcessor queryProcessor = new QueryProcessor(entityName,
                                                                       entityAttributeList,
                                                                       linkEntitiesList,
                                                                       mainFilterList,
                                                                       isDistinct
                                                                       );

                    sqlQuery = queryProcessor.CreateSqlQuery();

                }
            }
            catch (Exception ex)
            {
                sqlQuery = "";
                result.Exception = ex.Message;
                result.IsHasError = true;
            }

            result.SqlQuery = sqlQuery;

            return result;

        }

    }
}

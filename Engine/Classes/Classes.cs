using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Classes
{
    public struct linkEntitiesAndAttributes
    {
        public string linkEntityData { get; private set; }
        public string linkEntityAttributeData { get; private set; }

        public linkEntitiesAndAttributes(string linkEntityValue, string linkEntityAttributeValue) : this()
        {
            linkEntityData = linkEntityValue;
            linkEntityAttributeData = linkEntityAttributeValue;
        }
    }

    public class linkEntitiesFromTo
    {
        public string linkEntityName { get; set; }
        public string linkEntityFromToName { get; set; }
        public string linkEntityFrom { get; set; }
        public string linkEntityTo { get; set; }
        public string linkEntityJoinType { get; set; }
        public string linkEntityAlias { get; set; }
        public List<linkEntitiesFilters> linkEntitiesFiltersList { get; set; }
    }

    public class linkEntitiesFilters
    {
        public string linkEntitiesFiltersLinkEntityName { get; set; }
        public string linkEntitiesFiltersFilterType { get; set; }
        public string linkEntitiesFiltersLinkEntityAttribute { get; set; }
        public string linkEntitiesFiltersLinkEntityOperator { get; set; }
        public string linkEntitiesFiltersLinkEntityValue { get; set; }
    }

    public struct mainFilters
    {
        public string mainFiltersEntityName { get; private set; }
        public string mainFiltersFilterType { get; private set; }
        public string mainFiltersEntityAttribute { get; private set; }
        public string mainFiltersEntityOperator { get; private set; }
        public string mainFiltersEntityValue { get; private set; }

        public mainFilters(string mainFiltersEntityNameValue, string mainFiltersFilterTypeValue, string mainFiltersEntityAttributeValue, string mainFiltersEntityOperatorValue, string mainFiltersEntityValueValue) : this()
        {
            mainFiltersEntityName = mainFiltersEntityNameValue;
            mainFiltersFilterType = mainFiltersFilterTypeValue;
            mainFiltersEntityAttribute = mainFiltersEntityAttributeValue;
            mainFiltersEntityOperator = mainFiltersEntityOperatorValue;
            mainFiltersEntityValue = mainFiltersEntityValueValue;
        }

    }

    public class ReturnObject
    {
        public string SqlQuery { get; set; }
        public bool IsHasError { get; set; }
        public string Exception { get; set; }
    }


}

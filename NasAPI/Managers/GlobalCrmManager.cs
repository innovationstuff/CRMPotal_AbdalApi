using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using NasAPI.Models;
using NasAPI.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace NasAPI.Managers
{
    public class GlobalCrmManager : IDisposable
    {
        public IEnumerable<string> GetRequiredFieldsNamesForEntity(string entityName)
        {
            RetrieveEntityRequest req = new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.Attributes,
                RetrieveAsIfPublished = true,
                LogicalName = entityName,

            };

            RetrieveEntityResponse response = GlobalCode.Service.Execute(req) as RetrieveEntityResponse;
            return response.EntityMetadata.Attributes.Where(a => a.RequiredLevel?.Value == AttributeRequiredLevel.ApplicationRequired).Select(a => a.SchemaName.ToLower()).ToList();
        }

        public IEnumerable<string> GetEmptyFieldsNamesForRecord(string entityName, string searchColumn, string searchValue, string[] columns = null)
        {
            QueryExpression query = new QueryExpression(entityName);

            if (columns != null)
                query.ColumnSet = new ColumnSet(columns);
            else
                query.ColumnSet = new ColumnSet(true);

            query.Criteria.AddCondition(new ConditionExpression(searchColumn, ConditionOperator.Equal, searchValue));
            var record = GlobalCode.Service.RetrieveMultiple(query);
            if (record.Entities == null || record.Entities.Count == 0)
                throw new Exception("No record");



            var emptyData = record.Entities.FirstOrDefault().Attributes.Where(a => a.Value == null || string.IsNullOrEmpty(a.Value.ToString())).Select(a => a.Key.ToLower());
            if (columns != null)
                emptyData = emptyData.Union(columns.Except(record.Entities.FirstOrDefault().Attributes.Keys.Select(k => k.ToLower())));

            return emptyData;
        }

        public IEnumerable<string> GetAllEmptyRequiredFieldsNamesForRecord(string entityName, string searchColumn, string searchValue)
        {
            return GetEmptyFieldsNamesForRecord(entityName, searchColumn, searchValue, GetRequiredFieldsNamesForEntity(entityName).ToArray());
        }

        /// <summary>
        /// modified by m.abdelrahman remove lang segment from query
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="optionSetFieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>

        public IEnumerable<BaseOptionSet> GetOptionSetLookup(string entityName, string optionSetFieldName, UserLanguage lang = UserLanguage.English, string concattext = "")
        {
            //int langId = (lang == UserLanguage.English ? 1033 : 1025);
            int langId, oppositeLangId;
            switch (lang)
            {
                case UserLanguage.Arabic:
                    langId = 1025;
                    oppositeLangId = 1033;
                    break;
                default:
                    langId = 1033;
                    oppositeLangId = 1025;
                    break;
            }


            string query = String.Format(@";With main as(
                                            Select	distinct [Value] as Text,  AttributeValue as Value
                                            From	    StringMap s Inner Join EntityLogicalView e
                                       		            ON s.ObjectTypeCode = e.ObjectTypeCode
                                            Where	e.[Name]='{0}' and s.AttributeName = '{1}' and LangId = {2}
                                            ) ,
                                            
                                            Opposite as (
                                            select	distinct  opposite_s.[Value] as Text,  opposite_s.AttributeValue as Value 
                                            From	    StringMap opposite_s Inner Join EntityLogicalView e
                                                         ON opposite_s.ObjectTypeCode = e.ObjectTypeCode
                                            Where	e.[Name]='{3}' and opposite_s.AttributeName = '{4}' and opposite_s.LangId = {5}
                                            )

                                            select distinct ISNULL( main.Text , opposite.Text) as  Text, ISNULL(main.Value,opposite.Value) as Value
                                            from main full outer join Opposite on main.Value = Opposite.Value",

                                       entityName, optionSetFieldName, langId, entityName, optionSetFieldName, oppositeLangId);

            var dt = CRMAccessDB.SelectQ(query).Tables[0];
            return dt.AsEnumerable().Select(row => new BaseOptionSet() { Key = (int)row["Value"], Value = row["Text"].ToString() + concattext }).OrderBy(a => a.Key);
        }

        public IEnumerable<BaseQuickLookup> GetQuickLookup(string entityName, string idFieldName, string textFieldName, string otherTextFieldIfNull = null, string filterWhereCondition = "")
        {
            otherTextFieldIfNull = String.IsNullOrEmpty(otherTextFieldIfNull) ? textFieldName : otherTextFieldIfNull;

            string query = String.Format(@"Select IsNull({0},{1}) as Text, {2} as Value
                                       From	    {3}  {4}",
                                       textFieldName, otherTextFieldIfNull, idFieldName, entityName, filterWhereCondition);

            var dt = CRMAccessDB.SelectQ(query).Tables[0];
            return dt.AsEnumerable().Select(row => new BaseQuickLookup() { Key = row["Value"].ToString(), Value = row["Text"].ToString() });
        }

        public virtual Entity GetCrmEntity(string entityName, string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid)) return null;
            Entity entity = GlobalCode.Service.Retrieve(entityName, guid, new ColumnSet(true));
            return entity;
        }

        public virtual Entity GetCrmEntity(string entityName, string id, string[] columns)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid)) return null;
            Entity entity = GlobalCode.Service.Retrieve(entityName, guid, new ColumnSet(columns));
            return entity;
        }

        public void Dispose()
        {

        }
    }
}
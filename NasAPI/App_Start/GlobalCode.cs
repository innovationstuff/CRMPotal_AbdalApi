using System;
using System.Configuration;
using System.Data;
using System.ServiceModel.Description;
using System.Web.Mail;
using System.Xml;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System.Globalization;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;
//namespace EarlyPaymentPlugin


public class GlobalCode
{
    //#region AjeerClass
    //public static class Ajeer
    //{

    //    public static ContractHeader CreateContract(string userID, Guid projectID, string laborOfficeNumber, string sequenceNumber, Laborers laborers, bool isBulkChange)
    //    {
    //        //int allowedNumber = Convert.ToInt32(GetAllowedNumberforContract(projectID, laborOfficeNumber, sequenceNumber));
    //        //if (allowedNumber >= 0)
    //        //{
    //        //    if (laborers.Count > allowedNumber)
    //        //    {
    //        //        int removed = laborers.Count - allowedNumber;
    //        //        for (int i = 0; i < removed; i++)
    //        //        {
    //        //            laborers.RemoveAt(0);

    //        //        }
    //        //    }
    //        //}
    //        AjeerContractServiceClient client = new AjeerContractServiceClient();


    //        //Ajeer Establishment
    //        AjeerEstablishment ajeerEst = new AjeerEstablishment();
    //        ajeerEst.BranchLaborOffice = int.Parse(ConfigurationManager.AppSettings["BranchLaborOffice"]);
    //        ajeerEst.BranchSequenceNumber = int.Parse(ConfigurationManager.AppSettings["BranchSequenceNumber"]);
    //        ajeerEst.UserName = ConfigurationManager.AppSettings["EstablishmentUserName"]; //ajeer establishment username
    //        ajeerEst.Password = ConfigurationManager.AppSettings["EstablishmentPassword"]; //ajeer establishment password

    //        EstablishmentInfo estInfo = new EstablishmentInfo();
    //        estInfo.LaborOfficeId = int.Parse(laborOfficeNumber);
    //        estInfo.SequenceNumber = int.Parse(sequenceNumber);

    //        //get the project data first to check if the contract already exist didn't make it again





    //        //textMessage = "Create Contract Data..";
    //        ContractHeader contractHedader = new ContractHeader();
    //        //contractHedader.ContractNumber="";
    //        contractHedader.ContractStatus = ContractStatus.Active;
    //        contractHedader.StartDate = DateTime.Now.AddDays(1);
    //        contractHedader.EndDate = DateTime.Now.AddYears(2);
    //        //contractHedader.RefNumber="";




    //        ContractInfo contractInfo = new ContractInfo();
    //        contractInfo.Header = contractHedader;
    //        contractInfo.Laborers = laborers;


    //        ContractMessage contractMessage = new ContractMessage(ajeerEst, estInfo, contractInfo);
    //        contractMessage.ajeerEstablishment = ajeerEst;
    //        contractMessage.establishmentInfo = estInfo;
    //        contractMessage.contractInfo = contractInfo;



    //        try
    //        {
    //            //textMessage = "Calling the Ajeer service..";
    //            ContractActionResponse actionResponse = client.ContractNotice(contractMessage);


    //            ContractHeader resultContractHedader = actionResponse.contractHeader;

    //            ////set controls with the new data
    //            //lblContractNumber.Text = resultContractHedader.ContractNumber;
    //            //lblContractStatus.Text = resultContractHedader.ContractStatus.ToString();
    //            //lblEndDate.Text = resultContractHedader.EndDate.ToShortDateString();
    //            //lblStartDate.Text = resultContractHedader.StartDate.ToShortDateString();
    //            //lblRefNumber.Text = resultContractHedader.RefNumber;


    //            //update project info
    //            //textMessage = "Updating the project Info..";
    //            Entity projectEntity = GlobalCode.Service.Retrieve("new_project", projectID, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
    //            if (projectEntity != null)
    //            {
    //                //change all ajeer data for the project

    //                //projectEntity.Attributes["new_contractnumber"] = resultContractHedader.ContractNumber;
    //                //projectEntity.Attributes["new_ajeercontractstatus"] = new OptionSetValue((int)resultContractHedader.ContractStatus);
    //                ////   projectEntity.Attributes["new_ajeerstartdate"] = resultContractHedader.StartDate.ToShortDateString();
    //                ////  projectEntity.Attributes["new_ajeerenddate"] = resultContractHedader.EndDate.ToShortDateString();
    //                //projectEntity.Attributes["new_ajeerrefnumber"] = resultContractHedader.RefNumber;
    //                //GlobalCode.Service.Update(projectEntity);


    //                for (int i = 0; i < contractInfo.Laborers.Count; i++)
    //                {

    //                    Entity Employee = GlobalCode.GetOneEntityBy("new_employee", "new_idnumber", contractInfo.Laborers[i].IdNo.ToString());
    //                    Employee.Attributes.Clear();

    //                    Employee["new_ajeerpostdate"] = DateTime.Now;
    //                    Employee["new_ajeercontractnumber"] = resultContractHedader.ContractNumber;
    //                    Employee["new_ispostedtoajeer"] = new OptionSetValue(2);
    //                    Employee["new_paygroup"] = laborOfficeNumber + "-" + sequenceNumber;
    //                    GlobalCode.Service.Update(Employee);


    //                }

    //                //Create Log Record
    //                if (!string.IsNullOrEmpty(userID))
    //                    GlobalCode.AgeerLogRecord(userID, 1, resultContractHedader.ContractNumber, "", projectID.ToString());
    //            }


    //        }
    //        catch (Exception exc)
    //        {

    //            //textMessage = "لقد حدث خطأ أثناء الاتصال بالوزارة";
    //            //textMessage += "<br/>" + exc.Message;

    //            string ids = GetIdsWithProblems(laborers, exc.Message);


    //            //create ajeer fault entity
    //            Entity ajeerFault = new Entity("new_ajeerfault");
    //            ajeerFault["new_project"] = new EntityReference("new_project", projectID);
    //            ajeerFault["new_date"] = DateTime.Now;
    //            ajeerFault["new_error"] = exc.Message;
    //            ajeerFault["new_automatic"] = isBulkChange;
    //            ajeerFault["new_iqama"] = ids;
    //            ajeerFault["new_empcount"] = laborers.Count;
    //            GlobalCode.Service.Create(ajeerFault);
    //        }
    //        return null;
    //    }
    //    private static string GetIdsWithProblems(Laborers laborers, string message)
    //    {
    //        string ids = string.Empty;
    //        //int index = message.IndexOf("رقم هوية العامل");
    //        //string IDNumber = message.Substring(index + 15, 11).Trim();
    //        //txtIDs.Text = txtIDs.Text.Replace(IDNumber, String.Empty).Replace(",,", ",");
    //        //string[] ids = txtIDs.Text.Split(',');
    //        //string strIds = txtIDs.Text;

    //        for (int i = 0; i < laborers.Count; i++)
    //        {
    //            if (message.Contains(laborers[i].IdNo))
    //            {
    //                ids += laborers[i].IdNo + ",";
    //            }

    //        }
    //        return ids.Trim(',');
    //    }
    //    public static double GetAllowedNumberforContract(Guid projectID, string laborOfficeNumber, string sequenceNumber)
    //    {
    //        double allowedNumber = 0;
    //        EstablishmentInquiryServiceClient client = new EstablishmentInquiryServiceClient();
    //        AjeerEstablishment ajeerEst = new AjeerEstablishment();
    //        ajeerEst.BranchLaborOffice = int.Parse(ConfigurationManager.AppSettings["BranchLaborOffice"]);
    //        ajeerEst.BranchSequenceNumber = int.Parse(ConfigurationManager.AppSettings["BranchSequenceNumber"]);
    //        ajeerEst.UserName = ConfigurationManager.AppSettings["EstablishmentUserName"]; //ajeer establishment username
    //        ajeerEst.Password = ConfigurationManager.AppSettings["EstablishmentPassword"]; //ajeer establishment password
    //        EstablishmentInfo estInfo = new EstablishmentInfo();
    //        Laborer labor = new Laborer();





    //        try
    //        {
    //            //LaborerQueryMessage 
    //            if (laborOfficeNumber == "" || sequenceNumber == "")
    //            {
    //                throw new ArgumentException("رقم المنشاة غير موجود على النظام");
    //            }
    //            estInfo.LaborOfficeId = int.Parse(laborOfficeNumber);
    //            estInfo.SequenceNumber = int.Parse(sequenceNumber);
    //            EstablishmentQueryMessage estQuery = new EstablishmentQueryMessage(ajeerEst, estInfo);

    //            EstablishmentQueryResponse estResponse = client.GetEstablishmentInfo(estQuery);


    //            allowedNumber = estResponse.establishmentInfo.Quota;



    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //        return allowedNumber;

    //    }

    //}
    //#endregion

    public static IOrganizationService GetCRMService(string ServerURL, string Organization, string UserName, string Password, string DomainName, string UserGuid)
    {
        // CRMService.OrganizationServiceClient client = new OrganizationServiceClient();
        //  return client;
        ClientCredentials credentials = new ClientCredentials();
        credentials.Windows.ClientCredential = new System.Net.NetworkCredential(UserName, Password, DomainName);
        Uri organizationUri = new Uri(ServerURL + "/" + Organization + "/XRMServices/2011/Organization.svc");
        Uri homeRealmUri = null;
        OrganizationServiceProxy orgService = new OrganizationServiceProxy(organizationUri, homeRealmUri, credentials, null);
        if (!string.IsNullOrEmpty(UserGuid))
        {
            //orgService.CallerId = new Guid(UserGuid);

        }


        IOrganizationService _service = (IOrganizationService)orgService;
        return _service;
    }
    public static IOrganizationService GetCRMService(string ServerURL, string Organization, string UserName, string Password, string DomainName)
    {
        return GetCRMService(ServerURL, Organization, UserName, Password, DomainName, "");
    }
    static IOrganizationService clientService;
    public static IOrganizationService Service
    {
        get
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string serviceSessionId = "Global";
            string UserGuid = "";
            if (context.Request != null)
            {
                // if (context.Session["UserGuid"] == null)
                {
                    if (!string.IsNullOrEmpty(context.Request.QueryString["UID"]))
                    {
                        Guid UserID = new Guid(context.Request.QueryString["UID"]);

                        UserGuid = UserID.ToString();
                        serviceSessionId = UserID.ToString();
                    }
                }


            }

            // UserGuid="7C92A4B5-7FF8-E411-80C2-0050568B1DBC';
            // if (context.Session[serviceSessionId] == null)
            {
                string CRMServerURL = ConfigurationManager.AppSettings["serverUrl"];
                string CRMOrganiza = ConfigurationManager.AppSettings["organization"];
                string CRMUserName = ConfigurationManager.AppSettings["username"];
                string CRMPassword = ConfigurationManager.AppSettings["password"];
                CRMPassword = DecryptText(CRMPassword, "Ahmed");
                string CRMDomain = ConfigurationManager.AppSettings["domain"];



                //context.Session[serviceSessionId] =

               return  GetCRMService(CRMServerURL, CRMOrganiza, CRMUserName, CRMPassword, CRMDomain);

            }
            //return (Microsoft.Xrm.Sdk.IOrganizationService)context.Session[serviceSessionId];
        }
    }
    public static void ExecuteWorkflow(string entityID, string workflowID)
    {

        ExecuteWorkflowRequest request = new ExecuteWorkflowRequest();

        //Assign the ID of the workflow you want to execute to the request.        
        request.WorkflowId = new Guid(workflowID);

        //Assign the ID of the entity to execute the workflow on to the request.
        request.EntityId = new Guid(entityID);

        // Execute the workflow.
        ExecuteWorkflowResponse response = (ExecuteWorkflowResponse)GlobalCode.Service.Execute(request);
    }
    public static EntityCollection GetEntitiesBy(string entityName, string SearchColumn, object searchValue)
    {
        return GetEntitiesBy(entityName, SearchColumn, searchValue, true);

    }
    public static EntityCollection GetEntitiesBy(string entityName, string SearchColumn, object searchValue, bool AllColumns)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(AllColumns);
        query.Criteria.AddCondition(new ConditionExpression(SearchColumn, ConditionOperator.Equal, searchValue));
        return Service.RetrieveMultiple(query);

    }
    public static EntityCollection GetEntitiesBy(string entityName, string SearchColumn, object searchValue, string columnNotIn, object columnNotInValue)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(true);
        query.Criteria.AddCondition(new ConditionExpression(SearchColumn, ConditionOperator.Equal, searchValue));
        if (!string.IsNullOrEmpty(columnNotIn) && columnNotInValue != null && !string.IsNullOrEmpty(columnNotInValue.ToString()))
        {
            query.Criteria.AddCondition(new ConditionExpression(columnNotIn, ConditionOperator.NotIn, columnNotInValue));

        }
        return Service.RetrieveMultiple(query);

    }

    public static EntityCollection GetEntitiesIn(string entityName, string SearchColumn, params object[] searchValues)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(true);
        query.Criteria.AddCondition(new ConditionExpression(SearchColumn, ConditionOperator.In, searchValues));
        return Service.RetrieveMultiple(query);

    }

    public static void UpdateBulk(string EntityName, string[] ColumnsToUpdate, object[] Values, string FilterByColumn, string SearchInValues)
    {
        string[] obSearch = SearchInValues.Replace("\r\n", ",").Replace("\r", ",").Replace("\n", ",").Split(',');
        QueryExpression query = new QueryExpression(EntityName);
        query.ColumnSet = new ColumnSet(ColumnsToUpdate);
        query.Criteria.AddCondition(new ConditionExpression(FilterByColumn, ConditionOperator.In, obSearch));
        EntityCollection rows = GlobalCode.Service.RetrieveMultiple(query);
        for (int i = 0; i < rows.Entities.Count; i++)
        {
            for (int j = 0; j < Values.Length; j++)
            {
                rows[i][ColumnsToUpdate[j]] = Values[j];
            }
            GlobalCode.Service.Update(rows[i]);
        }
    }
    public static EntityCollection GetEntitiesBy(string entityName, string[] SearchColumn, object[] searchValue)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(true);
        for (int i = 0; i < SearchColumn.Length; i++)
        {
            if (searchValue[i] is object[])
            {
                query.Criteria.AddCondition(new ConditionExpression(SearchColumn[i], ConditionOperator.In, (object[])searchValue[i]));
            }
            else
            {
                query.Criteria.AddCondition(new ConditionExpression(SearchColumn[i], ConditionOperator.Equal, searchValue[i]));
            }
        }

        return Service.RetrieveMultiple(query);

    }
    public static EntityCollection IsExist(string entityName, string[] SearchColumn, object[] searchValue)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(false);
        for (int i = 0; i < SearchColumn.Length; i++)
        {
            if (searchValue[i] is object[])
            {
                query.Criteria.AddCondition(new ConditionExpression(SearchColumn[i], ConditionOperator.In, (object[])searchValue[i]));
            }
            else
            {
                query.Criteria.AddCondition(new ConditionExpression(SearchColumn[i], ConditionOperator.Equal, searchValue[i]));
            }
        }

        return Service.RetrieveMultiple(query);

    }
    public static Entity GetOneEntityBy(string entityName, string SearchColumn, object searchValue)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(true);
        query.Criteria.AddCondition(new ConditionExpression(SearchColumn, ConditionOperator.Equal, searchValue));
        EntityCollection entities = Service.RetrieveMultiple(query);
        if (entities.Entities.Count > 0)
        {
            return entities.Entities[0];
        }
        return null;
    }
    public static object GetColumnValueFromEntity(string entityName, string columnName, string SearchColumn, object searchValue)
    {
        QueryExpression query = new QueryExpression(entityName);
        query.ColumnSet = new ColumnSet(new string[] { columnName });
        query.Criteria.AddCondition(new ConditionExpression(SearchColumn, ConditionOperator.Equal, searchValue));
        EntityCollection entities = Service.RetrieveMultiple(query);
        if (entities.Entities.Count > 0)
        {
            return entities.Entities[0][columnName];
        }
        return null;
    }
    public static Guid LookUp(string EntityName, string columnName, string nameValue)
    {
        Entity entity = GlobalCode.GetOneEntityBy(EntityName, columnName, nameValue);
        if (entity != null)
        {
            return entity.Id;
        }
        else
        {
            entity = new Entity(EntityName);
            entity[columnName] = nameValue;
            Guid id = GlobalCode.Service.Create(entity);
            return id;
        }
    }
    public static Guid LookUp(string name, string entityName)
    {
        Entity entity = GlobalCode.GetOneEntityBy(entityName, "new_name", name);
        if (entity != null)
        {
            return entity.Id;
        }
        else
        {
            entity = new Entity(entityName);
            entity["new_name"] = name;
            Guid id = GlobalCode.Service.Create(entity);
            return id;
        }
    }
    public static DateTime GetDate(Entity entity, string columnName)
    {
        if (entity.Contains(columnName))
        {
            return (DateTime)entity[columnName];
        }
        else
            return new DateTime();
    }
    public static string GetStringValue(Entity entity, string columnName)
    {
        if (entity.Contains(columnName))
        {
            return entity[columnName].ToString();
        }
        else
            return "";
    }
    public static int GetIntValue(Entity entity, string columnName)
    {
        if (entity.Contains(columnName))
        {
            return int.Parse(entity[columnName].ToString());
        }
        else
            return 0;
    }
    public static string GetCodeValue(Entity srcEntity, string RefColumn, string entityName, string CodeColumn)
    {
        if (srcEntity.Contains(RefColumn))
        {
            Entity entity = GlobalCode.Service.Retrieve(entityName, ((EntityReference)srcEntity[RefColumn]).Id, new Microsoft.Xrm.Sdk.Query.ColumnSet());
            if (entity.Contains(CodeColumn))
            {
                return entity[CodeColumn].ToString();
            }
            else return "";
        }
        else return "";
    }
    public static string ConvertDateToHijri(DateTime date)
    {
        CultureInfo arSA = new CultureInfo("ar-SA");
        arSA.DateTimeFormat.Calendar = new UmAlQuraCalendar();
        return date.ToString("dd/MM/yyyy", arSA);
    }
    public static DateTime? ConvertHijriToDate(string HijriDate)
    {
        CultureInfo arCI = new CultureInfo("ar-SA");
        string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
                "dd/MM/yyyy","d/M/yyyy",
                "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
                "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
                "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
                "yyyy M d","dd MM yyyy","d M yyyy",
                "dd M yyyy","d MM yyyy"};
        string hijri = HijriDate;

        DateTime tempDate = DateTime.ParseExact(hijri, allFormats, arCI.DateTimeFormat, DateTimeStyles.AllowInnerWhite);
        return tempDate;

    }

    public static string InsertRepeatedRow(string xmlData, string WordInRowToReplace, System.Data.DataTable dtData)
    {

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlData);
        XmlNode trStart = null;
        XmlNode Table = null;
        XmlNode trEnd = null;
        XmlNodeList nodeList = doc.GetElementsByTagName("w:t");
        for (int i = 0; i < nodeList.Count; i++)
        {

            if (nodeList[i].InnerText.Contains(WordInRowToReplace))
            {
                //this is recursive function is used to search for parent node that contain tr
                //in workd document 
                trStart = GetParent(nodeList[i], "w:tr");
                Table = GetParent(nodeList[i], "w:tbl");
            }

        }
        //remove the row 
        Table.RemoveChild(trStart);

        //  XmlDataDocument docstart = new XmlDataDocument();
        //docstart.LoadXml(fileStart);
        XmlNode currentNode = trStart;
        string replaceText = trStart.OuterXml;
        System.Text.StringBuilder rows = new System.Text.StringBuilder();
        string tempRow = "";
        for (int rowIndex = 0; rowIndex < dtData.Rows.Count; rowIndex++)
        {
            //take a copy of temprow and replace by row values
            XmlNode node = trStart.CloneNode(true);
            for (int i = 0; i < dtData.Columns.Count; i++)
            {


                tempRow = node.InnerXml;
                tempRow = tempRow.Replace("SN", (rowIndex + 1).ToString());
                tempRow = tempRow.Replace("" + dtData.Columns[i].ColumnName, dtData.Rows[rowIndex][i].ToString().Replace(".0000000000", "").Replace("00000000", "").Replace(".00000000", "").Replace(".0000", "")).Replace("&", "").Replace(".00", "");
                node.InnerXml = tempRow;
            }



            Table.AppendChild(node);
        }



        return doc.OuterXml;

    }
    public static XmlNode GetParent(XmlNode child, string tagName)
    {
        if (child.ParentNode != null)
        {
            if (child.ParentNode.Name == tagName)
                return child.ParentNode;
            else
                return GetParent(child.ParentNode, tagName);
        }
        else
            return null;
    }

    public static string GetTeamEmails(string teamID)
    {
        string emails = "";


        string GetTeamEmail = @"SELECT distinct SystemUserBase.InternalEMailAddress
                         FROM TeamMembership INNER JOIN
                         TeamBase ON TeamMembership.TeamId = TeamBase.TeamId INNER JOIN
                         SystemUserBase ON TeamMembership.SystemUserId = SystemUserBase.SystemUserId";

        GetTeamEmail += " where TeamBase.TeamId = '" + teamID + "'";

        DataTable dt = CRMAccessDB.SelectQ(GetTeamEmail).Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["internalemailaddress"].ToString().Equals(""))
            {
                continue;
            }
            else
            {
                emails += dt.Rows[i]["internalemailaddress"].ToString() + ",";
            }
        }
        return emails;
    }

    public static void CreateManyToMany(string RelationShipName, string MasterEntityName, Guid MasterEntityId, string DetailEntityName, Guid DetailsEntityID)
    {
        EntityReferenceCollection collections = new EntityReferenceCollection();
        collections.Add(new EntityReference(DetailEntityName, DetailsEntityID));
        GlobalCode.Service.Associate(MasterEntityName, MasterEntityId, new Relationship(RelationShipName), collections);

    }


    public static string ReplaceTempPassword(string FileText, string hash, string salt, string ProviderType, string AlgorithmSid, string userID/*, string ReadPer*/)
    {

        return FileText;
        //if (IsUserHasEditTemplate(userID) /*|| ReadPer.Equals("2")*/)
        //    return RemovePassword(FileText);
        //else
        //{
        //    XmlDocument xml = new XmlDocument();
        //    xml.LoadXml(FileText);
        //    if (xml.GetElementsByTagName("w:documentProtection")[0] != null)
        //    {
        //        XmlNode node = xml.GetElementsByTagName("w:documentProtection")[0];
        //        if (node.Attributes["w:hash"] != null)
        //        {
        //            node.Attributes["w:hash"].Value = hash;
        //            node.Attributes["w:salt"].Value = salt;
        //            node.Attributes["w:cryptProviderType"].Value = ProviderType;
        //            node.Attributes["w:cryptAlgorithmSid"].Value = AlgorithmSid;
        //            //return xml.OuterXml;
        //        }

        //        return xml.OuterXml;
        //    }
        //    else
        //    {
        //        XmlNode SettingsNode = xml.GetElementsByTagName("w:settings")[0];
        //        string StringProtection = "<w:documentProtection w:edit='readOnly' w:enforcement='1' w:cryptProviderType='" + ProviderType
        //            + "' w:cryptAlgorithmClass='hash' w:cryptAlgorithmType='typeAny' w:cryptAlgorithmSid='" + AlgorithmSid + "' w:cryptSpinCount='100000' w:hash='" + hash + "' w:salt='" + salt + "' />";
        //        XmlNode Protection = xml.CreateTextNode(StringProtection);
        //        SettingsNode.InnerXml = StringProtection;
        //        //SettingsNode.AppendChild(Protection);
        //        //xml.ParentNode.AppendChild(SettingsNode);
        //        return xml.OuterXml;
        //    }

        //}
    }
    public static string GetRecordUrl(string serverIP, string entityName, string id)
    {
        object typecode = CRMAccessDB.ExecuteScalar("select ObjectTypeCode from MetadataSchema.Entity where Name='" + entityName + "'");
        string url = "http://" + serverIP + ":81/AMRCRM/main.aspx?etc=10050&extraqs=%3f_gridType%3d10050%26etc%3d10050%26id%3d%257b8EE8057D-CEC7-E411-B96D-005056866836%257d%26pagemode%3diframe%26preloadcache%3d1427006141976%26rskey%3d857033331&pagetype=entityrecord";
        if (typecode.ToString() != "")
        {

            url = url.Replace("10050", typecode.ToString());
            id = id.Replace("{", "").Replace("}", "");
            url = url.Replace("8EE8057D-CEC7-E411-B96D-005056866836", id);
            //url = Uri.EscapeDataString(url);
            return url;


        }
        return url;
    }

    public static string RemovePassword(string fileText)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(fileText);
        XmlNode node = xml.GetElementsByTagName("w:documentProtection")[0];
        if (node != null)
            node.ParentNode.RemoveChild(node);
        return xml.OuterXml;

    }
    public static bool IsUserHasEditTemplate(string userID)
    {
        string sql = @"
SELECT     count(TeamMembership.systemuserid)
FROM         TeamMembership INNER JOIN
                      TeamBase ON TeamMembership.TeamId = TeamBase.TeamId
                      where TeamBase.teamid='2A837651-4D5F-E411-9F86-005056866836'
                      and systemuserid='@userID'";
        sql = sql.Replace("@userID", userID);
        int count = Convert.ToInt16(CRMAccessDB.ExecuteScalar(sql));
        return count > 0;
    }

    public static string Permission_R_W(string FileText, string hash, string salt, string ProviderType, string AlgorithmSid, string UserID, string TemplateCode)
    {
        string UserPer = @"SELECT
	                    mw_sysdocumentBase.mw_name, mw_sysdocumentBase.mw_documentname,
	                    mw_userdocumentBase.mw_userid, mw_userdocumentBase.mw_teamid, 
	                    mw_userdocumentBase.mw_permissiontype		
                    FROM 
	                    mw_sysdocumentBase INNER JOIN
	                    mw_userdocumentBase ON mw_sysdocumentBase.mw_userdocument = mw_userdocumentBase.mw_userdocumentId
	
                    Where mw_userid = '" + UserID + "' and mw_sysdocumentBase.mw_name = '" + TemplateCode + "'";
        DataTable dt = CRMAccessDB.SelectQ(UserPer).Tables[0];

        if (dt.Rows.Count > 0)
        {
            return FileText = ReplaceTempPassword(FileText, ConfigurationManager.AppSettings["MSWordhash"], ConfigurationManager.AppSettings["MSWordsalt"],
                                          ConfigurationManager.AppSettings["MSWordProviderType"], ConfigurationManager.AppSettings["MSWordAlgorithmSid"], UserID/*, dt.Rows[0]["mw_permissiontype"].ToString()*/);
        }
        else
        {
            string SQL = @"SELECT
	                        mw_sysdocumentBase.mw_name, mw_sysdocumentBase.mw_documentname,
	                        mw_userdocumentBase.mw_userid, mw_userdocumentBase.mw_teamid, 
	                        mw_userdocumentBase.mw_permissiontype		
                        FROM 
	                        mw_sysdocumentBase INNER JOIN
	                        mw_userdocumentBase ON mw_sysdocumentBase.mw_userdocument = mw_userdocumentBase.mw_userdocumentId
                        Where mw_teamid in (SELECT teamid FROM TeamMembership where systemuserid = '" + UserID + "') and mw_sysdocumentBase.mw_name = '" + TemplateCode + "'";
            DataTable dt_Team = CRMAccessDB.SelectQ(SQL).Tables[0];

            if (dt_Team.Rows.Count > 0)
            {
                return FileText = ReplaceTempPassword(FileText, ConfigurationManager.AppSettings["MSWordhash"], ConfigurationManager.AppSettings["MSWordsalt"],
                                          ConfigurationManager.AppSettings["MSWordProviderType"], ConfigurationManager.AppSettings["MSWordAlgorithmSid"], UserID/*, dt.Rows[0]["mw_permissiontype"].ToString()*/);
            }
        }

        return "";
    }

    public static void addRowToTable(DataTable dt, DataRow row)
    {
        DataRow newRow = dt.NewRow();
        for (int i = 0; i < dt.Columns.Count; i++)
        {

            newRow[i] = row[i];
        }
        dt.Rows.Add(newRow);
    }
    public static string AddExcelRows(string FilePath, System.Data.DataTable dt)
    {
        string text = System.IO.File.ReadAllText(FilePath);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(text);
        XmlElement Sheet = (XmlElement)doc.GetElementsByTagName("Worksheet")[0];
        XmlElement Table = (XmlElement)doc.GetElementsByTagName("Table")[0];
        for (int i = 0; i < Table.Attributes.Count; i++)
        {
            if (Table.Attributes[i].Name.Contains("ExpandedRowCount"))
            {
                Table.Attributes[i].Value = (dt.Rows.Count + 5).ToString();
            }
        }

        if (Table.ChildNodes.Count >= 1)
        {
            XmlElement firstRow = (XmlElement)Table.GetElementsByTagName("Row")[1];
            Table.InnerXml.Replace(firstRow.OuterXml, "");

            XmlElement tempRow = (XmlElement)Table.GetElementsByTagName("Row")[1].CloneNode(true);
            Table.InnerXml = Table.InnerXml.Replace(firstRow.OuterXml, "");

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                XmlNodeList listData = tempRow.GetElementsByTagName("Data");
                for (int rowIndex = 0; rowIndex < listData.Count; rowIndex++)
                {
                    if (dt.Columns[rowIndex].DataType != typeof(DateTime))
                    {
                        listData[rowIndex].InnerText = dt.Rows[i][rowIndex].ToString();
                    }
                    else
                    {
                        if (dt.Rows[i][rowIndex].ToString() != "")
                        {
                            listData[rowIndex].InnerText = ((DateTime)dt.Rows[i][rowIndex]).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            listData[rowIndex].InnerText = "";
                        }
                    }
                }
                //if (i > 0)
                {
                    Table.InnerXml += tempRow.OuterXml;
                }
            }



        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        return doc.OuterXml;
    }
    public static string ConvertLinesToQuoteComma(string lines)
    {
        lines = lines.Replace("\r\n", ",").Replace("\r", ",").Replace("\n", ",").Replace(",", "','");
        lines = "'" + lines + "'";
        lines = lines.Replace(",''", "");

        return lines;
    }
    public static DataTable ConvertRowsToColumns(DataTable dtData, string XColumns, string YColumn, string ValueColumn, bool CalcTotal)
    {


        DataView dv = new DataView(dtData);
        DataTable dtResult = new DataTable();
        string[] xAxis = XColumns.Split(',');
        //string[] YAxis = YColumns.Split(',');
        for (int i = 0; i < xAxis.Length; i++)
        {
            dtResult.Columns.Add(xAxis[i], dtData.Columns[xAxis[i]].DataType);
        }
        DataTable dtColumns = dv.ToTable(true, YColumn.Split(','));
        DataTable dtDistinct = dtDistinct = dv.ToTable(true, xAxis);
        for (int i = 0; i < dtColumns.Rows.Count; i++)
        {
            dtResult.Columns.Add(dtColumns.Rows[i][YColumn].ToString());
        }
        for (int i = 0; i < dtDistinct.Rows.Count; i++)
        {
            DataRow row = dtResult.NewRow();
            string rowFilter = "";

            for (int x = 0; x < xAxis.Length; x++)
            {
                row[xAxis[x]] = dtDistinct.Rows[i][xAxis[x]];
                if (x == 0)
                {
                    rowFilter += xAxis[x] + "='" + dtDistinct.Rows[i][xAxis[x]].ToString() + "'" + " AND ";
                }
            }
            rowFilter = rowFilter.Substring(0, rowFilter.Length - 4);
            dv.RowFilter = rowFilter;
            for (int rowIndex = 0; rowIndex < dv.Count; rowIndex++)
            {
                if (dv[rowIndex][ValueColumn] == null) continue;
                if (dv[rowIndex][YColumn] == null) continue;
                if (row.Table.Columns.Contains(dv[rowIndex][YColumn].ToString()))
                {
                    row[dv[rowIndex][YColumn].ToString()] = dv[rowIndex][ValueColumn].ToString();
                }

            }

            dtResult.Rows.Add(row);
        }

        if (CalcTotal)
        {
            dtResult.Columns.Add("Total", typeof(decimal));
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                decimal total = 0;


                //xAxis.Split(',').Length
                for (int j = xAxis.Length; j < dtResult.Columns.Count - 1; j++)
                {
                    decimal value = 0;
                    decimal.TryParse(dtResult.Rows[i][j].ToString(), out value);

                    total += value;
                }
                dtResult.Rows[i]["Total"] = total;

            }
        }

        return dtResult;

    }
    public static DataTable ConvertRowsToColumns(DataTable dtData, string XColumns, int PivotXDepth, string YColumn, string ValueColumn, bool CalcTotal)
    {

        return ConvertRowsToColumns(dtData, XColumns, PivotXDepth, YColumn, ValueColumn, CalcTotal, "");

    }
    public static DataTable ConvertRowsToColumns(DataTable dtData, string XColumns, int PivotXDepth, string YColumn, string ValueColumn, bool CalcTotal, string LinkPage)
    {

        DataView dv = new DataView(dtData);
        DataTable dtResult = new DataTable();
        string[] xAxis = XColumns.Split(',');
        //string[] YAxis = YColumns.Split(',');
        for (int i = 0; i < xAxis.Length; i++)
        {
            dtResult.Columns.Add(xAxis[i], dtData.Columns[xAxis[i]].DataType);
        }

        DataTable dtColumns = dv.ToTable(true, YColumn.Split(','));
        DataTable dtDistinct = dtDistinct = dv.ToTable(true, xAxis);
        for (int i = 0; i < dtColumns.Rows.Count; i++)
        {
            dtResult.Columns.Add(dtColumns.Rows[i][YColumn].ToString());
        }
        for (int i = 0; i < dtDistinct.Rows.Count; i++)
        {
            DataRow row = dtResult.NewRow();
            string rowFilter = "";

            for (int x = 0; x < PivotXDepth; x++)
            {
                row[xAxis[x]] = dtDistinct.Rows[i][xAxis[x]];
                //if (x == 0)
                {
                    rowFilter += xAxis[x] + "='" + dtDistinct.Rows[i][xAxis[x]].ToString() + "'" + " AND ";
                }
            }
            rowFilter = rowFilter.Substring(0, rowFilter.Length - 4);
            dv.RowFilter = rowFilter;
            for (int rowIndex = 0; rowIndex < dv.Count; rowIndex++)
            {
                if (string.IsNullOrEmpty(LinkPage))
                {
                    row[dv[rowIndex][YColumn].ToString()] = dv[rowIndex][ValueColumn].ToString();
                }
                else
                {
                    if (LinkPage.Contains("?"))
                    {
                        row[dv[rowIndex][YColumn].ToString()] = "<a href='" + LinkPage + "&" + rowFilter.Replace(" AND ", "&").Replace("'", "") + "&" + YColumn + "=" + dv[rowIndex][YColumn].ToString() + "'>" + dv[rowIndex][ValueColumn].ToString() + "</a>";
                    }
                    else
                    {

                        row[dv[rowIndex][YColumn].ToString()] = "<a href='" + LinkPage + "?" + rowFilter.Replace(" AND ", "&").Replace("'", "") + "&" + YColumn + "=" + dv[rowIndex][YColumn].ToString() + "'>" + dv[rowIndex][ValueColumn].ToString() + "</a>";
                    }
                }
            }

            dtResult.Rows.Add(row);
        }

        if (CalcTotal)
        {
            dtResult.Columns.Add("Total", typeof(decimal));
            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                decimal total = 0;


                //xAxis.Split(',').Length
                for (int j = xAxis.Length; j < dtResult.Columns.Count - 1; j++)
                {
                    decimal value = 0;
                    decimal.TryParse(dtResult.Rows[i][j].ToString(), out value);

                    total += value;
                }
                dtResult.Rows[i]["Total"] = total;

            }
        }


        return dtResult;
    }

    public static void AgeerLogRecord(string UserID, int Action, string ContractNumber, string Iqama, string ProjectId)
    {
        Entity entity = new Entity("new_ageerlog");

        entity.Attributes["new_action"] = new OptionSetValue(Action);
        entity.Attributes["new_contractnumber"] = ContractNumber;
        entity.Attributes["new_iqama"] = Iqama;
        entity.Attributes["new_userid"] = new EntityReference("systemuser", new Guid(UserID));
        entity.Attributes["new_projectid"] = new EntityReference("new_project", new Guid(ProjectId));
        entity.Attributes["new_date"] = DateTime.Now;

        Service.Create(entity);
    }
    /// <summary>
    /// Auditing for simah in CRM
    /// </summary>
    /// <param name="UserID">User id in CRM</param>
    /// <param name="UploadType">0- No Action 1- Regular 2- Excel 3-Default</param>
    /// <param name="FileType">0- No Action 1 -Monthly 2- Weekly 3- Recovery Or Correction 4- Daily</param>
    /// <param name="DidQuery">0- No Action 1- SCORE_ONLY 2- Review 3- New Application 4- Negative 5- Miscellaneous</param>
    public static void SimahLogRecord(string UserID, int UploadType, int FileType, int DidQuery)
    {
        Entity entity = new Entity("new_simahlog");

        entity.Attributes["new_uploadtype"] = new OptionSetValue(UploadType);
        entity.Attributes["new_filetype"] = new OptionSetValue(FileType);
        entity.Attributes["new_query"] = new OptionSetValue(DidQuery);
        entity.Attributes["new_userid"] = new EntityReference("systemuser", new Guid(UserID));
        entity.Attributes["new_date"] = DateTime.Now;

        Service.Create(entity);
    }
    public static string GetSignature(string s)
    {
        int sum = 0;
        foreach (char c in s)
        {
            sum += Convert.ToInt16(c);
        }
        sum = sum * sum;
        return sum.ToString();
    }
    /// <summary>
    /// Auditing for simah in CRM
    /// </summary>
    /// <param name="UserID">User id in CRM</param>
    /// <param name="UploadType">0- No Action 1- Regular 2- Excel 3-Default</param>
    /// <param name="FileType">0- No Action 1 -Monthly 2- Weekly 3- Recovery Or Correction 4- Daily</param>
    /// <param name="DidQuery">0- No Action 1- SCORE_ONLY 2- Review 3- New Application 4- Negative 5- Miscellaneous</param>
    /// <param name="Iqama">set iqama number</param>
    public static void SimahLogRecord(string UserID, int UploadType, int FileType, int DidQuery, string Iqama)
    {
        Entity entity = new Entity("new_simahlog");

        entity.Attributes["new_uploadtype"] = new OptionSetValue(UploadType);
        entity.Attributes["new_filetype"] = new OptionSetValue(FileType);
        entity.Attributes["new_query"] = new OptionSetValue(DidQuery);
        entity.Attributes["new_userid"] = new EntityReference("systemuser", new Guid(UserID));
        entity.Attributes["new_date"] = DateTime.Now;
        entity.Attributes["new_iqama"] = Iqama;

        Service.Create(entity);
    }
    //public static string SendSMSM(string MobileNumber, String Message)
    //{
    //    if (Message.Contains("mawarid") || Message.Contains("موارد"))
    //    {
    //        return "";
    //    }

    //    //string UserName = ConfigurationManager.AppSettings["SMSUserName"];
    //    //string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
    //    //string TagName = ConfigurationManager.AppSettings["TagName"];
    //    ////SMSRef.SMSService sms = new SMSRef.SMSService();
    //    ////return sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);
    //    //string url = "http://www.fs4sms.com/sendsms.php?user=" + UserName + "&password=" + SMSPassword + "&senderid=" + TagName;
    //    //url += "&mobile=" + MobileNumber + "&message=" + Message;
    //    //string urlPath = url;
    //    //string request = urlPath;// +"index.php/org/get_org_form";
    //    //WebRequest webRequest = WebRequest.Create(request);
    //    //webRequest.Method = "GET";
    //    //webRequest.ContentType = "application/x-www-form-urlencoded";
    //    ////webRequest.ContentLength = dataStream.Length;
    //    ////Stream newStream = webRequest.GetRequestStream();
    //    //// Send the data.
    //    ////newStream.Write(dataStream, 0, dataStream.Length);
    //    //// newStream.Close();
    //    //WebResponse webResponse = webRequest.GetResponse();
    //    //var responseString = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

    //    //return "";

    //    string UserName = ConfigurationManager.AppSettings["SMSUserName"];
    //    string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
    //    string TagName = ConfigurationManager.AppSettings["TagName"];
    //    SMSRef.SMSService sms = new SMSRef.SMSService();
    //    string result = sms.SendBulkSMS(UserName, SMSPassword, TagName, MobileNumber, Message);
    //    return result;

    //}
    //public static void ConvertDocToPDF(string FileText, string FileName)
    //{
    //    string pdfName = "";
    //    ConvertDocToPDF(FileText, FileName, out pdfName, true);
    //}
    //public static void ConvertDocToPDF(string FileText, string FileName, out string PdfName, bool Redirect)
    //{
    //    PdfName = "";
    //    //
    //    //this important comment
    //    //to enable interop from asp.net
    //    //goto component Services=>DCOM Config => microsoft word =>right click properties >=Identity =>Inetractive user checkbox
    //    //and create desktop folder In this folders C:\Windows\SysWOW64\config\systemprofile
    //    //C:\Windows\System32\config\systemprofile
    //    try
    //    {


    //        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(HttpContext.Current.Server.MapPath("~"));
    //        dir = dir.Parent;
    //        dir = new System.IO.DirectoryInfo(dir.FullName + "/PDFFiles/");
    //        string filePath = @"C:\ISV\PDFFiles\" + DateTime.Now.Ticks.ToString() + FileName;
    //        string outPutFile = filePath.Replace(".doc", ".pdf").Replace(".xml", ".pdf");
    //        if (System.IO.File.Exists(filePath))
    //        {
    //            System.IO.File.Delete(filePath);
    //        }

    //        if (System.IO.File.Exists(outPutFile))
    //        {
    //            System.IO.File.Delete(outPutFile);
    //        }
    //        System.IO.File.WriteAllText(filePath, FileText);

    //        // Create an instance of Word.exe
    //        Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

    //        // Make this instance of word invisible (Can still see it in the taskmgr).
    //        oWord.Visible = false;

    //        // Interop requires objects.
    //        object oMissing = System.Reflection.Missing.Value;
    //        object isVisible = true;
    //        object readOnly = false;
    //        object oInput = filePath;
    //        object oOutput = outPutFile;
    //        PdfName = outPutFile;
    //        object oFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;


    //        // Load a document into our instance of word.exe
    //        Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

    //        // Make this document the active document.
    //        oDoc.Activate();

    //        // Save this document in Word 2003 format.
    //        oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

    //        oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
    //        //HttpContext.Current.Response.Clear();
    //        //HttpContext.Current.Response.ContentType = "application/pdf";
    //        //HttpContext.Current.Response.AddHeader("Content-Disposition",
    //        //    "attachment;filename=\"" + FileName + ".pdf\"");
    //        //HttpContext.Current.Response.TransmitFile(outPutFile);
    //        System.IO.FileInfo file = new System.IO.FileInfo(outPutFile);
    //        if (Redirect)
    //        {
    //            HttpContext.Current.Response.Redirect("~/PDFFiles/" + file.Name);
    //        }


    //    }
    //    catch (Exception exc)
    //    {
    //        if (Redirect)
    //        {

    //            HttpContext.Current.Response.Clear();
    //            HttpContext.Current.Response.ContentType = "application/ms-word";
    //            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
    //            HttpContext.Current.Response.Write(FileText);
    //            HttpContext.Current.Response.End();
    //        }

    //    }

    //}
    public static void DownloadExcel(DataTable dt)
    {
        System.Web.HttpContext.Current.Session["dtResult"] = dt;
        System.Web.HttpContext.Current.Response.Redirect("~/Agent/ExcelGrid.aspx");
    }
    public static DateTime? ConvertAllStrToDateTime(object ODate)
    {

        if (ODate == null || ODate.ToString() == "") return null;
        System.Globalization.CultureInfo info = new System.Globalization.CultureInfo("en-US");

        string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
                "dd/MM/yyyy","d/M/yyyy",
                "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
                "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
                "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
                "yyyy M d","dd MM yyyy","d M yyyy",
                "dd M yyyy","d MM yyyy","dd/MM/yyyy hh:mm:ss tt","yyyyMMddTHHmmss","yyyyMMddTHH:mm:ss.fff","yyyyMMddTHHmmssfff","yyyymmddhhmmssfff","yyyyMMddT","yyyyMMddt","u","U","T","t"};

        if (ODate is DateTime) return (DateTime)ODate;
        DateTime retDate = new DateTime();
        if (DateTime.TryParse(ODate.ToString(), out retDate))
            return retDate;
        retDate = DateTime.ParseExact(ODate.ToString(), allFormats, info, System.Globalization.DateTimeStyles.None);
        return retDate;
    }
    public static string EncryptText(string input, string password)
    {
        // Get the bytes of the string
        byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Hash the password with SHA256
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

        string result = Convert.ToBase64String(bytesEncrypted);

        return result;
    }





    public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] encryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }
        }

        return encryptedBytes;
    }

    public static string DecryptText(string input, string password)
    {
        // Get the bytes of the string
        byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

        string result = Encoding.UTF8.GetString(bytesDecrypted);

        return result;
    }
    public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        byte[] decryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.Close();
                }
                decryptedBytes = ms.ToArray();
            }
        }

        return decryptedBytes;
    }

    /*Method was added by Abdelrhman */
    //public static void GenerateSystemDocument(string SelectedDocID, string EntityId, string EntityTypeName)
    //{
    //    DataTable dtSqlQuery = new DataTable();
    //    DataTable dtDocQuery = new DataTable();
    //    StringBuilder sb = new StringBuilder();
    //    string SqlQuery = string.Empty;
    //    string sql = "select mw_serverurl,mw_documentname from mw_sysdocumentBase where (mw_sysdocumentId='@SysDocId')";
    //    sql = sql.Replace("@SysDocId", SelectedDocID);
    //    DataTable dtSystemDoc = CRMAccessDB.SelectQ(sql).Tables[0];
    //    sql = "select new_documentqueryid from new_new_documentquery_mw_sysdocumentBase where (mw_sysdocumentid='@SystemDocId')";
    //    sql = sql.Replace("@SystemDocId", SelectedDocID);
    //    DataTable dtDocQueryIDs = CRMAccessDB.SelectQ(sql).Tables[0];
    //    string filePath = HttpContext.Current.Server.MapPath(dtSystemDoc.Rows[0][0].ToString());
    //    string FileText = System.IO.File.ReadAllText(filePath);
    //    //FileText = GlobalCode.ReplaceTempPassword(FileText, ConfigurationManager.AppSettings["MSWordhash"], ConfigurationManager.AppSettings["MSWordsalt"],
    //    //                                ConfigurationManager.AppSettings["MSWordProviderType"], ConfigurationManager.AppSettings["MSWordAlgorithmSid"], Request.QueryString["UserID"]);
    //    for (int i = 0; i < dtDocQueryIDs.Rows.Count; i++)
    //    {
    //        sql = "select new_sqlquery,new_querytype,new_keyword from new_documentqueryBase where (new_documentqueryId='@DocQueryId')";
    //        sql = sql.Replace("@DocQueryId", dtDocQueryIDs.Rows[i][0].ToString());
    //        dtDocQuery = CRMAccessDB.SelectQ(sql).Tables[0];
    //        SqlQuery = dtDocQuery.Rows[0][0].ToString().Replace("@id", EntityId);
    //        dtSqlQuery = CRMAccessDB.SelectQ(SqlQuery).Tables[0];
    //        if (dtSqlQuery.Rows.Count > 0)
    //        {
    //            if (((bool)dtDocQuery.Rows[0][1] == true)) //details
    //            {
    //                FileText = GlobalCode.InsertRepeatedRow(FileText, dtDocQuery.Rows[0][2].ToString(), dtSqlQuery);
    //            }
    //            for (int rowIndex = 0; rowIndex < dtSqlQuery.Rows.Count; rowIndex++)
    //            {
    //                for (int j = 0; j < dtSqlQuery.Columns.Count; j++)
    //                {
    //                    FileText = FileText.Replace("" + dtSqlQuery.Columns[j].ColumnName, dtSqlQuery.Rows[rowIndex][j].ToString().Replace(".0000000000", "").Replace(".00000000", "").Replace(".0000", ""));
    //                }
    //            }

    //        }
    //    }
    //    string fileName = dtSystemDoc.Rows[0][1].ToString() + ".doc";
    //    ConvertDocToPDF(FileText, fileName);
    //}

 public static EntityReference GetLookUpSearchMultiColumns(string tableName, string ColumnValue, string SearchColumns)
    {
        if (HttpContext.Current.Cache[tableName + ColumnValue] == null)
        {
            string searchWhere = "";
            string[] columns = SearchColumns.Split(',');
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();

            for (int i = 0; i < columns.Length; i++)
            {
                if (searchWhere == "")
                {
                    searchWhere += " " + columns[i] + " =@value";
                }
                else
                {
                    searchWhere += " OR  " + columns[i] + " =@value";
                }
            }
            cmd.CommandText = "Select " + tableName + "id from " + tableName + " where " + searchWhere;
            cmd.Parameters.Add("@value", SqlDbType.NVarChar).Value = ColumnValue;
            DataTable dt = CRMAccessDB.SelectQ(cmd).Tables[0];
            if (dt.Rows.Count > 0)
            {
                EntityReference refEntity = new EntityReference(tableName, new Guid(dt.Rows[0][0].ToString()));
                HttpContext.Current.Cache[tableName + ColumnValue] = refEntity;
                return refEntity;
            }
            else
            {
                return null;
            }

        }
        else
        {
            return (EntityReference)HttpContext.Current.Cache[tableName + ColumnValue];
        }


    }
}

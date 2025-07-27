using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Text;
using System.Web;
using XONT.Common.Message;

namespace XONT.Ventura.AppConsole.DomainCore
{
    [Serializable]
    public class BusinessUnit
    {
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string FaxNumber { get; set; }
        public string PostCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string VATRegistrationNumber { get; set; }
        public string Logo { get; set; }

        public string GetFullAddress()
        {
            return AddressLine1 + '\n' + AddressLine2 + '\n' + AddressLine3 + '\n' + AddressLine4 + '\n' + AddressLine5;
        }

        //V2031Adding start
        public string GetBusinessUnitDetailForReport()
        {
            StringBuilder sb = new StringBuilder();

            //Addressing if addressLine3 were to be null
            AddressLine3 = AddressLine3 ?? "";

            //Business Unit Name
            sb.Append(AddressLine1.ToString().Trim() + " " + AddressLine2.ToString().Trim() + " " + AddressLine3.ToString().Trim());

            if (!string.IsNullOrWhiteSpace(AddressLine4) && !string.IsNullOrWhiteSpace(AddressLine5))
            {
                sb.Append("\n" + AddressLine4 + " " + AddressLine5);
            }
            else if (!string.IsNullOrWhiteSpace(AddressLine4))
            {
                sb.Append("\n" + AddressLine4);
            }
            else if (!string.IsNullOrWhiteSpace(AddressLine5))
            {
                sb.Append("\n" + AddressLine5);
            }

            if(!string.IsNullOrWhiteSpace(TelephoneNumber) && !string.IsNullOrWhiteSpace(FaxNumber))
            {
                sb.Append("\n Tel : " + TelephoneNumber.Trim() + "; Fax : " + FaxNumber.Trim());
            }
            else if (!string.IsNullOrWhiteSpace(TelephoneNumber))
            {
                sb.Append("\n Tel : " + TelephoneNumber.Trim());
            }
            else if (!string.IsNullOrWhiteSpace(FaxNumber))
            {
                sb.Append("\n Fax : " + FaxNumber.Trim());
            }

            if (!string.IsNullOrWhiteSpace(EmailAddress))
            {
                sb.Append("\n E-mail : " + EmailAddress.Trim());
            }
            if (!string.IsNullOrWhiteSpace(WebAddress))
            {
                sb.Append("\n Web : " + WebAddress.Trim());
            }
            #region V2040Removed
            //if (!string.IsNullOrEmpty(VATRegistrationNumber))
            //{
            //    sb.Append("\n VAT Registration No : " + VATRegistrationNumber.Trim());
            //} 
            #endregion

            return sb.ToString();
        }

        public static DataTable GetLogo(string businessUnit, out MessageSet msg, string userDbConnectionString)
        {
            DataTable toReturn = null;
            msg = null;
            try
            {
                string query = @" SELECT  BusinessUnit,Logo,ShowLogo = CASE WHEN Logo IS NULL THEN 0 ELSE 1 END FROM  ZYBusinessUnit WHERE ZYBusinessUnit.BusinessUnit=@BusinessUnit";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@BusinessUnit", SqlDbType.VarChar) { Value = businessUnit }
                };
                using (var conn = new SqlConnection(userDbConnectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(toReturn);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetLogo", "XONT.Ventura.AppConsole.Domain.dll");
            }

            return toReturn;
        }
        //V2031Adding end
    }
}
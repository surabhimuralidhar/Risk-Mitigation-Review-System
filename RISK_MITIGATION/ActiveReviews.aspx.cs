using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiskReviewForm
{
    public partial class ActiveReviews : System.Web.UI.Page
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RiskDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                litDetails.Text = "";
            }
        }

        protected void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT * FROM RiskReviews r
                    WHERE TokenId = (
                        SELECT MAX(TokenId) FROM RiskReviews 
                        WHERE RiskId = r.RiskId
                    )
                    ORDER BY RiskId, TokenId DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridViewActive.DataSource = dt;
                GridViewActive.DataBind();
            }

            litDetails.Text = "";
        }

        protected void GridViewActive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string riskId = GridViewActive.DataKeys[index].Value.ToString();

            switch (e.CommandName)
            {
                case "EditEntry":
                    Response.Redirect($"RiskForm.aspx?RiskId={riskId}");
                    break;

                case "DeleteEntry":
                    DeleteLatestVersion(riskId);
                    break;

                case "CloseEntry":
                    CloseLatestVersion(riskId);
                    break;

                case "Details":
                    ToggleDetails(riskId);
                    break;
            }
        }

        private void DeleteLatestVersion(string riskId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string deleteQuery = @"
                    DELETE FROM RiskReviews
                    WHERE RiskId = @RiskId
                    AND TokenId = (
                        SELECT MAX(TokenId) FROM RiskReviews WHERE RiskId = @RiskId
                    )";

                SqlCommand cmd = new SqlCommand(deleteQuery, con);
                cmd.Parameters.AddWithValue("@RiskId", riskId);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }

        private void CloseLatestVersion(string riskId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string selectQuery = @"
                    SELECT * FROM RiskReviews
                    WHERE RiskId = @RiskId
                    AND TokenId = (
                        SELECT MAX(TokenId) FROM RiskReviews WHERE RiskId = @RiskId
                    )";

                SqlCommand selectCmd = new SqlCommand(selectQuery, con);
                selectCmd.Parameters.AddWithValue("@RiskId", riskId);
                con.Open();
                SqlDataReader reader = selectCmd.ExecuteReader();

                if (reader.Read())
                {
                    SqlCommand insertCmd = new SqlCommand(@"
                        INSERT INTO ClosedRiskHistory
                        (RiskId, TokenId, SubKeyProcess, InterestedParty, RiskStatement, Owners, OpportunityIdentified,
                         Likelihood, Impact, Score, ReviewType, ImpactDetails, MitigationPlan, TargetDate, Remarks)
                        VALUES
                        (@RiskId, @TokenId, @SubKeyProcess, @InterestedParty, @RiskStatement, @Owners, @OpportunityIdentified,
                         @Likelihood, @Impact, @Score, @ReviewType, @ImpactDetails, @MitigationPlan, @TargetDate, @Remarks)", con);

                    insertCmd.Parameters.AddWithValue("@RiskId", reader["RiskId"]);
                    insertCmd.Parameters.AddWithValue("@TokenId", reader["TokenId"]);
                    insertCmd.Parameters.AddWithValue("@SubKeyProcess", reader["SubKeyProcess"]);
                    insertCmd.Parameters.AddWithValue("@InterestedParty", reader["InterestedParty"]);
                    insertCmd.Parameters.AddWithValue("@RiskStatement", reader["RiskStatement"]);
                    insertCmd.Parameters.AddWithValue("@Owners", reader["Owners"]);
                    insertCmd.Parameters.AddWithValue("@OpportunityIdentified", reader["OpportunityIdentified"]);
                    insertCmd.Parameters.AddWithValue("@Likelihood", reader["Likelihood"]);
                    insertCmd.Parameters.AddWithValue("@Impact", reader["Impact"]);
                    insertCmd.Parameters.AddWithValue("@Score", reader["Score"]);
                    insertCmd.Parameters.AddWithValue("@ReviewType", reader["ReviewType"]);
                    insertCmd.Parameters.AddWithValue("@ImpactDetails", reader["ImpactDetails"]);
                    insertCmd.Parameters.AddWithValue("@MitigationPlan", reader["MitigationPlan"]);
                    insertCmd.Parameters.AddWithValue("@TargetDate", reader["TargetDate"]);
                    insertCmd.Parameters.AddWithValue("@Remarks", reader["Remarks"]);

                    reader.Close();
                    insertCmd.ExecuteNonQuery();
                }

                con.Close();
            }

            DeleteLatestVersion(riskId);
        }

        private void ToggleDetails(string riskId)
        {
            if (ViewState["ExpandedRiskId"] != null && ViewState["ExpandedRiskId"].ToString() == riskId)
            {
                ViewState["ExpandedRiskId"] = null;
                litDetails.Text = "";
                return;
            }

            ViewState["ExpandedRiskId"] = riskId;

            StringBuilder sb = new StringBuilder();
            sb.Append("<h3 style='margin-top:20px;'>All Versions for Risk ID: " + riskId + "</h3>");
            sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'>");
            sb.Append("<tr style='background-color: #f2f2f2;'>");
            sb.Append("<th>TokenId</th><th>Sub & Key Process</th><th>Interested Party</th><th>Risk Statement</th><th>Owners</th>");
            sb.Append("<th>Opportunity Identified</th><th>Likelihood</th><th>Impact</th><th>Score</th><th>Review Type</th>");
            sb.Append("<th>Impact Details</th><th>Mitigation Plan</th><th>Target Date</th><th>Remarks</th>");
            sb.Append("</tr>");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT * FROM RiskReviews
                    WHERE RiskId = @RiskId
                    ORDER BY TokenId DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RiskId", riskId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", reader["TokenId"]);
                    sb.AppendFormat("<td>{0}</td>", reader["SubKeyProcess"]);
                    sb.AppendFormat("<td>{0}</td>", reader["InterestedParty"]);
                    sb.AppendFormat("<td>{0}</td>", reader["RiskStatement"]);
                    sb.AppendFormat("<td>{0}</td>", reader["Owners"]);
                    sb.AppendFormat("<td>{0}</td>", reader["OpportunityIdentified"]);
                    sb.AppendFormat("<td>{0}</td>", reader["Likelihood"]);
                    sb.AppendFormat("<td>{0}</td>", reader["Impact"]);
                    sb.AppendFormat("<td>{0}</td>", reader["Score"]);
                    sb.AppendFormat("<td>{0}</td>", reader["ReviewType"]);
                    sb.AppendFormat("<td>{0}</td>", reader["ImpactDetails"]);
                    sb.AppendFormat("<td>{0}</td>", reader["MitigationPlan"]);
                    sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(reader["TargetDate"]).ToString("yyyy-MM-dd"));
                    sb.AppendFormat("<td>{0}</td>", reader["Remarks"]);
                    sb.Append("</tr>");
                }

                reader.Close();
            }

            sb.Append("</table>");
            litDetails.Text = sb.ToString();
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        protected void GridViewActive_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RiskReviewForm
{
    public partial class RiskForm : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["RiskDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropdowns();

                if (Request.QueryString["RiskId"] != null)
                {
                    string riskId = Request.QueryString["RiskId"];
                    LoadRiskDetails(riskId);
                }
            }
        }

        private void BindDropdowns()
        {
            ddlLikelihood.Items.Clear();
            ddlImpact.Items.Clear();

            ddlLikelihood.Items.Add(new ListItem("--Select--", ""));
            ddlLikelihood.Items.Add(new ListItem("1"));
            ddlLikelihood.Items.Add(new ListItem("2"));
            ddlLikelihood.Items.Add(new ListItem("3"));
            ddlLikelihood.Items.Add(new ListItem("4"));
            ddlLikelihood.Items.Add(new ListItem("5"));

            ddlImpact.Items.Add(new ListItem("--Select--", ""));
            ddlImpact.Items.Add(new ListItem("1"));
            ddlImpact.Items.Add(new ListItem("2"));
            ddlImpact.Items.Add(new ListItem("3"));
            ddlImpact.Items.Add(new ListItem("4"));
            ddlImpact.Items.Add(new ListItem("5"));
        }

        private int GetNextTokenId(string riskId)
        {
            int maxToken = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT MAX(TokenId) FROM RiskReviews WHERE RiskId = @RiskId", conn);
                cmd.Parameters.AddWithValue("@RiskId", riskId);
                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                    maxToken = Convert.ToInt32(result);
            }
            return maxToken + 1;
        }

        private string GetReviewType(int score)
        {
            if (score == 5 || (score >= 6 && score <= 11))
                return "Half-Yearly";
            else if (score >= 12)
                return "Quarterly";
            else
                return "Annual";
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(ddlLikelihood.SelectedValue, out int likelihood) &&
                int.TryParse(ddlImpact.SelectedValue, out int impact))
            {
                int score = likelihood * impact;
                txtScore.Text = score.ToString();
                txtReviewType.Text = GetReviewType(score);
            }
            else
            {
                txtScore.Text = "";
                txtReviewType.Text = "";
            }
        }

        private void LoadRiskDetails(string riskId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT TOP 1 *
                    FROM RiskReviews
                    WHERE RiskId = @RiskId
                    ORDER BY TokenId DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RiskId", riskId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtSubKeyProcess.Text = reader["SubKeyProcess"].ToString();
                    txtInterestedParty.Text = reader["InterestedParty"].ToString();
                    txtRiskStatement.Text = reader["RiskStatement"].ToString();
                    txtOwners.Text = reader["Owners"].ToString();
                    txtOpportunityIdentified.Text = reader["OpportunityIdentified"].ToString();

                    ddlLikelihood.SelectedValue = reader["Likelihood"].ToString();
                    ddlImpact.SelectedValue = reader["Impact"].ToString();

                    txtScore.Text = reader["Score"].ToString();
                    txtReviewType.Text = reader["ReviewType"].ToString();
                    txtImpactDetails.Text = reader["ImpactDetails"].ToString();
                    txtMitigationPlan.Text = reader["MitigationPlan"].ToString();

                    if (DateTime.TryParse(reader["TargetDate"].ToString(), out DateTime targetDate))
                    {
                        txtTargetDate.Text = targetDate.ToString("yyyy-MM-dd");
                    }

                    txtRemarks.Text = reader["Remarks"].ToString();
                }

                reader.Close();
            }
        }
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RiskDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string riskId = Request.QueryString["RiskId"];
                bool isEdit = !string.IsNullOrEmpty(riskId);

                if (!isEdit)
                {
                    // New record: generate new RiskId
                    riskId = Guid.NewGuid().ToString();
                }

                int nextTokenId = GetNextTokenId(riskId);

                SqlCommand cmd = new SqlCommand(@"INSERT INTO RiskReviews 
        (RiskId, TokenId, SubKeyProcess, InterestedParty, RiskStatement, Owners, OpportunityIdentified,
         Likelihood, Impact, Score, ReviewType, ImpactDetails, MitigationPlan, TargetDate, Remarks) 
        VALUES 
        (@RiskId, @TokenId, @SubKeyProcess, @InterestedParty, @RiskStatement, @Owners, @OpportunityIdentified,
         @Likelihood, @Impact, @Score, @ReviewType, @ImpactDetails, @MitigationPlan, @TargetDate, @Remarks)", conn);

                cmd.Parameters.AddWithValue("@RiskId", riskId);
                cmd.Parameters.AddWithValue("@TokenId", nextTokenId);
                cmd.Parameters.AddWithValue("@SubKeyProcess", txtSubKeyProcess.Text.Trim());
                cmd.Parameters.AddWithValue("@InterestedParty", txtInterestedParty.Text.Trim());
                cmd.Parameters.AddWithValue("@RiskStatement", txtRiskStatement.Text.Trim());
                cmd.Parameters.AddWithValue("@Owners", txtOwners.Text.Trim());
                cmd.Parameters.AddWithValue("@OpportunityIdentified", txtOpportunityIdentified.Text.Trim());
                cmd.Parameters.AddWithValue("@Likelihood", ddlLikelihood.SelectedValue);
                cmd.Parameters.AddWithValue("@Impact", ddlImpact.SelectedValue);

                int score = int.Parse(ddlLikelihood.SelectedValue) * int.Parse(ddlImpact.SelectedValue);
                cmd.Parameters.AddWithValue("@Score", score);

                string reviewType = GetReviewType(score);
                cmd.Parameters.AddWithValue("@ReviewType", reviewType);

                cmd.Parameters.AddWithValue("@ImpactDetails", txtImpactDetails.Text.Trim());
                cmd.Parameters.AddWithValue("@MitigationPlan", txtMitigationPlan.Text.Trim());
                cmd.Parameters.AddWithValue("@TargetDate", txtTargetDate.Text.Trim());
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());

                cmd.ExecuteNonQuery();

                Response.Redirect("ActiveReviews.aspx");
            }
        }


        protected void ddlLikelihood_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace RiskReviewForm
{
    public partial class ClosedReviews : System.Web.UI.Page
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RiskDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindClosedReviews();
            }
        }

        protected void BindClosedReviews()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ClosedRiskHistory ORDER BY RiskId, TokenId DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridViewClosed.DataSource = dt;
                GridViewClosed.DataBind();
            }
        }

        protected void GridViewClosed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteClosed")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string riskId = GridViewClosed.DataKeys[index]["RiskId"].ToString();
                int tokenId = Convert.ToInt32(GridViewClosed.DataKeys[index]["TokenId"]);

                DeleteClosedReview(riskId, tokenId);
                BindClosedReviews();
            }
        }

        private void DeleteClosedReview(string riskId, int tokenId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ClosedRiskHistory WHERE RiskId = @RiskId AND TokenId = @TokenId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@RiskId", riskId);
                cmd.Parameters.AddWithValue("@TokenId", tokenId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}


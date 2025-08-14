<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RiskForm.aspx.cs" Inherits="RiskReviewForm.RiskForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Review Form</title>
    <script type="text/javascript">
        function calculateScore() {
            var likelihood = parseInt(document.getElementById('<%= ddlLikelihood.ClientID %>').value);
            var impact = parseInt(document.getElementById('<%= ddlImpact.ClientID %>').value);
            if (!isNaN(likelihood) && !isNaN(impact)) {
                var score = likelihood * impact;
                var reviewType = "";

                if (score >= 12) reviewType = "Quarterly";
                else if (score == 5 || (score >= 6 && score <= 11)) reviewType = "Half-Yearly";
                else reviewType = "Annual";

                document.getElementById('<%= txtScore.ClientID %>').value = score;
                document.getElementById('<%= txtReviewType.ClientID %>').value = reviewType;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-bottom: 15px;">
    \
    <a href="ActiveReviews.aspx">Active Reviews</a> |
    <a href="ClosedReviews.aspx">Closed Reviews</a>
</div>

        <div style="width:800px; margin:auto">
            <h2>Risk Review Form</h2>

            <asp:Label ID="lblSubKeyProcess" runat="server" Text="Sub & Key Process:" /><br />
            <asp:TextBox ID="txtSubKeyProcess" runat="server" Width="100%" /><br />

            <asp:Label ID="lblInterestedParty" runat="server" Text="Interested Party:" /><br />
            <asp:TextBox ID="txtInterestedParty" runat="server" Width="100%" /><br />

            <asp:Label ID="lblRiskStatement" runat="server" Text="Risk Statement:" /><br />
            <asp:TextBox ID="txtRiskStatement" runat="server" Width="100%" /><br />

            <asp:Label ID="lblOwners" runat="server" Text="Owners:" /><br />
            <asp:TextBox ID="txtOwners" runat="server" Width="100%" /><br />

            <asp:Label ID="lblOpportunityIdentified" runat="server" Text="Opportunity Identified:" /><br />
            <asp:TextBox ID="txtOpportunityIdentified" runat="server" Width="100%" /><br />

            <asp:Label ID="lblLikelihood" runat="server" Text="Likelihood of Occurrence:" /><br />
            <asp:DropDownList ID="ddlLikelihood" runat="server" AutoPostBack="false" Width="100%" onchange="calculateScore()" OnSelectedIndexChanged="ddlLikelihood_SelectedIndexChanged">
                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                <asp:ListItem Text="5" Value="5"></asp:ListItem>
            </asp:DropDownList><br />

            <asp:Label ID="lblImpact" runat="server" Text="Impact:" /><br />
            <asp:DropDownList ID="ddlImpact" runat="server" AutoPostBack="false" Width="100%" onchange="calculateScore()">
                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                <asp:ListItem Text="5" Value="5"></asp:ListItem>
            </asp:DropDownList><br />

            

            <asp:Label ID="lblScore" runat="server" Text="Score:" /><br />
            <asp:TextBox ID="txtScore" runat="server" Width="100%" ReadOnly="True" /><br />

            <asp:Label ID="lblReviewType" runat="server" Text="Review Type:" /><br />
            <asp:TextBox ID="txtReviewType" runat="server" Width="100%" ReadOnly="True" /><br />

            <asp:Label ID="lblImpactDetails" runat="server" Text="Impact Details:" /><br />
            <asp:TextBox ID="txtImpactDetails" runat="server" Width="100%" /><br />

            <asp:Label ID="lblMitigationPlan" runat="server" Text="Mitigation Plan:" /><br />
            <asp:TextBox ID="txtMitigationPlan" runat="server" Width="100%" /><br />

            <asp:Label ID="lblTargetDate" runat="server" Text="Target Date:" /><br />
            <asp:TextBox ID="txtTargetDate" runat="server" TextMode="Date" Width="100%" /><br />

            <asp:Label ID="lblRemarks" runat="server" Text="Remarks:" /><br />
            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" /><br /><br />

            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" PostBackUrl="~/ActiveReviews.aspx" />


        </div>
        <br />
<asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />

    </form>
</body>
</html>
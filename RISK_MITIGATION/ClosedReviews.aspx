<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClosedReviews.aspx.cs" Inherits="RiskReviewForm.ClosedReviews" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Closed Risk Reviews</title>
    <style>
        table, th, td {
            border: 1px solid black;
            border-collapse: collapse;
            padding: 5px;
        }

        th {
            background-color: #f2f2f2;
        }

        .action-buttons {
            display: flex;
            gap: 8px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-bottom: 15px;">
    <a href="RiskForm.aspx">New Risk Form</a> |
    <a href="ActiveReviews.aspx">Active Reviews</a> |
   
</div>

        <h2>Closed Risk Reviews</h2>
        <asp:GridView ID="GridViewClosed" runat="server" AutoGenerateColumns="False" 
                      OnRowCommand="GridViewClosed_RowCommand" DataKeyNames="RiskId,TokenId">
            <Columns>
                <asp:BoundField DataField="RiskId" HeaderText="Risk ID" />
                <asp:BoundField DataField="TokenId" HeaderText="Token ID" />
                <asp:BoundField DataField="SubKeyProcess" HeaderText="Sub & Key Process" />
                <asp:BoundField DataField="InterestedParty" HeaderText="Interested Party" />
                <asp:BoundField DataField="RiskStatement" HeaderText="Risk Statement" />
                <asp:BoundField DataField="Owners" HeaderText="Owners" />
                <asp:BoundField DataField="OpportunityIdentified" HeaderText="Opportunity" />
                <asp:BoundField DataField="Likelihood" HeaderText="Likelihood" />
                <asp:BoundField DataField="Impact" HeaderText="Impact" />
                <asp:BoundField DataField="Score" HeaderText="Score" />
                <asp:BoundField DataField="ReviewType" HeaderText="Review Type" />
                <asp:BoundField DataField="ImpactDetails" HeaderText="Impact Details" />
                <asp:BoundField DataField="MitigationPlan" HeaderText="Mitigation Plan" />
                <asp:BoundField DataField="TargetDate" HeaderText="Target Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" CommandName="DeleteClosed"
                                    CommandArgument='<%# Container.DataItemIndex %>' 
                                    Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this closed review?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>

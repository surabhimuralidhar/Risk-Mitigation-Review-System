CREATE TABLE RiskReviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RiskId UNIQUEIDENTIFIER NOT NULL,
    TokenId INT NOT NULL,
    
    SubKeyProcess NVARCHAR(255),
    InterestedParty NVARCHAR(255),
    
    RiskStatement NVARCHAR(MAX),
    Owners NVARCHAR(255),
    OpportunityIdentified NVARCHAR(MAX),
    
    LikelihoodOfOccurrence INT,
    Impact INT,
    Score INT,
    ReviewType NVARCHAR(50),
    
    ImpactDetails NVARCHAR(MAX),
    MitigationPlan NVARCHAR(MAX),
    TargetDate DATE,
    Remarks NVARCHAR(MAX),
    
    CreatedDate DATETIME DEFAULT GETDATE()
);


USE AndroAdmin2;
GO
IF OBJECT_ID (N'dbo.fn_GetStoresForUser', N'TF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetStoresForUser;
GO

CREATE FUNCTION dbo.fn_GetStoresForUser(@UserRecordID INTEGER)
RETURNS @kk TABLE
(storeName nvarchar(255)
)
AS
BEGIN
WITH CHAINS AS
(
SELECT chainID 
FROM MyAndromeda.dbo.UserChain AS U
WHERE UserRecordId=@UserRecordID),

SubChains(chainID) AS
(
SELECT C.chainID FROM CHAINS AS C

UNION ALL

SELECT CC.ChildChainID AS chainID 
FROM ChainChain AS CC
INNER JOIN SubChains AS SC ON SC.chainID=CC.ParentChainId
),

AllLeafChains
AS
(
SELECT ChainID 
FROM Store),

LeafChains(ChainID)
AS
(
SELECT S.ChainID
FROM SubChains AS S
INTERSECT
SELECT A.ChainID 
FROM AllLeafChains AS A),

StoreNames
AS
(
SELECT S.ClientSiteName AS StoreName
FROM
store AS S
WHERE S.ChainId IN(SELECT ChainID FROM LeafChains)
)
INSERT @kk
Select StoreName FROM StoreNames;
RETURN
END
GO
SELECT storename from fn_GetStoresForUser(7);
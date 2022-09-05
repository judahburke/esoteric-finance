select 
    c.PaymentCategoryId, 
    sc.PaymentSubCategoryId, 
    c.Name, 
    sc.Name,
from PaymentCategories c
inner join PaymentSubCategories sc
    on sc.PaymentCategoryId = c.PaymentCategoryId



select
    tx.TransactionDate,
    tx.PostedDate,
    total.Amount,
    txm.Amount As AmountByMethod
    m.Name as Method

    t.Name as Recipient,
from PaymentTransactions tx

inner join PaymentTransactionSubCategories txsc
    on txsc.PaymentTransactionId = tx.PaymentTransactionId
inner join PaymentSubCategories sc
    on sc.PaymentSubCategoryId = txsc.PaymentSubCategoryId
inner join PaymentCategories c
    on c.PaymentCategoryId = sc.PaymentCategoryId

inner join PaymentTransactionMethods txm
    on txm.PaymentTransactionId = tx.PaymentTransactionId
inner join PaymentMethods m
    on m.PaymentMethodId = txm.PaymentMethodId

inner join PaymentTargets t
    on t.PaymentTargetId = tx.PaymentTargetId

outer apply (
    select SUM(otm.Amount) as Amount
    from PaymentTransactionMethods
    where PaymentTransactionId = tx.PaymentTransactionId
) as total

order by tx.TransactionDate, tx.PaymentTransactionId, 

/* pivots aren't built into sqlite */
-- outer apply (
--     SELECT *  
--     FROM  
--     (
--     SELECT Name, Amount   
--     FROM dbo.PaymentTransactionMethods otxm
--     INNER JOIN dbo.PaymentMethods om ON om.PaymentMethodId = otxm.PaymentMethodId
--     WHERE otxm.PaymentTransactionId = tx.PaymentTransactionId
--     ) AS SourceTable  
--     PIVOT  
--     (  
--     SUM(Amount)  
--     FOR Name IN ([Cash], [Chase], [Discover], [TJMAXX], [PayPal])  
--     ) AS PivotTable;
-- ) as pivotm 
CREATE PROC FI_SP_ConsBeneficiario
	@ID BIGINT
AS
BEGIN
	SELECT NOME, CPF, ID, IDCLIENTE  FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @ID
END
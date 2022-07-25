-- - - Listar os primeiros 4 clientes que tenham alguma parcela com mais de 05 dias atrasadas (Data Vencimento maior que data atual E data pagamento nula)*---- * Correção do Enúnciado: Data Vencimento (DV) deve ser menor que a Data Atual (DA), pois se DV > DA sgingifica que Data Atual não chegou na Data de Vencimento logo, ---- parcela não está atrasada. Se DA > DV, então o dia de pagar a parcela foi ultrapassado e temos um atraso!
-- Estratégia: 
---- Listar Ids dos Clientes que tenham Parcelas onde Data Atual > (Data de Vencimento + 5) e Data Pagamento seja NULL
---- Limitar resultado em até 4
---- Agrupar resultado por cliente (para remover clientes duplicados)
---- Juntar esses resultados com a tabela de clientes (para termos todos os dados do cliente inadimplente)


---- Listar Ids dos Clientes que tenham Parcelas onde Data Atual > (Data de Vencimento + 5)
---- Limitar resultado em até 4
--SELECT *
--FROM Financiamentos fin
--JOIN Parcelas par
--ON par.Id_Financiamento = fin.Id_Financiamento
--WHERE GETDATE() > DATEADD(DAY, 5, par.Data_Vencimento) AND par.Data_Pagamento IS NULL
--ORDER BY par.Data_Vencimento
--OFFSET 0 ROWS FETCH NEXT 4 ROWS ONLY

---- Agrupar resultado por cliente (para remover clientes duplicados)
--SELECT * FROM
--(
--	SELECT fin.Id_Cliente
--	FROM Financiamentos fin
--	JOIN Parcelas par
--	ON par.Id_Financiamento = fin.Id_Financiamento
--	WHERE GETDATE() > DATEADD(DAY, 5, par.Data_Vencimento) AND par.Data_Pagamento IS NULL
--	ORDER BY par.Data_Vencimento
--	OFFSET 0 ROWS FETCH NEXT 4 ROWS ONLY
--) AS cli_atrasos
--GROUP BY cli_atrasos.Id_Cliente


---- Juntar esses resultados com a tabela de clientes (para termos todos os dados do cliente inadimplente)
SELECT cli_data.* FROM (
	SELECT * FROM
	(
		SELECT fin.Id_Cliente
		FROM Financiamentos fin
		JOIN Parcelas par
		ON par.Id_Financiamento = fin.Id_Financiamento
		WHERE GETDATE() > DATEADD(DAY, 5, par.Data_Vencimento) AND par.Data_Pagamento IS NULL
		ORDER BY par.Data_Vencimento
		OFFSET 0 ROWS FETCH NEXT 4 ROWS ONLY
	) AS cli_atrasos
	GROUP BY cli_atrasos.Id_Cliente
) AS cli_unic_atraso
JOIN 
(
	SELECT cli.Id_Cliente, cli.Nome, uf.Sigla, cli.Celular
	FROM Clientes cli
	JOIN UF uf
	ON cli.UF = uf.Id_UF
) AS cli_data
ON cli_unic_atraso.Id_Cliente = cli_data.Id_Cliente
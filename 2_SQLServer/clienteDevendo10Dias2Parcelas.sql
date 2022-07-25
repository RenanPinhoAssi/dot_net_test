-- - - Listar todos os clientes que já atrasaram em algum momento duas ou mais parcelas em mais de 10 dias, e que o valor do financiamento seja maior que R$ 10.000,00.

-- Estratégia: 
---- Listar Parcelas, com os IDs dos clientes, onde Data Atual > (Data de Vencimento + 10) e Data Pagamento seja NULL e Valor_Total > 10000
---- Agrupar parcelas por ID do cliente e contar quantas parcelas no nome do cliente
---- Filtrar para apenas clientes com >= 2 Parcelas atrasadas
---- Juntar esses resultados com a tabela de clientes (para termos todos os dados do cliente inadimplente)


---- Listar Parcelas, com os IDs dos clientes, onde Data Atual > (Data de Vencimento + 10) e Data Pagamento seja NULL e Valor_Total > 10000
--SELECT *
--FROM Financiamentos fin
--JOIN Parcelas par
--ON par.Id_Financiamento = fin.Id_Financiamento
--WHERE fin.Valor_Total > 10000 AND GETDATE() > DATEADD(DAY, 10, par.Data_Vencimento) AND par.Data_Pagamento IS NULL


---- Agrupar parcelas por ID do cliente e contar quantas parcelas no nome do cliente
--SELECT fin.Id_Cliente, COUNT(par.Id_Parcela) AS Par_Atr_10
--FROM Financiamentos fin
--JOIN Parcelas par
--ON par.Id_Financiamento = fin.Id_Financiamento
--WHERE fin.Valor_Total > 10000 AND GETDATE() > DATEADD(DAY, 10, par.Data_Vencimento) AND par.Data_Pagamento IS NULL 
--GROUP BY fin.Id_Cliente


---- Filtrar para apenas clientes com >= 2 Parcelas atrasadas
--SELECT * FROM 
--(
--	SELECT fin.Id_Cliente, COUNT(par.Id_Parcela) AS Par_Atr_10
--	FROM Financiamentos fin
--	JOIN Parcelas par
--	ON par.Id_Financiamento = fin.Id_Financiamento
--	WHERE fin.Valor_Total > 10000 AND GETDATE() > DATEADD(DAY, 10, par.Data_Vencimento) AND par.Data_Pagamento IS NULL 
--	GROUP BY fin.Id_Cliente
--) AS cli_atrasados
--WHERE cli_atrasados.Par_Atr_10 >= 2

---- Juntar esses resultados com a tabela de clientes (para termos todos os dados do cliente inadimplente)
SELECT cli_data.* FROM (
	SELECT * FROM 
	(
		SELECT fin.Id_Cliente, COUNT(par.Id_Parcela) AS Par_Atr_10
		FROM Financiamentos fin
		JOIN Parcelas par
		ON par.Id_Financiamento = fin.Id_Financiamento
		WHERE fin.Valor_Total > 10000 AND GETDATE() > DATEADD(DAY, 10, par.Data_Vencimento) AND par.Data_Pagamento IS NULL 
		GROUP BY fin.Id_Cliente
	) AS cli_atrasados
	WHERE cli_atrasados.Par_Atr_10 >= 2
) AS cli_unic_atraso
JOIN 
(
	SELECT cli.Id_Cliente, cli.Nome, uf.Sigla, cli.Celular
	FROM Clientes cli
	JOIN UF uf
	ON cli.UF = uf.Id_UF
) AS cli_data
ON cli_unic_atraso.Id_Cliente = cli_data.Id_Cliente
-- - Listar todos os clientes do estado de SP que tenham mais de 60% das parcelas pagas.

-- Estratégia: 
---- Filtrar os clientes por SP
---- Contar quantidade de parcelas de um financiamento agrupadas por clientes
---- Contar quantidade de parcelas abertas de um financiamento agrupadas por clientes
---- Juntar os resultados filtrando por porcentagem de parcelas pagas > 60%

---- Filtrar os clientes por SP
--SELECT cli.Id_Cliente, cli.Nome, uf.Nome As 'Cidade', uf.Sigla 
--FROM Clientes cli JOIN UF uf 
--ON cli.UF = uf.Id_UF AND uf.Sigla = 'SP'


---- Contar quantidade de parcelas de um financiamento agrupadas por clientes
---- Contar quantidade de parcelas abertas de um financiamento agrupadas por clientes
--SELECT fin.Id_Cliente, COUNT(CASE WHEN par.Data_Pagamento is NULL THEN 1 END) As [Parcelas Abertas], COUNT(fin.Id_Financiamento) As Parcelas
--FROM Financiamentos fin 
--JOIN Parcelas par 
--ON par.Id_Financiamento = fin.Id_Financiamento
--GROUP BY fin.Id_Cliente



-- - Listar Clientes de São Paulo com pelo menos 60% das parcelas pagas
SELECT cli_sp.Id_Cliente, cli_sp.Nome, cli_sp.Sigla, cli_sp.Celular, (par_fin_filtered.[Parcelas Abertas] * 100 / par_fin_filtered.Parcelas) As '% Parcelas Pagas'
FROM 
(
	SELECT cli.*, uf.Sigla FROM Clientes cli JOIN UF uf ON cli.UF = uf.Id_UF AND uf.Sigla = 'SP'
) AS cli_sp
JOIN 
(
	SELECT * FROM 
	(
		SELECT fin.Id_Cliente, COUNT(CASE WHEN par.Data_Pagamento is NULL THEN 1 END) As [Parcelas Abertas], COUNT(fin.Id_Financiamento) As Parcelas
		FROM Financiamentos fin 
		JOIN Parcelas par ON par.Id_Financiamento = fin.Id_Financiamento
		GROUP BY fin.Id_Cliente
	) AS par_fin
	WHERE par_fin.[Parcelas Abertas] * 100 /  par_fin.Parcelas > 60
) AS par_fin_filtered
ON par_fin_filtered.Id_Cliente = cli_sp.Id_Cliente
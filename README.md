# VerificaPendenciasArquivo
Verifica se no arquivo html/asp não foi informado a tag ID, senão foi incluso, irá incluir automaticamente no novo arquivo gerado.

Exemplo:

Arquivo de entrada: `<input type="text" name="nome_campo" value="">`

Arquivo de saída: `<input type="text" name="nome_campo" id="nome_campo" value="">`

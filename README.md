# TelemetryApp v1
Essa aplicação tem por função se conectar a medidores de energia por meio de um Socket TCP e realizar a coleta de dados referentes à data de coleta e valor do registro.
Desenvolvida utilizando [.NET CORE](https://docs.microsoft.com/pt-br/dotnet/core/) 2.0, possui compatibilidade com os sistemas operacionais Windows, Linux e MacOS.

## Instruções de execução
Para executar a aplicação, basta compilar utilizando sua IDE do Visual Studio ou via linha de comando utilizando `dotnet build` na pasta raiz do solution e, em seguida, ir até a pasta do projeto "Telemetry.App" e `dotnet run <caminho do arquivo de inicialização>`.

 
O arquivo de inicialização possui a seguinte sintaxe deve contar os campos **IP**, **PORTA**, **ÍNDICE INICIAL** e **ÍNDICE FINAL** separados por um espaço simples, podendo ter mais de uma linha. Cada linha, representa uma requisição.

**Sintaxe:**
  `<IP> <PORTA> <ÍNDICE INICIAL> <ÍNDICE FINAL>`

**Exemplo: listaDePedidos.exe**
`192.168.0.1 1000 0 50`
`127.0.0.3 2200 37 400`

De maneira geral:  **<Telemetry.App.exe> listaDePedidos.txt**

Project is still early WIP (playing around with different stuff). Will update this soon when I have some more time to clean this project up and give it the love it needs.. It does work for file level encryption (cli is a nightmare..but it works)


If you're working on this and want Test Coverage Reports

1) Run: `dotnet test --collect:"XPlat Code Coverage"` (https://github.com/coverlet-coverage/coverlet)
2) Install dotnet-reportgenerator: `dotnet tool install -g dotnet-reportgenerator-globaltool`
3) Run the Tool and point at output from coverlet coverage (https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=linux#generate-reports):
```
reportgenerator -reports:Path\To\TestProject\TestResults\{guid}\coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html
```


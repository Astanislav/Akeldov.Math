# Agent Notes

## Tests

For the Spatial2D NUnit tests, build the test project first, then run the test assembly directly:

```powershell
dotnet build tests\Akeldov.Math.Spatial2D.Tests\Akeldov.Math.Spatial2D.Tests.csproj --framework net6.0 --no-restore --disable-build-servers /maxcpucount:1
```

The explicit framework and single MSBuild node avoid intermittent empty `Build FAILED` results when resolving the multi-targeted Spatial2D project reference.

```powershell
dotnet vstest tests\Akeldov.Math.Spatial2D.Tests\bin\Debug\net6.0\Akeldov.Math.Spatial2D.Tests.dll "--logger:console;verbosity=detailed"
```
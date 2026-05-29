# Agent Notes

## Spatial2D Angle Units

In `Akeldov.Math.Spatial2D`, angles are expressed in radians by default.
Angle parameters and properties must state their units in XML comments.
Non-radian members must use an explicit suffix, such as `Deg`, and document their unit.

## MkDocs Image Paths

MkDocs rewrites normal Markdown links, but raw HTML image tags such as `<img src="...">`
are emitted as-is.
When using raw HTML images, calculate `src` relative to the published page URL directory,
not relative to the source `.md` file.

For example, `docs/Spatial2D/learn/curves.md` is published at
`Spatial2D/learn/curves/`, so root-level assets are reached with
`../../../assets/...`.
`docs/Spatial2D/learn/curves/linear.md` is published at
`Spatial2D/learn/curves/linear/`, so root-level assets are reached with
`../../../../assets/...`.

Prefer Markdown image syntax when fixed sizing or HTML layout is not needed.
After changing raw HTML image paths, verify with:

```powershell
python -m mkdocs build --strict --site-dir .mkdocs-site-temp
```

Then check the generated HTML image `src` values resolve to files under
`.mkdocs-site-temp\assets\...`, and remove `.mkdocs-site-temp` after verification.

## Tests

For the Spatial2D NUnit tests, build the test project first, then run the test assembly directly:

```powershell
dotnet build tests\Akeldov.Math.Spatial2D.Tests\Akeldov.Math.Spatial2D.Tests.csproj --framework net6.0 --no-restore --disable-build-servers /maxcpucount:1
```

The explicit framework and single MSBuild node avoid intermittent empty `Build FAILED` results when resolving the multi-targeted Spatial2D project reference.

```powershell
dotnet vstest tests\Akeldov.Math.Spatial2D.Tests\bin\Debug\net6.0\Akeldov.Math.Spatial2D.Tests.dll "--logger:console;verbosity=detailed"
```

# Semiology Atlas

Cross-platform .NET MAUI teaching app for general semiology, neurological movement disorders, and common bedside syndromes by system.

## Included teaching areas

- General appearance and first impression
- Neurological screening and movement phenomenology
- Cardiovascular bedside clues
- Respiratory pattern recognition
- Gastrointestinal and hepatology survey
- Dedicated movement-disorders lab with hypokinetic, hyperkinetic, and gait patterns
- Rapid quiz for bedside distinctions

## Build targets validated in this workspace

- `dotnet build SemiologyAtlas.csproj -f net9.0-windows10.0.19041.0`
- `dotnet build SemiologyAtlas.csproj -f net9.0-android`
- `dotnet build SemiologyAtlas.csproj -f net9.0-ios`

## Android deployment

Debug build:

```powershell
dotnet build SemiologyAtlas.csproj -f net9.0-android
```

Release package:

```powershell
dotnet publish SemiologyAtlas.csproj -f net9.0-android -c Release
```

Generated outputs land under `bin/Release/net9.0-android/publish/` and include signed `.apk` and `.aab` artifacts.

## iOS deployment

Simulator build:

```powershell
dotnet build SemiologyAtlas.csproj -f net9.0-ios
```

For an iPhone or App Store build you still need Apple signing assets and a Mac-capable iOS deployment flow. A typical release command is:

```powershell
dotnet publish SemiologyAtlas.csproj -f net9.0-ios -c Release -p:RuntimeIdentifier=ios-arm64 -p:ArchiveOnBuild=true
```

In this workspace the iOS device publish reaches the archive step and then stops until code signing is configured.

## Content structure

- App shell: `Atlas`, `Systems`, `Movement`, `Quiz`
- Curriculum data: `Services/CurriculumRepository.cs`
- Models: `Models/`
- Page view models: `ViewModels/`
- Screens: `Pages/`
- Mobile Lottie-only animations: `Resources/Raw/*.json`
- Lottie asset generator: `Scripts/Generate-LottieAssets.ps1`

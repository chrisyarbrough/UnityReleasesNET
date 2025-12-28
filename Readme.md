# UnityReleasesNET

This project can tell you which versions (and revision hashes) of the Unity editor have been released.


## Static Lookup File

See [build/UnityReleases.txt](build/UnityReleases.txt) for the output of all Unity releases.
The file is updated on a schedule, currently, once a week.


## CLI Tool

A C# client for the Unity services API to fetch all editor release versions of Unity.

This tool prints the release versions and revision hashes by release date in descending order.
For example:

```
6000.0.35f1 9a3bc604008a
6000.0.34f1 5ab2d9ed9190
2022.3.56f1 dd0c98481d00
6000.0.33f1 433b0a79340b
6000.0.32f1 b2e806cf271c
```

The output can be filtered via the options provided by the _Unity Release Admin API_:
https://services.docs.unity.com/release/v1/

Additionally, versions older than a certain date can be filtered out.


### Prerequisites

- .NET 8.0 Target Framework (runtime to run the tool, sdk to build from source)


### Usage

Invoke the help to see all options:

```shell
./UnityReleases --help
```

By default, all versions are shown:

```shell
./UnityReleases
```

Only show versions that match a full text search:

```shell
./UnityReleases -v 2022.3
```

Only show LTS and TECH stream versions:

```shell
./UnityReleases --stream LTS TECH
```

Only show versions released since a date:

```shell
./UnityReleases --since-date 2025-12-01
```

The date parsing can include a time but needs to be one of the standard formats supported by .NET `DateTime.Parse`.
Only show versions younger than 3 years using the macOS `date` command:

```shell
./UnityReleases -d $(date -v-3y -Iseconds)
```

Combined example:

```shell
./UnityReleases -l 15 -s LTS TECH -p MAC_OS -a ARM64 -d 2022-12-01
```


## Development

Remember to separate the options from the `dotnet run` command:

```shell
cd UnityReleases
dotnet run -- --help
```


### Publishing

```shell
dotnet build -c Release -r osx-arm64
dotnet build -c Release -r win-x64
dotnet build -c Release -r linux-x64
```

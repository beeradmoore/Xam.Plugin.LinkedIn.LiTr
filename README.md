# Xam.Plugin.LinkedIn.LiTr 


Xamarin.Android bindings for [LinkedIn.LiTr](https://github.com/linkedin/LiTr) v1.5.1

## Installation

LiTr is available as two seperate nuget packages. Xam.Plugin.LinkedIn.LiTr is a binding for the [LiTr core library](https://github.com/linkedin/LiTr/tree/main/litr). Xam.Plugin.LinkedIn.LiTr.Filters is a binding which contains the additional [filters library](https://github.com/linkedin/LiTr/tree/main/litr-filters).

| Package        | Current Version   |
|-----------------|--------|
| [Xam.Plugin.LinkedIn.LiTr](https://www.nuget.org/packages/Xam.Plugin.LinkedIn.LiTr/) | [![NuGet](https://img.shields.io/nuget/vpre/Xam.Plugin.LinkedIn.LiTr.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.LinkedIn.LiTr) |
| [Xam.Plugin.LinkedIn.LiTr.Filters](https://www.nuget.org/packages/Xam.Plugin.LinkedIn.LiTr.Filters/) | [![NuGet](https://img.shields.io/nuget/vpre/Xam.Plugin.LinkedIn.LiTr.Filters.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.LinkedIn.LiTr.Filters) |






## Usage

After you add the nuget package the examples listed over on the [LiTr](https://github.com/linkedin/LiTr) repository should work the same with the normal Xamarin.Android differences, eg:

```
MediaTransformer mediaTransformer = new MediaTransformer(getApplicationContext());

...

mediaTransformer.release();
```

would become 


```
MediaTransformer mediaTransformer = new MediaTransformer(ApplicationContext);

...

mediaTransformer.Release();
```


## Build

### Building a .nupkg
1. Navigate to `Binding` directory and run BuildNugetPackage.sh
    ``` sh
    $ cd Binding/
    $ ./BuildNugetPackage.sh
    ```
2. Fetch your Xam.Plugin.LinkedIn.LiTr.x.y.z.nupkg and Xam.Plugin.LinkedIn.LiTr.Filters.x.y.z.nupkg from the Binding directory.


## TODO
* Try figure out how to reduce the number of warnings.
* Improve example app




Pull requests welcome!
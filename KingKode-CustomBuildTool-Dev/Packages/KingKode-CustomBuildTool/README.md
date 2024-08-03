# KingKodeUnityPackages: CustomBuildTool
This package is used to add the a CustomBuildTool to your project.

## What Is This
You can use this package to manage your builds easily.

## How to Install
Insert the following link into package manager:
> https://github.com/ALI_P48/KingKodeUnityPackages.git?path=/KingKode-CustomBuildTool#v0.0.1


Also make sure you add the following in the main packages manifest.json file:
```json
  "scopedRegistries": [
		{
			"name": "package.openupm.com",
			"url": "https://package.openupm.com",
			"scopes": [
				"com.google.android.appbundle",
				"com.google.external-dependency-manager",
			]
		}
	]
```

## Dependencies
- com.google.android.appbundle
- com.google.external-dependency-manager
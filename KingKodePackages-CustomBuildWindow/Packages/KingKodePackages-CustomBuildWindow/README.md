# KingKodePackages: CustomBuildTool
This package is used to add the a CustomBuildTool to your project.

## What Is This
You can use this package to manage your builds easily.

## How to Install
Add the following line to dependencies of main manifest.json file:

```json
"com.kingkodepackages.custombuildtool": "https://github.com/ALI-P48/KingKodePackages.git?path=/KingKodePackages-CustomBuildTool"
```

Also make sure you add the following in the main packages manifest.json file:
```json
"scopedRegistries": [
	{
		"name": "package.openupm.com",
		"url": "https://package.openupm.com",
		"scopes": [
			"com.google.android.appbundle",
			"com.google.external-dependency-manager",
			"com.google.play.common",
			"com.google.play.core",
			"com.google.play.review"
		]
	}
]
```
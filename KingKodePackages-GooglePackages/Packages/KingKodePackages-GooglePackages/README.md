# KingKodePackages: GooglePackages
This package is used to add the Google packages to your project.

## What Is This
You can add the Google packages from web instead of donwloading local files with is recomended. This package contains:
- com.google.external-dependency-manager #1.2.172
- com.google.play.common #1.8.1
- com.google.play.core #1.8.1
- com.google.play.review #1.8.1
- com.google.android.appbundle #1.8.0

## How to Install
Add the following line to dependencies of main manifest.json file:

```json
"com.kingkodepackages.googlepackages": "https://github.com/ALI-P48/KingKodePackages.git?path=/KingKodePackages-GooglePackages"
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
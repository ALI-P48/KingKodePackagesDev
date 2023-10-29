# KingKodePackages: InAppReview

## What Is This
KingKodePackages InAppReview package is used to implement rating and in app review.

## How to Install
Add the following line to dependencies of main manifest.json file:

```json
"com.kingkodepackages.inappreview": "https://github.com/ALI-P48/KingKodePackages.git?path=/KingKodePackages-InAppReview"
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

## Dependencies
- com.google.play.review
- com.google.android.appbundle
- com.google.external-dependency-manager
- com.google.play.common
- com.google.play.core
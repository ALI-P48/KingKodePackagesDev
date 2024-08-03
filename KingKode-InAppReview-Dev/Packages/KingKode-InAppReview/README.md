# KingKodePackages: InAppReview

## What Is This
KingKodePackages InAppReview package is used to implement rating and in app review.

## How to Install
Insert the following link into package manager:
> https://github.com/ALI-P48/KingKodePackages.git?path=/KingKode-InAppReview

Also make sure you add the following in the main packages manifest.json file:
```json
"scopedRegistries": [
	{
		"name": "package.openupm.com",
		"url": "https://package.openupm.com",
		"scopes": [
			"com.google.play.review",
		]
	}
]
```

## Dependencies
- com.google.play.review
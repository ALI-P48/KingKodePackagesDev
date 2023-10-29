# KingKodePackages: InAppReview

## What Is This
KingKodePackages InAppReview package is used to implement rating and in app review.

## Dependencies
- com.google.play.review
- com.google.android.appbundle
- com.google.external-dependency-manager
- com.google.play.common
- com.google.play.core

Make sure you add the following in the main packages manifest.json file:
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
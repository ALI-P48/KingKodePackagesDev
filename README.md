# Dee1UnityPackages - Dev
This is the development repository of Dee1 Unity packages.

## Steps to Add a New Package:
1. Clone the Dee1PackagesDev repository.

2. In Dee1PackagesDev root folder, Copy the "_TEMPLATE" folder and rename the new folder to:
"**Dee1-<YOUR_PACKAGE_NAME>-Dev**".

3. This new folder is going to be your workspace for your new package.

4. Open up the project.

5. Head to the Packages section and find the "Dee1: Template" folder in packages.

6. Rename the "Dee1: Template" folder in windows file explorer to "**Dee1-<YOUR_PACKAGE_NAME>**".

7. In Unity, in your package folder, you will find a "package.json" file which is your **Package Manifest**. Edit the package manifest fields (Name, Display name, Version, Description) and hit apply.

8. Also edit the first line of "LICENSE.md" file and replace your package name.

9. After that, edit the "README.md" file and replace your package name and info into that.

10. There are two sub-folders (Editor/Runtime). Insert your code and assets into these folders and delete any of them if they are useless for you.

11. Each one of these folders contain a Assembly Definition which you have to rename the file and name of them and replace your own package name.

12. If you have other files and folder that are not meant to be in Runtime/Editor folders, here is a standard folder structure for packages:
```
<package-root>
  ├── package.json
  ├── README.md
  ├── CHANGELOG.md
  ├── LICENSE.md
  ├── Third Party Notices.md
  ├── Editor
  │   ├── <company-name>.<package-name>.Editor.asmdef
  │   └── EditorExample.cs
  ├── Runtime
  │   ├── <company-name>.<package-name>.asmdef
  │   └── RuntimeExample.cs
  ├── Tests
  │   ├── Editor
  │   │   ├── <company-name>.<package-name>.Editor.Tests.asmdef
  │   │   └── EditorExampleTest.cs
  │   └── Runtime
  │        ├── <company-name>.<package-name>.Tests.asmdef
  │        └── RuntimeExampleTest.cs
  ├── Samples~
  │        ├── SampleFolder1
  │        ├── SampleFolder2
  │        └── ...
  └── Documentation~
       └── <package-name>.md
```
13. It's done! but you now have to publish your package to Dee1Packages repository. To do that, clone the repository and copy your package from "Dee1PackagesDev\Dee1-<YOUR_PACKAGE_NAME>-Dev\Packages\Dee1-<YOUR_PACKAGE_NAME>" into the Dee1Packages root directory.

14. Make sure you publish the changes in both Dee1Packages and Dee1PackagesDev repositories to GitHub.

> **Thanks for making a good change <3**

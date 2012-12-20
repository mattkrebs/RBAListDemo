

###GETTING STARTED###
At the time of creating this project, Wsh Lst makes heavy use of the latest version of MvvmCross which in turn uses Portable Class libraries (PCL’s) extensively.  At this time, there are a few tweaks you must make to your system(s) before you may be able to compile the project.  The main issue is that the Mono for Android and MonoTouch profiles do not recognize Portable Class Libraries (PCL’s) as valid profile types to reference.  We need to ‘trick’ visual studio into allowing us to reference these PCL’s.

#####Windows Setup:#####
Since this app uses prebuilt binaries of MvvmCross, it should build for you as is, however if you have porblems getting the Android app to build, we need to trick Visual Studio Mono for Android projects to be able to reference Portable Class Libraries:

1. Open the folder: *C:\Program Files (x86)\Referenced Assemblies\Microsoft\Framework\.NETPortable\v4.0\Profile\Profile104\SupportedFrameworks\\*
2. Create a new file named *MonoAndroid,Version=v1.6+.xml* with the following contents:

	```
	<?xml version="1.0" encoding="utf-8"?>
	<Framework DisplayName="Mono for Android"
	  Identifier="MonoAndroid"
	  Profile="*"
	  MinimumVersion="1.6"
	  MaximumVersion="*" />
	```
3. If you had Visual Studio open, you'll need to restart it



#####Mac Setup:######
At this time, you should have no problems opening the MonoTouch and Mono for Android projects on the mac.

If you do have issues, try the following:

1. Edit the file */Library/Frameworks/Mono.framework/Versions/Current/lib/mono/xbuild/Microsoft/Portable/v4.0/Microsoft.Portable.CSharp.targets*
2. Find the PropertyGroup that sets *<TargetFrameworkIdentifier>MonoTouch</TargetFrameworkIdentifier>*
3. Ensure the following lines exist in this PropertyGroup:

	```
	<CscToolExe>smcs</CscToolExe>
	<CscToolPath>/Developer/MonoTouch/usr/bin</CscToolPath>
	```


#####Azure Setup:#####
1. Create a new Azure Mobile Service and open its dashboard
2. Note the *MOBILE SERVICE URL* (eg: https://wshlst.azure-mobile.net) on the right hand side
3. Click the *Manage Keys* button at the bottom.  Note the Application Key.
4. Edit the *WshLst.Core\Config.cs* file
	1. Set the AZURE\_MOBILE\_SERVICE\_URL constant value to the URL from step 2 (make sure you do NOT have a trailing slash)
	2. Set the AZURE\_MOBILE\_SERVICE\_APPKEY constant value to the Application key from step 3
5. In your Azure Mobile Service, Create the following Data tables (their columns will be dynamically created at runtime):
	1. WishList
	2. Entry
	3. EntryImage
6. Open the *Azure-Table-Scripts.js* file from this repository and copy/paste the corresponding scripts for each table's read/insert/update/delete operations in the azure portal
7. In your Azure Mobile Service's *Identity* configuration tab, setup the correct keys/secrets for all the authentication providers.  You will need to follow the Azure help section to setup applications on each authentication provider (eg: Twitter, Facebook, Google, Microsoft).
8. Create a new Azure Website and open its dashboard
9. Download the new website's publishing profile
10. Open the *WshLst.Web.sln* solution, build the web site, and publish it using your new website's publishing profile.
11. Edit the *WshLst.Core\Config.cs* file again
	1. Set the AZURE\_WEBSITE\_URL to the url of the website you just made (be sure you do NOT include the trailing slash)

#####OPTIONAL#####
Without these optional steps you will be unable to utilize the Google Places or Barcode Scanning features:

1. Signup for a Google Places API, note your API Key
2. Signup for a Scandit API key, note the key
3. Edit the *WshLst.Core\Config.cs* file again
	1. Set the GOOGLE\_PLACES\_API\_KEY to the key you just created
	2. Set the SCANDIT\_API\_KEY to the key you just created

----------

###TROUBLESHOOTING###

1. iOS App Crashes - Try increasing the number of trampolines: [http://docs.xamarin.com/ios/troubleshooting#Ran_out_of_trampolines_of_type_2](http://docs.xamarin.com/ios/troubleshooting#Ran_out_of_trampolines_of_type_2)


----------

###PROJECTS###
-	Xamarin.Mobile - http://xamarin.com/mobileapi
-	ZXing.Net.Mobile - https://github.com/Redth/ZXing.Net.Mobile
-	MvvmCross - https://github.com/slodge/MvvmCross
-	Azure Mobile Services - https://github.com/xamarin/azure-mobile-services




----------

##LICENSE##
Apache WshLst Copyright 2012 The Apache Software Foundation

This product includes software developed at The Apache Software Foundation (http://www.apache.org/).

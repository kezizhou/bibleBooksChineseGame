# bibleBooksChineseGame
A matching game for Chinese and English Bible books

## Sections:  
[Installation](#installation)  
[Sample Screenshots](#sample-screenshots)  

***
### Installation

To install, download [this](https://github.com/kezizhou/bibleBooksChineseGame/blob/master/publish/setup.exe) file.

#### Common Issues:

##### Administrator has blocked install
You may see an error like this that blocks the install:
![Blocked Install Security Warning](documentation/blockedSecurityWarning.png)

If you are the administrator account and are able to change computer settings: 
* Search for "Registry Editor" or "regedit" on your computer.
* Find the Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\Security\TrustManager\PromptingLevel security settings as shown below.
* Ensure that "Internet" is set to "Enabled". If it is disabled, double click "Internet" and change the data to be set to "Enabled".
![Change the "Internet" security setting](documentation/changeSecuritySettings.png)
* Note: Changing this setting will still allow you to decide whether or not to install applications from unknown publishers. You will still be warned, but you will be allowed to install if you choose to.

When these steps have been completed, you should see a window like this on the install: 
![Correct Install Security Warning](documentation/correctSecurityWarning.png)

**[Back to Top](#bibleBooksChineseGame)**

***
### Sample Screenshots

![Main Menu](documentation/mainMenu.png)
![Hebrew Scriptures Match Chinese to English](documentation/hebrewMatch.png)
![Greek Scriptures Match Chinese to Englishs](documentation/greekMatch.png)

**[Back to Top](#bibleBooksChineseGame)**
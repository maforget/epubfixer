-------------------------------------------
Introduction
-------------------------------------------

Calibre and SIgil are 2 great program for anyone interrested in eBooks, but I always felt that there was something missing.

Whenever I tried to convert a book, there was always something missing, so I created this program to help fill the void.


-------------------------------------------
Functionality
-------------------------------------------


** Table of Content Editor **

- Have you ever wanted to just edit the Table of Content order or just the different Level? Just need to Drag and Drop.
- Remove unwanted entries, Add new ones based on files or Anchor already present in the files.
- Just Rename the text? You can Mass Rename chapters by just typing a number when selecting the chapters, the will increment automatically.
	- You can also Convert number to Words (ie 1 --> One), It also works with Roman Numerals
	- You can also use the special tag "%T", that will keep the previous text. (ex : You have a Chapter Named : Atlantis, Typing 1 - %T will change the name to 1 - Atlantis). Also if you have a Chapter 1 entry you can just type %T and Number To Words and it will change it to Chapter One.
- Have you ever converted a book and the Table of COntent pointed to the end of the previous chapter? You can shift the source of chapters up or down (It will Remove any Anchors, pointing to the top of the page).
- You can add any html file to the Table of Content or any Anchor Found, The program will also present you with a list of the top 5 lines inside the files (or inclosed with the Anchor Tags) for the Chapter Name.
- The program can cut your chapter to seperate files, instead of multiple chapters per file.


** Reading Order Editor **

- Have you ever opened a book to find out the first page is at the end? You can reorder the order of the book pages or just remove some.



** Margin Fixer **

- This will change all the left and right margins value of the css file, while keeping the body tag to 5pt. It will also change any ident that are negative to 0
(Be carefull because some books use margins, instead of ident and it could remove them)



** Edit in Sigil **

- Sigil is great to cut chapters, Format the text or to correct some incorrectly cut files, but it is missing a very important function (at least before version v0.4 comes out). It does not keep the Table of COntent that is already present. Using this the book will be opened with Sigil, and when closed (be sure to save the file at the same place) it will replace it with your old Table of Content, Updating the new files location. If you delete or add new Files be sure to also run the table of Content & Reading ORder Editor after it. It will check for any files that are missing when opening and remove them, so just open it and save it again to remove entries.


You can use the Open With plugin to easily open your file from calibre
http://www.mobileread.com/forums/showthread.php?t=118761

Nigol

Please direct any comment or bugs here : http://code.google.com/p/epubfixer/


-------------------------------------------
Change Log
-------------------------------------------

v1.4.0
- Using SendTo will Save the most recent files
- Opening a file that does not exists anymore will not crash the program and a message will warn you.
- Added a Delete Files in the Add screen (it will remove the selected files from the ePub and delete entries from the manifest & spine)
- Fixed a bug with Mass Rename if the entry selected had different "parents".
- Fixed a crash if a file was in the spine, but not the manifest.
- Added a kind of a HACK to force files with XML version v1.1 to be parsed.
- If multiple css file are present, It will now (hopefully) select the correct one.
- The CSS tag being for margins is now determined by the first class attribute seen (previously it was the body tag, now it will look further if not found)
- The Option from the Mass Rename tool-Words are UpperCase-has changed, instead you will have Convert To Uppercase & To Title Case. It will work as Described but now doesn't work only on Number that where converted to Words but on all the text.
- Removed Wall of Text from the Mass Rename tool and added a help button that will show it for info.
- Added a Download button in the Add window, that will search the site Kobobooks for the TOC and add the entries found to the detected text menu.
- Added a Select Next Text (Incremental) in the Add window, to help selecting the chapters when using the Download button.
	(First item will increment by 1, the second by 2, etc. So use Select Next Text to select up to the first entry of the downloaded text and then choose Select Next Text (Incremental).
	(When using Select Next Text (Incremental), first item will not increment.)


v1.3.6 
- Put a Check that will prevent any none html file to be loaded or saved in the TOC.
- Changed the Version.xml location on the web, so it is more easily changed. Older version might not always show the new version number.
- Multiple Tweaks to Splitting so more file are compatible and do not crash.
- Changed the way settings are stored, so they are kept regardless of version, build or file location. 
	(The default MS was to have a setting file for each location and each version, so moving the executable would have all your setting lost.)
- Other small tweaks and fixes.
- A message will popup when opening a protected file, telling you it is protected and preventing you from editing this file.
- Added a Expand & Collapse All option in the Add window.
- When adding new Chapters, the text that was already selected in the Add window will now be remembered.
- In the Add window, clicking the columns header will sort the files.
- Added a Recent Files list under a new File Menu in the Main window.
- Fixed a Couple a Bug and some tweaks.


v1.3.5
- No more error when Saving at the same time as extracting, or closing when there are still file activity.
- Fixed a Bug when saving multiple times would lead to the file not being able to be opened again.

v1.3.4
- Found a situation where the Spine did not have a reference to the TOC (on retail books), So the App will look deeper if this situation arise.
- If the above situation arises opening the Reading Order Editor and Saving will force the reference into the spine. (a message box will open warning you about it)


v1.3.3
- The Show All checkbox will not be remembered anymore.
- The List of Anchors for each file will now be cached for each different Book. 
- The Detected Text will now be cached for each different Book, along with the Anchors text.
- Removed the Apply button, it was replaced by a Add button.
- Using the Add command from the context menu will Add all the items under the selected node (instead of at the end of the document).
- The Save button will no longer close the window (and each related windows, exception is split chapters). You will now need to Save and Close.
- The file column width from the Add screen, will no longer have a maximum width. 
- Also resizing the form will no longer adjust the columns width.
- The program will now correctly get the filenames for the NCX & OPF files 
	- For the OPF file it will get the path from the container.xml file (it used to check only for opf extension)
	- For the NCX it will get the id from the spine and lookup the file associated with it in the manifest (it used to check for toc.ncx)
- In the Add window all files or anchors present in the TOC will be shown in Green.
- In the Add window with Show All deselected, Anchors present in the TOC will no longer appear. 
- The Forms can now get on top of each other when opened and all will close when the main window closes (TOC or Reading Order editor).
- When opening the Add window, there will no longer be any validating to see if the files shown really exists in the ePub. It will instead 
check if the file is present in the TOC when saving the ePub.
- When draging and dropping Nodes they are no longer removed (with Show All off).
- The Add window will now remember if the nodes are Expanded when clicking Show All or Show Anchors.
- Fixed a problem with Reading order editor showing anchors.
- Shifting Chapters will now get the full source (with anchor) if the next/previous entry is the same file with a different anchor (before it just returned the top of the file).
- Fixed a problem when the source in the TOC was encoded with special caracters (ex: %20 instead of a space).

v1.3.2
- Upon more investigation the previous bug fix was because of some files where Sigil changed the case. 
It will now ignore the case of the files when restoring it.

V1.3.1 
- Fixed another Bug with Edit in Sigil not working, when the files were not in the root folder of the zip.

v1.3
- Clicking The Add button in the Add window will no longer close the window.
- The entries in the Add window will now be sorted like the reading Order (spine).
- Added a Save Backup Checkbox on the Main Screen.
- Using Send To from a folder would crash the program, it will now collect all the .ePub found inside. 
Useful for mass fixing from the calibre library.
- Added an option to check online if any new version is available (Default is enabled)
- The detected text for anchors, use to only find the text next to that anchors, it will now get the folowing lines. (up to 5)
- The program will now remember the Size and Position of the most windows.
- The Show All & Show Anchor Will now be remebered along with column width.
- Added a Split Chapters on Anchor checkbox in the TOC Editor, 
that will Split your Html file into multiple files when more than 1 chapter per file is found. (Is pretty slow right now)
- Fixed a bug with the edit in Sigil, where source that had Anchors where removed.
- Fixed a Bug with the Shift function (Again).
- Added an About box inside in the main dialog.
- Added a status strip inside of the preview window that will show the name of the file being previewed.
- Added a couple of tooltips on checkboxes



v1.2
- The Show All & Show Anchor Checkbox from the add windows are no longer shown when in Edit Reading Order Editor.
- Added a Remove Option in the Reading & TOC Editors, You could have done the same thing by deselecting and clicking apply, but it was not intuitive.
- The files are now being extracted in a background Theard, the Editors window will now appear much more quickly.
- A Progress Bar has been added to the bottom of the screen showing the extracting progress.
- Fixed a Crash with the Show Anchors Checkbox when certain files were in the manifest but not in the ePub.
- Added a new column in the Add Screen that Shows a list of Text (now cap to 5 lines) inside of a File or following an Anchor, The selected text will be used if present when adding files to the TOC (instead of the filename).
- Draging Items with Anchors from the Add Window to the TOC would Add all the Anchors with it.
- Fixed a Bug with Shift Up & Down not working correctly.
- Added a Take Next Text in The Add Window. That way say the first line is a chapter number and the second the title of the chapter, you can easily select the title by selecting all the wanted lines.
- Added a Special Tag in the Mass Rename (%T) that will keep the text in place. Useful with the above feature where you would want to add say a number in front to auto-increment.
(example : Chapter Name could become 1 - Chapter Name by typing 1 - %T)
	Also please note that any number in %T will also be detected (only the First one), So if %T is Chapter 1, putting Number to Words will result in Chapter One, if %t only is written. If You Write 1 - %T and %T is Chapter 1, you will have One - Chapter 1.
- Selecting multiple chapter in not the right order would make the renaming incorrect, Now the will all be renamed by the way the are on the screen regardless of the order that they were selected.



v1.1
- The Duplicate function was only duplicating 1 file when several were selected
- Added a Show All checkbox into the Add window, to show files that were already added
- Added a Show Anchor checkbox into the Add window, that will parse the html and show all the id tag into the file
- Changed Algorithm that checks for html file, it will now look for all file and media-type that Contains or Ends with html
- The preview into the Add window was only showing 1 preview when multiple were selected, it will now preview all of them


v1.02
- Fixed a Bug where the Temp Files where being Extracted over and OVer again
- Added a Duplicate Options
- Files with no source are now mark in red

v1.01
- When the Add dialog in shown it checkes for any type of file html inside the manifest, will now also look to see if the file extension finishes with html
- Files for the preview where extracted when the TOC and reading order was opening, they now wait for it to be opened. (Large files add a small delay)
- The program was looking for the Sigil app in the usual place, and if it was found use it in place, Removed the check, you will now have to set it first.
- Added some wait cursor when the TOC and Reading order where opening and when the files were being extracted.


** v1 First Release








 



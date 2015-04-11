# epubfixer

---

# Functionality #

---


The best way to use it is to add it to your Open With dialog (note that with multiple files selected, there seems to be a bug in windows that just opens the first book. But multiple Open With has been removed from Windows 7, or just put a link in the send to folder).

You can use the Open With plugin to easily open your file from calibre.


## Table of Content Editor ##

  * Reorder chapters, nest chapters inside others chapter / book and create a multi-level Table of Content by dragging and dropping.
    * When dragging look for the black line to know where your chapters will be.
    * Drag a chapter on an other one until the destination chapter is all black, this will nest the chapters inside each other.
  * Remove unwanted entries, Add new ones based on files or Anchor already present in the files. The Add window will present you with a list of the top 5 lines inside the files (or inclosed with the Anchor Tags) for the Chapter Name.
    * You can also download chapters from the site kobobooks, when there is no suitable text found.
    * Clicking the Add button will add new entries at the bottom, right clicking will add the chapters below the chosen chapters).
    * You can also drag & drop files from the Add windows inside the Table of Content.
  * You can Mass Rename chapters by just typing a number when selecting the chapters, the will increment automatically.
    * You can also Convert number to Words (ie 1 --> One), It also works with Roman Numerals.
    * You can also use the special tag "%T", that will keep the previous text. (ex : You have a Chapter Named : Atlantis, Typing 1 - %T will change the name to 1 - Atlantis). Also if you have a Chapter 1 entry you can just type %T and Number To Words and it will change it to Chapter One.
  * Change the case of the chapters.
  * You can shift the source of chapters up or down (It will Remove any Anchors, pointing to the top of the page).
  * The program can cut your chapter to separate files, instead of multiple chapters per file.
  * Delete files from your book along with any associated entries in the OPF file. (Look in the Add windows).
  * Create an inline TOC from your Table of content.
    * This will check the guide portion of the opf for the "toc" item, and replace it with your new Content file. If this does not exist it will create one at the end, where you can place it where you want.
    * If you want to edit an existing one, you can use the Add window and the Set as Table of Content command, this will set the file and tell the program to replace this file. Or you can use Sigil and the "Add Semantic" button and Table of Content.


## Reading Order Editor ##

  * Have you ever opened a book to find out the first page is at the end? You can reorder the order of the book pages or just remove some.
  * Using this will also remove any "linear" tags inside the books that makes some chapters appear at the end.
  * This will also recreate the file when there is no reference to you toc inside it, which makes some Table of Content unusable. (A message will popup tell you if it is the case.


## Margin Fixer ##

  * This will change all the left and right margins value of the css file, while keeping the body tag to 5pt. It will also change any indent that are negative to 0 (Be careful because some books use margins, instead of indent and it could remove them).
  * An option in the setting menu will let you remove margins instead of zeroing them, usefull with the Kobo Touch when margins can not the modified in book. Use this to remove them and it will remove all margins from the body style.


## Edit in Sigil ##

  * Sigil is great to cut chapters, Format the text or to correct some incorrectly cut files, but it is missing a very important function (at least before version v0.4 comes out). It does not keep the Table of Content that is already present. Using this the book will be opened with Sigil, and when closed (be sure to save the file at the same place) it will replace it with your old Table of Content, Updating the new files location. If you delete or add new Files be sure to also run the table of Content & Reading Order Editor after it. It will check for any files that are missing when opening and remove them, so just open it and save it again to remove entries.
  * You can also use any external exe, it will work also.

## Cover Editor ##

  * Change the Cover without having to reconvert your file (Like calibre does)(Note that using this will convert your covers to SVG).
    * There is also a from folder button that will take the cover.jpg file that is next to the ePub (useful with calibre, where you can download a new cover, and change it by clicking from folder)
  * Resize the Cover so that it fits the screen of your eReader.
    * Note that this has been tested on the Kobo Touch, it resizes the cover in a way that will have the Kobo created a cover without borders, but it might look weird in other viewer like calibre, etc (because it forces a Screen of 800 X 600)
  * Update all will use the selected settings for all the book that where selected, useful too update multiple covers at once.
  * Note that if the Cover is not explicitly set inside the opf file, it will use the first file in the book. (You can set it in the Add window)


---

# Requirement #

  * .NET Framework 3.5 or 4.
  * Mono for Linux & MacOSX (some feature might not work correctly like the preview)
    * For linux make sure "mono-runtime" & "libmono-winforms2.0-cil" are installed
    * Not tested on MacOSX, but it should work
    * Also make sure that the libtidy library is installed (should be by default)

```
    //Usage for Linux 
    mono ePubFixer.exe
```



![http://epubfixer.googlecode.com/files/MainWindow.jpg](http://epubfixer.googlecode.com/files/MainWindow.jpg)
![http://epubfixer.googlecode.com/files/TOCEditor.jpg](http://epubfixer.googlecode.com/files/TOCEditor.jpg)
![http://epubfixer.googlecode.com/files/CoverEditor.jpg](http://epubfixer.googlecode.com/files/CoverEditor.jpg)
![http://epubfixer.googlecode.com/files/TOCinLinux.jpg](http://epubfixer.googlecode.com/files/TOCinLinux.jpg)

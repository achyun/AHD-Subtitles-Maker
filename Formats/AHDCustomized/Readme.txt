// This file is part of AHD Subtitles Maker
// A program that can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This library is free software; you can redistribute it and/or modify 
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 3 of the License, 
// or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public 
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, 
// Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//

Script File Structure
---------------------
Line started with "//" get ignored. 
Empty line ignored...
Options codes start with ;
Codes surrounded with < and >

A script file should contain the following :

; AHD Customized
// Header, this should be presented at the first line of the script

; startf=hh:mm:ss.iii
// Start time format, this will set the start time timing format, see "Timing Fomrating" section.

; endf=hh:mm:ss.iii
// End time format, this will set the end time timing format, see "Timing Fomrating" section.

; durf=nnnn
// Optional: Duration format, this will set the duration timing format, see "Timing Fomrating" section.

; text_splitter=|
// Optional: Text new line splitter, this will replace new lines with a splitter provided.

; text_format=html
// Optional: Text formatting, this will use formatting you choose for styling. Can be html/ass.

// Now the subtitles data. 
; DATA
// Presentation of DATA code will start the repeated pattern that presented here for each subtitle.
<subi> - <subn> - <start> - <end> - <dur> : <text>
// <subi>: is subtitle zero-based index. For first subtitle is 0, second one is 1 ....etc
// <subn>: is subtitle number. <subn> = <subi> + 1. For first subtitle is 1, second one is 2 ....etc
// <start>: is the start time, formatted as startf.
// <end>: is the end time, formatted as endf.
// <dur>: is the duration time, formatted as durf.
// <text>: is the text, options will be applied as text_splitter and text_format

// WARNING: please set spaces between codes and splitters, if the splitter is one of these ':' , '-' , '.' , ';' , ','
// Example:
// <start>: <text> THIS IS WRONG
// <start> : <text> THIS IS RIGHT, there are spaces between codes and splitters.
// <start> (<text>) THIS IS RIGHT, the splitter is not one of ':' , '-' , '.' , ';' , ','
; NEW LINE
// This indicates a new empty line. Can be added as much as needed.

; END
// This is very important, to tell the decoder that the subtitle pattern script is finished.


Examples:
---------

SubRip should be like this:

; AHD Customized
; startf=hh:mm:ss.iii
; endf=hh:mm:ss.iii
; text_format=html

; DATA

<subn>
<start> --> <end>
<text>
; NEW LINE

; END

IMPORTANT NOTES:
----------------
. Please set spaces between codes and splitters.
  Example:
  <start>: <text> THIS IS WRONG
  <start> : <text> THIS IS RIGHT, there are spaces between codes and splitters.
. Please include either <start> and <dur> or <start> and <end> in the scripts, because of:
  * <start> code preseneted alone will not work (subtitles will never be imported)
  * After (or before) <start>, <end> or <dur> must be included, otherwise the decoder will not recognize the durations of the subtitles.

Timing Fomrating:
----------------

h-m-s-i-fxx-sfxx-n

-: a splitter. can be ':' , '-' , '.' , ';' , ','

* h: means hour. ranged between 0-23. Can be hh or h. 
  h is one digit if value < 10 (e.i 1=1, 2=2, 23=23 ...etc)
  hh means the entry is always 2 digits (e.i. 0=00, 1=01 ...etc)

* m: means minute. ranged between 0-59. Can be mm or m. 
  m is one digit if value < 10 (e.i 1=1, 2=2, 23=23 ...etc) 
  mm means the entry is always 2 digits (e.i. 0=00, 1=01 ...etc)

* s: means second. ranged between 0-59. Can be ss or s. 
  s is one digit if value < 10 (e.i 1=1, 2=2, 23=23 ...etc)
  ss means the entry is always 2 digits (e.i. 0=00, 1=01 ...etc)

* i: milliseconds. ranged between 0-999. Can be i, ii or iii
  i is one digit (e.i. 4=400, 5=500) 
  ii is 2 digits (e.i. 4=040, 5=050, 45=450 ..etc) 
  iii always 3 digits (e.i. 1=001, 2=002, 10=010, 244=244 ...etc)

* fxx: frame while xx is the framerate. (e.i. f25, f29_97 ...etc) each digit represents one frame.
  NOTE: for frame rate, use '_' instead of '.' . Example: for 29.97 FPS use 'f29_97' not 'f29.97' . 

* sfxx: sub frame while xx is the frame rate. Each digit represents one frame range.
  sfxx is one digit 
  sffxx is 2 digits

* n: means no decode. Return the second value as it is.
  n is absolute second. (i.e. 1, 2, 44, 5334 ....etc)
  nn second.milli one digit for milli (e.i. 1.4=1.400, 1.5=1.500) 
  nnn second.milli 2 digits for milli (e.i. 1.04=1.040, 1.05=050, 1.45=1.450 ..etc) 
  nnnn second.milli 3 digits for milli (e.i. 1.001, 1.002, 1.010, 1.244 ...etc) 

Examples:
hh:mm:ss,iii: timing format of SubRip, example values: 00:02:30,442 , 00:05:32,235 ...
hh:mm: example values: 00:02 , 00:05
hh.i: example values: 00.1 , 00.2, 01.3
mm:ss.ii: example values: 01:00.12 , 03:03.24, 05:22.01

NOTES:
. Never include a code more than once. For example, mm cannot be existed twice in a format.
. Splitters can be one of these: ':' , '-' , '.' , ';' , ','. Other splitters get ignored and considered part of the time.

Subtitle format file
---------------------
First lines contains the script lines that this subtitles format is exported from.
Script here is optional (i.e the subtitles can start normally after ; SUBS), in this case, when import, you'll need to select the script manually to import as.


; AHD Customized
// Header, this should be presented at the first line of the script
; startf=hh:mm:ss.iii
; endf=hh:mm:ss.iii
; text_format=html

// Descripe the format
; DATA
<subn>
<start> --> <end>
<text>
; NEW LINE
; END
// Now the subtitles data. DON'T ADD COMMENTS NOR EMPTY LINES AFTER THIS CODE.
; SUBS
1
00:00:38,295 --> 00:00:40,494
text1

2
00:00:40,799 --> 00:00:42,984
text2

3
00:00:43,223 --> 00:00:45,726
text3

4
00:00:48,054 --> 00:00:50,194
text4
....
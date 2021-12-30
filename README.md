# AHD SUBTITLES MAKER
A program that create and edit subtitles with no scripts ! 


**Please note that this is the official repository of the program, that means official updates on source and releases will be commited/published here.**

### [Download Latest Release](https://github.com/alaahadid/AHD-Subtitles-Maker/releases)
### [New to ASM ? Getting Started (Workflow And Tutorials)](https://github.com/alaahadid/AHD-Subtitles-Maker/wiki/Workflow-And-Tutorials)

## Donation

*Donation now can be done using PayPal - The safer, easier way to pay online!*

*To Donate, please click on this button below*

[![Donation](https://www.paypalobjects.com/en_US/DE/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate?hosted_button_id=KV25VFRMVKLM2)

*Also QR code can be scaned to do so !!*

![Donate QR Code](https://github.com/alaahadid/AHD-Subtitles-Maker/blob/main/QR%20Code.png)

*Thanks in advance :)*

## Introduction
AHD Subtitles Maker is a powerful tool designed to work with Windows®. It permits you to create the most common text-based subtitle formats in minutes.

AHD Subtitles Maker is written in C# using .NET Framework platform.

AHD Subtitles Maker is an application that creates subtitles automatically without the need of scripts. Also you can edit subtitles with it using the mouse ! no need to enter numbers, just move the subtitles, stretch them, synchronize them and more..... only with the mouse.
And plus, you convert between subtitle formats without losing any of accuracy.

It is a free comprehensive tool for subtitle editing and creation, with an extensive range of editing options and an impressive list of supported subtitle file formats. Thus, the program allows you to create your own subtitle streams, translate existing ones, or simply change the format of your subtitle file to fit any of the most widely used subtitle editing tools.

The list of editing functions is impressive. You can translate, insert, change, or copy the text of a selected subtitle, and you can move it from one time position to a different one with millisecond precision. You can also empty a subtitle stream from all its texts and replace them with different ones, but preserving original time positions.

It includes an auto encoding detecting feature which help user to choose the right encoding for importing and exporting, also it support all the encodings that your installed Windows® version may support.

AHD Subtitles Maker Supports the most common text-based subtitle formats that can be exported as a single file (but not inside the media file) and can be used in most media players and creation programs. It supports common subtitle formats like Power DivX / DVD Subtitles and more. Also it supports the media files types that your Windows® can support, because it depends on the Windows® media player and its media codec pack that is installed in your system. 

The beauty of this extensive list of formats is that it is classified by tool, not simply by the extension of the file, as this is usually shared by different programs. For example, you can produce many TXT files, but each will be customized according to the tool you want to use (Cavena, DVD Junior, DVD Subtitle System, Adobe Encore DVD, etc.).

AHD Subtitles Maker can get and save the ID3 tags (Synchronized Lyric). you can extract Synchronized Lyric to your project as subtitles track, work with it, and export it to another format as well. Also you can add a subtitles track as an ID3 tags (Synchronized Lyric) item and SAVE IT DIRECTLY INTO MP3 MEDIA FILE.

## Features
- Create the most common text-based subtitle formats in minutes without writing scripts !!.
- Ability to generate and show WaveForm of loaded media to line subtitles perfectly with media audio.
- Include AHD Customized Subtitles format, which allows to export and import user customized subtitles formats.
- Edit subtitles using the mouse !! don't enter numbers if you want and edit subtitle timings using the TimeLine control.
- Get access to mp3's ID3 Tag (Synchronized Lyrics) frames and manage them, load them to your project and save them to the same mp3 file.
- Use multi-tracks projects by storing subtitles data in subtitle tracks.
- Use the time format (Second, Millisecond) to guarantee the most accuracy.
- Translate subtitles using Google Translate® service.
- Spell check subtitles and support all dictionaries available at OpenOffice.com
- Supports all encoding that installed Windows® version may support.
- Convert between formats by importing them to your project, then export them to your desired format. Also convert using AHD Subtitles Convertor
- Convert subtitle formats and any text file encoding easily using AHD Encoding Converter tool.
- Synchronize subtitle timings using the synchronization tool inside the program or stand alone program "Synchronization Tool "
- Use your system's media codecs to be playable for any media kind (Audio and Video) available.
- Editable user-interface layout with save and load.
- Multilingual interface.

## System Requirements
Please make sure your pc meet these requirements to ensure the program work correctly:
- Works with Microsoft Windows® XP / Vista / 7 / 8 / 8.1 and 10.
- Processor: 1700 Mhz or higher (Intel or AMD). 
- Memory: 512 MB RAM or higher 
- Microsoft .Net Framework 4. 
- Media Codec Pack (optional to support more media files types for media playback. By default, Windows® comes with these formats listed here: <https://msdn.microsoft.com/en-us/library/windows/desktop/dd407173%28v=vs.85%29.aspx> ) 
- Drivers: DirectX 9 or higher 

## Important Notes
- When using Directshow media player, IF A MEDIA FILE CANNOT BE LOADED, please install the directshow filters required then try again. 
  Windows® comes with these formats listed here: <https://msdn.microsoft.com/en-us/library/windows/desktop/dd407173%28v=vs.85%29.aspx>, if the media format attempt to load is not listed, then you need to install the direct show filters that required to run such file formats. 
  For example, for mp4 files, these files usually are not supported in Windows. Thus, you'll need to install media codec pack in order ASM able to run this format.
- When using VLC media player, In order to use VLC media player, VLC version 3 or later must be installed in you pc (older versions won't work), 
  32 bit specifically (64 bit version won't work).

  ASM is programmed to locate VLC in the default installation folder of VLC.

  VLC player path in 64 bit system should be:
  C:\Program Files (x86)\VideoLAN\VLC

  VLC player path in 32 bit system should be:
  C:\Program Files\VideoLAN\VLC
  
  Also, VLC media player with ASM may play not smoothly. This is normal, since ASM uses milliseconds while VLC uses frames for playback. 
  This won't effect accuracy (in adding subtitles, editing ...etc) though.
- Generating wave files from media files is powered by ffmpeg. See "ffmpeg readme.txt" and "Copyright Notices - Credits.txt" files for more details.
- When generating WaveForm for a file, a new file under extension .wfpd will be added next to the media file, with the same name. 
- If the media file is edited (edited audio and/or video) please delete .wfpd file (if presented) to regenerate the wave form data again.
- WFPD data files are part of AHD Subtitles Maker, licensed under the same license (see COPYING above)
- If a black screen appears and hang when attempting to generate a waveform, please enter 'y' then press Enter.
- If the program fails to start, try to reset the settings, run "reset settings.bat" file that should be located in the program folder. If that doesn't work, go to this folder:
  C:\Users\<username>\AppData\Local\AHD\
  while <username> is the computer user name. Delete all folders there then try again.
- Please make sure the program is not blocked via Firewall and allowed to access the internet so that some program features function correctly.
- If lag, performance decrease, ... or something like that occur in your machine when using ASM, increasing the "player timer" in the settings may help improving performance with ASM.



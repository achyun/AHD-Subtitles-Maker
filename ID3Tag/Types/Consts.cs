/* This file is part of AHD ID3 Tag Editor (AITE)
 * A program that edit and create ID3 Tag.
 *
 * Copyright © Alaa Ibrahim Hadid 2012 - 2021
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace AHD.ID3.Types
{
    /// <summary>
    /// A class contains constant items appears in ID3v2 frames.
    /// </summary>
    public class ID3FrameConsts
    {
        #region languages
        static readonly string[] languages = new string[]
                {
"Abkhazian [abk]",
"Achinese [ace]",
"Acoli [ach]",
"Adangme [ada]",
"Adygei [ady]",
"Afar [aar]",
"Afrihili [afh]",
"Afrikaans [afr]",
"Afro-Asiatic (Other) [afa]",
"Ainu [ain]",
"Akan [aka]",
"Akkadian [akk]",
"Albanian [alb]",
"Aleut [ale]",
"Algonquian languages [alg]",
"Altaic (Other) [tut]",
"Amharic [amh]",
"Angika [anp]",
"Apache languages [apa]",
"Arabic [ara]",
"Aragonese [arg]",
"Aramaic [arc]",
"Arapaho [arp]",
"Arawak [arw]",
"Armenian [arm]",
"Artificial (Other) [art]",
"Assamese [asm]",
"Asturian; Bable [ast]",
"Athapascan languages [ath]",
"Australian languages [aus]",
"Austronesian (Other) [map]",
"Avaric [ava]",
"Avestan [ave]",
"Awadhi [awa]",
"Aymara [aym]",
"Azerbaijani [aze]",
"Balinese [ban]",
"Baltic (Other) [bat]",
"Baluchi [bal]",
"Bambara [bam]",
"Bamileke languages [bai]",
"Banda languages [bad]",
"Bantu (Other) [bnt]",
"Basa [bas]",
"Bashkir [bak]",
"Basque [baq]",
"Batak languages [btk]",
"Beja [bej]",
"Belarusian [bel]",
"Bemba [bem]",
"Bengali [ben]",
"Berber (Other) [ber]",
"Bhojpuri [bho]",
"Bihari [bih]",
"Bikol [bik]",
"Bini; Edo [bin]",
"Bislama [bis]",
"Blin; Bilin [byn]",
"Bosnian [bos]",
"Braj [bra]",
"Breton [bre]",
"Buginese [bug]",
"Bulgarian [bul]",
"Buriat [bua]",
"Burmese [bur]",
"Caddo [cad]",
"Caucasian (Other) [cau]",
"Cebuano [ceb]",
"Celtic (Other) [cel]",
"Central American Indian (Other) [cai]",
"Central Khmer [khm]",
"Chagatai [chg]",
"Chamic languages [cmc]",
"Chamorro [cha]",
"Chechen [che]",
"Cherokee [chr]",
"Cheyenne [chy]",
"Chibcha [chb]",
"Chinese [chi]",
"Chinook jargon [chn]",
"Chipewyan [chp]",
"Choctaw [cho]",
"Chuukese [chk]",
"Chuvash [chv]",
"Classical Nepal Bhasa [nwc]",
"Cook Islands Maori [rar]",
"Coptic [cop]",
"Cornish [cor]",
"Corsican [cos]",
"Cree [cre]",
"Creek [mus]",
"Creoles and pidgins (Other) [crp]",
"Crimean Turkish [crh]",
"Croatian [scr]",
"Cushitic (Other) [cus]",
"Czech [cze]",
"Dakota [dak]",
"Danish [dan]",
"Dargwa [dar]",
"Delaware [del]",
"Dinka [din]",
"Dogri [doi]",
"Dogrib [dgr]",
"Dravidian (Other) [dra]",
"Duala [dua]",
"Dutch, Middle (ca.1050-1350) [dum]",
"Dyula [dyu]",
"Dzongkha [dzo]",
"Eastern Frisian [frs]",
"Efik [efi]",
"Egyptian (Ancient) [egy]",
"Ekajuk [eka]",
"Elamite [elx]",
"English [eng]",
"English based (Other) [cpe]",
"English, Middle (1100-1500) [enm]",
"English, Old (ca.450-1100) [ang]",
"Erzya [myv]",
"Esperanto [epo]",
"Estonian [est]",
"Ewe [ewe]",
"Ewondo [ewo]",
"Fang [fan]",
"Fanti [fat]",
"Faroese [fao]",
"Fijian [fij]",
"Finnish [fin]",
"Finno-Ugrian (Other) [fiu]",
"Flemish [dut]",
"Fon [fon]",
"French-based (Other) [cpf]",
"French [fre]",
"French, Middle (ca.1400-1600) [frm]",
"French, Old (842-ca.1400) [fro]",
"Friulian [fur]",
"Fulah [ful]",
"Ga [gaa]",
"Galibi Carib [car]",
"Galician [glg]",
"Ganda [lug]",
"Gayo [gay]",
"Gbaya [gba]",
"Geez [gez]",
"Georgian [geo]",
"German [ger]",
"German, Middle High (ca.1050-1500) [gmh]",
"German, Old High (ca.750-1050) [goh]",
"Germanic (Other) [gem]",
"Gilbertese [gil]",
"Gondi [gon]",
"Gorontalo [gor]",
"Gothic [got]",
"Grebo [grb]",
"Greek, Ancient (to 1453) [grc]",
"Greek, Modern (1453-) [gre]",
"Greenlandic [kal]",
"Guarani [grn]",
"Gujarati [guj]",
"Gwich´in [gwi]",
"Haida [hai]",
"Haitian Creole [hat]",
"Hausa [hau]",
"Hawaiian [haw]",
"Hebrew [heb]",
"Herero [her]",
"Hiligaynon [hil]",
"Himachali [him]",
"Hindi [hin]",
"Hiri Motu [hmo]",
"Hittite [hit]",
"Hmong [hmn]",
"Hungarian [hun]",
"Hupa [hup]",
"Iban [iba]",
"Icelandic [ice]",
"Ido [ido]",
"Igbo [ibo]",
"Ijo languages [ijo]",
"Iloko [ilo]",
"Inari Sami [smn]",
"Indic (Other) [inc]",
"Indo-European (Other) [ine]",
"Indonesian [ind]",
"Ingush [inh]",
"Interlingua (International Auxiliary Language Association) [ina]",
"Interlingue [ile]",
"Inuktitut [iku]",
"Inupiaq [ipk]",
"Iranian (Other) [ira]",
"Irish [gle]",
"Irish, Middle (900-1200) [mga]",
"Irish, Old (to 900) [sga]",
"Iroquoian languages [iro]",
"Italian [ita]",
"Japanese [jpn]",
"Javanese [jav]",
"Jingpho [kac]",
"Judeo-Arabic [jrb]",
"Judeo-Persian [jpr]",
"Kabardian [kbd]",
"Kabyle [kab]",
"Kamba [kam]",
"Kannada [kan]",
"Kanuri [kau]",
"Kara-Kalpak [kaa]",
"Karachay-Balkar [krc]",
"Karelian [krl]",
"Karen languages [kar]",
"Kashmiri [kas]",
"Kashubian [csb]",
"Kawi [kaw]",
"Kazakh [kaz]",
"Khasi [kha]",
"Khoisan (Other) [khi]",
"Khotanese [kho]",
"Kikuyu; Gikuyu [kik]",
"Kimbundu [kmb]",
"Kinyarwanda [kin]",
"Klingon [tlh]",
"Komi [kom]",
"Kongo [kon]",
"Konkani [kok]",
"Korean [kor]",
"Kosraean [kos]",
"Kpelle [kpe]",
"Kru languages [kro]",
"Kumyk [kum]",
"Kurdish [kur]",
"Kurukh [kru]",
"Kutenai [kut]",
"Kwanyama [kua]",
"Kyrgyz [kir]",
"Ladino [lad]",
"Lahnda [lah]",
"Lamba [lam]",
"Land Dayak languages [day]",
"Lao [lao]",
"Latin [lat]",
"Latvian [lav]",
"Letzeburgesch [ltz]",
"Lezghian [lez]",
"Limburgish [lim]",
"Lingala [lin]",
"Lithuanian [lit]",
"Lojban [jbo]",
"Low German [nds]",
"Lower Sorbian [dsb]",
"Lozi [loz]",
"Luba-Katanga [lub]",
"Luba-Lulua [lua]",
"Luiseno [lui]",
"Lule Sami [smj]",
"Lunda [lun]",
"Luo (Kenya and Tanzania) [luo]",
"Lushai [lus]",
"Macedo-Romanian [rup]",
"Macedonian [mac]",
"Madurese [mad]",
"Magahi [mag]",
"Maithili [mai]",
"Makasar [mak]",
"Malagasy [mlg]",
"Malay [may]",
"Malayalam [mal]",
"Maldivian [div]",
"Maltese [mlt]",
"Manchu [mnc]",
"Mandar [mdr]",
"Mandingo [man]",
"Manipuri [mni]",
"Manobo languages [mno]",
"Manx [glv]",
"Maori [mao]",
"Mapuche [arn]",
"Marathi [mar]",
"Mari [chm]",
"Marshallese [mah]",
"Marwari [mwr]",
"Masai [mas]",
"Mayan languages [myn]",
"Mende [men]",
"Mi'kmaq; Micmac [mic]",
"Minangkabau [min]",
"Mirandese [mwl]",
"Miscellaneous languages [mis]",
"Mohawk [moh]",
"Moksha [mdf]",
"Moldavian [mol]",
"Mon-Khmer (Other) [mkh]",
"Mongo [lol]",
"Mongolian [mon]",
"Mossi [mos]",
"Multiple languages [mul]",
"Munda languages [mun]",
"N'Ko [nqo]",
"Nahuatl languages [nah]",
"Nauru [nau]",
"Navajo; Navaho [nav]",
"Ndonga [ndo]",
"Neapolitan [nap]",
"Nepal Bhasa [new]",
"Nepali [nep]",
"Nias [nia]",
"Niger-Kordofanian (Other) [nic]",
"Nilo-Saharan (Other) [ssa]",
"Niuean [niu]",
"Nogai [nog]",
"Norse, Old [non]",
"North American Indian [nai]",
"North Ndebele [nde]",
"Northern Frisian [frr]",
"Northern Sami [sme]",
"Northern Sotho [nso]",
"Norwegian [nor]",
"Norwegian Bokmal [nob]",
"Nubian languages [nub]",
"Nyamwezi [nym]",
"Nyanja [nya]",
"Nyankole [nyn]",
"Nynorsk Norwegian [nno]",
"Nyoro [nyo]",
"Nzima [nzi]",
"Occitan (post 1500) [oci]",
"Oirat [xal]",
"Ojibwa [oji]",
"Old Church Slavonic [chu]",
"Oriya [ori]",
"Oromo [orm]",
"Osage [osa]",
"Ossetic [oss]",
"Otomian languages [oto]",
"Pahlavi [pal]",
"Palauan [pau]",
"Pali [pli]",
"Pampanga [pam]",
"Pangasinan [pag]",
"Papiamento [pap]",
"Papuan (Other) [paa]",
"Persian [per]",
"Persian, Old (ca.600-400 B.C.) [peo]",
"Philippine (Other) [phi]",
"Phoenician [phn]",
"Pilipino [fil]",
"Pohnpeian [pon]",
"Polish [pol]",
"Portuguese-based (Other) [cpp]",
"Portuguese [por]",
"Prakrit languages [pra]",
"Provencal, Old (to 1500) [pro]",
"Punjabi [pan]",
"Pushto [pus]",
"Quechua [que]",
"Rajasthani [raj]",
"Rapanui [rap]",
"Romance (Other) [roa]",
"Romanian [rum]",
"Romansh [roh]",
"Romany [rom]",
"Rundi [run]",
"Russian [rus]",
"Salishan languages [sal]",
"Samaritan Aramaic [sam]",
"Sami languages (Other) [smi]",
"Samoan [smo]",
"Sandawe [sad]",
"Sango [sag]",
"Sanskrit [san]",
"Santali [sat]",
"Sardinian [srd]",
"Sasak [sas]",
"Scots [sco]",
"Scottish Gaelic [gla]",
"Selkup [sel]",
"Semitic (Other) [sem]",
"Serbian [scc]",
"Serer [srr]",
"Shan [shn]",
"Shona [sna]",
"Sichuan Yi [iii]",
"Sicilian [scn]",
"Sidamo [sid]",
"Sign Languages [sgn]",
"Siksika [bla]",
"Sindhi [snd]",
"Sinhala; Sinhalese [sin]",
"Sino-Tibetan (Other) [sit]",
"Siouan languages [sio]",
"Skolt Sami [sms]",
"Slave (Athapascan) [den]",
"Slavic (Other) [sla]",
"Slovak [slo]",
"Slovenian [slv]",
"Sogdian [sog]",
"Somali [som]",
"Songhai languages [son]",
"Soninke [snk]",
"Sorbian languages [wen]",
"South American Indian (Other) [sai]",
"South Ndebele [nbl]",
"Southern [sot]",
"Southern Altai [alt]",
"Southern Sami [sma]",
"Spanish [spa]",
"Sranan Tongo [srn]",
"Sukuma [suk]",
"Sumerian [sux]",
"Sundanese [sun]",
"Susu [sus]",
"Swahili [swa]",
"Swati [ssw]",
"Swedish [swe]",
"Swiss German [gsw]",
"Syriac [syr]",
"Tagalog [tgl]",
"Tahitian [tah]",
"Tai (Other) [tai]",
"Tajik [tgk]",
"Tamashek [tmh]",
"Tamil [tam]",
"Tatar [tat]",
"Telugu [tel]",
"Tereno [ter]",
"Tetum [tet]",
"Thai [tha]",
"Tibetan [tib]",
"Tigre [tig]",
"Tigrinya [tir]",
"Timne [tem]",
"Tiv [tiv]",
"Tlingit [tli]",
"Tok Pisin [tpi]",
"Tokelau [tkl]",
"Tonga (Nyasa) [tog]",
"Tonga (Tonga Islands) [ton]",
"Tsimshian [tsi]",
"Tsonga [tso]",
"Tswana [tsn]",
"Tumbuka [tum]",
"Tupi languages [tup]",
"Turkish [tur]",
"Turkish, Ottoman (1500-1928) [ota]",
"Turkmen [tuk]",
"Tuvalu [tvl]",
"Tuvinian [tyv]",
"Twi [twi]",
"Udmurt [udm]",
"Ugaritic [uga]",
"Ukrainian [ukr]",
"Umbundu [umb]",
"Undetermined [und]",
"Upper Sorbian [hsb]",
"Urdu [urd]",
"Uyghur [uig]",
"Uzbek [uzb]",
"Vai [vai]",
"Valencian [cat]",
"Venda [ven]",
"Vietnamese [vie]",
"Volapuk [vol]",
"Votic [vot]",
"Wakashan languages [wak]",
"Walamo [wal]",
"Walloon [wln]",
"Waray [war]",
"Washo [was]",
"Welsh [wel]",
"Western Frisian [fry]",
"Wolof [wol]",
"Xhosa [xho]",
"Yakut [sah]",
"Yao [yao]",
"Yapese [yap]",
"Yiddish [yid]",
"Yoruba [yor]",
"Yupik languages [ypk]",
"Zande languages [znd]",
"Zapotec [zap]",
"Zazaki [zza]",
"Zenaga [zen]",
"Zhuang; Chuang [zha]",
"Zulu [zul]",
"Zuni [zun]"
                };
        #endregion
        #region TKEY
        static readonly string[] tkey = 
        {
        "A",
"A (minor)",
"A flat",
"A flat (minor)",
"A sharp",
"A sharp (minor)",
"B",
"B (minor)",
"B flat",
"B flat (minor)",
"B sharp",
"B sharp (minor)",
"C",
"C (minor)",
"C flat",
"C flat (minor)",
"C sharp",
"C sharp (minor)",
"D",
"D (minor)",
"D flat",
"D flat (minor)",
"D sharp",
"D sharp (minor)",
"E",
"E (minor)",
"E flat",
"E flat (minor)",
"E sharp",
"E sharp (minor)",
"F",
"F (minor)",
"F flat",
"F flat (minor)",
"F sharp",
"F sharp (minor)",
"G",
"G (minor)",
"G flat",
"G flat (minor)",
"G sharp",
"G sharp (minor)",
"off key",
        };
        #endregion
        #region FileTypes
        static readonly string[] fileTypecodes =
        {
"MPG",
"MPG/1",
"MPG/2",
"MPG/3",
"MPG/2.5",
"MPG/AAC",
"VQF",
"PCM",
        };
        static readonly string[] fileTypes = {   
"MPEG Audio",
"MPEG 1/2 layer I",
"MPEG 1/2 layer II",
"MPEG 1/2 layer III",
"MPEG 2.5",
"Advanced audio compression",
"Transform-domain Weighted Interleave Vector Quantization",
"Pulse Code Modulated audio",
                         };
        #endregion
        #region picture Types
        static readonly string[] pictureTypes = 
        {
        "Other",
        "32x32 pixels 'file icon' (PNG only)",
        "Other file icon",
        "Cover (front)",
        "Cover (back)",
        "Leaflet page",
        "Media (e.g. label side of CD)",
        "Lead artist/lead performer/soloist",
        "Artist/performer",
        "Conductor",
        "Band/Orchestra",
        "Composer",
        "Lyricist/text writer",
        "Recording Location",
        "During recording",
        "During performance",
        "Movie/video screen capture",
        "A bright coloured fish",
        "Illustration",
        "Band/artist logotype",
        "Publisher/Studio logotype",
        };
        #endregion
        static readonly string[] SynchronisedLyricscontentTypes = { "Other", "Lyrics", "Text transcription", "Movement/part name", "Events", "Chord", "Trivia/'pop up' information", };
        #region Content types (Genres)
        static readonly string[] genres =
        {
"Blues",
"Classic Rock",
"Country",
"Dance",
"Disco",
"Funk",
"Grunge",
"Hip-Hop",
"Jazz",
"Metal",
"New Age",
"Oldies",
"Other",
"Pop",
"R&B",
"Rap",
"Reggae",
"Rock",
"Techno",
"Industrial",
"Alternative",
"Ska",
"Death Metal",
"Pranks",
"Soundtrack",
"Euro-Techno",
"Ambient",
"Trip-Hop",
"Vocal",
"Jazz+Funk",
"Fusion",
"Trance",
"Classical",
"Instrumental",
"Acid",
"House",
"Game",
"Sound Clip",
"Gospel",
"Noise",
"AlternRock",
"Bass",
"Soul",
"Punk",
"Space",
"Meditative",
"Instrumental Pop",
"Instrumental Rock",
"Ethnic",
"Gothic",
"Darkwave",
"Techno-Industrial",
"Electronic",
"Pop-Folk",
"Eurodance",
"Dream",
"Southern Rock",
"Comedy",
"Cult",
"Gangsta",
"Top 40",
"Christian Rap",
"Pop/Funk",
"Jungle",
"Native American",
"Cabaret",
"New Wave",
"Psychadelic",
"Rave",
"Showtunes",
"Trailer",
"Lo-Fi",
"Tribal",
"Acid Punk",
"Acid Jazz",
"Polka",
"Retro",
"Musical",
"Rock & Roll",
"Hard Rock",
        };
        #endregion
        #region Media type
        static readonly string[] MediaTypeCodes = 
        {
        "DIG",
    "DIG/A",

"ANA",
   "ANA/WAC",
   "ANA/8CA",

"CD",
     "CD/A",
    "CD/DD",
    "CD/AD",
    "CD/AA",

"LD",
     "LD/A",

"TT",
    "TT/33",
    "TT/45",
    "TT/71",
    "TT/76",
    "TT/78",
    "TT/80",

"MD",
     "MD/A",

"DAT",
     "DAT/A",
     "DAT/1",
     "DAT/2",
     "DAT/3",
     "DAT/4",
    "DAT/5",
     "DAT/6",

"DCC",
     "DCC/A",

"DVD",
     "DVD/A",

"TV",
   "TV/PAL",
  "TV/NTSC",
 "TV/SECAM",

"VID",
   "VID/PAL",
  "VID/NTSC",
 "VID/SECAM",
   "VID/VHS",
  "VID/SVHS",
  "VID/BETA",

"RAD",
    "RAD/FM",
    "RAD/AM",
    "RAD/LW",
    "RAD/MW",

"TEL",
     "TEL/I",

"MC",
     "MC/4",
     "MC/9",
     "MC/I",
    "MC/II",
   "MC/III",
    "MC/IV",

"REE",
     "REE/9",
    "REE/19",
    "REE/38",
   "REE/76",
     "REE/I",
    "REE/II",
   "REE/III",
    "REE/IV",
        };
        static readonly string[] mediaType = 
        {
"Other digital media", 
"Analog transfer from media",

"Other analog media",
"Wax cylinder",
"8-track tape cassette",

"CD",
"Analog transfer from media",
"DDD",
"ADD",
"AAD",

"Laserdisc",
"Analog transfer from media",

"Turntable records",
"33.33 rpm",
"45 rpm",
"71.29 rpm",
"76.59 rpm",
"78.26 rpm",
"80 rpm",

"MiniDisc",
"Analog transfer from media",

"DAT",
"Analog transfer from media",
"standard, 48 kHz/16 bits, linear",
"mode 2, 32 kHz/16 bits, linear",
"mode 3, 32 kHz/12 bits, nonlinear, low speed",
"mode 4, 32 kHz/12 bits, 4 channels",
"mode 5, 44.1 kHz/16 bits, linear",
"mode 6, 44.1 kHz/16 bits, 'wide track' play",

"DCC",
"Analog transfer from media",

"DVD",
"Analog transfer from media",

"Television",
"PAL",
"NTSC",
"SECAM",

"Video",
"PAL",
"NTSC",
"SECAM",
"VHS",
"S-VHS",
"BETAMAX",

"Radio",
"FM",
"AM",
"LW",
"MW",

"Telephone",
"ISDN",

"MC (normal cassette)",
"4.75 cm/s (normal speed for a two sided cassette)",
"9.5 cm/s",
"Type I cassette (ferric/normal)",
"Type II cassette (chrome)",
"Type III cassette (ferric chrome)",
"Type IV cassette (metal)",

"Reel",
"9.5 cm/s",
"19 cm/s",
"38 cm/s",
"76 cm/s",
"Type I cassette (ferric/normal)",
"Type II cassette (chrome)",
"Type III cassette (ferric chrome)",
"Type IV cassette (metal)",
        };
        #endregion
        #region EventTimingCodesEvents
        static readonly string[] eventTimingCodesEvents =
        { 
"padding (has no meaning)",
"end of initial silence",
"intro start",
"mainpart start",
"outro start",
"outro end",
"verse start",
"refrain start",
"interlude start",
"theme start",
"variation start",
"key change",
"time change",
"momentary unwanted noise (Snap, Crackle & Pop)",
"sustained noise",
"sustained noise end",
"intro end",
"mainpart end",
"verse end",
"refrain end",
"theme end",
"audio end (start of silence)",
"audio file ends",
        };
        #endregion
        #region commercial Received As
        static readonly string[] commercialReceivedAs = 
            { 
"Other",
"Standard CD album with other songs",
"Compressed audio on CD",
"File over the Internet",
"Stream over the Internet",
"As note sheets",
"As note sheets in a book with other sheets",
"Music on other media",
"Non-musical merchandise",
            };
        #endregion
        #region Currency
        static readonly string[] currency = 
        { 
            "Afghani [ AFN ]",
"Algerian Dinar [ DZD ]",
"Argentine Peso [ ARS ]",
"Armenian Dram [ AMD ]",
"Aruban Guilder [ AWG ]",
"Australian Dollar [ AUD ]",
"Azerbaijanian Manat [ AZN ]",
"Bahamian Dollar [ BSD ]",
"Bahraini Dinar [ BHD ]",
"Baht [ THB ]",
"Balboa [ PAB ]",
"Bangladeshi Taka [ BDT ]",
"Barbados Dollar [ BBD ]",
"Belarussian Ruble [ BYR ]",
"Belize Dollar [ BZD ]",
"Bermudian Dollar [ BMD ]",
"Bolivian Mvdol [ BOV ]",
"Boliviano [ BOB ]",
"Botswana Pula [ BWP ]",
"Brazilian Real [ BRL ]",
"Brunei Dollar [ BND ]",
"Bulgarian Lev [ BGN ]",
"Burundian Franc [ BIF ]",
"Canadian Dollar [ CAD ]",
"Cape Verde Escudo [ CVE ]",
"Cayman Islands Dollar [ KYD ]",
"Cedi [ GHC ]",
"Chilean Peso [ CLP ]",
"Colombian Peso [ COP ]",
"Comoro Franc [ KMF ]",
"Convertible Marks [ BAM ]",
"Cordoba Oro [ NIO ]",
"Costa Rican Colon [ CRC ]",
"Croatian Kuna [ HRK ]",
"Cuban Peso [ CUP ]",
"Cyprus Pound [ CYP ]",
"Czech Koruna [ CZK ]",
"Dalasi [ GMD ]",
"Danish Krone [ DKK ]",
"Denar [ MKD ]",
"Djibouti Franc [ DJF ]",
"Dobra [ STD ]",
"Dominican Peso [ DOP ]",
"Dong [ VND ]",
"East Caribbean Dollar [ XCD ]",
"Egyptian Pound [ EGP ]",
"Ethiopian Birr [ ETB ]",
"Euro [ EUR ]",
"Falkland Islands Pound [ FKP ]",
"Fiji Dollar [ FJD ]",
"Forint [ HUF ]",
"Franc Congolais [ CDF ]",
"Gibraltar pound [ GIP ]",
"Guarani [ PYG ]",
"Guinea Franc [ GNF ]",
"Guyana Dollar [ GYD ]",
"Haiti Gourde [ HTG ]",
"Hong Kong Dollar [ HKD ]",
"Hryvnia [ UAH ]",
"Iceland Krona [ ISK ]",
"Indian Rupee [ INR ]",
"Iranian Rial [ IRR ]",
"Iraqi Dinar [ IQD ]",
"Jamaican Dollar [ JMD ]",
"Japanese yen [ JPY ]",
"Jordanian Dinar [ JOD ]",
"Kenyan Shilling [ KES ]",
"Kina [ PGK ]",
"Kip [ LAK ]",
"Kroon [ EEK ]",
"Kuwaiti Dinar [ KWD ]",
"Kwacha [ MWK ]",
"Kwacha [ ZMK ]",
"Kwanza [ AOA ]",
"Kyat [ MMK ]",
"Lari [ GEL ]",
"Latvian Lats [ LVL ]",
"Lebanese Pound [ LBP ]",
"Lek [ ALL ]",
"Lempira [ HNL ]",
"Leone [ SLL ]",
"Liberian Dollar [ LRD ]",
"Libyan Dinar [ LYD ]",
"Lilangeni [ SZL ]",
"Lithuanian Litas [ LTL ]",
"Loti [ LSL ]",
"Malagasy Ariary [ MGA ]",
"Malaysian Ringgit [ MYR ]",
"Maltese Lira [ MTL ]",
"Manat [ TMM ]",
"Mauritius Rupee [ MUR ]",
"Metical [ MZN ]",
"Mexican Peso [ MXN ]",
"Mexican Unidad [ MXV ]",
"Moldovan Leu [ MDL ]",
"Moroccan Dirham [ MAD ]",
"Naira [ NGN ]",
"Nakfa [ ERN ]",
"Namibian Dollar [ NAD ]",
"Nepalese Rupee [ NPR ]",
"Netherlands Antillian Guilder [ ANG ]",
"New Israeli Shekel [ ILS ]",
"New Taiwan Dollar [ TWD ]",
"New Turkish Lira [ TRY ]",
"New Zealand Dollar [ NZD ]",
"Ngultrum [ BTN ]",
"North Korean Won [ KPW ]",
"Norwegian Krone [ NOK ]",
"Nuevo Sol [ PEN ]",
"Ouguiya [ MRO ]",
"Pa'anga [ TOP ]",
"Pakistan Rupee [ PKR ]",
"Pataca [ MOP ]",
"Peso Uruguayo [ UYU ]",
"Philippine Peso [ PHP ]",
"Pound Sterling [ GBP ]",
"Qatari Rial [ QAR ]",
"Quetzal [ GTQ ]",
"Rial Omani [ OMR ]",
"Riel [ KHR ]",
"Romanian Leu [ ROL ]",
"Romanian New Leu [ RON ]",
"Rufiyaa [ MVR ]",
"Rupiah [ IDR ]",
"Russian Ruble [ RUB ]",
"Rwanda Franc [ RWF ]",
"Saint Helena Pound [ SHP ]",
"Samoan Tala [ WST ]",
"Saudi Riyal [ SAR ]",
"Serbian Dinar [ RSD ]",
"Seychelles Rupee [ SCR ]",
"Singapore Dollar [ SGD ]",
"Slovak Koruna [ SKK ]",
"Solomon Islands Dollar [ SBD ]",
"Som [ KGS ]",
"Somali Shilling [ SOS ]",
"Somoni [ TJS ]",
"South African Rand [ ZAR ]",
"South Korean Won [ KRW ]",
"Sri Lanka Rupee [ LKR ]",
"Sudanese Dinar [ SDD ]",
"Surinam Dollar [ SRD ]",
"Swedish Krona [ SEK ]",
"Swiss Franc [ CHF ]",
"Syrian Pound [ SYP ]",
"Tanzanian Shilling [ TZS ]",
"Tenge [ KZT ]",
"Trinidad and Tobago Dollar [ TTD ]",
"Tugrik [ MNT ]",
"Tunisian Dinar [ TND ]",
"Uganda Shilling [ UGX ]",
"Unidad de Valor Real [ COU ]",
"Unidades de formento [ CLF ]",
"United Arab Emirates dirham [ AED ]",
"US Dollar [ USD ]",
"Uzbekistan Som [ UZS ]",
"Vatu [ VUV ]",
"Venezuelan bolívar [ VEB ]",
"Yemeni Rial [ YER ]",
"Yuan Renminbi [ CNY ]",
"Zimbabwe Dollar [ ZWD ]",
"Zloty [ PLN ]",
        };
        #endregion

        /// <summary>
        /// Get the languages list. The format is 'languageName [3 chars language id]'
        /// </summary>
        public static string[] Languages
        { get { return languages; } }
        /// <summary>
        /// Get the T Key for the key text frame
        /// </summary>
        public static string[] Tkey
        { get { return tkey; } }
        /// <summary>
        /// Get the file types for file type text frame
        /// </summary>
        public static string[] FileTypes
        { get { return fileTypes; } }
        /// <summary>
        /// Get the picture types for attached pictures frame
        /// </summary>
        public static string[] PictureTypes
        { get { return pictureTypes; } }
        /// <summary>
        /// Get the content types for Synchronised Lyrics frame
        /// </summary>
        public static string[] SynchronisedLyricsContentTypes
        { get { return SynchronisedLyricscontentTypes; } }
        /// <summary>
        /// Get the genres used in both ID3v1 and ID3v2
        /// </summary>
        public static string[] Genres
        { get { return genres; } }
        /// <summary>
        /// Get the media types
        /// </summary>
        public static string[] MediaTypes
        { get { return mediaType; } }
        /// <summary>
        /// Get the Event Timing Codes Events
        /// </summary>
        public static string[] EventTimingCodesEvents
        { get { return eventTimingCodesEvents; } }
        /// <summary>
        /// Get the Commercial Received As list
        /// </summary>
        public static string[] CommercialReceivedAs
        { get { return commercialReceivedAs; } }
        /// <summary>
        /// Get the Currency list
        /// </summary>
        public static string[] Currency
        { get { return currency; } }

        /// <summary>
        /// Get an event timing event using the index value of the frame data
        /// </summary>
        /// <param name="index">The index in the frame data</param>
        /// <returns>The event as string</returns>
        public static string GetEventTimingEvent(int index)
        {
            switch (index)
            {
                case 0xFD: return "audio end (start of silence)";
                case 0xFE: return "audio file ends";
            }
            for (int i = 0; i < eventTimingCodesEvents.Length; i++)
            {
                if (i == index)
                    return eventTimingCodesEvents[i];
            }
            //not found ?
            return "";
        }
        /// <summary>
        /// Get an event index to use in data saving
        /// </summary>
        /// <param name="eventType">The evemt type as string. This must be exist in the list of "EventTimingCodesEvents" property in this class.</param>
        /// <returns>The index to use in data saving as byte</returns>
        public static byte GetEventTimingEventIndex(string eventType)
        {
            //special indexed
            switch (eventType)
            {
                case "audio end (start of silence)": return 0xFD;
                case "audio file ends": return 0xFE;
            }
            //normal indexed
            for (byte i = 0; i < eventTimingCodesEvents.Length; i++)
            {
                if (eventTimingCodesEvents[i] == eventType)
                    return i;
            }
            //nothing found, unknown ? return "reserved for future use" index
            return 0x15;
        }
        /// <summary>
        /// Get a language using the language id.
        /// </summary>
        /// <param name="langID">The language id. 3 chars not case sensitive. The id must be exist in the "Language" property.</param>
        /// <returns>The language name.</returns>
        public static string GetLanguage(string langID)
        {
            foreach (string lan in languages)
            {
                if (lan.Substring(lan.Length - 4, 3).ToUpper() == langID.ToUpper())
                    return lan;
            }
            return "";
        }
        /// <summary>
        /// Get a language id using the name of the language.
        /// </summary>
        /// <param name="language">The language name. must be exist in the "Language" property.</param>
        /// <returns>The language id, no case sensitive.</returns>
        public static string GetLanguageID(string language)
        {
            foreach (string lan in languages)
            {
                if (lan.ToLower() == language.ToLower())
                    return lan.Substring(lan.Length - 4, 3).ToUpper();
            }
            return "";
        }
        /// <summary>
        /// Get media type item using id
        /// </summary>
        /// <param name="id">The media type id (e.g. DIG, ANA...etc)</param>
        /// <returns>The media type item</returns>
        public static string GetMediaType(string id)
        {
            for (int i = 0; i < MediaTypeCodes.Length; i++)
            {
                if (MediaTypeCodes[i].ToLower() == id.ToLower())
                    return mediaType[i];
            }
            return "";
        }
        /// <summary>
        /// Get media type id using media type
        /// </summary>
        /// <param name="type">The media type</param>
        /// <returns>The media type item</returns>
        public static string GetMediaTypeID(string type)
        {
            for (int i = 0; i < mediaType.Length; i++)
            {
                if (mediaType[i].ToLower() == type.ToLower())
                    return MediaTypeCodes[i];
            }
            return "";
        }

        /// <summary>
        /// Get file type item using id
        /// </summary>
        /// <param name="id">The file type id</param>
        /// <returns>The file type item</returns>
        public static string GetFileType(string id)
        {
            for (int i = 0; i < fileTypecodes.Length; i++)
            {
                if (fileTypecodes[i].ToLower() == id.ToLower())
                    return fileTypes[i];
            }
            return "";
        }
        /// <summary>
        /// Get file type id using file type
        /// </summary>
        /// <param name="type">The file type</param>
        /// <returns>The file type item</returns>
        public static string GetFileTypeID(string type)
        {
            for (int i = 0; i < fileTypes.Length; i++)
            {
                if (fileTypes[i].ToLower() == type.ToLower())
                    return fileTypecodes[i];
            }
            return "";
        }
        /// <summary>
        /// Get Tkey
        /// </summary>
        /// <param name="value">The value as presented in the tag</param>
        /// <returns>the key</returns>
        public static string GetTKey(string value)
        {
            if (value == "o")
            {
                value = value.Replace("o", "off key");
            }
            else
            {
                value = value.Replace("#", " sharp");
                value = value.Replace("b", " flat");
                value = value.Replace("m", " (minor)");
            }
            return value;
        }
        /// <summary>
        /// Get the TKey value using TKey
        /// </summary>
        /// <param name="text">The TKey item as presented in TKey property</param>
        /// <returns>The value to save in Tag</returns>
        public static string GetTKeyValue(string text)
        {
            text = text.Replace(" ", "");
            text = text.Replace("sharp", "#");
            text = text.Replace("flat", "b");
            text = text.Replace("(minor)", "m");
            text = text.Replace("offkey", "o");
            return text;
        }
    }
}

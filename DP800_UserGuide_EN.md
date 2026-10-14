#### **RIGOL User’s Guide** 

**DP800 Series Programmable Linear DC Power Supply** 

**Jun. 2016 RIGOL TECHNOLOGIES, INC.** 

**RIGOL** 

##### **Guaranty and Declaration** 

###### **Copyright** 

© 2012 **RIGOL** TECHNOLOGIES, INC. All Rights Reserved. 

###### **Trademark Information** 

**RIGOL** is a registered trademark of **RIGOL** TECHNOLOGIES, INC. 

###### **Publication Number** 

UGH04109-1110 

###### **Software Version** 

00.01.14 

Software upgrade might change or add product features. Please acquire the latest version of the manual from **RIGOL** website or contact **RIGOL** to upgrade the software. 

###### **Notices** 

- **RIGOL** products are covered by P.R.C. and foreign patents, issued and pending. 

- **RIGOL** reserves the right to modify or change parts of or all the specifications and pricing policies at the company’s sole decision. 

- Information in this publication replaces all previously released materials. 

- Information in this publication is subject to change without notice. 

- **RIGOL** shall not be liable for either incidental or consequential losses in connection with the furnishing, use, or performance of this manual, as well as any information contained. 

- Any part of this document is forbidden to be copied, photocopied, or rearranged without prior written approval of **RIGOL** . 

###### **Product Certification** 

**RIGOL** guarantees that this product conforms to the national and industrial standards in China as well as the ISO9001:2008 standard and the ISO14001:2004 standard. Other international standard conformance certifications are in progress. 

###### **Contact Us** 

If you have any problem or requirement when using our products or this manual, please contact **RIGOL** . 

E-mail: service@rigol.com Website: www.rigol.com 

DP800 User’s Guide 

I 

**RIGOL** 

##### **Safety Requirement** 

###### **General Safety Summary** 

Please review the following safety precautions carefully before putting the instrument into operation so as to avoid any personal injury or damage to the instrument and any product connected to it. To prevent potential hazards, please follow the instructions specified in this manual to use the instrument properly. 

###### **Use Proper Power Cord.** 

Only the exclusive power cord designed for the instrument and authorized for use within the local country could be used. 

###### **Ground the Instrument.** 

The instrument is grounded through the Protective Earth lead of the power cord. To avoid electric shock, connect the earth terminal of the power cord to the Protective Earth terminal before connecting any input or output terminals. 

###### **Connect the Probe Correctly.** 

If a probe is used, do not connect the ground lead to high voltage since it has isobaric electric potential as the ground. 

###### **Observe All Terminal Ratings.** 

To avoid fire or shock hazard, observe all ratings and markers on the instrument and check your manual for more information about ratings before connecting the instrument. 

###### **Use Proper Overvoltage Protection.** 

Ensure that no overvoltage (such as that caused by a bolt of lightning) can reach the product. Otherwise, the operator might be exposed to the danger of an electric shock. 

###### **Do Not Operate Without Covers.** 

Do not operate the instrument with covers or panels removed. 

###### **Do Not Insert Anything Into the Air Outlet.** 

Do not insert anything into the air outlet to avoid damage to the instrument. 

###### **Use Proper Fuse.** 

Please use the specified fuses. 

###### **Avoid Circuit or Wire Exposure.** 

Do not touch exposed junctions and components when the unit is powered on. 

DP800 User’s Guide 

II 

**RIGOL** 

###### **Do Not Operate With Suspected Failures.** 

If you suspect that any damage may occur to the instrument, have it inspected by **RIGOL** authorized personnel before further operations. Any maintenance, adjustment or replacement especially to circuits or accessories must be performed by **RIGOL** authorized personnel. 

###### **Provide Adequate Ventilation.** 

Inadequate ventilation may cause an increase of temperature in the instrument, which would cause damage to the instrument. So please keep the instrument well ventilated and inspect the air outlet and the fan regularly. 

###### **Do Not Operate in Wet Conditions.** 

To avoid short circuit inside the instrument or electric shock, never operate the instrument in a humid environment. 

###### **Do Not Operate in an Explosive Atmosphere.** 

To avoid personal injuries or damage to the instrument, never operate the instrument in an explosive atmosphere. 

###### **Keep Instrument Surfaces Clean and Dry.** 

To avoid dust or moisture from affecting the performance of the instrument, keep the surfaces of the instrument clean and dry. 

###### **Prevent Electrostatic Impact.** 

Operate the instrument in an electrostatic discharge protective environment to avoid damage induced by static discharges. Always ground both the internal and external conductors of cables to release static before making connections. 

###### **Use the Battery Properly.** 

Do not expose the battery (if available) to high temperature or fire. Keep it out of the reach of children. Improper change of a battery (lithium battery) may cause an explosion. Use the **RIGOL** specified battery only. 

###### **Handle with Caution.** 

Please handle with care during transportation to avoid damage to keys, knobs, interfaces, and other parts on the panels. 

###### **Do Not Provide Power for the Active Load.** 

In order to avoid the anti-irrigation current which leads to the power control loop out of control and damages the powered device, this power supply can only provide power for the pure load without the current output function. 

DP800 User’s Guide 

III 

**RIGOL** 

###### **Safety Notices and Symbols** 

###### **Safety Notices in this Manual:** 

###### **WARNING** 

Indicates a potentially hazardous situation or practice which, if not <u>avoided, will result in serious injury or death.</u> 

###### **CAUTION** 

Indicates a potentially hazardous situation or practice which, if not <u>avoided, could result in damage to the product or loss of important data.</u> 

###### **Safety Terms on the Product:** 

**DANGER** 

**WARNING** 

It calls attention to an operation, if not correctly performed, could result in injury or hazard immediately. 

It calls attention to an operation, if not correctly performed, could result in potential injury or hazard. 

**CAUTION** It calls attention to an operation, if not correctly performed, could result in damage to the product or other devices connected to the product. 

###### **Safety Symbols on the Product:** 

**Hazardous Voltage** 

**Safety Warning** 

**Protective Earth Terminal** 

**Chassis Test Ground Ground** 

DP800 User’s Guide 

IV 

**RIGOL** 

###### **Allgemeine Sicherheits Informationen** 

Überprüfen Sie diefolgenden Sicherheitshinweise sorgfältigumPersonenschädenoderSchäden am Gerätundan damit verbundenen weiteren Gerätenzu vermeiden. Zur Vermeidung vonGefahren, nutzen Sie bitte das Gerät nur so, wiein diesem Handbuchangegeben. 

###### **Um Feuer oder Verletzungen zu vermeiden, verwenden Sie ein ordnungsgemäßes Netzkabel.** 

Verwenden Sie für dieses Gerät nur das für ihr Land zugelassene und genehmigte Netzkabel. 

###### **Erden des Gerätes.** 

Das Gerät ist durch den Schutzleiter im Netzkabel geerdet. Um Gefahren durch elektrischen Schlag zu vermeiden, ist es unerlässlich, die Erdung durchzuführen. Erst dann dürfen weitere Ein- oder Ausgänge verbunden werden. 

###### **Anschluss einesTastkopfes.** 

Die Erdungsklemmen der Sonden sindauf dem gleichen Spannungspegel des Instruments geerdet. SchließenSie die Erdungsklemmen an keine hohe Spannung an. 

###### **Beachten Sie alle Anschlüsse.** 

Zur Vermeidung von Feuer oder Stromschlag, beachten Sie alle Bemerkungen und Markierungen auf dem Instrument. Befolgen Sie die Bedienungsanleitung für weitere Informationen, bevor Sie weitere Anschlüsse an das Instrument legen. 

###### **Verwenden Sie einen geeigneten Überspannungsschutz.** 

Stellen Sie sicher, daß keinerlei Überspannung (wie z.B. durch Gewitter verursacht) das Gerät erreichen kann. Andernfallsbestehtfür den Anwender die GefahreinesStromschlages. 

###### **Nicht ohne Abdeckung einschalten.** 

Betreiben Sie das Gerät nicht mit entfernten Gehäuse-Abdeckungen. 

###### **Betreiben Sie das Gerät nicht geöffnet.** 

Der Betrieb mit offenen oder entfernten Gehäuseteilen ist nicht zulässig. Nichts in entsprechende Öffnungen stecken (Lüfter z.B.) 

###### **Passende Sicherung verwenden.** 

Setzen Sie nur die spezifikationsgemäßen Sicherungen ein. 

###### **Vermeiden Sie ungeschützte Verbindungen.** 

Berühren Sie keine unisolierten Verbindungen oder Baugruppen, während das Gerät in Betrieb ist. 

DP800 User’s Guide 

V 

**RIGOL** 

###### **Betreiben Sie das Gerät nicht im Fehlerfall.** 

Wenn Sie am Gerät einen Defekt vermuten, sorgen Sie dafür, bevor Sie das Gerät wieder betreiben, dass eine Untersuchung durch **RIGOL** autorisiertem Personal durchgeführt wird. Jedwede Wartung, Einstellarbeiten oder Austausch von Teilen am Gerät, sowie am Zubehör dürfen nur von **RIGOL** autorisiertem Personal durchgeführt werden. 

###### **Belüftung sicherstellen.** 

Unzureichende Belüftung kann zu Temperaturanstiegen und somit zu thermischen Schäden am Gerät führen. Stellen Sie deswegen die Belüftung sicher und kontrollieren regelmäßig Lüfter und Belüftungsöffnungen. 

###### **Nicht in feuchter Umgebung betreiben.** 

Zur Vermeidung von Kurzschluß im Geräteinneren und Stromschlag betreiben Sie das Gerät bitte niemals in feuchter Umgebung. 

###### **Nicht in explosiver Atmosphäre betreiben.** 

Zur Vermeidung von Personen- und Sachschäden ist es unumgänglich, das Gerät ausschließlich fernab jedweder explosiven Atmosphäre zu betreiben. 

###### **Geräteoberflächen sauber und trocken halten.** 

Um den Einfluß von Staub und Feuchtigkeit aus der Luft auszuschließen, halten Sie bitte die Geräteoberflächen sauber und trocken. 

###### **Schutz gegen elektrostatische Entladung (ESD).** 

Sorgen Sie für eine elektrostatisch geschützte Umgebung, um somit Schäden und Funktionsstörungen durch ESD zu vermeiden. Erden Sie vor dem Anschluß immer Innen- und Außenleiter der Verbindungsleitung, um statische Aufladung zu entladen. 

###### **Die richtige Verwendung desAkku.** 

Wenneine Batterieverwendet wird, vermeiden Sie hohe Temperaturen bzw. Feuer ausgesetzt werden. Bewahren Sie es außerhalbder Reichweitevon Kindern auf. UnsachgemäßeÄnderung derBatterie (Anmerkung: Lithium-Batterie) kann zu einer Explosion führen. VerwendenSie nur von **RIGOL** angegebenenAkkus. 

###### **Sicherer Transport.** 

Transportieren Sie das Gerät sorgfältig (Verpackung!), um Schäden an Bedienelementen, Anschlüssen und anderen Teilen zu vermeiden. 

###### **Vermeiden Sie das einprägen von Strom und Spannung an den Testklemmen.** 

Das DP800 Power Supply kann hierdurch zerstört werden, keine akive Last. Das DP800 kann nur Strom und Spannungen leifern. 

DP800 User’s Guide 

VI 

**RIGOL** 

###### **Sicherheits Begriffe und Symbole** 

###### **Begriffe in diesem Guide:** 

###### **WARNING** 

Die Kennzeichnung WARNING beschreibt Gefahrenquellen die leibliche <u>Schäden oder den Tod von Personen zur Folge haben können.</u> 

###### **CAUTION** 

Die Kennzeichnung Caution (Vorsicht) beschreibt Gefahrenquellen die <u>Schäden am Gerät hervorrufen können.</u> 

###### **Begriffe auf dem Produkt:** 

**DANGER** weist auf eine Verletzung oder Gefährdung hin, die sofort geschehen kann. 

**WARNING** weist auf eine Verletzung oder Gefährdung hin, die möglicherweise nicht sofort geschehen. 

**CAUTION** weist auf eine Verletzung oder Gefährdung hin und bedeutet, dass eine mögliche Beschädigung des Instruments oder anderer Gegenstände auftreten kann. 

###### **Symbole auf dem Produkt:** 



<!-- Start of picture text -->
Gefährliche  Sicherheits-<br>Spannung Hinweis<br><!-- End of picture text -->



<!-- Start of picture text -->
Schutz-erde Gehäusemasse  Erde<br><!-- End of picture text -->

DP800 User’s Guide 

VII 

**RIGOL** 

###### **Care and Cleaning** 

###### **Care** 

Do not store or leave the instrument where it may be exposed to direct sunlight for long periods of time. 

###### **Cleaning** 

Clean the instrument regularly according to its operating conditions. 

1. Disconnect the instrument from all power sources. 

2. Clean the external surfaces of the instrument with a soft cloth dampened with mild detergent or water. When cleaning the LCD, take care to avoid scarifying it. 

###### **CAUTION** 

To avoid damage to the instrument, do not expose it to caustic liquids. 

###### **WARNING** 

To avoid short-circuit resulting from moisture or personal injuries, ensure that the instrument is completely dry before connecting it to the power <u>supply.</u> 

DP800 User’s Guide 

VIII 

**RIGOL** 

###### **Environmental Considerations** 

The following symbol indicates that this product complies with the requirements in WEEE Directive 2002/96/EC. 



###### **Product End-of-Life Handling** 

The equipment may contain substances that could be harmful to the environment or human health. To avoid the release of such substances into the environment and avoid harm to human health, we recommend you to recycle this product appropriately to ensure that most materials are reused or recycled properly. Please contact your local authorities for disposal or recycling information. 

DP800 User’s Guide 

IX 



<!-- Start of picture text -->
From: RIGOL Technologies Inc Date:May5.2014<br>156# Cai He Village, Sha He Town, Chang Ping District, Beijing, China<br>http://www.nigol.com<br>Declaration of ROAHS& WEEE Conformity<br>RoHS: The European Union of 2011/65/EU Restriction of Hazardous Substances(RoHS) Directive, which<br>applies to all electrical products and their component parts, offered for sale into the European Union<br>afterJuly 22, 2014, restricts the presence of the six substances: Lead(Pb), Cadmium(Cd), Mercury(Hg),<br>Hexavalent Chromium(Cr’’), Polybrominatedbiphenylethers(PBBs),and<br>Polybrominateddiphenylethers(PBDEs), In view of legal and market requirements,Rigol has restricted use of<br>Pb,Cd, Hg, Cr°’, PBBs, PBDEs in our products. In addition , we require all our direct suppliers to strictly<br>limit or prohibit use of hazardous substances. All its AVL(Approved Vendors List) components apply for<br>RoHS.<br>Contentof Compliance<br><0,01% by weight (100 ppm) <0.1% by weight (100 ppm)<br><0.1% by weight (1000 ppm) <0.1% by weight (1900 ppm)<br>WEEE: The European Union of2012/19/EU, Waste Electrical and Electronic Equipment percentaze of<br>reused, recycled and recovered materials, such as metals + plastics, and components of waste electrical and<br>electronic equipment.<br>We are registered in Germany in the WEEE(in Germany,EAR) register as a manufacturer of caegory<br>9(monitoring and control)equipment and have the assigned registration number<br>WEEE-Reg. -No. : DE88132002<br>in our commercial documents.<br>RIGOL Technologies In<br>my<br>SaneNe<br>Weiming Mao<br>Director of Central Quality and<br>Environmental Management<br><!-- End of picture text -->

**RIGOL** 

##### **DP800 Series Overview** 

DP800 series is high-performance programmable linear DC power supply. DP800 series which provides clear user interface, superb performance specifications, various analysis functions as well as various communication interfaces can fulfill versatile test requirements. 

###### **Main Features:** 

###### **User-friendly Design:** 

- 3.5 inch TFT display, can display multiple parameters and states at the same time 

- Support Chinese/English interface and input method 

- Novel and exquisite industrial design and easy operation 

- Provide waveform display function to provide real-time and dynamic display of the output voltage/current waveform, clearly showing the output state and tendency of the instrument in together with the digital display of the voltage, current and power values 

- Provide dial display function, indicating the current output state using the dial pointer by simulating the traditional power display mode 

- Provide on-line help system for easy acquisition of help information 

###### **Multiple Safety Protection:** 

- Provide overvoltage/overcurrent protection function; users can set the overvoltage and overcurrent parameters to realize effective protection of the load 

- Provide secondary over-temperature protection function to realize double over-temperature protection of the software and hardware 

- Provide intelligent fan speed control function; judge and control the fan speed automatically according to the working condition to effectively reduce the fan noise 

- Provide fan failure check and alarm function 

- Provide keyboard lock function to avoid misoperation 

###### **Various Functions and Superb Performance:** 

- Multi-channel output; up to 200W total output power; the output of each channel can be controlled independently 

- Superb load regulation rate and linear regulation rate 

- Provide ultra-low output ripples and noise 

- Provide timing output function and support infinite and specified number of cycles of output 

- Provide track function to track the channel voltage setting value and output on/off status 

- Provide Sense function to automatically compensate for the voltage drop caused by the load lead in high current output. 

- Provide delay on/off output function and support infinite or specified number of 

DP800 User’s Guide 

XI 

**RIGOL** 

cycles of channel on/off toggle 

- Support digital trigger (option) to realize digital trigger input and trigger output functions 

- Support on-line analyzer (option) to perform on-line analysis of various statistic parameters 

- Support monitor (option) to monitor the output according to the user-defined monitor condition 

- Provide built-in recorder to perform background recording of the output state after power-on according to certain record period 

- Provide dedicated preset key to perform one-key reset and one-key recall of the commonly used output voltage and current configurations 

- Support serial and parallel output functions 

- Support on-line self-test and manual calibration functions 

- Provide store and recall function 

- Support voltage and current linear programmable functions 

###### **Complete Interface Configurations and Flexible Control Method:** 

- Standard configuration interfaces: USB HOST, USB DEVICE, LAN (option), RS232 (option), Digital I/O (option), rear panel output interface (DP811) 

- Support to extend a GPIB interface via the USB-GPIB interface converter (option) 

- Support USB storage device (FAT32 format, flash memory) storage 

- Support SCPI remote command control 

- Conform to LXI Core 2011 Device instrument standard 

- Provide standard 9 pin RS232 interface with flow control function 

- Provide 4-wire digital I/O interface which supports the trigger input/output function 

DP800 User’s Guide 

XII 

**RIGOL** 

##### **Document Overview** 

###### **Chapter 1 Quick Start** 

Introduce the appearance and dimensions, front panel, rear panel as well as user interface of DP800. In addition, it provides the detailed procedures of power connection, power-on inspection and fuse replacement as well as the method of rack mount installation of DP800. 

###### **Chapter 2 Front Panel Operations** 

Introduce the function and operation method of each key at the front panel of DP800 in detail. 

###### **Chapter 3 Remote Control** 

Introduce how to realize the remote control of the instrument. 

###### **Chapter 4 Troubleshooting** 

Introduce the possible failures and their solutions when using DP800. 

###### **Chapter 5 Specifications** 

List the specifications of DP800. 

###### **Chapter 6 Appendix** 

Provide the accessory and option list as well as the warranty information of DP800. 

###### **Index** 

Provide keyword search to quickly locate the desired information. 

###### **Tip** 

For the newest version of this manual, download it from **RIGOL** official website (www.rigol.com). 

DP800 User’s Guide 

XIII 

**RIGOL** 

###### **Format Conventions in this Manual** 

**1. Key** 

   - The function key at the front panel is denoted by the format of "Key Name (Bold) + Text Box" in the manual. For example, **<u>Utility</u>** denotes the "Utility" key. 

###### **2. Menu** 

The menu item is denoted in the following two modes in this manual. 

- (1) The menu item can be denoted by the format of "Menu Word (Bold) + <u>Character Shading". For example,</u> **<mark>System</mark>** denotes the "System" item under **<u>Utility</u>** . 

- (2) The menu item can be denoted by the screenshot of the menu key. For example, . 

###### **3. Operation Step** 

The next step of the operation is denoted by an arrow "" in the manual. For example, **<u>Utility</u>**  **<mark>System</mark>** denotes pressing **<u>Utility</u>** at the front panel and then pressing **<mark>System</mark>** <mark>.</mark> 

###### **Content Conventions in this Manual** 

DP800 series programmable linear DC power supply includes the following models. Unless otherwise noted, this manual illustrates the functions and operation methods of DP800 series by taking DP832 as an example. 

|**Model**|**Number of Channels**|**Channel Output Voltage/Current**|
|---|---|---|
|DP832|3|30V/3A, 30V/3A, 5V/3A|
|DP831|3|8V/5A, 30V/2A,-30V/2A|
|DP821|2|60V/1A, 8V/10A|
|DP811|1|Range1: 20V/10A;Range2: 40V/5A|



DP800 User’s Guide 

XIV 

Contents 

**RIGOL** 

##### **Contents** 

|**Guaranty and Declaration ......................................................................... I**|
|---|
|**Safety Requirement ................................................................................ II**|
|General Safety Summary ........................................................................... II|
|Safety Notices and Symbols ...................................................................... IV|
|Allgemeine Sicherheits Informationen ......................................................... V<br>|
|Sicherheits Begriffe und Symbole ............................................................. VII<br>|
|Care and Cleaning ................................................................................. VIII|
|Environmental Considerations ................................................................... IX|
|**DP800 Series Overview .......................................................................... XI**|
|**Document Overview ............................................................................ XIII**|
|**Chapter 1**<br>**Quick Start ......................................................................... 1-1**|
|General Inspection ................................................................................ 1-2|
|Appearance and Dimensions ................................................................... 1-3<br>|
|Front Panel ........................................................................................... 1-4<br>|
|Rear Panel ........................................................................................... 1-11|
|To Connect to Power ............................................................................. 1-13|
|Power-on Inspection ............................................................................. 1-14|
|To Replace the Fuse ............................................................................. 1-14|
|User Interface ...................................................................................... 1-16|
|To Use the Built-in Help System ............................................................. 1-18|
|**Chapter 2**<br>**Front Panel Operations ...................................................... 2-1**|
|Constant Voltage Output ........................................................................ 2-2<br>|
|Constant Current Output ........................................................................ 2-6|
|Power Supply Series and Parallel Connections .......................................... 2-8|
|Power Supply Series Connection ....................................................... 2-8|
|Power Supply Parallel Connection ..................................................... 2-9<br>|
|Track Function ..................................................................................... 2-10|
|To Enable the Track Function .......................................................... 2-10|
|To Track the On/Off Status of the Channel Output ............................. 2-12|
|Sense Working Mode ............................................................................ 2-13<br>|
|Timer and Delayer ................................................................................ 2-15|
|To Set the Timer Parameters ........................................................... 2-16|
|To Enable the Timer ....................................................................... 2-25|
|To Set the Delay Parameters ........................................................... 2-26|
|To Enable the Delayer .................................................................... 2-30|
|Advanced Functions .............................................................................. 2-31|
|Recorder ....................................................................................... 2-32|
|Analyzer (option) ........................................................................... 2-33|
|Monitor (option) ............................................................................ 2-36<br>|
|Trigger (option) ............................................................................. 2-37|



DP800 User’s Guide 

XV 

Contents 

|**RIGOL**<br>Contents|
|---|
|Display Setting ..................................................................................... 2-41|
|Brightness .................................................................................... 2-41|
|Contrast ....................................................................................... 2-41|
|RGB Luminance ............................................................................. 2-41|
|Display Mode ................................................................................ 2-42|
|Display Theme .............................................................................. 2-42|
|User-defined Start-up Interface ....................................................... 2-43|
|Store and Recall ................................................................................... 2-45|
|Browser ........................................................................................ 2-46|
|File Type ....................................................................................... 2-46|
|Save ............................................................................................. 2-47|
|Read ............................................................................................ 2-49|
|Delete .......................................................................................... 2-49|
|Copy and Paste ............................................................................. 2-50|
|Utility .................................................................................................. 2-51|
|I/O Configuration .......................................................................... 2-52|
|System Setting .............................................................................. 2-57|
|System Information ....................................................................... 2-59|
|System Language .......................................................................... 2-59|
|Test/Calibration ............................................................................. 2-59|
|Print ............................................................................................. 2-60|
|Preset Setting................................................................................ 2-61|
|Options ........................................................................................ 2-68|
|Key Locking ......................................................................................... 2-70|
|**Chapter 3**<br>**Remote Control ................................................................. 3-1**|
|Remote Control via USB .......................................................................... 3-2|
|Remote Control via LAN (option).............................................................. 3-5|
|Remote Control via GPIB (option) ............................................................ 3-8|
|Remote Control via RS232 (option) ........................................................ 3-10|
|**Chapter 4**<br>**Troubleshooting ................................................................ 4-1**|
|**Chapter 5**<br>**Specifications .................................................................... 5-1**|
|**Chapter 6**<br>**Appendix ........................................................................... 6-1**|
|Appendix A: Accessories and Options ....................................................... 6-1|
|Appendix B: Warranty ............................................................................. 6-2|
|**Index ........................................................................................................ 1**|



DP800 User’s Guide 

XVI 

Chapter 1 Quick Start 

**RIGOL** 

##### **Chapter 1 Quick Start** 

The contents of this chapter are as follows: 

- General Inspection 

- Appearance and Dimensions 

- Front Panel 

- Rear Panel 

- To Connect to Power 

- Power-on Inspection 

- To Replace the Fuse 

- User Interface 

- To Use the Built-in Help System 

DP800 User’s Guide 

1-1 

Chapter 1 Quick Start 

**RIGOL** 

###### **General Inspection** 

###### **1. Inspect the packaging** 

If the packaging has been damaged, do not dispose the damaged packaging or cushioning materials until the shipment has been checked for completeness and has passed both electrical and mechanical tests. 

The consigner or carrier shall be liable for the damage to the instrument resulting from shipment. **RIGOL** would not be responsible for free maintenance/rework or replacement of the instrument. 

###### **2. Inspect the instrument** 

In case of any mechanical damage, missing parts, or failure in passing the electrical and mechanical tests, contact your **RIGOL** sales representative. 

###### **3. Check the accessories** 

Please check the accessories according to the packing lists. If the accessories are damaged or incomplete, please contact your **RIGOL** sales representative. 

DP800 User’s Guide 

1-2 



<!-- Start of picture text -->
239<br>\ a" }<br>L/LaiM4 d<br>D WeeYSow |<br><!-- End of picture text -->



<!-- Start of picture text -->
418<br>060 pO p06 05 08050686 8585088<br>0}\ fayOFOG0G0808SasDLOLOL420Lahtebteh oZogofaloPoSoSagooSa8o taba) OB DS0R0508080R08nete}-te) te) teh teh te) a<br>iH| BBOVS2Q6369 a9P6PSL 0852595269868 62020285 S G26<br>I}<br>‘@@<br>|_—— ————F<br>Cy|| 04,0,,0,0,0,0,0,0,0,0,0,0,0eyDol ehresetaseteneteheroPa2%aPoPoPNerehaeyeresehaseTeherhooP aP0P%aPa2o<br>iS @ tabcteictes=969ripetenetalGP 59596959 t oh=al t eh-te)obnob  tK G9 e hbetepctehate=toh teyGF cepeiel APS9SPSPOte} te bte))ote)<br>\<br><!-- End of picture text -->



<!-- Start of picture text -->
‘aay erey<br>sl: pf 3<br>ae cal<br><!-- End of picture text -->













<!-- Start of picture text -->
C S V<br><!-- End of picture text -->

Chapter 1 Quick Start 

**RIGOL** 

the keys are used to enter the voltage units (V and mV) or the current units (A and mA). 

###### (2) Numeric Keyboard 

Ring-type numeric keyboard: include numbers 0-9 and the decimal point. Press the corresponding key to directly input the number or decimal point. 

###### (3) Knob 

When setting parameters, rotate the knob to increase or reduce the value at the cursor. 

When browsing the setting objects (timer parameters, delay parameters, filename input, etc.), rotate the knob to quickly move the cursor. 

###### **4.** **<u>Preset</u>** 



Restore all the settings of the instrument to default values or recall the user-defined channel voltage/current configuration. 

###### **5.** **<u>OK</u>** 



Confirm the parameter setting. 

Press and hold this key to lock the front panel keys; at this point, 

the front panel keys (except the output on/off key of each 



channel and the power switch key ) are not available. When the keyboard lock password is disabled, press and hold this key again to unlock the front panel keys. When the keyboard lock password is enabled, you need to input the correct password (2012) to unlock the front panel keys. 

###### **6.** **<u>Back</u>** 



Delete the character currently before the cursor. When the instrument is in remote mode, press this key to return to local mode. 

DP800 User’s Guide 

1-7 

Chapter 1 Quick Start 

**RIGOL** 

###### **7. Output Terminals** 

The output terminals of different models of DP800 series are different. 

###### **DP832:** 



<!-- Start of picture text -->
(1)                (2)<br><!-- End of picture text -->

###### **DP821:** 



<!-- Start of picture text -->
(3)<br>60V/1A 8V/10A<br>DP811:<br>(1)<br>Output<br>S S<br><!-- End of picture text -->



- (1) Channel output terminals: used to output the voltage and current of the channel. 

- (2) Ground terminal: this terminal is connected to the instrument chassis and ground wire (the ground terminal of the power cord) and is in grounded state. 

- (3) Sense terminals: used to detect the actual voltage at the load terminal so as to compensate for the voltage drop caused by the load lead. 

DP800 User’s Guide 

1-8 











Chapter 1 Quick Start 

**RIGOL** 



Press this key to enter the system utility function setting interface. Users can set the remote interface parameters, system parameters and print parameters. Besides, users can also calibrate the instrument, view system information, define the recall configuration of **<u>Preset</u>** and install options. 



Press this key to enter the advanced function setting interface. Users can set the recorder, analyzer (option), monitor (option) and trigger (option) parameters. 



Press this key to enter the timer and delayer setting interface. Users can set the timer and delayer parameters as well as enable and disable the timer and delayer functions. 



Press this key to open the built-in help system and press the desired key to get the corresponding help information. For detailed introductions, refer to " **To Use the Built-in Help System** ". 

###### **9. Display Mode Switch/Return to the Main Interface** 

Switch between the current display mode and dial display mode. 

Besides, when the instrument is in a function interface (any interface under **<u>Timer</u>** , , **<u>Display</u>** , **<u>Store</u>** and **<u>Utility</u>** <u>),</u> press this key to exit the function interface and return to the main interface. 

###### **10. Menu Keys** 

The menu keys correspond to the menus above them respectively. Press any menu key to select the corresponding menu. 

###### **11.** **<u>Power Switch Key</u>** 



Turn on or off the instrument. 

DP800 User’s Guide 

1-10 

Chapter 1 Quick Start 

**RIGOL** 

###### **Rear Panel** 

This section introduces the rear panel of DP800 series by taking DP832 and DP811 (as shown in the figures below) as examples. The introduction of each part is as shown in Table 1-1. 



<!-- Start of picture text -->
         1      2     3        4            5      6<br>Opt. Opt. Opt.<br>AC 100V<br>T6.3A AC 115V<br>500VA T3.15A AC 230V<br>          10                    9                 8  7<br>Figure 1-4 DP832 Rear Panel<br><!-- End of picture text -->



<!-- Start of picture text -->
Opt. Opt. Opt.<br>AC 100V<br>T6.3A AC 115V<br>500VA T3.15A AC 230V<br>S+ S<br>11<br><!-- End of picture text -->

Figure 1-5 DP811 Rear Panel 

DP800 User’s Guide 

1-11 

Chapter 1 Quick Start 

**RIGOL** 

<u>Table 1-1 DP800 rear panel explanation</u> 

|**No. **|<br>**Name**|<br>**Explanation **|
|---|---|---|
|1|LAN Interface<br>(option)|The instrument is connected to the local network via<br>RJ45 interface|
|2|USB DEVICE|Connect the instrument (as "slave" device) to<br>externalUSBdevice (suchasPC)|
|3|USB HOST|Connect the instrument (as "host" device) to external<br>USB device (such as USB storage device);<br>extend a GPIB interface for the power supply using<br>USB-GPIB interface converter (option)|
|4|Digital I/O (option)|Digital I/O interface|
|5|RS232 Interface<br>(option)|Serial communication interface|
|6|Voltage Selector|Select the specification of the input voltage (100, 115<br>or 230; please refer to Table 1-2)|
|7|PowerSocket|AC power inputinterface.|
|8|Fuse|The specification of the fuse is related to the<br>instrument model and actual input voltage (please<br>refer to the "Input Power Requirements" at the rear<br>panel of the instrument or refer to Table 1-3).|
|9|Fan||
|10|Input Power<br>Requirement|Corresponding relations of the AC input power<br>frequency, voltage and the specification of the fuse.|
|11|Output Interface|DP811 provides this interface. The function of this<br>interface is the same as that of the "**Output**<br>**Terminals**"at thefront panel.|



**Note:** The "Output Terminals" at the front panel and "Output Interface" at the rear panel cannot be used for output at the same time. You can only select one of them for output at each time (wherein, the output terminals at the front panel provide higher output accuracy). 

DP800 User’s Guide 

1-12 

Chapter 1 Quick Start 

**RIGOL** 

###### **To Connect to Power** 

DP800 series power supply supports various AC power supply inputs. The voltage selector setting at the rear panel differs when the input power connected is different, as shown in the table below. 

|Table 1-2 AC input power specifications (including voltage selector settings)|
|---|



|**AC Input Power **|**Voltage Selector Setting**|
|---|---|
|100Vac±10%, 50Hz to 60Hz|100|
|115Vac±10%, 50Hzto 60Hz|115|
|230Vac±10% (250Vacmaximum), 50Hzto 60Hz|230|



Please connect the power following the steps below. 

###### **1. Check the input power** 

Make sure that the AC power to be connected to the instrument fulfills the requirements in Table 1-2. 

###### **2. Check the voltage selector at the rear panel** 

- Make sure that the voltage selector setting (110, 115 or 230) at the rear panel of the instrument matches the actual input voltage (for the matching relations refer to Table 1-2). 

###### **3. Check the fuse** 

When the instrument leaves factory, the specified fuse is installed. Please check whether the fuse matches the actual input voltage according to the "Input Power Requirements" at the rear panel of the instrument or Table 1-3. 

###### **4. Connect the AC power** 

Connect the instrument to AC power supply using the power cord provided in the accessories. 

###### **WARNING** 

To avoid electric shock, make sure that the instrument is correctly <u>grounded.</u> 

DP800 User’s Guide 

1-13 

Chapter 1 Quick Start 

**RIGOL** 

###### **Power-on Inspection** 

Press the power switch at the front panel, the instrument starts and executes self-test. If the instrument passes the self-test, the welcome interface will be displayed; otherwise, the corresponding self-test failure information (including the top board, bottom board, fan and temperature) will be displayed. 

###### **Tip** 

When powering on the instrument after powering off it, make sure that the time interval between the two operations is greater than 5s. 

###### **To Replace the Fuse** 

The fuse specification is related to the instrument model and actual input voltage, as shown in the table below. You can also refer to the "Input Power Requirements" at the rear panel of the instrument. 

<u>Table 1-3 Fuse specifications</u> 

|<br>**Input Voltage**|<br>**Fuse Specification**|
|---|---|
|**DP832/DP811**||
|100Vac/115Vac|T6.3A|
|230Vac|T3.15A|
|**DP831/DP821**||
|100Vac/115Vac|T5A|
|230Vac|T2.5A|



To replace the fuse, follow the steps below. 

1. Turn off the instrument and remove the power cord. 

2. Insert a small straight screwdriver into the slot at the power socket and prize out the fuse seat gently. 



<!-- Start of picture text -->
Fuse Seat<br>Fuse<br><!-- End of picture text -->

DP800 User’s Guide 

1-14 

Chapter 1 Quick Start 

**RIGOL** 

3. If needed, adjust the power voltage selector manually to select the voltage scale (please refer to Table 1-4) that matches the actual input voltage. 

4. Take out the fuse and replace it with a specified one (please refer to the "Input Power Requirements" at the rear panel of the instrument or Table 1-3). 

5. Re-insert the fuse seat into the power socket (pay attention to the direction). 

###### **WARNING** 

To avoid personal injuries, cut off the power supply before replacing the fuse; to avoid electric shock or fire, select the power supply specification that matches the actual input voltage and replace a fuse corresponding <u>to this specification before connecting to power.</u> 

DP800 User’s Guide 

1-15 



<!-- Start of picture text -->
[<br>a e<br>BOC Current [ovP off Jocp off |irack Off]<br><!-- End of picture text -->

om B DI [=| ci R <u>5</u> 

Chapter 1 Quick Start 

**RIGOL** 

###### **To Use the Built-in Help System** 

The built-in help system provides help information for any front panel key (except the parameter input area) and menu keys for users to quickly obtain the function prompts of the function keys or menus. 

###### **Obtain the help information of any key or menu key** 

Press **<u>Help</u>** to illuminate it and press the desired key or menu key to get the corresponding help information; at the same time, the backlight of **<u>Help</u>** goes off. Press to exit the help system. 

###### **Built-in help interface** 

Press **<u>Help</u>** to illuminate it and press **<u>Help</u>** again to open the built-in help interface. Use the up/down direction key or knob to select the desired help topic and press **<mark>View</mark>** to view the corresponding help information. 

The help topics include: 

1. View the last displayed message. 

2. View error queue of the remote commands. 

3. Get the help information of a key. 

4. Storage management. 

5. Abbreviations list. 

6. Series-parallel Help. 

7. Get technical support from **RIGOL** . 

DP800 User’s Guide 

1-18 

Chapter 2 Front Panel Operations 

**RIGOL** 

##### **Chapter 2 Front Panel Operations** 

The contents of this chapter are as follows: 

- Constant Voltage Output 

- Constant Current Output 

- Power Supply Series and Parallel Connections 

- Track Function 

- Sense Working Mode 

- Timer and Delayer 

- Advanced Functions 

- Display Setting 

- Store and Recall 

- Utility 

- Key Locking 

DP800 User’s Guide 

2-1 



<!-- Start of picture text -->
_<br>=<br><!-- End of picture text -->



<!-- Start of picture text -->
AS<br><!-- End of picture text -->

Chapter 2 Front Panel Operations 

**RIGOL** 

select the desired unit. Besides, you can also press **<u>OK</u>** to select the default unit (V). During the input, you can press **<u>Back</u>** to delete the character currently before the cursor or press **<mark>Cancel</mark>** to cancel the input. 

###### **Method 3** 

If **<mark>Current</mark>** is currently selected, you can also use the numeric keyboard to directly input the desired voltage value; and then press the unit selection key ( <u>or</u> ) to select the desired voltage unit. During the input, you can press **<u>Back</u>** to delete the character currently before the cursor or press **<mark>Cancel</mark>** to cancel the input. 

5. Set the current **Method 1** 

Press **<mark>Current</mark>** and use the left/right direction key to move the cursor; then, rotate the knob to quickly set the current and the default unit is A. After selecting the digit to be set, you can also use the up/down direction key to modify the value of the corresponding digit. The default unit is A. 

###### **Method 2** 

Press **<mark>Current</mark>** <mark>,</mark> use the numeric keyboard to directly input the desired current 

value and press **A** or **mA** or press the unit selection key ( or ) to select the desired unit. Besides, you can also press **<u>OK</u>** to select the default unit (A). During the input, you can press **<u>Back</u>** to delete the character currently before the cursor or press **<mark>Cancel</mark>** to cancel the input. 

###### **Method 3** 

If **<mark>Voltage</mark>** is currently selected, you can also use the numeric keyboard to directly input the desired current value; and then press the unit selection key ( <u>or</u> ) to select the desired current unit. During the input, you can press **<u>Back</u>** to delete the character currently before the cursor or press **<mark>Cancel</mark>** to cancel the input. 

6. Set the overcurrent protection Press **<mark>OCP</mark>** to set a proper overcurrent protection value (for the setting method, refer to " **Set the current** "). Then, enable the overcurrent protection function (you can enable or disable the OCP function by pressing **<mark>OCP</mark>** <mark>)</mark> and the output will be turned off automatically when the actual output current is greater than the overcurrent protection value. 

7. Turn on the output 

   - Turn on the output of the corresponding channel and the actual output voltage, output current, output power as well as the output mode (CV) are high-lighted in the user interface. 

DP800 User’s Guide 

2-3 

A ~~<u>R</u> AR~~ 



<!-- Start of picture text -->
mad *| + | —|S—<br>|<br>Te<br>For<br><!-- End of picture text -->

Chapter 2 Front Panel Operations 

**RIGOL** 

higher output accuracy). 

###### **Tip** 

In CV output mode, when the load current is greater than the current setting value, the power supply will switch to CC output mode automatically. At this point, the output current equals the current setting value and the output voltage reduces <u>proportionally.</u> 

DP800 User’s Guide 

2-5 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Constant Current Output** 

In constant current output mode, the output current equals the current setting value and the output voltage is determined by the load. This section introduces the operation method in constant current output mode. 

###### **Operation Method:** 

1. Connect the channel output terminals Connect the load with the channel output terminals of the corresponding channel at the front panel by referring to " **Connect the channel output terminals** " in "Constant Voltage Output". 

2. Turn on the power switch to start the instrument. 

3. Select the channel Select the proper output channel according to the desired output current. Press the corresponding channel selection key; at this point, this channel, its channel number, output state and output mode are high-lighted on the screen. 

4. Set the voltage 

   - Press **<mark>Voltage</mark>** to set the desired voltage according to " **Set the voltage** " in "Constant Voltage Output". 

5. Set the current 

Press **<mark>Current</mark>** to set the desired current according to " **Set the current** " in "Constant Voltage Output". 

6. Set the overvoltage protection 

Press **<mark>OVP</mark>** to set a proper overvoltage protection value (for the setting method, refer to " **Set the voltage** " in "Constant Voltage Output"). Then, enable the overvoltage protection function (you can enable or disable the OVP function by pressing **<mark>OVP</mark>** ) and the output will be turned off automatically when the actual output voltage is greater than the overvoltage protection value. 

7. Turn on the output 

Turn on the output of the corresponding channel and the actual output voltage, output current, output power as well as the output mode (CC) are high-lighted in the user interface. 

###### **Warning** 

To avoid electric shock, please turn on the output switch after the output <u>terminals are correctly connected.</u> 

DP800 User’s Guide 

2-6 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **CAUTION** 

When the fan stops, the channel switch cannot be turned on; otherwise, "The fan stops, stop the output!" will be displayed. 

###### 8. Check the output mode 

In constant current output mode, the output mode displayed should be "CC"; if "CV" is displayed, you can increase the voltage setting value properly and the power supply will switch to CC mode automatically. 

In addition, DP811 provides an output interface at the rear panel for constant current output. For the connecting method, please refer to the related introduction in " **Constant Voltage Output** ". 

**Note:** The output terminals at the front panel and the output interface at the rear panel cannot be used for output at the same time. You can only select one of them for output at each time (wherein, the output terminals at the front panel provide higher output accuracy). 

###### **Tip** 

In CC output mode, when the load voltage is greater than the voltage setting value, the power supply will switch to CV output mode automatically. At this point, the output voltage equals the voltage setting value and the output current reduces <u>proportionally.</u> 

DP800 User’s Guide 

2-7 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Power Supply Series and Parallel Connections** 

Higher voltages can be provided when two or more insulated channels (the channels can be from a single power supply or multiple power supplies) are connected in series. Higher currents can be provided when two or more insulated channels (the channels can be from a single power supply or multiple power supplies) are connected in parallel. 

###### **Note:** 

1. Only insulated channels can be connected in series or in parallel. 

   - For a single power supply, CH1 and CH2/CH3 of DP832 can be connected in series or in parallel, but CH2 and CH3 cannot be connected in series or parallel; any two of the three channels of DP831 can be connected in series, CH1 and CH2/CH3 can be connected in parallel, but CH2 and CH3 cannot be connected in parallel; the two channels of DP821 can be connected in series or in parallel. 

   - Channels (insulated channels) of different power supplies can be connected in series or parallel. 

2. In power series and parallel connections, the settings of the corresponding parameters must comply with the safety requirements. 

###### **Power Supply Series Connection** 

Higher voltages can be provided by connecting power supplies in series. In this case, the output voltage is the sum of the output voltages of all the channels. In power supply series connection, you need to set the same current setting value and overcurrent protection value for all the channels. Take the series connection of two channels as an example; the connection method is as shown in the figure below. 



<!-- Start of picture text -->
+<br>Power Supply<br>Channel #1 V1<br>- +<br>VL RLoad<br>-<br>+<br>Power Supply<br>Channel #2 - V2 VL=V1+V2<br><!-- End of picture text -->

###### **Operation Procedures:** 

1. Connect the power supply and load as shown in the figure above. Pay attention to the polarity when making connections. 

DP800 User’s Guide 

2-8 

Chapter 2 Front Panel Operations 

**RIGOL** 

2. Set proper voltage, current and overcurrent protection value for each channel (all the channels in series connection should be working in constant voltage mode; you should set the same current setting values and the same overcurrent protection values for all the channels) by referring to " **Constant Voltage Output** ". Turn on the output of each channel. 

**Note:** Make sure that all the channels in series connection are working in constant voltage mode. If one of the channels is working in constant current mode, the other channels will enter critical state in which their outputs are unpredictable. 

###### **Power Supply Parallel Connection** 

High currents can be provided by connecting power supplies in parallel. In this case, the output current is the sum of the output currents of all the channels. In power supply parallel connection, you can set the parameters of each power supply. Take the parallel connection of two channels as an example; the connection method is as shown in the figure below. 



<!-- Start of picture text -->
+ +<br>Power Supply  Power Supply<br>Channel #1 I1 Channel #2 I2<br>- -<br>IL RLoad<br>IL=I1+I2<br><!-- End of picture text -->

###### **Operation Procedures:** 

1. Connect the power supply and load as shown in the figure above. Pay attention to the polarity when making connections. 

2. Set proper parameters for each channel (all the channels can work in constant voltage or constant current mode) by referring to " **Constant Voltage Output** " and " **Constant Current Output** ". Turn on the output of each channel. 

**Note:** All the channels can work in constant voltage or constant current mode according to the actual need of the load. 

DP800 User’s Guide 

2-9 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Track Function** 

The track function is usually used to provide symmetric voltage for the operation amplifier or other circuit. For DP800 series power supply, the specified channels of the following models support the track function. 

DP832: CH1, CH2 DP831: CH2, CH3 

For channels that support the track function, you can set the tracking states of the voltage setting value and output on/off state respectively according to your need. 

###### **To Enable the Track Function** 

- Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>Track</mark>** to switch between "Synchronous" and "Independent". 

- **Independent:** for two channels (the channels should be of the same instrument) that support the track function, the status of the track function of the other channel will not be affected when the track function of a channel is enabled or disabled. 

- **Synchronous:** for two channels (the channels should be of the same instrument) that support the track function, the track function of the other channel will be enabled or disabled at the same time when the track function of a channel is enabled or disabled. 

###### **Tip** 

- When "Independent" is selected, users can enable or disable the track function of a single channel freely. 

- When "Synchronous" is selected, users can quickly enable or disable the track <u>functions of the two channels at the same time.</u> 

For channels (the channels should be of the same instrument) that support the track function, the tracking status of the channel voltage setting values is related to the number of channels of which the track functions are enabled. 

- If only the track function of a single channel is enabled, the voltage setting value of the other channel will change accordingly when the voltage setting value of this channel is modified. At this point, the voltage setting value of the channel of which the track function is not enabled cannot be set and can only change with that of the channel of which the track function is enabled. 

- If the track functions of the two channels are both enabled, the voltage setting value of the other channel will change accordingly when the voltage setting value of a channel is modified. At this point, the voltage setting values of both of the two channels can be set. 

DP800 User’s Guide 

2-10 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Tip** 

The track function only tracks the voltage setting value and the actual output <u>voltage will not be affected.</u> 

You can perform the following operations according to your need. 

###### **1. Only enable the track function of a single channel and track the voltage setting value** 

The operation procedures are as follows (in this example, only the track function of CH1 of DP832 is enabled). 

- Select the "Independent" tracking mode Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>Track</mark>** to select "Independent". 

- Enable the track function of CH1 Select CH1 and press **<mark>Track</mark>** to select "On". At this point, the track function of CH1 is enabled and the tracking status icon is displayed between the CH1 and CH2 areas in the user interface. 

- Disable the track function of CH2 (ignore this step if the track function of CH2 is currently disabled) 

   - Select CH2 and press **<mark>Track</mark>** to select "Off". At this point, the track function of CH2 is disabled. 

- Track the voltage setting value Select CH1, press **<mark>Voltage</mark>** and set the desired voltage. At this point, the voltage of CH2 changes accordingly. For example, set the voltage of CH1 to +5V; the voltage of CH2 will change to +5V automatically. 

**Note:** At this point, the voltage setting value of CH2 cannot be set and can only change with that of CH1. 

###### **2. Enable the track functions of both of the two channels and track the voltage setting value** 

First, you can enable the track functions of the two channels using any of the following methods (take DP832 as an example). 

###### **Method 1** 

- Select the "Synchronous" tracking mode Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>Track</mark>** to select "Synchronous". 

- Enable the track functions of CH1 and CH2 at the same time Select CH1 or CH2, press **<mark>Track</mark>** and select "On". At this point, the track functions of CH1 and CH2 are both enabled. The tracking status icon is 

DP800 User’s Guide 

2-11 

Chapter 2 Front Panel Operations 

**RIGOL** 

displayed between the CH1 and CH2 areas in the user interface. 

###### **Method 2** 

- Select the "Independent" tracking mode Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>Track</mark>** to select "Independent". 

- Enable the track functions of CH1 and CH2 Select CH1 and press **<mark>Track</mark>** to select "On". At this point, the track function of CH1 is enabled and the tracking status icon is displayed between the CH1 and CH2 areas in the user interface. Use the same method to enable the track function of CH2. 

Then, select CH1 or CH2 and press **<mark>Voltage</mark>** and set the desired voltage. At this point, the voltage setting value of the other channel changes accordingly. For example, set the voltage of CH1 to +5V; the voltage of CH2 will change to +5V automatically. 

**Note:** At this point, both of the voltage setting values of CH1 and CH2 can be set. 

###### **To Track the On/Off Status of the Channel Output** 

###### Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>On/Off</mark>** to select "Enable" or "Disable". 

- **Disable:** the output status of the other channel will not be affected when the output of a channel is turned on or off. 

- **Enable:** for channels (the channels should be of the same instrument) that support the track function, the tracking status of the on/off status of the channel output is related to the number of channels of which the track functions are enabled. 

   - If only the track function of a single channel is enabled (please refer to " **To Enable the Track Function** "), the output of the other channel will be turned on or off at the same time when the output of this channel is turned on or off. At this point, the output status of the channel of which the track function is not enabled cannot be set and can only change with that of the channel of which the track function is enabled. 

   - If the track functions of the two channels are both enabled (please refer to " **To Enable the Track Function** "), the output of the other channel will be turned on or off at the same time when the output of a channel is turned on or off. At this point, the output status of both of the two channels can be set. 

DP800 User’s Guide 

2-12 







fas*| + | —|S- 



<!-- Start of picture text -->
fas*| + | —|S-<br>ce<br><!-- End of picture text -->

~~<u>[</u>~~ 







Chapter 2 Front Panel Operations 

**RIGOL** 

###### **To Set the Number of Output Groups** 

The number of output groups is defined as the number of groups of preset voltage and current values that the power supply outputs in each cycle. Press **<mark>Groups</mark>** and use the numeric keyboard or the left/right direction key and knob to input the desired value. The range is from 1 to 2048. 

###### **To Set the Number of Cycles** 

The number of cycles is defined as the number of times that the instrument performs timing output according to the preset voltage and current. Press **<mark>Cycles</mark>** to set the number of cycles to "Infinite" or use the numeric keyboard or the left/right direction key and knob to input the desired value. The range is from 1 to 99999. 

###### **Tip** 

- The total number of groups in timing output = the number of output groups* the number of cycles 

- The power supply will terminate the timer function when the total number of groups of outputs is finished. At this point, the state of the power supply " " 

- <u>depends on the setting in</u> **<u>To Set the End State</u>** <u>.</u> 

###### **To Set the End State** 

The end state refers to the state of the instrument after it finishes outputting the total number of groups of voltage and current values when the number of cycles is a specified value. Press **<mark>End State</mark>** to select "Outp Off" or "Last". 

- Outp Off: the instrument turns off the output automatically after finishing the output. 

- Last: the instrument stops at the output state of the last group after finishing the output. 

**Note:** When the number of cycles is set to "Infinite", the end state is invalid. 

###### **To Edit the Timer Parameters** 

You can edit the timer parameters manually or using the built-in templet. 

DP800 User’s Guide 

2-17 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **To Edit the Timer Parameters Manually** 

You need to set **P** groups (No.0 to No.(P-1)) of timer parameters; wherein, **P** is the number of output groups currently set. Only 6 groups of parameters can be 





displayed on each page of the timer parameter list and you can press or to view and set the parameters of the other groups. This interface provides timer parameter preview; users can view the values on the current page of the timer parameter list (the horizontal axis represents time and the vertical axis represents voltage and current). 

1. Press **<mark>Parameter</mark>** and use the numeric keyboard, the left/right direction key or the knob to select the specified number (No.) in the timer parameter list. 

2. Use the up/down direction key to select the voltage (V), current (C) and time (s) of the current group respectively and use the numeric keyboard or the left/right direction key and knob to input the desired value. 

3. Use the same method to set the parameters of the other groups. 

###### **To Edit the Timer Parameters using Templet** 

The instrument provides various built-in output templets. Users can select the desired templet, edit the timer parameters using the templet and create waveform. The instrument will output according to the waveform currently created. 

Press **<mark>Templet</mark>** to open the templet editing menu. You can edit the timer parameters according to the steps below. 

1. Select the editing object 

2. Select the templet 

3. Edit the timer parameters 

4. Construct the output waveform 

###### **1. Select the editing object** 

Press **<mark>Edit Obj</mark>** to select "Voltage" or "Current". 

- Voltage: when it is selected, the interface displays the voltage preview and you can set all the groups to output a fixed current. Press **<mark>Current</mark>** and use the numeric keyboard or the left/right direction key and knob to set the current value. 

- Current: when it is selected, the interface displays the current preview and you can set all the groups to output a fixed voltage. Press **<mark>Voltage</mark>** and use the numeric keyboard or the left/right direction key and knob to set the voltage value. 

###### **2. Select the templet** 

Press **<mark>Type</mark>** to select the desired templet, including Sine, Pulse, Ramp, Stair Up, Stair Dn, StairUpDn, Exp Rise and Exp Fall. 

DP800 User’s Guide 

2-18 

Chapter 2 Front Panel Operations 

**RIGOL** 

######  **Sine** 

The Sine waveform is as shown in the figure below. The instrument determines the Sine amplitude according to the maximum and minimum currently set and determines the Sine period according to the total number of points (denoted by **P** ) and the time interval currently set, thus determining the Sine waveform. When creating parameters, the instrument draws **P** values from the preset Sine waveform according to the current time interval. 



<!-- Start of picture text -->
T<br><!-- End of picture text -->

######  **Pulse** 

The Pulse waveform is as shown in the figure below. The timer parameters created from Pulse waveform only contain two groups of data. 

The first group: the amplitude (voltage or current) is determined by the high level set; the time equals the pulse width ( **t** ) currently set. 

The second group: the amplitude (voltage or current) is determined by the low level set; the time equals the period currently set minus the pulse width currently set ( **T-t** ). 



<!-- Start of picture text -->
t<br>T<br><!-- End of picture text -->

###### **Tip** 

When Pulse is selected, if you want to output more than 2 groups of timer parameters, you can output the timer parameters created from the Pulse <u>templet repeatedly by increasing the number of cycles.</u> 

DP800 User’s Guide 

2-19 

Chapter 2 Front Panel Operations 

**RIGOL** 

######  **Ramp** 

The Ramp waveform is as shown in the figure below. The instrument determines the amplitude of the Ramp according to the maximum and minimum currently set, determines the period according to the total number of points (denoted by **P** ) and the time interval currently set and determines the Ramp waveform according to the symmetry (denoted by **Sym** ) currently set. When creating parameters, the instrument draws **int(P*Sym)**<sup>**[1]**</sup> values from the rising edge of the preset Ramp waveform at the same time interval and draws **P-int(P*Sym)** values from the falling edge of the preset Ramp waveform at the same time interval. The timing time is determined by the time interval currently set. 



<!-- Start of picture text -->
t Symmetry=t/T*100%<br>T<br><!-- End of picture text -->

###### **Note**<sup>**[1]**</sup> **: int(P*Sym)** refers to rounding **P*Sym** (discard the decimal part). 

######  **Stair Up** 

The Stair Up waveform is as shown in the figure below. The instrument determines the Stair Up waveform according to the maximum (denoted by **MAX** ), minimum (denoted by **MIN** ), total number of points (denoted by **P** , **P** ≥10) and time interval currently set and creates **P** parameters from **MIN** to **MAX** at the step of **(MAX-MIN)/(P-1)** . The timing time is determined by the time interval currently set. 

Stair Up 

######  **Stair Dn** 

The Stair Dn waveform is as shown in the figure below. The instrument determines the Stair Dn waveform according to the maximum (denoted by **MAX** ), minimum (denoted by **MIN** ), total number of points (denoted by **P** ) and time interval currently set and creates **P** parameters from **MIN** to **MAX** at the step of **(MAX-MIN)/(P-1)** . The timing time is determined by the time interval currently set. 

Stair Dn 

DP800 User’s Guide 

2-20 

Pan - 



<!-- Start of picture text -->
-<br><!-- End of picture text -->





<!-- Start of picture text -->
8 a = — 8<br>an oo iN ——Fallindex=1<br>7 a — 7 \\ —— Falllndex=4<br>& {/L focoa 6t \\\ih\ \ ————Fallindex=7Falllndex=10<br>f/ f WX<br>if // \\VA\<br>a ff 4 iV \<br>st jj / 3 \\ \<br>ififi off \\WN\ \ .N\<br>ol Hi|! // —— Riselndex=1 2 LON \ ~eNN<br>U if/ —— Riselndex=4 NN<br>tf ——  Riselndex=7 1 Mw ae<br>tif Riselndex=10i ae~, iaee<br>a a ——— _ —<br>o 0 2 9 4 68 © 7 &© © 100 o 0 2 2 4 8 © 7 © BD 100<br><!-- End of picture text -->

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **3. Edit the timer parameters** 

For different templets, the timer parameters to be set are different as shown in Table 2-1. 

<u>Table 2-1 Timer parameters (templet)</u> 

|<br>**Templet**|<br>**Parameter**||
|---|---|---|
|Sine|Max Value, Min Value, Points,|Interval, Inverted|
|Pulse|Hi Level, Lo Level, Width, Per|iod, Inverted|
|Ramp|Max Value, Min Value, Points,|Interval, Symmetry, Inverted|
|Stair Up|Max Value, Min Value, Points,|Interval|
|Stair Dn|Max Value, Min Value, Points,|Interval|
|StairUpDn|Max Value, Min Value, Points,|Interval|
|Exp Rise|Max Value, Min Value, Points,|Interval, Rise Index|
|Exp Fall|Max Value, Min Value, Points,|Interval, Fall Index|



######  Max Value 

Set the maximum voltage or current of the templet currently selected. The range is related to the channel currently selected. It should be greater than or equal to the minimum value currently set. 

######  Min Value 

Set the minimum voltage or current of the templet currently selected. The range is related to the channel currently selected. It should be lower than or equal to the maximum value currently set. 

######  Points 

Set the total number of points (namely the number of output groups in the timer parameters) of the templet currently selected. The range is from 10 to 2048. 

When the total number of points (denoted by **P** ) and the current number of output groups (denoted by **G** ) are different, the number of output groups will change to **P** automatically after output waveform is created using the templet. 

######  Interval 

Set the time interval between the points (the time required for the instrument to output each group of timer parameters) of the templet currently selected. The range is from 1s to 99999s. 

######  Inverted 

When the templet currently selected is Sine, Pulse or Ramp, if invert is enabled, the instrument will first turn the preset waveform upside down and then create the output waveform. 

DP800 User’s Guide 

2-23 

Chapter 2 Front Panel Operations 

**RIGOL** 

######  Hi Level 

When the templet currently selected is Pulse, set the high level of the Pulse and the range is related to the channel currently selected. It should be greater than or equal to the low level currently set. 

######  Lo Level 

When the templet currently selected is Pulse, set the low level of the Pulse and the range is related to the channel currently selected. The low level should be lower than or equal to the high level currently set. 

######  Width 

When the templet currently selected is Pulse, set the positive pulse width of the Pulse (namely the duration of high level within a period). The range is from 1s to 99998s. 

###### **Tip** 

The actual range of the positive pulse width is related to the period - <u>currently set. The actual range is from 1s to (Period 1s).</u> 

- Period 

When the templet currently selected is Pulse, set the period of the Pulse and the range is from 2s to 99999s. 

###### **Tip** 

The actual range of the period is related to the positive pulse width currently set. The actual range is from (Positive Pulse Width+1s) to <u>99999s.</u> 

- Symmetry 

When the templet currently selected is Ramp, set the symmetry of the Ramp (namely the ratio of the duration of the rising edge within a period to the whole period) and the range is from 0% to 100%. 

- Rise Index 

When the templet currently selected is Exp Rise, set the rise index of the Exp Rise and the range is from 0 to 10. 

######  Fall Index 

When the templet currently selected is Exp Fall, set the fall index of the Exp Fall and the range is from 0 to 10. 

###### **4. Construct the output waveform** 

After editing the timer parameters (templet), press **<mark>Construct</mark>** to construct the output waveform. The timer parameters corresponding to the output waveform constructed are displayed in the timer parameter list as shown in Figure 2-2. 

DP800 User’s Guide 

2-24 



### ~~<u>——</u>~~ 

~~<u>[</u>~~ 



Chapter 2 Front Panel Operations 

**RIGOL** 

###### **To Set the Number of Cycles** 

The number of cycles refers to the number of times that the instrument performs delay output according to the preset state. Press **<mark>Cycles</mark>** to set the number of cycles to "Infinite" or use the numeric keyboard or the left/right direction key and knob to input the desired value. The range is from 1 to 99999. 

###### **Tip** 

- The total number of groups in delay output = the number of output groups*the number of cycles 

- The power supply will terminate the delay function when the instrument detects the state that meets the condition set in " **To Set the Stop Condition** " or when the total number of groups of delay outputs is finished. At this point, the state of the power supply depends on the setting in " **To Set** " 

- **<u>the End State</u>** <u>.</u> 

###### **To Set the End State** 

The end state refers to the state of the instrument when the delay function is terminated. Press **<mark>End State</mark>** to select "Outp On", "Outp Off" or "Last". 

- Outp On: the instrument turns on the output automatically. 

- Outp Off: the instrument turns off the output automatically. 

- Last: the instrument stops at the output state of the last group. 

###### **To Edit the Delay Parameters** 

You can edit the delay parameters manually or generate the delay parameters automatically using the built-in pattern and built-in method. 

###### **To Edit the Delay Parameters Manually** 

You need to set **P** groups (No.0 to No.(P-1)) of delay parameters; wherein, **P** is the number of output groups currently set. Only 6 groups of parameters can be 





displayed on each page of the delayer parameter list and you can press or to view and set the parameters of the other groups. This interface provides delay parameter preview and users can view the values on the current page of the delay parameter list (high level indicates turning on the output and low level indicates turning off the output). 

1. Press **<mark>Parameter</mark>** and use the numeric keyboard or the left/right direction key or the knob to select the specified number (No.) in the delay parameter list. 

DP800 User’s Guide 

2-27 

Chapter 2 Front Panel Operations 

**RIGOL** 

2. Use the up/down direction key to select the state (State) and time (Delay) of the current group respectively. 

   - When state (State) is selected, press **<u>OK</u>** to switch to the desired state; when time (Delay) is selected, use the numeric keyboard or the left/right direction key and knob to input the desired value. 

3. Use the same method to set the parameters of the other groups. 

###### **To Generate the Delay Parameters Automatically** 

The instrument provides various kinds of built-in patterns and built-in time sequence generation algorithms used to automatically generate delay on/off state and time sequence respectively. Users can select the state generation mode and time sequence generation mode and the instrument will generate the delay parameters automatically according to the current settings. 

###### **1. Select the state generation mode** 

- Press **<mark>State Gen</mark>**  **<mark>0 1 Patt</mark>** ( **<mark>1 0Patt</mark>** <mark>)</mark> to select "0 1 Patt" ("1 0 Patt").  0 1 Patt: the state is set to "Off" and "On" alternately. 

- 1 0 Patt: the state is set to "On" and "Off" alternately. 

The instrument will generate the on/off state automatically according to the current setting and the length is the number of output groups currently set. 

###### **2. Select the time generation mode** 

- 1) Select the generation method 

Press **<mark>Time Gen</mark>**  **<mark>Method</mark>** to select "FixTime", "Increase" or "Decline". 

- FixTime 

Users set the output on delay time (on delay, namely the duration when the output status is "On") and the output off delay time (off delay, namely the duration when the output status is "Off"). 

   - Increase Generate the on/off delay time by increasing from the time base value at the specified step. Users can set the time base value and step. 

   - Decline Generate the on/off delay time by declining from the time base value at the specified step. Users can set the time base value and step. 

- 2) Set the on delay/off delay When the time generation method is "FixTime", users need to set the output on delay time and output off delay time. 

Press **<mark>Time Gen</mark>**  **<mark>On Delay</mark>** <mark>(</mark> **<mark>Off Delay</mark>** <mark>)</mark> ; use the numeric keyboard or the left/right direction key and knob to input the desired value. The ranges of on delay and off delay are both from 1s to 99999s. 

DP800 User’s Guide 

2-28 

Chapter 2 Front Panel Operations 

**RIGOL** 

- 3) Set the time base value and step 

   - When the time generation method is "Increase" or "Decline", users need to set the time base value and step. 

Press **<mark>Time Gen</mark>**  **<mark>Base Val</mark>** <mark>(</mark> **<mark>Step</mark>** <mark>)</mark> ; use the numeric keyboard or the left/right direction key and knob to input the desired value. The minimum time base value and step are both 1s and the two fulfills the relation: time base value + number of output groups*step ≤99999s. 

###### **To Set the Stop Condition** 

The instrument monitors the output voltage, current and power during delay output. You can set a stop condition and the instrument terminates the delay function (no matter whether the total number of groups of delay outputs are finished) when state that fulfills this condition is detected. At this point, the state of the instrument is determined by the setting in " **To Set the End State** ". 

Press **<mark>Stop Con</mark>** to set the stop condition to "None", "< Voltage", "> Voltage", "< Current", "> Current", "< Power" or "> Power". After selecting the desired condition, use the numeric keyboard or the left/right direction key and knob to input the desired value (you do not need to set this parameter when the stop condition is set to "None"). 

###### **To Save and Read the Delay File** 

You can save the delay parameters edited manually or generated automatically in internal or external memory and recall them when required. 

###### **1. Save** 

After finishing editing the delay parameters, press **<mark>Save</mark>** to enter the store and recall interface. The file type is fixed at "*.rdf" and please save the file according to the introduction in " **Save** ". 

###### **2. Read** 

Press **<mark>Read</mark>** to enter the store and recall interface. The file type is fixed at "*.rdf" and please read the desired file according to the introduction in " **Read** ". Users can edit the delay file read. 

DP800 User’s Guide 

2-29 



Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Advanced Functions** 

DP800 provides various advanced functions, including the recorder, analyzer (option<sup>**[1]**</sup> ), monitor (option) and trigger (option). Press to open the advanced function setting interface. 

**1. Recorder:** record the output state of each channel and store the record file. 

**2. Analyzer:** analyze the record file saved. 

**3. Monitor:** monitor the output of each channel and turn off the output, display the corresponding prompt message or sound the beeper when the monitor condition is met. 

**4. Trigger:** the rear panel provides a digital I/O interface which supports trigger input and trigger output. 

   - Trigger Input: the data lines of the digital I/O interface receive external trigger signal. The source under control (namely the output channel) turns on the output, turns off the output or inverts the output state when the preset trigger condition is met. 

   - Trigger Output: the data lines of the digital I/O interface output the level or square signal when the output of the control source (namely the output channel) meets the preset trigger condition. 

**Note**<sup>**[1]**</sup> **:** For function marked with "option", you need to order the corresponding option according to the order number provided in " **Appendix A: Accessories and Options** " in this manual and install the option correctly (refer to " **Options** ") before using the function. 

. 

DP800 User’s Guide 

2-31 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Recorder** 

When the recorder is enabled, users can record the current state of the instrument and if an analyzer is installed, users can also analyze the file recorded. 



Press  **Recorder** to open the recorder setting interface. You can turn on or off the recorder function, set the record period and select the store destination. 

**Note:** When the recorder is turned on, the record period and storage destination cannot be set. Therefore, please set the record period and storage destination before turning on the recorder. 

You can record and save the record file following the steps below. 

###### **1. Set the record period** 

The record period is the time interval at which the instrument samples and records the output of each channel when the recorder is turned on. Press **<mark>Period</mark>** and use the numeric keyboard or the left/right direction key and knob to set the desired value. The range is from 1s to 99999s and the default is 1s. 

###### **2. Set the storage destination** 

Press **<mark>Dest</mark>** to enter the store and recall interface and the file type is fixed at "*.rof". After selecting the desired destination, press **<mark>Save</mark>** and input the desired filename (for the detailed operations, refer to the introduction in " **Save** "); then, press **<mark>OK</mark>** . 

###### **Note:** 

- You can store the record file in the internal or external memory. In one record operation, the recorder can record up to 2048 points when the internal memory is selected or 614400 points when the external memory is selected. 

- When the recorder is turned off, the instrument will store the record file with the specified filename to the specified destination according to the settings in this step. 

###### **3. Turn on or off the recorder** 

- Press **<mark>Switch</mark>** to turn on the recorder. The instrument samples and records the output of each channel by using the current record period as the time interval. 

- Press **<mark>Switch</mark>** to turn off the recorder. When the recorder is turned off, the current record is finished; press **<mark>OK</mark>** and the instrument will store the recorded file to the specified destination according to the settings in step 2. 

- **Note:** 

- During the record, make sure that the output of each channel is enabled; otherwise, the record data will be 0. 

- You can only store the record file when at least a single point is recorded. 

DP800 User’s Guide 

2-32 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Analyzer (option)** 

The analyzer can analyze the file recorded and provides the analysis results of the channel voltage, current and power, such as the average, VAR and range. 



Press  **Analyzer** to open the analyzer setting interface. You can open the record file stored, set the analyzer parameters, execute analysis and view the analysis results. 

You can analyze the stored record file and view the analysis results following the steps below. 

###### **1. Open the record file** 

Press **<mark>Open File</mark>** to enter the store and recall interface and the file type is fixed at "*.rof". Select the desired record file and press **<mark>Read</mark>** <mark>.</mark> 

At this point, the current time and start time are both the **record period** of the file opened. When the **number of groups** of the file opened is less than or equal to 2048, the end time is the maximum record time (the **number of groups** *the **record period** of the record file opened) of the record file opened; when the **number of groups** of the file opened is greater than 2048, the end time is the **record period** of the file opened*2048. 

**Note:** The following operations are valid only when valid record file is opened. 

###### **2. Set the analysis parameters**  **Start Time** 

Press **<mark>Start Time</mark>** and use the numeric keyboard or the left/right direction key and knob to set the start time of the record file analysis. The range is from the **record period** of the file opened to the current **end time** . 

- **End Time** 

Press **<mark>End Time</mark>** and use the numeric keyboard or the left/right direction key and knob to set the end time of the record file analysis. When the **number of groups** of the file opened is less than or equal to 2048, the range is from the **start time** to the maximum record time (the **number of groups** *the **record period** ) of the file opened; when the **number of groups** of the file opened is greater than 2048, the range is from the **start time** to the **record period** of the file opened*2048. 

###### **3. Execute the analysis** 

Press **<mark>Analyze</mark>** and the instrument will start to analyze the record file opened according to the current setting. The analysis results include various statistic parameters of the channel voltage, current and power, such as the average, VAR and range. 

DP800 User’s Guide 

2-33 



<!-- Start of picture text -->
=<br><!-- End of picture text -->

# <u>=</u> ~~<u>—</u>~~ 



Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Monitor (option)** 

The monitor can monitor the current output state of the instrument. When the user-defined monitor condition is met, the instrument executes the corresponding operation according to the setting in " **Stop Mode** ". 



Press  **Monitor** to open the monitor setting interface. The status bar shows the channel currently selected. Press the channel selection keys at the front panel to switch the channel. 

**Note:** The instrument can monitor the output states of multiple channels simultaneously. 

You can set the monitor condition and turn on or off the monitor function following the steps below. 

###### **1. Monitor Condition** 

The monitor condition can be any logic combination of voltage, current and power and users can also set the voltage, current and power values. 

Press **<mark>Condition</mark>** and use the direction keys and **<u>OK</u>** to set the desired monitor condition. 

###### **2. Voltage** 

Press **<mark>Voltage</mark>** and use the numeric keyboard or the left/right direction key and knob to set the voltage in the monitor condition. 

###### **3. Current** 

Press **<mark>Current</mark>** and use the numeric keyboard or the left/right direction key and knob to set the current in the monitor condition. 

###### **4. Power** 

Press **<mark>Power</mark>** and use the numeric keyboard or the left/right direction key and knob to set the power in the monitor condition. 

###### **5. Stop Mode** 

When the channel output state meets the monitor condition set, the instrument will turn off the output, display the corresponding prompt message or sound the beeper according to the stop mode selected. The stop mode can be one or more of "Output Off", "Warning" and "Beeper". 

Press **<mark>Stop Mode</mark>** and use the direction keys to select the specified stop mode ("Output Off", "Warning" and "Beeper"); then, press **<u>OK</u>** to enable or disable the stop mode selected. 

###### **6. Switch** 

Press **<mark>Switch</mark>** to turn on or off the monitor function. 

DP800 User’s Guide 

2-36 



<!-- Start of picture text -->
| {<br>BK XG<br>= bigiatio<br>4371<br><!-- End of picture text -->



<!-- Start of picture text -->
(.-«)<br><!-- End of picture text -->









Chapter 2 Front Panel Operations 

**RIGOL** 

###### **3. Trigger Condition** 

- Press **<mark>Condition</mark>** to open the trigger condition setting interface and users can set the trigger condition of trigger output. 

- Outp Trig: the instrument triggers when the output of the control source is turned on or off. 

   - Press **<mark>Outp Trig</mark>** and use the up/down direction key to select "Output Off" or "Output On". 

- Volt Trig: the instrument triggers when the output voltage of the control source meets the voltage condition set. 

   - Press **<mark>Volt Trig</mark>** and use the up/down direction key to select ">", "<" or "="; use the numeric keyboard or the left/right direction key and knob to set the voltage value. 

- Curr Trig: the instrument triggers when the output current of the control source meets the current condition set. 

   - Press **<mark>Curr Trig</mark>** and use the up/down direction key to select ">", "<" or "="; use the numeric keyboard or the left/right direction key and knob to set the current value. 

- PowerTrig: the instrument triggers when the output power of the control source meets the power condition set. 

   - Press **<mark>PowerTrig</mark>** and use the up/down direction key to select ">", "<" or "="; use the numeric keyboard or the left/right direction key and knob to set the power value. 

- Auto Trig: when the trigger output is enabled, the instrument triggers automatically. 

###### **4. Output Signal** 

- Press **<mark>Signal</mark>** to set the signal type of the trigger output to "Level" or "Square".  Level: the output high level is from 2.6V to 3.5V and the output low level is from 0V to 0.4V. 

- Square: press **<mark>Period</mark>** to set the square period and the range is from 100μs to 2.5s; press **<mark>Duty</mark>** to set the duty cycle of the square waveform and the range is from 10% to 90%. 

###### **5. Polarity** 

- Press **<mark>Polarity</mark>** to set the polarity of the trigger output signal to "Positive" or "Negative". 

- Positive: output the signal selected in **<mark>Signal</mark>** when the trigger condition is met. 

- Negative: turn the signal selected in **<mark>Signal</mark>** upside down and then output the signal when the trigger condition is met. 

###### **6. Enable** 

Press **<mark>Enable</mark>** to enable or disable the trigger output function of the corresponding data line. 

DP800 User’s Guide 

2-40 

~~<u>[</u>~~ 





<!-- Start of picture text -->
[<br>[<br>[<br><!-- End of picture text -->

~~—_~~ 

~~—~~ 

how<sup>Me</sup> 





<!-- Start of picture text -->
IRIGOL wav oro<br>open Fie Doo] Provo [ea |<br><!-- End of picture text -->

Chapter 2 Front Panel Operations 

**RIGOL** 

**4. Save the settings** 

Press **<mark>Save</mark>**  **<mark>OK</mark>** to save the start-up interface settings. 

**5. Select the start-up interface** 

Press **<mark>Boot UI</mark>** to select the default start-up interface or the user-defined start-up interface. 

DP800 User’s Guide 

2-44 

~~[_]~~ 



Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Browser** 

Press **<u>Store</u>**  **<mark>Browser</mark>** to move the cursor to the directory or file area and then use the knob or the up/down direction key to select the desired directory or file. 

###### **File Type** 

Press **<u>Store</u>**  **<mark>Type</mark>** to select the desired file type (including state file, record file, timer file, delay file, bitmap file and all files). 

###### **1. State File** 

Store the current instrument state (such as the parameter setting and system state) in "*.RSF" format in internal or external memory. At most 10 instrument states can be stored in the internal memory. 

###### **2. Record File** 

When the recorder is turned on, the instrument records the current output state of each channel (such as the output voltage, current and power as well as sample interval) and store them in "*.ROF" format in internal or external memory when the recorder is turned off. At most 10 record files can be stored in the internal memory. 

###### **3. Timer File** 

Store the edited timer parameters (the voltage, current and time values) in "*.RTF" format in internal or external memory. At most 10 timer files can be stored in the internal memory. 

###### **4. Delay File** 

Store the edited delay parameters (the states and time values) in "*.RDF" format in internal or external memory. At most 10 delay files can be stored in the internal memory. 

###### **5. Bitmap File** 

You can browse the "*.BMP" format bitmap files under the current directory. As "*.BMP" format bitmap files cannot be stored in C disk, the right side of the store and recall interface is blank when the file type is set to "*.bmp" and the current directory is C disk. 

###### **6. All File** 

Display all the files and folders under the directory currently selected. When this menu is selected, the save operation is not available. 

DP800 User’s Guide 

2-46 

~~[| [|~~ ~~<u>[|</u> L_|~~ 

~~<u>L|</u>~~ 



~~<u>U</u>~~ 7 



<!-- Start of picture text -->
FN<br><!-- End of picture text -->



<!-- Start of picture text -->
Ld<br><!-- End of picture text -->



Chapter 2 Front Panel Operations 

**RIGOL** 

4. After inputting the filename in the filename input interface, press **<mark>OK</mark>** and the instrument stores the file with the specified filename in the specified file type under the storage directory currently selected. If the storage location selected already contains a valid file, the instrument will prompt whether to overwrite the original file and press **<mark>OK</mark>** to finish the save operation. 

###### **Read** 

You can read the specified type of file following the steps below. 

1. Press **<u>Store</u>**  **<mark>Type</mark>** to select the desired file type. 

   - **Note:** The read operation is available only when the file type is "*.rsf" or "All 

File". The timer file and delay file can be read separately by pressing **<u>Timer</u>**  **<mark>Timer Set</mark>**  **<mark>Read</mark>** and **<u>Timer</u>**  **<mark>Delay Set</mark>**  **<mark>Read</mark>** and the record file can be read by pressing  **<mark>Analyzer</mark>**  **<mark>Open File</mark>** <mark>.</mark> 

2. Press **<mark>Browser</mark>** to switch the cursor to the directory area and select the directory of the file to be read; press **<mark>Browser</mark>** again to switch to the file area and select the file to be read. 

3. Press **<mark>Read</mark>** and the instrument read the file currently selected. If the current location does not contain a valid file, the corresponding prompt message will be displayed. For a state file, the instrument enters the main interface after the file is successfully read; for other types of files, the instrument enters the corresponding function interface after the file is successfully read. 

###### **Delete** 

You can delete the files under C disk and D disk as well as the empty folders under D disk. 

1. Press **<u>Store</u>**  **<mark>Browser</mark>** to move the cursor to the directory area and select the directory of the file to be deleted; press **<mark>Browser</mark>** again to switch to the file area and select the file or empty folder to be deleted. 

2. Press **<mark>Delete</mark>** and the prompt message asking whether to delete the file or empty folder currently selected is displayed. Press **<mark>OK</mark>** to finish the delete operation. 

DP800 User’s Guide 

2-49 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Copy and Paste** 

You can only copy the files in C disk and paste the files to D disk. 

1. Press **<u>Store</u>**  **<mark>Browser</mark>** to move the cursor to the directory area and select C disk; press **<mark>Browser</mark>** again to switch to the file area and select the file to be copied. 

2. Press **<mark>Copy</mark>** and the instrument copies the file currently selected. 

3. Press **<mark>Browser</mark>** to switch to the directory area and select D disk. 

4. Press **<mark>Paste</mark>** , the instrument pastes the file copied to the D disk and the corresponding prompt message is displayed when the file is successfully pasted. 

DP800 User’s Guide 

2-50 

~~<u>[</u>~~ 



~~<u>[</u>~~ 



<!-- Start of picture text -->
[<br><!-- End of picture text -->



<!-- Start of picture text -->
|<br><!-- End of picture text -->



Chapter 2 Front Panel Operations 

**RIGOL** 

- **DHCP** : in this mode, the DHCP server in the current network assigns network parameters (such as the IP address) for the instrument. Press **<mark>DHCP</mark>** to enable or disable the DHCP configuration mode. By default, the DHCP configuration mode is "On". 

- **Auto IP:** in this mode, the instrument acquires the IP address from 169.254.0.1 to 169.254.255.254 and subnet mask 255.255.0.0 according to the current network configuration automatically. Press **<mark>Auto IP</mark>** to enable or disable the auto IP configuration mode. By default, the auto IP configuration mode is "On". 

**Note:** When the DHCP and auto IP configuration modes are enabled at the same time, the instrument uses the DHCP configuration mode. Therefore, to use the auto IP configuration mode, **<mark>DHCP</mark>** should be set to "Off". 

- **Manual IP:** in this mode, users define the network parameters (such as the IP address). Press **<mark>Manual IP</mark>** to enable or disable the manual IP configuration mode. By default, the manual IP configuration mode is "Off". 

**Note:** When all the three configuration modes are set to "On", the priority order of parameter configuration is "DHCP", "Auto IP" and "Manual IP". Therefore, to use the manual IP configuration mode, **<mark>DHCP</mark>** and **<mark>Auto IP</mark>** should be set to "Off". 

###### **Tip** 

- When all the three configuration modes are set to "On", the priority order of parameter configuration is "DHCP", "Auto IP" and "Manual IP". 

- The three IP configuration modes cannot all be set to "Off" at the same <u>time.</u> 

###### **3. MAC Address** 

The MAC (Media Access Control) address is also called hardware address and is used to define the location of the network device. For a power supply, the MAC address is unique and is usually used to recognize the instrument when assigning IP address for the instrument. The MAC address (48 bits, namely 6 bytes) is usually expressed in hexadecimal form, for example, 00-2A-A0-AA-E0-56. 

###### **4. VISA Descriptor** 

VISA (Virtual Instrument Software Architecture) is an advanced application programming interface developed by NI (National Instrument) and is used to communicate with various instrument buses. It uses the same method to communicate with the instrument regardless of the type of the instrument interface (GPIB, USB, LAN/Ethernet or RS232). The instrument communicating with it via the GPIB, USB, LAN/Ethernet or RS232 interface is called "Resource". 

DP800 User’s Guide 

2-53 

Chapter 2 Front Panel Operations 

**RIGOL** 

VISA descriptor is the resource name and describes the accurate name and location of the VISA resource. If the LAN interface is currently used to communicate with the instrument, the VISA descriptor is TCPIP::172.16.3.100::INSTR. 

###### **5. Set the IP Address** 

Please acquire a valid IP address from your network administrator firstly and set the IP address manually in manual IP configuration mode. The format of the IP address is nnn.nnn.nnn.nnn; wherein, the first nnn ranges from 1 to 223 (except 127) and the other three range from 0 to 255. 

Press **<mark>IP Addr</mark>** and use the numeric keyboard and left/right direction key or the knob and left/right direction key to input the desired IP address. This setting is stored in the non-volatile memory and the instrument will load the IP address set automatically at the next power-on if **<mark>DHCP</mark>** and **<mark>Auto IP</mark>** are set to "Off". 

###### **6. Set the Subnet Mask** 

Please acquire a valid subnet mask from your network administrator firstly and set the subnet mask manually in manual IP configuration mode. The format of the subnet mask is nnn.nnn.nnn.nnn; wherein, the range of nnn is from 0 to 255. 

Press **<mark>Sub Mask</mark>** and use the numeric keyboard and left/right direction key or the knob and left/right direction key to input the desired subnet mask. This setting is stored in the non-volatile memory and the instrument will load the subnet mask set automatically at the next power-on if **<mark>DHCP</mark>** and **<mark>Auto IP</mark>** are set to "Off". 

###### **7. Set the Gateway** 

Please acquire a valid gateway address from your network administrator firstly and set the gateway manually in manual IP configuration mode. The format of the gateway is nnn.nnn.nnn.nnn; wherein, the first nnn ranges from 1 to 223 (except 127) and the other three range from 0 to 255. 

Press **<mark>Gateway</mark>** and use the numeric keyboard and left/right direction key or the knob and left/right direction key to input the desired gateway address. This setting is stored in the non-volatile memory and the instrument will load the gateway address set automatically at the next power-on if **<mark>DHCP</mark>** and **<mark>Auto IP</mark>** are set to "Off". 

###### **8. Set the DNS Server** 

Please acquire a valid DNS server address from your network administrator firstly and set the DNS (Domain Name Service) address manually in manual IP configuration mode. The format of the DNS address is nnn.nnn.nnn.nnn; wherein, the first nnn ranges from 1 to 223 (except 127) and the other three range from 0 to 255. 

DP800 User’s Guide 

2-54 

Chapter 2 Front Panel Operations 

**RIGOL** 

Press **<mark>DNS Serv</mark>** and use the numeric keyboard and left/right direction key or the knob and left/right direction key to input the desired address. This setting is stored in the non-volatile memory and the instrument will load the server address set automatically at the next power-on if **<mark>DHCP</mark>** and **<mark>Auto IP</mark>** are set to "Off". 

###### **9. Default** 

Press **<mark>Def Cfg</mark>** and the prompt message "Restore LAN to default?" will be displayed; press **<mark>OK</mark>** to restore the network parameters to default values. At this point, the DHCP and auto IP configuration modes are enabled and the manual IP configuration mode is disabled. 

###### **10. Current Setting** 

Press **<mark>Cur Cfg</mark>** to view the MAC address, current network parameters and network status information of the instrument. 

###### **11. Apply the Setting** 

Press **<mark>Apply</mark>** to apply the network parameters currently set. 

**Note:** After setting the network parameters, you need to press **<mark>Apply</mark>** to apply the network parameters currently set; at this point, the new settings take effect. 

DP800 User’s Guide 

2-55 

## ~~<u><mark>a7</mark></u>~~ 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **To Set the GPIB Address** 

Before using the GPIB interface, use the USB to GPIB interface converter (option) to extend a GPIB interface and then use a GPIB cable to connect the instrument and PC. 

Press **<u>Utility</u>**  **<mark>I/O Config</mark>**  **<mark>GPIB</mark>** and use the numeric keyboard or the left/right direction key and knob to set the desired GPIB address. It can be any integer from 0 to 30. The default is 2. This setting is stored in the non-volatile memory and will not be affected by factory reset. 

###### **System Setting** 

###### **Power-on Setting** 

Select the instrument configuration ("Default" or "Last") the instrument uses at power-on and the default is "Default". 

- Last: use the system configuration before the last power-off. 

- Default: use the factory setting (except those parameters that are not affected by factory reset). For the details, refer to 

- Table 2-2. 

Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Power On</mark>** to select the desired configuration type. This setting is stored in the non-volatile memory and will not be affected by factory reset. 

###### **Over-temperature Protection** 

Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>OTP</mark>** to enable or disable the over-temperature protection (OTP) function. When the OTP function is enabled, the instrument turns off the output automatically when the temperature inside the instrument reaches the limit. 

###### **Beeper** 

Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Beeper</mark>** to enable or disable the beeper. When the beeper is enabled, the instrument generates prompt sound during front panel operation or when error occurs during remote operation. 

DP800 User’s Guide 

2-57 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Screen Saver** 

Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Scrn Svr</mark>** to enable or disable the screen saver function. When the screen saver function is enabled, the instrument will enter the screen saver mode automatically after standing by for 25 minutes and will enter the black screen state after another 12.5 minutes. 

###### **Keyboard Lock** 

DP800 allows users to lock the front panel keys to avoid the danger caused by misoperation. You can press and hold **<u>OK</u>** to lock the front panel keys; at this point, 

all the front panel keys (except the output on/off key of each channel and the 



power switch key ) are not available. When the keyboard lock password is disabled, you can press and hold this key again to unlock the front panel keys. When the keyboard lock password is enabled, you need to input the correct password (2012) to unlock the front panel keys. 

Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Key Lock</mark>** to enable or disable the keyboard lock password. 

**Tip:** You can also use the " **Key Locking** " function to lock or unlock the specified keys at the front panel. 

###### **Track Setting** 

The specified channels of DP832 and DP831 support the track function. You can select the desired tracking mode and on/off sync mode (for the details, refer to " **To Enable the Track Function** " and " **To Track the On/Off Status of the Channel Output** ") according to your need. 

1. Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>Track</mark>** to select "Synchronous" or "Independent". 

   - **Synchronous:** for two channels (the channels should be of the same instrument) that support the track function, the track function of the other channel will be enabled or disabled at the same time when the track function of a channel is enabled or disabled. 

   - **Independent:** for two channels (the channels should be of the same instrument) that support the track function, the status of the track function of the other channel will not be affected when the track function of a channel is enabled or disabled. 

DP800 User’s Guide 

2-58 

Chapter 2 Front Panel Operations 

**RIGOL** 

2. Press **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Track Set</mark>**  **<mark>On/Off</mark>** to select "Enable" or "Disable". 

   - **Disable:** do not track the output status of the channel. 

   - **Enable:** track the output status of the channel. 

###### **System Information** 

Press **<u>Utility</u>**  **<mark>Sys Info</mark>** to view the system information of the instrument, including the model, serial number and digital board version. 

###### **System Language** 

DP800 supports menus in various languages and provides Chinese/English help information, prompt messages and interface display. Press **<u>Utility</u>**  **<mark>Language</mark>** to select the desired language. 

###### **Test/Calibration** 

You can view the calibration information of the instrument and calibrate the instrument. 

1. Press **<u>Utility</u>**  **<mark>Test/Cal</mark>** to view the calibration state, calibration time and test information (include the top board, bottom board, fan and temperature) of the instrument. 

2. Press **<u>Utility</u>**  **<mark>Test/Cal</mark>**  **<mark>ManualCal</mark>** and input the correct password to enter the manual calibration interface. At this point, you can perform calibration on the instrument. 

A default password is assigned to the instrument when it left factory. To acquire the password, please contact **RIGOL** . If you already know the password, you can modify the password (press **<u>Utility</u>**  **<mark>Test/Cal</mark>**  **<mark>Password</mark>** and modify the password according to the interface prompts). 

###### **Note** 

The recommended calibration interval is 1 year. The instrument is calibrated before leaving factory. Calibration by users themselves is not recommended and if <u>calibration is required, please contact</u> **RIGOL** . 

DP800 User’s Guide 

2-59 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Print** 

You can store the screen content as a picture file in the USB storage device. **Note:** DP800 only supports the flash memory USB storage device of FAT32 format. 

1. Insert a USB storage device into the USB HOST interface at the rear panel of the instrument. When the USB storage device is detected, is displayed in the status bar on the screen and the corresponding prompt message is displayed. 

2. Press **<u>Utility</u>**  **<mark>Print Set</mark>**  **<mark>Print</mark>** and the countdown label (5s) is displayed at the left of the status bar. Switch the instrument to the desired interface within 5s and the instrument stores the screen content in "*.BMP" format in the USB storage device 5s later after you press **<mark>Print</mark>** . After that, the corresponding prompt message is displayed. 

When no USB storage device is currently detected, press **<mark>Print</mark>** and the prompt message "USB storage device does not exist." will be displayed. 

DP800 User’s Guide 

2-60 



<!-- Start of picture text -->
[_]<br>[<br>[<br><!-- End of picture text -->



<!-- Start of picture text -->
[<br>[<br><!-- End of picture text -->



Chapter 2 Front Panel Operations 

**RIGOL** 

###### <u>Table 2-2 Factory setting</u> 

|**Channel Parameters**||
|---|---|
|**DP832**||
|Voltage/Current Setting<br>Values|CH1: 00.00V/3.000A<br>CH2: 00.00V/3.000A<br>CH3: 0.00V/3.000A|
|Voltage/Current Limits|CH1: 33.00V/3.300A<br>CH2: 33.00V/3.300A<br>CH3: 5.50V/3.300A|
|OVP/OCP On/Off|CH1/CH2/CH3: Off/Off|
|Output On/Off|CH1/CH2/CH3: Off|
|Track On/Off|CH1/CH2: Off<br>CH3: None|
|Current Channel|CH1|
|**DP831**||
|Voltage/Current Setting<br>Values|CH1: 0.000V/5.000A<br>CH2: 00.00V/2.000A<br>CH3:-00.00V/2.000A|
|Voltage/Current Limits|CH1: 8.800V/5.500A<br>CH2: 33.00V/2.200A<br>CH3:-33.00V/2.200A|
|OVP/OCP On/Off|CH1/CH2/CH3: Off/Off|
|Output On/Off|CH1/CH2/CH3: Off|
|Track On/Off|CH1: None<br>CH2/CH3: Off|
|Current Channel|CH1|
|**DP821**||
|Voltage/Current Setting<br>Values|CH1: 00.00V/1.000A<br>CH2: 0.00V/10.00A|
|Voltage/Current Limits|CH1: 66.00V/1.100A<br>CH2: 8.80V/11.00A|
|OVP/OCP On/Off|CH1/CH2: Off/Off|
|Output On/Off|CH1/CH2: Off|
|Sense Function On/Off|CH1: None<br>CH2: Off|
|Current Channel|CH1|
|**DP811**||
|Voltage/Current Setting<br>Values|00.00V/05.00A|
|Voltage/Current Limits|22.00V/11.00A|
|OVP/OCP On/Off|Off/Off|
|Output On/Off|Off|
|Sense Function On/Off|Off|
|Current Range|Range1|



DP800 User’s Guide 

2-62 

Chapter 2 Front Panel Operations 

**RIGOL** 

|**Display**||
|---|---|
|Brightness*|50%|
|Contrast*|25%|
|RGB Luminance*|50%|
|Display Mode*|Normal|
|Theme*|Green|
|**System Setting**||
|Language*|Chinese(s)|
|Power-on Setting*|Default|
|OTP|On|
|Beeper|On|
|<br>Screen Saver|Off|
|Keyboard Lock*|Off|
|Track Mode|Synchronous|
|On/Off Sync|Disable|
|Preset|Default|
|Print Destination|USB Disk|
|Print Copies|1|
|Print Format|BMP|
|Print Invert|Yes|
|Print Color|GrayScale|
|**I/O Configuration***||
|<br>GPIB Address|2|
|**RS232**||
|Baud Rate|9600|
|Data Bit|8|
|Stop Bit|1|
|Parity Bit|None|
|Hardware Flow Control|Off|
|**LAN**||
|DHCP|On|
|Auto IP|On|
|Manual IP|Off|
|**Timer**||
|Channel|CH1|
|Timer On/Off|Off|
|Output Groups|1|
|Timer Parameters|Volt: 1V; Curr: 1A; Time: 1s|
|Cycles|1|
|<br>End State|Outp Off|
|Templet|<br>Sine|
|**Sine**||
|Object|Voltage|
|<br>Current|<br>0A|
|Maximum|1V|
|Minimum|0V|
|Total Number of Points|10|
|Interval|1s|
|Invert|Off|



DP800 User’s Guide 

2-63 

Chapter 2 Front Panel Operations 

**RIGOL** 

|**Delayer**||
|---|---|
|Channel|CH1|
|Delayer On/off|Off|
|Output Groups<br>|1<br>|
|Delay Parameter|State: Off, On alternately|
|Cycles|1|
|End State<br>|Outp Off<br>|
|State Generation|0 1Patt|
|Time Generation|FixTime|
|Fixed Time|On Delay: 1s; Off Delay: 1s<br>|
|Increase/Decline|Time Base: 1s; Step: 1s|
|Stop Condition|None|
|**Recorder**||
|Recorder Switch|Off|
|Record Period|1s|
|Destination|C:\REC 10:RIGOL.ROF|
|**Analyzer**||
|Channel|CH1|
|Analysis Object|Voltage|
|Display<br>|Curve<br>|
|Current Time|1s|
|Start Time|1s|
|End Time<br>|2s|
|Number of Groups|0|
|Median|0V|
|Mode|0V|
|Average|0V|
|Variance|0V|
|Range|0V|
|Maximum|0V|
|Minimum|0V|
|Mean Deviation|0V|
|**Monitor**||
|Channel|CH1|
|Monitor Switch|Off|
|Monitor Condition|>Volt|
|Voltage|Half of the rated value of CH1 (for DP811, it is half of the rated<br>value of Range1)|
|Current|Half of the rated value of CH1 (for DP811, it is half of the rated<br>value of Range1)|
|Power|The product of the two values above|
|Stop Mode|Output Off, Warning, Beeper|
|**Trigger**||
|Trig|In|
|**Trigger Input**||
|<br>Data Line<br>|D0<br>|
|Source under Control|CH1|
|Trigger Type|Rise Edge|



DP800 User’s Guide 

2-64 

Chapter 2 Front Panel Operations 

**RIGOL** 

|Output Response|Output Off|
|---|---|
|Sensitivity|Low|
|Enable|No|
|**Trigger Output**||
|Data Line|D0|
|Control Source|CH1|
|Trigger Condition|Output Off|
|Output Signal|Level|
|Square|Period: 1s; Duty Cycle: 50%|
|Polarity|Positive|
|Enable|No|
|**Store**||
|Browser|Directory|
|Directory|C:\|
|File|The first file|
|File Type|*.rsf|



**Note*:** These parameters are not affected by factory reset (when "Default" is selected in **<u>Utility</u>**  **<mark>System</mark>**  **<mark>Power On</mark>** , restarting the power supply will restored it to the factory setting). 

DP800 User’s Guide 

2-65 

~~<u>[</u>~~ 





Chapter 2 Front Panel Operations 

**RIGOL** 

6. Press **<mark>OTP</mark>** repeatedly to enable or disable the over-temperature function of the instrument. 

7. Press **<mark>OK</mark>** to save the current user-defined setting. 

You can also press **<mark>Default</mark>** to restore the parameters of the user-defined setting to the default. 

DP800 User’s Guide 

2-67 



<!-- Start of picture text -->
/ pei P832 High-<br>File Edit Format View Help<br>pP8&32001L001L01--——_—_——-<br>UVF 2LIN3XXEK YTB? SPPR544XDM5R<——————<br>| It<br><!-- End of picture text -->

Chapter 2 Front Panel Operations 

**RIGOL** 

The first line: instrument serial number (press **<u>Utility</u>**  **<mark>Sys Info</mark>** to acquire the serial number of the instrument); The second line: the option license; The third line: a blank line (cannot be omitted). 

**Note:** The hyphens are not included in the option license. 

###### **The option installation procedures:** 

- Edit the option installation file according to the above requirements and store it into a USB storage device. 

- Turn on the instrument and insert the USB storage device. Press **<u>Store</u>** to enter the store and recall interface. 

- Read the option installation file following the steps below in the store and recall interface (refer to " **Store and Recall** "). Select "D:"  set the file type to "All File"  select the installation file mentioned above  press **<mark>Read</mark>** . 

###### **3) Send SCPI command** 

- Open the remote control window and send the option installation command (:LIC:SET <license>) by referring to " **Remote Control** ". Wherein, <license> is the option license (note that the hyphens should be omitted). 

For example, :LIC:SET UVF2L3N3XXKYTB73PPRSA4XDMSRT. 

If the option is successfully installed, the prompt message informing you that the option installation succeeds will be displayed; otherwise, the corresponding error message will be displayed. 

DP800 User’s Guide 

2-69 

Chapter 2 Front Panel Operations 

**RIGOL** 

###### **Key Locking** 

To avoid the adverse effect caused by misoperation, you can lock the specified key or all the keys (except the power switch key) at the front panel using SCPI command. 

###### **1. Key locking command introduction** 

:SYSTem:KLOCk <key>,{ON|OFF|0|1}         /*Lock or unlock the specified key*/ :SYSTem:KLOCk? <key>                     /*Query whether the specified key is locked*/ 

Wherein, <key> is used to specify the key and the range is as follows. 

|DISPLAY|STORAGE|UTILITY|HELP|TIMER|<br>ADVANCE|PRESET||/*Function keys*/|
|---|---|
|CH1|CH2|CH3||/*Channel selection keys*/|
|RANGE1|RANGE2|<sup>**[1]**</sup>|/*Range selection keys*/|
|M1|M2|M3|M4|M5||/*Menu keys*/|
|NUM0|NUM1|NUM2|NUM3|NUM4|NUM5|<br>NUM6|NUM7|NUM8|NUM9|DOT||/*Numeric keys*/|
|LEFT|RIGHT|UP|DOWN||/*Direction keys*/|
|OUTPUT1|OUTPUT2|OUTPUT3|OUTPUTALL||/*Output on/off keys*/|
|KNOB|OK|BACK|DIAL||/*Knob, OK, Back and dial<br>keys*/|
|ALL|/*All the keys (except the<br>power switch key) and<br>knob at the front panel*/|



**Note**<sup>**[1]**</sup> **:** The parameters are for the single-channel model. 

{ON|OFF|0|1} is used to lock or unlock the keys. Selecting ON|1 denotes locking the specified key; selecting OFF|0 denotes unlocking the specified key. 

###### **2. The three methods to lock or unlock the keys** 

- **(1) Edit a command file that meets the following requirements and store it to a USB storage device; read this file via the instrument to lock or unlock the specified key** 

DP800 User’s Guide 

2-70 



<!-- Start of picture text -->
Tock NUMo.sc<br>File Edit Format View Help<br>|?<br>| :S¥STem:KLOCkK NUMO,1 =. —<br><!-- End of picture text -->

Chapter 3 Remote Control 

**RIGOL** 

##### **Chapter 3 Remote Control** 

DP800 series power supply can be controlled remotely via the following two modes. 

###### **User-defined programming** 

Users can program and control the instrument by using the SCPI (Standard Commands for Programmable Instruments) commands. For more information about the commands and programming, refer to the DP800 Programming Guide. 

###### **Use PC software** 

Users can use the PC software to send commands to control the instrument remotely. Ultra Sigma of **RIGOL** is recommended. You can download Ultra Sigma from **RIGOL** official website (www.rigol.com). 

This power supply can communicate with PC through USB, LAN, RS232 and GPIB (with the USB to GPIB interface converter provided by **RIGOL** ) instrument buses. This chapter will give a detailed introduction of how to use **Ultra Sigma** to control the power supply remotely through various interfaces. 

The contents of this chapter are as follows: 

- Remote Control via USB 

- Remote Control via LAN (option) 

- Remote Control via GPIB (option) 

- Remote Control via RS232 (option) 

DP800 User’s Guide 

3-1 



<!-- Start of picture text -->
Found New Hardware Wizard<br>Ss This wizard helps you install software for:<br>USB Test and Measurement Device (IVI)<br>(+) If your hardware came with an installation CD<br><3 or floppy disk, insert it now.<br>What do you want the wizard to do?<br>O Install the software qutomatically (Recommended)<br>©) Install from a list or specific location [Advanced<br>Click Next to continue.<br>(=<br><!-- End of picture text -->



<!-- Start of picture text -->
Found New Hardware Wizard<br>Please choose your search and installation options.<br>O Search for the best driver in these locations.<br>Use the check boxes below to limit or expand the default search, which includes local<br>paths and removable media. The best driver found will be installed.<br>*) Don't search. | will choose the driver to install.<br>Choose this option to select the device driver from a list. Windows does not guarantee that<br>the driver you choose will be the best match for your hardware.<br>ten)<br><!-- End of picture text -->



<!-- Start of picture text -->
Found New Hardware Wizard<br>Select the device driver you want to install for this hardware. a<br>Sy<br>hea haveSelect thea disk manufacturerthat contains andthe modeldriver of you yourwant hardwareto install, deviceclick andHave thenDisk. click Next. If you<br>Show compatible hardware<br>Mode<br><SNISE Test and Measurement Device (IVI) i<br>& This driver is not digitally signed!<br>Tell me why driver signing is important<br>=<br><!-- End of picture text -->



<!-- Start of picture text -->
Found New Hardware Wizard<br>Please wait while the wid installs the software... Pata<br>Sy,<br>z USB Test and Measurement Device (IVI)<br><!-- End of picture text -->



<!-- Start of picture text -->
Hardware Update Wizard<br>Ny WizardCompleting the Hardware Update<br>The wizard has finished installing the software for:<br>as USB Test and Measurement Device (IVI)<br>Click Finish to close the wizard.<br><!-- End of picture text -->

USB-TMC 



<!-- Start of picture text -->
LAN GPIB USB-TMC RS232 VerifyAl “™®& C9<br>a: RIGOL Online Resource ry<br>| Be Dpsoo<br>he @P DPSS2 (USBO: 0x1 ABI: :Ox0E11: :DPS3200100101 : : INSTR)<br>Ee IVI Class DriverControl<br>im Other Online Resource<br>“> Search USBIMC instrument. A<br><!-- End of picture text -->

Chapter 3 Remote Control 

**RIGOL** 

###### **Remote Control via LAN (option)** 

###### **1. Connect the device** 

Connect the power supply to your PC or the LAN of your PC using a network cable. 

###### **2. Configure network parameters** 

- 1) DHCP mode: 

If the network supports DHCP, the DHCP server in the network assigns network parameters (IP Address, Subnet Mask, Gateway and DNS) for the power supply automatically. 

- 2) Auto IP mode: 

When the network doesn’t support DHCP, the DHCP mode of the power supply is disabled or the power supply is connected to the PC directly, the power supply selects Auto IP mode and acquires the IP address from 169.254.0.1 to 169.254.255.254 and subnet mask 255.255.0.0 automatically. 

- 3) Manual IP mode: 

   - Enable the Manual IP mode and disable the DHCP mode as well as the Auto IP mode to set the network parameters manually. 

If the power supply is connected to the PC directly, set the IP Addresses, Subnet Masks and Gateways for both of the PC and the power supply. The Subnet Masks and Gateways of the power supply and PC must be the same and the IP Addresses of them must be within the same network segment (for details, refer to the TCP/IP protocal). An example is shown in the table <u>below.</u> 

|**Network Parameters**|**PC**|**Power Supply**|
|---|---|---|
|IP Address|192.16.3.3|192.16.3.8|
|Subnet Mask|255.255.255.0|255.255.255.0|
|Default Gateway|192.16.3.1|192.16.3.1|



If your power supply is connected to the LAN of your PC, please acquire a set of valid network parameters (IP Address, Subnet Mask, Gateway and DNS) from your network administrator and configure the network parameters of the power supply according to the description in " **To Set the LAN Parameters** ". 

DP800 User’s Guide 

3-5 









<!-- Start of picture text -->
Create LAH Instrument Eesource<br>| Remove ) OK<br>Manual Input LAN Instrument IF<br>| TEST<br>| Add<br>Autocdetect of LAN Instrument<br><!-- End of picture text -->



<!-- Start of picture text -->
Create LAH Instrument Eesource<br>Manual ) Remove | OK4<br>Input LAW Instrument IP TCPIP: 172. 16.3. 100: :THSTR<br>| TEST<br>| Add<br>Autocdeteet of LAH Instrument<br>| Search<br><!-- End of picture text -->





<!-- Start of picture text -->
Seu TRA =<br>LAN GPIB USB-TMC RS232 Verify All ~<br>fe; RIGOL Online Resource =<br>i DPS32 (ICPIP::172. 16.3. 100: : INSTR)<br>a IVI Class DriverControl<br>tee Other Online Resource<br>“> Search LAN instrument. cA<br><!-- End of picture text -->









<!-- Start of picture text -->
E5232 & GPIB Setting<br>| GPIEQ: : vr +] O: -INSTR ‘F = GPIBO: :0: :- INSTR<br>WB typer channel | ——4h<br>| TEST<br><!-- End of picture text -->



<!-- Start of picture text -->
E5232 & GPIB Setting<br>RS232 Setting | GPIB Setting | Remove | sa<br>GPIBO: 718: : INSTR<br>| GPIEQ: : vr +] O: -INSTR ‘F = GPIBO: :0: :- INSTR<br>WB typer channel | Search<br>| TEST<br><!-- End of picture text -->







<!-- Start of picture text -->
LAN GPIB USB-TMC R5232 Verify All ~, ]<br>Ei; RIGOL Online Resource ry<br>h4 DPS32 (GPIBO: :18: : INSTR)<br>i IVI Class DriverControl<br>tem Other Online Resource<br>“> Search GPIB or RSZ32 instrument. A<br><!-- End of picture text -->









<!-- Start of picture text -->
| ok<br><!-- End of picture text -->







<!-- Start of picture text -->
R5232 & GPIB Setting<br>R5232 Setting GFIB Setting | Renae | | OK<br>Serial Resource: [=<br>| ASRLI: : INSTR ll<br>Baud Rate: Parity:<br>—<br>| 9600 ~ | sHone vr Mikead 2 times<br>lata Bits: Stop Bit:<br>| 8 ~ | 1 vr J TEST<br>Flow Control: End Mark:<br>| Hone - | iin al [e<br><!-- End of picture text -->



<!-- Start of picture text -->
Bessage<br>Test successful, mow you can add for<br>modify) the instrument resources.<br>| OF<br><!-- End of picture text -->



<!-- Start of picture text -->
RS5232 & GPIB Setting<br>E5232 Setting GPIB Setting | Remove | OK<br>Serial Resource: ASRLI: -IHSTR ey<br>J ¥<br>Baud Rate: Farity:<br>—<br>J 9600 vr | dHone vr Mi kead 2 times<br>Data Bits: Stop Bit:<br>J 5 vr | 1 a | TEST<br>Flow Control: End Mark:<br>J Fone al | \whn ¥ eal<br><!-- End of picture text -->



<!-- Start of picture text -->
LAN GPIB USB-TMC RS232 Verify All ~«, ]<br>Re; EIGOL Online Resource |<br>i Ee: Drsoo<br>Ei IVI Class DriverControl<br>fee Other Online Resource<br><> Search GPIB or RS232 instrument. A<br><!-- End of picture text -->

Chapter 4 Troubleshooting 

**RIGOL** 

##### **Chapter 4 Troubleshooting** 

The following failures might occur when using this instrument. Please solve the problem according to the following method; if the problem remains, please contact **RIGOL** and provide the device information of your instrument ( **<u>Utility</u>**  **<mark>Sys Info</mark>** ). 

###### **1. The instrument does not start.** 

- (1) Check the power cord connection. 

- (2) Check whether the power switch at the front panel is turned on. 

- (3) Remove the power cord and check whether the voltage selector is at the proper scale and whether the fuse specification is correct and the fuse is in good condition. To replace the fuse, refer to " **To Replace the Fuse** ". 

- (4) If the problem remains, please contact **RIGOL** . 

###### **2. The constant voltage output is abnormal.** 

- (1) Check whether the maximum output power of the scale selected fulfills the load requirement. If yes, turn to the next step. 

- (2) Check whether the cable connecting the load and power supply is short-circuited and whether it is in good contact. 

- (3) Check whether the load works normally. 

- (4) Check whether the current setting value of this scale is proper; if it is too low, increase the current setting value properly. 

- (5) If the problem remains, please contact **RIGOL** . 

###### **3. The constant current output is abnormal.** 

- (1) Check whether the maximum output power of the scale selected meets the requirement of the load. If yes, turn to the next step. 

- (2) Check whether the cable connecting the load and power supply is short-circuited and whether it is in good contact. 

- (3) Check whether the load works normally. 

- (4) Check whether the voltage setting value of this scale is proper; if it is too low, increase the voltage setting value properly. 

- (5) If the problem remains, please contact **RIGOL** . 

###### **4. The USB storage device cannot be recognized.** 

- (1) Check whether the USB storage device can work normally. 

- (2) Make sure that a flash memory USB storage device of FAT32 format is used, as this instrument does not support hard disk USB storage device. 

- (3) Restart the instrument and insert the USB storage device to check it. 

- (4) If the USB storage device still cannot work normally, please contact **RIGOL** . 

DP800 User’s Guide 

4-1 

Chapter 5 Specifications 

**RIGOL** 

##### **Chapter 5 Specifications** 

All the specifications are guaranteed when the instrument has been working for more than 30 minutes under the specified operation temperature. 

**Note:** Unless otherwise noted, the specifications are applicable to all the channels of the specified model. 

###### **<mark>DC Output (0</mark>** <mark>℃</mark> **<mark>to 40</mark>** <mark>℃</mark> **<u><mark>)</mark></u>** 

|Channel(|Range)|Voltage/Current|OVP/OCP|
|---|---|---|---|
|**DP832**|CH1<br>CH2<br>CH3|0 to 30V/0 to 3A<br>0 to 30V/0 to 3A<br>0 to 5V/0 to 3A|10mV to 33V/1mA to 3.3A<br>10mV to 33V/1mA to 3.3A<br>10mV to 5.5V/1mA to 3.3A|
|**DP831**|CH1<br>CH2<br>CH3|0 to 8V/0 to 5A<br>0 to 30V/0 to 2A<br>0 to -30V/0 to 2A|10mV to 8.8V/1mA to 5.5A<br>10mV to 33V/1mA to 2.2A<br>-10mV to -33V/1mA to 2.2A|
|**DP821**|CH1<br>CH2|0 to 60V/0 to 1A<br>0 to 8V/0 to 10A|10mV to 66V/10mA to 1.1A<br>10mV to 8.8V/10mA to 11A|
|**DP811**|Range1<br>Range2|0 to 20V/0 to 10A<br>0 to 40V/0 to 5A|10mV to 22V/10mA to 11A<br>10mV to 44V/10mA to 5.5A|



|**Load Regulation Rate ±(Output Percentage+Offset)**|
|---|
|Voltage<br><0.01%+2mV|
|Current<br><0.01%+250μA|
|**Linear Regulation Rate ±(Output Percentage+Offset)**|
|Voltage<br><0.01%+2mV|
|Current<br><0.01%+250μA|



|**Ripples and Noise(2**|**0Hz to 20MHz)**|
|---|---|
|Normal Mode Voltage|DP831/DP832/DP821: <350μVrms/2mVpp<br>DP811: <350μVrms/3mVpp|
|Normal Mode Current|<2mArms|



|**Annual A**<br>Channel|**ccuracy**<sup>**[1]**</sup>|**(25**℃**±5**℃**)±(Ou**<br>Progra<br>|**tput Percentag**<br>mming<br>|**e+Offset)**<br>Rea<br>|dback<br>|
|---|---|---|---|---|---|
|||Voltage|Current|Voltage|Current|
|**DP832**|CH1<br>CH2<br>CH3|0.05%+20mV<br>0.05%+20mV<br>0.1%+5mV|0.2%+5mA<br>0.2%+5mA<br>0.2%+5mA|0.05%+10mV<br>0.05%+10mV<br>0.1%+5mV|0.15%+5mA<br>0.15%+5mA<br>0.15%+5mA|
|**DP831**|CH1<br>CH2<br>CH3|0.1%+5mV<br>0.05%+20mV<br>0.05%+20mV|0.2%+10mA<br>0.2%+5mA<br>0.2%+5mA|0.1%+5mV<br>0.05%+10mV<br>0.05%+10mV|0.2%+10mA<br>0.1%+5mA<br>0.1%+5mA|



DP800 User’s Guide 

5-1 

Chapter 5 Specifications 

###### **RIGOL** 

|**DP821**|CH1<br>CH2|0.1%+25<br>0.05%+10|mV<br>mV<br>0.2<br>0.2|%+10mA<br>%+10mA|0.1%+25m<br>0.05%+5m|V<br>V<br>0.15<br>0.15|%+10mA<br>%+10mA|
|---|---|---|---|---|---|---|---|
|**DP811**|CH1|0.05%+10|mV<br>0.1|%+10mA|0.05%+10|mV<br>0.1|%+10mA|
|**Resolutio**<br>Channel|**n**|Progra|mming|Rea|dback|Di|splay|
|||Voltage|Current|Voltage|Current|Voltage|Current|
|||Standard||||||
|||10mV<br>10mV<br>|1mA<br>1mA<br>|10mV<br>10mV<br>|1mA<br>1mA<br>|10mV<br>10mV<br>|10mA<br>10mA<br>|
||CH1<br>|10mV|1mA|10mV|1mA|10mV|10mA|
|**DP832**|CH2<br>CH3|With the h|igh-resolut|ion option||||
|||1mV<br>1mV<br>|1mA<br>1mA<br>|0.1mV<br>0.1mV<br>|0.1mA<br>0.1mA<br>|1mV<br>1mV<br>|1mA<br>1mA<br>|
|||1mV|1mA|0.1mV|0.1mA|1mV|1mA|
|||Standard||||||
||CH1|1mV<br>10mV<br>|1mA<br>1mA<br>|1mV<br>1mV<br>|1mA<br>1mA<br>|10mV<br>10mV<br>|10mA<br>10mA<br>|
|||10mV|1mA|1mV|1mA|10mV|10mA|
|**DP831**|CH2<br>CH3|With the h|igh-resolut|ion option||||
|||1mV<br>1mV<br>|0.3mA<br>0.1mA<br>|0.1mV<br>0.1mV<br>|0.1mA<br>0.1mA<br>|1mV<br>1mV<br>|1mA<br>1mA<br>|
|||1mV|0.1mA|0.1mV|0.1mA|1mV|1mA|
|||Standard||||||
|||10mV|1mA|10mV|1mA|10mV|1mA|
||CH1|10mV|10mA|10mV|10mA|10mV|10mA|
|**DP821**|<br>CH2|With the h|igh-resolut|ion option||||
|||1mV<br>1mV|0.1mA<br>1mA|1mV<br>1mV|0.1mA<br>1mA|1mV<br>1mV|0.1mA<br>1mA|
|||<br>Standard||||||
|||10mV|10mA|1mV|1mA|10mV|10mA|
|**DP811**|CH1|With the h|igh-resolut|ion option||||
|||1mV|0.5mA|0.1mV|0.1mA|1mV|1mA|



###### **<mark>Transient Response Time</mark>** 

Less than 50μs for output voltage to recover to within 15mV following a change in output current <u>from full load to half load or vice versa.</u> 

###### **<mark>Command Processing Time</mark>**<sup>**[2]**</sup> 

<118ms 

DP800 User’s Guide 

5-2 

Chapter 5 Specifications 

**RIGOL** 

###### **<mark>OVP/OCP</mark>** 

Accuracy ±(Output Percentage+Offset) 0.5%+0.5V/0.5%+0.5A 

|**Voltage P**<br>Channel|**rogramm**|**ing Control Spe **<br>|**ed(1% within t**<br>Rise<br>|**he total variatio**<br>|**n range)**<br>Fall<br>|
|---|---|---|---|---|---|
|||Full Load|No Load|Full Load|No Load|
|**DP832**|CH1<br>CH2<br>CH3|<50ms<br><50ms<br><15ms|<33ms<br><38ms<br><14ms|<46ms<br><46ms<br><24ms|<400ms<br><400ms<br><100ms|
|**DP831**|CH1<br>CH2<br>CH3|<18ms<br><33ms<br><35ms|<17ms<br><36ms<br><42ms|<20ms<br><44ms<br><45ms|<200ms<br><400ms<br><400ms|
|**DP821**|CH1<br>CH2|<110ms<br><15ms|<30ms<br><15ms|<110ms<br><20ms|<800ms<br><400ms|
|**DP811**|CH1|<45ms|<42ms|<51ms|<1089ms|



|**Tempera**<br>|**ture Coef**|**ficientper**℃ **(Output Per**<br>|**centage+Offset)**<br>|
|---|---|---|---|
|Channel||Voltage|Current|
|**DP832**|CH1<br>CH2<br>CH3|0.01%+5mV<br>0.01%+5mV<br>0.01%+2mV|0.01%+2mA<br>0.01%+2mA<br>0.01%+2mA|
|**DP831**|CH1<br>CH2<br>CH3|0.01%+2mV<br>0.01%+2mV<br>0.01%+2mV|0.02%+3mA<br>0.02%+3mA<br>0.02%+3mA|
|**DP821**|CH1<br>CH2|0.01%+3mV<br>0.01%+3mV|0.02%+3mA<br>0.02%+3mA|
|**DP811**|CH1|0.01%+3mV|0.02%+3mA|



|**Stability**<sup>**[**</sup><br>|<sup>**3]**</sup>**±(Outp**|**ut Percentage+Offset)**<br>||
|---|---|---|---|
|Channel||Voltage|Current|
|**DP832**|CH1<br>CH2<br>CH3|0.02%+2mV<br>0.02%+2mV<br>0.01%+1mV|0.05%+2mA<br>0.05%+2mA<br>0.05%+2mA|
|**DP831**|CH1<br>CH2<br>CH3|0.03%+1mV<br>0.02%+2mV<br>0.02%+2mV|0.1%+3mA<br>0.05%+1mA<br>0.05%+1mA|
|**DP821**|CH1<br>CH2|0.02%+1mV<br>0.02%+1mV|0.1%+1mA<br>0.1%+1mA|
|**DP811**|CH1|0.02%+1mV|0.1%+1mA|



DP800 User’s Guide 

5-3 

Chapter 5 Specifications 

**RIGOL** 

|**Mechanical**||
|---|---|
|Dimensions|239mm(W)x 157mm(H)x 418mm(D)|
|Weight|DP832: 10.5kg<br>DP831: 9.75kg<br>DP821: 10.0kg<br>DP811: 10.3kg|
|**Power**||
|AC Input<br>(50Hz-60Hz)|100Vac±10%, 115Vac±10%, 230Vac±10% (maximum 250Vac)|
|Maximum Power|DP832: 521VA<br>DP831: 416VA<br>DP821: 450VA<br>DP811: 503VA|
|**I/O**||
|USB Device|1|
|USB Host|1|
|LAN|1(Option)|
|RS232|1(Option)|
|Digital IO|1(Option)|
|USB-GPIB|1 (Option, extend a GPIB interface using the USB-GPIB interface<br>converter)|
|Rear Output Interface|1(onlyfor DP811)|
|**Environment**||
|CoolingMethod|Fan Cooling|
|WorkingTemperature|0℃to 40℃|
|Storage Temperature|-40℃ to 70℃|
|Humidity|5% to 80% relative humidity|
|Altitude|Below 1500m|



**Note**<sup>**[1]**</sup> **:** The accuracy param eters are acquired via calibration under 25℃ after 1 -hour warm-up. **Note**<sup>**[2]**</sup> **:** The maximum time required for the output to change accordingly after receiving the APPLy and SOURce commands. **Note**<sup>**[3]**</sup> **:** The variation of the output within 8 hours after 30-minute warm-up when the load circuit and environment temperature are constant. 

DP800 User’s Guide 

5-4 

Chapter 6 Appendix 

**RIGOL** 

##### **Chapter 6 Appendix** 

###### **Appendix A: Accessories and Options** 

||**Description**|**Order NO.**|
|---|---|---|
||Programmable Linear DC Power Supply<br>(Three-channel)|DP832|
|**Mdl**|Programmable Linear DC Power Supply<br>(Three-channel)|DP831|
|**oes**|Programmable Linear DC Power Supply<br>(Dual-channel)|DP821|
||Programmable Linear DC Power Supply<br>(Single-channel)|DP811|
||Power Cord|--|
||USB Cable|CB-USBA-USBB-FF-150|
|**Standard**<br>**Ai**|Fuse 50T-032H 250V 3.15A (DP832/DP811)<br>Fuse 50T-025H 250V 2.5A (DP831/DP821)|--|
|**ccessores**|Short-circuit Equipment (DP821/DP811)|--|
||Quick Guide (Hard Copy)|--|
||Digital I/O Interface Connecting Terminal|Terminal-Digital I/O-DP800|
||Provide high-resolution setting|HIRES-DP800|
||Provide 4 trigger input and output channels|DIGITALIO-DP800|
||Provide on-line monitor and analysis functions|AFK-DP800|
||Provide RS232 and LAN communication<br>interfaces|INTERFACE-DP800|
|**Optional**|USB to GPIB Interface Converter|USB-GPIB|
|**Accessories**|DP800 Series Rack Mount Kit (Single<br>Instrument)|RM-1-DP800|
||DP800 Series Rack Mount Kit (Two Instruments)|RM-2-DP800|
||DP800 Series Red Safety Plug|SPR-DP800|
||DP800 Series Black Safety Plug|SPB-DP800|
||DP800 Series Green Safety Plug|SPG-DP800|



**Note:** For all the accessories and options, please contact the local office of **RIGOL** . 

DP800 User’s Guide 

6-1 

Chapter 6 Appendix 

**RIGOL** 

###### **Appendix B: Warranty** 

**RIGOL** warrants that the product mainframe and product accessories will be free from defects in materials and workmanship within the warranty period. 

If a product proves defective within the respective period, **RIGOL** guarantees free replacement or repair of any defective products within a reasonable period of time. To get repair service, please contact with your nearest **RIGOL** sales or service office. 

There is no other warranty, expressed or implied, except such as is expressly set forth herein or other applicable warranty card. There is no implied warranty of merchantability or fitness for a particular purpose. Under no circumstances shall **RIGOL** be liable for any consequential, indirect, ensuing or special damages for any breach of warranty in any case. 

DP800 User’s Guide 

6-2 

Index 

**RIGOL** 

##### **Index** 

|Advanced Functions .................. 2-31|
|---|
|All File ..................................... 2-46|
|Analysis object ......................... 2-34|
|analysis parameters .................. 2-33|
|analysis results ......................... 2-34|
|Auto IP .................................... 2-53|
|Back .......................................... 1-7|
|Bitmap File ............................... 2-46|
|Brightness ................................ 2-41|
|Built-in help system ................... 1-18<br>|
|CC ............................................. 2-6|
|Channel (Range) Selection ........... 1-5|
|Channel output terminals ............. 1-8|
|Check the output mode ............... 2-4|
|Connect the channel output terminals<br>|
|................................................. 2-2|
|Constant current output .............. 2-6|
|Constant voltage output .............. 2-2|
|Construct the output waveform .. 2-24|
|Contrast ................................... 2-41|
|Control Source .......................... 2-39|
|Copy and Paste a file ................ 2-50|
|Current Time ............................ 2-35|
|CV ............................................. 2-2|
|Data Line ................................. 2-38|
|Delay File ................................. 2-46|
|Delay Parameters ...................... 2-27|
|Delayer .................................... 2-15|
|Delayer End State ..................... 2-27|
|Delayer Stop Condition .............. 2-29|
|Delete a file ............................. 2-49|
|DHCP....................................... 2-53|
|display mode ............................ 2-42|
|Display Mode Switch ................. 1-10|
|<br>Display type ............................. 2-34|
|<br>DNS Server .............................. 2-54|
|Enable ............................. 2-39, 2-40|
|FixTime.................................... 2-28|
|Function Menu Area .................... 1-9|
|Fuse ........................................ 1-14|
|Gateway .................................. 2-54|
|GPIB Address ........................... 2-57|
|Ground terminal ......................... 1-8|



|Hardware Flow Control............... 2-56|
|---|
|Input Power Requirements ......... 1-13|
|IP Address ................................ 2-54|
|IP Configuration Mode ............... 2-52|
|Key Locking .............................. 2-70|
|Keyboard Lock .......................... 2-58|
|<br>LAN Parameters ........................ 2-52|
|LAN Status ................................ 2-52|
|MAC Address ............................ 2-53|
|Manual IP ................................. 2-53|
|Menu Keys ................................ 1-10|
|Monitor Condition ...................... 2-36|
|Monitor Stop Mode .................... 2-36|
|Number of Cycles of Delayer ...... 2-27|
|Number of Cycles of Timer ......... 2-17|
|Number of Output Groups of Delayer|
|............................................... 2-26|
|Number of Output Groups of Timer<br>|
|............................................... 2-17|
|OK ............................................ 1-7|
|on delay/off delay ..................... 2-28|
|On/Off Sync .............................. 2-12|
|option installation file ................ 2-68|
|option license ............................ 2-68|
|Output Interface ........................ 2-4|
|Output Response ....................... 2-38|
|Output Signal ............................ 2-40|
|Output Terminals ........................ 1-8|
|Parameter Input Area ................. 1-6|
|Power Supply Parallel Connection . 2-9|
|Power Supply Series Connection .. 2-8|
|Power Switch Key ...................... 1-10|
|Preset ....................................... 1-7|
|Read a file ................................ 2-49|
|Record File ............................... 2-46|
|Record period ........................... 2-32|
|<br>RGB luminance.......................... 2-41|
|RS232 Parameters ..................... 2-56|
|Save a File ................................ 2-47|
|Sense connecting method .......... 2-13|
|Sense Mode .............................. 2-13|
|Sense terminals .......................... 1-8|
|Set the current ........................... 2-3|



DP800 User’s Guide 

1 

**RIGOL** 

|Set the overcurrent protection ..... 2-3|
|---|
|Set the voltage ........................... 2-2|
|Source under Control ................ 2-38|
|State File ................................. 2-46|
|state generation ....................... 2-28|
|Store and Recall ....................... 2-45|
|Subnet Mask ............................ 2-54|
|templet.................................... 2-18|
|templet editing object ............... 2-18|
|time base value and step ........... 2-29|
|time generation ........................ 2-28|
|Timer ...................................... 2-15|
|Timer End State ....................... 2-17|
|Timer File ................................ 2-46|
|Timer Parameters ..................... 2-17|



|Index|
|---|
|Track Function .......................... 2-10|
|Tracking Mode .......................... 2-10|
|Trigger Condition....................... 2-40|
|trigger connections .................... 2-37|
|Trigger Input ............................ 2-37|
|Trigger Output .......................... 2-37|
|Trigger Type ............................. 2-38|
|Turn on the output ...................... 2-3|
|User Interface ........................... 1-17|
|User-defined Setting .................. 2-66|
|User-defined Start-up Interface .. 2-43|
|Utility ....................................... 2-51|
|VISA Descriptor ........................ 2-53|
|voltage selector ........................ 1-13|



DP800 User’s Guide 

2 


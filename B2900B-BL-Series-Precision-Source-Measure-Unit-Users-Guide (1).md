## **B2900B/BL Series Precision Source/Measure Unit** 

USER’S GUIDE 

##### Notices 

###### Copyright Notice 

© Keysight Technologies 2020, 2023 

No part of this manual may be reproduced in any form or by any means (including electronic storage and retrieval or translation into a foreign language) without prior agreement and written consent from Keysight Technologies as governed by United States and international copyright laws. 

###### Manual Part Number 

B2910-90110 

###### Edition 

Edition 2, May 2023 

###### Published by: 

Keysight Technologies Japan K.K. 9-1, Takakura-machi, Hachioji-shi, Tokyo 192-8550 Japan 

###### Technology Licenses 

The hardware and/or software described in this document are furnished under a license and may be used or copied only in accordance with the terms of such license. 

###### U.S. Government Rights 

The Software is “commercial computer software,” as defined by Federal Acquisition Regulation (“FAR”) 2.101. Pursuant to FAR 12.212 and 27.405-3 and Department of Defense FAR Supplement (“DFARS”) 227.7202, the U.S. government acquires commercial computer software under the same terms by which the software is customarily provided to the public. Accordingly, Keysight provides the Software to U.S. government customers under its standard commercial license, which is embodied in its End User License Agreement (EULA), a copy of which can be found at http:// www.keysight.com/find/sweula. The license set forth in the EULA represents the exclusive authority by which the U.S. government may use, modify, distribute, or disclose the Software. The EULA and the license set forth therein, does not require or permit, among 

other things, that Keysight: (1) Furnish technical information related to commercial computer software or commercial computer software documentation that is not customarily provided to the public; or (2) Relinquish to, or otherwise provide, the  government rights in excess of these rights customarily provided to the public to use, modify, reproduce, release, perform, display, or disclose commercial computer software or commercial computer software documentation. No additional government requirements beyond those set forth in the EULA shall apply, except to the extent that those terms, rights, or licenses are explicitly required from all providers of commercial computer software pursuant to the FAR and the DFARS and are set forth specifically in writing elsewhere in the EULA. Keysight shall be under no obligation to update, revise or otherwise modify the Software.  With respect to any technical data as defined by FAR 2.101, pursuant to FAR 12.211 and 27.404.2 and DFARS 227.7102, the U.S. government acquires no greater than Limited Rights as defined in FAR 27.401 or DFAR 227.7103-5 (c), as applicable in any technical data. 

###### Warranty 

THE MATERIAL CONTAINED IN THIS DOCUMENT IS PROVIDED “AS IS,” AND IS SUBJECT TO BEING CHANGED, WITHOUT NOTICE, IN FUTURE EDITIONS. FURTHER, TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, KEYSIGHT DISCLAIMS ALL WARRANTIES, EITHER EXPRESS OR IMPLIED, WITH REGARD TO THIS MANUAL AND ANY INFORMATION CONTAINED HEREIN, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. KEYSIGHT SHALL NOT BE LIABLE FOR ERRORS OR FOR INCIDENTAL OR CONSEQUENTIAL DAMAGES IN CONNECTION WITH THE FURNISHING, USE, OR PERFORMANCE OF THIS DOCUMENT OR OF ANY INFORMATION CONTAINED HEREIN. SHOULD KEYSIGHT AND THE USER HAVE A SEPARATE WRITTEN AGREEMENT WITH WARRANTY TERMS COVERING THE MATERIAL IN THIS DOCUMENT THAT CONFLICT WITH THESE 

TERMS, THE WARRANTY TERMS IN THE SEPARATE AGREEMENT SHALL CONTROL. 

###### Open Software License 

A portion of the software in this product is licensed under terms of the General Public License Version 2 (“GPLv2”). The text of the license and source code can be found at: www.keysight.com/find/GPLV2 

###### Declaration of Conformity 

Declarations of Conformity for this product and for other Keysight products may be downloaded from the Web. Go to http://www.keysight.com/go/ conformity. You can then search by product number to find the latest Declaration of Conformity. 

###### Latest Information 

To get the latest firmware/software/electronic manuals/specifications/support information, go to www.keysight.com and type in the product number in the Search field at the top of the page. 

<u>MS AOHU =</u> O| JIVE AES HSM ASS SHOe YESS He 71712M LBS BAUM MBSE SF Mato] Pe WLC. MSA CUBS “UPS ISS AIA OU HSstct. 





<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 

###### • _DANGEROUS PROCEDURE WARNINGS_ 

Warnings, such as WARNING on the previous page, shall be complied. Procedures throughout in this manual prevent you from potentially hazard. Their instructions contained in the warnings must be followed. 

- _BEFORE APPLYING POWER_ 

Verify that all safety precautions are taken. Make all connections to the instrument before applying power. Note the instrument's external markings described under “Safety Symbols”. 

- _GROUND THE INSTRUMENT_ 

This is Safety Class I instrument. To minimize shock hazard, the instrument chassis and cabinet must be connected to an electrical ground. The power terminal and the power cable must meet International Electrotechnical Commission (IEC) safety standards. 

- _DO NOT OPERATE IN AN EXPLOSIVE ATMOSPHERE_ 

Do not operate the instrument in the presence of flammable gases or fumes. Operation of any electrical instrument in such an environment constitutes a definite safety hazard. 

- _DO NOT REMOVE COVERS_ 

No operator serviceable parts inside. Refer servicing to qualified personnel. To prevent electrical shock do not remove covers. 

- _IN CASE OF DAMAGE_ 

Instruments that appear damaged or defective should be made inoperative and secured against unintended operation until they can be repaired by qualified service personnel. Return the instrument to a Keysight Technologies sales or service office for services and repair to ensure that safety features are maintained. 

- _USE ONLY THE SPECIFIC ACCESSORIES_ 

Specific accessories satisfy the requirements for specific characteristics for using the instrument. Use the specific accessories, cables, adapters, and so on for safety reasons. 

###### Safety Symbols 

The general definitions of safety symbols used on equipment or in manuals are listed below. 

Direct current. 

- Alternating current. 

Earth ground terminal. 

Protective conductor terminal. For protection against electrical shock in case of a fault. Used with field wiring terminals to indicate the terminal which must be connected to ground before operating equipment. 

Frame or chassis terminal. A connection to the frame (chassis) of the equipment which normally includes all exposed metal structures. 

- Grounded terminal which indicates the earth potential. 

- On supply. 

Off supply. 

Standby supply. The equipment will be marked with this symbol is not completely disconnected from AC mains when power switch is in the standby position. 

In position of a bi-stable push switch. 

Out position of a bi-stable push switch. 

Hazardous voltage and potential for electrical shock. Do not touch terminals that have this symbol when the equipment is on. 



Hot surface. Avoid contact. Surfaces are hot and may cause personal injury if touched. 

- Caution, refer to accompanying documentation. The equipment will be marked with this symbol 

- <u>�</u> when it is necessary for the user to refer to the instruction manual. 

Read operator's manual. To indicate that the operator's manual or card should be read before continuing the operation. 

###### CAT I 



IEC Measurement Category I 

The CE mark shows that the product complies with all applicable European Directives. 

The CSA mark is a registered trademark of the Canadian Standards Association. 



The RCM mark is a registered trademark of the Australian Communications Authority. This signifies compliance with the Australian EMC Framework Regulations under the terms of the Radio communications Act. 

This ISM device complies with Canadian ICES-001. 



Cet appareil ISM est conforme à la norme NMB-001 du Canada. 

This ISM device complies with Canadian ICES-001 Class A. Cet appareil ISM est conforme à la norme NMB-001 classe A du Canada. 



This is the symbol for an Industrial, Scientific and Medical, Group 1 Class A product. (CISPR 11) 



The UKCA mark shows that the product complies with all applicable UK regulations. 

Korea’s safety and EMC mark 

China RoHS -  Environmentally Green Product Label 

China RoHS -  Product with Toxic Substance 40 yr EPUP 





The Chinese mark for paper-based packaging materials; Paperboard and Corrugated Fiberboard 

Plastic Material Coding Identification 



A WARNING notice denotes a hazard. It calls attention to an operating procedure, practice, or the like that, if not correctly performed or adhered to, could result in personal injury or death. Do not proceed beyond a WARNING notice until the indicated conditions are fully understood and met. 



A CAUTION notice denotes a hazard. It calls attention to an operating procedure, practice, or the like that, if not correctly performed or adhered to, could result in damage to the product or loss of important data. Do not proceed beyond a CAUTION notice until the indicated conditions are fully understood and met. 

###### Power Supply and Measurement Safety 

- Power Supply Safety 

This instrument can output high currents and voltages. Make sure that the load or device under test can safely handle the output current and voltage. Also, make sure that the connection leads can safely withstand the expected currents and are insulated for the expected voltages. 

The instrument outputs may be connected so as to float relative to earth ground. Isolation or floating voltage ratings are indicated on the instrument, near the output terminal or the Chassis ground terminal. There is the danger of electric shock by touching the floated measurement terminals. Keep in mind it to protect yourself. And it is a reason of using the recommended accessories. 

- Voltage/Current Measurement Safety 

Multimeters and other instruments capable of measuring high voltages and currents are subject to specific safety concerns because of the circuits to which they may be connected. To safely use these instruments, you need to understand the markings on the instrument near the input terminals, which include the Protection Limits and the IEC Measurement Category. 

- Protection Limits 

Keysight multimeters and other voltage measurement instruments provide protection circuitry to prevent damage to the instrument and to protect against the danger of electric shock, provided the Protection Limits are not exceeded. To ensure safe operation of the instrument, do not exceed the Protection Limits shown on the input terminals. 

- Source/Measure Terminals 

Source/measure unit, SMU, can simultaneously perform DC voltage or current output and measurement. Typical SMU has the Force, Sense, and Guard terminals as shown below. Normally the Force, Sense, and Guard are the same potential. Voltage marked around the terminals indicates the Protection Limits. 

Force and Sense must be connected to a terminal of a device under test (DUT) for the 4-wire connection (Kelvin connection) which is effective for high current measurement and low resistance measurement. For the 2-wire connection to ease the connections, connect Force only. Do not connect Sense. It must be opened. 

Guard should be connected to the guard shield which covers the DUT high side wiring for reducing leakage current caused by the wire. Or else, the Guard must be opened. 

Chassis ground (green terminal) should be connected to the ground shield which covers the DUT including the guard shield to minimize the affect of noise. Or else, the chassis ground should be opened. 

The following image is the source and measurement terminals of Keysight B2900. For the B2900, the High Force, High Sense, and Guard are the same potential. And the Low Force and Low Sense are the same potential. 



<!-- Start of picture text -->
High Sense, for 4-wire connection<br>High Force<br>Guard<br>Chassis ground<br>Low Force<br>Low Sense, for 4-wire connection<br><!-- End of picture text -->



###### <u>�</u> 

###### High Voltage Shock Hazard 

Keysight B2900 can force dangerous voltages (±210 V) at the High Force, Guard, and High Sense terminals. To prevent electric shock hazard, the following safety precautions must be observed during the use of Keysight B2900. 

- Use a three-conductor AC power cable to appliance coupler (inlet) and the instrument to an electric ground (safety ground). 

- Prepare shielding box which covers interface to a device under test and equipped with interlock circuit that opens when the door is opened. 

- Before performing measurement, connect the interlock circuit to the Interlock terminal of this instrument. 

- Confirm periodically that the interlock function works normally. 

- Before touching the connections of the High Force, Guard, and High Sense terminals, turn the instrument off and discharge any capacitors of the measurement path. If you do not turn the instrument off, complete “all” of the following items, regardless of any instrument’s settings. 

   - Terminate source output by pressing the On/Off switch, confirm that the On/Off switch turns off. 

   - Confirm that the HV (high voltage) status indicator is not lit. 

   - Open the shielding box access door (open the Interlock terminal). 

   - Discharge any capacitors if the capacitance is connected to this instrument. 

- Warn workers in the vicinity of the instrument about hazardous conditions. 

###### <u>�</u> 

###### Gefahr durch Hochspannung 

Von den Geräten Keysight B2900 können Spannungen an den Anschlüssen “High Force”, “Guard” und “High Sense” von bis zu 210 V ausgehen. Um elektrischem Schlag vorzubeugen, ist bei der Benützung der Geräte Keysight B2900 folgendes zu beachten. 

- Verwenden Sie ein dreiphasiges AC-Stromkabel für die Gerätsteckvorrichtung (Eingang) und schließen Sie das Instrument an eine Erdung an (Sicherheitserdung). 

- Bereiten Sie das Abschirmungsgehäuse vor, dass die Oberfläche eines zu testenden Geräts abdeckt und mit einem Verriegelungsstromkreis ausgestattet ist, der bei geöffneter Tür unterbrochen wird. 

- Vor der Messung verbinden Sie den Verriegelungsstromkreis mit dem Interlock-Anschluss dieses Instruments. 

- Prüfen Sie in regelmäßigen Abständen, dass die Verriegelungsfunktion ordnungsgemäß funktioniert. 

- Bevor Sie die Verbindungen zu den Anschlüssen “High Force”, “Guard” und “High Sense” berühren, schalten Sie das Instrument aus und entladen alle Kondensatoren des Messwegs. Wenn Sie das Instrument nicht ausschalten, führen Sie, unabhängig von den Instrumenteinstellungen, alle folgenden Schritte durch. 

   - Beenden Sie die Messung, indem Sie auf die Taste “On/Off” drücken. Stellen Sie sicher, dass die Statusanzeige “On/Off” nicht leuchtet. 

   - Stellen Sie sicher, dass die Anzeige “HV” nicht leuchtet. 

   - Öffnen Sie die Tür des Abschirmungsgehäuses (öffnen des Interlock-Anschlusses). 

   - Entladen Sie alle Kondensatoren, wenn die Kapazität mit das Instrument verbunden ist. 

- Warnen Sie Mitarbeiter in der Umgebung des Instruments vor den Gefahren. 

###### <u>�</u> 

###### Danger de choc dû à une haute tension 

Une tension dangereuse  (max. ± pour; 210 Vdc) émanant du dispositif Keysight B2900 peut être sortie aux bornes High Force, Guard et High Sense, d'appareil de protection ou de détection. Les précautions suivantes doivent être obserées contre commotion électrique accidentelle. 

- Utilisez un câble d’alimentation CA à trois conducteurs vers le coupleur secteur (entrée) et branchez l’instrument sur une mise électrique à la terre (prise de terre de sécurité). 

- Préparez le boîtier de protection qui couvre l’interface avec le dispositif à tester et équipez-le d’un circuit de sécurité qui s’ouvre lors de l’ouverture d’une porte. 

- Avant de procéder aux mesures, connectez le circuit de sécurité à la borne Interlock de l’instrument. 

- Vérifiez régulièrement le bon fonctionnement de la fonction de sécurité. 

- Avant de toucher les connexions des bornes High Force, Guard et High Sense, mettez l’instrument hors tension et déchargez tout condensateur du chemin de mesure. Si vous ne mettez pas l’instrument hors tension, effectuez « toutes » les opérations ci-dessous, quels que soient les paramètres de l’instrument. 

   - Terminez les mesures en appuyant sur la touche On/Off ; vérifiez que l’indicateur d’état On/Off est éteint. 

   - Vérifiez que le témoin HV est éteint. 

   - Ouvrez la trappe d’accès au boîtier de protection (ouvrez la borne Interlock). 

   - Déchargez les éventuels condensateurs si la capacité est connectée à l’instrument. 

- Informez les personnes travaillant à proximité de l’instrument des conditions. 

###### <u>�</u> 

###### 高電圧感電注意 

Keysight B2900 の High Force 、 Guard 、 High Sense 端子には、危険電圧が出力される ことがあります（最大 ±210 Vdc ）。感電事故防止のため、必ず以下の事柄を 守ってください。 

- 3 極電源ケーブルを使用して本器を接地してください。 

- ドアを開くことによって開放されるインターロック回路を装備し、被測定 デバイスとのインタフェースを覆うことのできるシールド・ボックスを用 意してください。 

- 測定を開始する前にはインターロック回路を本器の Interlock 端子に接続して ください。 

- インターロック機能が正常であることを定期的に確認してください。 

- High Force 、 Guard 、 High Sense 端子に繋がる接続部に触れる前には、本器の電 源を切断してください。また、測定系のキャパシタを放電してください。 電源を切らない場合は、以下の事項を全て実施してください。 

   - On/Off スイッチを押して On/Off スイッチが消灯したことを確認してくださ い。 

   - 高電圧警告インジケータ（ HV ）が消灯していることを確認してくださ い。 

   - シールド・ボックスのドアを開けてください（ Interlock 端子を開放して ください）。 

   - 本器にキャパシタが接続されているならば、キャパシタを放電してくだ さい。 

- 周囲のほかの作業者に対しても、高電圧危険に対する注意を徹底してくだ さい。 

###### Product Stewardship 

- Waste Electrical and Electronic Equipment (WEEE) 



The crossed out wheeled bin symbol indicates that separate collection for waste electric and electronic equipment (WEEE) is required, as obligated by the EU DIRECTIVE and other National legislation. 

Please refer to http://keysight.com/go/takeback to understand your Trade in options with Keysight in addition to product takeback instructions. 

- LCD Fluorescent Lamp 

Certain products sold by Keysight contain a liquid crystal display (LCD); backlighting for the LCD is provided by a fluorescent lamp which contains mercury, and must be managed, recycled, and/or disposed in accordance with all applicable laws, ordinances and regulations. 

For information on how to recycle or dispose of the fluorescent lamp contained in your own product, visit the following website. 

http://about.keysight.com/en/quality/env_compliance.shtml 

If you live in the U.S., also visit the following websites. 

http://www.lamprecycle.org 

http://www.eiae.org 

If you have additional questions, please visit the following website. 

http://www.keysight.com/go/contactus 

- Perchlorate Information 

Perchlorate Material - special handling may apply. Visit the following website. 

http://www.dtsc.ca.gov/hazardouswaste/perchlorate/ 

Equipment's real-time clock battery or coin cell battery may contain perchlorate and may require special handling when recycled or disposed of in California. 

###### NOTE 



NOTE 

###### Contents 

|1|Getting Started|
|---|---|
||Applying DC Output . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .27|
||Performing Spot Measurement. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .29|
||Performing Sweep Measurement . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .30|
||Operation Tips . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .32|
||Operation Summary . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .34|
|2|Introduction|
||Keysight B2900 Series . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .42|
|�|Front View . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .43|
|�|Rear View. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .47|
||Source/Measure Unit . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .49|
||Measurement Parameters . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .50|
||Limit/Compliance . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .50|
||Output and Measurement Ranges . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .51|
||Operation and Functions. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .59|
||Front Panel Interface . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .59|
||Source/Measurement Capabilities . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .61|
||Program and Interface Capabilities. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .62|
||Software and Drivers. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .64|
||Accessories . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .65|
||Furnished Accessories . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .65|
||Available Accessories. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .65|
||Options . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .66|
|3|Installation|
|�|. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .67|



||. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 68|
|---|---|
|�|Inspecting the Shipment . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 70|
||Checking the Operation of Keysight B2900 . . . . . . . . . . . . . . . . . . . . . . . . . 70|
||Checking for Errors . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 71|
||Installing the Keysight B2900 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 72|
||Safety Considerations. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 72|
|�|Environment . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 72|
|�|Connecting the Power Cord . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 73|
||Setting the Power Line Frequency . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 76|
||Setting Date and Time . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 76|
||Bench Installation . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 76|
||Rack Installation . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 77|
||Maintenance. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 78|
|�|Cleaning . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 78|
||Self-test . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 78|
||Self-calibration . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 79|
||Calibration. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 80|
|�|Connecting a DUT . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 81|
||2-Wire Connections or 4-Wire Connections . . . . . . . . . . . . . . . . . . . . . . . . 82|
|�|Floating . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 83|
||Using Test Leads. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 85|
|�|Using the N1295A Test Fixture. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 86|
|�|Using the 16442B Test Fixture . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 87|
||Guarding . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 90|
||Performing Low Current Measurements . . . . . . . . . . . . . . . . . . . . . . . . . . . 90|
|�|Installing the Interlock Circuit . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 91|
||Connecting to the Interfaces . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 94|
||GPIB/USB Interfaces. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 94|
||LAN Interface . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 96|
||Communicating Over the LAN . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 98|



|Using the Graphical Web Interface. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .98|
|---|
|Using Telnet. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .99|
|Using Sockets . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .99|
|Using Digital I/O . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 101|
|Accessory for Digital I/O Connector . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 103|
|4 Front Panel Reference|
|Hard Keys and Rotary Knob . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 106|
|Display and Assist Keys. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 109|
|Dual View . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 110|
|Single View. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 113|
|Graph View. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 124|
|Roll View . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 126|
|Status Information . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 129|
|Function Keys . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 130|
|Config Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 132|
|Output Filter Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 133|
|Output Connection Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 134|
|Sweep Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 135|
|Ranging Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 136|
|Wait Control Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 137|
|Function Key Group. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 138|
|Math Expression Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 138|
|Composite Limit Test Setup Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . 139|
|Limit Test Setup Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 140|
|Trace Buffer Setup Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 141|
|Trigger Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 143|
|Trigger Configuration Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 144|
|Result Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 147|



|Measure Result Dialog Box. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 147|
|---|
|Limit Test Result Dialog Box. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 148|
|Trace Statistical Result Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 149|
|File Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 150|
|File Selection Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 150|
|Program Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 151|
|I/O Key Group. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 152|
|Data Output Format . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 153|
|Format (Measure) Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 154|
|Format (Math/Limit) Dialog Box. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 154|
|Format (Trace) Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 154|
|LAN Configuration Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 155|
|DIO Configuration Dialog Box. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 155|
|DIO Read/Write Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 156|
|Display Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 158|
|Display Preference Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 159|
|System Key Group . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 160|
|5 Front Panel Operations|
|Basic Operations . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 165|
|To Change the Setting in a Field . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 165|
|To Change the Settings on a Dialog Box . . . . . . . . . . . . . . . . . . . . . . . . . . 166|
|To Use Functions Other Than Measurement . . . . . . . . . . . . . . . . . . . . . . . . . 167|
|To Set Power Line Frequency . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 168|
|To Return to Factory Shipment Condition . . . . . . . . . . . . . . . . . . . . . . . . . 168|
|To Apply Initial Settings . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 168|
|To Set Beeper . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 168|
|To Set Date and Time . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 168|
|To Perform Self-Test. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 169|



|To Perform Self-Calibration. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 169|
|---|
|To Set Operations at Power On. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 169|
|To Display Error Message . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 169|
|To Clear Error Buffer . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 170|
|To Clear Timestamp  . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 170|
|To Set Automatic Clear of Timestamp . . . . . . . . . . . . . . . . . . . . . . . . . . . 170|
|To Display Firmware Revision . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 170|
|To Display License Information . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 170|
|To Set GPIB Address . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 171|
|To Set Remote Control Command Set . . . . . . . . . . . . . . . . . . . . . . . . . . . 171|
|To Set Remote Display Mode . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 171|
|To Save/Recall All Setup Information . . . . . . . . . . . . . . . . . . . . . . . . . . . . 172|
|To Enable/Disable Media Transfer Protocol for Easy File Access . . . . . . 172|
|To Enable/Disable the Function to Restore the Trigger Mode Settings on the<br>Local State . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 172|
|To Enable/Disable Source/Limit Real Time Update by Using Digit Pointer and<br>Knob . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 173|
|To Set Source Output . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 174|
|To Set Source Output Mode . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 175|
|To Apply DC Voltage/Current . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 175|
|To Stop Source Output . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 175|
|To Set Limit (Compliance) Value . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 175|
|To Set Output Range . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 176|
|To Set Pulse Output. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 176|
|To Set Sweep Output. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 177|
|To Set List Sweep Output . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 178|
|To Set Source Output Trigger Parameters . . . . . . . . . . . . . . . . . . . . . . . . 179|
|To Set Source Wait Time . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 179|
|To Set Output Filter . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 180|
|To Set Connection Type. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 180|
|To Set Low Terminal State . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 180|



|To Enable or Disable High Capacitance Mode. . . . . . . . . . . . . . . . . . . . . . 181|
|---|
|To Enable or Disable Over Voltage/Current Protection. . . . . . . . . . . . . . . 181|
|To Specify Output-Off Status . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 181|
|To Enable or Disable Automatic Output-On Function . . . . . . . . . . . . . . . . 182|
|To Enable or Disable Automatic Output-Off Function. . . . . . . . . . . . . . . . 182|
|To Set Ranging Mode of the Sweep Source. . . . . . . . . . . . . . . . . . . . . . . . 183|
|To Set Sweep Direction . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 183|
|To Set Source Output Value after Sweep. . . . . . . . . . . . . . . . . . . . . . . . . . 183|
|To Execute Measurement. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 185|
|To Enable Resistance Measurement . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 185|
|To Set Measurement Mode. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 186|
|To Perform Spot Measurement. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 186|
|To Stop Measurement. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 186|
|To Set Measurement Time . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 186|
|To Set Measurement Range . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 187|
|To Perform Sweep Measurement . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 187|
|To Set Measurement Trigger Parameters . . . . . . . . . . . . . . . . . . . . . . . . . 188|
|To Set Measurement Wait Time . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 188|
|To Set Measurement Auto Range Operation . . . . . . . . . . . . . . . . . . . . . . . 189|
|To Enable or Disable Resistance Compensation . . . . . . . . . . . . . . . . . . . . 189|
|To Use Math Function . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 191|
|To Execute Limit Test . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 192|
|To Set Composite Limit Test. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 192|
|To Set Individual Limit Tests. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 193|
|To Display Limit Test Result . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 195|
|To Use Trace Buffer . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 196|
|To Set Trace Buffer . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 196|
|To Display Statistical Data . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 197|
|To Use Program Memory . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 198|
|To Select a Program . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 198|



|To Control Program Operation . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 198|
|---|
|6 Function Details|
|Limit/Compliance . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 200|
|To Set Compliance. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 200|
|Ranging Mode . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 201|
|Setting the Ranging Mode. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 201|
|Measurement Time . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 202|
|Aperture Time. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 202|
|Overhead Time. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 202|
|To Control Source/Measure Timing. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 202|
|Pulse Output . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 204|
|To Control Pulse Output/Measure Timing. . . . . . . . . . . . . . . . . . . . . . . . . 204|
|To Set Pulse Output. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 205|
|Sweep Output . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 206|
|To Set Sweep Output. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 206|
|List Sweep . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 207|
|Output Filter . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 208|
|Over Voltage/Current Protection . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 209|
|Output-Off Status . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 210|
|Automatic Output-ON/OFF Function . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 211|
|High Capacitance Mode . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 212|
|Resistance Measurement . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 213|
|Resistance Compensation . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 213|
|Math Expression . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 214|
|Predefined Math Expressions. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 214|
|Resources Used in the Expressions. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 215|



|Limit Test . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 217|
|---|
|Trace Buffer . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 220|
|Program Memory . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 222|
|Channel Grouping . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 223|
|Trigger System . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 224|
|Trigger Source. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 227|
|Device Actions. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 227|
|Trigger Output. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 229|
|File Access Function (Easy File Access). . . . . . . . . . . . . . . . . . . . . . . . . . . . . 230|
|Interlock Function. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 231<br>�|
|Over Temperature Protection. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 232|
|Initial Settings  . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 233|







<!-- Start of picture text -->
(ec)<br><!-- End of picture text -->



<!-- Start of picture text -->
—--.---- A rca000.0000VOLTS nvMode<br>Limit (Compliance) : Source<br>---,---- V ( |__|<br>1 MeasureMeasureSpeed:Speed AUTOAUTO |————tmF<br>Rangesa SourceMeasureVoltsAmps AUTOAUTO 200mvVOngi Measure<br>Measure Ohm AUTO 2)<br>—<br>LAN (1<br>Config Function Trigger Result File Mores.<br><!-- End of picture text -->



<!-- Start of picture text -->
---.---- A ce!000.0000VOLTS nv |ee<br>Limit (Compliance) : gus<br>---,.---- V (<br>chiLimit<br>Source: VOLTS<br>payee. A 000.0000000.0000 mvmv jFramoe<br>eS se ss<br>=<br>LAN D<br>Config Function Trigger Result File More.<br><!-- End of picture text -->



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->









Source: YoL7rg 000.0000 my 

Source: YoL7rg O00.0000 my 



<!-- Start of picture text -->
O00.0000<br><!-- End of picture text -->



<!-- Start of picture text -->
SOUICE thle Ss<br>A —---—.---—) A G500.0000 my<br>Limit (Compliance) :<br>V<br><!-- End of picture text -->



<!-- Start of picture text -->
Limit (Compliance) :<br>+100,0000<br><!-- End of picture text -->



<!-- Start of picture text -->
1 2 ABC 3 DEF<br>4 GH 5 JKL Cael<br>Lewis 8 Tu Sa<br>; o<br><!-- End of picture text -->



<!-- Start of picture text -->
VOLTS<br>A ---.---- A_ +0.500000 v<br>Limit, (Gonapliqmeaees<br>---.----— VC #1000000<br><!-- End of picture text -->



<!-- Start of picture text -->
ifi<br><!-- End of picture text -->





<!-- Start of picture text -->
ove<br><!-- End of picture text -->

Getting Started Performing Spot Measurement 

#### Performing Spot Measurement 

B2900 can be used as a DC voltage or current meter. The following procedure performs one point current measurement. 

Step 1. Setting the measurement mode (current measurement) 

Press the **Measure** assist key on a 1-ch model, or the **Ch1 Measure** assist key on a 2-ch model. And press the **AMPS (I)** assist key. 

The rotary knob and arrow keys are also available for setting the measurement mode, in the same way as for setting the Source mode under “Applying DC Output”. 

Step 2. Enabling the channel 



Press the Ch 1 switch to enable channel 1. This turns the switch green. Channel 1 starts applying the voltage specified by the Source value. 

Step 3. Starting measurement 



- Press the key to start a single (one shot) measurement. Spot measurement is performed once. 



- Press the key to start a repeat (continuous) measurement. Spot measurement is performed repeatedly. 

The measurement results will be displayed in the left half of the LCD. 

The following example shows the measurement results for a 1 kΩ resistor connected between the High Force and Low Force terminals of channel 1. 



Step 4. Disabling the channel 



Press the Ch 1 switch to disable channel 1. This turns off the switch light. 

B2900B/BL Series User’s Guide, Edition 2 

29 



<!-- Start of picture text -->
View<br><!-- End of picture text -->



<!-- Start of picture text -->
---.----/ A)\ Source000-0000{\ VOLTS nv Le |<br>r,s V +10.00000<br>' Measure Speed; AUTO ro |<br>Ranges ; SourceMeasure VoltsAmps: Spot AUTO 200mVv10nA sae<br>Measure Ohms AUTO 22 - 200M2 ae. |<br>LAN 1<br><!-- End of picture text -->



<!-- Start of picture text -->
Source: VOLTS 7 |<br>—---,.---- A 000. 0000=re’<br>Limit (Compligfice) :<br>. f<br>1<br>Measure Speed: AUTO<br>Sweep Parameters :\ LINEAR SINGLE<br>Start 000.0000 my Stop 000.0000 my<br>Points 1 Step 000.0000 my<br><!-- End of picture text -->



<!-- Start of picture text -->
LINEAR<br><!-- End of picture text -->



<!-- Start of picture text -->
Source: VOLTS oe<br>——— ---—-— A 000.0000mv<br>Limit (Compliance) :<br>1<br>Measure Speed: AUTO<br>Sweep Parameters: LINEAR SINGLE<br>Stat: +1.000000 v Stop: +2.000000 v<br>Points 11 Step: +100.0000 mv<br><!-- End of picture text -->



<!-- Start of picture text -->
View<br><!-- End of picture text -->







<!-- Start of picture text -->
1 | ON ON Y: I{A) LINEAR X: V(¥) LINEAR Auto<br>+002.000a ma Scale<br>=<br>=F Dump<br>ae Screen<br>eet show<br>bee Cursors<br>ral Source<br>40 01.000 mA ,+001.000 V +002.000 ¥ |<br>LAN (G<br><!-- End of picture text -->



<!-- Start of picture text -->
On/Off |<br><!-- End of picture text -->

Getting Started Operation Tips 

#### Operation Tips 

This section introduces key operation and status information on B2900. 

- “Changing the View Mode” 

- “Editing the Setup” 

- “Status Information” 

Figure 1-1 Changing the View Mode 





<!-- Start of picture text -->
Single<br>1<br><!-- End of picture text -->



<!-- Start of picture text -->
Graph<br>G<br><!-- End of picture text -->



<!-- Start of picture text -->
Single Ch2<br>2<br><!-- End of picture text -->



<!-- Start of picture text -->
Graph<br>G<br><!-- End of picture text -->



<!-- Start of picture text -->
D<br><!-- End of picture text -->



<!-- Start of picture text -->
1<br><!-- End of picture text -->



<!-- Start of picture text -->
B2911B Single<br>1<br><!-- End of picture text -->



<!-- Start of picture text -->
Graph<br>G<br><!-- End of picture text -->



<!-- Start of picture text -->
R<br><!-- End of picture text -->



<!-- Start of picture text -->
Single Ch2<br>2<br><!-- End of picture text -->



<!-- Start of picture text -->
Graph<br>G<br><!-- End of picture text -->



<!-- Start of picture text -->
D<br><!-- End of picture text -->



<!-- Start of picture text -->
R<br><!-- End of picture text -->



<!-- Start of picture text -->
1<br><!-- End of picture text -->

For details about the View mode, see “Display and Assist Keys” on page 109. 

B2900B/BL Series User’s Guide, Edition 2 

32 



<!-- Start of picture text -->
A) Ow<br>Source: VOLTS co SOuppee a co Sor - a<br>+0.500000 v (+0.500000) +0.500000 WY<br>|<br>200mv10nA 200mv10nA 200mv10nA ||<br>22 - 200M2 22 - 200MQ2 22 - 200MQ2 ||<br>Result Can | @p mz @ om<br>File XOF* {Fie “XO [[ntrsertin WD<br>B Unit f Unit teh LXI<br>ii 1 irs | cane | oF i Stree |[E.ioa<br>000 v ome vy<br>> = | » |<br>in : 0 + 4 > . 0 +. < > | “Cancel<br>co |<br>Sle} |_vore.._] RY View<br>Cancel Cancel<br><!-- End of picture text -->



<!-- Start of picture text -->
AUTO ARM Hy ic%y 2c, REM & ERR D<br><!-- End of picture text -->

Getting Started Operation Summary 

#### Operation Summary 

This section summarizes front panel operations on B2900. 

- “Basic Operations” 

- “Channel Setup” 

- “Source Setup” 

- “Measurement Setup” 

- “Display Setup” 

- “File Operations” 

- “Miscellaneous Functions” 

- “Interface Setup” 

- “System Setup and Operations” 

For details about front panel operations, see Chapter 4, “Front Panel Reference.” 

B2900B/BL Series User’s Guide, Edition 2 

34 

Getting Started Operation Summary 

|Table 1-1<br>Basic Operations||
|---|---|
|**Task**|**Relevant front panel key**|
|To turn the B2900 on/off|Standby switch|
|To change the display mode|Viewkey|
|To enable/disable the specified channel|Ch 1On/Offswitch or Ch 2On/Offswitch|
|To cancel the previous setup operation|Cancel/Localkey|
|To return to the upper menu of a function key or softkey|Cancel/Localkey|
|To return to the local status from the remote status|Cancel/Localkey|
|To start a single (one shot) output/measurement|Triggerkey|
|To start a repeat (continuous) measurement|Autokey|
|To move the field pointer|Rotary knob or arrow keys|
|To move the digit pointer|Rotary knob or arrow keys|
|To switch the EDIT/MOVE status|Rotary knob on a setup field|
|To select the setup value|Assist keys, rotary knob, or arrow keys|
|To save/recall all setup information of the B2900|Config > Save or Recall function keys|



|Table 1-2<br>Channel Setup||
|---|---|
|**Task**|**Relevant front panel key**|
|To enable/disable the specified channel|Ch 1On/Offswitch or Ch 2On/Offswitch|
|To select the sensing type: 2-wire or 4-wire|Config > Source > Connection function keys|
|To select the low terminal status; grounded or floating|Config > Source > Connection function keys|
|To enable/disable high capacitance mode|Config > Source > Connection function keys|
|To enable/disable over voltage/current protection|Config > Source > Connection function keys|
|To enable/disable resistance compensation|Config > Measure > R Compen function keys|
|To enable/disable 2-channel synchronous operation|Config > Common > Group function keys|



B2900B/BL Series User’s Guide, Edition 2 

35 

Getting Started Operation Summary 

Table 1-3 Source Setup 

|**Task**|**Relevant front panel key**|
|---|---|
|To enable/disable source output|Ch 1On/Offswitch or Ch 2On/Offswitch|
|To select source output mode|Mode, Ch1 Mode, or Ch2 Mode assist key|
|To set source output value|Source, Ch1 Source, or Ch2 Source assist key|
|To set limit/compliance value|Limit, Ch1 Limit, or Ch2 Limit assist key|
|To set constant source ranging mode|Speed assist key or Hide Sweep/Pulse/Trigger assist key on Single<br>view|
|To set sweep source|Show Sweep assist key on Single view|
|To set list sweep source|Edit assist key in the EDIT status for the LIST sweep<br>Start/Stop/Points field|
|To set sweep source ranging mode|Config > Source > Sweep function keys|
|To set sweep direction|Config > Source > Sweep function keys|
|To set source output value after sweep|Config > Source > Sweep function keys|
|To set pulse source|Show Pulse assist key on Single view|
|To set source wait time|Config > Common > Wait function keys|
|To set output filter|Config > Source > Filter function keys|
|To select output-off status|Config > Source > Connection function keys|
|To enable/disable automatic output ON|Config > Source > Connection function keys|
|To enable/disable automatic output OFF|Config > Source > Connection function keys|



Table 1-4 Measurement Setup 

|**Task**|**Relevant front panel key**|
|---|---|
|To enable/disable the specified channel|Ch 1On/Offswitch or Ch 2On/Offswitch|
|To select measurement mode|Measure, Ch1 Measure, or Ch2 Measure assist key|
|To set measurement speed|Speed assist key on Single view|



B2900B/BL Series User’s Guide, Edition 2 

36 

Getting Started Operation Summary 

|**Task**|**Relevant front panel key**|
|---|---|
|To set measurement ranging mode|Speed assist key or Hide Sweep/Pulse/Trigger assist key on Single<br>view|
|To define automatic ranging operation|Config > Measure > Ranging function keys|
|To set measurement wait time|Config > Common > Wait function keys|



###### Table 1-5 Display Setup 

|**Task**|**Relevant front panel key**|
|---|---|
|To change the display mode|Viewkey|
|To change the color set|Display > Color function keys|
|To enable zoom-in|Display > Zoom > ON function keys|
|To disable zoom-in|Zoom Out assist key for the zoom-in display status|
|To change the data digits|Display > Digits function keys|
|To enable/disable the front panel display in<br>the remote condition|Display > Remote function keys|



###### Table 1-6 File Operations 

|**Task**|**Relevant front panel key**|
|---|---|
|To save a measurement result data to USB memory|File > Save > Measure function keys|
|To save a math result data to USB memory|File > Save > Math function keys|
|To save a limit test result data to USB memory|File > Save > Limit Test function keys|
|To save a trace buffer data to USB memory|File > Save > Trace function keys|
|To save a system setting data to USB memory|File > Save > Config function keys|
|To save a graph screen damp to USB memory|Dump Screen assist key on Graph or Roll view|



B2900B/BL Series User’s Guide, Edition 2 

37 

Getting Started Operation Summary 

|**Task**|**Relevant front panel key**|
|---|---|
|To load a system setting data from USB memory|File > Load > Config function keys|
|To load a list sweep data from USB memory|Load assist key in the EDIT status for the LIST sweep<br>Start/Stop/Points field|



|Table 1-7<br>Miscellaneous Func|tions|
|---|---|
|**Task**|**Relevant front panel key**|
|To enable/disable Limit (Compliance) for each<br>polarity|Display > Pref. function keys|
|To enable/disable Source/Limit real time<br>update by using digit pointer and knob|Display > Pref. function keys|
|To see measurement result|Result > Measure function keys|
|To use math expression|Function > Math function keys|
|To see math result|Result > Measure function keys|
|To set limit test|Function > Limit Test function keys|
|To see limit test result|Result > Limit Test function keys|
|To set trace buffer|Function > Trace function keys|
|To see trace statistical result|Result > Trace function keys|
|To select program memory|Program > Catalog function keys|
|To control program memory|Program > Control function keys|
|To set trigger parameters easily|Show Trigger assist key on Single view|
|To set trigger parameters in detail|Function > Trigger > Config function keys|
|To control trigger system|Function > Trigger > Initiate/Abort/Immediate function keys|



B2900B/BL Series User’s Guide, Edition 2 

38 

Getting Started Operation Summary 

Table 1-8 Interface Setup 

|**Task**|**Relevant front panel key**|
|---|---|
|To specify measurement data elements|I/O > Format > Measure function keys|
|To specify math and limit test data elements|I/O > Format > Math/Limit function keys|
|To specify trace statistical data elements|I/O > Format > Trace function keys|
|To select data output format|I/O > Format > Data Type function keys|
|To enable/disable byte swap of binary data|I/O > Format > Data Swap function keys|
|To set LAN configuration|I/O > LAN > Config function keys|
|To see status of LAN interface|I/O > LAN > Status function keys|
|To reset all of LAN connections|I/O > LAN > Reset function keys|
|To sets LAN settings to the default settings|I/O > LAN > Default function keys|
|To enable/disable Media Transfer Protocol for<br>easy file access|I/O > USB function keys|
|To see status of USB interface|I/O > USB function keys|
|To set GPIB address|I/O > GPIB function keys|
|To see status of GPIB interface|I/O > GPIB function keys|
|To set configuration of Digital I/O|I/O > DIO > Config function keys|
|To read/write a value set to Digital I/O|I/O > DIO > R/W function keys|



Table 1-9 System Setup and Operations 

|**Task**|**Relevant front panel key**|
|---|---|
|To set power line frequency|System > PLC function keys|
|To return to the factory shipment condition|System > More > Factory function keys|
|To apply initial settings|System > Reset function keys|
|To perform self-calibration|System > Cal/Test > Self-Cal function keys|
|To perform self-test|System > Cal/Test > Self-Test function keys|



B2900B/BL Series User’s Guide, Edition 2 

39 

Getting Started Operation Summary 

|**Task**|**Relevant front panel key**|
|---|---|
|To check errors|System > Error > Log function keys|
|To clear error log|System > Error > Clear function keys|
|To clear time stamp|System > Timestamp > Clear function keys|
|To set automatic clear of time stamp|System > Timestamp > Auto CLR function keys|
|To set start-up operation|System > More > Start-up function keys|
|To enable/disable beep and sound|System > More > Sound function keys|
|To select remote control command set|System > More > Language function keys|
|To enable/disable the function to restore the<br>trigger mode settings on the local state|Display > Pref. function keys|
|To set date and time|System > More > Info. > Date/Time function keys|
|To perform firmware update|System > More > Info. > Update > Firmware function keys|
|To perform demonstration|System > More > Info. > Demo. function keys|



B2900B/BL Series User’s Guide, Edition 2 

40 

B2900B/BL Series Precision Source/Measure Unit User’s Guide 

2 Introduction 

Keysight B2900 Series 42 Front View 43 Rear View 47 Source/Measure Unit 49 Operation and Functions 59 Software and Drivers 64 Accessories 65 Options 66 

This chapter describes the basic functions and features of the Keysight B2900. 

Introduction Keysight B2900 Series 

#### Keysight B2900 Series 

Keysight B2900 is a series of precision SMU (source/measure unit, SMU). B2900 provides the LCD, front panel keys, and rotary knob for applying voltage/current or measuring voltage/current/resistance. B2900 also supports several functions, such as sweep output, pulse output, arbitrary waveform generation, limit test, trace buffer, math expressions, and graph plot. Hence, B2900 can be used as a DC (constant) voltage/current source, sweep voltage/current source, pulse generator, arbitrary waveform generator, and multimeter. 

The product lineup of the Keysight B2900 series includes the following products. 

Table 2-1 

###### Keysight B2900 Series 

|**Model No.**|**Number of**<br>**SMU**<br>**channels**|**Minimum**<br>**Set**|**Effective se**<br>**resolution**<br>**Measure**|**t and measure valu**<br>**Maximum**<br>**current**|**e**<br>**Maximum**<br>**voltage**|
|---|---|---|---|---|---|
|B2901BL|1|1 pA,<br>1μV|1 pA,<br>0.1μV|DC:±1.5A|±21 V|
|B2910BL|1|0.1 pA,<br>1μV|0.01 pA,<br>0.1μV||±210 V|
|B2901B|1|1 pA,|0.1 pA,|DC:±3.03 A||
|B2902B|2|1μV|0.1μV|Pulse:±10.5 A||
|B2911B|1|0.01 pA|, 0.1μV|||
|B2912B|2|||||



B2900 is a LAN eXtended Interface Core compliant instrument. 

B2900B/BL Series User’s Guide, Edition 2 

42 



<!-- Start of picture text -->
KEYSIGHT 8B2902B Precision Source/MeasureUnit 100fA 2ch LXI 1 2 fe, Ey Siew<br>ane a nn has oe e KL Une<br>A-083.0495 nA : ead<br>-000.1418 mv el<br>= z yay : o + < ><br>- 0.081397 pA p ca80 habsMousze Ach1A Sense a Force Guardia<br>-000.0186 mv re T =<br><!-- End of picture text -->

###### NOTE 

CAUTION 

Introduction Front View 

Dual → Single for channel 1 → Single for channel 2 → Graph → (return to Dual) 

- B2911B 

Single → Graph → Roll → (return to Single) 

- B2912B 

Dual → Single for channel 1 → Single for channel 2 → Graph → Roll → (return to Dual) 

- Cancel/Local key 

Cancels the setup operation if the instrument is in the local status. Returns the instrument to the local status if it is in the remote status. 

- Numeric/alpha keys 

Used to enter the value of setup parameters such as the source output value, limit (compliance) value, and message, specified by the field pointer. 

- Rotary knob 

If the field pointer is in the MOVE (blue) status, turning the knob moves the pointer. Pressing the knob fixes the pointer position and changes the pointer status to EDIT (green). 

If the field pointer is in the EDIT (green) status, turning the knob changes the value of the setup parameter specified by the pointer. Pressing the knob fixes the value and changes the pointer status to MOVE (blue). 

- Left and right keys 

If the field pointer is in the MOVE (blue) status, pressing the key moves the pointer. 

If the field pointer is in the EDIT (green) status, pressing the key changes the value of the setup parameter specified by the pointer. 

If the field pointer is in the EDIT (green) status on a numeric value entry field, pressing the key changes the pointer to a digit pointer. 

- 

- On/Off switch(es) 

Used to enable or disable the SMU channel. Turns the channel off if it is in the output status even if it is in the remote status. One switch on 1-channel models, and two switches on 2-channel models. 

The switch turns green if the channel is enabled. 

B2900B/BL Series User’s Guide, Edition 2 

45 

CAUTION 

CAUTION 



<!-- Start of picture text -->
© ! J | Ep! Ne<br>EE<br>e POLI<br><!-- End of picture text -->

~~Be~~ 



~~SO~~ 





<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->



Introduction Source/Measure Unit 

#### Source/Measure Unit 

The source/measure unit (SMU) can apply voltage or current, and can measure current or voltage. 

Figure 2-1 shows a simplified SMU circuit diagrams. The SMU can perform the following operations. 

- Applies voltage and measures current 

- Applies current and measures voltage 

- Applies voltage and measures voltage 

- Applies current and measures current 

Figure 2-1 

Simplified SMU Circuit Diagram 



<!-- Start of picture text -->
2-wire connections<br>+ A - High Force<br>+<br>- V<br>Low Force<br>Chassis ground<br>4-wire connections<br>+ A - High Force<br>High Sense<br>+<br>- V<br>Low Sense<br>Low Force<br>Chassis ground<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

49 

Introduction Source/Measure Unit 

###### Measurement Parameters 

B2900 supports the following measurement parameters. 

- Current 

- Voltage 

- Resistance 

- Power 

Resistance data is given by Resistance = Vmeas/Imeas. 

Power data is given by Power = Vmeas × Imeas. 

In the above formula, Vmeas is the voltage measurement data, and Imeas is the current measurement data. 

For using the resistance compensation, see “Resistance Compensation” on page 213. 

###### Limit/Compliance 

B2900 has a compliance feature that limits the output voltage or current to prevent damage to the device under test. When the SMU applies voltage, you can specify the current limit/compliance. When the SMU applies current, you can specify the voltage limit/compliance. For details about limit/compliance, see “Limit/Compliance” on page 200. 

B2900B/BL Series User’s Guide, Edition 2 

50 

Introduction Source/Measure Unit 

###### Output and Measurement Ranges 

This section describes typical specifications of the Keysight B2900 series. 

- Maximum voltage and current: see Figures 2-2 and 2-3. 

- Maximum power: 31.8 W 

- Output/measurement value and resolution: see Tables 2-2 to 2-9. 

Figure 2-2 

Maximum Voltage and Current (B2901B, B2902B, B2911B, and B2912B) 



<!-- Start of picture text -->
Current (A)<br>DC output<br>Pulsed output 3.03<br>Duty cycle    99.9999 %~<br>1.515<br>0.105<br>-  21 21<br>- 210 - 6 6 210  Voltage (V)<br>- 0.105<br>Current (A) - 1.515<br>10.5<br>- 3.03<br>1.515<br>Voltage (V)<br>- 6 6<br>- 200 200<br>-1.515<br>Pulsed output<br>Duty cycle    2.5 %~<br>-10.5<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

51 

Introduction Source/Measure Unit 

Figure 2-3 

###### Maximum Voltage and Current (B2901BL and B2910BL) 



<!-- Start of picture text -->
B2901BL<br>Current (A)<br>1.515<br>- 21 21<br>Voltage (V)<br>- 1.515<br>B2910BL Current (A)<br>1.515<br>0.105<br>-  21 21<br>- 210 210  Voltage (V)<br>- 0.105<br>- 1.515<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

52 

Introduction Source/Measure Unit 

Table 2-2 Current Output Range (B2901B, B2902B, B2911B, and B2912B) 

|**Range**<br>|**Setting re**<br>|**solution**<br>|**DC output current or**<br>**pulse peak/base**|**Maximum**<br>|**voltage**<br>|**Pulse width t**<sup>**c**</sup>|
|---|---|---|---|---|---|---|
|**value**|**B2901B**<br>**B2902B**|**B2911B**<br>**B2912B**|<br>**current**<sup>**a**</sup> <sup>**b**</sup>|**DC**<br>**output**|**Pulsed**<br>**output**||
|10 nA<sup>d</sup>|−|10 fA|0≤|I|≤10.5 nA|±210 V|±210 V|50μs≤t≤tmax|
|100 nA|1 pA|100 fA|0≤|I|≤105 nA||||
|1μA|10 pA|1 pA|0≤|I|≤1.05μA||||
|10μA|100 pA|10 pA|0≤|I|≤10.5μA||||
|100μA|1 nA|100 pA|0≤|I|≤105μA||||
|1 mA|10 nA|1 nA|0≤|I|≤1.05 mA||||
|10 mA|100 nA|10 nA|0≤|I|≤10.5 mA||||
|100 mA|1μA|100 nA|0≤|I|≤105 mA||||
|1 A|10μA|1μA|0≤|I|≤105 mA||||
||||105 mA < |I|≤1.05 A|±21 V|±21 V||
||||0≤|I|≤1.05 A|−|±200 V|50μs≤t≤2.5 ms|
|||||−|±180 V|50μs≤t≤10 ms|
|1.5 A|||0≤|I|≤105 mA|±210 V|±210 V|50μs≤t≤tmax|
||||105 mA < |I|≤1.515 A|±21 V|±21 V||
||||0≤|I|≤1.515 A|−|±200 V|50μs≤t≤2.5 ms|
||||0≤|I|≤1.05 A|−|±180 V|50μs≤t≤10 ms|
|3 A|100μA|10μA|0≤|I|≤105 mA|±210 V|±210 V|50μs≤t≤tmax|
||||105 mA < |I|≤1.515 A|±21 V|±21 V||
||||1.515 A < |I|≤3.03 A|±6 V|±6 V||
|10 A|||0≤|I|≤10.5 A|−|±6 V|50μs≤t≤1 ms|
||||0≤|I|≤1.515 A|−|±200 V|50μs≤t≤2.5 ms|
||||0≤|I|≤1.05 A|−|±180 V|50μs≤t≤10 ms|



B2900B/BL Series User’s Guide, Edition 2 

53 

Introduction Source/Measure Unit 

- a. Table 2-6 shows the limitations when using Channels 1 and 2 for DC output or Pulsed output with 50 μs ≤ t ≤ tmax (=99.9999 ks). b. Maximum base current is 500 mA for the pulse with 50 μs ≤ t ≤ 1 ms, and 50 mA for the pulse with 50 μs ≤ t ≤ 2.5 ms or 50 μs ≤ t ≤ 10 ms. 

- c. Maximum duty cycle is 99.9999 % for the pulse with 50 μs ≤ t ≤ tmax, and 2.5 % for the pulse with 50 μs ≤ t ≤ 1 ms, 50 μs ≤ t ≤ 2.5 ms, or 50 μs ≤ t ≤ 10 ms. 

d. Available for B2911B and B2912B. Not available for B2901B and B2902B. 

Table 2-3 Current Output Range (B2901BL and B2910BL) 

|**Range**|**Setting re**|**solution**|**DC outut current**|**Maximum**|**voltage**|
|---|---|---|---|---|---|
|**value**|||**p**|||
||**B2901BL**|**B2910BL**||**B2901BL**|**B2910BL**|
|10 nA<sup>a</sup>|−|100 fA|0≤|I|≤10.5 nA|−|±210 V|
|100 nA <sup>a</sup>|−|1 nA|0≤|I|≤105 nA|−||
|1μA|10 pA|10 pA|0≤|I|≤1.05μA|±21 V||
|10μA|100 pA|100 pA|0≤|I|≤10.5μA|||
|100μA|1 nA|1 nA|0≤|I|≤105μA|||
|1 mA|10 nA|10 nA|0≤|I|≤1.05 mA|||
|10 mA|100 nA|100 nA|0≤|I|≤10.5 mA|||
|100 mA|1μA|1μA|0≤|I|≤105 mA|||
|1 A|10μA|10μA|0≤|I|≤105 mA|||
||||105 mA < |I|≤1.05 A||±21 V|
|1.5 A|||0≤|I|≤105 mA||±210 V|
||||105 mA < |I|≤1.515 A||±21 V|



a. Available for B2910BL. Not available for B2901BL. 

B2900B/BL Series User’s Guide, Edition 2 

54 

Introduction Source/Measure Unit 

Table 2-4 Voltage Output Range (B2901B, B2902B, B2911B, and B2912B) 

|**Range**<br>**value**|**Setting r**<br>**B2901B**<br>**B2902B**|**esolution**<br>**B2911B**<br>**B2912B**|**DC output voltage or**<br>**pulse peak/base**<br>**voltage**|**Maximum**<br>**DC output**|**current**<sup>**a**</sup><br>**Pulsed**<br>**output**|**Pulse width t**<sup>**b**</sup>|
|---|---|---|---|---|---|---|
|0.2 V|1μV|0.1μV|0≤|V|≤0.21 V|±3.03 A|±3.03 A w|ith 50μs≤t≤tmax|
|2 V|10μV|1μV|0≤|V|≤2.1 V||±10.5 A wi|th 50μs≤t≤1 ms|
|20 V|100μV|10μV|0≤|V|≤6 V||||
||||6 V < |V|≤21 V|±1.515 A|±1.515 A w|ith 50μs≤t≤tmax|
|200 V|1 mV|100μV|0≤|V|≤6 V|±3.03 A|±3.03 A w|ith 50μs≤t≤tmax|
||||||±10.5 A wi|th 50μs≤t≤1 ms|
||||6 V < |V|≤21 V|±1.515 A|±1.515 A|50μs≤t≤tmax|
||||21 V < |V|≤210 V|±105 mA|±105 mA||
||||0≤|V|≤180 V|−|±1.05 A|50μs≤t≤10 ms|
||||0≤|V|≤200 V|−|±1.515 A|50μs≤t≤2.5 ms|



a. Table 2-6 shows the limitations when using Channels 1 and 2 for DC output or Pulsed output with 50 μs ≤ t ≤ tmax (=99.9999 ks). 

b. Maximum duty cycle is 99.9999 % for the pulse with 50 μs ≤ t ≤ tmax, and 2.5 % for the pulse with 50 μs ≤ t ≤ 1 ms, 50 μs ≤ t ≤ 2.5 ms, or 50 μs ≤ t ≤ 10 ms. 

B2900B/BL Series User’s Guide, Edition 2 

55 

Introduction Source/Measure Unit 

Table 2-5 

Voltage Output Range (B2901BL and B2910BL) 

|**Range**<br>**value**|**Setting r**<br>|**esolution**<br>|**DC output voltage or**<br>**pulse peak/base**<br>|**Maximum**<br>|**current**<br>|
|---|---|---|---|---|---|
||**B2901BL**|**B2910BL**|**voltage**|**B2901BL**|**B2910BL**|
|0.2 V|1μV|1μV|0≤|V|≤0.21 V|±1.515 A|±1.515 A|
|2 V|10μV|10μV|0≤|V|≤2.1 V|||
|20 V|100μV|100μV|0≤|V|≤6 V|||
||||6 V < |V|≤21 V|||
|200 V <sup>a</sup>|−|1 mV|0≤|V|≤6 V|−||
||||6 V < |V|≤21 V|||
||||21 V < |V|≤210 V||±105 mA|



a. Available for B2910BL. Not available for B2901BL. 

Table 2-6 

Limitations for using Channels 1 and 2 

|**Channel 1 voltage V1**|**Channel 2 voltage V2**|**Current limit**<sup>**a**</sup>|
|---|---|---|
|0 < |V1|≤6 V|0 < |V2|≤6 V|I1 + I2≤4 A|
||6 V < |V2|≤21 V|I1 + I2×1.6≤4 A|
|6 V < |V1|≤21 V|0 < |V2|≤6 V|I1 + I2×0.625≤2.5 A|
||6 V < |V2|≤21 V|I1 + I2≤2.5 A|



a. I1: Channel 1 current, I2: Channel 2 current 

B2900B/BL Series User’s Guide, Edition 2 

56 

Introduction Source/Measure Unit 

Table 2-7 

###### Current Measurement Range 

|**Range value**|**Current measurement value**|**Resolution**|
|---|---|---|
|10 nA<sup>a</sup>|0≤|I|≤10.6 nA|10 fA|
|100 nA<sup>b</sup>|0≤|I|≤106 nA|100 fA|
|1μA|0≤|I|≤1.06μA|1 pA|
|10μA|0≤|I|≤10.6μA|10 pA|
|100μA|0≤|I|≤106μA|100 pA|
|1 mA|0≤|I|≤1.06 mA|1 nA|
|10 mA|0≤|I|≤10.6 mA|10 nA|
|100 mA|0≤|I|≤106 mA|100 nA|
|1 A|0≤|I|≤1.06 A|1μA|
|1.5 A|0≤|I|≤1.53 A||
|3 A<sup>c</sup>|0≤|I|≤3.06 A|10μA|
|10 A<sup>b d</sup>|0≤|I|≤10.6 A||



a. Available for B2910BL, B2911B, and B2912B. Not available for B2901BL, B2901B and B2902B. b. Available for B2901B, B2902B, B2911B, B2912B, and B2910BL. Not Available for B2901BL. 

c. Available for B2901B, B2902B, B2911B, and B2912B. Not available for B2901BL and B2910BL. d. Available for pulse mode. Not available for DC mode. 

B2900B/BL Series User’s Guide, Edition 2 

57 

Introduction Source/Measure Unit 

Table 2-8 

###### Voltage Measurement Range 

|**Range value**|**Voltage measurement value**|**Resolution**|
|---|---|---|
|0.2 V|0≤|V|≤0.212 V|0.1μV|
|2 V|0≤|V|≤2.12 V|1μV|
|20 V|0≤|V|≤21.2 V|10μV|
|200 V<sup>a</sup>|0≤|V|≤212 V|100μV|



a. Available for B2910BL, B2901B, B2902B, B2911B, and B2912B. Not available for B2901BL. 

Table 2-9 

Resistance Measurement Range<sup>1</sup> 

|**Range value**|**Resistance measurement value**|**Display**<br>**resolution**|**Test current**|
|---|---|---|---|
|2Ω|0 < R≤2Ω|1μΩ|1 A|
|20Ω|2Ω< R≤20Ω|10μΩ|100 mA|
|200Ω|20Ω< R≤200Ω|100μΩ|10 mA|
|2 kΩ|200Ω< R≤2 kΩ|1 mΩ|1 mA|
|20 kΩ|2 kΩ< R≤20 kΩ|10 mΩ|100μA|
|200 kΩ|20 kΩ< R≤200 kΩ|100 mΩ|10μA|
|2 MΩ|200 kΩ< R≤2 MΩ|1Ω|1μA|
|20 MΩ<sup>a</sup>|2 MΩ< R≤20 MΩ|10Ω|100 nA|
|200 MΩ<sup>a</sup>|20 MΩ< R≤200 MΩ|100Ω|10 nA|



a. Available for B2910BL, B2901B, B2902B, B2911B, and B2912B. Not available for B2901BL. 

> 1. The resistance measurement range is effective for the resistance measurements set to the AUTO mode which is selected by the :SENS:RES:MODE command. 

B2900B/BL Series User’s Guide, Edition 2 

58 

Introduction Operation and Functions 

#### Operation and Functions 

###### Front Panel Interface 

4.3 inch color TFT WQVGA (480×272, with LED backlight) provides a graphical user interface (GUI) with the following features. 

- 4-view modes (Single, Dual, Graph, Roll view) 

- Easy-to-use key operations 

- Easy access to basic and advanced source/measurement capabilities 

###### Single view 

Single view provides basic and advanced settings and display capabilities for the selected channel from the front panel of the instrument. No additional controller or software is required. 

###### Dual view 

Dual view provides the basic settings and display capabilities for both channels 1 and 2. Up to 6½ digits can be displayed in the Dual view mode. This mode is available only for the B2902B and B2912B. 

###### Graph view 

Graph view displays measurement results on XY graphs (such as I-V and I-t/V-t curves) on up to 2 channels. This is useful for quick evaluation of device characteristics, especially those obtained from sweep measurements. 

###### Roll view 

Roll view draws I-t or V-t curves similar to the curves drawn by a strip chart recorder. Up to 1000 acquired data points can be displayed and updated while the measurement is still in progress. Roll view’s continuous measurement capability is especially useful for monitoring low frequency phenomena. Roll view is available only for the B2911B and B2912B. 

B2900B/BL Series User’s Guide, Edition 2 

59 

Introduction Operation and Functions 

###### Easy-to-Use Key Operations 

- Numeric/alpha keys, assist keys, and rotary knob for easy operation 

- Numeric/alpha keys enable direct input of alphanumeric values. 

- Assist key guides operation on the front panel. 

Easy Access to Source/Measurement Capabilities 

- Settings: 

   - V/I source mode 

   - Source value 

   - Limit (Compliance) value 

   - Sweep parameters 

   - Pulse parameters 

   - Measure speed 

   - Range setting 

   - Trigger setting 

- Result Displays: 

   - Numeric results fully up to 6½ digits both in Single and Dual view modes 

   - Characterization in Graph view and Roll view 

   - Data list 

- Advanced Capabilities: 

   - Limit testing by user definable limits working with component handlers 

   - Trace memory for accumulating measurement (or math) result and collecting their statistics 

- File Operations (USB memory): 

Save: 

- System configuration 

- Measurement/Math result 

- Limit test result 

B2900B/BL Series User’s Guide, Edition 2 

60 

Introduction Operation and Functions 

- Trace data 

- Graph screen dump 

Load: 

- System configuration 

- List sweep data 

###### Source/Measurement Capabilities 

###### Sweep Measurement 

- Number of sweep steps: B2901BL: 1 to 10000 

B2910BL, B2901B, B2902B, B2911B, and B2912B: 1 to 100000 

- Sweep mode: linear, logarithmic (log), or list 

- Sweep direction: single or double 

- Type: 

B2901BL and B2910BL: DC 

B2901B, B2902B, B2911B, and B2912B: DC or pulse 

- Minimum programmable value to create list sweep waveform: 

   - B2901BL: 200 μs with 1 μs resolution 

B2910BL: 50 μs with 1 μs resolution 

B2901B and B2902B: 20 μs with 1 μs resolution 

B2911B and B2912B: 10 μs with 1 μs resolution 

###### Digitizing/Sampling Measurement 

Digitizing or sampling measurement can be performed by appropriate setting of measure speed and trigger interval. 

Minimum trigger interval (maximum speed of measurement): 

B2901BL: 200 μs (5000 points/s) 

B2910BL: 50 μs (20000 points/s) 

B2901B and B2902B: 20 μs (50000 points/s) 

B2900B/BL Series User’s Guide, Edition 2 

61 

Introduction Operation and Functions 

B2911B and B2912B: 10 μs (100000 points/s) 

###### Data Buffers 

Data buffers are array of measurement elements. Each elements hold the following values. 

- Measured voltage 

- Measured current 

- Measured resistance 

- Time stamp 

- Measurement status 

- Source setting 

Maximum buffer size: 

B2901BL: 10000 points/channel 

B2910BL, B2901B, B2902B, B2911B, and B2912B: 100000 points/channel 

###### Program and Interface Capabilities 

###### Programming Language 

The B2900 supports SCPI (Standard Commands for Programmable Instruments). 

- Default command set: Supports all of the advanced features of the B2900. 

- Conventional command set: Supports industry standard conventional SCPI command set for basic compatibility. 

###### Program Memory 

Program memory allows you to store long strings of SCPI command lines once into the built-in volatile memory and then recall those strings multiple times while the program is executing using a single SCPI command. By storing the command strings in memory, the time that would have been spent sending those same commands over a communication bus is eliminated. For tests that utilize lots of repeated code (such as subroutines), program memory can dramatically reduce test times. 

- Maximum number of characters of program name: 32 with alphabets, numbers, hyphens, and underscores 

- Maximum memory size: 100 KB (2500 lines typical) 

B2900B/BL Series User’s Guide, Edition 2 

62 

Introduction Operation and Functions 

###### LXI 

LXI core conformant. The B2900 follows specified LAN protocols and adhere to LXI requirements such as a built-in Web control server and IVI-COM driver. 

- Ethernet: 10/100Base-T 

- USB2.0: USB-TMC488 protocol (rear × 1) 

- GPIB: IEEE-488.2 compliant 

###### USB File System 

USB 2.0 high-speed mass storage (MSC) class device (front × 1) 

###### Digital I/O Interface 

- Connector: 25-pin female D 

- Input/output pins: 14 open drain I/O bits 

- Absolute maximum input voltage: 5.25 V 

- Absolute minimum input voltage: −0.25 V 

- Logic low maximum input voltage: 0.8 V 

- Logic high minimum input voltage: 2.0 V 

- Maximum source current: 1 mA, Vout = 0 V 

- Maximum sink current: 50 mA, Vout = 5 V 

- 5 V power supply pin: 

Limited to 600 mA, solid state fuse protected 

- Safety interlock pin: 

One active high pin and one active low pin. Activation of both pin enables output voltage > 42 V. 

B2900B/BL Series User’s Guide, Edition 2 

63 

Introduction Software and Drivers 

#### Software and Drivers 

- Graphical Web Interface 

The Graphical Web Interface is a web browser based instrument control panel. It enables you to set up and perform a measurement easily and quickly from a web browser using the built-in web server. This allows you to control one B2900 over a LAN connection. 

- IVI-C or IVI-COM drivers 

Compatible with Windows 8 (64 bit/32 bit), 10 (64 bit/32 bit) IO Libraries 17.0 or later. Supports Keysight VEE, Microsoft Visual Studio (Visual Basic, Visual C++, Visual C#), National Instruments LabWindows and LabVIEW. 

- LabVIEW driver (VI) 

National Instruments LabVIEW 8.5/2010 or later. LabVIEW drivers are available at NI.COM. 



For the latest system requirements, go to www.keysight.com/find/b2900 and get the information. 

B2900B/BL Series User’s Guide, Edition 2 

64 

Introduction Accessories 

#### Accessories 

###### Furnished Accessories 

The Keysight B2900B/BL is furnished with the following accessories. 

- Quick Startup Poster, 1 ea. 

- Quick Reference, 1 ea. 

- License-to-use for EasyEXPERT 

- Power cord, 1 ea. 

- USB cable, 1 ea. 

###### Available Accessories 

For the available accessories for the Keysight B2900 series, see the catalog _Accessories for the B2900 Precision Instrument Family_ . 

To get the latest catalog, go to www.keysight.com/find/b2900. 

B2900B/BL Series User’s Guide, Edition 2 

65 

Introduction Options 

Options 

Table 2-10 lists the options for the Keysight B2900. 

Table 2-10 

Options 

|**Option**|**Description**|
|---|---|
|1A7|Calibration, uncertainties, and guardbanding (not accredited)|
|A6J|ANSI Z540 compliant calibration|
|UK6|Commercial calibration certificate with test data|



B2900B/BL Series User’s Guide, Edition 2 

66 

B2900B/BL Series Precision Source/Measure Unit User’s Guide 

3 Installation 

Inspecting the Shipment 70 Installing the Keysight B2900 72 Maintenance 78 Connecting a DUT 81 Installing the Interlock Circuit 91 Connecting to the Interfaces 94 Communicating Over the LAN 98 Using Digital I/O 101 

This chapter describes how to install Keysight B2900 and accessories. 



To avoid electrical shock and instrument damage, turn the instrument off before connecting or disconnecting measurement cable. 

<u>�</u> Mettez la machine hors tension pour fixer le connecteur ou pour retirer le connecteur de l’unité centrale. 

~~LN~~ 



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 

~~Z\~~ 



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 

Installation Inspecting the Shipment 

#### Inspecting the Shipment 

Perform the following inspections when the Keysight B2900 and accessories arrive at your site. 

1. Before unpacking any component, inspect all boxes for any signs of damage that might have occurred during shipment, such as: 

   - dents 

   - scratches 

   - cuts 

   - water marks 

If you suspect any damage, contact your nearest Keysight Sales and Support Office. 

2. When you open the boxes that contain the B2900 and accessories, check the components against the contents lists attached to the boxes. 

If anything is missing, contact your nearest Keysight Sales and Support Office. 

3. Verify the operation of B2900 as described in “Checking the Operation of Keysight B2900”. 

If any problem occurs, contact your nearest Keysight Sales and Support Office. 

###### Checking the Operation of Keysight B2900 

1. Make sure that the standby switch is set to off. 

2. Connect the power cord from the B2900’s rear panel AC input connector (receptacle) to an AC power outlet at your site. 

3. Press the standby switch to turn on the instrument. 

The initialization screen will appear on the B2900’s front panel display, and the power-on self-test is automatically executed. 

If the B2900 is operating normally, the front panel LCD displays as shown in Figures 3-1 or 3-2. 

B2900B/BL Series User’s Guide, Edition 2 

70 



<!-- Start of picture text -->
Source; VOLTS Mode<br>---,---- A 000.0000 my =<br>~ Limit (Compliance) : Source<br>I V +1 00.0000 pa<br>ule Limit<br>Measure Speed: AUTO<br>Ranges : Source Valts AUTO 200mv¥ Measure<br>Measure Amps AUTO 1Ona<br>Measure Ohms AUTO 2m —- 2Z00MR More<br>LAN 1<br>Config Function Trigger Result File Mores,<br><!-- End of picture text -->



<!-- Start of picture text -->
{| SSeS Source:000.0000VOLTS  my iode:<br>~ Linitsakes(Compliance)eeerietiet!: Sourcecht<br>chi<br>Lirnit<br>Source: VOLTS<br>---,---- A 000.0000 mv ;Paen<br>Limit (Compliance) :<br>LAN DPD<br>Config Function Trigger Result File More...<br><!-- End of picture text -->



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 

~~L\~~ 





<!-- Start of picture text -->
iad<br>errr<br><!-- End of picture text -->

# ~~Laid~~ ~~<u>Lia</u> Ladi~~ 





NOTE 



<!-- Start of picture text -->
7(a -<br><!-- End of picture text -->

CAUTION 



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING ~~A~~ 

Installation Maintenance 

Perform the self-test to unlock the channel. The channel can be used soon if the self-test does not report any problem. 

- If you feel that the instrument may be defective 

- For preventive maintenance 

###### To perform The following procedure shows how to perform the self-test. self-test 

1. If it is in the remote state, press the Cancel / Local key. 

2. Press the On/Off switch and confirm that the switch turns off. 

3. Disconnect test leads and cables from the channel terminals. 

4. Press the following function keys. 

More > System > Cal/Test > Self-Test 

Confirmation dialog box is opened. 

5. Press the OK key. This starts the self-test. 

###### Self-calibration 

Keysight B2900 provides the self-calibration function to maintain the measurement performance. If the environmental temperature changes ±3 °C or more, perform the self-calibration. This is effective for the accurate measurements by minimizing the effect of thermal drift. The self-calibration must be performed after warming-up of 60 minutes. Before performing the self-calibration, turn the channel output off and disconnect test leads and cables from the terminals. 

To perform The following procedure shows how to perform the self-calibration. self-calibration 

1. If it is in the remote state, press the Cancel / Local key. 

2. Press the On/Off switch and confirm that the switch turns off. 

3. Disconnect test leads and cables from the channel terminals. 

4. Press the following function keys. 

More > System > Cal/Test > Self-Cal 

Confirmation dialog box is opened. 

5. Press the OK key. This starts the self-calibration. 

B2900B/BL Series User’s Guide, Edition 2 

79 

Installation Maintenance 

###### Calibration 

Calibration and adjustments must be performed periodically so that the instruments satisfy the specifications, and keep a good condition. It is recommended to perform the calibration once a year at least. For the calibration and adjustments, contact your nearest Keysight Sales and Support Office. Trained service personnel will perform the calibration and adjustments. 

B2900B/BL Series User’s Guide, Edition 2 

80 

NOTE 

NOTE 



NOTE 

~~L\~~ 



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 





<!-- Start of picture text -->
{| SS SS Aaw” Source;000.0000VOLTS my aode2<br>—— eS chi<br>Limit (Compliance) : Sains<br>chi<br>Lirnit<br>Source: VOLTS<br>—--—- ---- A 000.0000 mv Jee<br>Limit (Compliance) :<br>LAN D<br>Config Function Trigger Result File More...<br><!-- End of picture text -->



<!-- Start of picture text -->
1-x<br><!-- End of picture text -->



<!-- Start of picture text -->
A ---.---- A Source:000.0000VOLTS my ilodea<br>~ Limitsoak (Compliance)ereert fered: Sourceous<br>chi<br>Lirnit<br>Source: VOLTS<br>---,---- A 000.0000 mv [eae<br>Limit (Compliance) :<br>pon LAND<br>Config Function Trigger Result File More...<br><!-- End of picture text -->







<!-- Start of picture text -->
CL GH OLE<br><!-- End of picture text -->



<!-- Start of picture text -->
(O}<br><!-- End of picture text -->







<!-- Start of picture text -->
oe, a<br><!-- End of picture text -->



<!-- Start of picture text -->
&<br><3 Ag<br><!-- End of picture text -->



<!-- Start of picture text -->
aa<br>“Mar<br><!-- End of picture text -->



<!-- Start of picture text -->
wi<br><!-- End of picture text -->



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->



<!-- Start of picture text -->
CAUTION<br><!-- End of picture text -->

NOTE 

Installation Connecting a DUT 



###### **Maximum voltage and current** 

The test fixture and adapters must be used under the following limitations to prevent damage. 

Keysight N1297A: ±250 V maximum, ±200 V maximum for connecting 16442B Keysight N1297B: ±250 V maximum, ±200 V maximum for connecting 16442B Keysight 16442B: ±200 V, 1 A maximum for SMU input 

- Connections 1. Attach the banana to triaxial adapter to the B2900 source/measure terminals. See Figure 3-6. If there is some space as “Bad connection”, the contact is not 

- <u>�</u> enough. 

   2. Connect the triaxial cables between the adapter and the 16442B. Figure 3-8 shows a connection example. 

   3. Connect the DUT on the 16442B wiring panel by using the 16442B’s furnished accessories, socket module, blank board, and wire suitable for the DUT. 

      - Figure 3-9 shows the connection example for the connections of Figure 3-8. 

      - For 2-wire connections, connect the SMU 1 Force terminal to a DUT terminal, and connect the SMU 2 Force terminal to the other DUT terminal. 

For 4-wire connections, connect the SMU 1 Force and Sense terminals to a DUT terminal. And connect the SMU 3 Sense and Guard terminals to the other DUT terminal if the adapter’s Low Force/Low Sense connector is connected to the SMU 3 Sense connector. The Low Force signal appears on the Guard terminal. 

###### Figure 3-8 

Connection Example between N1297A and 16442B 



<!-- Start of picture text -->
N1297A Adapter<br>for 2-wire connections<br>Low Force<br>to SMU 2<br>       Force<br><!-- End of picture text -->



<!-- Start of picture text -->
N1297B Adapter<br>for 4-wire connections<br>Low Force/<br>Low Sense<br>to SMU 3 Sense<br>(or GNDU)<br>High Sense High Force<br>to SMU 1 Sense to SMU 1 Force<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

88 



<!-- Start of picture text -->
© © © © © © ©<br>©\o o\ © © © ©\oO © © ©<br><!-- End of picture text -->

NOTE 





<!-- Start of picture text -->
CAUTION<br><!-- End of picture text -->

Installation Installing the Interlock Circuit 

#### Installing the Interlock Circuit 

The interlock circuit is a simple electric circuit, as shown in Figure 3-11. The circuit 

- electrically opens when an access door is opened, and closes when the door is closed. 

B2900 cannot apply high voltages over ±42 V when the Digital I/O interlock terminal is open. To perform high voltage measurements, the B2900 interlock terminal must be connected to the interlock circuit installed in the measurement environment such as the shielding box. The interlock circuit is important and necessary to prevent electrical shock when the user touches the measurement terminals. 



- 

Hazardous voltage, instrument maximum output voltage may appear at the High Force, High Sense, and Guard terminals if the fixture cover is closed. To prevent electrical shock, do _not_ expose these lines. 

Une tension dangereuse, une tension de sortie maximale de l'appareil peut apparaître aux bornes High Force, High Sense et Guard si le couvercle de l'équipement est fermé. Afin d’éviter toute décharge électrique, n’exposez pas ces lignes. 

Figure 3-11 

###### Interlock Circuit 



<!-- Start of picture text -->
Access door<br>Mechanical switches<br><!-- End of picture text -->



<!-- Start of picture text -->
R<br>25<br>15, 19<br>16, 17<br>14<br>24<br><!-- End of picture text -->



<!-- Start of picture text -->
24, 25<br>R<br>22<br>15, 19<br>14<br>17, 18 24<br><!-- End of picture text -->





B2900B/BL Series User’s Guide, Edition 2 

91 

Installation Installing the Interlock Circuit 

Requirements 

   - LED (Keysight part number 1990-0486 or equivalent), 1 ea. 

   - Mechanical switch (Keysight N1254A-402 or equivalent), 2 ea. 

   - Resistor, 500 Ω to 1 kΩ (choose lower one to get higher output of LED), 1 ea. 

   - 25-pin D-sub connector (for wiring, mounted on the shielding box), 1 ea. 

   - Connection wire 

   - 25-pin D-sub cable, 1 ea. 

- Procedure 1. Mount two mechanical switches onto your shielding box, so that the switches close when the access door is closed, and open when the door is opened. See Figure 3-13 for the switch dimensions. 

   2. Mount an LED onto your shielding box. See Figure 3-12 for the LED dimensions. 

The LED is used as a high voltage indicator which is lit when B2900 is in the high voltage output status. 

3. Mount a D-sub connector onto your shielding box. 

4. Use a wire and short the following pins of the D-sub connector. See Figure 3-11 for a connection example. 

For the negative logic: Pins 24 and 25 

For the positive logic: Pins 16 and 17 

5. Use a wire and connect the two switches in series between the following pins of the D-sub connector. See Table 3-1 for the Digital I/O pin assignment. 

   - For the negative logic: Pins 16 and some ground pins 

The ground pins are pin numbers 15 and 17 to 21. 

For the positive logic: Pins 24 and some +5 V pins 

The +5 V pins are pin numbers 22, 23, and 25. 

6. Use a wire and connect the LED between the pin 14 and some ground pins of the D-sub connector. Then set the pin 14 to HIGH VOLTAGE LAMP. See “DIO Configuration Dialog Box” on page 155. 

7. Connect a resistor between the pin 14 and an unused +5 V pin. 

B2900B/BL Series User’s Guide, Edition 2 

92 

Installation Installing the Interlock Circuit 

Figure 3-12 

Dimensions of the LED (Keysight part number 1990-0486) 



<!-- Start of picture text -->
0.8 24<br>Cathode (-) 1.3<br>3.2 2.5<br>Anode (+)<br>4.5<br>5.9 Units: mm<br><!-- End of picture text -->

Figure 3-13 

Dimensions of the Interlock Switch (Keysight N1254A-402) 



<!-- Start of picture text -->
2.8<br>10.3 4.3 4.75<br>2.8<br>6.4<br>35.6<br>Switch off 8.1 2.8<br>3.1<br>3.2<br>2.8<br>15.3<br>10.3 NC<br>6.3 15.9 18.8<br>NO<br>COM<br>3.1<br>22.2<br>14<br>27.8 Units: mm<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

93 



<!-- Start of picture text -->
CAUTION<br>NOTE<br>Ean *<br>y—s_(elf CN<br><!-- End of picture text -->











<!-- Start of picture text -->
oo<br><!-- End of picture text -->





<!-- Start of picture text -->
» A T E<br><!-- End of picture text -->



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

NOTE 

NOTE 





<!-- Start of picture text -->
Ww KEYSIGHTTECHNOLOGIES "SerialB2912Bnumber: MY12345678 Login %<br>Home Control Instrument LXI Event Logs SCPI Reference Guide Configure LAN >)<br>Connected to B2912B LxXI<br>at IP address 192.168.1.100<br>— iSsi ea<br>C1 Enable front panel identification indicator<br>Description<br>Model number B2912B<br>Manufacturer Keysight<br>Serial number MY12345678<br>Firmware revision 0.0.0.0<br>Description Keysight B2912B Precision Source/Measure Unit - MY12345678<br>VISA instrument addresses<br><!-- End of picture text -->

NOTE 

Installation Communicating Over the LAN 

The socket programming interface also allows control socket connections. The control socket can be used by a client to send device clear and to receive service requests. Unlike the data socket, which uses a fixed port number, the port number for a control socket varies and must be obtained by sending the following SCPI query to the data socket: SYSTem:COMMunicate:TCPip:CONTrol? 

Once the port number is obtained, a control socket connection can be opened. As with the data socket, all commands to the control socket must be terminated with a newline, and all query responses returned on the control socket will be terminated with a newline. 

To send a device clear, send the string “DCL” to the control socket. When the B2900 has finished performing the device clear, it will echo the string “DCL” back to the control socket. 

Service requests are enabled for control sockets using the Service Request Enable register. Once service requests have been enabled, the client program listens on the control connection. When SRQ becomes true, the instrument will send the string “SRQ +nn” to the client. The “nn” is the status byte value, which the client can use to determine the source of the service request. 

100 

B2900B/BL Series User’s Guide, Edition 2 

Installation Using Digital I/O 

#### Using Digital I/O 

B2900 has a Digital I/O connector, D-sub 25 pin female, for general purpose input/output (GPIO). It can be used for: 

- Trigger input 

- Trigger output 

- Digital signal input/output 

- Digital signal input 

- Start of Test (SOT) input (for component handlers) 

- Busy status output (for component handlers) 

- End of Test (EOT) output (for component handlers) 

- Interlock control 

- High voltage status output (shared with the digital signal input/output DIO 14) 

The pin assignment of the Digital I/O connector is shown in Table 3-1. Pins DIO 1 to DIO 14 can be assigned to one of the above functions, except for interlock control. To set the DIO functions, press the More function key, then the I/O > DIO > Config softkeys. For details, see “DIO Configuration Dialog Box” on page 155. 

Figure 3-14 shows the input/output circuits internally connected to each pin of the Digital I/O connector. 

For the available accessory, see “Accessory for Digital I/O Connector” on page 103. 

B2900B/BL Series User’s Guide, Edition 2 

101 

Installation Using Digital I/O 

Figure 3-14 

Table 3-1 



<!-- Start of picture text -->
Digital I/O Internal Circuit<br>Vcc<br>R1<br>R2<br>to Input control to Digital I/O<br>connector<br>from Output control<br>Vcc: +5 V<br>R1: 5 kohm<br>R2: 100 ohm<br><!-- End of picture text -->

Digital I/O Pin Assignment 

|**Description**|**Pin Nu**|**mber**|**Description**|
|---|---|---|---|
|+5 V<sup>a</sup>|25|13|DIO 13 (bit 13)|
|Interlock control<sup>b</sup>|24|12|DIO 12 (bit 12)|
|+5 V<sup>a</sup>|23|11|DIO 11 (bit 11)|
|+5 V<sup>a</sup>|22|10|DIO 10 (bit 10)|
|GND|21|9|DIO 9 (bit 9)|
|GND|20|8|DIO 8 (bit 8)|
|GND|19|7|DIO 7 (bit 7)|
|GND|18|6|DIO 6 (bit 6)|
|GND|17|5|DIO 5 (bit 5)|
|Interlock control<sup>c</sup>|16|4|DIO 4 (bit 4)|
|GND|15|3|DIO 3 (bit 3)|
|DIO 14 (bit 14) or<br>|14|2|DIO 2 (bit 2)|
|High voltage status||1|DIO 1 (bit 1)|



a. Current limit: 600 mA (total current to the pins 22, 23, and 25) 

b. Used for the positive logic. Connected to the pin 25 for the negative logic. c. Used for the negative logic. Connected to the pin 17 for the positive logic. 

102 

B2900B/BL Series User’s Guide, Edition 2 



<!-- Start of picture text -->
au [TT ;<br>a l E e<br><!-- End of picture text -->



Installation Using Digital I/O 

104 

B2900B/BL Series User’s Guide, Edition 2 



<!-- Start of picture text -->
KEYSIGHT B2902B Precision Source/Measure Unit 100fA 2ch LX/ 1 2 Ey<br>Trigger : Lt mide has bien | Gime<br>A-083.0495 nA : ge<br>-000.1418 mv ‘cal<br>= aH : o *% < ><br>-0.081397 pA 100.0 nS ACh1 Sense 7 Force Guard<br>-000.0186 mv — T =<br><!-- End of picture text -->



Front Panel Reference Hard Keys and Rotary Knob 

#### Hard Keys and Rotary Knob 

Standby switch Turns the instrument on or off. When the power is on, the LED below the switch is turned green. 

Trigger Starts a single (one shot) output/measurement or initiates trigger system. If a repeat measurement is in progress, stops it. 

The single measurement is performed with the DC bias output, staircase sweep output, pulsed bias output, or pulsed sweep output set to the channel. One single measurement can contain the maximum of 100000 measurement points for B2900 except B2901BL and 10000 points for B2901BL. 

Once a single measurement starts, the data buffer is cleared, and the last single measurement result is stored in the buffer. The maximum data buffer is 100000 for B2900 except B2901BL and 10000 for B2901BL. The measurement result is displayed on the Single view, Dual view, Graph view, or Roll view. Also the last single measurement result can be listed and displayed on the Measure Result dialog box. 

Auto Starts a repeat (continuous) measurement. If a repeat measurement is in progress, stops it. The repeat measurement is performed with the DC bias output of the Source value. And the measurement result is displayed on the Single view, Dual view, or Roll view. The repeat measurement result is not stored in the buffer. 

The repeat measurement is performed with the following trigger setup. Settings of “Trigger parameters” are ignored. 

- Acquire trigger (measurement trigger): Initiate 

- ARM acquire count: Infinite 

- ARM acquire source: AUTO (automatic internal) 

- TRIGger acquire count (Measure Count) 100 

- TRIGger acquire source (Measure Trigger): AUTO (automatic internal) 

- TRIGger acquire timer period (Measure Period): 10 ms (this is automatically extended if the measurement is not completed.) 

- Trigger delay (Source Delay = Measure Delay): 0 s 

106 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Hard Keys and Rotary Knob 

      - Trigger output: Disable 

   - View Changes the display mode. See “Display and Assist Keys” on page 109. 

- Cancel / Local Cancels the setup operation if the instrument is in the local status. Returns the instrument to the local status if it is in the remote status. 

   - On/Off Used to enable or disable the SMU channel. Turns the channel off if it is in the output status even if it is in the remote status. One switch on 1-channel models, and two switches on 2-channel models. 

The switch turns green if the channel is enabled. 

The switch turns red if the channel is in the high voltage state. 

- Numeric/alpha Used to enter the value for setup parameters such as the source output value, keys limit (compliance) value, and message, specified by the field pointer. The value can be changed if the field pointer is in the EDIT (green) status. 

   - Rotary knob If the field pointer is in the MOVE (blue) status, turning the knob moves the pointer. Pressing the knob fixes the pointer position and changes the pointer status to EDIT (green). 

If the field pointer is in the EDIT (green) status, turning the knob changes the value of the setup parameter specified by the pointer. Pressing the knob fixes the value and changes the pointer status to MOVE (blue). 

If the field pointer is in the EDIT (green) status on a setup field other than the Source field and the Limit (Compliance) field, turning the knob changes the setting value. The value is applied by pressing the knob. 

If the field pointer is in the EDIT (green) status on the Source or Limit field, turning the rotary knob changes the display value and updates the source channel output in real time. However, if the digit pointer is available, the source channel works depending on the setting of the real time update (the setting of the Immediate V/I Update by Knob field). For the Immediate V/I Update by Knob field, see “Display Preference Dialog Box” on page 159. 

If the digit pointer is on a digit of a numeric value entry field, turning the knob changes the value of the digit. Note that changing the value from 9 to 0 or from 0 to 9 changes the value of the next digit. 

If the digit pointer is on the decimal point of a numeric value entry field, turning the knob moves the decimal point if the Immediate V/I Update by Knob setup is OFF. 

B2900B/BL Series User’s Guide, Edition 2 

107 

Front Panel Reference Hard Keys and Rotary Knob 

Left and right If the field pointer is in the MOVE (blue) status, pressing the key moves the pointer. keys If the field pointer is in the EDIT (green) status, pressing the key changes the value of the setup parameter specified by the pointer. 

If the field pointer is in the EDIT (green) status on a numeric value entry field, pressing the key changes the pointer to the digit pointer. 

If the digit pointer is on a digit of a numeric value entry field, pressing the key moves the pointer on the digits. 

108 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Display and Assist Keys 

#### Display and Assist Keys 

Keysight B2900 provides several display modes which depend on the model as shown below. The display mode is changed by the View key. Pressing the key changes the mode as shown below. 

- B2901BL, B2910BL, and B2901B 

Single → Graph → (return to Single) 

- B2902B 

Dual → Single for channel 1 → Single for channel 2 → Graph → (return to Dual) 

- B2911B 

Single → Graph → Roll → (return to Single) 

- B2912B 

Dual → Single for channel 1 → Single for channel 2 → Graph → Roll → (return to Dual) 

For each display mode, five assist keys are available on the right side of the display. They are assigned to several softkeys such as Mode, Source, Limit, Measure, and More. The softkey assignment depends on the display mode. 

For details of each display mode and assist keys, see the following sections. 

- “Dual View” 

- “Single View” 

- “Graph View” 

- “Roll View” 

- “Status Information” 

The status information is common for all display modes. It is displayed above the bottom softkeys associated with the function keys. This display area is also used to display system messages and error messages. 

B2900B/BL Series User’s Guide, Edition 2 

109 



<!-- Start of picture text -->
HC ac Source: VOLTS 4<br>1 0 0 0 0 0 2 1 8 mA +01.00000 ¥ _<br>T T Lirnit (Cornplianice) : San<br>0 1 . 0 0 6 4 3 V +050.0000 ma<br>chi<br>[nlc aa Source: VOLTS<br>= 000.0155 pA o0.00000 v<br>0 0 . 8) 0 2 4 3 V Limit+100,0000(Compliance) :  pa<br>D<br>Config Function Trigger Result File More...<br><!-- End of picture text -->

Front Panel Reference Display and Assist Keys 

Assist keys In the following descriptions, _Ch n_ indicates _Ch 1_ or _Ch 2_ . 

_Ch n_ Mode Changes the assist keys to VOLTS (V) and AMPS (I) used to select the source function. VOLTS (V) sets the voltage source. AMPS (I) sets the current source. _Ch n_ Source Changes the assist keys to the unit keys shown below. μV, mV, and V for the voltage source pA, nA, μA, mA, and A for the current source 

First, enter or set the output value by using the numeric/alpha keys, rotary knob, and arrow keys. Then press one of the unit keys to apply the value. 

_Ch n_ Limit Changes the assist keys to the unit keys shown below. pA, nA, μA, mA, and A for the current compliance μV, mV, and V for the voltage compliance 

First, enter or set the limit value by using the numeric/alpha keys, rotary knob, and arrow keys. Then press one of the unit keys to apply the value. 

_Ch n_ Measure Changes the assist keys to AMPS (I), VOLTS (V), OHMS (R), and WATTS (P) used to select the measurement function. AMPS (I) sets the current measurement. VOLTS (V) sets the voltage measurement. OHMS (R) sets the resistance measurement. WATTS (P) sets the power measurement. Resistance data is given by Resistance = Vmeas/Imeas. Power data is given by Power = Vmeas×Imeas. 

In the above formula, Vmeas is the voltage measurement data, and Imeas is the current measurement data. 

For details on using resistance compensation, see “Resistance Compensation” on page 213. 

B2900B/BL Series User’s Guide, Edition 2 

111 

Front Panel Reference Display and Assist Keys 

More... 

Changes the assist keys to _Ch 1_ assist keys or _Ch 2_ assist keys. _Ch 1_ assist keys are effective for channel 1. _Ch 2_ assist keys are effective for channel 2. 

112 

B2900B/BL Series User’s Guide, Edition 2 



<!-- Start of picture text -->
HC5 Hee4W Source: VOLTS Mode<br>000.0567 pA 00.00000 v<br>0 Limit (Compliance) : Source<br>'  0 . 0) 0 0 0) 1 , +100.0000 pa<br>Measure Speed: MANUAL 020.0ms (01.00PLC)<br>Ranges : Source Volts FIXED! 20 V<br>Measure Amps<br>Measure Ohms AUTO 22 —- 200M2<br>1<br>Confia Function Trager Result File More...<br><!-- End of picture text -->

Front Panel Reference Display and Assist Keys 

D2. Secondary measurement data. Turns orange if the data exceeds the Limit value. 

For the limit test, D2 displays PASS or FAIL instead of the measurement data. The limit test is not available for B2901BL. 

Assist keys 1 Mode Changes the assist keys to VOLTS (V) and AMPS (I) used to select the source function. VOLTS (V) sets the voltage source. AMPS (I) sets the current source. Source Changes the assist keys to the unit keys shown below. μV, mV, and V for the voltage source pA, nA, μA, mA, and A for the current source First, enter or set the output value by using the numeric/alpha keys, rotary knob, and arrow keys. Then press one of the unit keys to apply the value. Limit Changes the assist keys to the unit keys shown below. pA, nA, μA, mA, and A for the current compliance μV, mV, and V for the voltage compliance First, enter or set the limit value by using the numeric/alpha keys, rotary knob, and arrow keys. Then press one of the unit keys to apply the value. Measure Changes the assist keys to AMPS (I), VOLTS (V), OHMS (R), and WATTS (P) used to select the measurement function. AMPS (I) sets the current measurement. VOLTS (V) sets the voltage measurement. OHMS (R) sets the resistance measurement. WATTS (P) sets the power measurement. Resistance data is given by Resistance = Vmeas/Imeas. Power data is given by Power = Vmeas×Imeas. In the above formula, Vmeas is the voltage measurement data, and Imeas is the current measurement data. 

114 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Display and Assist Keys 

For details on using resistance compensation, see “Resistance Compensation” on page 213. 

More... 

Changes the assist keys to Assist keys 2. 

Assist keys 2 Speed Changes the assist keys to AUTO (1 PLC for 10 nA and 100 nA ranges, 0.01 PLC for other ranges), SHORT (0.01 PLC), MEDIUM (0.1 PLC), NORMAL (1 PLC), LONG (10 PLC), or MANUAL used to select the measurement speed. The aperture time is automatically set to the above values in parentheses. For the aperture time, see “Measurement Time” on page 202. 

For the MANUAL speed, the aperture time must be set to the right field in seconds or PLC (power line cycle, number of power line cycles per measurement). Move the field pointer to the one by using the arrow keys or rotary knob and press the knob to set the pointer to the EDIT mode. Enter or set the value by using the numeric/alpha keys, knob, or arrow keys, then press the knob or one of the unit keys to apply the value. For setting the seconds value, the following unit keys are available as assist keys. 

μs, ms, and s 

Show Sweep Displays the sweep setup parameters shown in “Sweep parameters” on page 117, and changes the softkey label to _Hide Sweep_ . Hide Sweep Displays the range setup parameters shown in “Range parameters” on page 116, and changes the softkey label to _Show Sweep_ . 

Show Pulse Displays the pulse setup parameters shown in “Pulse parameters” on page 121, and changes the softkey label to _Hide Pulse_ . Pulse is not available for B2901BL and B2910BL. 

Hide Pulse Displays the range setup parameters shown in “Range parameters” on page 116, and changes the softkey label to _Show Pulse_ . Pulse is not available for B2901BL and B2910BL. 

Show Trigger Displays the trigger setup parameters shown in “Trigger parameters” on page 122, and changes the softkey label to _Hide Trigger_ . 

B2900B/BL Series User’s Guide, Edition 2 

115 

Ranges: Source Volts: Spot AUTO 200mv Measure 4mps Measure Ghms AUTO 2 f -— 2O0Ma 

NOTE 

NOTE 

Sweep Parameters: LINEAR SINGLE Stat: OOO.000Q0my Stop: +1.500000 v Points : 101 Step: +015.0000my¥ 

Front Panel Reference Display and Assist Keys 

Points Sets the number of sweep steps. Step Sets the sweep step value (incremental step value). Not available for the LOG and LIST sweep operations. 

When the field pointer is in the EDIT (green) status on an entry field, the assist keys are changed to the unit keys as shown below. 

- pA, nA, μA, mA, and A for current sweep 

- μV, mV, and V for voltage sweep 

For the range operation of the sweep source, see “Sweep Dialog Box” on page 135. The range parameters _Source Volt: Spot_ and _Source Amps: Spot_ are not effective for the AUTO and BEST range operations of the sweep source. They are effective for the constant source and FIXED range operation of the sweep source. 

- List sweep When the field pointer is in the EDIT (green) status on the LIST sweep setup Start/Stop/Points field, the following assist keys are available for setting the list sweep source. 

Edit Opens the List Sweep dialog box used to set the list sweep source. 

Load Opens the Load List Sweep Data dialog box used to load a list sweep data from a USB memory connected to the front panel USB-A connector. 

- List Sweep dialog box 

This dialog box provides the following fields for setting the list sweep source. After the list is completed, press OK to apply the data and close the dialog box. 

(data graph) Displays the shape of the list sweep output (data list) Lists the data index in the left cell and the output value in the right cell. 

When the pointer is in the MOVE status, the pointer can be moved in the crosswise direction. And pressing the knob changes the status to EDIT. 

When the right cell is in the EDIT status, the output value can be entered to the cell. 

118 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Display and Assist Keys 

When the left cell is in the EDIT status, the following assist keys are available for creating and scrolling the list. Then the pointer can be moved in the lengthwise direction. And pressing the knob changes the status to MOVE. 

Append Adds a line below. The same output value is entered into the right cell. Insert Adds a line above. The same output value is entered into the right cell. Delete Deletes the line. Scroll - 100 Moves the pointer to the maximum 100 points above. Scroll +100 Moves the pointer to the maximum 100 points below. Type Data type V (voltage) or I (current) CH Channel number 1 or 2, only on 2-channel models Points Number of data points Max Maximum value Min Minimum value 

- Load List Sweep Data dialog box 

This dialog box provides the following fields for loading a list sweep data from a file stored on a USB memory. Highlight a file and press OK to load the data and close the dialog box. 

(data graph) Displays the shape of the list sweep data selected by the file list (file list) List of the list sweep data file Path Folder into which the list sweep data file is saved Points Number of data points Max Maximum value Min Minimum value 

The following data can be loaded as the list sweep data. 

- Comma separated value format, file extension csv 

B2900B/BL Series User’s Guide, Edition 2 

119 

Front Panel Reference Display and Assist Keys 

For example, 

0.0,0.1,0.2,0.3 

- Carriage return or line feed separated value format, file extension txt For example, 

   - 0.0 

   - 0.1 

   - 0.2 0.3 

- Space separated value format in a line, file extension prn For example, 0.0 0.1 0.2 0.3 

120 

B2900B/BL Series User’s Guide, Edition 2 

Pulse : OM Peak: +05. 00000 ¥ Delay: OOL.2000ms Width: O25, 0000ms 

NOTE 

Trigger: MANTIAL souiice measure ee ~_ Count : 1 1 Delay : O.000 pe Oo.000 ps Period : 0.000 ps O.000 ps Trigger: auto AUTO 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->



<!-- Start of picture text -->
4 | ON OFF Yi Ift4i LINEAR 2; Vi¥) LOG Aut<br>amt aa ame3. ee eeah: ees se ScaleUt<br>H' T'' f} ii 4 ee ee ee u. DurnpScreen. |<br>f'i<br>;<br>Cursors<br>areAl Hide<br>a : CHL Yo ooo. co00mt<br>_itt ' su? Y ooo. c000my ch 2<br>-002.000nk , +010. 000m% +o10.000 ¥ Lirnit<br>MOVE LAN G<br>| Config Function Triqger Result File More...<br><!-- End of picture text -->

Front Panel Reference Display and Assist Keys 

Table 4-2 

X and Y Axes Data Types for Graph View 

|**Data type**|**Assist key**|**Description**|
|---|---|---|
|I (A)|AMPS (I)|Current measurement data|
|V (V)|VOLTS (V)|Voltage measurement data|
|R (Ω)|OHMS (R)|Resistance measurement data|
|P (W)|WATTS (P)|Power measurement data|
|MATH|MATH|Math result data|
|t (s)|TIME (t)|Time data. Only for the X-axis data.|
|V1|Ch 1 V (V1)|Only on 2-channel models. Voltage data of channel 1<br>|
|V2|Ch 2 V (V2)|~~o~~r 2. Only for the X-axis data.|



Assist keys Auto Scale Changes graph scale to fit the trace in the graph automatically. Dump Screen Opens the File Selection (Dump Screen) dialog box which is used to save the screen dump as a JPEG file. The file can be saved onto a USB memory connected to the front panel USB-A connector. The file is saved with the specified name. If the file extension is not specified, “.jpg” is added automatically. Show Cursors Displays the cursors (Y-cursors 1 and 2, X-cursors 1 and 2, and cursor data) and changes the softkey label to _Hide Cursors_ . Hide Cursors Hides the cursors and changes the softkey label to _Show Cursors_ . Ch _n_ Source Displays the channel _n_ source output value and changes the softkey label to _Ch n Limit_ . Ch _n_ Limit Hides the channel _n_ source output value and displays the limit value. Also changes the softkey label to _Hide Ch n_ . Hide Ch _n_ Hides the channel _n_ limit value and changes the softkey label to _Ch n Source_ . 

In the above descriptions, _Ch n_ indicates _Ch 1_ or _Ch 2_ . 

B2900B/BL Series User’s Guide, Edition 2 

125 





<!-- Start of picture text -->
Il: -O60.000n8 IZ: -O40.000p8 AI: OF0.000pa8 scaleuto<br>4 | CH1L I 50pa/tl: OO2.286msdiv. CHetz: “I OOF.786msoma div.Ar: 500.00S500ps/div. Ops Aut |<br>: ' ee Screeis<br>lieieaieat aieineineiat aieetieteet oleteteiet oats Selet oats: tetel oieietetelsieietetelsiietetetenietene me) 11°01Line 2<br>a<br>ee Lirnit<br>i CHIL Vo O00. 0000mt<br>' CH? Vo 000. oooomy¥ Ch 2<br>036.000 pe a o50.000pu 4alz -O10.000m4 Oo05.036ms Lirnit<br>LAN R<br>Config Function Trigger Result File More...<br><!-- End of picture text -->

Front Panel Reference Display and Assist Keys 

- Display area 1. Displays status ON or OFF, on B2911B 

Displays status Ch 1, Ch 2, or OFF, on B2912B 

Two lines can be displayed on the graph. [1] indicates the graph setup for line 1. [2] indicates the graph setup for line 2. 

2. Y-axis data type I, V, R, or P. 

3. Y-axis scale per division A/div., V/div., Ω/div., or W/div. 

4. X-axis scale per division s/div. 

5. X-axis minimum value (minimum timestamp) 

6. Y-axis offset values for line 1 and 2 

7. X-axis maximum value (maximum timestamp) 

8. Channel 1 and/or 2 source output value, limit value, or none (controlled by the Ch _n_ Source, Ch _n_ Limit, or Hide Ch _n_ assist key) 

9. Channel 1 and/or 2 Y-axis data at the active X-cursor position. ----.---- is displayed for the _no-data_ position. 

10. Cursor data (controlled by the Line 1 Cursors, Line 2 Cursors, or Hide Cursors assist key) 

   - First line Positions and distance (e.g. I1, I2, ΔA) of Y-cursors 1 and 2 Second line Positions and distance (e.g. t1, t2, Δt) of X-cursors 1 and 2 

Table 4-3 

Y-Axis Data Types for Roll View 

|**Data type**|**Assist key**|**Description**|
|---|---|---|
|I|AMPS (I)|Current measurement data|
|V|VOLTS (V)|Voltage measurement data|
|R|OHMS (Ω)|Resistance measurement data|
|P|WATTS (P)|Power measurement data|



B2900B/BL Series User’s Guide, Edition 2 

127 

Front Panel Reference Display and Assist Keys 

Assist keys Auto Scale Changes graph scale to fit the trace in the graph automatically. Dump Screen Opens the File Selection (Dump Screen) dialog box which is used to save the screen dump as a JPEG file. 

The file can be saved onto a USB memory connected to the front panel USB-A connector. The file is saved with the specified name. If the file extension is not specified, “.jpg” is added automatically. 

Line 1 Cursors Displays the cursors (Y-cursors 1 and 2, X-cursors 1 and 2, and cursor data) for line 1, and changes the softkey label to _Line 2 Cursors_ . Line 2 Cursors Displays the cursors (Y-cursors 1 and 2, X-cursors 1 and 2, and cursor data) for line 2, and changes the softkey label to _Hide Cursors_ . Hide Cursors Hides the cursors and changes the softkey label to _Line 1 Cursors_ . Ch _n_ Source Displays the channel _n_ source output value and changes the softkey label to _Ch n Limit_ . Ch _n_ Limit Hides the channel _n_ source output value and displays the limit value. Also changes the softkey label to _Hide Ch n_ . Hide Ch _n_ Hides the channel _n_ limit value and changes the softkey label to _Ch n Source_ . 

In the above descriptions, _Ch n_ indicates _Ch 1_ or _Ch 2_ . 

128 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Display and Assist Keys 

###### Status Information 

The status information is common for all display modes. It is displayed above the bottom softkeys associated with the function keys. 



Table 4-4 

Status Indicator 

|**Label**|**Color**|**Description**|
|---|---|---|
|AUTO|white|Trigger auto. Automatic trigger is enabled now.|
|ARM|white|Trigger active. Trigger system is active now.|
|HV|yellow|High voltage. Output voltage setting is over±42 V.|
|||Channel 1 floating status. Channel 1 is not grounded.|
|||Channel 2 floating status. Channel 2 is not grounded.|
|REM|white|Remote. Instrument is in the remote state.|
|||Local lockout.<br>Instrument is in the local lockout (LLO) status.|
|ERR|white|Error. At least one error is detected.|
|EDIT|green|Edit mode. The field specified by the pointer can be edited.<br>Not in the move mode.|
|MOVE|blue|Move mode. The field pointer can be moved. Not in the edit<br>mode.|
|LAN|green or<br>red|LXI LAN status indicator. Green indicates normal LAN status.<br>Red indicates abnormal condition. Blinking indicates the LAN<br>identification status.|
|D|white|Display mode: Dual view|
|1|white|Display mode: Single view for channel 1|
|2|white|Display mode: Single view for channel 2|
|G|white|Display mode: Graph view|
|R|white|Display mode: Roll view|



B2900B/BL Series User’s Guide, Edition 2 

129 

Front Panel Reference Function Keys 

#### Function Keys 

Keysight B2900 has six function keys below the front panel display, and provides the following softkeys. 

Function keys 1 Config SMU configuration setup. Displays the softkeys for setting several functions of the SMU. See “Config Key Group” on page 132. Function Math, limit test, and trace function setup. Displays the softkeys for setting these functions. See “Function Key Group” on page 138. Trigger Trigger configuration and control. Displays the softkeys for setting and controlling the trigger system. See “Trigger Key Group” on page 143. Result Measurement, limit test, and trace result display. Displays the softkeys for displaying these results. See “Result Key Group” on page 147. File File operation. Displays the softkeys for saving and loading a file. See “File Key Group” on page 150. More... Changes the function keys to Function keys 2. Function keys 2 Program Program configuration and control. Displays the softkeys for setting and controlling the program memory. See “Program Key Group” on page 151. I/O I/O setup. Displays the softkeys for setting the I/O interfaces. See “I/O Key Group” on page 152. Display Display setup. Displays the softkeys for setting the display functions. See “Display Key Group” on page 158. System System setup. Displays the softkeys used for several system setups. See “System Key Group” on page 160. LED Demo For demonstration. If a LED is connected between the High Force and Low Force terminals, pressing OK starts the LED IV measurement demonstration. 

130 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Function Keys 

More... 

Changes the function keys to Function keys 1. 

B2900B/BL Series User’s Guide, Edition 2 

131 

Front Panel Reference Config Key Group 

#### Config Key Group 

Pressing the Config key displays the following softkeys for setting several functions of the SMU. 

Source Displays the following softkeys for setting the SMU source operation. 

Connection Channel operation and connection setup. See “Output Connection Dialog Box” on page 134. Filter Output filter setup. See “Output Filter Dialog Box” on page 133. Sweep Sweep source detail setup. See “Sweep Dialog Box” on page 135. 

Measure Displays the following softkeys for setting the SMU measurement operation. R Compen Resistance compensation ON or OFF. See “Resistance Compensation” on page 213. 

On a 1-channel model, pressing this key displays the ON and OFF keys used to set the resistance compensation on or off. On a 2-channel model, pressing this key displays the ALL, Ch 1, and Ch 2 keys. They are used to specify the channel for setting the resistance compensation on or off. 

ALL specifies channels 1 and 2. Ch 1 specifies channel 1 only. Ch 2 specifies channel 2 only. Ranging Measurement range detail setup. See “Ranging Dialog Box” on page 136. 

Common Displays the following softkeys for setting the SMU’s miscellaneous function. Wait Source and measurement wait time setup. See “Wait Control Dialog Box” on page 137. 

132 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Config Key Group 

Group 

Only on 2-channel models. Channel grouping ON or OFF. If this function is ON, the channels perform synchronous channel operation. The present setting is indicated by an asterisk on the softkey label. 

Save, Recall Saves/recalls all setup information of the B2900. Five storage areas are available in the built-in non-volatile memory (NVRAM) for this purpose. 

Press the Save or Recall softkey to display the softkeys #1, #2, #3, #4, and #5 used for selecting the storage area. Press a softkey to save or recall the setup information. 

###### Output Filter Dialog Box 

This dialog box provides the following parameters for setting the output filter. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Filter State Output filter ON or OFF Set the filter ON to obtain clean source output without spikes and overshooting. Note, however, that using a filter may increase the SMU settling time. 

###### Automatic Filter Automatic filter ON or OFF 

Set the function ON to automatically set the output filter which provides the best filter characteristics and cutoff frequency. Time Constant Filter time constant, 5 μs to 500 μs 

B2900B/BL Series User’s Guide, Edition 2 

133 

Front Panel Reference Config Key Group 

###### Output Connection Dialog Box 

This dialog box provides the following parameters for setting the channel operation and connection. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Sensing Type 2-wire connection (2-WIRE) or 4-wire connection (4-WIRE). Set 4-WIRE for remote sensing. Low Terminal State Low sense terminal connection. Grounded (GROUND) or floating (FLOAT) High Capacitance Mode High capacitance mode ON or OFF Set the mode to ON to perform high capacitive load measurements. See “High Capacitance Mode” on page 212. Over Voltage/Current Protection Over voltage or current protection ON or OFF Set the function ON to turn off the channel output automatically and immediately when it reaches its compliance. Output-Off State High impedance (HIGH Z), normal (NORMAL), or zero volt (ZERO) This is the source setup condition after output off. See Table 6-1 on page 210. Auto Output-On Automatic output on function ON or OFF Set the function ON to turn on the channel output automatically just before the trigger system is initiated by the SCPI command (not by front panel operation). Auto Output-Off Automatic output off function ON or OFF Set the function ON to turn off the channel output automatically and immediately when all trigger system changes the status from busy to idle. 

134 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Config Key Group 

###### Sweep Dialog Box 

This dialog box provides the following parameters for setting details of the sweep source operation. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Sweep Ranging Ranging mode used for the sweep source operation. BEST, AUTO, or FIXED. See Table 4-5 on page 135. 

Sweep Direction UP (start to stop direction) or DOWN (stop to start direction) 

Output after Sweep The value the source channel applies after it completes the sweep output 

START VALUE (START): The source channel changes the output to the DC output value just before applying the sweep output start value. 

END VALUE (END): The source channel keeps the output value when the sweep output is stopped. 

Table 4-5 

Ranging Mode used for Sweep Source Operation 

||**Description**|
|---|---|
|BEST|In the linear sweep mode, the sweep source channel automatically<br>uses the minimum range which covers the whole sweep output.<br>In the log sweep mode, the sweep source channel automatically uses<br>the range which provides the best resolution for each sweep step<br>output.|
|AUTO|The sweep source channel automatically changes and sets the range<br>which provides the best resolution to apply the source output for each<br>sweep step.|
|FIXED|The sweep source channel sets the range specified by the range<br>parameter_Source Volts: Spot_or_Source Amps: Spot_. See“Range<br>parameters” on page 116.|



B2900B/BL Series User’s Guide, Edition 2 

135 

Front Panel Reference Config Key Group 

###### Ranging Dialog Box 

This dialog box provides the following parameters for setting details of the auto ranging operation for measurements. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Current Auto Ranging Current measurement auto range operation. NORMAL, SPEED, or RESOLUTION (RESOLN). See Table 4-6. Voltage Auto Ranging Voltage measurement auto range operation. NORMAL, SPEED, or RESOLUTION (RESOLN). See Table 4-6. Threshold Sets the _rate_ value of the formula shown below. 

Table 4-6 

Measurement Auto Range Operation 

||**Description**|
|---|---|
|NORMAL|Supports basic operation and downward changing operation described<br>below|
|SPEED|Supports basic operation and upward and downward changing<br>operations described below|
|RESOLN|Supports basic operation and upward changing operation described<br>below|



- Basic operation 

The channel automatically sets the range which provides the best resolution in performing the measurement. 

- Upward changing operation 

If measured data ≥ _value1_ , the range changes up after the measurement. 

_value1_ = measurement range × _rate_ / 100 

- Downward changing operation 

If measured data ≤ _value2_ , the range changes down immediately. 

_value2_ = measurement range × _rate_ / 1000 

136 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Config Key Group 

###### Wait Control Dialog Box 

This dialog box provides the following parameters for setting the source and measurement wait time. 

The source wait time is defined as the time the source channel cannot change the output after the start of a DC output or the trailing edge of a pulse. 

The measurement wait time is defined as the time the measurement channel cannot start measurement after the start of a DC output or the trailing edge of a pulse. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. State Wait time ON or OFF Automatic Automatic wait time ON or OFF 

Gain and Offset Parameters for calculating the wait time. See the following formula. 

- If State = ON and Automatic = ON: 

wait time = _Gain_ × initial wait time + _Offset_ 

- If State = ON and Automatic = OFF: 

wait time = _Offset_ 

- If State = OFF: 

wait time = 0 

The initial wait time is automatically set by the instrument, and cannot be changed. 

B2900B/BL Series User’s Guide, Edition 2 

137 

Front Panel Reference Function Key Group 

#### Function Key Group 

Pressing the Function key displays the following softkeys for setting the math, limit test, and trace functions. 

- Math Math function setup. See “Math Expression Dialog Box” on page 138. 

Limit Test Displays the following softkeys for setting the limit test function. The limit test is not available for B2901BL. 

Composite Composite limit test setup. See “Composite Limit Test Setup Dialog Box” on page 139. Limits Limit test setup. See “Limit Test Setup Dialog Box” on page 140. 

Trace Trace function setup. See “Trace Buffer Setup Dialog Box” on page 141. 

###### Math Expression Dialog Box 

This dialog box provides the following parameters for setting the math function. If the math function is ON, the measurement data can be calculated by using the specified math expression. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Status Math function ON or OFF Unit String Unit for the calculation result of the math expression 

The available math expressions are listed in the area below the Unit String field. This area can be used to select the math expression for data calculation. To select a math expression, highlight the name in this area. 

The data calculation is effective for data measured after the math function is set by this dialog box. The calculation result can be displayed on the dialog box opened by using the Result key group. See “Result Key Group” on page 147. 

Math expressions can be defined by using the SCPI commands while the B2900 is in the remote mode. 

138 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Function Key Group 

For the predefined math expressions, see “Predefined Math Expressions” on page 214. 

###### Composite Limit Test Setup Dialog Box 

This dialog box provides the following parameters for setting the limit test. The limit test is not available for B2901BL. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Limit Test Composite limit test ON or OFF Mode Operation mode. GRADING (GRADE) or SORTING (SORT) GRADING: Grading mode. See Figure 6-5 for the operation. SORTING: Sorting mode. See Figure 6-6 for the operation. Auto Clear Automatic clear of the composite limit test result ON or OFF 

If this parameter is ON, the composite limit test results and the DIO lines are automatically cleared. 

Update Only for the GRADING mode. Test result output timing, IMMEDIATE (IMM.) or END. See “Immediate?” shown in Figure 6-5. 

IMMEDIATE: Output after every test (Immediate? Yes) END: Output after the last test (Immediate? No) 

Offset Cancel Offset cancel ON or OFF 

If this parameter is ON, the data for the limit test judgement will be as follows. 

Data for judgement = measurement data - offset value 

Offset Offset value used for the offset cancel, - 9.999999E+20 to +9.999999E+20 

Pass Pattern Bit pattern for the limit test _pass_ state. For the GRADING mode. Fail Pattern Bit pattern for the limit test _fail_ state. For the SORTING mode. GPIO Pins Displays the DIO pins assigned to the bit pattern output. To set the pins, use the :CALC:DIG:BIT command. 

/BUSY 

DIO pin assigned to the BUSY (busy) signal output 

B2900B/BL Series User’s Guide, Edition 2 

139 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Front Panel Reference Function Key Group 

COMPLIANCE: Compliance check LIMIT: Limit test 

Pass Pattern Bit pattern for the limit test _pass_ state. For the SORTING mode. Fail on Only for compliance check. OUT or IN. Fail on = IN judges limit test failure if the channel goes into the compliance state. 

Fail on = OUT judges limit test failure if the channel comes out of the compliance state. Fail Pattern Only for compliance check. Bit pattern for the limit test _fail_ state. Up Pattern Not available for compliance check. Bit pattern for the _failed-by-exceeding-upper-limit_ state. Used for the GRADING mode. Up Limit Not available for compliance check. Upper limit for the pass/fail judgement. Used for the GRADING mode. Low Pattern Only for limit test. Bit pattern for the _failed-by-exceeding-lower-limit_ state. Used for the GRADING mode. Low Limit Only for limit test. Lower limit for the pass/fail judgement. Used for the GRADING mode. 

Bit pattern is sent to the DIO pins specified by the GPIO Pins field on the Composite Limit Test Setup dialog box. 

###### Trace Buffer Setup Dialog Box 

This dialog box provides the following parameters for setting the trace function. The data specified by the Feed Data parameter can be stored in the trace buffer if the Buffer Control parameter is NEXT. The maximum data size is specified by the Buffer Size parameter. See Figure 6-7 on page 221 for the trace buffer. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Feed Data Type of data placed in the trace buffer. SENSE, MATH, or LIMIT SENSE: Measurement result data 

B2900B/BL Series User’s Guide, Edition 2 

141 

Front Panel Reference Function Key Group 

MATH: Calculation result data of math expression 

LIMIT: Limit test data 

The data contains the voltage measurement data, current measurement data, resistance measurement data, source output setting data, calculation result data, limit test data, time data, or status data selected by using the Format keys of the I/O key group. See “I/O Key Group” on page 152. Buffer Control Trace buffer control mode. NEVER or NEXT NEVER: Disables the write operation to the trace buffer. NEXT: Enables the write operation until buffer full. Buffer full will change the mode to NEVER. Buffer Size Size of the trace buffer. 1 to 100000 data for B2910BL, B2901B, B2902B, B2911B, and B2912B; 1 to 10000 data for B2901BL. 

142 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Trigger Key Group 

#### Trigger Key Group 

Pressing the Trigger key displays the following softkeys for setting the trigger parameters in detail and controlling the trigger system. See Figure 6-8 on page 226. 

Config Opens a dialog box. See “Trigger Configuration Dialog Box” on page 144. 

   - Initiate Displays the softkeys for selecting the device action to initiate (to go to the arm layer of the trigger system). See Table 4-7. 

   - Abort Displays the softkeys for selecting the device action to abort (to return to the idle state of the trigger system). See Table 4-7. 

- Immediate Displays the following softkeys for selecting the arm layer or trigger layer for sending the immediate trigger. 

Trigger Selects the trigger layer. Arm Selects the arm layer. 

Selecting the layer displays the softkeys for selecting the device action to send the immediate trigger. See Table 4-7. 

Table 4-7 

Softkeys to Select Device Action and Channel 

|**Softkey label**|**Description**|
|---|---|
|ALL|Selects the transient and acquire device actions.|
|Trans.|Selects the transient (source output) device action only.|
|Acq.|Selects the acquire (measurement) device action only.|



On a 1-channel model, selecting the device action executes Initiate, Abort, or Immediate for the specified device action. 

On a 2-channel model, selecting the device action displays the following softkeys for selecting the channel. 

|ALL|Selects channels 1 and 2.|
|---|---|
|Ch 1|Selects channel 1 only.|



B2900B/BL Series User’s Guide, Edition 2 

143 

Front Panel Reference Trigger Key Group 

|**Softkey label**||**Description**|
|---|---|---|
|Ch 2|Selects channel 2 only.||



Selecting the channel executes Initiate, Abort, or Immediate for the specified device action of the specified channel. 

###### Trigger Configuration Dialog Box 

This dialog box is used to set the trigger parameters in detail. Note that the overlapped parameter values are ignored and replaced with the settings made on the Single view. See “Trigger parameters” on page 122. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Layer Specifies the layer or device action set by this dialog box. ARM: Arm layer TRIGGER: Trigger layer ACTION: Device action For ACTION, the available parameters are Ch, Layer, Action, and Trigger Output. Action Specifies the device action type set by this dialog box. TRANS.: Transient (source output) device action ACQ.: Acquire (measurement) device action Count Count for the action specified by the Ch, Layer, and Action parameters. 1 to 100000 or INF. (infinity) for B2901B, B2902B, B2911B, and B2912B; 1 to 10000 for B2901BL; and 1 to 100000 for B2910BL. Bypass Bypass ON or OFF Bypass = ON enables the bypass only for the first passage to the event detector for the action specified by the Ch, Layer, and Action parameters. 

Bypass = OFF disables the bypass. 

144 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Trigger Key Group 

Trigger Source Event for the action specified by the Ch, Layer, and Action parameters. AUTO, BUS, TIMER, INT1, INT2, LAN, EXT1, EXT2, EXT3, EXT4, EXT5, EXT6, EXT7, EXT8, EXT9, EXT10, EXT11, EXT12, EXT13, or EXT14. See Table 4-8. 

Period Only for the TIMER event. 

Interval of the TIMER event for the action specified by the Ch, Layer, and Action parameters, 20 μs to 100000 s for B2901B and B2902B; 10 μs to 100000 s for B2911B and B2912B; 200 μs to 100000 s for B2901BL; and 50 μs to 100000 s for B2910BL. 

Trigger Delay Delay time for the action specified by the Ch, Layer, and Action parameters, 0 to 100000 s Trigger Output Trigger output ON or OFF Trigger output terminal. Internal bus (INT1, INT2), LAN port (LAN), or a digital I/O pin EXT _n_ ( _n_ = 1 to 14) 

If this parameter is ON, B2900 sends an output trigger when it changes the trigger state for the action specified by the Ch, Layer, and Action parameters. See Figure 6-8 on page 226. 

For using a digital I/O pin, it is necessary to set function of the pin, output trigger polarity, type, pulse width, and so on. See “DIO Configuration Dialog Box” on page 155 and “Using Digital I/O” on page 101. 

Table 4-8 

Trigger Source 

|**Trigger Source**|**Description**|
|---|---|
|AUTO|Trigger source best suited for the present operating mode is<br>automatically selected by the internal algorithms.|
|BUS|Remote interface trigger command such as GET, TRIGger, and *TRG|
|TIMER|Signal internally generated every interval set by the Period field|
|INT1 or INT2|Signal from the internal bus 1 or 2|



B2900B/BL Series User’s Guide, Edition 2 

145 

Front Panel Reference Trigger Key Group 

|**Trigger Source**|**Description**|
|---|---|
|LAN|LXI trigger specified by the :ARM[:ACQ|:TRAN]:SOUR:LAN and<br>:TRIG[:ACQ|:TRAN]:SOUR:LAN commands|
|EXT_n_|Signal from the DIO pin_n_, which is an I/O port of the Digital I/O<br>D-sub connector on the rear panel._n_= 1 to 14|



146 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Result Key Group 

#### Result Key Group 

Pressing the Result key displays the following softkeys for displaying the measurement, limit test, and trace results. 

Measure Displays the measurement result. See “Measure Result Dialog Box” on page 147. 

Limit Test Displays the limit test result. See “Limit Test Result Dialog Box” on page 148. The limit test is not available for B2901BL. 

Trace Displays the trace result. See “Trace Statistical Result Dialog Box” on page 149. 

###### Measure Result Dialog Box 

This dialog box is used to display the last single (one shot) measurement result stored in the data buffer. The maximum data is 100000 data for B2900 except B2901BL and 10000 data for B2901BL. 

This dialog box provides the following GUI for displaying the measurement result. The result data is shown in the index and data table below the Type field. Also, the data is plotted in the graph area above the Points fields. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel of the data to display. Type Type of data to display. AMPS, VOLTS, OHMS, WATTS, MATH, or TIME 

AMPS: Current measurement data VOLTS: Voltage measurement data OHMS: Resistance data given by Resistance = Vmeas/Imeas WATTS: Power data given by Power = Vmeas×Imeas MATH: Calculation result data of math expression TIME: Time data 

In the above formula, Vmeas is the voltage measurement data, and Imeas is the current measurement data. 

B2900B/BL Series User’s Guide, Edition 2 

147 

Front Panel Reference Result Key Group 

For details on using resistance compensation, see “Resistance Compensation” on page 213. Points Number of data points Max. Y-axis maximum value of the graph Min. Y-axis minimum value of the graph 

###### Limit Test Result Dialog Box 

This dialog box provides the following GUI for displaying the limit test result. The result data is shown in the data list area below the Length field. The limit test is not available for B2901BL. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel of the data to display. Length Data length The limit test data contains the following information. (aaaaa) BIN: bb DATA: +c.ccccccE+dd (aaaaa) Data index aaaaa BIN: Bin number bb ( 01 to 12 ) The bin numbers 1 to 12 are associated with the index numbers 1 to 12. See “Limit Test Setup Dialog Box” on page 140. If the limit test data is out of the bins, 00 is set for the GRADING mode and 15 is set for the SORTING mode. DATA: Limit test data +c.ccccccE+dd 

148 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Result Key Group 

###### Trace Statistical Result Dialog Box 

This dialog box provides the following GUI for displaying the trace statistical result. The result data is displayed in the Mean, Std. Dev., Min., and Max. fields. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel of the data to display. Feed Always SENSE Element Type of data to display. AMPS, VOLTS, or OHMS AMPS: Current measurement data VOLTS: Voltage measurement data OHMS: Resistance data given by Resistance = Vmeas/Imeas 

In the above formula, Vmeas is the voltage measurement data, and Imeas is the current measurement data. 

For details on using resistance compensation, see “Resistance Compensation” on page 213. Length Data length Mean Mean value Std. Dev. Standard deviation Min. Minimum value Max. Maximum value 

B2900B/BL Series User’s Guide, Edition 2 

149 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Front Panel Reference Program Key Group 

#### Program Key Group 

Pressing the Program key displays the following softkeys for setting and controlling the program memory. 

The program memory can be defined by using the SCPI commands while the B2900 is in the remote mode. 

- Catalog Displays the Program Catalog dialog box which lists the programs saved in the program memory. This dialog box is also used to specify the memory program to use. To select a memory program, highlight its name in this list. 

   - View Displays the Program View dialog box which shows the program code of the specified program. 

- Variable Displays the Variable dialog box which lists the variables used in the memory programs. Available index is 1 to 100. 

Control Displays the following softkeys for controlling the operation of the program memory. 

Run Starts the specified memory program. Pause Pauses the execution of the memory program. Step Starts the step execution of the specified memory program. Stop Stops the execution of the memory program. Continue Continues the execution of the memory program presently paused. 

B2900B/BL Series User’s Guide, Edition 2 

151 

Front Panel Reference I/O Key Group 

#### I/O Key Group 

Pressing the I/O key displays the following softkeys for setting the I/O interfaces. 

- Format Displays the softkeys for setting the data output format. See “Data Output Format” on page 153. 

LAN Displays the following softkeys for managing the LAN interface. Config Displays the LAN Configuration dialog box used to set the configuration of the LAN interface. See “LAN Configuration Dialog Box” on page 155. 

Status Displays the LAN Status dialog box which displays the status of the LAN interface. Reset Resets all of the LAN connections. Defaults Sets the LAN settings to the factory default settings. 

Pressing the Reset softkey or the Defaults softkey displays a confirmation dialog box. Press the OK softkey to confirm the specified action, or press the Cancel/Local key to cancel the action. 

USB Displays the USB Status dialog box. 

Easy File Access File access function setup ON (enable) or OFF (disable). When this is set to ON, restart this instrument. When you use this function, connect this instrument to a PC using a USB cable. See “File Access Function (Easy File Access)” on page 230. This function is not available for B2901BL and B2910BL. 

Text display field Displays the VISA USB connect string. 

Example: USB0::10893::38145::MY12345678::0::INSTR 

GPIB Displays the GPIB Configuration dialog box used to set the GPIB address of the B2900. This dialog also displays the VISA GPIB connect string. 

Example: GPIB0::23::INSTR 

DIO Displays the following softkeys for managing the Digital I/O interface. 

152 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference I/O Key Group 

Config Displays the DIO Configuration dialog box used to set the configuration of the Digital I/O interface. See “DIO Configuration Dialog Box” on page 155. R/W Displays the DIO Read/Write dialog box used to read or write a value set to the Digital I/O interface. See “DIO Read/Write Dialog Box” on page 156. 

###### Data Output Format 

Pressing the Format softkey displays the following softkeys for setting the format and elements of the output data. 

Measure Displays the Format (Measure) dialog box used to set the elements of the measurement data output. See “Format (Measure) Dialog Box” on page 154. Math/Limit Displays the Format (Math/Limit) dialog box used to set the elements of the math result data output and the limit test result data output. See “Format (Math/Limit) Dialog Box” on page 154. Trace Displays the Format (Trace) dialog box used to set the elements of the trace data output. See “Format (Trace) Dialog Box” on page 154. Data Type Displays the following softkeys for specifying the data output format. The present setting is indicated by an asterisk on the softkey label. ASCII ASCII format REAL32 IEEE-754 single precision format, 4-byte REAL64 IEEE-754 double precision format, 8-byte Data Swap Displays the following softkeys for enabling or disabling the byte swap for the IEEE-754 precision format data output. The present setting is indicated by an asterisk on the softkey label. OFF Disables the byte swap. Normal byte order. ON Enables the byte swap. Reverse byte order. 

If Data Swap = ON, the byte order is reversed. Byte 1 to byte 4 are sent in the order of byte 4 to byte 1 for the IEEE-754 single precision format, and byte 1 to byte 8 are sent in the order of byte 8 to byte 1 for the IEEE-754 double precision format. 

B2900B/BL Series User’s Guide, Edition 2 

153 

Front Panel Reference I/O Key Group 

###### Format (Measure) Dialog Box 

This dialog box provides the following parameters for setting the elements of the measurement data output. 

Voltage Voltage data output ON or OFF Current Current data output ON or OFF Resistance Resistance data output ON or OFF Source Source data output ON or OFF Time Time data output ON or OFF Status Status data output ON or OFF 

###### Format (Math/Limit) Dialog Box 

This dialog box provides the following parameters for setting the elements of the math result data output and the limit test result data output. 

Data Result data output ON or OFF Time Time data output ON or OFF Status Status data output ON or OFF 

###### Format (Trace) Dialog Box 

This dialog box provides the following parameters for setting the elements of the trace statistical data output. 

Ch Only on 2-channel models. Channel 1 (Ch 1) or 2 (Ch 2) This field specifies the channel set by this dialog box. Data Trace data. Selects one from the following data elements. MEAN Mean value STD.DEV. Standard deviation MIN. Minimum value MAX. Maximum value PK-PK Peak to peak value Timestamp Timestamp data format. Selects one from the following selections. 

154 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference I/O Key Group 

ABSOLUTE Absolute value (ABS.). Returns the incremental value for the first timestamp data. DELTA Delta value (DELTA). Returns the incremental value for the previous timestamp data. 

###### LAN Configuration Dialog Box 

This dialog box provides the following parameters for setting the configuration of the LAN interface. 

mDNS mDNS (multicast DNS) status ON or OFF 

IP Address Config. IP address configuration. AUTO or MANUAL AUTO configuration uses the DHCP server. IP Address IP address of this instrument, used for the MANUAL IP address configuration Subnet Subnet mask, used for the MANUAL IP address configuration Gateway IP address of gateway, used for the MANUAL IP address configuration DNS Server Config. DNS server configuration. AUTO or MANUAL DNS Server IP address of DNS server, primary and secondary, used for the MANUAL DNS server configuration Hostname Host name of this instrument WINS Server IP address of WINS server, primary and secondary 

###### DIO Configuration Dialog Box 

This dialog box provides the following parameters for setting the configuration of the Digital I/O interface. 

Pin # Digital I/O pin number, 1 to 14 

This field specifies the pin set by this dialog box. 

B2900B/BL Series User’s Guide, Edition 2 

155 

Front Panel Reference I/O Key Group 

Function Function of the specified pin of the Digital I/O interface, digital signal input/output (DIGITAL I/O), digital signal input (DIGITAL IN), trigger output (TRIGGER OUT), trigger input (TRIGGER IN), or high voltage state (HIGH VOLTAGE LAMP, HI-VOLT LAMP, only for pin 14) 

If Function is set to HIGH VOLTAGE LAMP, other setup parameters on this dialog box is ignored. Polarity Polarity of the input/output function. Positive (POSITIVE, POS.) or negative (NEGATIVE, NEG.) Output Trigger Type Type of output trigger, edge (EDGE) or level (LEVEL) Output Trigger Timing Timing of the output trigger, after action (arm, trigger, and device action) (AFTER), before action (BEFORE), or both (BOTH) 

Output Pulse Width Pulse width of the output trigger, 10 μs to 10 ms For more information about the trigger function, see “Trigger Key Group” on page 143. 

###### DIO Read/Write Dialog Box 

This dialog box provides the following parameters for reading or writing a value set to the Digital I/O interface. 

Format Format of the value set to the Mask Value field and the Value field, binary (BIN), decimal (DEC), or hexadecimal (HEX) Mask Value Mask value which indicates the pattern of the un-used bits of the Digital I/O interface 

The READ assist key is used to read the mask value presently set to the Digital I/O interface. 

The WRITE assist key is used to set the specified mask value to the Digital I/O interface. 

Value Value set to the Digital I/O interface The READ assist key is used to read the value presently set to the Digital I/O interface. 

156 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference I/O Key Group 

The WRITE assist key is used to write the specified value to the Digital I/O interface. 

B2900B/BL Series User’s Guide, Edition 2 

157 

Front Panel Reference Display Key Group 

#### Display Key Group 

Pressing the Display key displays the following softkeys for setting the display functions. The present setting is indicated by an asterisk on the softkey label. 

- Remote Displays the following softkeys for enabling or disabling the front panel display when the B2900 is in the remote mode. 

OFF Disables the front panel display. Effective for fast operation. ON Enables the front panel display. 

Color Displays the following softkeys for specifying the display color set. Set 1 Sets the color set 1. Set 2 Sets the color set 2. 

Zoom Displays the following softkeys for enabling or disabling the display zoom. OFF Disables the zoom. Normal display. ON Enables the zoom. Only the measured data is displayed. Zoom-in can be canceled by pressing the Zoom Out assist key. 

In the zoom-in state, setup information is not displayed, and the measurement result is zoomed. Then, 

- the Dual view displays the primary measurement data with a large font, and the secondary measurement data for each channel with a small font. 

- the Single view displays the primary and secondary measurement data with a large font. 

Digits Displays the following softkeys for specifying the data display resolution. 

4 Sets 3½ digit resolution. 5 Sets 4½ digit resolution. 6 Sets 5½ digit resolution. 7 Sets 6½ digit resolution. 

158 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference Display Key Group 

Pref. Displays the Display Preference dialog box. See “Display Preference Dialog Box” on page 159. 

###### Display Preference Dialog Box 

This dialog box is used to set the following parameters. The settings are saved to the non-volatile memory. 

- V/I Limit for Each Polarity 

Selects the Limit (Compliance) setup operation. 

ON Individual setup ON Positive and negative limits can be set individually. OFF Individual setup OFF ± Limit value is set. 

- Immediate V/I Update by Knob 

Selects the channel operation when the Source value or the Limit (Compliance) value is changed by using the digit pointer and the rotary knob. 

ON Real time update ON 

Turning the rotary knob changes the display value and updates the channel output in real time. 

OFF Real time update OFF Turning the rotary knob just changes the display value. The value is applied to the channel output by pressing the rotary knob. 

- Restore Trigger Mode on Local 

Stores the trigger mode setting on entering GP-IB remote state, ON or OFF. 

ON Saves the trigger mode setting in the non-volatile memory on entering GPIB remote state and restores the saved trigger mode setting when returning to GPIB local mode. OFF Disables this function. 

B2900B/BL Series User’s Guide, Edition 2 

159 

Front Panel Reference System Key Group 

#### System Key Group 

Pressing the System key displays the following softkeys used for several system setups. 

Error Displays the following softkeys to view or clear errors. Log Displays the Error Log dialog box which displays the SCPI errors. Clear Clears the error buffer immediately. 

Reset Initializes the B2900. Press the Reset softkey to display a confirmation dialog box and press one of the following softkeys to perform the initialization.To cancel the initialization and close a confirmation dialog box, press the Cancel/Local key. 

Display Initializes the display settings. H/W Initializes the hardware settings. ALL Initializes all settings. 

Cal/Test Displays the following softkeys for performing self-calibration or self-test. 

Self-Cal Performs self-calibration. Self-Test Performs self-test. 

Pressing the softkey displays a confirmation dialog box. Press the OK softkey to start self-calibration or self-test, or the Cancel/Local key to cancel. 

PLC Displays the following softkeys to specify the power line frequency. The present setting is indicated by an asterisk on the softkey label. 

50 Hz Specifies the power line frequency 50 Hz. 60 Hz Specifies the power line frequency 60 Hz. 

Timestamp Displays the following softkeys to clear timestamp. 

160 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Reference System Key Group 

Clear 

Auto CLR 

Clears the timestamp. Pressing this softkey displays the confirmation dialog box. Press the OK softkey to clear the timestamp, or the Cancel/Local key to cancel. 

Displays the following softkeys for setting the automatic clear of the timestamp. The present setting is indicated by an asterisk on the softkey label. 

OFF Disables the automatic clear of the timestamp. ON Enables the automatic clear of the timestamp. 

- Start-up Displays the System Start-up dialog box which provides the following setup parameters. 

Power-on State RST, RCL0, RCL1, RCL2, RCL3, or RCL4 

The power-on state can be selected from the factory default reset condition (RST) and user conditions RCL0, RCL1, RCL2, RCL3, and RCL4 which can be defined by using the *SAV 0, *SAV 1, *SAV 2, *SAV 3, and *SAV 4 commands, respectively, while the B2900 is in the remote mode. 

Power-on Program Power-on program execution ON or OFF 

If this parameter is ON, the power-on program is automatically executed when the B2900 is turned on. 

The power-on program can be defined by using the :PROG:PON:COPY command while the B2900 is in the remote mode. 

Sound Displays the following softkeys to enable or disable the beep and sound. The present setting is indicated by an asterisk on the softkey label. 

OFF Disables the beep and sound. ON Enables the beep and sound. 

Language Displays the following softkeys for specifying the B2900 remote control command set. The present setting is indicated by an asterisk on the softkey label. 

Default Specifies the default command set which supports all B2900 functions. 

B2900B/BL Series User’s Guide, Edition 2 

161 

Front Panel Reference System Key Group 

2400 Specifies the conventional command set designed for using the existing programs which you created to control existing instruments, such as Series 2400 by Keithley Instruments, Inc. Script Not effective. 

The default command set and the conventional command set are available. 

Info. Displays the following softkeys. 

Revision Displays the Revision dialog box which displays the model number, serial number, and firmware revision of the B2900. Date/Time Displays the Date and Time dialog box which is used to view or set the date and time. Update Used for firmware update. Demo. Performs demonstration. Pressing this softkey displays a confirmation dialog box. Press the OK softkey to start demonstration, or the Cancel/Local key to cancel. 

Factory Returns the B2900 to the condition when it is shipped from the factory. Pressing this softkey displays a confirmation dialog box. Press the OK softkey to start the initialization, or the Cancel/Local key to cancel. 

162 

B2900B/BL Series User’s Guide, Edition 2 







W KEYSIGHT 

NOTE 

NOTE 



NOTE 

Front Panel Operations To Use Functions Other Than Measurement 

#### To Use Functions Other Than Measurement 

This section describes the following miscellaneous operations other than the source output and measurement functions. 

- “To Set Power Line Frequency” 

- “To Return to Factory Shipment Condition” 

- “To Apply Initial Settings” 

- “To Set Beeper” 

- “To Set Date and Time” 

- “To Perform Self-Test” 

- “To Perform Self-Calibration” 

- “To Set Operations at Power On” 

- “To Display Error Message” 

- “To Clear Error Buffer” 

- “To Clear Timestamp” 

- “To Set Automatic Clear of Timestamp” 

- “To Display Firmware Revision” 

- “To Display License Information” 

- “To Set GPIB Address” 

- “To Set Remote Control Command Set” 

- “To Set Remote Display Mode” 

- “To Save/Recall All Setup Information” 

- “To Enable/Disable Media Transfer Protocol for Easy File Access” 

- “To Enable/Disable the Function to Restore the Trigger Mode Settings on the Local State” 

- “To Enable/Disable Source/Limit Real Time Update by Using Digit Pointer and Knob” 

B2900B/BL Series User’s Guide, Edition 2 

167 

Front Panel Operations To Use Functions Other Than Measurement 

###### To Set Power Line Frequency 

1. Press the More > System > PLC function keys. 

2. Press 50 Hz or 60 Hz. To cancel the setting change, press the Cancel/Local key. 

###### To Return to Factory Shipment Condition 

1. Press the More > System > More > Factory function keys. 

2. Press OK to apply. To cancel the operation, press the Cancel/Local key. 

###### To Apply Initial Settings 

1. Press the More > System > Reset function keys. 

The Confirmation dialog box and the following function keys appear. 

Display Initializes the display settings. H/W Initializes the hardware settings. ALL Initializes all settings. 

2. Press one of these keys. To cancel the operation, press the Cancel/Local key. 

###### To Set Beeper 

1. Press the More > System > More > Sound function keys. 

2. To enable the beeper, press ON. To disable it, press OFF. To cancel the setting change, press the Cancel/Local key. 

###### To Set Date and Time 

1. Press the More > System > More > Info. function keys. 

2. Press Date/Time. 

The Date and Time dialog box opens. 

3. Set the date and time. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

168 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Use Functions Other Than Measurement 

###### To Perform Self-Test 

1. Press the On/Off switch and confirm that the switch is turned off. 

2. Disconnect test leads and cables from the channel terminals. 

3. Press the More > System > Cal/Test > Self-Test function keys. 

   - The Confirmation dialog box opens. 

4. Press OK to perform the self-test. To cancel the operation, press the Cancel/Local key. 

###### To Perform Self-Calibration 

The self-calibration must be performed after warming-up of 60 minutes. 

1. Press the On/Off switch and confirm that the switch is turned off. 

2. Disconnect test leads and cables from the channel terminals. 

3. Press the More > System > Cal/Test > Self-Cal function keys. 

   - The Confirmation dialog box opens. 

4. Press OK to perform the self-calibration. To cancel the operation, press the Cancel/Local key. 

###### To Set Operations at Power On 

1. Press the More > System > More > Start-up function keys. 

The System Start-up dialog box opens. 

2. See “Start-up” on page 161 to set each parameter. 

###### To Display Error Message 

1. Press the More > System > Error function keys. 

2. Press Log. 

The error message is displayed in the Error Log dialog box. 

3. Press OK to close the dialog box. 

B2900B/BL Series User’s Guide, Edition 2 

169 

Front Panel Operations To Use Functions Other Than Measurement 

###### To Clear Error Buffer 

1. Press the More > System > Error function keys. 

2. To clear the error buffer, press Clear. To cancel the operation, press the Cancel/Local key. 

###### To Clear Timestamp 

1. Press the More > System > Timestamp function keys. 

2. Press Clear. The Confirmation dialog box opens. 

3. To perform the operation, press OK. To cancel the operation, press the Cancel/Local key. 

###### To Set Automatic Clear of Timestamp 

1. Press the More > System > Timestamp function keys. 

2. Press Auto CLR. 

3. To enable the automatic clear, press ON. To disable it, press OFF. To cancel the setting change, press the Cancel/Local key. 

###### To Display Firmware Revision 

1. Press the More > System > More > Info. function keys. 

2. Press Revision. 

The revision information is displayed in the Revision dialog box. 

Press OK to close the dialog box. 

###### To Display License Information 

Perform the following steps to display the license applied to the B2900. 

1. Press the More > System > More > Info. function keys. 

2. Press Revision. 

The license information is displayed in the Revision dialog box. 

Press OK to close the dialog box. 

170 

B2900B/BL Series User’s Guide, Edition 2 

NOTE 

Front Panel Operations To Use Functions Other Than Measurement 

###### To Save/Recall All Setup Information 

This function saves/recalls all setup information of the B2900. For this function, five storage areas are available in the built-in non-volatile memory (NVRAM). 

To save the setup information, do the following steps. 

1. Press the Config > Save function keys. 

2. Press a function key #1, #2, #3, #4, or #5 corresponding to the storage area. To recall the setup information, do the following steps. 

1. Press the Config > Recall function keys. 

2. Press a function key #1, #2, #3, #4, or #5 corresponding to the storage area which stores the setup information to recall. 

To Enable/Disable Media Transfer Protocol for Easy File Access 

For details on this function, see “File Access Function (Easy File Access)” on page 230. This function is not available for B2901BL and B2910BL. 

1. Press the More > I/O > USB function keys. The USB Status dialog box opens. 

2. Set the Easy File Access field to ON (enable) or OFF (disable). 

3. To apply the settings and close the dialog box, press OK. 

   - To cancel the setting change, press the Cancel/Local key. 

Changing the setting needs rebooting the B2900. 

To Enable/Disable the Function to Restore the Trigger Mode Settings on the Local State 

For details on this function, see “Display Preference Dialog Box” on page 159. 

1. Press the More > Display > Pref. function keys. The Display Preference dialog box opens. 

2. Set the Restore Trigger Mode on Local field to ON (enable) or OFF (disable). 

3. To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

172 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Use Functions Other Than Measurement 

###### To Enable/Disable Source/Limit Real Time Update by Using Digit Pointer and Knob 

For details on this function, see “Display Preference Dialog Box” on page 159. 

1. Press the More > Display > Pref. function keys. The Display Preference dialog box opens. 

2. Set the Immediate V/I Update by Knob field to ON (enable) or OFF (disable). 

3. To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

B2900B/BL Series User’s Guide, Edition 2 

173 

Front Panel Operations To Set Source Output 

#### To Set Source Output 

This section describes the following source output operations. 

- “To Set Source Output Mode” 

- “To Apply DC Voltage/Current” 

- “To Stop Source Output” 

- “To Set Limit (Compliance) Value” 

- “To Set Output Range” 

- “To Set Pulse Output” 

- “To Set Sweep Output” 

- “To Set List Sweep Output” 

- “To Set Source Output Trigger Parameters” 

- “To Set Source Wait Time” 

- “To Set Output Filter” 

- “To Set Connection Type” 

- “To Set Low Terminal State” 

- “To Enable or Disable High Capacitance Mode” 

- “To Enable or Disable Over Voltage/Current Protection” 

- “To Specify Output-Off Status” 

- “To Enable or Disable Automatic Output-On Function” 

- “To Enable or Disable Automatic Output-Off Function” 

- “To Set Ranging Mode of the Sweep Source” 

- “To Set Sweep Direction” 

- “To Set Source Output Value after Sweep” 

174 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Set Source Output 

###### To Set Source Output Mode 

1. For the Single view, press the Mode assist key. For the Dual view, press the Ch1 Mode or Ch2 Mode assist key. 

The field pointer moves to the Source mode setup field. 

2. Press VOLTS (V) for the voltage output or AMPS (I) for the current output. 

###### To Apply DC Voltage/Current 

1. For the Single view, press the Source assist key. For the Dual view, press the Ch1 Source or Ch2 Source assist key. 

The field pointer moves to the Source value setup field. 

2. Enter the output value by using the numeric/alpha keys, rotary knob, and arrow keys. 

3. Press the rotary knob or assist key to fix the setting value. 

4. Press the On/Off switch for channel 1 or 2 (Ch 1 or Ch 2). 

The channel starts the specified voltage/current output. While the switch is turned green, the channel continues the output and changes the value immediately if it is changed. 

###### To Stop Source Output 

1. Press the On/Off switch for channel 1 or 2 (Ch 1 or Ch 2). 

The output and measurement are stopped and the switch turns off. 

###### To Set Limit (Compliance) Value 

For details on this function, see “Limit/Compliance” on page 200. 

1. For the Single view, press the Limit assist key. For the Dual view, press the Ch1 Limit or Ch2 Limit assist key. 

The field pointer moves to the Limit (Compliance) field. 

2. Enter the limit value by using the numeric/alpha keys, rotary knob, and arrow keys. 

3. Press the rotary knob or assist key to fix the setting value. 

B2900B/BL Series User’s Guide, Edition 2 

175 

NOTE 

NOTE 

NOTE 

NOTE 

###### NOTE 

NOTE 

Front Panel Operations To Set Source Output 

The Wait Control dialog box opens. 

2. Set each parameter in the Source column. 

3. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Output Filter 

For details on this function, see “Output Filter” on page 208 and “Output Filter Dialog Box” on page 133. 

1. Press the Config > Source > Filter function keys. 

   - The Output Filter dialog box opens. 

2. Set each parameter. 

3. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Connection Type 

For details on this function, see “2-Wire Connections or 4-Wire Connections” on page 82. 

1. Press the Config > Source > Connection function keys. The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set 2-WIRE or 4-WIRE in the Sensing Type field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Low Terminal State 

For details on this function, see “Floating” on page 83. 

1. Press the Config > Source > Connection function keys. 

180 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Set Source Output 

The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set GROUNDED or FLOATING in the Low Terminal State field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Enable or Disable High Capacitance Mode 

For details on this function, see “High Capacitance Mode” on page 212. 

1. Press the Config > Source > Connection function keys. 

The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set ON (enable) or OFF (disable) in the High Capacitance Mode field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Enable or Disable Over Voltage/Current Protection 

For details on this function, see “Over Voltage/Current Protection” on page 209. 

1. Press the Config > Source > Connection function keys. 

The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set ON (enable) or OFF (disable) in the Over Voltage/Current Protection field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Specify Output-Off Status 

For details on this function, see “Output-Off Status” on page 210. 

B2900B/BL Series User’s Guide, Edition 2 

181 

Front Panel Operations To Set Source Output 

1. Press the Config > Source > Connection function keys. 

The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set HIGH Z (high impedance), NORMAL (normal), or ZERO (zero volt) in the Output-Off State field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Enable or Disable Automatic Output-On Function 

For details on this function, see “Automatic Output-ON/OFF Function” on page 211. 

1. Press the Config > Source > Connection function keys. 

The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set ON (enable) or OFF (disable) in the Auto Output-On field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Enable or Disable Automatic Output-Off Function 

For details on this function, see “Automatic Output-ON/OFF Function” on page 211. 

1. Press the Config > Source > Connection function keys. The Output Connection dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set ON (enable) or OFF (disable) in the Auto Output-Off field. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

182 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Set Source Output 

###### To Set Ranging Mode of the Sweep Source 

For details on this function, see “Ranging Mode” on page 201. 

1. Press the Config > Source > Sweep function keys. 

The Sweep dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set BEST, AUTO, or FIXED in the Sweep Ranging field. For details, see “Sweep Dialog Box” on page 135. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Sweep Direction 

1. Press the Config > Source > Sweep function keys. 

The Sweep dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set UP or DOWN in the Sweep Direction field. For details, see “Sweep Dialog Box” on page 135. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Source Output Value after Sweep 

1. Press the Config > Source > Sweep function keys. 

The Sweep dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set START VALUE or END VALUE in the Output after Sweep field. For details, see “Sweep Dialog Box” on page 135. 

4. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

B2900B/BL Series User’s Guide, Edition 2 

183 

Front Panel Operations To Set Source Output 

To cancel the setting change, press the Cancel/Local key. 

184 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Execute Measurement 

#### To Execute Measurement 

This section describes the following measurement operations. 

- “To Enable Resistance Measurement” 

- “To Set Measurement Mode” 

- “To Perform Spot Measurement” 

- “To Stop Measurement” 

- “To Set Measurement Time” 

- “To Set Measurement Range” 

- “To Perform Sweep Measurement” 

- “To Set Measurement Trigger Parameters” 

- “To Set Measurement Wait Time” 

- “To Set Measurement Auto Range Operation” 

- “To Enable or Disable Resistance Compensation” 

###### To Enable Resistance Measurement 

1. Press the View key to display the Single view. If the Range parameters are not displayed, press the Hide XXXX assist key. 

2. Move the field pointer to the Measure Ohms (resistance measurement range) field. If this field is set to OFF, the resistance measurement is disabled. 

3. Press the rotary knob to change the pointer status to EDIT. 

4. Press the AUTO, FIXED, or V/I assist key to set the resistance measurement operation. The pointer status is changed to MOVE. 

   - For details on the resistance measurement operation, see “Range parameters” on page 116. 

To set the measurement range continuously, see “To Set Measurement Range” on page 187. 

B2900B/BL Series User’s Guide, Edition 2 

185 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Front Panel Operations To Execute Measurement 

4. When MANUAL is specified, the aperture time setup field and PLC setup field are displayed. Enter and fix a value in either of the fields to set the measurement time. 

###### To Set Measurement Range 

For details on this function, see “Ranging Mode” on page 201 and “Range parameters” on page 116. 

1. Press the View key to display the Single view. If the Range parameters are not displayed, press the Hide XXXX assist key. 

2. Move the field pointer to the Measure Volts (voltage measurement range), Measure Amps (current measurement range), or Measure Ohms (resistance measurement range) field. 

3. Press the rotary knob to change the pointer status to EDIT. 

4. Press AUTO to set the auto range operation or FIXED to set the fixed range operation. The pointer status is changed to MOVE. 

5. Move the field pointer to the range value setup field (on the right side of the Measure Volts, Measure Amps, or Measure Ohms field). 

6. Press the rotary knob to change the pointer status to EDIT. 

7. Press an assist key to set the following value. 

For the auto range operation: Minimum range of the range operation 

For the fixed range operation: Range used for measurement 

###### To Perform Sweep Measurement 

For details on this function, see “Sweep Output” on page 206. 

1. Press the View key to display the Single view. 

2. Set the source function (see “To Set Source Output Mode” on page 175). 

3. Set the Source value and Limit (Compliance) value (see “To Apply DC Voltage/Current” and “To Set Limit (Compliance) Value” on page 175). 

4. Set the measurement mode (see “To Set Measurement Mode” on page 186). 

5. Set the sweep source (see “To Set Sweep Output” on page 177). 

6. Press the View key to display the Graph view. 

7. Press the On/Off switch for channel 1 or 2 (Ch1 or Ch2). 

B2900B/BL Series User’s Guide, Edition 2 

187 

NOTE 

Front Panel Operations To Execute Measurement 

The Wait Control dialog box opens. 

2. Set each parameter in the Measure column. 

3. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Measurement Auto Range Operation 

For details on this function, see “Ranging Dialog Box” on page 136. 

1. Press the Config > Measure > Ranging function keys. 

The Ranging dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. Set the current measurement auto range operation (NORMAL, SPEED, or RESOLN) in the Current Auto Ranging field. 

Also, set the threshold of the auto range operation in the Threshold field. 

4. Set the voltage measurement auto range operation (NORMAL, SPEED, or RESOLN) in the Voltage Auto Ranging field. 

Also, set the threshold of the auto range operation in the Threshold field. 

5. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Enable or Disable Resistance Compensation 

1. Press the Config > Measure > R Compen function keys. 

2. For the 2-channel models, specify the channel to which the setting change is applied by using the following function keys. 

ALL: Channels 1 and 2 (Ch 1 and Ch 2) 

Ch 1: Channel 1 (Ch 1) only 

Ch 2: Channel 2 (Ch 2) only 

B2900B/BL Series User’s Guide, Edition 2 

189 

Front Panel Operations To Execute Measurement 

3. To enable the resistance compensation, press ON. To disable it, press OFF. 

To cancel the setting change, press the Cancel/Local key. 

190 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Use Math Function 

#### To Use Math Function 

This section describes how to use the math function. For details on this function, see “Math Expression” on page 214. Also, for details on the setup parameters, see “Math Expression Dialog Box” on page 138. 

1. Press the Function > Math function keys. 

The Math Expression dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. In the Status field, set the math function ON (enable) or OFF (disable). 

4. In the Unit String field, enter the unit for the calculation result data. 

To enter alphabets, press the ABC assist key and then use the numeric/alpha keys. 

To enter numbers, press the 123 assist key and then use the numeric/alpha keys. 

To delete a character, highlight the character to be deleted and then press the Delete function key. 

To insert a character, highlight the character at the position where to insert the character and then press the Insert function key. 

5. Select a math expression to use. 

To select a math expression, highlight the name of the math expression in the list area below the Unit String field. 

6. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

B2900B/BL Series User’s Guide, Edition 2 

191 

Front Panel Operations To Execute Limit Test 

#### To Execute Limit Test 

This section describes how to set the limit test and how to display the result of the limit test. For details on this function, see “Limit Test” on page 217. The limit test is not available for B2901BL. 

To perform the limit test, set the composite limit test, individual limit tests, and source output and measurement conditions, and then press the Trigger key. 

- “To Set Composite Limit Test” 

- “To Set Individual Limit Tests” 

- “To Display Limit Test Result” 

###### To Set Composite Limit Test 

For details on the setup parameters, see “Composite Limit Test Setup Dialog Box” on page 139. 

1. Press the Function > Limit Test > Composite function keys. 

   - The Composite Limit Test dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. In the Limit Test field, set the composite limit test ON (enable) or OFF (disable). 

4. In the Mode field, set the operation mode GRADING (grading mode) or SORTING (sorting mode). 

5. In the Auto Clear field, set the automatic clear ON (enable) or OFF (disable) for the composite limit test result. 

6. When the operation mode is GRADING, set the test result output timing IMMEDIATE (after every test) or END (after the last test) in the Update field. 

7. In the Offset Cancel field, set the offset cancel ON (enable) or OFF (disable). 

8. In the Offset field, set the offset value used for the offset cancel. 

9. When the operation mode is GRADING, set a bit pattern that represents the limit test pass state (Pass) in the Pass Pattern field. 

10. When the operation mode is SORTING, set a bit pattern that represents the limit test fail state (Fail) in the Fail Pattern field. 

192 

B2900B/BL Series User’s Guide, Edition 2 

Front Panel Operations To Execute Limit Test 

11. In the /BUSY field, set the DIO pin number used for the BUSY signal output. 

12. In the /SOT field, set the DIO pin number used for the start-of-test (SOT) signal output. 

13. In the /EOT field, set the DIO pin number used for the end-of-test (EOT) signal output. 

14. To apply the settings, press Apply. 

To apply the settings and close the dialog box, press OK. 

To cancel the setting change, press the Cancel/Local key. 

###### To Set Individual Limit Tests 

For details on the setup parameters, see “Limit Test Setup Dialog Box” on page 140. 

1. Press the Function > Limit Test > Limits function keys. 

The Limit Test Setup dialog box opens. 

2. In the Ch field, set the channel to which the setting change is applied. 

3. In the Feed Data field, set the type of data used for the pass/fail judgement of the limit test. 

MATH: Calculation result data of math expression 

VOLTS: Voltage measurement data (Vmeas) 

AMPS: Current measurement data (Imeas) 

OHMS: Resistance data (= Vmeas/Imeas) 

4. In the Test Index field, set the index of the limit test between numbers 1 and 12. 

The index numbers 1 to 12 are associated with the bin numbers 1 to 12. See “To Display Limit Test Result” on page 195. 

5. In the Limit Test field, set the limit test ON (enable) or OFF (disable) for the test identified by Test Index. 

6. In the Function field, set the test mode COMPLIANCE (compliance check) or LIMIT (limit test). 

7. When the operation mode is SORTING, set a bit pattern that represents the limit test pass state (Pass) in the Pass Pattern field. 

B2900B/BL Series User’s Guide, Edition 2 

193 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Front Panel Operations To Execute Limit Test 

###### To Display Limit Test Result 

For details on the setup parameters, see “Limit Test Result Dialog Box” on page 148. 

1. Press the Result > Limit Test function keys. 

The Limit Test Result dialog box opens. 

2. In the Ch field, set the channel for the data to be displayed. 

The limit test result is displayed in the area below the Length field. 

3. Press OK to close the dialog box. 

The Length field displays the number of data. 

In the area below the Length field, the limit test result is displayed in the following format: 

(aaaaa) BIN: bb DATA: +c.ccccccE+ddd 

(aaaaa) Data index aaaaa BIN: Bin number bb ( 01 to 12 ) The bin numbers 1 to 12 are associated with the index numbers 1 to 12. See “To Set Individual Limit Tests” on page 193. 

If the limit test data is out of the bins, 00 is set for the GRADING mode and 15 is set for the SORTING mode. DATA: Limit test data +c.ccccccE+ddd 

B2900B/BL Series User’s Guide, Edition 2 

195 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Front Panel Operations To Use Trace Buffer 

To change the statistical data to be saved in the trace buffer, use “Format (Trace) Dialog Box” on page 154. 

###### To Display Statistical Data 

For details on the setup parameters, see “Trace Statistical Result Dialog Box” on page 149. 

1. Press the Result > Trace function keys. 

The Trace Statistical Result dialog box opens. 

2. In the Ch field, set the channel for the data to be displayed. 

3. In the Element field, set the data type SOURCE (source output data), VOLTS (voltage measurement data), AMPS (current measurement data), OHMS (resistance measurement data) for the statistical calculation. 

The statistical data for the data specified here is displayed in the area below the Length field. 

4. Press OK to close the dialog box. 

The Length field displays the number of data. 

The statistical data is displayed in the following fields: 

Mean: Mean value Std. Dev.: Standard deviation 

Min.: Minimum value Max.: Maximum value 

B2900B/BL Series User’s Guide, Edition 2 

197 

Front Panel Operations To Use Program Memory 

#### To Use Program Memory 

This section describes how to select and run a memory program. For details on this function, see “Program Memory” on page 222. 

See also “Program Key Group” on page 151. 

- “To Select a Program” 

- “To Control Program Operation” 

###### To Select a Program 

1. Press the More > Program > Catalog function keys. 

The Program Catalog dialog box opens. 

2. Highlight the name of the memory program to run. 

3. Press OK. 



To set a value for a variable used in the memory program, use the Variable dialog box. For details, see “Program Key Group” on page 151. 

###### To Control Program Operation 

1. Press the More > Program > Control function keys. 

2. Use the following keys to control the operation of a memory program. 

Run Starts the execution of the specified memory program. Pause Pauses the execution of the memory program. Step Starts the step execution of the specified memory program. Stop Stops the execution of the memory program. Continue Continues the execution of the memory program presently paused. 

198 

B2900B/BL Series User’s Guide, Edition 2 

B2900B/BL Series Precision Source/Measure Unit User’s Guide 

6 

### Function Details 

Limit/Compliance 200 Ranging Mode 201 Measurement Time 202 Pulse Output 204 Sweep Output 206 List Sweep 207 Output Filter 208 Over Voltage/Current Protection 209 Output-Off Status 210 Automatic Output-ON/OFF Function 211 High Capacitance Mode 212 Resistance Measurement 213 Math Expression 214 Limit Test 217 Trace Buffer 220 Program Memory 222 Channel Grouping 223 Trigger System 224 File Access Function (Easy File Access) 230 Interlock Function 231 Over Temperature Protection 232 Initial Settings 233 

This chapter describes the functions of the Keysight B2900. 

Function Details Limit/Compliance 

#### Limit/Compliance 

Limit/Compliance is the output limiter for preventing damage to the test device from overcurrent or overvoltage. Voltage compliance is for current output channels, and current compliance is for voltage output channels. 

When a channel reaches compliance, the channel acts as a constant voltage source or a constant current source. The channel keeps the output value once compliance has been reached. 

Compliance can be set with the same resolution and accuracy as output current or output voltage. 

###### To Set Compliance 

The following points must be noted when setting the compliance. 

- The minimum compliance value is limited as follows. 

   - Current compliance 

      - 1 % of the range (at 100 nA Range) 

1 nA (at 10 nA Range) 

- Voltage compliance 

20 mV (at 0.2 V range) 

- If the current compliance value is too low, the SMU will require a long settling time. 

- Positive and negative compliance values can be set individually. See “Display Preference Dialog Box” on page 159. 

B2900B/BL Series User’s Guide, Edition 2 

200 

Function Details Ranging Mode 

#### Ranging Mode 

The following ranging modes are available for performing source output or measurement. 

- FIXED (fixed range) 

The channel uses the specified range only. 

- AUTO (auto range) 

The channel automatically uses the range which provides the best resolution for the source output value or the measurement value. It is possible to specify the minimum range effective for the auto range operation. 

- BEST (best, only for the sweep source channel) 

In the linear sweep mode, the channel automatically uses the minimum range which covers the whole sweep output. 

In the log sweep mode, the channel automatically uses the range which provides the best resolution for each sweep step output. 

###### Setting the Ranging Mode 

The following points must be noted when setting the ranging mode. 

- The measurement channel does not use ranges higher than the minimum range which covers the Limit (Compliance) value. 

- The pulsed source channel uses the fixed range for measurement. If the ranging mode other than FIXED is set, the channel automatically uses the minimum range which covers the Limit (Compliance) value. 

- For the source side measurement, the channel uses the present output range. 

- To set the ranging mode of the DC (constant) source or measurement channel, see “Range parameters” on page 116. 

- To set the ranging mode of the sweep source channel, see “Sweep Dialog Box” on page 135. 

- To set the detail of the auto ranging operation for measurements, see “Ranging Dialog Box” on page 136. 

B2900B/BL Series User’s Guide, Edition 2 

201 

Function Details Measurement Time 

#### Measurement Time 

Measurement time depends on aperture time, measurement range, and other measurement conditions, and can be expressed by the following formula: 

###### _Measurement time_ = _Aperture time_ + _Overhead time_ 

Aperture time is the time required for measurement, and does not include such factors as range change or data compensation, which would be the overhead time. 

###### Aperture Time 

Aperture time is the time required to acquire the measurement data. For accurate and reliable measurement, the aperture time should be increased. 

The aperture time is set by using the Measure Speed parameter on the Single view. See “Speed” on page 115. 

###### Overhead Time 

Overhead time is the time required for range change, etc. This time depends on the measurement condition, and cannot be specified. Major elements of the overhead time are: 

- Range change time during measurement (when measurement ranging mode is set to AUTO) 

- Range change time at the start of a measurement (when the measurement range is less than the compliance value) 

###### To Control Source/Measure Timing 

The source output and measurement timing can be controlled by the following parameters. See Figure 6-1. This figure shows an example of sweep output. For a bias output, focus the shape of a sweep step only. 

1. Source delay 

The source delay time is defined as the time from trigger to start of a source output. 

B2900B/BL Series User’s Guide, Edition 2 

202 

Function Details Measurement Time 

2. Measure delay 

The measurement delay time is defined as the time from trigger to start of a measurement. 

3. Source wait 

The source wait time is defined as the time the source channel cannot change the output value after starting an output. 

4. Measure wait 

The measurement wait time is defined as the time the measurement channel cannot start measurement after starting an output. 

5. Period 

The period is the trigger interval. It can be defined for the TIMER or MANUAL trigger type (trigger source) and for the source output and measurement actions individually. 

Figure 6-1 Source Output and Measurement Timing, Sweep Output Example 



<!-- Start of picture text -->
Voltage or current<br>Trigger Trigger Trigger<br>Stop value<br>Source delay (1) (1) (1)<br>Measure delay (2) (2) (2)<br>Source wait (3)<br>(3) (3)<br>Measure wa it (4)<br>Previous value (4) (4)<br>Start value :  Measurement<br>Time<br>Period (5) (5)<br><!-- End of picture text -->

To set the delay time and the period, see “Trigger parameters” on page 122. 

To set the wait time, see “Wait Control Dialog Box” on page 137. 

For more information about the trigger setup, see “Trigger Key Group” on page 143. 

B2900B/BL Series User’s Guide, Edition 2 

203 

Function Details Pulse Output 

#### Pulse Output 

The source/measure unit SMU can apply the pulsed voltage or current. Figure 6-2 shows a pulsed sweep output example. For a pulsed bias output, focus the shape of a pulse only. The pulse output is not available for B2901BL and B2910BL. 

###### To Control Pulse Output/Measure Timing 

The pulse output and measurement timing can be controlled by the following parameters. See Figure 6-2. 

1. Source delay 

The source delay time is defined as the time from trigger to start of a source output. 

2. Measure delay 

The measurement delay time is defined as the time from trigger to start of a measurement. 

3. Pulse delay 

The pulse delay time is defined as the time from the start of a source output to the start of a pulse (peak) output. 

4. Source wait 

The source wait time is defined as the time the source channel cannot change the output value after the trailing edge of a pulse. 

5. Measure wait 

The measurement wait time is defined as the time the measurement channel cannot start measurement after the trailing edge of a pulse. 

6. Pulse width 

The pulse width is the time from the start of a pulse output to the end of a pulse (peak) output. However, it is strictly defined as the time from 10 % of peak level at the leading edge to 90 % of peak level at the trailing edge. The available values are 50 μs to 100000 s. 

B2900B/BL Series User’s Guide, Edition 2 

204 

Function Details Pulse Output 

###### Figure 6-2 

Pulse Output and Measurement Timing, Sweep Output Example 



<!-- Start of picture text -->
Voltage or current<br>Trigger Trigger Trigger<br>Stop value<br>Source delay (1) (1) (1)<br>Measure delay  (2) (2) (2)<br>Pulse delay (3) (3) (3)<br>Source wait (4 )<br>(4) (4)<br>Measure wait  (5)<br>(5) (5)<br>Start value<br>Previous value (6)<br>Pulse width (6) (6) :  Measurement<br>Time<br><!-- End of picture text -->

To set the delay time, see “Trigger parameters” on page 122. 

To set the pulse delay time and the pulse width, see “Pulse parameters” on page 121. 

To set the wait time, see “Wait Control Dialog Box” on page 137. 

For more information about the trigger setup, see “Trigger Key Group” on page 143. 

###### To Set Pulse Output 

The following points must be noted when setting the pulse output. 

- Pulse base value is set by the Source value on the Single or Dual view. 

- Pulse peak value is set by the Single view. 

Set the Peak value of “Pulse parameters” on page 121 for the pulsed bias output. 

Or set the Start, Stop, and Points values of “Sweep parameters” on page 117 for the pulsed sweep output. 

B2900B/BL Series User’s Guide, Edition 2 

205 

Function Details Sweep Output 

#### Sweep Output 

The source/measure unit SMU can apply a sweep voltage or current. It supports several shapes of sweep output, as shown in Figure 6-3. The SMU not only performs the sweep output but performs the measurement for each sweep step as shown in Figures 6-1 and 6-2. 

Figure 6-3 

###### Variety of Sweep Outputs 



<!-- Start of picture text -->
Single Sweep Double Sweep<br>DC output Pulse output DC output Pulse output<br>Linear Sweep<br>DC output Pulse output DC output Pulse output<br>Logarithmic Sweep<br><!-- End of picture text -->

###### To Set Sweep Output 

The following points must be noted when setting the sweep output. 

- To set the staircase sweep source, see “Sweep parameters” on page 117. 

- To set the ranging mode of the sweep source, see “Sweep Dialog Box” on page 135. 

- To set the sweep direction, see “Sweep Dialog Box” on page 135. 

- To set the output condition after sweep, see “Sweep Dialog Box” on page 135. 

- To set the pulse delay time and the pulse width, see “Pulse parameters” on page 121. 

- To set the list sweep source, see “List sweep setup” on page 118. 

B2900B/BL Series User’s Guide, Edition 2 

206 



<!-- Start of picture text -->
Ai Type: W CH: 2<br>Points 100 [ooa7 [+0. 99za18| Ww<br>Max: +¢1.000000 ¥ v<br>Min : 000.0000 mv<br><!-- End of picture text -->

Function Details Output Filter 

#### Output Filter 

The filter is mounted on the source/measure unit (SMU). It assures clean source output without spikes, noise, and overshooting. However, using a filter may increase the SMU settling time. 

To set the filter, see “Output Filter Dialog Box” on page 133. 

B2900B/BL Series User’s Guide, Edition 2 

208 

Function Details Over Voltage/Current Protection 

#### Over Voltage/Current Protection 

The over voltage/current protection is effective for preventing damage to the test device from overcurrent or overvoltage. If this function is enabled, the source/measure unit (SMU) sets the output to 0 V and sets the output switch to off automatically and immediately when it reaches the compliance status. 

To set the over voltage/current protection, see “Output Connection Dialog Box” on page 134. 

B2900B/BL Series User’s Guide, Edition 2 

209 

NOTE 

Function Details Automatic Output-ON/OFF Function 

#### Automatic Output-ON/OFF Function 

The automatic output-ON/OFF function defines the source channel’s output ON/OFF operation when the trigger system changes the status. 

- Automatic-ON function 

If this function is enabled, the source/measure unit (SMU) automatically turns the channel output ON just before the trigger system is initiated by an SCPI command, not by a front panel operation. 

- Automatic-OFF function 

If this function is enabled, the SMU automatically turns the channel output OFF immediately when all trigger system changes the status from busy to idle. 

To set the automatic output ON/OFF function, see “Output Connection Dialog Box” on page 134. 

B2900B/BL Series User’s Guide, Edition 2 

211 

Function Details High Capacitance Mode 

#### High Capacitance Mode 

High capacitance mode is effective for the measurement of a capacitive load greater than 0.01 μF. 

If the measurement result data is not stable, set this function ON. The measurement data may come stable. This function is effective for measurements of capacitive devices up to 50 μF. 

High capacitance mode is available for the following source/measure condition. 

- Operation mode: voltage source (Source Volts) and current measurement (Measure Amps) 

- Measurement ranging mode: FIXED 

- Measurement range: 1 μA to 10 A 

To set the high capacitance mode, see “Output Connection Dialog Box” on page 134. 

212 

B2900B/BL Series User’s Guide, Edition 2 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Function Details Math Expression 

#### Math Expression 

B2900 provides a math function for performing calculations using the measurement result data. The calculation result can be displayed and used for the limit test and trace statistics. 

For the predefined math expressions, see “Predefined Math Expressions” on page 214. 

To define a math expression, see _Keysight B2900 SCPI Command Reference_ . Math expressions can be defined by using the :CALC:MATH commands. For resources effective for the expression, see “Resources Used in the Expressions” on page 215. 

To use the math function, see “Math Expression Dialog Box” on page 138. 

To display the calculation result, see “Graph View” on page 124 and “Measure Result Dialog Box” on page 147. 

###### Predefined Math Expressions 

The following math expressions have already been defined in B2900. The predefined math expressions are not cleared by the power off and on operations. 

- Power (POWER) 

- Offset Compensated Ohms (OFFCOMPOHM) 

- Varistor Alpha (VARALPHA) 

- Voltage Coefficient (VOLTCOEF) 

In the following formula, [ _c_ ] specifies the channel (1 or 2) used for the measurement. 

POWER Calculates power using the following formula. 

POWER = VOLT[ _c_ ] * CURR[ _c_ ] 

OFFCOMPOHM Calculates offset compensated ohms (resistance) using the following formula. OFFCOMPOHM = (VOLT[ _c_ ][1] - VOLT[ _c_ ][0]) / (CURR[ _c_ ][1] - CURR[ _c_ ][0]) 

214 

B2900B/BL Series User’s Guide, Edition 2 

Function Details Math Expression 

where, VOLT[ _c_ ][0] and CURR[ _c_ ][0] are the data measured with a current output level, and VOLT[ _c_ ][1] and CURR[ _c_ ][1] are the data measured with a different current output level or zero output. 

This function is effective for reducing measurement errors in low resistance measurements. 

- VARALPHA Calculates varistor alpha using the following formula. 

VARALPHA = log(CURR[ _c_ ][1] / CURR[ _c_ ][0]) / log(VOLT[ _c_ ][1] / VOLT[ _c_ ][0]) 

where, CURR[ _c_ ][0] and VOLT[ _c_ ][0] are the measurement data at a point on a varistor’s non-linear I-V characteristics curve, and CURR[ _c_ ][1] and VOLT[ _c_ ][1] are the data at the another point. 

- VOLTCOEF Calculates voltage coefficient using the following formula. 

VOLTCOEF = (RES[ _c_ ][1] - RES[ _c_ ][0]) / (RES[ _c_ ][1] * (VOLT[ _c_ ][1] - VOLT[ _c_ ][0])) * 100 % 

where, RES[ _c_ ][0] and RES[ _c_ ][1] are the resistance measurement data at the first and second measurement points, respectively, and VOLT[ _c_ ][0] and VOLT[ _c_ ][1] are the voltage measurement data at the first and second measurement points, respectively. 

The voltage coefficient is known as the ratio of the fractional change for a resistor whose resistance varies with voltage. 

###### Resources Used in the Expressions 

The following resources can be used in user-defined math expressions. 

- Reserved variables 

The variables listed in Table 6-2 are reserved for reading the channel output or measurement data. 

Scalar variable is used for spot measurement data. 

Vector (array) variable is used for sweep measurement data. 

- Math operators 

The following operators are available. 

- Arithmetic operators: +, - , *, /, ^, see Table 6-3. 

- Elementary functions: ln, log, sin, cos, tan, exp 

B2900B/BL Series User’s Guide, Edition 2 

215 

Function Details Math Expression 

The functions log and ln perform the operation after calculating the absolute value. So if a negative value is specified, they do not cause an error but - calculate as a positive value. For example, log( 10) results in log(10)=1. 

Table 6-2 

Reserved Variables 

|**Reserved**<br>**Scalar**|**variable**<sup>**a**</sup><br>**Vector**|**Description**|
|---|---|---|
|SOUR[_c_]|SOUR[_c_][]|Source output setting data|
|VOLT[_c_]|VOLT[_c_][]|Voltage measurement data|
|CURR[_c_]|CURR[_c_][]|Current measurement data|
|RES[_c_]|RES[_c_][]|Resistance measurement data|
|TIME[_c_]|TIME[_c_][]|Time (timestamp) data|



a. The numeric suffix [ _c_ ] is effective for specifying the channel. For example, use CURR2 to read the current spot measurement data of the channel 2. 

Table 6-3 

Arithmetic and Unary Operators 

|**Priority of task**|**Operator**|**Description**|
|---|---|---|
|High|( )|Parentheses|
|:<br>:|+ and-|Unary plus operator and unary minus operator|
|:<br>|^|Exponentiation operator|
|:<br>:|* and /|Multiplication operator and division operator|
|Low|+ and-|Additive operator and subtraction operator|



216 

B2900B/BL Series User’s Guide, Edition 2 

NOTE 

Function Details Limit Test 

Figure 6-5 

Composite Limit Test Flowchart Example for Grading Mode 



<!-- Start of picture text -->
START Display example of PASS status when Zoom is ON:<br>Single view Dual view<br>Wait for SOT<br>Source and Sense<br>Perform<br>Limit X ? Yes Pass ? No Display “FAIL”<br>No No Yes<br>Last Limit ? Immediate ?<br>Yes<br>Yes No<br>Pass ? Yes Display“PASS” First<br>Failure ?<br>No<br>No<br>Yes Output Fail<br>Pattern<br>Immediate ? Yes<br>Save Fail<br>No Output Pattern<br>Pass Pattern<br>Another<br>Yes<br>Data ?<br>No<br>Immediate ?<br>Failures ? No<br>Yes<br>No<br>Yes<br>Output Saved Output<br>Fail Pattern Pass Pattern<br>Another<br>Yes<br>Device ?<br>No GRADING MODE<br>END<br><!-- End of picture text -->

218 

B2900B/BL Series User’s Guide, Edition 2 

Function Details Limit Test 

Figure 6-6 

Composite Limit Test Flowchart Example for Sorting Mode 



<!-- Start of picture text -->
START Display example of PASS status<br>when Zoom is ON:<br>Single view<br>Wait for SOT<br>Source and Sense<br>Dual view<br>Perform<br>No<br>Limit X ?<br>Yes<br>Pass ? Yes<br>No<br>No Last Limit ?<br>Yes<br>Display “FAIL” Display “PASS”<br>Output Output<br>Fail Pattern Pass Pattern<br>Another<br>Yes<br>Device ? SORTING MODE<br>END<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

219 

Function Details Trace Buffer 

#### Trace Buffer 

The trace buffer collects the test result data until a buffer full is detected. The maximum data size is 100,000 blocks for each channel of B2900 except B2901BL. For B2901BL, the maximum data size is 10,000 blocks. The data flow is shown in Figure 6-7. One data block may contain multiple data, such as voltage measurement data, current measurement data, resistance measurement data, source output data, calculation result data, limit test data, time data, and status data. They are selected by using the Format keys of the I/O key group. See “I/O Key Group” on page 152. 

For setting the trace buffer, see “Trace Buffer Setup Dialog Box” on page 141. In Figure 6-7, the variables indicate the following data. 

- VOLT: Voltage measurement data 

- CURR: Current measurement data 

- RES: Resistance measurement data 

- TIME: Time data (timestamp of the measurement start trigger) 

- STAT: Status data or limit test status 

- SOUR: Source output data 

- CALC: Math (calculation) result data or limit test data (= raw data - offset data) 

If data is stored in the trace buffer, its statistical data can be calculated. Calculable statistical data is as follows. 

- MEAN: Mean value 

- SDEV: Standard deviation 

- MIN: Minimum value 

- MAX: Maximum value 

- PKPK: Peak to peak value 

The statistical data (except for PKPK) of VOLT, CURR, or RES data can be displayed on the Trace Statistical Result dialog box. See “Trace Statistical Result Dialog Box” on page 149 for displaying the data. 

B2900B/BL Series User’s Guide, Edition 2 

220 

Function Details Trace Buffer 

Use an external computer to display the data which cannot be displayed on the B2900 screen. The data can be saved in an USB memory connected to the front panel USB-A connector or can be read by using SCPI commands. 



Do not forget to save or read the data before turning the B2900 off. The trace buffer is cleared by turning the instrument off. 

Figure 6-7 

###### Data Flow to Trace Buffer for Each Channel 



<!-- Start of picture text -->
Sense<br>(Voltage, Current)<br>VOLT<br>CURR<br>Result > Meas<br>File >Save > Meas RES<br>TIME<br>STAT<br>SOUR<br>MATH<br>Feeding<br>(WATT, MATH)<br>Result > Meas CALC<br>Limit Test *<br>TIME<br>File >Save > Math STAT Offset<br>Binning<br>Result > Limit CALC<br>File >Save > Limit TIME<br>STAT<br>Trace<br>Feeding<br>VOLT<br>CURR MEAN<br>RES SDEV<br>CALC MIN<br>TIME MAX<br>STAT PKPK<br>SOUR<br>File > Save > Trace<br>Result > Trace<br><!-- End of picture text -->

* Limit Test is not available for B2901BL. 

B2900B/BL Series User’s Guide, Edition 2 

221 

Function Details Program Memory 

#### Program Memory 

The program memory stores command strings temporarily. The stored program can be executed by using the front panel keys, or automatically when B2900 is turned on. See “Program Key Group” on page 151 and “Start-up” on page 161. The keys used for controlling the program memory are shown in Table 6-4. This table also shows the status changed by pressing the program control keys. 

The program memory can eliminate several processes in the program execution, such as transferring commands, checking command syntax, and converting commands to the internal codes. Thus, using the program memory speeds up program execution. If frequently used command strings are stored in the program memory, interface/computer activity is minimized. 

The program memory can be defined by using SCPI commands while B2900 is in the remote mode. See Keysight B2900 series _SCPI Command Reference_ . 

- Number of programs saved in the memory: 100 

- Number of programs used for start-up automatic execution: 1 

- Total memory size: 100 KB 

- Maximum length for a line: 256 bytes 

- Maximum number of characters of program name: 32 with alphabets, numbers, hyphens, and underscores 

Table 6-4 

Program Control Keys (Function Keys) and Status Changes 

|**Control key**||**Present execution stat**|**us**|
|---|---|---|---|
||**Running**|**Paused**|**Stopped**|
|Run|Error|to Running|to Running|
|Pause|to Paused|Paused|Stopped|
|Step|Error|to Running to Paused|to Running to Paused|
|Stop|to Stopped|to Stopped|Stopped|
|Continue|Error|to Running|Error|



B2900B/BL Series User’s Guide, Edition 2 

222 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->

Function Details Trigger System 

#### Trigger System 

B2900 supports the ARM-TRIGGER model described in _1999 SCPI Command Reference_ . Operation summary of this trigger model is described below. Also see Figure 6-8. 

The trigger system can be initiated by pressing the Trigger key on the front panel. 

For the trigger setup parameters, see “Trigger parameters” on page 122 and “Trigger Configuration Dialog Box” on page 144. 

1. Moves to the ARM layer if the trigger system was initiated. 

2. Waits for the ARM event which is a signal from the ARM trigger source. 

3. Moves to the TRIGGER layer if the ARM event was detected and the ARM delay time elapsed. 

4. Waits for the TRIGGER event which is a signal from the TRIGGER trigger source. 

5. Executes the device action if the TRIGGER event was detected and the TRIGGER delay time elapsed. 

6. Repeats 4 and 5 until the TRIGGER count reaches the specified count. And then moves to the ARM layer. 

7. Repeats 2 to 6 until the ARM count reaches the specified count. And then moves to the Idle layer. 

This trigger model is applied to two device actions, Transient (source output) and Acquire (measurement) independently. Two actions can start simultaneously or separately. 

Also for the 2-channel models, two channels can perform synchronous operation or asynchronous operation. 

B2900B/BL Series User’s Guide, Edition 2 

224 

NOTE 

Function Details Trigger System 

Figure 6-8 B2900 Trigger System 



<!-- Start of picture text -->
Trigger key<br>Auto key<br>Idle<br>No<br>Initiated ?<br>Yes<br>ARM Layer Trigger output EXT1-14INT1-2 Trigger output EXT1-14INT1-2<br>before ARM LAN after ARM LAN<br>ARM count = 0<br>AUTO Yes<br>BUS<br>TIMER ARM Event No<br>INT1-2 event detector Last count ?<br>EXT1-14 1 to 100000 (1 to 10000 for B2901BL)<br>LAN INF. (infinity)<br>             (For B2901BL and B2910BL,<br>ARM count + 1               INF. is set only in AUTO mode)<br>ARM delay<br>TRIGGER Layer Trigger output EXT1-14INT1-2 Trigger output EXT1-14INT1-2<br>before TRIGGER LAN after TRIGGER LAN<br>TRIGGER count = 0<br>AUTO Yes<br>BUS<br>TIMER TRIGGER Event No<br>INT1-2 event detector Last count ?<br>EXT1-14 1 to 100000 (1 to 10000 for B2901BL)<br>LAN INF. (infinity) (Not available for B2901BL and B2910BL)<br>TRIGGER count + 1<br>TRIGGER delay<br>Trigger output EXT1-14INT1-2 Trigger output EXT1-14INT1-2<br>before Device action LAN after Device action LAN<br>Device action (Transient or Acquire )<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

226 

NOTE 

Function Details Trigger System 

   - Measurement wait time control is set to OFF. 

   - Measurement ranging mode is set to the fixed mode. 

- To synchronize measurement: 

   - Trigger source is set to the same mode. 

   - Delay time is set to the same value. 

   - Measurement wait time control is set to OFF. 

   - Measurement ranging mode is set to the fixed mode. 

Figure 6-9 

###### Transient and Acquire Device Actions 

###### Transient device action 

- for voltage or current output - 



<!-- Start of picture text -->
Source delay Source delay Step value<br>Step value<br>Trigger Trigger<br><!-- End of picture text -->

- Acquire device action - for voltage or current measurement - 



<!-- Start of picture text -->
Measure delay Measure Measure delay Measure<br>Trigger Trigger<br><!-- End of picture text -->

B2900B/BL Series User’s Guide, Edition 2 

228 

Function Details Trigger System 

###### Trigger Output 

B2900 can output trigger at the timing of _Trigger output_ shown in Figure 6-8. The trigger output terminal can be selected from the following terminals. 

- Internal bus, INT1, INT2 

- LAN port 

- Digital I/O pin, EXT1, EXT2, EXT3, EXT4, EXT5, EXT6, EXT7, EXT8, EXT9, EXT10, EXT11, EXT12, EXT13, EXT14 

The trigger output can be set by using “Trigger Configuration Dialog Box” on page 144. The trigger output timing and the setup parameters are shown in Table 6-5. 

###### Table 6-5 Trigger Output Timing and Setup Parameters 

|**Trigger output timing**|**Layer**|**Action**|**Trigger**<br>**Output**|**Output**<br>**Trigger**<br>**Timing**|
|---|---|---|---|---|
|Starting ARM loop|ARM|TRANS. for<br>|ON|BEFORE|
|Ending ARM loop|ARM|transient<br>action or ACQ.|ON|AFTER|
|Starting TRIGGER loop|TRIGGER|for acquire<br>i|ON|BEFORE|
|Ending TRIGGER loop|TRIGGER|acton|ON|AFTER|
|Starting Transient device action|ACTION|TRANS.|ON|BEFORE|
|Ending Transient device action|ACTION|TRANS.|ON|AFTER|
|Starting Acquire device action|ACTION|ACQ.|ON|BEFORE|
|Ending Acquire device action|ACTION|ACQ.|ON|AFTER|



For using a digital I/O pin, it is necessary to set function of the pin, output trigger polarity, type, pulse width, and the Output Trigger Timing parameter. See “DIO Configuration Dialog Box” on page 155 and “Using Digital I/O” on page 101. 

B2900B/BL Series User’s Guide, Edition 2 

229 



<!-- Start of picture text -->
NOTE<br><!-- End of picture text -->



<!-- Start of picture text -->
WARNING<br><!-- End of picture text -->

WARNING 

~~L\~~ 

Function Details Over Temperature Protection 

#### Over Temperature Protection 

The over temperature protection is effective for preventing damage to a channel from over temperature. If B2900 is used in an environment over 30 °C, channel outputs are limited to the value less than the maximum value. If a channel output reaches the limit, all channels automatically and immediately set the output to 0 V and set the output switch to off. And the causal channel is locked automatically. 

To unlock the channel, the self-test must be performed. If the self-test does not report any problem, you can use the channel soon. For performing the self-test, see “Self-test” on page 78. 

The maximum output for the condition over 30 °C is less than the maximum output for the specification condition of 23 °C ± 5 °C. The channel output limit is given by the following formula. 

DCMAX ≤ ( [ ( PCS + 30 - TAMB) - |VOAB × IB| ] / |VOAP × IP| )<sup>2</sup> × 100 

The parameters are described below. 

- DCMAX: Allowable maximum duty ratio in %, or 0 for DC bias output 

- PCS: Maximum power, 64 W 

- TAMB: Ambient temperature, in °C 

- VOAB = 250 V, for |IB| ≤ 105 mA 

   - VOAB = 39 V, for |IB| > 105 mA and |VB| > 6V 

   - VOAB = 21 V, for |IB| > 105 mA and |VB| ≤ 6V 

VB: Pulse base voltage setting value or DC bias voltage setting value, in V 

- IB: Current at pulse base or DC current, in A 

- VOAP = 250 V, for |IP| ≤ 105 mA 

   - VOAP = 39 V, for |IP| > 105 mA and |VP| > 6V 

   - VOAP = 21 V, for |IP| > 105 mA and |VP| ≤ 6V 

VP: Pulse peak voltage setting value, in V 

- IP: Current at pulse peak, in A 

B2900B/BL Series User’s Guide, Edition 2 

232 

Function Details Initial Settings 

#### Initial Settings 

The B2900 is initialized by turning the B2900 on, the *RST command, or the device clear. Initial settings of the B2900 are shown in this section. 

Table 6-6 

###### System Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|GPIO function|DIGITAL IN||
|GPIO function (D14)|HIGH VOLTAGE LAMP||
|GPIO polarity|NEGATIVE||
|GPIO output trigger type|EDGE||
|GPIO output trigger timing|BOTH||
|GPIO output trigger pulse width|100μs||
|Display digits|7||
|Zoomed display|OFF|Not changed|
|Display image format|JPG||
|User message enable|Disabled||
|User message|“”||
|Byte swap (measurement data)|Normal||
|Data format (measurement data)|ASCII||
|Data format (GPIO data)|ASCII||
|Data format (status register)|ASCII||
|Data elements (measurement data)|All (V/I/R/S/T/Stat)||
|Data elements (calculate)|CALC||
|Automatic timestamp reset|ON||
|Mass storage directory|(Root)||



B2900B/BL Series User’s Guide, Edition 2 

233 

Function Details Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|Program variables|None|Not changed|
|Program selection|None||
|Program status|Idle||



###### Table 6-7 SMU Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|Low terminal state|GROUNDED|Not changed|
|Output state|OFF||
|Output auto filter enable|ON||
|Output filter enable|ON||
|Output filter time constant|5μs||
|Output filter frequency|31.8309886 kHz||
|Output OFF state|NORMAL||
|Output protection|OFF||
|Source mode|VOLTS||
|Source shape|DC||
|Automatic output-on|ON||
|Automatic output-off|OFF||
|Voltage source|0 V||
|Voltage protection level|2 V||
|Voltage source auto ranging|ON||
|Voltage source range|2 V||
|Voltage source range lower limit|0.2 V||
|Voltage source mode|Constant voltage||



B2900B/BL Series User’s Guide, Edition 2 

234 

Function Details Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|Voltage sweep points|1||
|Voltage sweep start|0 V||
|Voltage sweep stop|0 V||
|Voltage list points|1||
|Voltage list values|0 V||
|Current source|100μA||
|Current protection level|100μA||
|Current source auto ranging|ON||
|Current source range|100μA||
|Current source range lower limit|1μA||
|Current source mode|Constant current||
|Current sweep points|1||
|Current sweep start|0 A||
|Current sweep stop|0 A||
|Current list points|1||
|Current list values|0 A||
|Sweep direction|UP||
|Double sweep|OFF||
|Sweep ranging|BEST||
|Continuous triggered source|ON||
|Pulse delay|0 s||
|Pulse width|50μs||
|Auto settling time|ON||
|Settling time|0 s||



B2900B/BL Series User’s Guide, Edition 2 

235 

Function Details Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|Measure function|Current measure||
|Auto aperture|ON||
|Aperture time|0.1 PLC||
|Voltage measure range mode|AUTO||
|Voltage measure range limit|0.2 V||
|Voltage measure range|2 V||
|Current measure range mode|AUTO||
|Current measure range limit|1μA||
|Current measure range|100μA||
|Resistance measure mode|MAN||
|Res. meas. range|2Ω||
|Res. meas. range limit (Low)|2Ω||
|Res. meas. range limit (High)|200 MΩ||
|Res. meas. compensation|OFF||
|Remote sensing|OFF||
|Measurement auto range operation|NORMAL||
|Measurement auto range threshold|90||



Table 6-8 

Trigger System Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|ARM count|1||
|ARM source|AINT||



B2900B/BL Series User’s Guide, Edition 2 

236 

Function Details Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|ARM timer|200μs for B2901BL<br>50μs for B2910BL<br>20μs for B2901B/02B<br>10μs for B2911B/12B||
|ARM delay time|0 s||
|ARM bypass|OFF||
|Trigger count|1||
|Trigger source|AINT||
|Trigger timer|200μs for B2901BL<br>50μs for B2910BL<br>20μs for B2901B/02B<br>10μs for B2911B/12B||
|Trigger delay time|0 s||
|Trigger bypass|OFF||
|External trigger output|EXT1||
|External trigger output (LAN)|LAN0-7 (all)||
|External trigger output enable|OFF||



B2900B/BL Series User’s Guide, Edition 2 

237 

Function Details Initial Settings 

Table 6-9 LXI Trigger Events Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|Event domain|0||
|LAN event|“WaitingForAcquireArm1”<br>“WaitingForTransitionArm1”<br>“WaitingForAcquireTrigger1”<br>“WaitingForTransitionTrigger1”<br>“Measuring1”<br>“Settling1”<br>“WaitingForAcquireArm2”<br>“WaitingForTransitionArm2”<br>“WaitingForAcquireTrigger2”<br>“WaitingForTransitionTrigger2”<br>“Measuring2”<br>“Settling2”||
|Delay time|0||
|Input/output filter string|“ALL:5044”||
|Input/output status|OFF||
|Input detection|RISE||
|Output drive|OFF||
|Output slope|Positive||
|Output source|“”||
|Output timestamp delta|0||
|Event logging|ON||
|Circular event logging|ON||
|Event log size|100||



B2900B/BL Series User’s Guide, Edition 2 

238 

Function Details Initial Settings 

Table 6-10 

Calculate Functions Initial Settings 

|**Setup item**|**Power on**|**Reset**|
|---|---|---|
|Composite limit test result transfer<br>timing|IMM||
|Composite limit test result automatic<br>clearance|ON||
|Composite limit test fail bit pattern|All 0||
|Composite limit test pass bit pattern|All 0||
|Composite limit test mode|GRADing||
|DIO pins for bit pattern output|None||
|DIO pin for BUSY signal output|None||
|DIO pin for EOT signal output|None||
|DIO pin for SOT signal input|None||
|Limit test feed source|VOLTage||
|Compliance check fail bit pattern|All 0||
|Compliance check fail condition|IN||
|Limit test function|LIM||
|Upper limit|+1.0||
|Lower limit|-1.0||
|Upper bit pattern|All 0||
|Pass bit pattern|All 0||
|Lower bit pattern|All 0||
|Limit test enable|OFF||
|Math function enable|OFF||
|Math function expression|(VOLT*CURR)||
|Math function expression name|“POWER”||



B2900B/BL Series User’s Guide, Edition 2 

239 

Function Details Initial Settings 

|**Setup item**|**Power on**<br>**Reset**|
|---|---|
|Math function catalog|“POWER” “OFFCOMPOHM”<br>“VOLTCOEF”<br>“VARALPHA”|
|Math function unit name|“W”|
|Offset value for test|0|
|Offset value enable|OFF|
|Trace feed source|SENSe|
|Trace control|NEVer|
|Trace points|100000 for models except B2901BL<br>10000 for B2901BL|
|Trace statistics format|MEAN|
|Trace timestamp format|ABS|



Table 6-11 

Non-volatile Communication Settings 

|**Setup item**|**Factory default setting**|
|---|---|
|DHCP|Enabled|
|IP address|169.254.5.2|
|Subnet mask|255.255.0.0|
|Default gateway|0.0.0.0|
|Obtain DNS server from DHCP|Enabled|
|DNS server|0.0.0.0|
|WINS server|0.0.0.0|
|Hostname|K-B29_xx_B-_nnnnn_or<br>|
|Desired hostname|K-B29_xx_BL-_nnnnn_<br>B29_xx_B, B29_xx_BL: model number|
|Desired service name|_nnnnn_: suffix of serial number.|



B2900B/BL Series User’s Guide, Edition 2 

240 

Function Details Initial Settings 

|**Setup item**|**Factory default setting**|
|---|---|
|mDNS|Enabled|
|Use DNS naming service|Enabled|
|Use NetBIOS naming service|Enabled|
|Domain name|Not set|
|GPIB address|23|
|LXI identify|Disabled|
|GPIB command interface|Enabled|
|USB command interface|Enabled|
|VXI-11 command interface|Enabled|
|SCPI telnet command interface|Enabled|
|SCPI socket command interface|Enabled|
|SCPI HiSLIP command interface|Enabled|
|Web interface|Enabled|
|Command prompt for a Telnet session|B2900B>|
|Welcome message for a Telnet session|Welcome to Keysight B2900B Series|



Table 6-12 

Other Non-volatile Settings 

|**Setup item**|**Factory default setting**|
|---|---|
|Channel grouping|“1” for 1-ch models|
||“1-2” for 2-ch models|
|Remote display|Enabled|
|Display color set|1|
|Beeper|Enabled|
|Graphical web interface (web server)|Enabled|



B2900B/BL Series User’s Guide, Edition 2 

241 

Function Details Initial Settings 

|**Setup item**|**Factory default setting**|
|---|---|
|SCPI language mode|Default|
|Power-on program|Not set|
|Line frequency|50 Hz|
|Fan control mode|Normal|
|V/I Limit for each polarity|OFF|
|Immediate V/I update by knob|OFF|
|Restore trigger mode on Local|OFF|



B2900B/BL Series User’s Guide, Edition 2 

242 

This information is subject to change without notice. © Keysight Technologies 2020, 2023 Edition 2, May 2023 *B2910-90110* 

B2910-90110 www.keysight.com 


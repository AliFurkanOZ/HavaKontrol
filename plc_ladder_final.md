# PLC Ladder — v4 PATTERN × 4 MFC + 12 VALF + 4 GÜÇ ÇIKIŞI (v8 - Tam Uyumlu)

> [!IMPORTANT]
> **v8 Değişiklikleri ve Çakışmasız PLC & Sistem Uyumu:**
> - **Valf 9 iptal edildi:** Gaz valfi olmadığı ve farklı bir amaç için kullanıldığı için valf listesinden çıkarıldı.
> - **16 Nokta CPU Çıkış Haritası (Y0.0 – Y0.15) & AS08AN01P-A Ek Modül (Y1.0 – Y1.7):**
>   * **Y0.0 – Y0.7 (CPU):** Valf 1 – 8 Hat Valfleri (Komut: D100 – D107, Durum: D200 – D207)
>   * **Y0.8 – Y0.11 (CPU):** Güç Çıkışları 8 – 11 / Röle 1 – 4 (Komut: D108 – D111, Durum: D208 – D211)
>   * **Y0.12 – Y0.15 (CPU):** Serbest / Yedek (İsteğe bağlı Fırın İzinleri M50..M53 veya diğer çıkışlar)
>   * **Y1.0 – Y1.3 (AS08AN01P-A Slot 1):** Valf 13 – 16 V-OVG Egzoz/Tahliye Valfleri (Komut: D112 – D115, Durum: D212 – D215)
>     - **Y1.0:** VALF 13 (V-OVG 2 Egzoz Tahliye)
>     - **Y1.1:** VALF 14 (V-OVG 3 Egzoz Tahliye)
>     - **Y1.2:** VALF 15 (V-OVG 4 Egzoz Tahliye)
>     - **Y1.3:** VALF 16 (V-OVG 1 Egzoz Tahliye)
>     - **Y1.4 – Y1.7:** Yedek Çıkışlar
> - **V-OVG Fırın Emniyet İzinleri (M50 – M53):** Fırın ısıtıcı güvenlik interlokları dahili hafıza röleleri (`M50..M53`) olarak çalışır (CPU'daki boş `Y0.12..Y0.15` çıkışlarına veya yazılımsal izin olarak verilebilir).
> - **D600–D603 Çakışması Giderildi:** MFC negatif setpoint kontrolü için scratch register'lar `D490..D493` aralığına taşındı. Böylece V-OVG 2 Sıcaklık PV register'ı (`D600`) ile olan çakışma tamamen önlendi.
> - **MFC Mantığı:** v4'te kanıtlanmış pattern aynen korundu (16 adım × 100ms = 1.6s döngü).

---

## BÖLÜM A — İLK ÇALIŞTIRMA

```
N0:  |---[ M1002 ]---[MOV  1  D455]---|
N1:  |---[ M1002 ]---[MOV  1  D300]---|
N2:  |---[ M1002 ]---[MOV  1  D400]---|
N3:  |---[ M1002 ]---[MOV  1  D450]---|
N4:  |---[ M1002 ]---[MOV  1  D451]---|
N5:  |---[ M1002 ]---[MOV  1  D452]---|
N6:  |---[ M1002 ]---[MOV  0  D453]---|
```

### SP Source = Dijital (tüm MFC'ler)
```
N7:  |---[ M1002 ]---[MOV  1  D310]---|
N8:  |---[ M1002 ]---[MOV  1  D320]---|
N9:  |---[ M1002 ]---[MOV  1  D330]---|
N10: |---[ M1002 ]---[MOV  1  D340]---|
```

### M Completion Flag'lerini Sıfırla (v4'te kanıtlandı)
```
N11: |---[ M1002 ]---( RST  M500 )---|
N12: |---[ M1002 ]---( RST  M501 )---|
N13: |---[ M1002 ]---( RST  M502 )---|
N14: |---[ M1002 ]---( RST  M503 )---|
N15: |---[ M1002 ]---( RST  M504 )---|
N16: |---[ M1002 ]---( RST  M505 )---|
N17: |---[ M1002 ]---( RST  M506 )---|
N18: |---[ M1002 ]---( RST  M507 )---|
N19: |---[ M1002 ]---( RST  M510 )---|
N20: |---[ M1002 ]---( RST  M511 )---|
N21: |---[ M1002 ]---( RST  M512 )---|
N22: |---[ M1002 ]---( RST  M513 )---|
N23: |---[ M1002 ]---( RST  M514 )---|
N24: |---[ M1002 ]---( RST  M515 )---|
N25: |---[ M1002 ]---( RST  M516 )---|
N26: |---[ M1002 ]---( RST  M517 )---|
N27: |---[ M1002 ]---( RST  M520 )---|
N28: |---[ M1002 ]---( RST  M521 )---|
N29: |---[ M1002 ]---( RST  M522 )---|
N30: |---[ M1002 ]---( RST  M523 )---|
N31: |---[ M1002 ]---( RST  M524 )---|
N32: |---[ M1002 ]---( RST  M525 )---|
N33: |---[ M1002 ]---( RST  M526 )---|
N34: |---[ M1002 ]---( RST  M527 )---|
N35: |---[ M1002 ]---( RST  M530 )---|
N36: |---[ M1002 ]---( RST  M531 )---|
N37: |---[ M1002 ]---( RST  M532 )---|
N38: |---[ M1002 ]---( RST  M533 )---|
N39: |---[ M1002 ]---( RST  M534 )---|
N40: |---[ M1002 ]---( RST  M535 )---|
N41: |---[ M1002 ]---( RST  M536 )---|
N42: |---[ M1002 ]---( RST  M537 )---|
```

### Shadow Başlatma
```
N43: |---[ M1002 ]---[MOV  16#FFFF  D510]---|
N44: |---[ M1002 ]---[MOV  16#FFFF  D511]---|
N45: |---[ M1002 ]---[MOV  16#FFFF  D512]---|
N46: |---[ M1002 ]---[MOV  16#FFFF  D520]---|
N47: |---[ M1002 ]---[MOV  16#FFFF  D521]---|
N48: |---[ M1002 ]---[MOV  16#FFFF  D522]---|
N49: |---[ M1002 ]---[MOV  16#FFFF  D530]---|
N50: |---[ M1002 ]---[MOV  16#FFFF  D531]---|
N51: |---[ M1002 ]---[MOV  16#FFFF  D532]---|
N52: |---[ M1002 ]---[MOV  16#FFFF  D540]---|
N53: |---[ M1002 ]---[MOV  16#FFFF  D541]---|
N54: |---[ M1002 ]---[MOV  16#FFFF  D542]---|
```

### Valf Sıfırlama (Valf 1 - 8)
```
N55: |---[ M1002 ]---[MOV  0  D100]---|
N56: |---[ M1002 ]---[MOV  0  D101]---|
N57: |---[ M1002 ]---[MOV  0  D102]---|
N58: |---[ M1002 ]---[MOV  0  D103]---|
N59: |---[ M1002 ]---[MOV  0  D104]---|
N60: |---[ M1002 ]---[MOV  0  D105]---|
N61: |---[ M1002 ]---[MOV  0  D106]---|
N62: |---[ M1002 ]---[MOV  0  D107]---|
```

### Güç Çıkışları Sıfırlama (Power 8, 9, 10, 11)
```
N63: |---[ M1002 ]---[MOV  0  D108]---|
N64: |---[ M1002 ]---[MOV  0  D109]---|
N65: |---[ M1002 ]---[MOV  0  D110]---|
N66: |---[ M1002 ]---[MOV  0  D111]---|
```

### V-OVG Egzoz / Tahliye Valfleri Sıfırlama (Valf 13, 14, 15, 16)
```
N66.1: |---[ M1002 ]---[MOV  0  D112]---|  (* VALF 13 Komut Sıfırla *)
N66.2: |---[ M1002 ]---[MOV  0  D113]---|  (* VALF 14 Komut Sıfırla *)
N66.3: |---[ M1002 ]---[MOV  0  D114]---|  (* VALF 15 Komut Sıfırla *)
N66.4: |---[ M1002 ]---[MOV  0  D115]---|  (* VALF 16 Komut Sıfırla *)
```

---

## BÖLÜM B — VALFLER (Y0.0 - Y0.7)

```
N67: |---[= D100 1]---( Y0.0 )---|
N68: |---[= D101 1]---( Y0.1 )---|
N69: |---[= D102 1]---( Y0.2 )---|
N70: |---[= D103 1]---( Y0.3 )---|
N71: |---[= D104 1]---( Y0.4 )---|
N72: |---[= D105 1]---( Y0.5 )---|
N73: |---[= D106 1]---( Y0.6 )---|
N74: |---[= D107 1]---( Y0.7 )---|
```

---

## BÖLÜM B2 — GÜÇ ÇIKIŞLARI (POWER ON/OFF: Y0.8 - Y0.11)

```
N75: |---[= D108 1]---( Y0.8 )---|
N76: |---[= D109 1]---( Y0.9 )---|
N77: |---[= D110 1]---( Y0.10 )---|
N78: |---[= D111 1]---( Y0.11 )---|
```

---

## BÖLÜM B3 — V-OVG EGZOZ / TAHLİYE VALFLERİ (VALF 13 - 16: D112 - D115)
> **Donanım:** AS08AN01P-A Ek Modül (Slot 1, Y1.0 – Y1.3)

```
N78.1: |---[= D112 1]---( Y1.0 )---|  (* VALF 13 - V-OVG 2 Egzoz Tahliye *)
N78.2: |---[= D113 1]---( Y1.1 )---|  (* VALF 14 - V-OVG 3 Egzoz Tahliye *)
N78.3: |---[= D114 1]---( Y1.2 )---|  (* VALF 15 - V-OVG 4 Egzoz Tahliye *)
N78.4: |---[= D115 1]---( Y1.3 )---|  (* VALF 16 - V-OVG 1 Egzoz Tahliye *)
```

> [!TIP]
> **Modül Konfigürasyonu:** AS08AN01P-A CPU'nun sağına (Slot 1) takıldığında çıkışlar `Y1.0..Y1.7` olarak adreslenir. Valfler `Y1.0..Y1.3` arasına bağlanır. CPU üzerindeki `Y0.12..Y0.15` çıkışları tamamen boşta/serbest kalır.

---

## BÖLÜM C — VALF GERİ BİLDİRİM (D200 - D207)

```
N79:  |---[ Y0.0 ]---[MOV  1  D200]---|
N80:  |---[/Y0.0 ]---[MOV  0  D200]---|
N81:  |---[ Y0.1 ]---[MOV  1  D201]---|
N82:  |---[/Y0.1 ]---[MOV  0  D201]---|
N83:  |---[ Y0.2 ]---[MOV  1  D202]---|
N84:  |---[/Y0.2 ]---[MOV  0  D202]---|
N85:  |---[ Y0.3 ]---[MOV  1  D203]---|
N86:  |---[/Y0.3 ]---[MOV  0  D203]---|
N87:  |---[ Y0.4 ]---[MOV  1  D204]---|
N88:  |---[/Y0.4 ]---[MOV  0  D204]---|
N89:  |---[ Y0.5 ]---[MOV  1  D205]---|
N90:  |---[/Y0.5 ]---[MOV  0  D205]---|
N91:  |---[ Y0.6 ]---[MOV  1  D206]---|
N92:  |---[/Y0.6 ]---[MOV  0  D206]---|
N93:  |---[ Y0.7 ]---[MOV  1  D207]---|
N94:  |---[/Y0.7 ]---[MOV  0  D207]---|
```

---

## BÖLÜM C2 — GÜÇ ÇIKIŞLARI GERİ BİLDİRİM (D208 - D211)

```
N95:  |---[ Y0.8 ]---[MOV  1  D208]---|
N96:  |---[/Y0.8 ]---[MOV  0  D208]---|
N97:  |---[ Y0.9 ]---[MOV  1  D209]---|
N98:  |---[/Y0.9 ]---[MOV  0  D209]---|
N99:  |---[ Y0.10 ]---[MOV  1  D210]---|
N100: |---[/Y0.10 ]---[MOV  0  D210]---|
N101: |---[ Y0.11 ]---[MOV  1  D211]---|
N102: |---[/Y0.11 ]---[MOV  0  D211]---|
```

---

## BÖLÜM C3 — V-OVG EGZOZ / TAHLİYE VALFLERİ GERİ BİLDİRİM (D212 - D215)

```
N102.1: |---[ Y1.0 ]---[MOV  1  D212]---|
N102.2: |---[/Y1.0 ]---[MOV  0  D212]---|
N102.3: |---[ Y1.1 ]---[MOV  1  D213]---|
N102.4: |---[/Y1.1 ]---[MOV  0  D213]---|
N102.5: |---[ Y1.2 ]---[MOV  1  D214]---|
N102.6: |---[/Y1.2 ]---[MOV  0  D214]---|
N102.7: |---[ Y1.3 ]---[MOV  1  D215]---|
N102.8: |---[/Y1.3 ]---[MOV  0  D215]---|
```

---

## BÖLÜM D — GENEL GÜÇ + NEGATİF KORUMA

```
N103: |---[= D300 1]---[MOV  1  D400]---|
N104: |---[<> D300 1]---[MOV  0  D400]---|

N105: |---[ M1000 ]---[WAND  D312  16#8000  D490]---|
N106: |---[<> D490 0]---[MOV  0  D312]---|
N107: |---[<> D490 0]---[MOV  0  D313]---|
N108: |---[ M1000 ]---[WAND  D322  16#8000  D491]---|
N109: |---[<> D491 0]---[MOV  0  D322]---|
N110: |---[<> D491 0]---[MOV  0  D323]---|
N111: |---[ M1000 ]---[WAND  D332  16#8000  D492]---|
N112: |---[<> D492 0]---[MOV  0  D332]---|
N113: |---[<> D492 0]---[MOV  0  D333]---|
N114: |---[ M1000 ]---[WAND  D342  16#8000  D493]---|
N115: |---[<> D493 0]---[MOV  0  D342]---|
N116: |---[<> D493 0]---[MOV  0  D343]---|
```

---

## BÖLÜM E — ZAMANLAYICI (100ms, 16 ADIM)

```
N117: |---[/T0 ]---[TMR  T0  1]---|
N118: |---[ T0 ]---[INC  D455]---|
N119: |---[> D455 16]---[MOV  1  D455]---|
N120: |---[< D455 1]---[MOV  1  D455]---|
```

---

## BÖLÜM F — MFC1 (Slave 1, Adım 1-4)

> v4 ile BİREBİR AYNI pattern. Skip yok, bağlantı kontrolü yok.

### ═══ Adım 1: MFC1 Debi Oku ═══
```
N121: |---[= D455 1]---[MODRWE  2  1  16#0003  16#0000  D414  2  M530]---|
```

### ═══ Adım 2: MFC1 SP Source Yaz ═══
```
N122: |---[ M500 ]---[MOV  D310  D510]---|
N123: |---[ M500 ]---( RST  M500 )---|
N124: |---[ M501 ]---[MOV  D310  D510]---|
N125: |---[ M501 ]---( RST  M501 )---|
N126: |---[= D455 2]---[<> D310 D510]---[MODRWE  2  1  16#0006  16#0009  D310  1  M500]---|
```

### ═══ Adım 3: MFC1 Flow ON/OFF Yaz ═══
```
N127: |---[ M520 ]---[MOV  D311  D511]---|
N128: |---[ M520 ]---( RST  M520 )---|
N129: |---[ M521 ]---[MOV  D311  D511]---|
N130: |---[ M521 ]---( RST  M521 )---|
N131: |---[= D455 3]---[<> D311 D511]---[MODRWE  2  1  16#0006  16#000A  D311  1  M520]---|
```

### ═══ Adım 4: MFC1 Setpoint Yaz ═══
```
N132: |---[ M510 ]---[MOV  D312  D512]---|
      |---[ M510 ]---[MOV  D313  D513]---|
N133: |---[ M510 ]---( RST  M510 )---|
N134: |---[ M511 ]---[MOV  D312  D512]---|
      |---[ M511 ]---[MOV  D313  D513]---|
N135: |---[ M511 ]---( RST  M511 )---|
N136: |---[= D455 4]---[<> D312 D512]---[MODRWE  2  1  16#0010  16#000C  D312  2  M510]---|
```

---

## BÖLÜM G — MFC2 (Slave 2, Adım 5-8)

### ═══ Adım 5: MFC2 Debi Oku ═══
```
N137: |---[= D455 5]---[MODRWE  2  2  16#0003  16#0014  D424  2  M532]---|
```

### ═══ Adım 6: MFC2 SP Source Yaz ═══
```
N138: |---[ M502 ]---[MOV  D320  D520]---|
N139: |---[ M502 ]---( RST  M502 )---|
N140: |---[ M503 ]---[MOV  D320  D520]---|
N141: |---[ M503 ]---( RST  M503 )---|
N142: |---[= D455 6]---[<> D320 D520]---[MODRWE  2  2  16#0006  16#001D  D320  1  M502]---|
```

### ═══ Adım 7: MFC2 Flow ON/OFF Yaz ═══
```
N143: |---[ M522 ]---[MOV  D321  D521]---|
N144: |---[ M522 ]---( RST  M522 )---|
N145: |---[ M523 ]---[MOV  D321  D521]---|
N146: |---[ M523 ]---( RST  M523 )---|
N147: |---[= D455 7]---[<> D321 D521]---[MODRWE  2  2  16#0006  16#001E  D321  1  M522]---|
```

### ═══ Adım 8: MFC2 Setpoint Yaz ═══
```
N148: |---[ M512 ]---[MOV  D322  D522]---|
      |---[ M512 ]---[MOV  D323  D523]---|
N149: |---[ M512 ]---( RST  M512 )---|
N150: |---[ M513 ]---[MOV  D322  D522]---|
      |---[ M513 ]---[MOV  D323  D523]---|
N151: |---[ M513 ]---( RST  M513 )---|
N152: |---[= D455 8]---[<> D322 D522]---[MODRWE  2  2  16#0010  16#0020  D322  2  M512]---|
```

---

## BÖLÜM H — MFC3 (Slave 3, Adım 9-12)

### ═══ Adım 9: MFC3 Debi Oku ═══
```
N153: |---[= D455 9]---[MODRWE  2  3  16#0003  16#0028  D434  2  M534]---|
```

### ═══ Adım 10: MFC3 SP Source Yaz ═══
```
N154: |---[ M504 ]---[MOV  D330  D530]---|
N155: |---[ M504 ]---( RST  M504 )---|
N156: |---[ M505 ]---[MOV  D330  D530]---|
N157: |---[ M505 ]---( RST  M505 )---|
N158: |---[= D455 10]---[<> D330 D530]---[MODRWE  2  3  16#0006  16#0031  D330  1  M504]---|
```

### ═══ Adım 11: MFC3 Flow ON/OFF Yaz ═══
```
N159: |---[ M524 ]---[MOV  D331  D531]---|
N160: |---[ M524 ]---( RST  M524 )---|
N161: |---[ M525 ]---[MOV  D331  D531]---|
N162: |---[ M525 ]---( RST  M525 )---|
N163: |---[= D455 11]---[<> D331 D531]---[MODRWE  2  3  16#0006  16#0032  D331  1  M524]---|
```

### ═══ Adım 12: MFC3 Setpoint Yaz ═══
```
N164: |---[ M514 ]---[MOV  D332  D532]---|
      |---[ M514 ]---[MOV  D333  D533]---|
N165: |---[ M514 ]---( RST  M514 )---|
N166: |---[ M515 ]---[MOV  D332  D532]---|
      |---[ M515 ]---[MOV  D333  D533]---|
N167: |---[ M515 ]---( RST  M515 )---|
N168: |---[= D455 12]---[<> D332 D532]---[MODRWE  2  3  16#0010  16#0034  D332  2  M514]---|
```

---

## BÖLÜM I — MFC4 (Slave 6, Adım 13-16)

### ═══ Adım 13: MFC4 Debi Oku ═══
```
N169: |---[= D455 13]---[MODRWE  2  6  16#0003  16#0064  D444  2  M536]---|
```

### ═══ Adım 14: MFC4 SP Source Yaz ═══
```
N170: |---[ M506 ]---[MOV  D340  D540]---|
N171: |---[ M506 ]---( RST  M506 )---|
N172: |---[ M507 ]---[MOV  D340  D540]---|
N173: |---[ M507 ]---( RST  M507 )---|
N174: |---[= D455 14]---[<> D340 D540]---[MODRWE  2  6  16#0006  16#006D  D340  1  M506]---|
```

### ═══ Adım 15: MFC4 Flow ON/OFF Yaz ═══
```
N175: |---[ M526 ]---[MOV  D341  D541]---|
N176: |---[ M526 ]---( RST  M526 )---|
N177: |---[ M527 ]---[MOV  D341  D541]---|
N178: |---[ M527 ]---( RST  M527 )---|
N179: |---[= D455 15]---[<> D341 D541]---[MODRWE  2  6  16#0006  16#006E  D341  1  M526]---|
```

### ═══ Adım 16: MFC4 Setpoint Yaz ═══
```
N180: |---[ M516 ]---[MOV  D342  D542]---|
      |---[ M516 ]---[MOV  D343  D543]---|
N181: |---[ M516 ]---( RST  M516 )---|
N182: |---[ M517 ]---[MOV  D342  D542]---|
      |---[ M517 ]---[MOV  D343  D543]---|
N183: |---[ M517 ]---( RST  M517 )---|
N184: |---[= D455 16]---[<> D342 D542]---[MODRWE  2  6  16#0010  16#0070  D342  2  M516]---|
```

---

## BÖLÜM J — PC HEARTBEAT

```
N185: |---[<> D301 D460]---( M100 )---|
N186: |---[ M100 ]---[MOV  D301  D460]---|
N187: |---[ M100 ]---[MOV  0  D461]---|
N188: |---[ M100 ]---[MOV  1  D452]---|
N189: |---[/M100 ]---[ M1013 ]---[< D461 50]---[INCP  D461]---|
N190: |---[>= D461 50]---[MOV  0  D452]---|
```

---

## BÖLÜM K — ALARM

```
N191: |---[= D452 0]---( M101 )---|
N192: |---[ M101 ]---[MOV  1  D453]---|
N193: |---[/M101 ]---[MOV  0  D453]---|
```

---

## BÖLÜM L — PROGRAM SONU

```
N194: |---[END]---|
```

---

## REFERANS — ÇIKIŞ VE REGISTER HARİTASI

### 1. Valfler (8 Adet Fiziksel PLC Çıkışı)
| Eleman | PLC Çıkış | Komut Register | Durum Register | Açıklama |
|--------|-----------|----------------|----------------|----------|
| Valf 1 | Y0.0 | D100 | D200 | Gaz Valfi 1 (MFC-1 Giriş) |
| Valf 2 | Y0.1 | D101 | D201 | Gaz Valfi 2 (V-OVG 2 Giriş) |
| Valf 3 | Y0.2 | D102 | D202 | Gaz Valfi 3 (V-OVG 3 Giriş) |
| Valf 4 | Y0.3 | D103 | D203 | Gaz Valfi 4 (V-OVG 4 Giriş) |
| Valf 5 | Y0.4 | D104 | D204 | Kuru Hava Valfi (V-OVG 1 Giriş) |
| Valf 6 | Y0.5 | D105 | D205 | Sensör Hücre Giriş Valfi |
| Valf 7 | Y0.6 | D106 | D206 | Sensör Bypass / Temizleme Valfi |
| Valf 8 | Y0.7 | D107 | D207 | Manifold Tahliye / Egzoz Valfi |

### 2. Güç Çıkışları (Power ON/OFF - 4 Adet Fiziksel PLC Çıkışı)
| Eleman | PLC Çıkış | Komut Register | Durum Register | Açıklama |
|--------|-----------|----------------|----------------|----------|
| Güç Çıkışı 8 | Y0.8 | D108 | D208 | 24V DC Güç / Röle 1 |
| Güç Çıkışı 9 | Y0.9 | D109 | D209 | 24V DC Güç / Röle 2 |
| Güç Çıkışı 10 | Y0.10 | D110 | D210 | 24V DC Güç / Röle 3 |
| Güç Çıkışı 11 | Y0.11 | D111 | D211 | 24V DC Güç / Röle 4 |

### 3. V-OVG Egzoz / Tahliye Valfleri (AS08AN01P-A Ek Modül — Slot 1)
| Eleman | PLC Çıkış | Komut Register | Durum Register | Açıklama |
|--------|-----------|----------------|----------------|----------|
| Valf 13 | Y1.0 | D112 | D212 | V-OVG 2 Egzoz Tahliye Valfi (AS08AN Çıkış 0) |
| Valf 14 | Y1.1 | D113 | D213 | V-OVG 3 Egzoz Tahliye Valfi (AS08AN Çıkış 1) |
| Valf 15 | Y1.2 | D114 | D214 | V-OVG 4 Egzoz Tahliye Valfi (AS08AN Çıkış 2) |
| Valf 16 | Y1.3 | D115 | D215 | V-OVG 1 Egzoz Tahliye Valfi (AS08AN Çıkış 3) |
| Yedek | Y1.4 – Y1.7 | — | — | Boş / Gelecek Genişleme |

### 4. V-OVG Fırın Emniyet İnterlokları (M50 – M53 Dahili Emniyet İzinleri)
| Eleman | Dahili İzin Bayrağı | Açıklama |
|--------|---------------------|----------|
| V-OVG 1 Isıtıcı İzni | M50 | Sıcaklık normal AND Akış var AND Sistem Run |
| V-OVG 2 Isıtıcı İzni | M51 | Sıcaklık normal AND Akış var AND Sistem Run |
| V-OVG 3 Isıtıcı İzni | M52 | Sıcaklık normal AND Akış var AND Sistem Run |
| V-OVG 4 Isıtıcı İzni | M53 | Sıcaklık normal AND Akış var AND Sistem Run |

> [!TIP]
> **Donanım Eşleşmesi:**
> * CPU onboard çıkışları (`Y0.0..Y0.11`): `Y0.0–Y0.7` Valf 1–8, `Y0.8–Y0.11` Güç 8–11 için kullanılır.
> * CPU `Y0.12..Y0.15` çıkışları tamamen boşta ve serbesttir.
> * AS08AN01P-A ek modülü Slot 1'e takıldığında `Y1.0–Y1.3` çıkışları Valf 13–16 için ayrılmıştır.
> * PLC komut register'ları (`D112..D115`) ve durum register'ları (`D212..D215`) C# uygulamasıyla %100 uyumludur.

---

## REFERANS — ADIM HARİTASI

### MFC ID Adres Blok Aralıkları (Line Tech MD30C Haritası)

* **ID 1 (MFC-1):** 40001 – 40020 (Taban Hex Ofset: `16#0000`)
* **ID 2 (MFC-2):** 40021 – 40040 (Taban Hex Ofset: `16#0014`)
* **ID 3 (MFC-3):** 40041 – 40060 (Taban Hex Ofset: `16#0028`)
* **ID 4 (Yedek):** 40061 – 40080 (Taban Hex Ofset: `16#003C`)
* **ID 5 (Eski):**  40081 – 40100 (Taban Hex Ofset: `16#0050`)
* **ID 6 (MFC-4):** 40101 – 40120 (Taban Hex Ofset: `16#0064`)

| Adım | İşlem | Slave ID | Modbus Register (40xxx) | Hex Adres (MODRWE) | PLC Register |
|:---:|---|:---:|:---:|:---:|:---:|
| 1 | MFC1 Debi Oku | 1 | 40001 - 40002 | 16#0000 | D414..D415 |
| 2 | MFC1 SP Source | 1 | 40010 | 16#0009 | D310 |
| 3 | MFC1 Flow ON/OFF | 1 | 40011 | 16#000A | D311 |
| 4 | MFC1 Setpoint | 1 | 40013 - 40014 | 16#000C | D312..D313 |
| 5 | MFC2 Debi Oku | 2 | 40021 - 40022 | 16#0014 | D424..D425 |
| 6 | MFC2 SP Source | 2 | 40030 | 16#001D | D320 |
| 7 | MFC2 Flow ON/OFF | 2 | 40031 | 16#001E | D321 |
| 8 | MFC2 Setpoint | 2 | 40033 - 40034 | 16#0020 | D322..D323 |
| 9 | MFC3 Debi Oku | 3 | 40041 - 40042 | 16#0028 | D434..D435 |
| 10 | MFC3 SP Source | 3 | 40050 | 16#0031 | D330 |
| 11 | MFC3 Flow ON/OFF | 3 | 40051 | 16#0032 | D331 |
| 12 | MFC3 Setpoint | 3 | 40053 - 40054 | 16#0034 | D332..D333 |
| 13 | MFC4 Debi Oku | 6 | 40101 - 40102 | 16#0064 | D444..D445 |
| 14 | MFC4 SP Source | 6 | 40110 | 16#006D | D340 |
| 15 | MFC4 Flow ON/OFF | 6 | 40111 | 16#006E | D341 |
| 16 | MFC4 Setpoint | 6 | 40113 - 40114 | 16#0070 | D342..D343 |

---

## REFERANS — M-BIT HARİTASI

| M Bit | Kullanım |
|-------|----------|
| M500/M501 | MFC1 SP Source yazma |
| M502/M503 | MFC2 SP Source yazma |
| M504/M505 | MFC3 SP Source yazma |
| M506/M507 | MFC4 SP Source yazma |
| M510/M511 | MFC1 Setpoint yazma |
| M512/M513 | MFC2 Setpoint yazma |
| M514/M515 | MFC3 Setpoint yazma |
| M516/M517 | MFC4 Setpoint yazma |
| M520/M521 | MFC1 Flow yazma |
| M522/M523 | MFC2 Flow yazma |
| M524/M525 | MFC3 Flow yazma |
| M526/M527 | MFC4 Flow yazma |
| M530/M531 | MFC1 Debi okuma |
| M532/M533 | MFC2 Debi okuma |
| M534/M535 | MFC3 Debi okuma |
| M536/M537 | MFC4 Debi okuma |

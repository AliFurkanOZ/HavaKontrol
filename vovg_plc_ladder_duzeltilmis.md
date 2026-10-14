# V-OVG PLC Entegrasyonu — 4 Cihaz (8 Kontrolcü) Kesin ve Düzeltilmiş Ladder Diyagramı (v4.0)

Bu doküman, **Delta AS228P PLC (COM1: RS-485, 19200 Baud, 8-N-1)** ile **4 Adet V-OVG Cihazı (Toplam 8 Adet Eurotherm 3200 Serisi Sıcaklık ve Akış Kontrolcüsü)** arasındaki Modbus RTU haberleşmesini yöneten **kesin, kilitlenmeyen ve kanıtlanmış** ladder diyagramıdır.

> [!IMPORTANT]
> **v4.0 Yenilikleri ve Cihaz ID Eşleşmeleri:**
> 1. **4 Cihaz Desteği:** 
>    * **V-OVG 1:** Sıcaklık = **Slave 3**, Akış = **Slave 4**
>    * **V-OVG 2:** Sıcaklık = **Slave 5**, Akış = **Slave 6**
>    * **V-OVG 3:** Sıcaklık = **Slave 7**, Akış = **Slave 8**
>    * **V-OVG 4:** Sıcaklık = **Slave 9**, Akış = **Slave 10**
> 2. **24 Adımlı Kilitlenmeyen Çevrim (24 × 100ms = 2.4s):** Adım sayacı `D456` 1'den 24'e kadar döner. Cihaz kapalı veya kablo kopuk olsa dahi `T1` zamanlayıcısı sayesinde döngü asla takılmaz.
> 3. **Tampon/Çakışmasız MODRWE Bayrakları:** Her adım için 10'ar aralıkla ayrılmış ayrık `M` bayrakları (`M550..M600`, `M700..M870`).
> 4. **İlk Açılışta Setpoint Koruma:** Cihazların fabrika/mevcut ayarlarının sıfırlanmaması için ilk başarılı okumada SP shadow register'ına aktarılır (`M632..M639`).
> 5. **Çakışmasız PLC Register Haritası:** MFC register'ları ve MFC gölgeleri (D510..D542) ile çakışma olmaması için V-OVG 2 durumları `D600`, V-OVG 3 durumları `D620`, V-OVG 4 durumları `D640` alanına yerleştirilmiştir. `plc_ladder_final.md`'deki MFC negatif setpoint scratch register'ları ise `D490..D493`'e alınarak tam uyum sağlanmıştır.
> 6. **Fiziksel Çıkış (Y) Netliği:** AS228P CPU üzerindeki 16 çıkış: Valf 1..8 -> `Y0.0..Y0.7`, Güç Çıkışları 8..11 -> `Y0.8..Y0.11`, `Y0.12..Y0.15` serbest/yedektir. Valf 13..16 (V-OVG egzoz valfleri) ise sağdaki **AS08AN01P-A ek çıkış modülü (Slot 1: `Y1.0..Y1.3`)** üzerinden kontrol edilir. Fırın ısıtıcı güvenlik izinleri dahili `M50..M53` bayraklarıyla yönetilir (veya CPU'daki boş `Y0.12..Y0.15` klemenslerine bağlanabilir). PLC çıkışlarında sıfır çakışma ve sıfır çift bobin garanti edilmiştir.

---

## 1. Kullanılan Register ve Bit Tablosu

### V-OVG 1 (Sıcaklık: Slave 3 | Akış: Slave 4)
| Register / Bit | Değişken Adı | Açıklama |
|---|---|---|
| **`D350`** | `Vovg1_Temp_SP_Cmd` | PC Sıcaklık Hedef Değeri (°C x 10, örn: 45.0°C = 450) |
| **`D351`** | `Vovg1_Flow_SP_Cmd` | PC Akış Hedef Değeri (sccm, 0–250) |
| **`D470`** | `Vovg1_Temp_PV` | Ölçülen Sıcaklık (Reg 1, °C x 10) |
| **`D471`** | `Vovg1_Temp_TargetSP` | Cihaz Hedef SP (Reg 2, °C x 10) |
| **`D472`** | `Vovg1_Temp_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D473`** | `Vovg1_Temp_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D474`** | `Vovg1_Temp_WorkingSP`| Aktif Çalışma SP (Reg 5, °C x 10) |
| **`D475`** | `Vovg1_Flow_PV` | Ölçülen Akış (Reg 1, sccm) |
| **`D476`** | `Vovg1_Flow_TargetSP` | Cihaz Hedef SP (Reg 2, sccm) |
| **`D477`** | `Vovg1_Flow_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D478`** | `Vovg1_Flow_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D479`** | `Vovg1_Flow_WorkingSP`| Aktif Çalışma SP (Reg 5, sccm) |
| **`D480`** | `Vovg1_Temp_Comm_OK` | **Sıcaklık İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D481`** | `Vovg1_Temp_Alarm1` | Sıcaklık Alarm 1 (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D482`** | `Vovg1_Flow_Comm_OK` | **Akış İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D483`** | `Vovg1_Flow_Alarm1` | Akış Alarm 1 / Düşük Akış (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D550`** | `Vovg1_Temp_Shadow` | Sıcaklık Setpoint Gölge (Shadow) Register'ı |
| **`D551`** | `Vovg1_Flow_Shadow` | Akış Setpoint Gölge (Shadow) Register'ı |
| **`M632`** | `Vovg1_Temp_SP_Init` | İlk Açılış Sıcaklık SP Okundu Bayrağı |
| **`M633`** | `Vovg1_Flow_SP_Init` | İlk Açılış Akış SP Okundu Bayrağı |

---

### V-OVG 2 (Sıcaklık: Slave 5 | Akış: Slave 6)
| Register / Bit | Değişken Adı | Açıklama |
|---|---|---|
| **`D360`** | `Vovg2_Temp_SP_Cmd` | PC Sıcaklık Hedef Değeri (°C x 10) |
| **`D361`** | `Vovg2_Flow_SP_Cmd` | PC Akış Hedef Değeri (sccm, 0–250) |
| **`D600`** | `Vovg2_Temp_PV` | Ölçülen Sıcaklık (Reg 1, °C x 10) |
| **`D601`** | `Vovg2_Temp_TargetSP` | Cihaz Hedef SP (Reg 2, °C x 10) |
| **`D602`** | `Vovg2_Temp_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D603`** | `Vovg2_Temp_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D604`** | `Vovg2_Temp_WorkingSP`| Aktif Çalışma SP (Reg 5, °C x 10) |
| **`D605`** | `Vovg2_Flow_PV` | Ölçülen Akış (Reg 1, sccm) |
| **`D606`** | `Vovg2_Flow_TargetSP` | Cihaz Hedef SP (Reg 2, sccm) |
| **`D607`** | `Vovg2_Flow_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D608`** | `Vovg2_Flow_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D609`** | `Vovg2_Flow_WorkingSP`| Aktif Çalışma SP (Reg 5, sccm) |
| **`D610`** | `Vovg2_Temp_Comm_OK` | **Sıcaklık İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D611`** | `Vovg2_Temp_Alarm1` | Sıcaklık Alarm 1 (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D612`** | `Vovg2_Flow_Comm_OK` | **Akış İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D613`** | `Vovg2_Flow_Alarm1` | Akış Alarm 1 / Düşük Akış (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D552`** | `Vovg2_Temp_Shadow` | Sıcaklık Setpoint Gölge (Shadow) Register'ı |
| **`D553`** | `Vovg2_Flow_Shadow` | Akış Setpoint Gölge (Shadow) Register'ı |
| **`M634`** | `Vovg2_Temp_SP_Init` | İlk Açılış Sıcaklık SP Okundu Bayrağı |
| **`M635`** | `Vovg2_Flow_SP_Init` | İlk Açılış Akış SP Okundu Bayrağı |

---

### V-OVG 3 (Sıcaklık: Slave 7 | Akış: Slave 8)
| Register / Bit | Değişken Adı | Açıklama |
|---|---|---|
| **`D370`** | `Vovg3_Temp_SP_Cmd` | PC Sıcaklık Hedef Değeri (°C x 10) |
| **`D371`** | `Vovg3_Flow_SP_Cmd` | PC Akış Hedef Değeri (sccm, 0–250) |
| **`D620`** | `Vovg3_Temp_PV` | Ölçülen Sıcaklık (Reg 1, °C x 10) |
| **`D621`** | `Vovg3_Temp_TargetSP` | Cihaz Hedef SP (Reg 2, °C x 10) |
| **`D622`** | `Vovg3_Temp_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D623`** | `Vovg3_Temp_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D624`** | `Vovg3_Temp_WorkingSP`| Aktif Çalışma SP (Reg 5, °C x 10) |
| **`D625`** | `Vovg3_Flow_PV` | Ölçülen Akış (Reg 1, sccm) |
| **`D626`** | `Vovg3_Flow_TargetSP` | Cihaz Hedef SP (Reg 2, sccm) |
| **`D627`** | `Vovg3_Flow_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D628`** | `Vovg3_Flow_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D629`** | `Vovg3_Flow_WorkingSP`| Aktif Çalışma SP (Reg 5, sccm) |
| **`D630`** | `Vovg3_Temp_Comm_OK` | **Sıcaklık İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D631`** | `Vovg3_Temp_Alarm1` | Sıcaklık Alarm 1 (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D632`** | `Vovg3_Flow_Comm_OK` | **Akış İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D633`** | `Vovg3_Flow_Alarm1` | Akış Alarm 1 / Düşük Akış (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D554`** | `Vovg3_Temp_Shadow` | Sıcaklık Setpoint Gölge (Shadow) Register'ı |
| **`D555`** | `Vovg3_Flow_Shadow` | Akış Setpoint Gölge (Shadow) Register'ı |
| **`M636`** | `Vovg3_Temp_SP_Init` | İlk Açılış Sıcaklık SP Okundu Bayrağı |
| **`M637`** | `Vovg3_Flow_SP_Init` | İlk Açılış Akış SP Okundu Bayrağı |

---

### V-OVG 4 (Sıcaklık: Slave 9 | Akış: Slave 10)
| Register / Bit | Değişken Adı | Açıklama |
|---|---|---|
| **`D380`** | `Vovg4_Temp_SP_Cmd` | PC Sıcaklık Hedef Değeri (°C x 10) |
| **`D381`** | `Vovg4_Flow_SP_Cmd` | PC Akış Hedef Değeri (sccm, 0–250) |
| **`D640`** | `Vovg4_Temp_PV` | Ölçülen Sıcaklık (Reg 1, °C x 10) |
| **`D641`** | `Vovg4_Temp_TargetSP` | Cihaz Hedef SP (Reg 2, °C x 10) |
| **`D642`** | `Vovg4_Temp_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D643`** | `Vovg4_Temp_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D644`** | `Vovg4_Temp_WorkingSP`| Aktif Çalışma SP (Reg 5, °C x 10) |
| **`D645`** | `Vovg4_Flow_PV` | Ölçülen Akış (Reg 1, sccm) |
| **`D646`** | `Vovg4_Flow_TargetSP` | Cihaz Hedef SP (Reg 2, sccm) |
| **`D647`** | `Vovg4_Flow_ManualOut`| Manuel Çıkış (Reg 3) |
| **`D648`** | `Vovg4_Flow_OutputPwr`| Çıkış Gücü (Reg 4, % x 10) |
| **`D649`** | `Vovg4_Flow_WorkingSP`| Aktif Çalışma SP (Reg 5, sccm) |
| **`D650`** | `Vovg4_Temp_Comm_OK` | **Sıcaklık İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D651`** | `Vovg4_Temp_Alarm1` | Sıcaklık Alarm 1 (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D652`** | `Vovg4_Flow_Comm_OK` | **Akış İletişim Durumu (1 = OK, 0 = Hata/Yok)** |
| **`D653`** | `Vovg4_Flow_Alarm1` | Akış Alarm 1 / Düşük Akış (Reg 294, 0 = Normal, 1 = Alarm) |
| **`D556`** | `Vovg4_Temp_Shadow` | Sıcaklık Setpoint Gölge (Shadow) Register'ı |
| **`D557`** | `Vovg4_Flow_Shadow` | Akış Setpoint Gölge (Shadow) Register'ı |
| **`M638`** | `Vovg4_Temp_SP_Init` | İlk Açılış Sıcaklık SP Okundu Bayrağı |
| **`M639`** | `Vovg4_Flow_SP_Init` | İlk Açılış Akış SP Okundu Bayrağı |

---

### Kontrol, Emniyet ve Adım Değişkenleri
| Adres | Değişken Adı | Açıklama |
|---|---|---|
| **`D456`** | `Eurotherm_CommStep` | COM1 Çevrim Adım Sayacı (1 .. 24) |
| **`T1`** | `CommTimer_COM1` | 100ms Çevrim Zamanlayıcısı |
| **`M0`** | `System_Run` | Sistem Çalışıyor (Start/Stop Mühürleme: X0.0, X0.1, X0.2) |
| **`M50`** | `Oven1_Heat_Permissive`| V-OVG 1 Isıtıcı Çalışma İzni (Güvenlik İnterlok Bayrağı) |
| **`M51`** | `Oven2_Heat_Permissive`| V-OVG 2 Isıtıcı Çalışma İzni (Güvenlik İnterlok Bayrağı) |
| **`M52`** | `Oven3_Heat_Permissive`| V-OVG 3 Isıtıcı Çalışma İzni (Güvenlik İnterlok Bayrağı) |
| **`M53`** | `Oven4_Heat_Permissive`| V-OVG 4 Isıtıcı Çalışma İzni (Güvenlik İnterlok Bayrağı) |
| **`Y1.0`** | `Valf13_Output` | VALF 13 Fiziksel Çıkışı (V-OVG 2 Egzoz Tahliye, AS08AN Çıkış 0, D112) |
| **`Y1.1`** | `Valf14_Output` | VALF 14 Fiziksel Çıkışı (V-OVG 3 Egzoz Tahliye, AS08AN Çıkış 1, D113) |
| **`Y1.2`** | `Valf15_Output` | VALF 15 Fiziksel Çıkışı (V-OVG 4 Egzoz Tahliye, AS08AN Çıkış 2, D114) |
| **`Y1.3`** | `Valf16_Output` | VALF 16 Fiziksel Çıkışı (V-OVG 1 Egzoz Tahliye, AS08AN Çıkış 3, D115) |

> [!IMPORTANT]
> **V-OVG EGZOZ / TAHLİYE VALFLERİ (VALF 13, 14, 15, 16) PLC ENTEGRASYONU:**
> - VALF 13, 14, 15, 16 PLC'ye **tam uyumlu** hale getirilmiştir.
> - **Komut Register'ları:** `D112` (Valf 13), `D113` (Valf 14), `D114` (Valf 15), `D115` (Valf 16)
> - **Durum Register'ları:** `D212` (Valf 13), `D213` (Valf 14), `D214` (Valf 15), `D215` (Valf 16)
> - **PLC Çıkış Eşleşmesi:** AS08AN01P-A Slot 1 genişleme modülü çıkışlarında `Y1.0..Y1.3` valflere tahsis edilmiştir.
> - Fırın emniyet kontrolleri için `M50..M53` izin bayrakları dahili hafızada çalışır; böylece PLC'de hiçbir bobin (coil) veya register çakışması yaşanmaz.

---

## 2. 24 Adımlı Çevrim Mimarisi

```
Toplam Çevrim Süresi: 24 Adım × 100ms = 2400ms (2.4 Saniye)

[ V-OVG 1 ]
Adım 1: Slave 3 Temp PV Oku       ──> Adım 2: Slave 3 Temp Alm Oku     ──> Adım 3: Slave 3 Temp SP Yaz
Adım 4: Slave 4 Flow PV Oku       ──> Adım 5: Slave 4 Flow Alm Oku     ──> Adım 6: Slave 4 Flow SP Yaz

[ V-OVG 2 ]
Adım 7: Slave 5 Temp PV Oku       ──> Adım 8: Slave 5 Temp Alm Oku     ──> Adım 9: Slave 5 Temp SP Yaz
Adım 10: Slave 6 Flow PV Oku      ──> Adım 11: Slave 6 Flow Alm Oku    ──> Adım 12: Slave 6 Flow SP Yaz

[ V-OVG 3 ]
Adım 13: Slave 7 Temp PV Oku      ──> Adım 14: Slave 7 Temp Alm Oku    ──> Adım 15: Slave 7 Temp SP Yaz
Adım 16: Slave 8 Flow PV Oku      ──> Adım 17: Slave 8 Flow Alm Oku    ──> Adım 18: Slave 8 Flow SP Yaz

[ V-OVG 4 ]
Adım 19: Slave 9 Temp PV Oku      ──> Adım 20: Slave 9 Temp Alm Oku    ──> Adım 21: Slave 9 Temp SP Yaz
Adım 22: Slave 10 Flow PV Oku     ──> Adım 23: Slave 10 Flow Alm Oku   ──> Adım 24: Slave 10 Flow SP Yaz
   ^                                                                              │
   └──────────────────────────────────────────────────────────────────────────────┘
```

---

## 3. ISPSoft Ladder Networkleri

### BÖLÜM 1 — İlk Açılış (First Scan) ve Sıfırlama
```text
Network 1: Adım Sayacı Başlatma
|---[ M1002 ]-----------------------------------[ MOV  K1  D456 ]---|

Network 2: İletişim Bayraklarını Sıfırla (V-OVG 1, 2, 3, 4)
|---[ M1002 ]-----------------------------------[ MOV  K0  D480 ]---|  (* V-OVG 1 Temp Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D482 ]---|  (* V-OVG 1 Flow Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D610 ]---|  (* V-OVG 2 Temp Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D612 ]---|  (* V-OVG 2 Flow Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D630 ]---|  (* V-OVG 3 Temp Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D632 ]---|  (* V-OVG 3 Flow Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D650 ]---|  (* V-OVG 4 Temp Comm *)
|---[ M1002 ]-----------------------------------[ MOV  K0  D652 ]---|  (* V-OVG 4 Flow Comm *)

Network 3: İlk Açılış SP Senkronizasyon Bayraklarını Sıfırla
|---[ M1002 ]-----------------------------------( RST  M632 )-------|  (* V-OVG 1 Temp *)
|---[ M1002 ]-----------------------------------( RST  M633 )-------|  (* V-OVG 1 Flow *)
|---[ M1002 ]-----------------------------------( RST  M634 )-------|  (* V-OVG 2 Temp *)
|---[ M1002 ]-----------------------------------( RST  M635 )-------|  (* V-OVG 2 Flow *)
|---[ M1002 ]-----------------------------------( RST  M636 )-------|  (* V-OVG 3 Temp *)
|---[ M1002 ]-----------------------------------( RST  M637 )-------|  (* V-OVG 3 Flow *)
|---[ M1002 ]-----------------------------------( RST  M638 )-------|  (* V-OVG 4 Temp *)
|---[ M1002 ]-----------------------------------( RST  M639 )-------|  (* V-OVG 4 Flow *)

Network 4: MODRWE Tamamlama Bayraklarını Sıfırla
|---[ M1002 ]-----------------------------------( RST  M550 )-------|
|---[ M1002 ]-----------------------------------( RST  M560 )-------|
|---[ M1002 ]-----------------------------------( RST  M570 )-------|
|---[ M1002 ]-----------------------------------( RST  M580 )-------|
|---[ M1002 ]-----------------------------------( RST  M590 )-------|
|---[ M1002 ]-----------------------------------( RST  M600 )-------|
|---[ M1002 ]-----------------------------------( RST  M700 )-------|
|---[ M1002 ]-----------------------------------( RST  M710 )-------|
|---[ M1002 ]-----------------------------------( RST  M720 )-------|
|---[ M1002 ]-----------------------------------( RST  M730 )-------|
|---[ M1002 ]-----------------------------------( RST  M740 )-------|
|---[ M1002 ]-----------------------------------( RST  M750 )-------|
|---[ M1002 ]-----------------------------------( RST  M760 )-------|
|---[ M1002 ]-----------------------------------( RST  M770 )-------|
|---[ M1002 ]-----------------------------------( RST  M780 )-------|
|---[ M1002 ]-----------------------------------( RST  M790 )-------|
|---[ M1002 ]-----------------------------------( RST  M800 )-------|
|---[ M1002 ]-----------------------------------( RST  M810 )-------|
|---[ M1002 ]-----------------------------------( RST  M820 )-------|
|---[ M1002 ]-----------------------------------( RST  M830 )-------|
|---[ M1002 ]-----------------------------------( RST  M840 )-------|
|---[ M1002 ]-----------------------------------( RST  M850 )-------|
|---[ M1002 ]-----------------------------------( RST  M860 )-------|
|---[ M1002 ]-----------------------------------( RST  M870 )-------|
```

---

### BÖLÜM 2 — 100ms Adım Zamanlayıcısı (Çevrim Motoru)
```text
Network 5: 100ms Serbest Zamanlayıcı
|---[/T1 ]--------------------------------------[ TMR  T1  K1 ]-----|

Network 6: Adım Sayacını Artır
|---[ T1 ]--------------------------------------[ INC  D456 ]-------|

Network 7: Sınır Kontrolü (1..24 Döngüsü)
|---[ D456 > K24 ]------------------------------[ MOV  K1  D456 ]---|
|---[ D456 < K1 ]-------------------------------[ MOV  K1  D456 ]---|
```

---

### BÖLÜM 3 — V-OVG 1 İLETİŞİM ADIMLARI (ADIM 1 .. 6)

#### Adım 1: V-OVG 1 Sıcaklık PV, OP, WSP Oku (Slave 3, Reg 1–5 -> D470..D474)
```text
Network 8:
|---[ D456 = K1 ]---[ MODRWE  1  3  16#0003  16#0001  D470  5  M550 ]---|
|---[ M550 ]------------------------------------[ MOV  K1  D480 ]---|
|----+---[ M551 ]---+---------------------------[ MOV  K0  D480 ]---|
|    +---[ M552 ]---+
|---[ M550 ]---[/M632 ]-------------------------[ MOV  D474  D350 ]--|
|---[ M550 ]---[/M632 ]-------------------------[ MOV  D474  D550 ]--|
|---[ M550 ]---[/M632 ]-------------------------( SET  M632 )--------|
|----+---[ M550 ]---+---------------------------( RST  M550 )--------|
|    +---[ M551 ]---+---------------------------( RST  M551 )--------|
|    +---[ M552 ]---+---------------------------( RST  M552 )--------|
```

#### Adım 2: V-OVG 1 Sıcaklık Alarm 1 Oku (Slave 3, Reg 294 -> D481)
```text
Network 9:
|---[ D456 = K2 ]---[ MODRWE  1  3  16#0003  16#0126  D481  1  M560 ]---|
|----+---[ M560 ]---+---------------------------( RST  M560 )--------|
|    +---[ M561 ]---+---------------------------( RST  M561 )--------|
|    +---[ M562 ]---+---------------------------( RST  M562 )--------|
```

#### Adım 3: V-OVG 1 Sıcaklık Setpoint Yaz (Slave 3, Reg 24 <- D350)
```text
Network 10:
|---[ M570 ]------------------------------------[ MOV  D350  D550 ]--|
|----+---[ M570 ]---+---------------------------( RST  M570 )--------|
|    +---[ M571 ]---+---------------------------( RST  M571 )--------|
|    +---[ M572 ]---+---------------------------( RST  M572 )--------|
|---[ D456 = K3 ]---[ D350 <> D550 ]---[ D350 >= K250 ]---[ D350 <= K1000 ]---|
|                   [ MODRWE  1  3  16#0006  16#0018  D350  1  M570 ]---------|
```

#### Adım 4: V-OVG 1 Akış PV, OP, WSP Oku (Slave 4, Reg 1–5 -> D475..D479)
```text
Network 11:
|---[ D456 = K4 ]---[ MODRWE  1  4  16#0003  16#0001  D475  5  M580 ]---|
|---[ M580 ]------------------------------------[ MOV  K1  D482 ]---|
|----+---[ M581 ]---+---------------------------[ MOV  K0  D482 ]---|
|    +---[ M582 ]---+
|---[ M580 ]---[/M633 ]-------------------------[ MOV  D479  D351 ]--|
|---[ M580 ]---[/M633 ]-------------------------[ MOV  D479  D551 ]--|
|---[ M580 ]---[/M633 ]-------------------------( SET  M633 )--------|
|----+---[ M580 ]---+---------------------------( RST  M580 )--------|
|    +---[ M581 ]---+---------------------------( RST  M581 )--------|
|    +---[ M582 ]---+---------------------------( RST  M582 )--------|
```

#### Adım 5: V-OVG 1 Akış Alarm 1 Oku (Slave 4, Reg 294 -> D483)
```text
Network 12:
|---[ D456 = K5 ]---[ MODRWE  1  4  16#0003  16#0126  D483  1  M590 ]---|
|----+---[ M590 ]---+---------------------------( RST  M590 )--------|
|    +---[ M591 ]---+---------------------------( RST  M591 )--------|
|    +---[ M592 ]---+---------------------------( RST  M592 )--------|
```

#### Adım 6: V-OVG 1 Akış Setpoint Yaz (Slave 4, Reg 24 <- D351)
```text
Network 13:
|---[ M600 ]------------------------------------[ MOV  D351  D551 ]--|
|----+---[ M600 ]---+---------------------------( RST  M600 )--------|
|    +---[ M601 ]---+---------------------------( RST  M601 )--------|
|    +---[ M602 ]---+---------------------------( RST  M602 )--------|
|---[ D456 = K6 ]---[ D351 <> D551 ]---[ D351 >= K0 ]---[ D351 <= K250 ]-------|
|                   [ MODRWE  1  4  16#0006  16#0018  D351  1  M600 ]---------|
```

---

### BÖLÜM 4 — V-OVG 2 İLETİŞİM ADIMLARI (ADIM 7 .. 12)

#### Adım 7: V-OVG 2 Sıcaklık PV, OP, WSP Oku (Slave 5, Reg 1–5 -> D600..D604)
```text
Network 14:
|---[ D456 = K7 ]---[ MODRWE  1  5  16#0003  16#0001  D600  5  M700 ]---|
|---[ M700 ]------------------------------------[ MOV  K1  D610 ]---|
|----+---[ M701 ]---+---------------------------[ MOV  K0  D610 ]---|
|    +---[ M702 ]---+
|---[ M700 ]---[/M634 ]-------------------------[ MOV  D604  D360 ]--|
|---[ M700 ]---[/M634 ]-------------------------[ MOV  D604  D552 ]--|
|---[ M700 ]---[/M634 ]-------------------------( SET  M634 )--------|
|----+---[ M700 ]---+---------------------------( RST  M700 )--------|
|    +---[ M701 ]---+---------------------------( RST  M701 )--------|
|    +---[ M702 ]---+---------------------------( RST  M702 )--------|
```

#### Adım 8: V-OVG 2 Sıcaklık Alarm 1 Oku (Slave 5, Reg 294 -> D611)
```text
Network 15:
|---[ D456 = K8 ]---[ MODRWE  1  5  16#0003  16#0126  D611  1  M710 ]---|
|----+---[ M710 ]---+---------------------------( RST  M710 )--------|
|    +---[ M711 ]---+---------------------------( RST  M711 )--------|
|    +---[ M712 ]---+---------------------------( RST  M712 )--------|
```

#### Adım 9: V-OVG 2 Sıcaklık Setpoint Yaz (Slave 5, Reg 24 <- D360)
```text
Network 16:
|---[ M720 ]------------------------------------[ MOV  D360  D552 ]--|
|----+---[ M720 ]---+---------------------------( RST  M720 )--------|
|    +---[ M721 ]---+---------------------------( RST  M721 )--------|
|    +---[ M722 ]---+---------------------------( RST  M722 )--------|
|---[ D456 = K9 ]---[ D360 <> D552 ]---[ D360 >= K250 ]---[ D360 <= K1000 ]---|
|                   [ MODRWE  1  5  16#0006  16#0018  D360  1  M720 ]---------|
```

#### Adım 10: V-OVG 2 Akış PV, OP, WSP Oku (Slave 6, Reg 1–5 -> D605..D609)
```text
Network 17:
|---[ D456 = K10 ]--[ MODRWE  1  6  16#0003  16#0001  D605  5  M730 ]---|
|---[ M730 ]------------------------------------[ MOV  K1  D612 ]---|
|----+---[ M731 ]---+---------------------------[ MOV  K0  D612 ]---|
|    +---[ M732 ]---+
|---[ M730 ]---[/M635 ]-------------------------[ MOV  D609  D361 ]--|
|---[ M730 ]---[/M635 ]-------------------------[ MOV  D609  D553 ]--|
|---[ M730 ]---[/M635 ]-------------------------( SET  M635 )--------|
|----+---[ M730 ]---+---------------------------( RST  M730 )--------|
|    +---[ M731 ]---+---------------------------( RST  M731 )--------|
|    +---[ M732 ]---+---------------------------( RST  M732 )--------|
```

#### Adım 11: V-OVG 2 Akış Alarm 1 Oku (Slave 6, Reg 294 -> D613)
```text
Network 18:
|---[ D456 = K11 ]--[ MODRWE  1  6  16#0003  16#0126  D613  1  M740 ]---|
|----+---[ M740 ]---+---------------------------( RST  M740 )--------|
|    +---[ M741 ]---+---------------------------( RST  M741 )--------|
|    +---[ M742 ]---+---------------------------( RST  M742 )--------|
```

#### Adım 12: V-OVG 2 Akış Setpoint Yaz (Slave 6, Reg 24 <- D361)
```text
Network 19:
|---[ M750 ]------------------------------------[ MOV  D361  D553 ]--|
|----+---[ M750 ]---+---------------------------( RST  M750 )--------|
|    +---[ M751 ]---+---------------------------( RST  M751 )--------|
|    +---[ M752 ]---+---------------------------( RST  M752 )--------|
|---[ D456 = K12 ]--[ D361 <> D553 ]---[ D361 >= K0 ]---[ D361 <= K250 ]-------|
|                   [ MODRWE  1  6  16#0006  16#0018  D361  1  M750 ]---------|
```

---

### BÖLÜM 5 — V-OVG 3 İLETİŞİM ADIMLARI (ADIM 13 .. 18)

#### Adım 13: V-OVG 3 Sıcaklık PV, OP, WSP Oku (Slave 7, Reg 1–5 -> D620..D624)
```text
Network 20:
|---[ D456 = K13 ]--[ MODRWE  1  7  16#0003  16#0001  D620  5  M760 ]---|
|---[ M760 ]------------------------------------[ MOV  K1  D630 ]---|
|----+---[ M761 ]---+---------------------------[ MOV  K0  D630 ]---|
|    +---[ M762 ]---+
|---[ M760 ]---[/M636 ]-------------------------[ MOV  D624  D370 ]--|
|---[ M760 ]---[/M636 ]-------------------------[ MOV  D624  D554 ]--|
|---[ M760 ]---[/M636 ]-------------------------( SET  M636 )--------|
|----+---[ M760 ]---+---------------------------( RST  M760 )--------|
|    +---[ M761 ]---+---------------------------( RST  M761 )--------|
|    +---[ M762 ]---+---------------------------( RST  M762 )--------|
```

#### Adım 14: V-OVG 3 Sıcaklık Alarm 1 Oku (Slave 7, Reg 294 -> D631)
```text
Network 21:
|---[ D456 = K14 ]--[ MODRWE  1  7  16#0003  16#0126  D631  1  M770 ]---|
|----+---[ M770 ]---+---------------------------( RST  M770 )--------|
|    +---[ M771 ]---+---------------------------( RST  M771 )--------|
|    +---[ M772 ]---+---------------------------( RST  M772 )--------|
```

#### Adım 15: V-OVG 3 Sıcaklık Setpoint Yaz (Slave 7, Reg 24 <- D370)
```text
Network 22:
|---[ M780 ]------------------------------------[ MOV  D370  D554 ]--|
|----+---[ M780 ]---+---------------------------( RST  M780 )--------|
|    +---[ M781 ]---+---------------------------( RST  M781 )--------|
|    +---[ M782 ]---+---------------------------( RST  M782 )--------|
|---[ D456 = K15 ]--[ D370 <> D554 ]---[ D370 >= K250 ]---[ D370 <= K1000 ]---|
|                   [ MODRWE  1  7  16#0006  16#0018  D370  1  M780 ]---------|
```

#### Adım 16: V-OVG 3 Akış PV, OP, WSP Oku (Slave 8, Reg 1–5 -> D625..D629)
```text
Network 23:
|---[ D456 = K16 ]--[ MODRWE  1  8  16#0003  16#0001  D625  5  M790 ]---|
|---[ M790 ]------------------------------------[ MOV  K1  D632 ]---|
|----+---[ M791 ]---+---------------------------[ MOV  K0  D632 ]---|
|    +---[ M792 ]---+
|---[ M790 ]---[/M637 ]-------------------------[ MOV  D629  D371 ]--|
|---[ M790 ]---[/M637 ]-------------------------[ MOV  D629  D555 ]--|
|---[ M790 ]---[/M637 ]-------------------------( SET  M637 )--------|
|----+---[ M790 ]---+---------------------------( RST  M790 )--------|
|    +---[ M791 ]---+---------------------------( RST  M791 )--------|
|    +---[ M792 ]---+---------------------------( RST  M792 )--------|
```

#### Adım 17: V-OVG 3 Akış Alarm 1 Oku (Slave 8, Reg 294 -> D633)
```text
Network 24:
|---[ D456 = K17 ]--[ MODRWE  1  8  16#0003  16#0126  D633  1  M800 ]---|
|----+---[ M800 ]---+---------------------------( RST  M800 )--------|
|    +---[ M801 ]---+---------------------------( RST  M801 )--------|
|    +---[ M802 ]---+---------------------------( RST  M802 )--------|
```

#### Adım 18: V-OVG 3 Akış Setpoint Yaz (Slave 8, Reg 24 <- D371)
```text
Network 25:
|---[ M810 ]------------------------------------[ MOV  D371  D555 ]--|
|----+---[ M810 ]---+---------------------------( RST  M810 )--------|
|    +---[ M811 ]---+---------------------------( RST  M811 )--------|
|    +---[ M812 ]---+---------------------------( RST  M812 )--------|
|---[ D456 = K18 ]--[ D371 <> D555 ]---[ D371 >= K0 ]---[ D371 <= K250 ]-------|
|                   [ MODRWE  1  8  16#0006  16#0018  D371  1  M810 ]---------|
```

---

### BÖLÜM 5.1 — V-OVG 4 İLETİŞİM ADIMLARI (ADIM 19 .. 24)

#### Adım 19: V-OVG 4 Sıcaklık PV, OP, WSP Oku (Slave 9, Reg 1–5 -> D640..D644)
```text
Network 25.1:
|---[ D456 = K19 ]--[ MODRWE  1  9  16#0003  16#0001  D640  5  M820 ]---|
|---[ M820 ]------------------------------------[ MOV  K1  D650 ]---|  (* Temp Comm OK *)
|----+---[ M821 ]---+---------------------------[ MOV  K0  D650 ]---|  (* Hata/Timeout *)
|    +---[ M822 ]---+
|---[ M820 ]---[/M638 ]-------------------------[ MOV  D644  D380 ]--|  (* İlk okumada WSP -> SP_Cmd *)
|---[ M820 ]---[/M638 ]-------------------------[ MOV  D644  D556 ]--|  (* İlk okumada WSP -> Shadow *)
|---[ M820 ]---[/M638 ]-------------------------( SET  M638 )--------|  (* SP Init tamamlandı *)
|----+---[ M820 ]---+---------------------------( RST  M820 )--------|
|    +---[ M821 ]---+---------------------------( RST  M821 )--------|
|    +---[ M822 ]---+---------------------------( RST  M822 )--------|
```

#### Adım 20: V-OVG 4 Sıcaklık Alarm 1 Oku (Slave 9, Reg 294 -> D651)
```text
Network 25.2:
|---[ D456 = K20 ]--[ MODRWE  1  9  16#0003  16#0126  D651  1  M830 ]---|
|----+---[ M830 ]---+---------------------------( RST  M830 )--------|
|    +---[ M831 ]---+---------------------------( RST  M831 )--------|
|    +---[ M832 ]---+---------------------------( RST  M832 )--------|
```

#### Adım 21: V-OVG 4 Sıcaklık Setpoint Yaz (Slave 9, Reg 24 <- D380)
```text
Network 25.3:
|---[ M840 ]------------------------------------[ MOV  D380  D556 ]--|  (* Başarılı yazmada Shadow güncelle *)
|----+---[ M840 ]---+---------------------------( RST  M840 )--------|
|    +---[ M841 ]---+---------------------------( RST  M841 )--------|
|    +---[ M842 ]---+---------------------------( RST  M842 )--------|
|---[ D456 = K21 ]--[ D380 <> D556 ]---[ D380 >= K250 ]---[ D380 <= K1000 ]---|
|                   [ MODRWE  1  9  16#0006  16#0018  D380  1  M840 ]---------|
```

#### Adım 22: V-OVG 4 Akış PV, OP, WSP Oku (Slave 10, Reg 1–5 -> D645..D649)
```text
Network 25.4:
|---[ D456 = K22 ]--[ MODRWE  1  10  16#0003  16#0001  D645  5  M850 ]---|
|---[ M850 ]------------------------------------[ MOV  K1  D652 ]---|  (* Flow Comm OK *)
|----+---[ M851 ]---+---------------------------[ MOV  K0  D652 ]---|  (* Hata/Timeout *)
|    +---[ M852 ]---+
|---[ M850 ]---[/M639 ]-------------------------[ MOV  D649  D381 ]--|  (* İlk okumada WSP -> SP_Cmd *)
|---[ M850 ]---[/M639 ]-------------------------[ MOV  D649  D557 ]--|  (* İlk okumada WSP -> Shadow *)
|---[ M850 ]---[/M639 ]-------------------------( SET  M639 )--------|  (* SP Init tamamlandı *)
|----+---[ M850 ]---+---------------------------( RST  M850 )--------|
|    +---[ M851 ]---+---------------------------( RST  M851 )--------|
|    +---[ M852 ]---+---------------------------( RST  M852 )--------|
```

#### Adım 23: V-OVG 4 Akış Alarm 1 Oku (Slave 10, Reg 294 -> D653)
```text
Network 25.5:
|---[ D456 = K23 ]--[ MODRWE  1  10  16#0003  16#0126  D653  1  M860 ]---|
|----+---[ M860 ]---+---------------------------( RST  M860 )--------|
|    +---[ M861 ]---+---------------------------( RST  M861 )--------|
|    +---[ M862 ]---+---------------------------( RST  M862 )--------|
```

#### Adım 24: V-OVG 4 Akış Setpoint Yaz (Slave 10, Reg 24 <- D381)
```text
Network 25.6:
|---[ M870 ]------------------------------------[ MOV  D381  D557 ]--|  (* Başarılı yazmada Shadow güncelle *)
|----+---[ M870 ]---+---------------------------( RST  M870 )--------|
|    +---[ M871 ]---+---------------------------( RST  M871 )--------|
|    +---[ M872 ]---+---------------------------( RST  M872 )--------|
|---[ D456 = K24 ]--[ D381 <> D557 ]---[ D381 >= K0 ]---[ D381 <= K250 ]--------|
|                   [ MODRWE  1  10  16#0006  16#0018  D381  1  M870 ]----------|
```

---

### BÖLÜM 6 — Güvenlik İnterlokları ve Fırın İzin Kontrolü

> [!IMPORTANT]
> Bu bölüm, sistemin emniyetini ve fırın ısıtıcı kontaktörlerini kontrol eder.
> İletişim koparsa veya aşırı sıcaklık / akış kesilmesi algılanırsa fırın ısıtıcı enerjisi anında kesilir!

```text
(* ========================================================================= *)
(* 1. V-OVG 1 EMNİYET KONTROLLERİ                                            *)
(* ========================================================================= *)
Network 26: V-OVG 1 Sıcaklık Arıza Kontrolü (Haberleşme kesilirse veya aşırı sıcaklık olursa)
|----+---[ D480 = K0 ]---+----------------------( M641 )-------------|
|    +---[ D470 > K1050 ]+  (* 105.0°C üzeri kritik koruma *)

Network 27: V-OVG 1 Akış Arıza Kontrolü (Haberleşme kesilirse veya düşük akış alarmı varsa)
|----+---[ D482 = K0 ]---+----------------------( M642 )-------------|
|    +---[ D483 <> K0 ]--+  (* Alarm 1 devrede ise *)

Network 28: V-OVG 1 Fırın Isıtıcı İzin Kontrolü (M50)
|---[ M0 ]---[/M641 ]---[/M642 ]----------------( M50 )--------------|
(* Şart: Sistem Çalışıyor (M0) AND Sıcaklık Arızası YOK AND Akış Arızası YOK *)
(* M50 = Fırın 1 Isıtıcı Güvenlik İzni Dahili Rölesi *)

(* ========================================================================= *)
(* 2. V-OVG 2 EMNİYET KONTROLLERİ                                            *)
(* ========================================================================= *)
Network 30: V-OVG 2 Sıcaklık Arıza Kontrolü (Haberleşme kesilirse veya aşırı sıcaklık olursa)
|----+---[ D610 = K0 ]---+----------------------( M643 )-------------|
|    +---[ D600 > K1050 ]+

Network 31: V-OVG 2 Akış Arıza Kontrolü (Haberleşme kesilirse veya düşük akış alarmı varsa)
|----+---[ D612 = K0 ]---+----------------------( M644 )-------------|
|    +---[ D613 <> K0 ]--+

Network 32: V-OVG 2 Fırın Isıtıcı İzin Kontrolü (M51)
|---[ M0 ]---[/M643 ]---[/M644 ]----------------( M51 )--------------|
(* M51 = Fırın 2 Isıtıcı Güvenlik İzni Dahili Rölesi *)

(* ========================================================================= *)
(* 3. V-OVG 3 EMNİYET KONTROLLERİ                                            *)
(* ========================================================================= *)
Network 34: V-OVG 3 Sıcaklık Arıza Kontrolü (Haberleşme kesilirse veya aşırı sıcaklık olursa)
|----+---[ D630 = K0 ]---+----------------------( M645 )-------------|
|    +---[ D620 > K1050 ]+

Network 35: V-OVG 3 Akış Arıza Kontrolü (Haberleşme kesilirse veya düşük akış alarmı varsa)
|----+---[ D632 = K0 ]---+----------------------( M646 )-------------|
|    +---[ D633 <> K0 ]--+

Network 36: V-OVG 3 Fırın Isıtıcı İzin Kontrolü (M52)
|---[ M0 ]---[/M645 ]---[/M646 ]----------------( M52 )--------------|
(* M52 = Fırın 3 Isıtıcı Güvenlik İzni Dahili Rölesi *)

(* ========================================================================= *)
(* 4. V-OVG 4 EMNİYET KONTROLLERİ                                            *)
(* ========================================================================= *)
Network 37.1: V-OVG 4 Sıcaklık Arıza Kontrolü (Haberleşme kesilirse veya aşırı sıcaklık olursa)
|----+---[ D650 = K0 ]---+----------------------( M647 )-------------|
|    +---[ D640 > K1050 ]+

Network 37.2: V-OVG 4 Akış Arıza Kontrolü (Haberleşme kesilirse veya düşük akış alarmı varsa)
|----+---[ D652 = K0 ]---+----------------------( M648 )-------------|
|    +---[ D653 <> K0 ]--+

Network 37.3: V-OVG 4 Fırın Isıtıcı İzin Kontrolü (M53)
|---[ M0 ]---[/M647 ]---[/M648 ]----------------( M53 )--------------|
(* M53 = Fırın 4 Isıtıcı Güvenlik İzni Dahili Rölesi *)

(* ========================================================================= *)
(* 5. SİSTEM GENEL START / STOP (M0 MÜHÜRLEME)                               *)
(* ========================================================================= *)
Network 38: Sistem Start / Stop
|---┬---[ X0.0 START ]---┬---[/X0.1 STOP ]---[/X0.2 E-STOP ]---( M0 )---|
|   └───[ M0 ]───────────┘
```

---

## 4. Donanım & ISPSoft Kontrol Listesi

1. **Delta PLC COM1 (RS-485) Port Ayarları (HWCONFIG):**
   * Mod: `Modbus Master`
   * Baud: `19200 bps`
   * Data Length: `8 bits`
   * Parity: `None`
   * Stop Bit: `1 bit`
2. **Fiziksel Kablolama:**
   * Tüm 8 Eurotherm cihazının `D+` hatları paralel olarak PLC COM1 `D+` ucuna,
   * Tüm 8 Eurotherm cihazının `D-` hatları paralel olarak PLC COM1 `D-` ucuna,
   * Ekranlama (Shield) ortak `SG / GND` klemensine bağlanmalıdır.
3. **Cihaz ID Ayarları (Eurotherm Ön Panelinden):**
   * V-OVG 1 Sıcaklık: `ADDR = 3`
   * V-OVG 1 Akış: `ADDR = 4`
   * V-OVG 2 Sıcaklık: `ADDR = 5`
   * V-OVG 2 Akış: `ADDR = 6`
   * V-OVG 3 Sıcaklık: `ADDR = 7`
   * V-OVG 3 Akış: `ADDR = 8`
   * V-OVG 4 Sıcaklık: `ADDR = 9`
   * V-OVG 4 Akış: `ADDR = 10`

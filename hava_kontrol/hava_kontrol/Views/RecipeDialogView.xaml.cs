using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using hava_kontrol.Models;
using hava_kontrol.ViewModels;
using Microsoft.Win32;

namespace hava_kontrol.Views
{
    /// <summary>
    /// Reçete Modu ve Otomasyon Sekans Yönetimi etkileşim mantığı.
    /// Endüstriyel otomasyon ve fiziki donanım kontrolleri için sıfır toleranslı sağlam mimari.
    /// </summary>
    public partial class RecipeDialogView : UserControl
    {
        public class ProductItem
        {
            public string DisplayName { get; set; } = string.Empty;
            public RecipeDeviceType DeviceType { get; set; }
            public int DeviceIndex { get; set; }
            public string Category { get; set; } = string.Empty;

            public override string ToString() => DisplayName;
        }

        private readonly List<ProductItem> _products = new();
        private RecipeStep? _editingStep;
        private int? _insertAtIndex;
        private bool _isInternalSelection;

        public RecipeDialogView()
        {
            InitializeComponent();
            InitializeProducts();
            Loaded += RecipeDialogView_Loaded;
            DataContextChanged += RecipeDialogView_DataContextChanged;
        }

        private MainViewModel? ViewModel => DataContext as MainViewModel;

        private void InitializeProducts()
        {
            _products.Clear();

            // 1. Kütle Akış Kontrolcüleri (MFC 1..4)
            _products.Add(new ProductItem { DisplayName = "MFC-1 (Hedef Gaz 1)", DeviceType = RecipeDeviceType.Mfc, DeviceIndex = 1, Category = "MFC" });
            _products.Add(new ProductItem { DisplayName = "MFC-2 (N2 / Taşıyıcı Gaz)", DeviceType = RecipeDeviceType.Mfc, DeviceIndex = 2, Category = "MFC" });
            _products.Add(new ProductItem { DisplayName = "MFC-3 (Kalibrasyon / Hava)", DeviceType = RecipeDeviceType.Mfc, DeviceIndex = 3, Category = "MFC" });
            _products.Add(new ProductItem { DisplayName = "MFC-4 (Özel Gaz Hattı)", DeviceType = RecipeDeviceType.Mfc, DeviceIndex = 4, Category = "MFC" });

            // 2. V-OVG Buharlaştırıcı Fırınlar (1..4)
            _products.Add(new ProductItem { DisplayName = "V-OVG 1 (Buharlaştırıcı Fırın 1 - Kuru Hava)", DeviceType = RecipeDeviceType.Vovg, DeviceIndex = 1, Category = "V-OVG" });
            _products.Add(new ProductItem { DisplayName = "V-OVG 2 (Buharlaştırıcı Fırın 2 - Gaz 2)", DeviceType = RecipeDeviceType.Vovg, DeviceIndex = 2, Category = "V-OVG" });
            _products.Add(new ProductItem { DisplayName = "V-OVG 3 (Buharlaştırıcı Fırın 3 - Gaz 3)", DeviceType = RecipeDeviceType.Vovg, DeviceIndex = 3, Category = "V-OVG" });
            _products.Add(new ProductItem { DisplayName = "V-OVG 4 (Buharlaştırıcı Fırın 4 - Gaz 4)", DeviceType = RecipeDeviceType.Vovg, DeviceIndex = 4, Category = "V-OVG" });

            // 3. Solenoid Valfler (12 Adet PLC Çıkışı: Valf 1..8 ve 13..16)
            _products.Add(new ProductItem { DisplayName = "Valf 1 (MFC-1 Giriş - Y0.0)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 1, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 2 (MFC-2 Giriş - Y0.1)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 2, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 3 (MFC-3 Giriş - Y0.2)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 3, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 4 (MFC-4 Giriş - Y0.3)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 4, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 5 (Kuru Hava Manifold - Y0.4)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 5, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 6 (Sensör Hücre Giriş - Y0.5)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 6, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 7 (Bypass / Temizleme - Y0.6)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 7, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 8 (Manifold Egzoz - Y0.7)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 8, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 13 (V-OVG 2 Egzoz - Y1.0)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 13, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 14 (V-OVG 3 Egzoz - Y1.1)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 14, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 15 (V-OVG 4 Egzoz - Y1.2)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 15, Category = "Valf (PLC)" });
            _products.Add(new ProductItem { DisplayName = "Valf 16 (V-OVG 1 Egzoz - Y1.3)", DeviceType = RecipeDeviceType.Valve, DeviceIndex = 16, Category = "Valf (PLC)" });

            // 4. PLC Güç Çıkışları (8..11)
            _products.Add(new ProductItem { DisplayName = "Güç Çıkışı 8 (PLC Röle 1)", DeviceType = RecipeDeviceType.PowerOutput, DeviceIndex = 8, Category = "Röle" });
            _products.Add(new ProductItem { DisplayName = "Güç Çıkışı 9 (PLC Röle 2)", DeviceType = RecipeDeviceType.PowerOutput, DeviceIndex = 9, Category = "Röle" });
            _products.Add(new ProductItem { DisplayName = "Güç Çıkışı 10 (PLC Röle 3)", DeviceType = RecipeDeviceType.PowerOutput, DeviceIndex = 10, Category = "Röle" });
            _products.Add(new ProductItem { DisplayName = "Güç Çıkışı 11 (PLC Röle 4)", DeviceType = RecipeDeviceType.PowerOutput, DeviceIndex = 11, Category = "Röle" });

            // 5. RIGOL DP831 DC Güç Kaynağı
            _products.Add(new ProductItem { DisplayName = "RIGOL DP831 - CH1 (+8V / 5A)", DeviceType = RecipeDeviceType.PowerSupply, DeviceIndex = 1, Category = "Güç Kaynağı" });
            _products.Add(new ProductItem { DisplayName = "RIGOL DP831 - CH2 (+30V / 2A)", DeviceType = RecipeDeviceType.PowerSupply, DeviceIndex = 2, Category = "Güç Kaynağı" });
            _products.Add(new ProductItem { DisplayName = "RIGOL DP831 - CH3 (-30V / 2A)", DeviceType = RecipeDeviceType.PowerSupply, DeviceIndex = 3, Category = "Güç Kaynağı" });

            // 6. Keysight SMU / Elektrometre
            _products.Add(new ProductItem { DisplayName = "Keysight SMU - Kanal 1 (Sensör)", DeviceType = RecipeDeviceType.Electrometer, DeviceIndex = 1, Category = "SMU" });
            _products.Add(new ProductItem { DisplayName = "Keysight SMU - Kanal 2 (Bias/İkincil)", DeviceType = RecipeDeviceType.Electrometer, DeviceIndex = 2, Category = "SMU" });

            CmbProductList.ItemsSource = _products;
            if (_products.Count > 0)
                CmbProductList.SelectedIndex = 0;
        }

        private void RecipeDialogView_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDataGridBinding();
        }

        private void RecipeDialogView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshDataGridBinding();
        }

        /// <summary>
        /// DataGrid'in ItemsSource bağını garantili olarak günceller ve ilk adımı seçer.
        /// </summary>
        private void RefreshDataGridBinding()
        {
            var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
            if (recipe != null)
            {
                StepsDataGrid.ItemsSource = recipe.Steps;
                if (recipe.Steps.Count > 0 && StepsDataGrid.SelectedItem == null)
                {
                    _isInternalSelection = true;
                    StepsDataGrid.SelectedIndex = 0;
                    _isInternalSelection = false;
                    LoadStepIntoForm(recipe.Steps[0]);
                }
            }
        }

        // ======================================
        // ÜRÜN SEÇİMİ VE DİNAMİK ALAN YÖNETİMİ
        // ======================================

        private void CmbProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbProductList.SelectedItem is not ProductItem selected)
                return;

            // Panellerin görünürlüklerini ayarla
            PanelMfcSettings.Visibility = selected.DeviceType == RecipeDeviceType.Mfc ? Visibility.Visible : Visibility.Collapsed;
            PanelVovgSettings.Visibility = selected.DeviceType == RecipeDeviceType.Vovg ? Visibility.Visible : Visibility.Collapsed;
            PanelValveSettings.Visibility = selected.DeviceType == RecipeDeviceType.Valve ? Visibility.Visible : Visibility.Collapsed;
            PanelPowerOutputSettings.Visibility = selected.DeviceType == RecipeDeviceType.PowerOutput ? Visibility.Visible : Visibility.Collapsed;
            PanelPowerSupplySettings.Visibility = selected.DeviceType == RecipeDeviceType.PowerSupply ? Visibility.Visible : Visibility.Collapsed;
            PanelSmuSettings.Visibility = selected.DeviceType == RecipeDeviceType.Electrometer ? Visibility.Visible : Visibility.Collapsed;

            // Eğer yeni adım modundaysak isim önerisi yap
            if (_editingStep == null)
            {
                TxtStepName.Text = selected.DeviceType switch
                {
                    RecipeDeviceType.Mfc => $"{selected.DisplayName} Akış Ayarı",
                    RecipeDeviceType.Vovg => $"{selected.DisplayName} Isıtma & Gaz",
                    RecipeDeviceType.Valve => $"{selected.DisplayName} Konumu",
                    RecipeDeviceType.PowerOutput => $"{selected.DisplayName} Durumu",
                    RecipeDeviceType.PowerSupply => $"{selected.DisplayName} Çıkış Ayarı",
                    RecipeDeviceType.Electrometer => $"{selected.DisplayName} Kaynak & Ölçüm",
                    _ => selected.DisplayName
                };
            }
        }

        // ======================================
        // ADIM FORMUNU DOLDURMA / TEMİZLEME
        // ======================================

        private void LoadStepIntoForm(RecipeStep step)
        {
            _editingStep = step;
            _insertAtIndex = null;

            TxtFormModeTitle.Text = $"✏️ ADIM #{step.StepNumber} DÜZENLE";
            TxtStepNumberBadge.Text = $"Adım {step.StepNumber}";
            BtnSaveStep.Content = "💾 Değişiklikleri Kaydet";

            // Ürünü seç
            var product = _products.FirstOrDefault(p => p.DeviceType == step.DeviceType && p.DeviceIndex == step.DeviceIndex);
            if (product != null)
            {
                CmbProductList.SelectedItem = product;
            }

            TxtStepName.Text = step.Name;
            TxtDurationSeconds.Text = step.DurationSeconds.ToString();

            // Parametreleri doldur
            TxtMfcFlow.Text = step.MfcFlowSccm.ToString("F1", CultureInfo.InvariantCulture);
            ChkMfcFlowEnabled.IsChecked = step.MfcFlowEnabled;

            TxtVovgTemp.Text = step.VovgTempC.ToString("F1", CultureInfo.InvariantCulture);
            TxtVovgFlow.Text = step.VovgFlowSccm.ToString("F0", CultureInfo.InvariantCulture);

            ChkValveOpen.IsChecked = step.ValveIsOpen;
            ChkPowerOutputOpen.IsChecked = step.PowerOutputIsOpen;

            TxtPsVoltage.Text = step.PsVoltage.ToString("F2", CultureInfo.InvariantCulture);
            TxtPsCurrent.Text = step.PsCurrentLimit.ToString("F2", CultureInfo.InvariantCulture);
            ChkPsOutputOpen.IsChecked = step.PsOutputIsOpen;

            RadSmuVolt.IsChecked = step.SmuIsVoltageSource;
            RadSmuCurr.IsChecked = !step.SmuIsVoltageSource;
            TxtSmuValue.Text = step.SmuSourceValue.ToString("F3", CultureInfo.InvariantCulture);
            TxtSmuCompliance.Text = step.SmuCompliance.ToString("F3", CultureInfo.InvariantCulture);
            ChkSmuOutputOpen.IsChecked = step.SmuOutputIsOpen;

            // Bitiş aksiyonu (Koru veya Kapat)
            RadEndKeepState.IsChecked = step.EndAction == RecipeStepEndAction.KeepState;
            RadEndTurnOff.IsChecked = step.EndAction == RecipeStepEndAction.TurnOff;
        }

        private void ResetFormToNewMode(int? insertAt = null)
        {
            _editingStep = null;
            _insertAtIndex = insertAt;

            // Bitiş aksiyonu varsayılanı: Mevcut durumu koru
            RadEndKeepState.IsChecked = true;
            RadEndTurnOff.IsChecked = false;

            var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
            int nextNumber = insertAt.HasValue ? insertAt.Value + 1 : (recipe?.Steps.Count ?? 0) + 1;

            if (insertAt.HasValue)
            {
                TxtFormModeTitle.Text = $"⤵️ ARAYA YENİ ADIM EKLE (#{nextNumber})";
                TxtStepNumberBadge.Text = $"Araya (#{nextNumber})";
                BtnSaveStep.Content = $"✅ Adımı #{nextNumber} Araya Ekle";
            }
            else
            {
                TxtFormModeTitle.Text = "➕ YENİ ADIM YAPILANDIRMASI";
                TxtStepNumberBadge.Text = $"Yeni (#{nextNumber})";
                BtnSaveStep.Content = "✅ Bu Adımı Onayla ve Ekle";
            }

            if (CmbProductList.SelectedItem is ProductItem selected)
            {
                TxtStepName.Text = $"{selected.DisplayName} Ayarı";
            }
        }

        // ======================================
        // BUTON VE ADIM İŞLEMLERİ
        // ======================================

        private void BtnSaveStep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
                if (recipe == null) return;

                if (CmbProductList.SelectedItem is not ProductItem selectedProduct)
                {
                    MessageBox.Show("Lütfen bir ürün / cihaz seçiniz.", "Eksik Bilgi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(TxtDurationSeconds.Text.Trim(), out int duration) || duration <= 0)
                {
                    MessageBox.Show("Lütfen geçerli bir süre (pozitif saniye) giriniz.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtDurationSeconds.Focus();
                    return;
                }

                // Esnek ve hatasız sayısal ayrıştırma (hem virgül hem nokta destekler)
                float mfcFlow = ParseFloatFlexible(TxtMfcFlow.Text, 100f);
                float vovgTemp = ParseFloatFlexible(TxtVovgTemp.Text, 25f);
                float vovgFlow = ParseFloatFlexible(TxtVovgFlow.Text, 50f);
                float psVolt = ParseFloatFlexible(TxtPsVoltage.Text, 5f);
                float psCurr = ParseFloatFlexible(TxtPsCurrent.Text, 1f);
                float smuVal = ParseFloatFlexible(TxtSmuValue.Text, 1f);
                float smuComp = ParseFloatFlexible(TxtSmuCompliance.Text, 0.05f);

                string stepName = string.IsNullOrWhiteSpace(TxtStepName.Text) ? selectedProduct.DisplayName : TxtStepName.Text.Trim();
                var endAction = RadEndTurnOff.IsChecked == true ? RecipeStepEndAction.TurnOff : RecipeStepEndAction.KeepState;

                if (_editingStep != null)
                {
                    // MEVCUT ADIMI GÜNCELLE
                    _editingStep.Name = stepName;
                    _editingStep.DeviceType = selectedProduct.DeviceType;
                    _editingStep.DeviceIndex = selectedProduct.DeviceIndex;
                    _editingStep.DurationSeconds = duration;
                    _editingStep.EndAction = endAction;

                    _editingStep.MfcFlowSccm = mfcFlow;
                    _editingStep.MfcFlowEnabled = ChkMfcFlowEnabled.IsChecked == true;

                    _editingStep.VovgTempC = vovgTemp;
                    _editingStep.VovgFlowSccm = vovgFlow;

                    _editingStep.ValveIsOpen = ChkValveOpen.IsChecked == true;
                    _editingStep.PowerOutputIsOpen = ChkPowerOutputOpen.IsChecked == true;

                    _editingStep.PsVoltage = psVolt;
                    _editingStep.PsCurrentLimit = psCurr;
                    _editingStep.PsOutputIsOpen = ChkPsOutputOpen.IsChecked == true;

                    _editingStep.SmuIsVoltageSource = RadSmuVolt.IsChecked == true;
                    _editingStep.SmuSourceValue = smuVal;
                    _editingStep.SmuCompliance = smuComp;
                    _editingStep.SmuOutputIsOpen = ChkSmuOutputOpen.IsChecked == true;

                    // UI'ın anında tazelenmesi için
                    _editingStep.NotifyAllPropertiesChanged();
                }
                else
                {
                    // YENİ ADIM OLUŞTUR
                    var newStep = new RecipeStep
                    {
                        Name = stepName,
                        DeviceType = selectedProduct.DeviceType,
                        DeviceIndex = selectedProduct.DeviceIndex,
                        DurationSeconds = duration,
                        EndAction = endAction,
                        MfcFlowSccm = mfcFlow,
                        MfcFlowEnabled = ChkMfcFlowEnabled.IsChecked == true,
                        VovgTempC = vovgTemp,
                        VovgFlowSccm = vovgFlow,
                        ValveIsOpen = ChkValveOpen.IsChecked == true,
                        PowerOutputIsOpen = ChkPowerOutputOpen.IsChecked == true,
                        PsVoltage = psVolt,
                        PsCurrentLimit = psCurr,
                        PsOutputIsOpen = ChkPsOutputOpen.IsChecked == true,
                        SmuIsVoltageSource = RadSmuVolt.IsChecked == true,
                        SmuSourceValue = smuVal,
                        SmuCompliance = smuComp,
                        SmuOutputIsOpen = ChkSmuOutputOpen.IsChecked == true
                    };

                    if (_insertAtIndex.HasValue && _insertAtIndex.Value >= 0 && _insertAtIndex.Value <= recipe.Steps.Count)
                    {
                        recipe.Steps.Insert(_insertAtIndex.Value, newStep);
                    }
                    else
                    {
                        recipe.Steps.Add(newStep);
                    }

                    ReorderStepNumbers(recipe);
                    newStep.NotifyAllPropertiesChanged();

                    _isInternalSelection = true;
                    StepsDataGrid.SelectedItem = newStep;
                    StepsDataGrid.ScrollIntoView(newStep);
                    _isInternalSelection = false;
                }

                ResetFormToNewMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Adım kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddStepEnd_Click(object sender, RoutedEventArgs e)
        {
            StepsDataGrid.SelectedItem = null;
            ResetFormToNewMode(null);
            TxtStepName.Focus();
        }

        private void BtnInsertStep_Click(object sender, RoutedEventArgs e)
        {
            int selectedIdx = StepsDataGrid.SelectedIndex;
            if (selectedIdx < 0)
            {
                selectedIdx = ViewModel?.RecipeEngine.CurrentRecipe?.Steps.Count ?? 0;
            }
            ResetFormToNewMode(selectedIdx);
            TxtStepName.Focus();
        }

        private void BtnEditSelectedStep_Click(object sender, RoutedEventArgs e)
        {
            if (StepsDataGrid.SelectedItem is RecipeStep selected)
            {
                LoadStepIntoForm(selected);
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz adımı listeden seçiniz.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDeleteStep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
                if (recipe == null) return;

                if (StepsDataGrid.SelectedItem is RecipeStep selected)
                {
                    int oldIdx = recipe.Steps.IndexOf(selected);
                    recipe.Steps.Remove(selected);
                    ReorderStepNumbers(recipe);

                    if (recipe.Steps.Count > 0)
                    {
                        int newIdx = Math.Clamp(oldIdx, 0, recipe.Steps.Count - 1);
                        _isInternalSelection = true;
                        StepsDataGrid.SelectedIndex = newIdx;
                        _isInternalSelection = false;
                        LoadStepIntoForm(recipe.Steps[newIdx]);
                    }
                    else
                    {
                        ResetFormToNewMode(null);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silmek istediğiniz adımı listeden seçiniz.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Adım silinirken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
                if (recipe == null) return;

                int idx = StepsDataGrid.SelectedIndex;
                if (idx > 0)
                {
                    var step = recipe.Steps[idx];
                    recipe.Steps.RemoveAt(idx);
                    recipe.Steps.Insert(idx - 1, step);
                    ReorderStepNumbers(recipe);

                    _isInternalSelection = true;
                    StepsDataGrid.SelectedIndex = idx - 1;
                    StepsDataGrid.ScrollIntoView(step);
                    _isInternalSelection = false;

                    step.NotifyAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Adım taşınırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
                if (recipe == null) return;

                int idx = StepsDataGrid.SelectedIndex;
                if (idx >= 0 && idx < recipe.Steps.Count - 1)
                {
                    var step = recipe.Steps[idx];
                    recipe.Steps.RemoveAt(idx);
                    recipe.Steps.Insert(idx + 1, step);
                    ReorderStepNumbers(recipe);

                    _isInternalSelection = true;
                    StepsDataGrid.SelectedIndex = idx + 1;
                    StepsDataGrid.ScrollIntoView(step);
                    _isInternalSelection = false;

                    step.NotifyAllPropertiesChanged();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Adım taşınırken hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            StepsDataGrid.SelectedItem = null;
            ResetFormToNewMode(null);
        }

        private void StepsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isInternalSelection) return;

            // Kullanıcı listeden bir satıra tıkladığında anında o adımın özelliklerini forma doldur
            if (StepsDataGrid.SelectedItem is RecipeStep selected)
            {
                LoadStepIntoForm(selected);
            }
        }

        private static void ReorderStepNumbers(Recipe recipe)
        {
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                recipe.Steps[i].StepNumber = i + 1;
                recipe.Steps[i].NotifyAllPropertiesChanged();
            }
        }

        // ======================================
        // SÜRE VE SAYISAL AYRIŞTIRMA YARDIMCILARI
        // ======================================

        private static float ParseFloatFlexible(string? input, float defaultValue = 0f)
        {
            if (string.IsNullOrWhiteSpace(input)) return defaultValue;
            string cleaned = input.Trim().Replace(',', '.');
            return float.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out float val) ? val : defaultValue;
        }

        private void TxtDurationSeconds_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtDurationPreview == null) return;

            if (int.TryParse(TxtDurationSeconds.Text.Trim(), out int secs) && secs >= 0)
            {
                TxtDurationPreview.Text = $"({secs / 60:D2}:{secs % 60:D2} dk)";
            }
            else
            {
                TxtDurationPreview.Text = "(Geçersiz)";
            }
        }

        private void BtnQuickDuration_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tagStr)
            {
                TxtDurationSeconds.Text = tagStr;
            }
        }

        // ======================================
        // DOSYA İŞLEMLERİ (YENİ / KAYDET / YÜKLE)
        // ======================================

        private void BtnNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            var result = MessageBox.Show(
                "Yeni bir boş reçete oluşturmak istiyor musunuz? Kaydedilmemiş değişiklikler sıfırlanacaktır.",
                "Yeni Reçete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var newRecipe = new Recipe
                {
                    Name = "Yeni Reçete",
                    Description = "Özel kullanıcı otomasyon sekansı",
                    ExecutionMode = RecipeExecutionMode.PerDeviceQueue,
                    Steps = new System.Collections.ObjectModel.ObservableCollection<RecipeStep>()
                };

                ViewModel.RecipeEngine.LoadRecipe(newRecipe);
                StepsDataGrid.ItemsSource = newRecipe.Steps;
                ResetFormToNewMode(null);
            }
        }

        private void BtnSaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            var recipe = ViewModel?.RecipeEngine.CurrentRecipe;
            if (recipe == null) return;

            string safeName = string.Join("_", recipe.Name.Split(Path.GetInvalidFileNameChars()));
            var sfd = new SaveFileDialog
            {
                Filter = "Reçete Dosyası (*.json)|*.json",
                FileName = $"{safeName}.json"
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    string json = recipe.ToJson();
                    File.WriteAllText(sfd.FileName, json);
                    MessageBox.Show("Reçete başarıyla kaydedildi.", "Kaydedildi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Reçete kaydedilemedi: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnLoadRecipe_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Reçete Dosyası (*.json)|*.json"
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(ofd.FileName);
                    var recipe = Recipe.FromJson(json);
                    if (recipe != null && ViewModel != null)
                    {
                        ViewModel.RecipeEngine.LoadRecipe(recipe);
                        StepsDataGrid.ItemsSource = recipe.Steps;

                        if (recipe.Steps.Count > 0)
                        {
                            _isInternalSelection = true;
                            StepsDataGrid.SelectedIndex = 0;
                            _isInternalSelection = false;
                            LoadStepIntoForm(recipe.Steps[0]);
                        }
                        else
                        {
                            ResetFormToNewMode(null);
                        }

                        MessageBox.Show($"'{recipe.Name}' reçetesi başarıyla yüklendi ({recipe.Steps.Count} adım).", "Yüklendi", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Reçete dosyası geçersiz veya okunamadı!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Reçete yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

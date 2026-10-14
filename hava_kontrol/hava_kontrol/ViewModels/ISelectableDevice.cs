namespace hava_kontrol.ViewModels
{
    /// <summary>
    /// P&ID diyagramı ve yan detay panelinde seçilebilen donanım modelleri için arayüz.
    /// Seçili olan cihaz P&ID üzerinde vurgulanır ve gölgelendirilir.
    /// </summary>
    public interface ISelectableDevice
    {
        bool IsSelected { get; set; }
    }
}

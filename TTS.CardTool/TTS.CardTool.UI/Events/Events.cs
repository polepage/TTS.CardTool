using CS.Utils.Events;

namespace TTS.CardTool.UI.Events
{
    class ShowWaiter : SingleSubEvent { }
    class HideWaiter : SingleSubEvent { }
    class UpdateWaiterStatus : SingleSubEvent<WaiterStatus> { }
    class UpdateWaiterValue : SingleSubEvent<int> { }
}

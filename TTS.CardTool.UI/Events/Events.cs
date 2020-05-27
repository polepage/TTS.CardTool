using CS.Utils.Events;

namespace TTS.CardTool.UI.Events
{
    class RequestInput: SingleSubEvent { }
    class PostInput: SingleSubEvent<string> { }
    class PostOutput: SingleSubEvent<string> { }
}


using System.Collections.Generic;

public class Define
{
    public Dictionary<string, string> CtrlNames = new Dictionary<string, string>() {
        { "Prompt" , "PromptCtrl" },
        { "Message" , "MessageCtrl" }
    };

    public List<string> PanelNames = new List<string>() {
        "PromptPanel",
        "MessagePanel"
    };

    public enum ProtocalType {
        BINARY = 0,
        PB_LUA = 1,
        PBC = 2,
        SPROTO = 3
    }
}
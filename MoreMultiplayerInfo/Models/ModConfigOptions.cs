namespace MoreMultiplayerInfo
{
    public class ModConfigOptions
    {

        [OptionDisplay("Show Inventory")]
        public bool ShowInventory { get; set; }

        [OptionDisplay("Show Info in Text Box")]
        public bool ShowReadyInfoInChatBox { get; set; }

        [OptionDisplay("Hide in Single Player")]
        public bool HideInSinglePlayer { get; set; }

        [OptionDisplay("Last Player Alert")]
        public bool ShowLastPlayerReadyInfoInChatBox { get; set; }

        /*[OptionDisplay("Notify for Cutscenes")]
        public bool ShowCutsceneInfoInChatBox { get; set; }*/

        [OptionDisplay("Draw Inventory Grid")]
        public bool InventoryGrid { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;
using Beamable;
using Beamable.Player;
using TMPro;
using UnityEngine;
using Zenject;

public class ZenjectExampleBehaviour : MonoBehaviour
{
    [Header("Scene references")]
    public TextMeshProUGUI dbidLabel;
    public TextMeshProUGUI currencyLabel;
    
    [Inject]
    private BeamContext ctx;
    
    [Inject]
    private PlayerInventory inventory;
    
    async void Start()
    {
        dbidLabel.text = $"DBID: (loading)";
        
        // only need this because I don't have a "loading scene"
        await ctx.OnReady; // I wish we could enforce this timing into the Zenject cycle :/ 

        dbidLabel.text = $"DBID: {ctx.PlayerId}";
        inventory.GetCurrencies().OnDataUpdated += UpdateDisplay;
    }

    void UpdateDisplay(List<PlayerCurrency> currencies)
    {
        Debug.Log("updating display");
        var newText = string.Join("\n", currencies.Select(curr => $"{curr.CurrencyId}={curr.Amount}"));
        currencyLabel.text = newText;
    }

}

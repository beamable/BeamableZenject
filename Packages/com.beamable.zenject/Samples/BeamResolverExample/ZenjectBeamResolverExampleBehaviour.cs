using System.Collections.Generic;
using System.Linq;
using Beamable;
using Beamable.Player;
using Beamable.Zenject;
using TMPro;
using UnityEngine;
using Zenject;

public class ZenjectBeamResolverExampleBehaviour : MonoBehaviour
{
    [Header("Scene references")]
    public TextMeshProUGUI dbidLabel;
    public TextMeshProUGUI currencyLabel;
    
    [Inject]
    private BeamContext ctx;
    
    [Inject]
    private BeamResolver<PlayerInventory> inventoryResolver;
    
    async void Start()
    {
        await ctx.OnReady;
        dbidLabel.text = $"DBID: (loading)";
        dbidLabel.text = $"DBID: {ctx.PlayerId}";
        inventoryResolver().GetCurrencies().OnDataUpdated += UpdateDisplay;
    }

    void UpdateDisplay(List<PlayerCurrency> currencies)
    {
        Debug.Log("updating display");
        var newText = string.Join("\n", currencies.Select(curr => $"{curr.CurrencyId}={curr.Amount}"));
        currencyLabel.text = newText;
    }
}
